(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-757a14b9"],{2579:function(t,e,a){"use strict";var s=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("div",{staticClass:"row align-items-center"},[a("div",{staticClass:"col-sm-12"},[a("div",{staticClass:"page-title-box",staticStyle:{display:"flex","justify-content":"space-between","padding-top":"10px","padding-bottom":"10px"}},[a("h4",{staticClass:"font-size-18"},[t._v(t._s(t.title))]),a("div",{staticClass:"page-title-right"},[a("b-breadcrumb",{staticClass:"m-0",attrs:{items:t.items}})],1)])])])},o=[],i={components:{},props:{title:{type:String,default:""},items:{type:Array,default:function(){return[]}}}},n=i,l=a("0c7c"),r=Object(l["a"])(n,s,o,!1,null,null,null);e["a"]=r.exports},"3d2d":function(t,e,a){"use strict";a.d(e,"a",(function(){return n}));var s=function(t){return{currentPage:t.currentPage,totalPages:t.totalPages,totalCount:t.totalCount,pageSize:t.pageSize,hasPrevious:t.hasPrevious,hasNext:t.hasNext}},o=function(t){return{currentPage:t.currentPage,totalPages:t.totalPages,totalCount:t.totalCount,pageSize:t.pageSize,hasPrevious:t.hasPrevious,hasNext:t.hasNext,maxVisibleButtons:t.totalPages<=5?t.totalPages:5}},i=function(){return{currentPage:1,totalPages:0,totalCount:0,pageSize:10,hasPrevious:!1,hasNext:!1,maxVisibleButtons:5}},n={toJson:s,fromJson:o,baseJson:i}},"47c6":function(t,e,a){"use strict";a.d(e,"a",(function(){return l}));a("d81d");var s=function(t){return{resultString:t.resultString,resultCode:t.resultCode}},o=function(t){return{id:t.id,title:t.title,content:t.content,read:t.read,recipientId:t.recipientId,recipient:t.recipient,senderId:t.senderId,url:t.url}},i=function(){return{id:null,title:null,content:null,read:null,recipientId:null,recipient:null,senderId:null,url:null}},n=function(t){if(t.lenght>0){var e=[];return t.map((function(t,a){e.push(o(t))})),null!==e&&void 0!==e?e:[]}return[]},l={addMessage:s,fromJson:o,toListModel:n,baseJson:i}},6662:function(t,e,a){"use strict";a.d(e,"a",(function(){return l}));a("d81d");var s=function(t){return{id:t.id,code:t.code,ten:t.ten,thuTu:t.thuTu,loaiKyBaoCao:t.loaiKyBaoCao,nam:t.nam,thang:t.thang}},o=function(t){return{id:t.id,code:t.code,ten:t.ten,thuTu:t.thuTu,nam:t.nam,thang:t.thang,loaiKyBaoCao:t.loaiKyBaoCao,createAt:t.createAt,modifiedAt:t.modifiedAt,createdBy:t.createdBy,modifiedBy:t.modifiedBy,lastModifiedShow:t.lastModifiedShow,createdAtShow:t.createdAtShow}},i=function(){return{id:null,code:null,ten:null,thuTu:0,loaiKyBaoCao:!1,nam:null,thang:null,createdAt:null,modifiedAt:null,createdBy:null,modifiedBy:null}},n=function(t){if(t.length>0){var e=[];return t.map((function(t,a){e.push(o(t))})),null!==e&&void 0!==e?e:[]}return[]},l={toJson:s,fromJson:o,baseJson:i,toListModel:n}},"9fe8":function(t,e,a){"use strict";a("d9bd")},ad99:function(t,e,a){"use strict";a.r(e);var s=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("Layout",[a("PageHeader",{attrs:{title:t.title,items:t.items}}),a("div",{staticClass:"row"},[a("div",{staticClass:"col-12"},[a("div",{staticClass:"card"},[a("div",{staticClass:"card-body"},[a("div",{staticClass:"row mb-2"},[a("div",{staticClass:"col-sm-4"},[a("div",{staticClass:"search-box me-2 mb-2 d-inline-block"},[a("div",{staticClass:"position-relative"},[a("input",{directives:[{name:"model",rawName:"v-model",value:t.filter,expression:"filter"}],staticClass:"form-control",attrs:{type:"text",placeholder:"Tìm kiếm ..."},domProps:{value:t.filter},on:{input:function(e){e.target.composing||(t.filter=e.target.value)}}}),a("i",{staticClass:"bx bx-search-alt search-icon"})])])]),a("div",{staticClass:"col-sm-8"},[a("div",{staticClass:"text-sm-end"},[a("b-button",{staticClass:"btn w-md btn-primary",attrs:{variant:"primary",type:"button",size:"sm"},on:{click:function(e){t.showModal=!0}}},[a("i",{staticClass:"mdi mdi-plus me-1"}),t._v(" Tạo Kỳ báo cáo ")]),a("b-modal",{attrs:{title:"Thông tin Kỳ báo cáo","title-class":"text-black font-18","body-class":"p-3","hide-footer":"",centered:"","no-close-on-backdrop":"",size:"lg"},model:{value:t.showModal,callback:function(e){t.showModal=e},expression:"showModal"}},[a("form",{ref:"formContainer",on:{submit:function(e){return e.preventDefault(),t.handleSubmit.apply(null,arguments)}}},[a("div",{staticClass:"row"},[a("div",{staticClass:"col-12"},[a("div",{staticClass:"mb-3"},[a("label",{staticClass:"text-left"},[t._v("Mã kỳ báo cáo")]),a("span",{staticStyle:{color:"red"}},[t._v(" *")]),a("input",{directives:[{name:"model",rawName:"v-model.trim",value:t.model.code,expression:"model.code",modifiers:{trim:!0}}],staticClass:"form-control",class:{"is-invalid":t.submitted&&t.$v.model.code.$error},attrs:{id:"ten",type:"text",placeholder:"Nhập mã kỳ báo cáo"},domProps:{value:t.model.code},on:{input:function(e){e.target.composing||t.$set(t.model,"code",e.target.value.trim())},blur:function(e){return t.$forceUpdate()}}}),t.submitted&&!t.$v.model.code.required?a("div",{staticClass:"invalid-feedback"},[t._v(" Tên mã báo cáo không được để trống. ")]):t._e()])]),a("div",{staticClass:"col-12"},[a("div",{staticClass:"mb-3"},[a("label",{staticClass:"text-left"},[t._v("Tên Kỳ báo cáo")]),a("span",{staticStyle:{color:"red"}},[t._v(" *")]),a("input",{directives:[{name:"model",rawName:"v-model",value:t.model.id,expression:"model.id"}],attrs:{type:"hidden"},domProps:{value:t.model.id},on:{input:function(e){e.target.composing||t.$set(t.model,"id",e.target.value)}}}),a("input",{directives:[{name:"model",rawName:"v-model.trim",value:t.model.ten,expression:"model.ten",modifiers:{trim:!0}}],staticClass:"form-control",class:{"is-invalid":t.submitted&&t.$v.model.ten.$error},attrs:{id:"ten",type:"text",placeholder:"Nhập tên Kỳ báo cáo"},domProps:{value:t.model.ten},on:{input:function(e){e.target.composing||t.$set(t.model,"ten",e.target.value.trim())},blur:function(e){return t.$forceUpdate()}}}),t.submitted&&!t.$v.model.ten.required?a("div",{staticClass:"invalid-feedback"},[t._v(" Tên Kỳ báo cáo không được để trống. ")]):t._e()])]),a("div",{staticClass:"col-6"},[a("div",{staticClass:"mb-3"},[a("label",{staticClass:"text-left"},[t._v("Loại ký báo cáo")]),a("br"),a("switches",{staticClass:"mt-2",attrs:{"type-bold":"false",color:"info"},model:{value:t.model.loaiKyBaoCao,callback:function(e){t.$set(t.model,"loaiKyBaoCao",e)},expression:"model.loaiKyBaoCao"}})],1)]),a("div",{staticClass:"col-6"},[a("div",{staticClass:"mb-3"},[a("label",{staticClass:"text-left"},[t._v("Nhập số thứ tự")]),a("span",{staticStyle:{color:"red"}},[t._v(" *")]),a("input",{directives:[{name:"model",rawName:"v-model",value:t.model.thuTu,expression:"model.thuTu"}],staticClass:"form-control",class:{"is-invalid":t.submitted&&t.$v.model.thuTu.$error},attrs:{id:"thuTu",type:"number",min:"0",oninput:"validity.valid||(value='');",placeholder:"Nhập số thứ tự"},domProps:{value:t.model.thuTu},on:{input:function(e){e.target.composing||t.$set(t.model,"thuTu",e.target.value)}}}),t.submitted&&!t.$v.model.thuTu.required?a("div",{staticClass:"invalid-feedback"},[t._v(" Thứ tự không được để trống. ")]):t._e()])])]),a("div",{staticClass:"text-end pt-2 mt-3"},[a("b-button",{staticClass:"w-md",attrs:{variant:"light",size:"sm"},on:{click:function(e){t.showModal=!1}}},[t._v(" Đóng ")]),a("b-button",{staticClass:"ms-1 w-md",attrs:{type:"submit",variant:"primary",size:"sm"}},[t._v(" Lưu ")])],1)])]),a("b-modal",{attrs:{title:"Thông tin chi tiết Kỳ báo cáo","title-class":"text-black font-18","body-class":"p-3","hide-footer":"",centered:"","no-close-on-backdrop":"",size:"lg"},model:{value:t.showDetail,callback:function(e){t.showDetail=e},expression:"showDetail"}},[a("form",{ref:"formContainer",on:{submit:function(e){return e.preventDefault(),t.handleSubmit.apply(null,arguments)}}},[a("div",{staticClass:"row"},[a("div",{staticClass:"col-12"},[a("div",{staticClass:"mb-3"},[a("label",{staticClass:"text-left"},[t._v("Tên kỳ báo cáo : ")]),a("input",{directives:[{name:"model",rawName:"v-model",value:t.model.ten,expression:"model.ten"}],staticClass:"form-control",attrs:{type:"text"},domProps:{value:t.model.ten},on:{input:function(e){e.target.composing||t.$set(t.model,"ten",e.target.value)}}})])]),a("div",{staticClass:"col-12"},[a("div",{staticClass:"mb-3"},[a("label",{staticClass:"text-left"},[t._v("Thứ tự : ")]),a("input",{directives:[{name:"model",rawName:"v-model",value:t.model.thuTu,expression:"model.thuTu"}],staticClass:"form-control",attrs:{type:"number",min:"0",oninput:"validity.valid||(value='');"},domProps:{value:t.model.thuTu},on:{input:function(e){e.target.composing||t.$set(t.model,"thuTu",e.target.value)}}})])]),a("div",{staticClass:"col-12"},[a("div",{staticClass:"mb-3"},[a("label",{staticClass:"text-left"},[t._v("Người tạo : ")]),a("input",{directives:[{name:"model",rawName:"v-model",value:t.model.createdBy,expression:"model.createdBy"}],staticClass:"form-control",attrs:{type:"text"},domProps:{value:t.model.createdBy},on:{input:function(e){e.target.composing||t.$set(t.model,"createdBy",e.target.value)}}})])]),a("div",{staticClass:"col-12"},[a("div",{staticClass:"mb-3"},[a("label",{staticClass:"text-left"},[t._v("Ngày tạo: ")]),a("input",{directives:[{name:"model",rawName:"v-model",value:t.model.createdAtShow,expression:"model.createdAtShow"}],staticClass:"form-control",attrs:{type:"text"},domProps:{value:t.model.createdAtShow},on:{input:function(e){e.target.composing||t.$set(t.model,"createdAtShow",e.target.value)}}})])]),a("div",{staticClass:"col-12"},[a("div",{staticClass:"mb-3"},[a("label",{staticClass:"text-left"},[t._v("Người cập nhật : ")]),a("input",{directives:[{name:"model",rawName:"v-model",value:t.model.modifiedBy,expression:"model.modifiedBy"}],staticClass:"form-control",attrs:{type:"text"},domProps:{value:t.model.modifiedBy},on:{input:function(e){e.target.composing||t.$set(t.model,"modifiedBy",e.target.value)}}})])]),a("div",{staticClass:"col-12"},[a("div",{staticClass:"mb-3"},[a("label",{staticClass:"text-left"},[t._v("Ngày cập nhật : ")]),a("input",{directives:[{name:"model",rawName:"v-model",value:t.model.lastModifiedShow,expression:"model.lastModifiedShow"}],staticClass:"form-control",attrs:{type:"text"},domProps:{value:t.model.lastModifiedShow},on:{input:function(e){e.target.composing||t.$set(t.model,"lastModifiedShow",e.target.value)}}})])])]),a("div",{staticClass:"text-end pt-2 mt-3"},[a("b-button",{attrs:{variant:"light"},on:{click:function(e){t.showDetail=!1}}},[t._v(" Đóng ")])],1)])])],1)])]),a("div",{staticClass:"row"},[a("div",{staticClass:"col-12"},[a("div",{staticClass:"row mb-3"},[a("div",{staticClass:"col-sm-12 col-md-6"},[a("div",{staticClass:"dataTables_length",attrs:{id:"tickets-table_length"}},[a("label",{staticClass:"d-inline-flex align-items-center"},[t._v(" Hiện "),a("b-form-select",{staticClass:"form-select form-select-sm",attrs:{size:"sm",options:t.pageOptions},model:{value:t.perPage,callback:function(e){t.perPage=e},expression:"perPage"}}),t._v(" dòng ")],1)])])]),a("div",{staticClass:"table-responsive-sm"},[a("b-table",{ref:"tblList",staticClass:"datatables",attrs:{items:t.myProvider,fields:t.fields,striped:"",bordered:"",responsive:"sm","per-page":t.perPage,"current-page":t.currentPage,"sort-by":t.sortBy,"sort-desc":t.sortDesc,filter:t.filter,"filter-included-fields":t.filterOn,"primary-key":"id",busy:t.isBusy},on:{"update:sortBy":function(e){t.sortBy=e},"update:sort-by":function(e){t.sortBy=e},"update:sortDesc":function(e){t.sortDesc=e},"update:sort-desc":function(e){t.sortDesc=e},"update:busy":function(e){t.isBusy=e}},scopedSlots:t._u([{key:"cell(STT)",fn:function(e){return[t._v(" "+t._s(e.index+(t.currentPage-1)*t.perPage+1)+" ")]}},{key:"cell(loaiKyBaoCao)",fn:function(e){return["nam"==e.item.loaiKyBaoCao?a("div",[t._v(" Năm ")]):"thang"==e.item.loaiKyBaoCao?a("div",[t._v("Tháng ")]):a("div",[t._v(" Giai đoạn ")])]}},{key:"cell(process)",fn:function(e){return[a("button",{staticClass:"btn btn-outline btn-sm",attrs:{type:"button",size:"sm","data-toggle":"tooltip","data-placement":"bottom",title:"Chi tiết"},on:{click:function(a){return t.handleDetail(e.item.id)}}},[a("i",{staticClass:"fas fa-eye  text-warning me-1"})]),a("button",{staticClass:"btn btn-outline btn-sm",attrs:{type:"button",size:"sm","data-toggle":"tooltip","data-placement":"bottom",title:"Cập nhật"},on:{click:function(a){return t.handleUpdate(e.item.id)}}},[a("i",{staticClass:"fas fa-pencil-alt text-success me-1"})]),a("button",{staticClass:"btn btn-outline btn-sm",attrs:{type:"button",size:"sm","data-toggle":"tooltip","data-placement":"bottom",title:"Xóa"},on:{click:function(a){return t.handleShowDeleteModal(e.item.id)}}},[a("i",{staticClass:"fas fa-trash-alt text-danger me-1"})])]}},{key:"cell(ten)",fn:function(e){return[t._v("   "+t._s(e.item.ten)+" ")]}}])}),t.isBusy?[a("div",{attrs:{align:"center"}},[t._v("Đang tải dữ liệu")])]:t._e(),t.totalRows<=0&&!t.isBusy?[a("div",{attrs:{align:"center"}},[t._v("Không có dữ liệu")])]:t._e()],2),a("div",{staticClass:"row"},[a("b-col",[a("div",[t._v("Hiển thị "+t._s(t.numberOfElement)+" trên tổng số "+t._s(t.totalRows)+" dòng")])]),a("div",{staticClass:"col"},[a("div",{staticClass:"dataTables_paginate paging_simple_numbers float-end"},[a("ul",{staticClass:"pagination pagination-rounded mb-0"},[a("b-pagination",{attrs:{"total-rows":t.totalRows,"per-page":t.perPage},model:{value:t.currentPage,callback:function(e){t.currentPage=e},expression:"currentPage"}})],1)])])],1)])])])]),a("b-modal",{attrs:{centered:"",title:"Xóa dữ liệu","title-class":"font-18","no-close-on-backdrop":""},scopedSlots:t._u([{key:"modal-footer",fn:function(){return[a("b-button",{directives:[{name:"b-modal",rawName:"v-b-modal.modal-close_visit",modifiers:{"modal-close_visit":!0}}],staticClass:"btn btn-outline-info w-md",attrs:{size:"sm"},on:{click:function(e){t.showDeleteModal=!1}}},[t._v(" Đóng ")]),a("b-button",{directives:[{name:"b-modal",rawName:"v-b-modal.modal-close_visit",modifiers:{"modal-close_visit":!0}}],staticClass:"w-md",attrs:{size:"sm",variant:"danger",type:"button"},on:{click:t.handleDelete}},[t._v(" Xóa ")])]},proxy:!0}]),model:{value:t.showDeleteModal,callback:function(e){t.showDeleteModal=e},expression:"showDeleteModal"}},[a("p",[t._v(" Dữ liệu xóa sẽ không được phục hồi! ")])])],1)])],1)},o=[],i=a("1da1"),n=(a("96cf"),a("a4d3"),a("e01a"),a("4de4"),a("d3b7"),a("6a34")),l=a("2579"),r=a("b5ae"),d=a("c2a4"),c=a("47c6"),u=a("3d2d"),m=a("5b55"),p=a("6662"),f=a("5c47"),v={page:{title:"Ký báo cáo",meta:[{name:"description",content:d.description}]},components:{Layout:n["a"],PageHeader:l["a"],Switches:f["a"]},data:function(){return{title:"Kỳ báo cáo",items:[{text:"Kỳ báo cáo",href:"/ky-bao-cao"},{text:"Danh sách",active:!0}],data:[],showModal:!1,showDetail:!1,showDeleteModal:!1,submitted:!1,model:p["a"].baseJson(),listCoQuan:[],listRole:[],pagination:u["a"].baseJson(),totalRows:1,todoTotalRows:1,currentPage:1,numberOfElement:1,perPage:10,pageOptions:[5,10,25,50,100],filter:null,todoFilter:null,filterOn:[],todofilterOn:[],isBusy:!1,sortBy:"age",sortDesc:!1,fields:[{key:"STT",label:"STT",class:"td-stt",sortable:!1,thClass:"hidden-sortable"},{key:"code",label:"Mã kỳ báo cáo",sortable:!0,class:"td-xuly"},{key:"ten",label:"Tên kỳ báo cáo",sortable:!0,thStyle:"text-align:left"},{key:"loaiKyBaoCao",label:"Loại báo cáo",class:"td-xuly",sortable:!0},{key:"thuTu",label:"Thứ tự",class:"td-xuly",sortable:!0},{key:"process",label:"Xử lý",class:"td-xuly",thClass:"hidden-sortable",sortable:!1}]}},validations:{model:{ten:{required:r["required"]},code:{required:r["required"]},thuTu:{required:r["required"]}}},created:function(){this.fnGetList()},watch:{model:{deep:!0,handler:function(t){}},showModal:function(t){0==t&&(this.model=p["a"].baseJson())},showDeleteModal:function(t){0==t&&(this.model.id=null)}},methods:{fnGetList:function(){var t=this;return Object(i["a"])(regeneratorRuntime.mark((function e(){return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:return e.next=2,t.onPageChange();case 2:case"end":return e.stop()}}),e)})))()},onPageChange:function(){var t=arguments,e=this;return Object(i["a"])(regeneratorRuntime.mark((function a(){var s,o;return regeneratorRuntime.wrap((function(a){while(1)switch(a.prev=a.next){case 0:o=t.length>0&&void 0!==t[0]?t[0]:1,e.pagination.currentPage=o,{pageNumber:e.pagination.currentPage,pageSize:e.pagination.pageSize},null===(s=e.$refs.tblList)||void 0===s||s.refresh();case 4:case"end":return a.stop()}}),a)})))()},handleUpdate:function(t){var e=this;return Object(i["a"])(regeneratorRuntime.mark((function a(){return regeneratorRuntime.wrap((function(a){while(1)switch(a.prev=a.next){case 0:return a.next=2,e.$store.dispatch("kyBaoCaoStore/getById",t).then((function(t){"SUCCESS"===t.resultCode?(e.model=p["a"].fromJson(t.data),e.showModal=!0):(e.$store.dispatch("snackBarStore/addNotify",c["a"].addMessage(t)),e.$refs.tblList.refresh())}));case 2:case"end":return a.stop()}}),a)})))()},handleDetail:function(t){var e=this;return Object(i["a"])(regeneratorRuntime.mark((function a(){return regeneratorRuntime.wrap((function(a){while(1)switch(a.prev=a.next){case 0:return a.next=2,e.$store.dispatch("kyBaoCaoStore/getById",t).then((function(t){"SUCCESS"===t.resultCode?(e.model=p["a"].fromJson(t.data),e.showDetail=!0):e.$store.dispatch("snackBarStore/addNotify",c["a"].addMessage(t))}));case 2:case"end":return a.stop()}}),a)})))()},handleDelete:function(){var t=this;return Object(i["a"])(regeneratorRuntime.mark((function e(){return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:if(0==t.model.id||null==t.model.id||!t.model.id){e.next=3;break}return e.next=3,t.$store.dispatch("kyBaoCaoStore/delete",t.model.id).then((function(e){"SUCCESS"===e.resultCode&&(t.fnGetList(),t.showDeleteModal=!1,t.$refs.tblList.refresh()),t.$store.dispatch("snackBarStore/addNotify",c["a"].addMessage(e))}));case 3:case"end":return e.stop()}}),e)})))()},handleShowDeleteModal:function(t){this.model.id=t,this.showDeleteModal=!0},handleSubmit:function(t){var e=this;return Object(i["a"])(regeneratorRuntime.mark((function a(){var s;return regeneratorRuntime.wrap((function(a){while(1)switch(a.prev=a.next){case 0:if(t.preventDefault(),e.submitted=!0,e.$v.$touch(),!e.$v.$invalid){a.next=7;break}return a.abrupt("return");case 7:if(s=e.$loading.show({container:e.$refs.formContainer}),0==e.model.id||null==e.model.id||!e.model.id){a.next=13;break}return a.next=11,e.$store.dispatch("kyBaoCaoStore/update",e.model).then((function(t){"SUCCESS"===t.resultCode&&(e.fnGetList(),e.showModal=!1,e.$refs.tblList.refresh()),e.$store.dispatch("snackBarStore/addNotify",c["a"].addMessage(t))}));case 11:a.next=15;break;case 13:return a.next=15,e.$store.dispatch("kyBaoCaoStore/create",p["a"].toJson(e.model)).then((function(t){"SUCCESS"===t.resultCode&&(e.fnGetList(),e.showModal=!1,e.$refs.tblList.refresh(),e.model={}),e.$store.dispatch("snackBarStore/addNotify",c["a"].addMessage(t))}));case 15:s.hide();case 16:e.submitted=!1;case 17:case"end":return a.stop()}}),a)})))()},myProvider:function(t){var e=this,a={start:t.currentPage,limit:t.perPage,content:this.filter,sortBy:t.sortBy,sortDesc:t.sortDesc};this.loading=!0;try{var s=this.$store.dispatch("kyBaoCaoStore/getPagingParams",a);return s.then((function(t){if(t.resultCode==m["a"].SUCCESS){var a=t.data;e.totalRows=a.totalRows;var s=a.data;return e.numberOfElement=s.length,e.loading=!1,s||[]}return[]}))}finally{this.loading=!1}}}},b=v,h=(a("9fe8"),a("0c7c")),g=Object(h["a"])(b,s,o,!1,null,null,null);e["default"]=g.exports},d9bd:function(t,e,a){}}]);
//# sourceMappingURL=chunk-757a14b9.ad1df2ca.js.map