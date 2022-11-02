const toJson = (item) => {
    return {
        id: item.id,
        name: item.name,
        sort: item.sort,
        permissions: item.permissions,
        // createdAt: item.createdAt,
        // modifiedAt: item.modifiedAt
    }
}

const fromJson = (item) => {
    return {
        id: item.id,
        name: item.name,
        sort: item.sort,
        permissions: item.permissions,
        createdAt:item.createdAt,
        modifiedAt: item.modifiedAt,
        isDeleted: item.isDeleted
    }
}

const baseJson = () => {
    return {
        id: null,
        name: null,
        sort: 0,
        permissions: null,
        isDeleted: false
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

export const moduleModel = {
    toJson, fromJson, baseJson, toListModel
}
