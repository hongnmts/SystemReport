using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class HopThuService: BaseService, IHopThuService
    {
        private readonly DataContext _context;
        private readonly BaseMongoDb<HopThu, string> BaseMongoDb;
        private readonly IMongoCollection<HopThu> _collection;
        private readonly IDbSettings _settings;
        private readonly IFileService _fileService;
        private ILoggingService _logger;
        private INotifyService _notifyService;
        private readonly HistoryVanBanDiService _history;
        private List<String> filePicture = new List<string>() { ".jpeg", ".jpg", ".gif", ".png" };
        private List<String> filSystemReport = new List<string>() { ".docx", ".doc", ".pdf" };

        public HopThuService(HistoryVanBanDiService history, ILoggingService logger, IDbSettings settings,
            INotifyService notifyService,
            DataContext context,
            IFileService fileService,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<HopThu, string>(_context.HopThu);
            _collection = context.HopThu;
            _settings = settings;
            _fileService = fileService;
            _notifyService = notifyService;
            _logger = logger.WithCollectionName(_settings.VanBanDiCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
            _history = history.WithUserName(CurrentUserName);
        }
        
        public async Task<HopThu> CreateSend(HopThu model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = new HopThu()
            {
                Id = BsonObjectId.GenerateNewId().ToString(),
                TieuDe = model.TieuDe,
                NoiDung = model.NoiDung,
                NguoiNhans = model.NguoiNhans,
                Cc = model.Cc,
            };
            
             if (model.UploadFiles != default && model.UploadFiles.Count > 0)
             {
                 foreach (var file in model.UploadFiles)
                 {
                     if (entity.Files == default)
                         entity.Files = new List<FileShort>();
                     var newFile = new FileShort();
                     newFile.FileId = file.FileId;
                     newFile.FileName = file.FileName;
                     newFile.Ext = file.Ext;
                     entity.Files.Add(newFile);
                 }
             }

             var userRep = new List<UserTreeChilVM>();

             if (model.NguoiNhans != default)
             {
                 userRep.AddRange(model.NguoiNhans);
             }
             if (model.Cc != default)
             {
                 userRep.AddRange(model.Cc);
             }

             entity.NguoiGui = CurrentUserShort;
             entity.NgayGui = DateTime.Now;
             
             foreach (var item in userRep)
             {
                 var tempEmail = entity;
                 tempEmail.Id = BsonObjectId.GenerateNewId().ToString();
                 tempEmail.DaXem = false;
                 tempEmail.NgayNhan = DateTime.Now;
                 tempEmail.NguoiNhan = new UserShort()
                 {
                     Id = item.Id,
                     UserName = item.UserName,
                     FullName = item.FullName,
                     DonVi = item.DonVi,
                     ChucVu = item.ChucVu
                 };
                 
                 await BaseMongoDb.CreateAsync(tempEmail);
             }

             return entity;
        }
        
        public Task<HopThu> Create(HopThu model)
        {
            throw new NotImplementedException();
        }

        public Task<HopThu> Update(HopThu model)
        {
            throw new NotImplementedException();
        }

        public async Task<HopThu> GetById(string id)
        {
            return await _context.HopThu.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task Delete(string id)
        {
            var entity = _context.HopThu.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy thư!");
            }

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserName;
            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Xóa thư không thành công!");
            }
        }
        public async Task DeleteR(string id)
        {
            var entity = _context.HopThu.Find(x => x.Id == id && x.IsDeleted == true).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy thư!");
            }
            
            var result = await BaseMongoDb.DeletedAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Xóa thư không thành công!");
            }
        }
        public async Task<PagingModel<HopThu>> GetPaging(HopThuParam param)
        {
            var result = new PagingModel<HopThu>();
            var builder = Builders<HopThu>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.NguoiNhan.UserName == CurrentUserName && x.IsDeleted == false));
            
            string sortBy = nameof(VanBanDi.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<PagingModel<HopThu>> GetPagingDaGui(HopThuParam param)
        {
            var result = new PagingModel<HopThu>();
            var builder = Builders<HopThu>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.NguoiGui.UserName == CurrentUserName && x.IsDeleted == false));
            
            string sortBy = nameof(VanBanDi.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }
        
        public async Task<PagingModel<HopThu>> GetPagingRac(HopThuParam param)
        {
            var result = new PagingModel<HopThu>();
            var builder = Builders<HopThu>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x =>( x.NguoiGui.UserName == CurrentUserName || x.NguoiNhan.UserName == CurrentUserName ) && x.IsDeleted == true));
            
            string sortBy = nameof(VanBanDi.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }
    }
}