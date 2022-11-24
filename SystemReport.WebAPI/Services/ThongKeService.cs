using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Http;
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
using SystemReport.WebAPI.ViewModels;
using EResultResponse = SystemReport.WebAPI.Helpers.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class ThongKeService : BaseService, IThongKeService
    {
        private DataContext _context;
        private BaseMongoDb<ChucVu, string> BaseMongoDb;
        private IMongoCollection<ChucVu> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;
        private IThuocTinhService _thuocTinhService;
        private IBangBieuService _bangBieuService;
        private IRowValueService _rowValueService;

        public ThongKeService(ILoggingService logger, IDbSettings settings, DataContext context,
            IThuocTinhService thuocTinhService,
            IBangBieuService bangBieuService,
            IRowValueService rowValueService,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            _settings = settings;
            _thuocTinhService = thuocTinhService;
            _bangBieuService = bangBieuService;
            _rowValueService = rowValueService;
        }


        public TieuChiThongKeVM GetTieuChiTongKe(string bangBieuId)
        {
            var bangBieu = _context.BangBieu.Find(x => x.Id == bangBieuId).FirstOrDefault();
            var notChiTieus = new List<string>();
            if(bangBieu == default)
            {
                throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var thuocTinhs = _thuocTinhService.GetNodeLeftChiTieu(bangBieu.Id);
            var rowValues = _context.RowValue.Find(x => x.BangBieuId == bangBieu.Id).ToList();

            var result = new List<TieuChiVM>();
            var tempResult = new List<TieuChiVM>();
            foreach (var item in thuocTinhs)
            {
                var subResult = new TieuChiVM(item);
                tempResult.Add(subResult);
                var subValue = rowValues.Where(x => x.Value != null && x.ThuocTinhId == item.Id && !string.IsNullOrEmpty(x.Value as string)).ToList();

                foreach (RowValue sv in subValue)
                {
                    sv.Value = (sv.Value as string).Trim().ToLower().Replace("\n", "").Replace("\"","");
                }
                var tempValue = new List<RowValue>();
              
                foreach (RowValue rv in subValue)
                {
                    var find = tempValue.Where(x => x != null && (x.Value as string).Trim().Replace("\n", "").ToLower().Contains((rv.Value as string).Trim().Replace("\n", "").ToLower())).FirstOrDefault();
                    if(find == default)
                    {
                        tempValue.Add(rv);
                    }
                }

                foreach (RowValue t in tempValue)
                {
                    subResult.Children.Add(new TieuChiVM(t));
                    tempResult.Add(new TieuChiVM(t));
                }
                result.Add(subResult);
            }

            var endData = new TieuChiThongKeVM();
            endData.TieuChiView = result;
            endData.TieuChis = tempResult;
            return endData;
        }


        public async Task<DemoMauBieuVM> FilterBangBieu(List<TieuChiVM> models, string bangBieuId)
        {
            var listKeyRowRemove = new List<string>();
            if(models == default)
            {
                throw new ResponseMessageException()
                       .WithCode(EResultResponse.FAIL.ToString())
                       .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var idBangBieu = models.FirstOrDefault().BangBieuId;
            var bangBieu = _context.BangBieu.Find(x => x.Id == idBangBieu).FirstOrDefault();
            var ttList = _context.ThuocTinh.Find(x => x.BangBieuId == idBangBieu).ToList();

            var rowValues = models.Where(x => x.Type == false).ToList();
            List<string> rowValuesIds = rowValues.Select(x => x.Id).ToList();

            var rowValueImportant = _context.RowValue.Find(x => x.BangBieuId == idBangBieu).ToList();
            foreach (RowValue sv in rowValueImportant)
            {
                if(sv.Value != null)
                {
                    sv.ValueTemp = Convert.ToString(sv.Value).Trim().ToLower().Replace("\n", "").Replace("\"", "");
                }
                else
                {
                    sv.ValueTemp = "";
                }
                
            }

            var rowData = new List<RowValue>();

            // Lấy ra row value mà k ai chọn
            foreach (var rv in rowValues.Where(x => x.Selected == false).ToList())
            {
                var find = rowValueImportant.Where(x => x.ValueTemp.ToLower() == rv.Text).ToList();
                if (find != default)
                {
                    rowData.AddRange(find);
                }
            }
            var tempVRlist = new List<RowValue>();
            foreach (var item in rowData)
            {
                var _temp = ttList.Where(x => x.Id == item.ThuocTinhId).FirstOrDefault();

                //var rv = rowValueImportant.Where(x => x.ThuocTinhId == item.ThuocTinhId && (x.Value == null || (x.Value != null && string.IsNullOrEmpty(x.Value)))).ToList();
                //if(rv.Count > 0)
                //{
                //    tempVRlist.AddRange(rv);
                //}
            }
             //tempVRlist = rowValueImportant.Where(x => !rowValuesIds.Contains(x.Id)).ToList();
            if (tempVRlist.Count > 0)
            {
                rowData.AddRange(tempVRlist);
            }
            listKeyRowRemove = rowData.Select(x => x.KeyRow).ToList();

            listKeyRowRemove.Distinct();

            var rowValuesLast = rowValueImportant.Where(x => !listKeyRowRemove.Contains(x.KeyRow)).ToList();

            var thuocTinhInRowValue = rowValuesLast.Select(x => x.ThuocTinhId).ToList();
            /// Xử lý thuộc tính
            var thuocTinhs = models.Where(x => x.Type = true).ToList();

            var thuocTinhIds = rowValues.Where(x => x.Selected == true).Select(x => x.ThuocTinhId).ToList();

            foreach (var tt in thuocTinhs)
            {
                if (!thuocTinhIds.Contains(tt.Id))
                {
                    thuocTinhIds.Add(tt.Id);
                }
            }

            // Xu ly lan cuoi khi render
            var thuocTinhTonTai = new List<string>();
            var thuocTinhsSelectFalse = models.Where(x => x.Type = true && x.Selected == false).ToList();
            if (thuocTinhsSelectFalse.Count > 0)
            {
                foreach (var tt in thuocTinhsSelectFalse)
                {
                    var checkTonTai = thuocTinhInRowValue.Contains(tt.Id);
                    if (!checkTonTai)
                    {
                        thuocTinhTonTai.Add(tt.Id);
                    }
                }
         
            }

            var notExInListRowValueModel = models.Where(x => x.Type == false && x.Selected == true).ToList();
            var rwRemove = new List<RowValue>();

             foreach (var rv in notExInListRowValueModel)
            {
                var find = rowValueImportant.Where(x => x.ValueTemp.ToLower() == rv.Text).ToList();
                if (find != default)
                {
                    rwRemove.AddRange(find);
                }
            }

            rowValuesLast = rowValuesLast.Where(x => rwRemove.Any(p => p.Id != x.Id)).ToList();


            var thuocTinhLast = _context.ThuocTinh.Find(x => x.BangBieuId == bangBieu.Id && thuocTinhIds.Contains(x.Id)).ToList();
            
            var lastTT = thuocTinhLast.Where(x => !thuocTinhTonTai.Contains(x.Id)).ToList();
            var lastRV = rowValuesLast.Where(x => !thuocTinhTonTai.Contains(x.ThuocTinhId)).ToList();

            var headers = await _bangBieuService.RenderHeader(lastTT);
            var body = await _rowValueService.RenderBodyMainByBangBieuId(lastRV);

            var table = new DemoMauBieuVM();
            table.TenBangBieu = bangBieu.Ten;
            table.IsHienThiTen = bangBieu.HienThiTen;
            table.ThuTu = bangBieu.ThuTu;
            table.Headers = headers;
            table.Body = body;

            return table;
        }
    }
}
