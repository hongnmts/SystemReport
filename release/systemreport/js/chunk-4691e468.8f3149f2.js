(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-4691e468"],{2579:function(t,e,n){"use strict";var i=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{staticClass:"row align-items-center"},[n("div",{staticClass:"col-sm-12"},[n("div",{staticClass:"page-title-box",staticStyle:{display:"flex","justify-content":"space-between","padding-top":"10px","padding-bottom":"10px"}},[n("h4",{staticClass:"font-size-18"},[t._v(t._s(t.title))]),n("div",{staticClass:"page-title-right"},[n("b-breadcrumb",{staticClass:"m-0",attrs:{items:t.items}})],1)])])])},s=[],r={components:{},props:{title:{type:String,default:""},items:{type:Array,default:function(){return[]}}}},a=r,o=n("0c7c"),c=Object(o["a"])(a,i,s,!1,null,null,null);e["a"]=c.exports},"3de8":function(t,e,n){"use strict";n.r(e);var i=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("Layout",[n("PageHeader",{attrs:{title:t.title,items:t.items}}),n("div",{staticClass:"row"},[n("div",{staticClass:"col-lg-10"}),n("div",{staticClass:"col-lg-2"},[n("b-button",{staticClass:"w-md",staticStyle:{float:"right","margin-bottom":"10px"},attrs:{type:"button",variant:"primary",size:"sm"},on:{click:t.handleSubmitRole}},[n("i",{staticClass:"mdi mdi-plus me-1 label-icon"}),t._v(" Lưu ")])],1)]),n("div",{staticClass:"row"},[n("div",{staticClass:"col-4"},[n("div",{staticClass:"card"},[n("div",{staticClass:"card-body"},[n("div",{staticClass:"font-weight-bold text-success",staticStyle:{"margin-bottom":"5px"}},[t.model?[t._v(" "+t._s(t.model.ten)+" ")]:t._e()],2),n("v-jstree",{attrs:{data:t.treeView,"text-field-name":"name"},on:{"item-click":t.itemClick}})],1)])]),n("div",{staticClass:"col-8"},[n("div",{staticClass:"card"},[n("div",{staticClass:"card-body"},[n("h4",{staticClass:"text-success font-weight-bold font-size-18 mb-2"},[t._v(t._s(this.node.name))]),n("b-table",{staticClass:"datatables",attrs:{items:this.node.permissions,fields:t.fields,striped:"",bordered:"",responsive:"sm"},scopedSlots:t._u([{key:"cell(STT)",fn:function(e){return[t._v(" "+t._s(e.index+1)+" ")]}},{key:"cell(process)",fn:function(e){return[n("div",{staticClass:"form-check form-check-success",staticStyle:{display:"flex","justify-content":"center"}},[n("input",{staticClass:"form-check-input",attrs:{type:"checkbox",id:"formCheckcolor1"},domProps:{checked:t.statusPermission(e.item)},on:{click:function(n){return t.onChecked(e.item)}}})])]}}])})],1)])])])],1)},s=[],r=n("1da1"),a=(n("96cf"),n("a4d3"),n("e01a"),n("d3b7"),n("159b"),n("c740"),n("b0c0"),n("4de4"),n("6a34")),o=n("2579"),c=n("c2a4"),l=n("47c6"),d={page:{title:"Thêm quyền vào vai trò",meta:[{name:"description",content:c.description}]},components:{Layout:a["a"],PageHeader:o["a"]},data:function(){return{node:[],model:null,title:"Thiết lập quyền",items:[{text:"Vai trò",active:!0},{text:"Thêm quyền",active:!0}],showModal:!1,showDeleteModal:!1,submitted:!1,treeView:[],itemEvents:{mouseover:function(){},contextmenu:function(){arguments[2].preventDefault()}},fields:[{key:"STT",label:"STT",class:"text-center",thStyle:{width:"30px",minWidth:"30px"},thClass:"hidden-sortable"},{key:"name",label:"Tên",class:"text-center",thStyle:{width:"80px",minWidth:"80px"},thClass:"hidden-sortable"},{key:"action",label:"Hành động",class:"text-center",thStyle:{width:"80px",minWidth:"80px"},thClass:"hidden-sortable"},{key:"process",label:"Xử lý",class:"text-center",thStyle:{width:"10px",minWidth:"10px"},thClass:"hidden-sortable"}]}},created:function(){this.GetTreeList(),this.GetPermissionsByRoleId()},methods:{GetTreeList:function(){var t=this;return Object(r["a"])(regeneratorRuntime.mark((function e(){return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:return e.next=2,t.$store.dispatch("moduleStore/getAll").then((function(e){"SUCCESS"===e.resultCode&&e.data.forEach((function(e){return t.treeView.push(e)}))}));case 2:case"end":return e.stop()}}),e)})))()},itemClick:function(t){var e=this;return Object(r["a"])(regeneratorRuntime.mark((function n(){return regeneratorRuntime.wrap((function(n){while(1)switch(n.prev=n.next){case 0:e.node=t.model;case 1:case"end":return n.stop()}}),n)})))()},GetPermissionsByRoleId:function(){var t=this;return Object(r["a"])(regeneratorRuntime.mark((function e(){return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:return e.next=2,t.$store.dispatch("roleStore/getById",t.$route.params.id).then((function(e){"SUCCESS"===e.resultCode?t.model=e.data:t.$store.dispatch("snackBarStore/addNotify",l["a"].addMessage(e))}));case 2:case"end":return e.stop()}}),e)})))()},statusPermission:function(t){var e=this.model.permissions.findIndex((function(e){return e.id==t.id}));return void 0!=e&&-1!=e},onChecked:function(t){var e,n=this.model.permissions.findIndex((function(e){return e.id==t.id}));void 0==n||-1==n?this.model.permissions.push({id:t.id,name:t.name,action:t.action}):this.model.permissions=null===(e=this.model.permissions)||void 0===e?void 0:e.filter((function(e){if(e.id!=t.id)return e}))},handleSubmitRole:function(){var t=this;return Object(r["a"])(regeneratorRuntime.mark((function e(){return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:return e.next=2,t.$store.dispatch("roleStore/update",t.model).then((function(e){"SUCCESS"===e.resultCode&&t.$store.dispatch("snackBarStore/addNotify",l["a"].addMessage(e))}));case 2:case"end":return e.stop()}}),e)})))()}}},u=d,m=(n("9f1f"),n("0c7c")),p=Object(m["a"])(u,i,s,!1,null,null,null);e["default"]=p.exports},"47c6":function(t,e,n){"use strict";n.d(e,"a",(function(){return o}));n("d81d");var i=function(t){return{resultString:t.resultString,resultCode:t.resultCode}},s=function(t){return{id:t.id,title:t.title,content:t.content,read:t.read,recipientId:t.recipientId,recipient:t.recipient,senderId:t.senderId,url:t.url}},r=function(){return{id:null,title:null,content:null,read:null,recipientId:null,recipient:null,senderId:null,url:null}},a=function(t){if(t.lenght>0){var e=[];return t.map((function(t,n){e.push(s(t))})),null!==e&&void 0!==e?e:[]}return[]},o={addMessage:i,fromJson:s,toListModel:a,baseJson:r}},"9f1f":function(t,e,n){"use strict";n("b4a9")},b4a9:function(t,e,n){}}]);
//# sourceMappingURL=chunk-4691e468.8f3149f2.js.map