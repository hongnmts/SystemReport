using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
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
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class CongVanService : BaseService, ICongVanService
    {
        private readonly DataContext _context;
        private readonly BaseMongoDb<CongVan, string> BaseMongoDb;
        private readonly IMongoCollection<CongVan> _collection;
        private readonly IDbSettings _settings;
        private ILoggingService _logger;
        private List<String> filePicture = new List<string>() { ".jpeg", ".jpg", ".gif", ".png" };
        private List<String> filSystemReport = new List<string>() { ".docx", ".doc", ".csv", ".xlsx", ".pptx", ".pdf" };
        private ICongVanService _congVanServiceImplementation;

        public CongVanService(ILoggingService logger, IDbSettings settings,
            DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<CongVan, string>(_context.CongVan);
            _collection = context.CongVan;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.CongVanCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<CongVan> Create(CongVan model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = new CongVan()
            {
                Id = BsonObjectId.GenerateNewId().ToString(),
                Version = 1,
                Number = 0,
                TrangThai = model.TrangThai,
                SoLuuCV = model.SoLuuCV,
                SoVBDen = model.SoVBDen,
                NgayNhap = model.NgayNhap,
                NgayNhan = model.NgayNhan,
                NgayBanHanh = model.NgayBanHanh,
                TrichYeu = model.TrichYeu,
                LinhVuc = model.LinhVuc,
                MucDoBaoMat = model.MucDoBaoMat,
                MucDoTinhChat = model.MucDoTinhChat,
                NoiLuuTru = model.NoiLuuTru,
                CoQuanGui = model.CoQuanGui,
                KhoiCoQuanGui = model.KhoiCoQuanGui,
                HanXuLy = model.HanXuLy,
                CongVanChiDoc = model.CongVanChiDoc,
                BanChinh = model.BanChinh,
                HienThiThongBao = model.HienThiThongBao,
                NguoiKy = model.NguoiKy,
                NgayKy = model.NgayKy,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };
            if (model.HoSoDonVi != default)
            {
                entity.HoSoDonVi = model.HoSoDonVi;
            }

            if (model.HinhThucNhan != default)
            {
                entity.HinhThucNhan = model.HinhThucNhan;
            }

            if (model.LoaiVanBan != default)
            {
                entity.LoaiVanBan = model.LoaiVanBan;
            }

            if (model.TrangThai != default)
            {
                entity.TrangThai = model.TrangThai;
            }

            if (model.UploadFiles != default)
            {
                var exps = model.UploadFiles.Select(x => x.Ext).ToList();
                var tempExt = filSystemReport.Where(x => exps.Contains(x)).FirstOrDefault();
                if (tempExt == default)
                {
                    throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Định dạng tệp tin không đúng.");
                }

                foreach (var item in model.UploadFiles)
                {
                    var newFile = new FileShort();
                    newFile.FileId = item.FileId;
                    newFile.FileName = item.FileName;
                    newFile.Ext = item.Ext;
                    if (entity.File == default)
                    {
                        entity.File = new List<FileShort>();
                    }

                    entity.File.Add(newFile);
                }
            }


            var result = await BaseMongoDb.CreateAsync(entity);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);
            }

            // await _history.WithQuestionId(entity.Id)
            //     .WithAction(EAction.CREATE)
            //     .WithStatus(entity.TrangThai.Id, entity.TrangThaiTen)
            //     .WithType(ETypeHistory.Question, null)
            //     .WithTitle("Thêm thành công")
            //     .SaveChangeHistoryQuestion();
            return entity;
        }

        public async Task<CongVan> Update(CongVan model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.CongVan.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var oldValue = entity;
            entity.LoaiVanBan = model.LoaiVanBan;
            entity.TrangThai = model.TrangThai;
            entity.SoLuuCV = model.SoLuuCV;
            entity.SoVBDen = model.SoVBDen;
            entity.NgayNhap = DateTime.Now;
            entity.NgayNhan = model.NgayNhan;
            entity.NgayBanHanh = model.NgayBanHanh;
            entity.TrichYeu = model.TrichYeu;
            entity.HinhThucNhan = model.HinhThucNhan;
            entity.LinhVuc = model.LinhVuc;
            entity.MucDoBaoMat = model.MucDoBaoMat;
            entity.MucDoTinhChat = model.MucDoTinhChat;
            entity.HoSoDonVi = model.HoSoDonVi;
            entity.NoiLuuTru = model.NoiLuuTru;
            entity.CoQuanGui = model.CoQuanGui;
            entity.KhoiCoQuanGui = model.KhoiCoQuanGui;
            entity.HanXuLy = model.HanXuLy;
            entity.CongVanChiDoc = model.CongVanChiDoc;
            entity.BanChinh = model.BanChinh;
            entity.HienThiThongBao = model.HienThiThongBao;
            entity.NguoiKy = model.NguoiKy;
            entity.NgayKy = model.NgayKy;
            entity.CreatedBy = CurrentUserName;
            entity.ModifiedBy = CurrentUserName;
            entity.CreatedAt = DateTime.Now;
            entity.ModifiedAt = DateTime.Now;
            if (model.UploadFiles != default)
            {
                var exps = model.UploadFiles.Select(x => x.Ext).ToList();
                var tempExt = filSystemReport.Where(x => exps.Contains(x)).FirstOrDefault();
                if (tempExt == default)
                {
                    throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Định dạng tệp tin không đúng.");
                }

                foreach (var item in model.UploadFiles)
                {
                    var newFile = new FileShort();
                    newFile.FileId = item.FileId;
                    newFile.FileName = item.FileName;
                    newFile.Ext = item.Ext;
                    if (entity.File == default)
                    {
                        entity.File = new List<FileShort>();
                    }

                    entity.File.Add(newFile);
                }
            }


            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            // await _history.WithQuestionId(entity.Id)
            //     .WithAction(EAction.UPDATE)
            //     .WithStatus(entity.TrangThai.Id, entity.TrangThaiTen)
            //     .WithType(ETypeHistory.Question, oldValue)
            //     .WithTitle("Cập nhập thành công.")
            //     .SaveChangeHistoryQuestion();
            return entity;
        }

        public async Task Delete(string id)
        {
            if (id == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }


            var entity = _context.CongVan.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var result = await BaseMongoDb.DeleteByIdsync(id);

            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
            }
            //
            // await _history.WithQuestionId(entity.Id)
            //     .WithType(ETypeHistory.Question, entity)
            //     .WithStatus(entity.TrangThai.Id, entity.TrangThaiTen)
            //     .WithAction(EAction.DELETE)
            //     .WithTitle("Xóa văn bản đi.")
            //     .SaveChangeHistoryQuestion();
        }

        public async Task<List<CongVan>> Get()
        {
            return await _context.CongVan.Find(x => x.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<CongVan> GetById(string id)
        {
            var model = await _context.CongVan.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
            return model;
        }

        public Task<PagingModel<CongVan>> GetPaging(CongVan param)
        {
            throw new NotImplementedException();
        }

        public async Task<PagingModel<CongVan>> GetPaging(CongVanParam param)
        {
            var result = new PagingModel<CongVan>();
            var builder = Builders<CongVan>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            // filter = filter & builder.In(x => x.IdOwner, CurrentUser.DonViIds);

            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.SoLuuCV.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (!String.IsNullOrEmpty(param.TrangThai))
            {
                filter = builder.And(filter, builder.Eq(x => x.TrangThai.Id, param.TrangThai));
            }

            if (!String.IsNullOrEmpty(param.LinhVuc))
            {
                filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVuc));
            }

            string sortBy = nameof(CongVan.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<PagingModel<LuuCVDen>> GetPagingLuuCVDen(CongVanParam param)
        {
            var result = new PagingModel<LuuCVDen>();
            var builder = Builders<LuuCVDen>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            // filter = filter & builder.In(x => x.IdOwner, CurrentUser.DonViIds);

            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.SoLuuCV.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (!String.IsNullOrEmpty(param.TrangThai))
            {
                filter = builder.And(filter, builder.Eq(x => x.TrangThai.Id, param.TrangThai));
            }

            if (!String.IsNullOrEmpty(param.LinhVuc))
            {
                filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVuc));
            }

            string sortBy = nameof(CongVan.ModifiedAt);
            result.TotalRows = await _context.LuuCVDen.CountDocumentsAsync(filter);
            result.Data = await _context.LuuCVDen.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<PagingModel<LuuCVDi>> GetPagingLuuCVDi(CongVanParam param)
        {
            var result = new PagingModel<LuuCVDi>();
            var builder = Builders<LuuCVDi>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            // filter = filter & builder.In(x => x.IdOwner, CurrentUser.DonViIds);

            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.SoLuuCV.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (!String.IsNullOrEmpty(param.TrangThai))
            {
                filter = builder.And(filter, builder.Eq(x => x.TrangThai.Id, param.TrangThai));
            }

            if (!String.IsNullOrEmpty(param.LinhVuc))
            {
                filter = builder.And(filter, builder.Eq(x => x.LinhVuc.Id, param.LinhVuc));
            }

            string sortBy = nameof(CongVan.ModifiedAt);
            result.TotalRows = await _context.LuuCVDi.CountDocumentsAsync(filter);
            result.Data = await _context.LuuCVDi.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<LuuCVDen> GetByIdLuuCVDen(string id)
        {
            var model = await _context.LuuCVDen.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
            return model;
        }

        public async Task<LuuCVDi> GetByIdLuuCVDi(string id)
        {
            var model = await _context.LuuCVDi.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
            return model;
        }
    }
}