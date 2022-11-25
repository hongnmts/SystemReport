const toJson = (item) => {
    return {
        id: item.id,
        toChuc: item.toChuc,
        loaiHinh: item.loaiHinh,
        diaChi: item.diaChi,
        donViHanhChinh: item.donViHanhChinh,
        noiDungHoTro: item.noiDungHoTro,
        soTien: item.soTien,
        quyetDinh: item.quyetDinh,
        ngayKy: item.ngayKy,
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        tenDeTai: item.tenDeTai,
        chuTri: item.chuTri,
        chuNhiem: item.chuNhiem,
        linhVuc: item.linhVuc,
        quyetDinhPDKQ: item.quyetDinhPDKQ,
        ngayPDKQ: item.ngayPDKQ,
        nguonNSNN: 0,
        nguonKhac: 0,
        ngayBatDau: null,
        ngayKetThuc: null,
        ngayGiaHan: null,
        dangThucHien: null,
        ngayNghiemThu: null,
        xepLoai: null,
        quyetDinhCQ: null,
        ngayChuyenGiao: null,
        donViTiepNhan: null,
        nucTieu: null,
        noiDung: null,
        sanPham: null,
        capQuanLy: null,
        ngayDungNghiemThu: null,
        pheDuyetNhiemVu: null,
        ngayPheDuyetNhiemVu: null,
        createAt:item.createAt,
        modifiedAt: item.modifiedAt,
        createdBy: item.createdBy,
        modifiedBy: item.modifiedBy,
        lastModifiedShow: item.lastModifiedShow,
        createdAtShow : item.createdAtShow
    }
}

const baseJson = () => {
    return {
        id: null,
        tenDeTai: null,
        chuTri: null,
        chuNhiem: null,
        linhVuc: null,
        quyetDinhPDKQ: null,
        ngayPDKQ: null,
        nguonNSNN: 0,
        nguonKhac: 0,
        ngayBatDau: null,
        ngayKetThuc: null,
        ngayGiaHan: null,
        dangThucHien: null,
        ngayNghiemThu: null,
        xepLoai: null,
        quyetDinhCQ: null,
        ngayChuyenGiao: null,
        donViTiepNhan: null,
        mucTieu: null,
        noiDung: null,
        sanPham: null,
        capQuanLy: null,
        ngayDungNghiemThu: null,
        pheDuyetNhiemVu: null,
        ngayPheDuyetNhiemVu: null,
        createdAt: null,
        modifiedAt: null,
        createdBy: null,
        modifiedBy: null,
    }
}

const baseTKJson = () => {
    return {
        capQuanLy: null,
        tenDeTai: null,
        chuTri: null,
        chuNhiem: null,
        linhVuc: null,
        pheDuyetNV: null,
        quyetDinhPDKQ: null,
        dangThucHien: null,
        xepLoai: null,
        quyetDinhCQ: null,
        donViTiepNhan: null,
        ngayPDKQStart: null,
        ngayPDKQEnd: null,
        ngayBatDauStart: null,
        ngayBatDauEnd: null,
        ngayKetThucStart: null,
        ngayKetThucEnd: null,
        ngayGiaHanStart: null,
        ngayGiaHanEnd: null,
        ngayNghiemThuStart: null,
        ngayNghiemThuEnd: null,
        ngayChuyenGiaoStart: null,
        ngayChuyenGiaoEnd: null,
        start: 0,
        limit: 10
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

export const quanLyKhoaHocModel = {
    toJson, fromJson, baseJson, toListModel, baseTKJson
}
