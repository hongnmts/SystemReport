const toJson = (item) => {
    return {
        id: item.id,
        version: item.version,
        number: item.number,
        loaiVanBan: item.loaiVanBan,
        loaiVanBanTen: item.loaiVanBanTen,
        trangThai: item.trangThai,
        trangThaiTen: item.trangThaiTen,
        soLuuCV: item.soLuuCV,
        soVBDi: item.soVBDi,
        ngayNhap: item.ngayNhap,
        ngayBanHanh: item.ngayBanHanh,
        ngayTraLoi: item.ngayTraLoi,
        traLoiCVSo: item.traLoiCVSo,
        soBan: item.soBan,
        trichYeu: item.trichYeu,
        donViSoan: item.donViSoan,
        donViSoanTen: item.donViSoanTen,
        canBoSoan: item.canBoSoan,
        canBoSoanTen: item.canBoSoanTen,
        hinhThucGui: item.hinhThucGui,
        hinhThucGuiTen: item.hinhThucGuiTen,
        hanXuLy: item.hanXuLy,
        linhVuc: item.linhVuc,
        linhVucTen: item.linhVucTen,
        mucDoBaoMat: item.mucDoBaoMat,
        mucDoBaoMatTen: item.mucDoBaoMatTen,
        mucDoTinhChat: item.mucDoTinhChat,
        mucDoTinhChatTen: item.mucDoTinhChatTen,
        hoSoDonVi: item.hoSoDonVi,
        hoSoDonViTen: item.hoSoDonViTen,
        noiLuuTru: item.noiLuuTru,
        coQuanNhan: item.coQuanNhan,
        coQuanNhanTen: item.coQuanNhanTen,
        congVanChiDoc: item.congVanChiDoc,
        banChinh: item.banChinh,
        hienThiThongBao: item.hienThiThongBao,
        ngayNhan: item.ngayNhan,
        coQuanGui: item.coQuanGui,
        khoiCoQuanGui: item.khoiCoQuanGui,
        hinhThucNhan: item.hinhThucNhan,
        nguoiKy: item.nguoiKy,

        butphe: item.butphe,
        donViNhanXuLy: item.donViNhanXuLy,
        phanCong: item.phanCong,
        trinhLanhDaoTruong: item.trinhLanhDaoTruong,
        ower: item.ower,
        signDigitals: item.signDigitals
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        version: item.version,
        number: item.number,
        loaiVanBan: item.loaiVanBan,
        loaiVanBanTen: item.loaiVanBanTen,
        trangThai: item.trangThai,
        trangThaiTen: item.trangThaiTen,
        soLuuCV: item.soLuuCV,
        soVBDi: item.soVBDi,
        ngayNhap: item.ngayNhap,
        ngayTraLoi: item.ngayTraLoi,
        ngayBanHanh: item.ngayBanHanh,
        traLoiCVSo: item.traLoiCVSo,
        soBan: item.soBan,
        trichYeu: item.trichYeu,
        donViSoan: item.donViSoan,
        donViSoanTen: item.donViSoanTen,
        canBoSoan: item.canBoSoan,
        canBoSoanTen: item.canBoSoanTen,
        hinhThucGui: item.hinhThucGui,
        hinhThucGuiTen: item.hinhThucGuiTen,
        hanXuLy: item.hanXuLy,
        linhVuc: item.linhVuc,
        linhVucTen: item.linhVucTen,
        mucDoBaoMat: item.mucDoBaoMat,
        mucDoBaoMatTen: item.mucDoBaoMatTen,
        mucDoTinhChat: item.mucDoTinhChat,
        mucDoTinhChatTen: item.mucDoTinhChatTen,
        hoSoDonVi: item.hoSoDonVi,
        hoSoDonViTen: item.hoSoDonViTen,
        noiLuuTru: item.noiLuuTru,
        coQuanNhan: item.coQuanNhan,
        coQuanNhanTen: item.coQuanNhanTen,
        congVanChiDoc: item.congVanChiDoc,
        banChinh: item.banChinh,
        hienThiThongBao: item.hienThiThongBao,
        ngayNhan: item.ngayNhan,
        coQuanGui: item.coQuanGui,
        khoiCoQuanGui: item.khoiCoQuanGui,
        hinhThucNhan: item.hinhThucNhan,
        nguoiKy: item.nguoiKy,

        butphe: item.butphe,
        donViNhanXuLy: item.donViNhanXuLy,
        phanCong: item.phanCong,
        trinhLanhDaoTruong: item.trinhLanhDaoTruong,
        ower: item.ower,
        signDigitals: item.signDigitals
    }
}

const baseJson = (items) => {
    return {
        version: 1,
        id: null,
        number: 0,
        loaiVanBan: null,
        loaiVanBanTen: null,
        trangThai: null,
        trangThaiTen: null,
        soLuuCV: null,
        soVBDi: null,
        ngayNhap: null,
        ngayTraLoi: null,
        ngayBanHanh: null,
        traLoiCVSo: null,
        soBan: 0,
        trichYeu: null,
        donViSoan: null,
        donViSoanTen: null,
        canBoSoan: null,
        canBoSoanTen: null,
        hinhThucGui: null,
        hinhThucGuiTen: null,
        hanXuLy: null,
        linhVuc: null,
        linhVucTen: null,
        mucDoBaoMat: null,
        mucDoBaoMatTen: null,
        mucDoTinhChat: null,
        mucDoTinhChatTen: null,
        hoSoDonVi: null,
        hoSoDonViTen: null,
        noiLuuTru: null,
        coQuanNhan: null,
        coQuanNhanTen: null,

        donViXuLy: null,
        congVanChiDoc: false,
        banChinh: false,
        hienThiThongBao: false,
        ngayNhan: null,
        coQuanGui: null,
        khoiCoQuanGui: null,
        nguoiKy: null,
        ngayKy: null,
        file: null,
        filePDF: null,
        uploadFiles: null,
        nguoiPhanCong: null,
        trinhLanhDaoTruong: false,
        ower: null,
        signDigitals: null
    }
}

const toListModel = (items) =>{
    if(items.length > 0){
        let data = [];
        items.map((value, index) =>{
            data.push(fromJson(value));
        })
        return data??[];
    }
    return [];
}

export const vanBanDiModel = {
    toJson, fromJson, baseJson, toListModel
}
