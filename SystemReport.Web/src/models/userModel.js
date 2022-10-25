const toJson = (item) => {
    return {
        id: item.id,
        userName: item.userName,
        firstName: item.firstName,
        lastName: item.lastName,
        fullName: item.fullName,
        phoneNumber: item.phoneNumber,
        email: item.email,
        note: item.note,
        avatar: item.avatar,
        donVi: item.donVi,
        roles: item.roles,
        permissions: item.permissions,
        menu: item.menu,
        kySo: item.kySo,
        chucVu: item.chucVu,
        userNameKySo: item.userNameKySo,
        passwordKySo: item.passwordKySo,
        tuNgay: item.tuNgay,
        denNgay: item.denNgay,
    }
}

const fromJson = (item) => {
    return {
        id: item.id,
        usrName: item.userName,
        firstName: item.firstName,
        lastName: item.lastName,
        fullName: item.fullName,
        phoneNumber: item.phoneNumber,
        email: item.email,
        note: item.note,
        avatar: item.avatar,
        donVi: item.donVi,
        roles: item.roles,
        permissions: item.permissions,
        menu: item.menu,
        kySo: item.kySo,
        chucVu: item.chucVu,
        userNameKySo: item.userNameKySo,
        passwordKySo: item.passwordKySo,
        tuNgay: item.tuNgay,
        denNgay: item.denNgay,
    }
}

const baseJson = () => {
    return {
        id: null,
        usrName: null,
        firstName: null,
        lastName: null,
        fullName: null,
        phoneNumber: null,
        email: null,
        note: null,
        avatar: null,
        kySo: null,
        donVi: {id: null, ten: null},
        roles: null,
        chucVu: null,
        userNameKySo: null,
        passwordKySo: null,
        tuNgay: null,
        denNgay: null
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
export const userModel = {
    toJson, fromJson, baseJson, toListModel
}
