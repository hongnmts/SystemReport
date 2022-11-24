using MongoDB.Driver;
using System.Security.Authentication;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.Data
{
    public class DataContext
    {
        private readonly IMongoClient _client = null;
        private readonly IMongoDatabase _context = null;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Role> _roles;
        private readonly IMongoCollection<Menu> _menu;
        private readonly IMongoCollection<Module> _module;
        private readonly IMongoCollection<File> _file;
        private readonly IMongoCollection<Logging> _logging;
        private readonly IMongoCollection<RefreshToken> _refreshToken;

        // Nghiệp vụ
        private readonly IMongoCollection<DonVi> _donVi;
        private readonly IMongoCollection<LinhVuc> _linhVuc;
        private readonly IMongoCollection<ChucVu> _chucVu;
        private readonly IMongoCollection<TrangThai> _trangThai;
        private readonly IMongoCollection<ModuleTrangThai> _moduleTrangThai;
        private readonly IMongoCollection<Notify> _notify;
        private readonly IMongoCollection<CoQuan> _coQuan;
        private readonly IMongoCollection<KhoiCoQuan> _khoiCoQuan;
        private readonly IMongoCollection<LoaiTrangThai> _loaiTrangThai;



        // System Report
        private readonly IMongoCollection<KyBaoCao> _kyBaoCao;
        private readonly IMongoCollection<DonViTinh> _donViTinh;
        private readonly IMongoCollection<MauBieu> _mauBieu;
        private readonly IMongoCollection<BangBieu> _bangBieu;
        private readonly IMongoCollection<ThuocTinh> _thuocTinh;
        private readonly IMongoCollection<RowValue> _rowValue;
        private readonly IMongoCollection<LoaiMauBieu> _loaiMauBieu;
        private readonly IMongoCollection<HistoryMauBieu> _historyMauBieu;
        private readonly IMongoCollection<CommonItem> _commonItem;
        private readonly IMongoCollection<HoTroDN> _hoTroDN;
        private readonly IMongoCollection<QuanLyKH> _quanLyKhoaHoc;

        private readonly IDbSettings _settings;
        public DataContext(IDbSettings settings)
        {
            _settings = settings;
            if (_settings.Environment != "Production")
            {
                MongoClientSettings settings1 = new MongoClientSettings();
                settings1.Server = new MongoServerAddress("anhdev99.com", 27017);
                settings1.UseTls = false;
                settings1.SslSettings = new SslSettings();
                settings1.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

                MongoIdentity identity = new MongoInternalIdentity("SystemReport", "admin");
                MongoIdentityEvidence evidence = new PasswordEvidence("DongThap@123");

                settings1.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);
                _client = new MongoClient(settings1);
            }
            else
            {

                _client = new MongoClient("mongodb://localhost:27017");
            }

            if (_client != null)
            {
                _context = _client.GetDatabase(_settings.DatabaseName);
            }

            _users = _context.GetCollection<User>(_settings.UserCollectionName);
            _roles = _context.GetCollection<Role>(_settings.RoleCollectionName);
            _menu = _context.GetCollection<Menu>(_settings.MenuCollectionName);
            _module = _context.GetCollection<Module>(_settings.ModuleCollectionName);
            _file = _context.GetCollection<File>(_settings.FileCollectionName);
            _logging = _context.GetCollection<Logging>(_settings.LoggingCollectionName);
            _refreshToken = _context.GetCollection<RefreshToken>(_settings.RefreshTokenCollectionName);

            // Nghiệp vụ
            _donVi = _context.GetCollection<DonVi>(_settings.DonViCollectionName);
            _linhVuc = _context.GetCollection<LinhVuc>(_settings.LinhVucCollectionName);
            _chucVu = _context.GetCollection<ChucVu>(_settings.ChucVuCollectionName);
            _trangThai = _context.GetCollection<TrangThai>(_settings.TrangThaiCollectionName);
            _moduleTrangThai = _context.GetCollection<ModuleTrangThai>(_settings.ModuleTrangThaiCollectionName);
            _notify = _context.GetCollection<Notify>(_settings.NotifyCollectionName);
            _coQuan = _context.GetCollection<CoQuan>(_settings.CoQuanCollectionName);
            _khoiCoQuan = _context.GetCollection<KhoiCoQuan>(_settings.KhoiCoQuanCollectionName);
            _loaiTrangThai = _context.GetCollection<LoaiTrangThai>(_settings.LoaiTrangThaiCollectionName);

            _kyBaoCao = _context.GetCollection<KyBaoCao>(_settings.KyBaoCaoCollectionName);
            _donViTinh = _context.GetCollection<DonViTinh>(_settings.DonViTinhCollectionName);
            _mauBieu = _context.GetCollection<MauBieu>(_settings.MauBieuCollectionName);
            _bangBieu = _context.GetCollection<BangBieu>(_settings.BangBieuCollectionName);
            _thuocTinh = _context.GetCollection<ThuocTinh>(_settings.ThuocTinhCollectionName);
            _rowValue = _context.GetCollection<RowValue>(_settings.RowValueCollectionName);
            _loaiMauBieu = _context.GetCollection<LoaiMauBieu>(_settings.LoaiMauBieuCollectionName);
            // History
            _historyMauBieu = _context.GetCollection<HistoryMauBieu>(_settings.HistoryMauBieuCollectionName);
            _commonItem = _context.GetCollection<CommonItem>(_settings.CommonItemCollectionName);
            _hoTroDN = _context.GetCollection<HoTroDN>(_settings.HoTroDNCollectionName);
            _quanLyKhoaHoc = _context.GetCollection<QuanLyKH>(_settings.QuanLyKhoaHocCollectionName);
        }

        public IMongoDatabase Database
        {
            get { return _context; }
        }
        public IMongoClient Client
        {
            get { return _client; }
        }
        public IMongoCollection<User> Users { get => _users; }
        public IMongoCollection<Role> Roles { get => _roles; }
        public IMongoCollection<Menu> Menu { get => _menu; }
        public IMongoCollection<File> Files { get => _file; }
        public IMongoCollection<Module> Modules { get => _module; }
        public IMongoCollection<Logging> Loggings { get => _logging; }
        public IMongoCollection<RefreshToken> RefreshToken { get => _refreshToken; }

        // Nghiệp vụ
        public IMongoCollection<DonVi> DonVis { get => _donVi; }
        public IMongoCollection<LinhVuc> LinhVuc { get => _linhVuc; }
        public IMongoCollection<CommonItem> CommonItem { get => _commonItem; }
        public IMongoCollection<ChucVu> ChucVu { get => _chucVu; }
        public IMongoCollection<HistoryMauBieu> HistoryMauBieu { get => _historyMauBieu; }
        public IMongoCollection<TrangThai> TrangThai { get => _trangThai; }
        public IMongoCollection<ModuleTrangThai> ModuleTrangThai { get => _moduleTrangThai; }
        public IMongoCollection<Notify> Notify { get => _notify; }
        public IMongoCollection<CoQuan> CoQuan { get => _coQuan; }
        public IMongoCollection<KhoiCoQuan> KhoiCoQuan { get => _khoiCoQuan; }
        public IMongoCollection<LoaiTrangThai> LoaiTrangThai { get => _loaiTrangThai; }
        public IMongoCollection<HoTroDN> HoTroDN { get => _hoTroDN; }
        public IMongoCollection<QuanLyKH> QuanLyKH { get => _quanLyKhoaHoc; }

        //
        public IMongoCollection<KyBaoCao> KyBaoCao
        {
            get => _kyBaoCao;
        }

        public IMongoCollection<DonViTinh> DonViTinh
        {
            get => _donViTinh;
        }

        public IMongoCollection<MauBieu> MauBieu
        {
            get => _mauBieu;
        }

        public IMongoCollection<BangBieu> BangBieu
        {
            get => _bangBieu;
        }

        public IMongoCollection<ThuocTinh> ThuocTinh
        {
            get => _thuocTinh;
        }

        public IMongoCollection<RowValue> RowValue
        {
            get => _rowValue;
        }

        public IMongoCollection<LoaiMauBieu> LoaiMauBieu
        {
            get => _loaiMauBieu;
        }
    }
}