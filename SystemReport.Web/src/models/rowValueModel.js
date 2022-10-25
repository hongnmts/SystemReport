import {styleModel} from "@/models/StyleModel";
import {formulaModel} from "@/models/FormulaModel";

const toJson = (item) => {
    return {
        id: item.id,
        code: item.code,
        thuTu: item.thuTu,
        level: item.level,

        formula: item.formula,
        styleInput: item.styleInput,
        ghiChu: item.ghiChu,
        fontStyle: item.fontStyle,
        fontWeight: item.fontWeight,
        fontHorizontalAlign: item.fontHorizontalAlign,
        fontVerticalAlign: item.fontVerticalAlign,
        width: item.width,
        tinhTongChiTieuCon: item.tinhTongChiTieuCon,
        isTong: item.isTong,
        parentId: item.parentId,
        bangBieuId: item.bangBieuId,
        thuocTinhId: item.thuocTinhId,

        value: item.value,
        styleValue: item.styleValue,
        order: item.order
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
        code: item.code,
        thuTu: item.thuTu,
        level: item.level,

        formula: item.formula,
        styleInput: item.styleInput,
        ghiChu: item.ghiChu,
        fontStyle: item.fontStyle,
        fontWeight: item.fontWeight,
        fontHorizontalAlign: item.fontHorizontalAlign,
        fontVerticalAlign: item.fontVerticalAlign,
        width: item.width,
        tinhTongChiTieuCon: item.tinhTongChiTieuCon,
        parentId: item.parentId,
        bangBieuId: item.bangBieuId,
        thuocTinhId: item.thuocTinhId,
        isTong: item.isTong,

        value: item.value,
        styleValue: item.styleValue,
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
        code: null,
        thuTu:0,
        level: 1,

        donViTinh: null,
        formula: formulaModel.FormulaModel[0],
        styleInput: formulaModel.StyleInputModel[0],
        ghiChu:{
            kyHieu: "",
            noiDung: ""
        },
        fontStyle: styleModel.FontStyle[0],
        fontWeight: null,
        fontHorizontalAlign: styleModel.FontHorizontalAlign[1],
        fontVerticalAlign:  styleModel.FontVerticalAlign[0],
        width:0,
        tinhTongChiTieuCon: false,
        isTong: false,
        parentId: null,
        bangBieuId: null,
        thuocTinhId: null,
        tenThuocTinh: null,
        value: null,
        styleValue: null,

        createdAt: null,
        modifiedAt: null,
        createdBy: null,
        modifiedBy: null,
        order: 0
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

export const rowValueModel = {
    toJson, fromJson, baseJson, toListModel
}
