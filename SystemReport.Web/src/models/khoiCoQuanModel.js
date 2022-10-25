const toJson = (item) => {
    return {
        id: item.id,
        ten: item.ten,
        thuTu: item.thuTu,
        code: item.code
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        ten: item.ten,
        thuTu: item.thuTu,
        code: item.code,
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
        ten: null,
        thuTu: 0,
        code: null,
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

export const khoiCoQuanModel = {
    toJson, fromJson, baseJson, toListModel
}
