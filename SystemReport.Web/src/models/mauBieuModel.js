import {kyBaoCaoModel} from "@/models/kyBaoCaoModel";

const toJson = (item) => {
    return {
        id: item.id,
        code: item.code,
        ten: item.ten,
        tenNgan: item.tenNgan,
        kyHieu: item.kyHieu,
        kyBaoCao: item.kyBaoCao,
        loaiMauBieu: item.loaiMauBieu,
        phanQuyenDonVi: item.phanQuyenDonVi,
        donViNhan: item.donViNhan,
        donViBaoCao: item.donViBaoCao,
        tuNgay: item.tuNgay,
        denNgay: item.denNgay,
        isTemplate: item.isTemplate,
        thuTu: item.thuTu
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        code: item.code,
        ten: item.ten,
        tenNgan: item.tenNgan,
        kyHieu: item.kyHieu,
        loaiMauBieu: item.loaiMauBieu,
        kyBaoCao: item.kyBaoCao,
        donViNhan: item.donViNhan,
        donViBaoCao: item.donViBaoCao,
        phanQuyenDonVi: item.phanQuyenDonVi,
        tuNgay: item.tuNgay,
        denNgay: item.denNgay,
        isTemplate: item.isTemplate,
        thuTu: item.thuTu,
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
        id:null,
        code: null,
        ten: null,
        tenNgan: null,
        kyHieu: null,
        donViNhan: null,
        donViBaoCao: null,
        loaiMauBieu: null,
        phanQuyenDonVi: null,
        tuNgay: null,
        denNgay: null,
        kyBaoCao: kyBaoCaoModel.baseJson(),
        isTemplate: false,
        thuTu: 0,
        createdAt: null,
        modifiedAt: null,
        createdBy: null,
        modifiedBy: null,
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

export const mauBieuModel = {
    toJson, fromJson, baseJson, toListModel
}
