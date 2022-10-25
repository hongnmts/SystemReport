const toJson = (item) => {
    return {
        id: item.id,
        code: item.code,
        ten: item.ten,
        thuTu: item.thuTu,
        hienThiTen: item.hienThiTen,
        donViTinh: item.donViTinh,
        mauBieuId: item.mauBieuId
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        code: item.code,
        ten: item.ten,
        thuTu: item.thuTu,
        hienThiTen: item.hienThiTen,
        donViTinh: item.donViTinh,
        mauBieuId: item.mauBieuId,
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
        code: null,
        ten:null,
        thuTu: 0,
        hienThiTen: false,
        donViTinh: null,
        mauBieuId: null,
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

export const bangBieuModel = {
    toJson, fromJson, baseJson, toListModel
}
