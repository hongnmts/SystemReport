(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-4cf6aa25"],{"1ab5":function(e,t,a){"use strict";a("4b06")},"4b06":function(e,t,a){},aa1a:function(e,t,a){"use strict";a("c32d")},be9d:function(e,t,a){"use strict";a.d(t,"a",(function(){return l}));a("d81d");var n=function(e){return{id:e.id,toChuc:e.toChuc,loaiHinh:e.loaiHinh,diaChi:e.diaChi,donViHanhChinh:e.donViHanhChinh,noiDungHoTro:e.noiDungHoTro,soTien:e.soTien,quyetDinh:e.quyetDinh,ngayKy:e.ngayKy}},o=function(e){return{id:e.id,toChuc:e.toChuc,loaiHinh:e.loaiHinh,diaChi:e.diaChi,donViHanhChinh:e.donViHanhChinh,noiDungHoTro:e.noiDungHoTro,soTien:e.soTien,quyetDinh:e.quyetDinh,ngayKy:e.ngayKy,createAt:e.createAt,modifiedAt:e.modifiedAt,createdBy:e.createdBy,modifiedBy:e.modifiedBy,lastModifiedShow:e.lastModifiedShow,createdAtShow:e.createdAtShow}},i=function(){return{id:null,toChuc:null,loaiHinh:null,diaChi:null,donViHanhChinh:null,noiDungHoTro:null,soTien:0,quyetDinh:null,ngayKy:null,createdAt:null,modifiedAt:null,createdBy:null,modifiedBy:null}},r=function(){return{toChuc:null,loaiHinh:null,diaChi:null,donViHanhChinh:null,noiDungHoTro:null,soTien:0,quyetDinh:null,ngayKyStart:null,ngayKyEnd:null,ngayNhapEnd:null,ngayNhapStart:null,start:0,limit:10}},s=function(e){if(e.length>0){var t=[];return e.map((function(e,a){t.push(o(e))})),null!==t&&void 0!==t?t:[]}return[]},l={toJson:n,fromJson:o,baseJson:i,toListModel:s,baseTKJson:r}},c32d:function(e,t,a){},d8cc:function(e,t,a){"use strict";a.d(t,"a",(function(){return l}));a("d81d");var n=function(e){return{id:e.id,toChuc:e.toChuc,loaiHinh:e.loaiHinh,diaChi:e.diaChi,donViHanhChinh:e.donViHanhChinh,noiDungHoTro:e.noiDungHoTro,soTien:e.soTien,quyetDinh:e.quyetDinh,ngayKy:e.ngayKy}},o=function(e){return{id:e.id,tenDeTai:e.tenDeTai,chuTri:e.chuTri,chuNhiem:e.chuNhiem,linhVuc:e.linhVuc,quyetDinhPDKQ:e.quyetDinhPDKQ,ngayPDKQ:e.ngayPDKQ,nguonNSNN:0,nguonKhac:0,ngayBatDau:null,ngayKetThuc:null,ngayGiaHan:null,dangThucHien:null,ngayNghiemThu:null,xepLoai:null,quyetDinhCQ:null,ngayChuyenGiao:null,donViTiepNhan:null,nucTieu:null,noiDung:null,sanPham:null,createAt:e.createAt,modifiedAt:e.modifiedAt,createdBy:e.createdBy,modifiedBy:e.modifiedBy,lastModifiedShow:e.lastModifiedShow,createdAtShow:e.createdAtShow}},i=function(){return{id:null,tenDeTai:null,chuTri:null,chuNhiem:null,linhVuc:null,quyetDinhPDKQ:null,ngayPDKQ:null,nguonNSNN:0,nguonKhac:0,ngayBatDau:null,ngayKetThuc:null,ngayGiaHan:null,dangThucHien:null,ngayNghiemThu:null,xepLoai:null,quyetDinhCQ:null,ngayChuyenGiao:null,donViTiepNhan:null,nucTieu:null,noiDung:null,sanPham:null,createdAt:null,modifiedAt:null,createdBy:null,modifiedBy:null}},r=function(){return{tenDeTai:null,chuTri:null,chuNhiem:null,linhVuc:null,quyetDinhPDKQ:null,dangThucHien:null,xepLoai:null,quyetDinhCQ:null,donViTiepNhan:null,ngayPDKQStart:null,ngayPDKQEnd:null,ngayBatDauStart:null,ngayBatDauEnd:null,ngayKetThucStart:null,ngayKetThucEnd:null,ngayGiaHanStart:null,ngayGiaHanEnd:null,ngayNghiemThuStart:null,ngayNghiemThuEnd:null,ngayChuyenGiaoStart:null,ngayChuyenGiaoEnd:null,start:0,limit:10}},s=function(e){if(e.length>0){var t=[];return e.map((function(e,a){t.push(o(e))})),null!==t&&void 0!==t?t:[]}return[]},l={toJson:n,fromJson:o,baseJson:i,toListModel:s,baseTKJson:r}},fb0d:function(e,t,a){"use strict";a.r(t);var n=function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("Layout",[a("PageHeader",{attrs:{title:e.title,items:e.items}}),a("b-card",[a("form",{ref:"formContainer",on:{submit:function(t){return t.preventDefault(),e.handleSubmit.apply(null,arguments)}}},[a("div",{staticClass:"text-end pt-2 mb-2"},[a("b-button",{staticClass:"ms-1 w-md",attrs:{type:"submit",variant:"primary",size:"sm"}},[e._v(" Lưu ")]),a("b-button",{staticClass:"ms-1 w-md",attrs:{type:"submit",variant:"danger",size:"sm"},on:{click:function(t){return e.$router.push("/quan-ly-de-tai")}}},[e._v(" Trở về ")])],1),a("div",{staticClass:"row"},[a("div",{staticClass:"col-lg-12 col-md-12"},[a("div",{staticClass:"row"},[a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Tên đề tài")]),e._v(" "),a("span",{staticClass:"text-danger"},[e._v("*")]),a("multiselect",{attrs:{options:e.optionsDeTai,"track-by":"id",label:"name",placeholder:"Chọn","deselect-label":"Nhấn để xoá",selectLabel:"Nhấn enter để chọn",selectedLabel:"Đã chọn",taggable:!0},on:{tag:e.addTagDeTai},model:{value:e.model.tenDeTai,callback:function(t){e.$set(e.model,"tenDeTai",t)},expression:"model.tenDeTai"}})],1)]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Tổ chức chủ trì")]),e._v(" "),a("span",{staticClass:"text-danger"},[e._v("*")]),a("multiselect",{attrs:{options:e.optionsChuTri,"track-by":"id",label:"name",placeholder:"Chọn","deselect-label":"Nhấn để xoá",selectLabel:"Nhấn enter để chọn",selectedLabel:"Đã chọn",taggable:!0},on:{tag:e.addTagChuTri},model:{value:e.model.chuTri,callback:function(t){e.$set(e.model,"chuTri",t)},expression:"model.chuTri"}})],1)]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Chủ nhiệm")]),e._v(" "),a("span",{staticClass:"text-danger"},[e._v("*")]),a("multiselect",{attrs:{options:e.optionsChuNhiem,"track-by":"id",label:"name",placeholder:"Chọn","deselect-label":"Nhấn để xoá",selectLabel:"Nhấn enter để chọn",selectedLabel:"Đã chọn",taggable:!0},on:{tag:e.addTagChuNhiem},model:{value:e.model.chuNhiem,callback:function(t){e.$set(e.model,"chuNhiem",t)},expression:"model.chuNhiem"}})],1)]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Lĩnh vực")]),e._v(" "),a("span",{staticClass:"text-danger"},[e._v("*")]),a("multiselect",{attrs:{options:e.optionsLinhVuc,"track-by":"id",label:"name",placeholder:"Chọn","deselect-label":"Nhấn để xoá",selectLabel:"Nhấn enter để chọn",selectedLabel:"Đã chọn",taggable:!0},on:{tag:e.addTagLinhVuc},model:{value:e.model.linhVuc,callback:function(t){e.$set(e.model,"linhVuc",t)},expression:"model.linhVuc"}})],1)]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("QĐ phê duyệt KP")]),e._v(" "),a("span",{staticClass:"text-danger"},[e._v("*")]),a("multiselect",{attrs:{options:e.optionsQDPheDuyetKP,"track-by":"id",label:"name",placeholder:"Chọn","deselect-label":"Nhấn để xoá",selectLabel:"Nhấn enter để chọn",selectedLabel:"Đã chọn",taggable:!0},on:{tag:e.addTagQDPheDuyet},model:{value:e.model.quyetDinhPDKQ,callback:function(t){e.$set(e.model,"quyetDinhPDKQ",t)},expression:"model.quyetDinhPDKQ"}})],1)]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Ngày phê duyệt")]),a("date-picker",{attrs:{format:"DD/MM/YYYY","value-type":"format"},model:{value:e.model.ngayPDKQ,callback:function(t){e.$set(e.model,"ngayPDKQ",t)},expression:"model.ngayPDKQ"}},[a("div",{attrs:{slot:"input"},slot:"input"},[a("input",{directives:[{name:"model",rawName:"v-model",value:e.model.ngayPDKQ,expression:"model.ngayPDKQ"},{name:"mask",rawName:"v-mask",value:"##/##/####",expression:"'##/##/####'"}],staticClass:"form-control",attrs:{type:"text",placeholder:""},domProps:{value:e.model.ngayPDKQ},on:{input:function(t){t.target.composing||e.$set(e.model,"ngayPDKQ",t.target.value)}}})])])],1)]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Nguồn NSNN")]),a("input",{directives:[{name:"model",rawName:"v-model",value:e.model.nguonNSNN,expression:"model.nguonNSNN"}],staticClass:"form-control",attrs:{id:"validationCustom01",type:"text",placeholder:""},domProps:{value:e.model.nguonNSNN},on:{input:function(t){t.target.composing||e.$set(e.model,"nguonNSNN",t.target.value)}}})])]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Nguồn khác")]),a("input",{directives:[{name:"model",rawName:"v-model",value:e.model.nguonKhac,expression:"model.nguonKhac"}],staticClass:"form-control",attrs:{id:"validationCustom01",type:"text",placeholder:""},domProps:{value:e.model.nguonKhac},on:{input:function(t){t.target.composing||e.$set(e.model,"nguonKhac",t.target.value)}}})])]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Bắt đầu")]),a("date-picker",{attrs:{format:"DD/MM/YYYY","value-type":"format"},model:{value:e.model.ngayBatDau,callback:function(t){e.$set(e.model,"ngayBatDau",t)},expression:"model.ngayBatDau"}},[a("div",{attrs:{slot:"input"},slot:"input"},[a("input",{directives:[{name:"model",rawName:"v-model",value:e.model.ngayBatDau,expression:"model.ngayBatDau"},{name:"mask",rawName:"v-mask",value:"##/##/####",expression:"'##/##/####'"}],staticClass:"form-control",attrs:{type:"text",placeholder:"Nhập ngày ký"},domProps:{value:e.model.ngayBatDau},on:{input:function(t){t.target.composing||e.$set(e.model,"ngayBatDau",t.target.value)}}})])])],1)]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Kết thúc")]),a("date-picker",{attrs:{format:"DD/MM/YYYY","value-type":"format"},model:{value:e.model.ngayKetThuc,callback:function(t){e.$set(e.model,"ngayKetThuc",t)},expression:"model.ngayKetThuc"}},[a("div",{attrs:{slot:"input"},slot:"input"},[a("input",{directives:[{name:"model",rawName:"v-model",value:e.model.ngayKetThuc,expression:"model.ngayKetThuc"},{name:"mask",rawName:"v-mask",value:"##/##/####",expression:"'##/##/####'"}],staticClass:"form-control",attrs:{type:"text",placeholder:""},domProps:{value:e.model.ngayKetThuc},on:{input:function(t){t.target.composing||e.$set(e.model,"ngayKetThuc",t.target.value)}}})])])],1)]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Gia hạn")]),a("date-picker",{attrs:{format:"DD/MM/YYYY","value-type":"format"},model:{value:e.model.ngayGiaHan,callback:function(t){e.$set(e.model,"ngayGiaHan",t)},expression:"model.ngayGiaHan"}},[a("div",{attrs:{slot:"input"},slot:"input"},[a("input",{directives:[{name:"model",rawName:"v-model",value:e.model.ngayGiaHan,expression:"model.ngayGiaHan"},{name:"mask",rawName:"v-mask",value:"##/##/####",expression:"'##/##/####'"}],staticClass:"form-control",attrs:{type:"text",placeholder:""},domProps:{value:e.model.ngayGiaHan},on:{input:function(t){t.target.composing||e.$set(e.model,"ngayGiaHan",t.target.value)}}})])])],1)]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Đang thực hiện")]),e._v(" "),a("span",{staticClass:"text-danger"},[e._v("*")]),a("multiselect",{attrs:{options:e.optionsDangThucHien,"track-by":"id",label:"name",placeholder:"Chọn","deselect-label":"Nhấn để xoá",selectLabel:"Nhấn enter để chọn",selectedLabel:"Đã chọn",taggable:!0},on:{tag:e.addTagDangThucHien},model:{value:e.model.dangThucHien,callback:function(t){e.$set(e.model,"dangThucHien",t)},expression:"model.dangThucHien"}})],1)]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Ngày nghiệm thu")]),a("date-picker",{attrs:{format:"DD/MM/YYYY","value-type":"format"},model:{value:e.model.ngayNghiemThu,callback:function(t){e.$set(e.model,"ngayNghiemThu",t)},expression:"model.ngayNghiemThu"}},[a("div",{attrs:{slot:"input"},slot:"input"},[a("input",{directives:[{name:"model",rawName:"v-model",value:e.model.ngayNghiemThu,expression:"model.ngayNghiemThu"},{name:"mask",rawName:"v-mask",value:"##/##/####",expression:"'##/##/####'"}],staticClass:"form-control",attrs:{type:"text",placeholder:""},domProps:{value:e.model.ngayNghiemThu},on:{input:function(t){t.target.composing||e.$set(e.model,"ngayNghiemThu",t.target.value)}}})])])],1)]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Xếp loại")]),e._v(" "),a("span",{staticClass:"text-danger"},[e._v("*")]),a("multiselect",{attrs:{options:e.optionsXepLoai,"track-by":"id",label:"name",placeholder:"Chọn","deselect-label":"Nhấn để xoá",selectLabel:"Nhấn enter để chọn",selectedLabel:"Đã chọn",taggable:!0},on:{tag:e.addTagXepLoai},model:{value:e.model.xepLoai,callback:function(t){e.$set(e.model,"xepLoai",t)},expression:"model.xepLoai"}})],1)]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("QĐ chuyển giao")]),e._v(" "),a("span",{staticClass:"text-danger"},[e._v("*")]),a("multiselect",{attrs:{options:e.optionsQuyetDinhCG,"track-by":"id",label:"name",placeholder:"Chọn","deselect-label":"Nhấn để xoá",selectLabel:"Nhấn enter để chọn",selectedLabel:"Đã chọn",taggable:!0},on:{tag:e.addTagQDChuyenGiao},model:{value:e.model.quyetDinhCQ,callback:function(t){e.$set(e.model,"quyetDinhCQ",t)},expression:"model.quyetDinhCQ"}})],1)]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Ngày chuyển giao")]),a("date-picker",{attrs:{format:"DD/MM/YYYY","value-type":"format"},model:{value:e.model.ngayChuyenGiao,callback:function(t){e.$set(e.model,"ngayChuyenGiao",t)},expression:"model.ngayChuyenGiao"}},[a("div",{attrs:{slot:"input"},slot:"input"},[a("input",{directives:[{name:"model",rawName:"v-model",value:e.model.ngayChuyenGiao,expression:"model.ngayChuyenGiao"},{name:"mask",rawName:"v-mask",value:"##/##/####",expression:"'##/##/####'"}],staticClass:"form-control",attrs:{type:"text",placeholder:""},domProps:{value:e.model.ngayChuyenGiao},on:{input:function(t){t.target.composing||e.$set(e.model,"ngayChuyenGiao",t.target.value)}}})])])],1)]),a("div",{staticClass:"col-md-6"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Đơn vị tiếp nhận")]),e._v(" "),a("span",{staticClass:"text-danger"},[e._v("*")]),a("multiselect",{attrs:{options:e.optionsDonViTiepNhan,"track-by":"id",label:"name",placeholder:"Chọn","deselect-label":"Nhấn để xoá",selectLabel:"Nhấn enter để chọn",selectedLabel:"Đã chọn",taggable:!0,multiple:!0},on:{tag:e.addTagDonViTiepNhan},model:{value:e.model.donViTiepNhan,callback:function(t){e.$set(e.model,"donViTiepNhan",t)},expression:"model.donViTiepNhan"}})],1)]),a("div",{staticClass:"col-md-12"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Mục tiêu")]),a("span",{staticClass:"text-danger"},[e._v("*")]),a("ckeditor",{attrs:{editor:e.editor,config:e.editorConfig},model:{value:e.model.mucTieu,callback:function(t){e.$set(e.model,"mucTieu",t)},expression:"model.mucTieu"}})],1)]),a("div",{staticClass:"col-md-12"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Nội dung")]),a("span",{staticClass:"text-danger"},[e._v("*")]),a("ckeditor",{attrs:{editor:e.editor,config:e.editorConfig},model:{value:e.model.noiDung,callback:function(t){e.$set(e.model,"noiDung",t)},expression:"model.noiDung"}})],1)]),a("div",{staticClass:"col-md-12"},[a("div",{staticClass:"mb-2"},[a("label",{staticClass:"form-label",attrs:{for:"validationCustom01"}},[e._v("Sản phẩm")]),a("span",{staticClass:"text-danger"},[e._v("*")]),a("ckeditor",{attrs:{editor:e.editor,config:e.editorConfig},model:{value:e.model.sanPham,callback:function(t){e.$set(e.model,"sanPham",t)},expression:"model.sanPham"}})],1)])])])])])])],1)},o=[],i=a("2909"),r=a("1da1"),s=(a("96cf"),a("a4d3"),a("e01a"),a("ac1f"),a("1276"),a("99af"),a("6a34")),l=a("2579"),u=a("c2a4"),c=a("8e5f"),d=a.n(c),m=a("be9d"),h=a("47c6"),p=a("ec45"),g=a("d8cc"),v=a("3730"),f=a.n(v),C=a("fb3d"),y=a.n(C),b={page:{title:"Quản lý đề tài",meta:[{name:"description",content:u.description}]},components:{Layout:s["a"],PageHeader:l["a"],Multiselect:d.a,DatePicker:p["a"],ckeditor:f.a.component},data:function(){return{title:"Thêm hoặc chỉnh sửa",items:[{text:"Quản lý đề tài",href:"/"}],submitted:!1,model:g["a"].baseJson(),optionsLoaiVanBan:[],editorConfig:{height:"200px",toolbar:{items:["heading","bold","italic","bulletedList","numberedList","undo","redo"]}},editor:y.a,apiUrl:"https://apistis.dongthap.gov.vn/api/v1/",url:"".concat("https://apistis.dongthap.gov.vn/api/v1/","files/upload"),dropzoneOptions:{url:"".concat("https://apistis.dongthap.gov.vn/api/v1/","files/upload"),thumbnailWidth:300,thumbnailHeight:160,maxFiles:1,maxFilesize:30,headers:{"My-Awesome-Header":"header value"},addRemoveLinks:!0,acceptedFiles:".jpeg,.jpg,.png,.gif,.doc,.docx,.xlsx,.pptx,.pdf"},optionsDeTai:[],optionsChuTri:[],optionsChuNhiem:[],optionsLinhVuc:[],optionsQDPheDuyetKP:[],optionsDangThucHien:[],optionsXepLoai:[],optionsQuyetDinhCG:[],optionsDonViTiepNhan:[],action:null}},validations:{},mounted:function(){},watch:{},created:function(){var e=this;return Object(r["a"])(regeneratorRuntime.mark((function t(){return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:e.getDeTai(),e.getChuNhiem(),e.getXepLoai(),e.getDangThucHien(),e.getDonViTiepNhan(),e.getChuTri(),e.getLinhVuc(),e.getQDChuyenGiao(),e.getQDPheDuyet(),e.$route.params.id?e.handleDetail():e.handleCreate();case 10:case"end":return t.stop()}}),t)})))()},methods:{getDeTai:function(){var e=this;return Object(r["a"])(regeneratorRuntime.mark((function t(){return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:return t.prev=0,t.next=3,e.$store.dispatch("commonItemStore/getByType","DETAI").then((function(t){if("SUCCESS"==t.resultCode){var a=t.data;e.loading=!1,e.optionsDeTai=a}return[]}));case 3:return t.prev=3,e.loading=!1,t.finish(3);case 6:case"end":return t.stop()}}),t,null,[[0,,3,6]])})))()},addTagDeTai:function(e){var t=this;return Object(r["a"])(regeneratorRuntime.mark((function a(){var n,o;return regeneratorRuntime.wrap((function(a){while(1)switch(a.prev=a.next){case 0:return n=e.split(", "),o={name:n.pop(),type:"DETAI"},a.next=4,t.$store.dispatch("commonItemStore/create",o).then((function(e){"SUCCESS"===e.resultCode&&(t.optionsDeTai=[e.data].concat(Object(i["a"])(t.optionsDeTai)),t.model.tenDeTai=e.data),t.$store.dispatch("snackBarStore/addNotify",h["a"].addMessage(e))}));case 4:case"end":return a.stop()}}),a)})))()},getChuNhiem:function(){var e=this;return Object(r["a"])(regeneratorRuntime.mark((function t(){return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:return t.prev=0,t.next=3,e.$store.dispatch("commonItemStore/getByType","CHUNHIEM").then((function(t){if("SUCCESS"==t.resultCode){var a=t.data;e.loading=!1,e.optionsChuNhiem=a}return[]}));case 3:return t.prev=3,e.loading=!1,t.finish(3);case 6:case"end":return t.stop()}}),t,null,[[0,,3,6]])})))()},addTagChuNhiem:function(e){var t=this;return Object(r["a"])(regeneratorRuntime.mark((function a(){var n,o;return regeneratorRuntime.wrap((function(a){while(1)switch(a.prev=a.next){case 0:return n=e.split(", "),o={name:n.pop(),type:"CHUNHIEM"},a.next=4,t.$store.dispatch("commonItemStore/create",o).then((function(e){"SUCCESS"===e.resultCode&&(t.optionsChuNhiem=[e.data].concat(Object(i["a"])(t.optionsChuNhiem)),t.model.chuNhiem=e.data),t.$store.dispatch("snackBarStore/addNotify",h["a"].addMessage(e))}));case 4:case"end":return a.stop()}}),a)})))()},getChuTri:function(){var e=this;return Object(r["a"])(regeneratorRuntime.mark((function t(){return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:return t.prev=0,t.next=3,e.$store.dispatch("commonItemStore/getByType","CHUTRI").then((function(t){if("SUCCESS"==t.resultCode){var a=t.data;e.loading=!1,e.optionsChuTri=a}return[]}));case 3:return t.prev=3,e.loading=!1,t.finish(3);case 6:case"end":return t.stop()}}),t,null,[[0,,3,6]])})))()},addTagChuTri:function(e){var t=this;return Object(r["a"])(regeneratorRuntime.mark((function a(){var n,o;return regeneratorRuntime.wrap((function(a){while(1)switch(a.prev=a.next){case 0:return n=e.split(", "),o={name:n.pop(),type:"CHUTRI"},a.next=4,t.$store.dispatch("commonItemStore/create",o).then((function(e){"SUCCESS"===e.resultCode&&(t.optionsChuTri=[e.data].concat(Object(i["a"])(t.optionsChuTri)),t.model.chuTri=e.data),t.$store.dispatch("snackBarStore/addNotify",h["a"].addMessage(e))}));case 4:case"end":return a.stop()}}),a)})))()},getLinhVuc:function(){var e=this;return Object(r["a"])(regeneratorRuntime.mark((function t(){return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:return t.prev=0,t.next=3,e.$store.dispatch("commonItemStore/getByType","LINHVUC").then((function(t){if("SUCCESS"==t.resultCode){var a=t.data;e.loading=!1,e.optionsLinhVuc=a}return[]}));case 3:return t.prev=3,e.loading=!1,t.finish(3);case 6:case"end":return t.stop()}}),t,null,[[0,,3,6]])})))()},addTagLinhVuc:function(e){var t=this;return Object(r["a"])(regeneratorRuntime.mark((function a(){var n,o;return regeneratorRuntime.wrap((function(a){while(1)switch(a.prev=a.next){case 0:return n=e.split(", "),o={name:n.pop(),type:"LINHVUC"},a.next=4,t.$store.dispatch("commonItemStore/create",o).then((function(e){"SUCCESS"===e.resultCode&&(t.optionsLinhVuc=[e.data].concat(Object(i["a"])(t.optionsLinhVuc)),t.model.linhVuc=e.data),t.$store.dispatch("snackBarStore/addNotify",h["a"].addMessage(e))}));case 4:case"end":return a.stop()}}),a)})))()},getQDPheDuyet:function(){var e=this;return Object(r["a"])(regeneratorRuntime.mark((function t(){return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:return t.prev=0,t.next=3,e.$store.dispatch("commonItemStore/getByType","QUYETDINHPHEQUYET").then((function(t){if("SUCCESS"==t.resultCode){var a=t.data;e.loading=!1,e.optionsQDPheDuyetKP=a}return[]}));case 3:return t.prev=3,e.loading=!1,t.finish(3);case 6:case"end":return t.stop()}}),t,null,[[0,,3,6]])})))()},addTagQDPheDuyet:function(e){var t=this;return Object(r["a"])(regeneratorRuntime.mark((function a(){var n,o;return regeneratorRuntime.wrap((function(a){while(1)switch(a.prev=a.next){case 0:return n=e.split(", "),o={name:n.pop(),type:"QUYETDINHPHEQUYET"},a.next=4,t.$store.dispatch("commonItemStore/create",o).then((function(e){"SUCCESS"===e.resultCode&&(t.optionsQDPheDuyetKP=[e.data].concat(Object(i["a"])(t.optionsQDPheDuyetKP)),t.model.quyetDinhPDKQ=e.data),t.$store.dispatch("snackBarStore/addNotify",h["a"].addMessage(e))}));case 4:case"end":return a.stop()}}),a)})))()},getDangThucHien:function(){var e=this;return Object(r["a"])(regeneratorRuntime.mark((function t(){return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:return t.prev=0,t.next=3,e.$store.dispatch("commonItemStore/getByType","DANGTHUCHIEN").then((function(t){if("SUCCESS"==t.resultCode){var a=t.data;e.loading=!1,e.optionsDangThucHien=a}return[]}));case 3:return t.prev=3,e.loading=!1,t.finish(3);case 6:case"end":return t.stop()}}),t,null,[[0,,3,6]])})))()},addTagDangThucHien:function(e){var t=this;return Object(r["a"])(regeneratorRuntime.mark((function a(){var n,o;return regeneratorRuntime.wrap((function(a){while(1)switch(a.prev=a.next){case 0:return n=e.split(", "),o={name:n.pop(),type:"DANGTHUCHIEN"},a.next=4,t.$store.dispatch("commonItemStore/create",o).then((function(e){"SUCCESS"===e.resultCode&&(t.optionsDangThucHien=[e.data].concat(Object(i["a"])(t.optionsDangThucHien)),t.model.dangThucHien=e.data),t.$store.dispatch("snackBarStore/addNotify",h["a"].addMessage(e))}));case 4:case"end":return a.stop()}}),a)})))()},getXepLoai:function(){var e=this;return Object(r["a"])(regeneratorRuntime.mark((function t(){return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:return t.prev=0,t.next=3,e.$store.dispatch("commonItemStore/getByType","XEPLOAI").then((function(t){if("SUCCESS"==t.resultCode){var a=t.data;e.loading=!1,e.optionsXepLoai=a}return[]}));case 3:return t.prev=3,e.loading=!1,t.finish(3);case 6:case"end":return t.stop()}}),t,null,[[0,,3,6]])})))()},addTagXepLoai:function(e){var t=this;return Object(r["a"])(regeneratorRuntime.mark((function a(){var n,o;return regeneratorRuntime.wrap((function(a){while(1)switch(a.prev=a.next){case 0:return n=e.split(", "),o={name:n.pop(),type:"XEPLOAI"},a.next=4,t.$store.dispatch("commonItemStore/create",o).then((function(e){"SUCCESS"===e.resultCode&&(t.optionsXepLoai=[e.data].concat(Object(i["a"])(t.optionsXepLoai)),t.model.xepLoai=e.data),t.$store.dispatch("snackBarStore/addNotify",h["a"].addMessage(e))}));case 4:case"end":return a.stop()}}),a)})))()},getQDChuyenGiao:function(){var e=this;return Object(r["a"])(regeneratorRuntime.mark((function t(){return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:return t.prev=0,t.next=3,e.$store.dispatch("commonItemStore/getByType","QUYETDINHCHUYENGIAO").then((function(t){if("SUCCESS"==t.resultCode){var a=t.data;e.loading=!1,e.optionsQuyetDinhCG=a}return[]}));case 3:return t.prev=3,e.loading=!1,t.finish(3);case 6:case"end":return t.stop()}}),t,null,[[0,,3,6]])})))()},addTagQDChuyenGiao:function(e){var t=this;return Object(r["a"])(regeneratorRuntime.mark((function a(){var n,o;return regeneratorRuntime.wrap((function(a){while(1)switch(a.prev=a.next){case 0:return n=e.split(", "),o={name:n.pop(),type:"QUYETDINHCHUYENGIAO"},a.next=4,t.$store.dispatch("commonItemStore/create",o).then((function(e){"SUCCESS"===e.resultCode&&(t.optionsQuyetDinhCG=[e.data].concat(Object(i["a"])(t.optionsQuyetDinhCG)),t.model.quyetDinhCQ=e.data),t.$store.dispatch("snackBarStore/addNotify",h["a"].addMessage(e))}));case 4:case"end":return a.stop()}}),a)})))()},getDonViTiepNhan:function(){var e=this;return Object(r["a"])(regeneratorRuntime.mark((function t(){return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:return t.prev=0,t.next=3,e.$store.dispatch("commonItemStore/getByType","DONVITIEPNHAN").then((function(t){if("SUCCESS"==t.resultCode){var a=t.data;e.loading=!1,e.optionsDonViTiepNhan=a}return[]}));case 3:return t.prev=3,e.loading=!1,t.finish(3);case 6:case"end":return t.stop()}}),t,null,[[0,,3,6]])})))()},addTagDonViTiepNhan:function(e){var t=this;return Object(r["a"])(regeneratorRuntime.mark((function a(){var n,o;return regeneratorRuntime.wrap((function(a){while(1)switch(a.prev=a.next){case 0:return n=e.split(", "),o={name:n.pop(),type:"DONVITIEPNHAN"},a.next=4,t.$store.dispatch("commonItemStore/create",o).then((function(e){"SUCCESS"===e.resultCode&&(t.optionsDonViTiepNhan=[e.data].concat(Object(i["a"])(t.optionsDonViTiepNhan)),t.model.donViTiepNhan=e.data),t.$store.dispatch("snackBarStore/addNotify",h["a"].addMessage(e))}));case 4:case"end":return a.stop()}}),a)})))()},handleCreate:function(){this.model=m["a"].baseJson()},handleSubmit:function(e){var t=this;return Object(r["a"])(regeneratorRuntime.mark((function a(){var n;return regeneratorRuntime.wrap((function(a){while(1)switch(a.prev=a.next){case 0:if(e.preventDefault(),t.submitted=!0,console.log(t.model),n=t.$loading.show({container:t.$refs.formContainer}),0==t.model.id||null==t.model.id||!t.model.id){a.next=9;break}return a.next=7,t.$store.dispatch("quanLyKhoaHocStore/update",t.model).then((function(e){"SUCCESS"===e.resultCode&&(t.showModal=!1,t.model=g["a"].baseJson()),t.$store.dispatch("snackBarStore/addNotify",h["a"].addMessage(e))}));case 7:a.next=11;break;case 9:return a.next=11,t.$store.dispatch("quanLyKhoaHocStore/create",t.model).then((function(e){"SUCCESS"===e.resultCode&&(t.showModal=!1,t.model=g["a"].baseJson()),t.$store.dispatch("snackBarStore/addNotify",h["a"].addMessage(e))}));case 11:n.hide();case 12:case"end":return a.stop()}}),a)})))()},handleDetail:function(){var e=this;return Object(r["a"])(regeneratorRuntime.mark((function t(){var a;return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:return a=e.$loading.show({container:e.$refs.formContainer}),t.next=3,e.$store.dispatch("quanLyKhoaHocStore/getById",e.$route.params.id).then((function(t){"SUCCESS"==t.resultCode&&(e.model=t.data,a.hide())}));case 3:case"end":return t.stop()}}),t)})))()}}},D=b,T=(a("1ab5"),a("aa1a"),a("0c7c")),N=Object(T["a"])(D,n,o,!1,null,null,null);t["default"]=N.exports}}]);
//# sourceMappingURL=chunk-4cf6aa25.e3468959.js.map