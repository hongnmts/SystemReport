import moment from "moment";
const toJson = (item) => {
    return {
        id: item.id,
        ten: item.ten,
        thuTu: item.thuTu
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        ten: item.ten,
        thuTu: item.thuTu,
        //createdAt: moment(item.createdAt).format('YYYY-MM-DD'),
        createAt:item.createAt,
        // modifiedAt: moment(item.modifiedAt).format('YYYY-MM-DD'),
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

export const chucVuModel = {
    toJson, fromJson, baseJson, toListModel
}
