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
using SystemReport.WebAPI.ViewModels;
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class NotifyService : BaseService, INotifyService
    {
        private DataContext _context;
        private IMongoCollection<Notify> _collection;
        private BaseMongoDb<Notify, string> BaseMongoDb;
        private BaseMongoDb<LuuCVDi, string> BaseMongoVanBanDi;
        private BaseMongoDb<LuuCVDen, string> BaseMongoVanBanDen;
        public NotifyService(DataContext context, IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            _collection = _context.Notify;
            BaseMongoDb = new BaseMongoDb<Notify, string>(context.Notify);
            BaseMongoVanBanDi = new BaseMongoDb<LuuCVDi, string>(context.LuuCVDi);
            BaseMongoVanBanDen = new BaseMongoDb<LuuCVDen, string>(context.LuuCVDen);
        }

        private List<string> _recipientIds { get; set; }

        private Notify _notify;

        public NotifyService WithNotify(Notify notify)
        {
            if (notify != default)
            {
                _notify = notify;
            }
            return this;
        }

        public NotifyService WithRecipients(List<string> recipientIds)
        {
            if (recipientIds != default)
            {
                _recipientIds = recipientIds;
            }

            return this;
        }

        public NotifyService WithRecipient(string recipientId)
        {
            if (!string.IsNullOrEmpty(recipientId))
            {
                _recipientIds = new List<string>()
                {
                    recipientId
                };
            }

            return this;
        }

        public async Task PushNotify()
        {
            if (_recipientIds == default)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Recipient not empty");
            }

            if (_notify == default)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Notifycation not empty");
            }

            var users = _context.Users.Find(x => _recipientIds.Contains(x.Id) && x.IsDeleted != true).ToList();
            foreach (var user in users)
            {
                _notify.Id = BsonObjectId.GenerateNewId().ToString();
                _notify.Sender = CurrentUser.FullName;
                _notify.SenderId = CurrentUser.Id;
                _notify.Recipient = user.FullName;
                _notify.RecipientId = user.Id;

                var result = await BaseMongoDb.CreateAsync(_notify);
            }

            _notify = default;
            _recipientIds = default;
        }
        public async Task<PagingModel<Notify>> GetPaging(NotifyParam param)
        {
            var result = new PagingModel<Notify>();
            var builder = Builders<Notify>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.RecipientId == CurrentUser.Id && x.IsDeleted == false));

            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<Notify> GetById(string id)
        {
            var data = await _context.Notify.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (data != default)
            {
                var files = new List<FileShort>();
                if (data.LoaiCongVan == ELoaiCongVan.CONG_VAN_DI)
                {
                    var vanBan = _context.VanBanDi.Find(x => x.Id == data.CongVanId).FirstOrDefault();
                    if (vanBan != default)
                    {
                        if (vanBan.File != default)
                        {
                            files.AddRange(vanBan.File);
                        }

                        if (vanBan.FilePDF != default)
                        {
                            files.Add(vanBan.FilePDF.LastOrDefault());
                        }
                    }
                }
                else if (data.LoaiCongVan == ELoaiCongVan.CONG_VAN_DEN)
                {
                    var vanBan = _context.VanBanDen.Find(x => x.Id == data.CongVanId).FirstOrDefault();
                    if (vanBan != default)
                    {
                        if (vanBan.File != default)
                        {
                            files.AddRange(vanBan.File);
                        }
                    }
                }

                data.Files = files;
                return data;
            }

            return null;
        }

        public async Task<ResultResponse<NotifyVM>> GetListNotify()
        {
            ResultResponse<NotifyVM> resultResponse = new ResultResponse<NotifyVM>();

            var builder = Builders<Notify>.Filter;
            var filter = builder.Empty;
            try
            {
                var notifyVm = new NotifyVM();
                filter = builder.And(filter, builder.Where(x => x.RecipientId == CurrentUser.Id && x.Read == false));
                var TotalRows = await _context.Notify.CountDocumentsAsync(filter);
                var data = await _context.Notify.Find(filter).SortByDescending(x => x.ModifiedAt)
                    .Skip(0)
                    .Limit(5)
                    .ToListAsync();
                notifyVm.Number = TotalRows;
                notifyVm.ListNotify = data;

                resultResponse.ResultCode = EResultResponse.SUCCESS.ToString();
                resultResponse.ResultString = "Lấy thông báo thành công";
                resultResponse.Data = notifyVm;

            }
            catch (Exception ex)
            {

                resultResponse.ResultCode = EResultResponse.ERROR.ToString();
                resultResponse.ResultString = "Lỗi" + ex.Message;
            }

            return resultResponse;
        }

        public async Task<ResultResponse<Notify>> ChangeStatus(string id)
        {
            ResultResponse<Notify> resultResponse = new ResultResponse<Notify>();
            if (id == null || id == "")
            {
                resultResponse.ResultCode = EResultResponse.FAIL.ToString();
                resultResponse.ResultString = "Dữ liệu không được để trống.";
                return resultResponse;
            }
            try
            {
                var entity = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (entity == default)
                {
                    resultResponse.ResultCode = EResultResponse.NOT_EXIST.ToString();
                    resultResponse.ResultString = "Không tồn tại dữ liệu.";
                    return resultResponse;
                }

                entity.Read = true;
                var result = await BaseMongoDb.UpdateAsync(entity);
                if (result.Success)
                {
                    resultResponse.ResultCode = EResultResponse.SUCCESS.ToString();
                    resultResponse.ResultString = "Cập nhật thành công.";
                    resultResponse.Data = entity;
                    return resultResponse;
                }

                resultResponse.ResultCode = EResultResponse.FAIL.ToString();
                resultResponse.ResultString = "Cập nhật không thành công.";

            }
            catch (Exception ex)
            {
                resultResponse.ResultCode = EResultResponse.SUCCESS.ToString();
                resultResponse.ResultString = "Lỗi" + ex.Message;
            }
            return resultResponse;
        }

        public async Task LuuCVNoiBo(string idNotify)
        {
            var notify = _context.Notify.Find(x => x.Id == idNotify).FirstOrDefault();
            if (notify == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy thông báo!");
            }

            if (notify.LoaiCongVan == 0 && string.IsNullOrEmpty(notify.CongVanId))
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy công văn");
            }

            if (notify.LoaiCongVan == ELoaiCongVan.CONG_VAN_DI)
            {
                var congVanDi = _context.VanBanDi.Find(x => x.Id == notify.CongVanId && x.IsDeleted != true).FirstOrDefault();
                if (congVanDi != default)
                {
                    var checkSoLuu = _context.LuuCVDi.Find(x => x.SoLuuCV == congVanDi.SoLuuCV && x.IsDeleted != true)
                        .FirstOrDefault();
                    if (checkSoLuu != default)
                    {
                        throw new ResponseMessageException()
                            .WithCode(EResultResponse.FAIL.ToString())
                            .WithMessage("Số lưu công văn đã tồn tại: " + congVanDi.SoLuuCV);
                    }
                    var entity = new LuuCVDi();
                    entity.Id = BsonObjectId.GenerateNewId().ToString();
                    entity.LoaiVanBan = congVanDi.LoaiVanBan;
                    entity.VanBanId = congVanDi.Id;
                    entity.TrangThai = congVanDi.TrangThai;
                    entity.SoLuuCV = congVanDi.SoLuuCV;
                    entity.SoVBDi = congVanDi.SoVBDi;
                    entity.NgayNhap = congVanDi.NgayNhap;
                    entity.NgayBanHanh = congVanDi.NgayBanHanh;
                    entity.NgayTraLoi = congVanDi.NgayTraLoi;
                    entity.TraLoiCVSo = congVanDi.TraLoiCVSo;
                    entity.SoBan = congVanDi.SoBan;
                    entity.TrichYeu = congVanDi.TrichYeu;
                    entity.DonViSoan = congVanDi.DonViSoan;
                    entity.HinhThucGui = congVanDi.HinhThucGui;
                    entity.CanBoSoan = congVanDi.CanBoSoan;
                    entity.LinhVuc = congVanDi.LinhVuc;
                    entity.NoiLuuTru = congVanDi.NoiLuuTru;
                    entity.KhoiCoQuanNhan = congVanDi.KhoiCoQuanNhan;
                    entity.CoQuanNhan = congVanDi.CoQuanNhan;
                    entity.NguoiKy = congVanDi.NguoiKy;
                    entity.NgayKy = congVanDi.NgayKy;
                    entity.File = congVanDi.File;
                    entity.FilePDF = congVanDi.FilePDF;
                    entity.PhanCongKySo = congVanDi.PhanCongKySo;
                    entity.NguoiPhanCong = congVanDi.NguoiPhanCong;
                    entity.MucDoBaoMat = congVanDi.MucDoBaoMat;
                    entity.MucDoTinhChat = congVanDi.MucDoTinhChat;
                    entity.HoSoDonVi = congVanDi.HoSoDonVi;

                    entity.CreatedBy = CurrentUserName;
                    entity.ModifiedBy = CurrentUserName;
                    entity.CreatedAt = DateTime.Now;
                    entity.ModifiedAt = DateTime.Now;

                    var result = await BaseMongoVanBanDi.CreateAsync(entity);
                    if (result.Entity.Id == default || !result.Success)
                    {
                        throw new ResponseMessageException()
                            .WithCode(EResultResponse.FAIL.ToString())
                            .WithMessage(DefaultMessage.CREATE_FAILURE);
                    }
                }
            }
            else if (notify.LoaiCongVan == ELoaiCongVan.CONG_VAN_DEN)
            {
                var congVanDen = _context.VanBanDen.Find(x => x.Id == notify.CongVanId && x.IsDeleted != true).FirstOrDefault();
                if (congVanDen != default)
                {
                    var checkSoLuu = _context.LuuCVDen.Find(x => x.SoLuuCV == congVanDen.SoLuuCV && x.IsDeleted != true)
                        .FirstOrDefault();
                    if (checkSoLuu != default)
                    {
                        throw new ResponseMessageException()
                            .WithCode(EResultResponse.FAIL.ToString())
                            .WithMessage("Số lưu công văn đã tồn tại: " + congVanDen.SoLuuCV);
                    }
                    var entity = new LuuCVDen();
                    entity.Id = BsonObjectId.GenerateNewId().ToString();
                    entity.VanBanId = congVanDen.Id;
                    entity.LoaiVanBan = congVanDen.LoaiVanBan;
                    entity.TrangThai = congVanDen.TrangThai;
                    entity.SoLuuCV = congVanDen.SoLuuCV;
                    entity.SoVBDen = congVanDen.SoVBDen;
                    entity.NgayNhap = congVanDen.NgayNhap;
                    entity.NgayBanHanh = congVanDen.NgayBanHanh;
                    entity.NgayTraLoi = congVanDen.NgayTraLoi;
                    entity.TraLoiCVSo = congVanDen.TraLoiCVSo;
                    entity.SoBan = congVanDen.SoBan;
                    entity.TrichYeu = congVanDen.TrichYeu;
                    entity.DonViSoan = congVanDen.DonViSoan;
                    entity.HinhThucNhan = congVanDen.HinhThucNhan;
                    entity.CanBoSoan = congVanDen.CanBoSoan;
                    entity.LinhVuc = congVanDen.LinhVuc;
                    entity.NoiLuuTru = congVanDen.NoiLuuTru;
                    entity.KhoiCoQuanNhan = congVanDen.KhoiCoQuanNhan;
                    entity.CoQuanNhan = congVanDen.CoQuanNhan;
                    entity.NguoiKy = congVanDen.NguoiKy;
                    entity.NgayKy = congVanDen.NgayKy;
                    entity.File = congVanDen.File;
                    entity.MucDoBaoMat = congVanDen.MucDoBaoMat;
                    entity.MucDoTinhChat = congVanDen.MucDoTinhChat;
                    entity.HoSoDonVi = congVanDen.HoSoDonVi;

                    entity.CreatedBy = CurrentUserName;
                    entity.ModifiedBy = CurrentUserName;
                    entity.CreatedAt = DateTime.Now;
                    entity.ModifiedAt = DateTime.Now;

                    var result = await BaseMongoVanBanDen.CreateAsync(entity);
                    if (result.Entity.Id == default || !result.Success)
                    {
                        throw new ResponseMessageException()
                            .WithCode(EResultResponse.FAIL.ToString())
                            .WithMessage(DefaultMessage.CREATE_FAILURE);
                    }
                }
            }
            else
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy công văn");
            }
        }
    }
}