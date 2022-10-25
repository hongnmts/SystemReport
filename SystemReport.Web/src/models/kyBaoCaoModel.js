const toJson = (item) => {
    return {
        id: item.id,
        code: item.code,
        ten: item.ten,
        thuTu: item.thuTu,
        loaiKyBaoCao: item.loaiKyBaoCao,
        nam: item.nam,
        thang: item.thang
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        code: item.code,
        ten: item.ten,
        thuTu: item.thuTu,
        nam: item.nam,
        thang: item.thang,
        loaiKyBaoCao: item.loaiKyBaoCao,
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
        ten: null,
        thuTu: 0,
        loaiKyBaoCao: false,
        nam: null,
        thang: null,
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

export const kyBaoCaoModel = {
    toJson, fromJson, baseJson, toListModel
}
