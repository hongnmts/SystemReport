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
        toChuc: item.toChuc,
        loaiHinh: item.loaiHinh,
        diaChi: item.diaChi,
        donViHanhChinh: item.donViHanhChinh,
        noiDungHoTro: item.noiDungHoTro,
        soTien: item.soTien,
        quyetDinh: item.quyetDinh,
        ngayKy: item.ngayKy,
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
        toChuc: null,
        loaiHinh: null,
        diaChi: null,
        donViHanhChinh: null,
        noiDungHoTro: null,
        soTien: 0,
        quyetDinh: null,
        ngayKy: null,
        createdAt: null,
        modifiedAt: null,
        createdBy: null,
        modifiedBy: null,
    }
}

const baseTKJson = () => {
    return {
        toChuc: null,
        loaiHinh: null,
        diaChi: null,
        donViHanhChinh: null,
        noiDungHoTro: null,
        soTien: 0,
        quyetDinh: null,
        ngayKyStart: null,
        ngayKyEnd: null,
        ngayNhapEnd: null,
        ngayNhapStart: null,
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

export const hoTroDoanhNghiepModel = {
    toJson, fromJson, baseJson, toListModel, baseTKJson
}
