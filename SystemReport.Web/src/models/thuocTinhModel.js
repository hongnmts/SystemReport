import {styleModel} from "@/models/StyleModel";
import {formulaModel} from "@/models/FormulaModel";

const toJson = (item) => {
    return {
        id: item.id,
        ten: item.ten,
        donViTinh: item.donViTinh,
        formula: item.formula,
        styleInput: item.styleInput,
        ghiChu: item.ghiChu??{
            kyHieu: "",
            noiDung: ""
        },
        stringCongThuc: item.stringCongThuc,
        tenKhongDau: item.tenKhongDau,
        thuTu: item.thuTu,
        level: item.level,
        parentId: item.parentId,
        bangBieuId: item.bangBieuId,
        width:item.width,
        fontStyle: item.fontStyle,
        fontWeight: item.fontWeight,
        fontHorizontalAlign: item.fontHorizontalAlign,
        fontVerticalAlign: item.fontVerticalAlign,
        isChiTieu: item.isChiTieu,
        hasChildren: item.hasChildren,
        tinhChiTieuCon:item.tinhChiTieuCon,
        order: item.order
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        ten: item.ten,
        tenKhongDau: item.tenKhongDau,
        thuTu: item.thuTu,
        level: item.level,
        donViTinh: item.donViTinh,
        formula: item.formula,
        styleInput: item.styleInput,
        ghiChu: item.ghiChu??{
            kyHieu: "",
            noiDung: ""
        },
        parentId: item.parentId,
        stringCongThuc: item.stringCongThuc,
        bangBieuId: item.bangBieuId,
        width:item.width,
        tinhChiTieuCon:item.tinhChiTieuCon,
        fontStyle: item.fontStyle,
        fontWeight: item.fontWeight,
        fontHorizontalAlign: item.fontHorizontalAlign,
        fontVerticalAlign: item.fontVerticalAlign,
        isChiTieu: item.isChiTieu,
        hasChildren: item.hasChildren,
        createAt:item.createAt,
        modifiedAt: item.modifiedAt,
        createdBy: item.createdBy,
        modifiedBy: item.modifiedBy,
        lastModifiedShow: item.lastModifiedShow,
        createdAtShow : item.createdAtShow,
        order: item.order
    }
}

const baseJson = () => {
    return {
        id: null,
        ten: null,
        tenKhongDau: null,
        thuTu:0,
        width:0,
        donViTinh: null,
        formula: formulaModel.FormulaModel[0],
        styleInput: formulaModel.StyleInputModel[0],
        ghiChu:{
          kyHieu: "",
          noiDung: ""
        },
        stringCongThuc: null,
        fontStyle: styleModel.FontStyle[0],
        fontWeight: styleModel.FontWeight[0],
        fontHorizontalAlign: styleModel.FontHorizontalAlign[0],
        fontVerticalAlign:  styleModel.FontVerticalAlign[0],
        isChiTieu: false,
        tinhChiTieuCon: false,
        hasChildren: false,
        level: 1,
        parentId: null,
        bangBieuId: null,
        createdAt: null,
        modifiedAt: null,
        createdBy: null,
        modifiedBy: null,
        order: 0,
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

export const thuocTinhModel = {
    toJson, fromJson, baseJson, toListModel
}
