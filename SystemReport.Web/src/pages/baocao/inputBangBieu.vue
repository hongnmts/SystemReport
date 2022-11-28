<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body" style="display: flex; justify-content: end">
              <b-button
                variant="info"
                type="button"
                class="btn w-md btn-primary"
                @click="createSubRowValue(null, false)"
                size="sm"
                style="margin-right: 10px"
            >
              <i class="mdi mdi-plus me-1"></i> Thêm dòng mới
            </b-button>
            <b-button
                variant="primary"
                type="button"
                class="btn w-md btn-primary"
                @click="handleSubmitBangBieu"
                size="sm"
                style="margin-right: 10px"
            >
              <i class="mdi mdi-content-save-all me-1"></i> Lưu thông tin
            </b-button>
            <b-button
                variant="warning"
                type="button"
                class="btn w-md btn-primary"
                @click="addRowTong"
                size="sm"
                style="margin-right: 10px"
            >
              <i class="mdi mdi-chart-box-plus-outline me-1"></i> Thêm tổng
            </b-button>

            <b-button
                variant="secondary"
                type="button"
                class="btn btn-primary"
                @click="handleGoBack"
                size="sm"
            >
              <i class="mdi  mdi-keyboard-backspace me-1"></i>

              Quay lại
            </b-button>
          </div>
        </div>
      </div>
      <div class="col-12">
        <div class="card">
          <div class="card-body" style="overflow-x: auto">
            <table id="dynamic-table" class="dynamic-table" v-if="model">
              <thead v-if="model.headers">
              <tr v-for="(row, indexRow) in model.headers" :key="indexRow">
                <td v-for="(data, indexData) in row.tHeaderVms" :key="indexData" :rowspan="data.rowSpan"
                    :colspan="data.colSpan"
                    :style="{ 'width': data.width == 0?'auto':  data.width + 'px',
                     'text-align': data.fontHorizontalAlign?data.fontHorizontalAlign.id: '',
                     'font-weight': data.fontWeight?data.fontWeight.id: '',
                     'font-style': data.fontStyle?data.fontStyle.id: '',
                     }"
                >
                  {{ data.tenThuocTinh }}
                  <template v-if="data.ghiChu">
                               <span v-if="checkIsEmpty(data.ghiChu.kyHieu)">
                                 <template v-if="checkIsStar(data.ghiChu.kyHieu)">
                                        ({{ data.ghiChu.kyHieu }})
                                 </template>
                                       <template v-else>
                                       <sup>{{ data.ghiChu.kyHieu }}</sup>
                                 </template>

                  </span>
                  </template>

                  <template v-if="data.donViTinh">

                    <div style="font-weight: normal; font-style: italic">
                      ({{ data.donViTinh.ten }})
                    </div>
                  </template>
                </td>
                <td></td>
              </tr>
              </thead>
              <tbody>
              <tr v-for="(value, index) in model.body" :key="index" style="min-height: 50px; height: 50px">
                <td v-for="(item, index1) in value.rowValues" :key="index1"
                    style="padding: 0px 5px"
                    :style="{ 'width': item.width == 0?'auto':  item.width + 'px !important',
                     'text-align': item.fontHorizontalAlign?item.fontHorizontalAlign.id: '',
                     'font-weight': item.fontWeight?item.fontWeight.id: '',
                     'font-style': item.fontStyle?item.fontStyle.id: '',
                     }"
                >
                  <template v-if="item.styleInput">
                    <textarea :id="item.code" :ref="item.code" class="form-control" v-if="item.styleInput && item.styleInput.id == 'content'"
                              v-model="item.value"
                              :style="{
                     'text-align': item.fontHorizontalAlign?item.fontHorizontalAlign.id: '',
                     'font-weight': item.fontWeight?item.fontWeight.id: '',
                     'font-style': item.fontStyle?item.fontStyle.id: '',
                     }"
                              style=" margin: 0px"
                    ></textarea>
                    <textarea :id="item.code" :ref="item.code" class="form-control" v-else-if="item.styleInput && item.styleInput.id == 'text'"
                              v-model="item.value"
                              :style="{
                     'text-align': item.fontHorizontalAlign?item.fontHorizontalAlign.id: '',
                     'font-weight': item.fontWeight?item.fontWeight.id: '',
                     'font-style': item.fontStyle?item.fontStyle.id: '',
                     }" style="width: 100%; height: 100%;  margin: 0px"> </textarea>

                    <input :id="item.code" :ref="item.code" class="form-control" type="text"
                           pattern="(^[0-9]{0,2}$)|(^[0-9]{0,2}\.[0-9]{0,5}$)"
                           v-else-if="item.styleInput && item.styleInput.id == 'int'" v-model="item.value"
                           :style="{
                     'text-align': item.fontHorizontalAlign?item.fontHorizontalAlign.id: '',
                     'font-weight': item.fontWeight?item.fontWeight.id: '',
                     'font-style': item.fontStyle?item.fontStyle.id: '',
                     }" style="width: 100%; height: 100%;  margin: 0px"/>
                    <input :id="item.code" :ref="item.code" class="form-control" type="number" inputmode="decimal"
                           v-else-if="item.styleInput && item.styleInput.id == 'float'" v-model="item.value"
                           :style="{
                     'text-align': item.fontHorizontalAlign?item.fontHorizontalAlign.id: '',
                     'font-weight': item.fontWeight?item.fontWeight.id: '',
                     'font-style': item.fontStyle?item.fontStyle.id: '',
                     }" style="width: 100%; height: 100%; margin: 0px; text-align: center"/>
                    <input :id="item.code" :ref="item.code" class="form-control" type="number" readonly
                           inputmode="decimal" v-else-if="item.styleInput && item.styleInput.id == 'formula'"
                           v-model="item.value"
                           :style="{
                     'text-align': item.fontHorizontalAlign?item.fontHorizontalAlign.id: '',
                     'font-weight': item.fontWeight?item.fontWeight.id: '',
                     'font-style': item.fontStyle?item.fontStyle.id: '',
                     }" style="width: 100%; height: 100%; margin: 0px;  text-align: center"/>
                  </template>
                  <!--                  <input  :style="{ 'width': item.width == 0?'auto':  item.width + 'px !important',-->
                  <!--                     'text-align': item.fontHorizontalAlign?item.fontHorizontalAlign.id: '',-->
                  <!--                     'font-weight': item.fontWeight?item.fontWeight.id: '',-->
                  <!--                     'font-style': item.fontStyle?item.fontStyle.id: '',-->
                  <!--                     }" style="width: 100%; height: 100%" :value="item.value" />-->
                  <!--                  {{ item.value }}-->
                  <!--                  <template v-if="item.ghiChu">-->
                  <!--                               <span v-if="checkIsEmpty(item.ghiChu.kyHieu)">-->
                  <!--                           <sup>{{ item.ghiChu.kyHieu }}</sup>-->

                  <!--                  </span>-->
                  <!--                  </template>-->
                </td>
                <td style="text-align: center">
                  <!--                  <button-->
                  <!--                      type="button"-->
                  <!--                      size="sm"-->
                  <!--                      class="btn btn-outline btn-sm"-->
                  <!--                      data-toggle="tooltip" data-placement="bottom" title="Cập nhật"-->
                  <!--                      v-on:click="getRowValueByKeyRow(value.keyRow)">-->
                  <!--                    <i class="fas fa-pencil-alt text-success me-1"></i>-->
                  <!--                  </button>-->
                  <button
                      type="button"
                      size="sm"
                      class="btn btn-outline btn-sm"
                      data-toggle="tooltip" data-placement="bottom" title="Thêm chỉ tiêu con"
                      v-on:click="createSubRowValue(value, true)">
                    <i class="fas fa-plus text-info me-1"></i>
                  </button>
                  <button
                      type="button"
                      size="sm"
                      class="btn btn-outline btn-sm"
                      data-toggle="tooltip" data-placement="bottom" title="Xóa chỉ tiêu"
                      v-on:click="handleSubmitDeleteRowValue(value)">
                    <i class="fas fa-trash text-danger me-1"></i>
                  </button>
                </td>
              </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>

    <b-modal
        v-model="showModalInputRowValue"
        centered
        title="Thông tin chỉ tiêu"
        title-class="font-18"
        no-close-on-backdrop
        hide-footer
        size="xl"
    >
      <div class="row">

        <div class="col-12">
          <div class="row">
            <div class="col-12">
              <div class="text-start mb-2">
                <b-button variant="warning" class="w-md mx-1" size="sm"
                          @click="handleResetThuocTinhIsChiTieu">
                  Làm mới
                </b-button>
                <b-button @click="handleSubmitRowValue" type="submit" variant="primary" size="sm"
                          class="ms-1 w-md">
                  Lưu
                </b-button>
                <b-button v-if="checkValueChiTieu()" variant="danger" class="w-md mx-1" size="sm"
                          @click="handleSubmitDeleteRowValue">
                  Xóa
                </b-button>
              </div>
            </div>
            <div class="col-8">
              <div class="mb-3">
                <label class="text-left">Chỉ tiêu cha</label>
                <treeselect
                    :normalizer="normalizer"
                    v-model="configChiTieu.parentId"
                    :options="optionsRowValueFilter"
                    :searchable="false"
                    :show-count="true"
                    :default-expand-level="1"
                >
                  <label slot="option-label"
                         slot-scope="{ node, shouldShowCount, count, labelClassName, countClassName }"
                         :class="labelClassName">
                    {{ node.label }}
                    <span v-if="shouldShowCount" :class="countClassName">({{ count }})</span>
                  </label>
                </treeselect>
              </div>
            </div>
            <div class="col-4">
              <div class="mb-3">
                <label class="text-left">Nhập số thứ tự</label>
                <input
                    id="thuTu"
                    v-model="configChiTieu.thuTu"
                    type="number"
                    min="0"
                    oninput="validity.valid||(value='');"
                    class="form-control"
                    placeholder="Nhập số thứ tự"

                />
              </div>
            </div>

            <div class="col-12 card" v-for="(value, index) in chiTieus" :key="index">
              <div class="card-body">
                <div style="font-weight: bold; font-size: 18px">#{{ ++index }} - {{ value.tenThuocTinh }}
                </div>
                <form @submit.prevent="handleSubmitThuocTinh"
                      ref="formContainer">
                  <div class="row">
                    <div class="col-12">
                      <div class="mb-3">
                        <label class="text-left">Tên chỉ tiêu</label>
                        <input type="hidden" v-model="value.id"/>
                        <input
                            id="ten"
                            v-model.trim="value.value"
                            type="text"
                            class="form-control"
                            placeholder=""
                        />
                      </div>
                    </div>
                    <hr/>
                    <h6>Ghi chú</h6>
                    <div class="col-2">
                      <div class="mb-3">
                        <label class="text-left">Ký hiệu</label>
                        <input
                            v-model.trim="value.ghiChu.kyHieu"
                            type="text"
                            class="form-control"
                            placeholder=""
                        />
                      </div>
                    </div>
                    <div class="col-10">
                      <div class="mb-3">
                        <label class="text-left">Nội dung</label>
                        <textarea
                            v-model.trim="value.ghiChu.noiDung"
                            type="text"
                            class="form-control"
                            placeholder=""
                        />
                      </div>
                    </div>
                    <hr/>
                    <div class="col-4">
                      <div class="mb-3">
                        <label class="text-left">Kiểu chữ</label>
                        <multiselect v-model="value.fontStyle" :options="fontStyle" label="name" track="id"
                                     :searchable="false" :show-labels="false"
                                     placeholder="Chọn kiểu chữ"></multiselect>
                      </div>
                    </div>
                    <div class="col-4">
                      <div class="mb-3">
                        <label class="text-left">Phông chữ</label>
                        <multiselect v-model="value.fontWeight" :options="fontWeight" label="name" track="id"
                                     :searchable="false" :show-labels="false"
                                     placeholder="Chọn phông chữ"></multiselect>
                      </div>
                    </div>
                    <div class="col-4">
                      <div class="mb-3">
                        <label class="text-left">Canh chiều ngang</label>

                        <multiselect v-model="value.fontHorizontalAlign" :options="fontHorizontalAlign"
                                     label="name"
                        ></multiselect>
                      </div>
                    </div>
                  </div>

                </form>
              </div>

              <hr/>
            </div>
          </div>
        </div>
      </div>
    </b-modal>
  </Layout>
</template>

<script>
import appConfig from "@/app.config.json";
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import {notifyModel} from "@/models/notifyModel";
import {bangBieuModel} from "@/models/bangBieuModel";
import {thangModel} from "@/models/thangModel";
import {rowValueModel} from "@/models/rowValueModel";
import Treeselect from "@riophae/vue-treeselect";
import Multiselect from 'vue-multiselect'
import {styleModel} from "@/models/StyleModel";
import {formulaModel} from "@/models/FormulaModel";
import StringMathEvaluator from "@/helpers/StringMathEvaluator";

export default {
  page: {
    title: "Nhập liệu",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, Treeselect, Multiselect},
  data() {
    return {
      title: "Nhập liệu",
      items: [
        {
          text: "Nhập liệu",
          href: "/nhap-lieu",
          // active: true,
        },
        {
          text: "Bảng biểu",
          active: true,
        }
      ],
      modelBangBieu: bangBieuModel.baseJson(),
      listBangBieu: [],
      submitted: false,
      optionDonViTinh: [],
      optionLoaiMauBieu: [],
      optionDonVis: [],
      showModalXemBangBieu: false,
      showModalXemMauBieu: false,
      renderMainRowValue: [],
      headers: [],
      listTable: [],
      listTableMauBieu: [],
      optionCoQuans: [],
      showModalPhanMauBieu: false,
      optionKyBaoCao: [],
      thangModel: thangModel.calThang,
      modelInputMauBieu: {
        mauBieuId: null,
        thang: null,
        nam: null,
        kyBaoCao: null
      },
      strIdMauBieu: null,
      model: null,
      showModalInputRowValue: false,
      configChiTieu: {
        parentId: null,
        thuTu: 0
      },
      optionsRowValueFilter: [],
      chiTieus: [],
      thuocTinhIsChiTieu: [],
      fontStyle: styleModel.FontStyle,
      fontWeight: styleModel.FontWeight,
      fontHorizontalAlign: styleModel.FontHorizontalAlign,
      fontVerticalAlign: styleModel.FontVerticalAlign,
      formula: formulaModel.FormulaModel,
      styleInput: formulaModel.StyleInputModel,
      arrayValues: [],
      timeout: null,
    }
  },
  validations: {},
  created() {
    this.renderNhapLieuBangBieu();
    this.getTreeParentRowValue();
    this.getThuocTinhChiTieu();

  },
  watch: {
    model: {
      deep: true,
      handler(values) {

        if (this.timeout)
          clearTimeout(this.timeout);

        this.timeout = setTimeout(() => {
          if (this.model && this.model.body && this.model.body.length > 0) {
            this.model.body.map((value) => {
              if (value && value.rowValues && value.rowValues.length > 0) {
                value.rowValues.map((val) => {
                  if (val && val.styleInput && val.styleInput.id == "formula") {
                    this.formalaCal(val.stringCongThuc, val.code)
                  }
                })
              }
            })
          }
        }, 500);

        // if (this.model && this.model.body && this.model.body.length > 0) {
        //   this.model.body.map((value) => {
        //     if (value && value.rowValues && value.rowValues.length > 0) {
        //       value.rowValues.map((val) => {
        //         if (val && val.styleInput && val.styleInput.id == "formula") {
        //           this.formalaCal(val.stringCongThuc, val.code)
        //         }
        //       })
        //     }
        //   })
        // }
        // this.$store.dispatch("globalStore/setChiTieus", this.model);
      }
    },
    modelInputMauBieu: {
      deep: true,
      handler(val) {
        if (val.kyBaoCao == '' || val.kyBaoCao == null || val.kyBaoCao == undefined) {
          this.modelInputMauBieu.nam = null;
          this.modelInputMauBieu.thang = null;
        }
      }
    },
    thuocTinhIsChiTieu: {
      deep: true,
      handler(val) {
        this.chiTieus = [];
        val.map((value, index) => {
          var ct = rowValueModel.baseJson();
          ct.bangBieuId = value.bangBieuId;
          ct.thuocTinhId = value.id;
          ct.tenThuocTinh = value.ten;
          ct.styleInput = value.styleInput;
          ct.tinhTongChiTieuCon = value.tinhChiTieuCon

          this.chiTieus.push(ct);
        })
      }
    },
    configChiTieu: {
      deep: true,
      handler(val) {
        if (this.chiTieus && this.chiTieus.length > 0) {
          this.chiTieus.map(value => {
            value.thuTu = val.thuTu;
            value.parentId = val.parentId;
          })
        }
      }
    },
  },
  methods: {
    exportTableToExcel(tableId, filename) {
  let dataType = 'application/vnd.ms-excel';
  let extension = '.xls';

  let base64 = function(s) {
    return window.btoa(unescape(encodeURIComponent(s)))
  };

  let template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>';
  let render = function(template, content) {
    return template.replace(/{(\w+)}/g, function(m, p) { return content[p]; });
  };

  let tableElement = document.getElementById(tableId);

  let tableExcel = render(template, {
    worksheet: filename,
    table: tableElement.innerHTML
  });

  filename = filename + extension;

  if (navigator.msSaveOrOpenBlob)
  {
    let blob = new Blob(
        [ '\ufeff', tableExcel ],
        { type: dataType }
    );

    navigator.msSaveOrOpenBlob(blob, filename);
  } else {
    let downloadLink = document.createElement("a");

    document.body.appendChild(downloadLink);

    downloadLink.href = 'data:' + dataType + ';base64,' + base64(tableExcel);

    downloadLink.download = filename;

    downloadLink.click();
  }
},
    /// Tính toán công thức
    formalaCal(congthuc, code) {
      console.log("cong thuc", congthuc, code);

      var getFromBetween = {
        results: [],
        string: "",
        getFromBetween: function (sub1, sub2) {
          if (this.string.indexOf(sub1) < 0 || this.string.indexOf(sub2) < 0) return false;
          var SP = this.string.indexOf(sub1) + sub1.length;
          var string1 = this.string.substr(0, SP);
          var string2 = this.string.substr(SP);
          var TP = string1.length + string2.indexOf(sub2);
          return this.string.substring(SP, TP);
        },
        removeFromBetween: function (sub1, sub2) {
          if (this.string.indexOf(sub1) < 0 || this.string.indexOf(sub2) < 0) return false;
          var removal = sub1 + this.getFromBetween(sub1, sub2) + sub2;
          this.string = this.string.replace(removal, "");
        },
        getAllResults: function (sub1, sub2) {
          // first check to see if we do have both substrings
          if (this.string.indexOf(sub1) < 0 || this.string.indexOf(sub2) < 0) return;

          // find one result
          var result = this.getFromBetween(sub1, sub2);
          // push it to the results array
          this.results.push(result);
          // remove the most recently found one from the string
          this.removeFromBetween(sub1, sub2);

          // if there's more substrings
          if (this.string.indexOf(sub1) > -1 && this.string.indexOf(sub2) > -1) {
            this.getAllResults(sub1, sub2);
          } else return;
        },
        get: function (string, sub1, sub2) {
          this.results = [];
          this.string = string;
          this.getAllResults(sub1, sub2);
          return this.results;
        }
      };
      var arrays = [];
      var arrValues = [];
      var str = congthuc;

      arrays = getFromBetween.get(str, "<", ">");

      // if(this.arrayValues && this.arrayValues.length > 0){
      //   this.arrayValues.map((val) => {
      //     if (arrays.includes(val.code)) {
      //       arrValues.push(val);
      //     }
      //   })
      // }
      if (this.model && this.model.body && this.model.body.length > 0) {
        this.model.body.map((value) => {
          if (value && value.rowValues && value.rowValues.length > 0) {
            value.rowValues.map((val) => {
              if (arrays.includes(val.code)) {
                arrValues.push(val);
              }
            })
          }
        })
      }
      var strCongThuc = str;
      arrValues.forEach((value) => {
        strCongThuc = strCongThuc.replace("<" + value.code + ">", value.value);
      })
      const math = new StringMathEvaluator();

      if (this.model && this.model.body && this.model.body.length > 0) {
        this.model.body.map((value) => {
          if (value && value.rowValues && value.rowValues.length > 0) {
            value.rowValues.map((val) => {
              if (val.code == code) {
                const twentyOne = math.eval(strCongThuc);
                if(Number(twentyOne)){
                  val.value = twentyOne;
                }else{
                  val.value = "";
                }
              }
            })
          }
        })
      }
    },
    // thêm dòng tính tổng
    async addRowTong() {
      await this.$store.dispatch("rowValueStore/addRowTong", this.$route.params.id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.renderNhapLieuBangBieu();
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },
    ////////////////////////////////
    async createSubRowValue(keyRow, isSubValue) {
      var rowValue = [];
      await this.$store.dispatch("bangBieuStore/saveDataBangBieu", this.model.body).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          // this.renderNhapLieuBangBieu();
          // this.$store.dispatch("globalStore/clearChiTieus");

        }
      });
      if (isSubValue) {
        keyRow.rowValues.forEach(value => rowValue.push({
          bangBieuId: value.bangBieuId,
          mauBieuId: value.mauBieuId,
          parentId: keyRow.keyRow,
          thuocTinhId: value.thuocTinhId,
          value: null,
          width: value.width,
          level: value.level + 1,
          fontStyle: styleModel.FontStyle[0],
          fontWeight: null,
          order: value.order,
          fontHorizontalAlign: styleModel.FontHorizontalAlign[1],
          // fontVerticalAlign: styleModel.FontVerticalAlign,
          // formula: formulaModel.FormulaModel,
          styleInput: value.styleInput,
        }));
        await this.$store.dispatch("rowValueStore/createSub", rowValue).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.renderNhapLieuBangBieu();
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
        });
      } else {
        // var thuocTinhs = [];
        await this.$store.dispatch("thuocTinhStore/getThuocTinhHasChildren", this.$route.params.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            // thuocTinhs = res.data;
            if (res.data && res.data.length > 0) {
              res.data.forEach(value => rowValue.push({
                bangBieuId: value.bangBieuId,
                mauBieuId: value.mauBieuId,
                parentId: null,
                thuocTinhId: value.id,
                value: null,
                width: value.width,
                level: value.level + 1,
                fontStyle: styleModel.FontStyle[0],
                fontWeight: null,
                fontHorizontalAlign: styleModel.FontHorizontalAlign[1],
                // fontVerticalAlign: styleModel.FontVerticalAlign,
                // formula: formulaModel.FormulaModel,
                order: value.order,
                styleInput: value.styleInput,
              }));
            }
          }
        });
        await this.$store.dispatch("rowValueStore/createSub", rowValue).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.renderNhapLieuBangBieu();
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
        });
      }
    },
    async getRowValueByKeyRow(keyRow) {
      try {
        await this.$store.dispatch("rowValueStore/getRowValueByKeyRow", keyRow).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            // this.optionsRowValueFilter = items;
            let temp = [];
            this.chiTieus.map(value => {
              var val = items.find(x => x.thuocTinhId == value.thuocTinhId);
              if (val != null) {
                // value = val;
                val.tenThuocTinh = value.tenThuocTinh;
                this.configChiTieu.parentId = val.parentId;
                this.configChiTieu.thuTu = val.thuTu;
                temp.push(val)
              }
            })
            this.chiTieus = temp;
            this.showModalInputRowValue = true;
          }
          return [];
        });
      } finally {
        console.log("");
      }
    },
    async clearRowValue() {
      this.chiTieus = [];
      this.thuocTinhIsChiTieu.map((value, index) => {
        var ct = rowValueModel.baseJson();
        ct.bangBieuId = value.bangBieuId;
        ct.thuocTinhId = value.id;
        ct.tenThuocTinh = value.ten;
        ct.styleInput = value.styleInput;

        this.chiTieus.push(ct);
      })

      this.configChiTieu = {
        parentId: null,
        thuTu: 0
      };
    },
    handleShowModalInputRowValue() {
      this.getThuocTinhChiTieu();
      this.showModalInputRowValue = true;
    },
    async handleSubmitDeleteRowValue(value) {
      await this.$store.dispatch("bangBieuStore/saveDataBangBieu", this.model.body).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          // this.renderNhapLieuBangBieu();
          // this.$store.dispatch("globalStore/clearChiTieus");

        }
      });
      if (value.rowValues && value.rowValues.length > 0) {
        await this.$store.dispatch("rowValueStore/deleteRowValue", value.rowValues).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            // this.getThuocTinhChiTieu();
            // this.getTreeParentRowValue();
            // this.clearRowValue();
            this.renderNhapLieuBangBieu();
            this.showModalInputRowValue = false;
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
        });
      }
    },
    checkValueChiTieu() {
      let check = false;
      this.chiTieus && this.chiTieus.map(value => {
        if (value.id)
          check = true;
      })
      return check;
    },
    async handleSubmitRowValue() {
      this.submitted = true;
      await this.$store.dispatch("rowValueStore/create", this.chiTieus).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.chiTieus = [];
          this.getThuocTinhChiTieu();
          //this.renderBodyByBangBieuId();
          this.getTreeParentRowValue();
          this.clearRowValue();
          this.renderNhapLieuBangBieu();
          this.showModalInputRowValue = false;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
      this.submitted = false;
    },
    async handleResetThuocTinhIsChiTieu() {
      await this.getThuocTinhChiTieu();
      this.chiTieus = [];
      this.thuocTinhIsChiTieu.map((value, index) => {
        var ct = rowValueModel.baseJson();
        ct.bangBieuId = value.bangBieuId;
        ct.thuocTinhId = value.id;
        ct.tenThuocTinh = value.ten;
        ct.styleInput = value.styleInput;

        this.chiTieus.push(ct);
      })
    },
    async getThuocTinhChiTieu() {
      try {
        await this.$store.dispatch("thuocTinhStore/getThuocTinhChiTieu", this.$route.params.id).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.thuocTinhIsChiTieu = items;
          }
          return [];
        });
      } finally {
        console.log("");
      }
    },
    async getTreeParentRowValue() {
      try {
        await this.$store.dispatch("rowValueStore/getTreeParentRowValue", this.$route.params.id).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.optionsRowValueFilter = items;
          }
          return [];
        });
      } finally {
        console.log("");
      }
    },
    normalizer(node) {
      if (node.children == null || node.children == 'null') {
        delete node.children;
      }
    },
    handleGoBack() {
      this.$router.go(-1);
    },
    async handleSubmitBangBieu() {
      await this.$store.dispatch("bangBieuStore/saveDataBangBieu", this.model.body).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.renderNhapLieuBangBieu();
          // this.$store.dispatch("globalStore/clearChiTieus");

        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
      console.log(this.model.body);
    },
    async renderNhapLieuBangBieu() {
      try {
        await this.$store.dispatch("bangBieuStore/renderNhapLieuBangBieu", this.$route.params.id).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.model = items;
            this.arrayValues = this.model.values;
            if (this.model && this.model.bangBieu)
              this.title = this.model.bangBieu.ten
          }
        });
      } finally {
        console.log("");
      }
    },

    checkIsStar(value) {
      return value && value.length > 0 && value.includes('*');
    },
    checkIsEmpty(value) {
      return value && value.length > 0;
    },
  }

}
</script>

<style>
.dynamic-table table {
  border-collapse: collapse;

  width: 100%;
}

.dynamic-table td {
  border: 1px solid;
}

.td-stt {
  text-align: center;
  width: 50px;
}

.td-xuly {
  text-align: center;
  width: 100px;
}

.td-thuTu {
  text-align: center;
  width: 70px;
}

.table > tbody > tr > td {
  padding: 0px;
  line-height: 30px;
}

.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}


.show-detail-vbd {
  background-color: #eda910;
  border: 1px solid #eda910;
  border-radius: 0px;
}

@media only screen and (max-width: 768px) {
  .show-detail-vbd {
    margin: 0px;
  }
}

.show-detail-vbd .card-body {
  padding: 0;
}

.show-detail-vbd .card-body .nav-tabs .nav-link {
  border-top-left-radius: 0px;
  border-top-right-radius: 0px;
}

.show-detail-vbd .card-body .nav-tabs .nav-link.active {
  color: #055E8F;
}

.show-detail-vbd .card-body .nav-tabs .nav-link {
  color: #fff;
}

.text-black-2a {
  color: #2a2a2a;
}

.text-black {
  color: #000;
}

.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}

@media only screen and (max-width: 768px) {
  .b-table-vbd > td:nth-of-type(1):before {
    content: "STT";
    font-weight: bold;
    color: #00568c;
  }

  /*.b-table-vbd>td:nth-of-type(2):before {*/
  /*  content: "Số VB đi";*/
  /*  font-weight: bold;*/
  /*  color: #00568c;*/
  /*}*/
  /*.b-table-vbd>td:nth-of-type(3):before {*/
  /*  content: "Trích yếu";*/
  /*  font-weight: bold;*/
  /*  color: #00568c;*/
  /*}*/
  /*.b-table-vbd>td:nth-of-type(4):before {*/
  /*  content: "Ngày nhập";*/
  /*  font-weight: bold;*/
  /*  color: #00568c;*/
  /*}*/
  /*.b-table-vbd>td:nth-of-type(5):before {*/
  /*  content: "Ngày ký";*/
  /*  font-weight: bold;*/
  /*  color: #00568c;*/
  /*}*/
  /*.b-table-vbd>td:nth-of-type(6):before {*/
  /*  content: "Trạng thái";*/
  /*  font-weight: bold;*/
  /*  color: #00568c;*/
  /*}*/
  /*.b-table-vbd>td:nth-of-type(7):before {*/
  /*  content: "";*/
  /*  font-weight: bold;*/
  /*  color: #00568c;*/
  /*}*/
}
.dynamic-table{
  width: 100%;
}

@media only screen and (max-width: 768px){
  .dynamic-table{
    border-spacing: 0;
    border-collapse: collapse;
    width: 1440px;
    border: 1px solid #ddd;
  }

  .dynamic-table tr th,#dynamic-table1 table tr td{
    text-align: left;
    padding: 8px;
  }

 .dynamic-table thead tr:nth-child(even){background-color: #f2f2f2}
}
</style>
