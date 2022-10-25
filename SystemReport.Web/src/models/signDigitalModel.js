import moment from 'moment';
const toJson = (item) => {
    return {
        id: item.id,
        type: item.type,
        typeKySo: item.typeKySo,
        x: item.x,
        y: item.y,
        width: item.width,
        height: item.height,
        page: item.page,
        image: item.image,
        imageBase64: item.imageBase64,
        payload: item.payload,
        file: item.file,
        fontFamily: item.fontFamily,
        lineHeight: item.lineHeight,
        lines: item.lines,
        size: item.size,
        text: item.lines != null && item.lines.length > 0? item.lines[0]: "",
        absolute: item.absolute,
        currentuser: item.currentuser,
        fontWeight: item.fontWeight,
        fontSize: item.fontSize,
        widthPage:  item.widthPage,
        heightPage: item.heightPage
    }
}

const fromJson = (item) => {
    return {
        id: item.id,
        type: item.type,
        typeKySo: item.typeKySo,
        x: item.x,
        y: item.y,
        width: item.width,
        height: item.height,
        page: item.page,
        image: item.image,
        imageBase64: item.imageBase64,
        payload: item.payload,
        file: item.file,
        fontFamily: item.fontFamily,
        lineHeight: item.lineHeight,
        lines: item.lines,
        size: item.size,
        text: item.text,
        absolute: item.absolute,
        currentuser: item.currentuser,
        fontWeight: item.fontWeight,
        fontSize: item.fontSize,
        widthPage:  item.widthPage,
        heightPage: item.heightPage
    }
}

const baseJson = () => {
    return {
        id: null,
        type: null,
        typeKySo: null,
        x: null,
        y: null,
        width: null,
        height: null,
        page: 0,
        image: null,
        imageBase64: null,
        payload: null,
        file: null,
        fontFamily: null,
        lineHeight: null,
        lines: null,
        size: null,
        text: null,
        absolute: false,
        currentuser: null,
        fontWeight: "0",
        fontSize: 13,
        widthPage: 0,
        heightPage: 0
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

export const signDigitalModel = {
    toJson, fromJson, baseJson, toListModel
}
