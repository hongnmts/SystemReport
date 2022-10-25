const toJson = (item) => {
    return {
        soCVDen: item.soCVDen,
        soCVDi: item.soCVDi,
        soThu: item.soThu,
        soThongBao: item.soThongBao,
    }
}
const fromJson = (item) => {
    return {
        soCVDen: item.soCVDen,
        soCVDi: item.soCVDi,
        soThu: item.soThu,
        soThongBao: item.soThongBao,
    }
}

const baseJson = () => {
    return {
        soCVDen: 0,
        soCVDi: 0,
        soThu: 0,
        soThongBao: 0,
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

export const dashboardModel = {
    toJson, fromJson, baseJson, toListModel
}
