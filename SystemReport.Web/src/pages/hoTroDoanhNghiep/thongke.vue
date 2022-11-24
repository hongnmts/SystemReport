<script>
import Layout from "../../layouts/main";

import PageHeader from "@/components/page-header";
import {required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {notifyModel} from "@/models/notifyModel";
import {pagingModel} from "@/models/pagingModel";
import {linhVucModel} from "@/models/linhVucModel";
import {CONSTANTS} from "@/helpers/constants";
import {hoTroDoanhNghiepModel} from "@/models/hoTroDoanhNghiepModel";
import Multiselect from "vue-multiselect";
import DatePicker from "vue2-datepicker";
import Treeselect from '@riophae/vue-treeselect'

export default {
  page: {
    title: "Hỗ trợ doanh nghiệp",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {
    Layout, PageHeader,
    Multiselect,
    DatePicker,
    Treeselect
  },
  data() {
    return {
      title: "Hỗ trợ doanh nghiệp",
      items: [
        {
          text: "Hỗ trợ doanh nghiệp",
          href: "/ho-tro-doanh-nghiep",
          // active: true,
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      data: [],
      dataThongKe: [],
      showModal: false,
      showDetail: false,
      showDeleteModal: false,
      submitted: false,
      model: hoTroDoanhNghiepModel.baseJson(),
      filterModel: hoTroDoanhNghiepModel.baseTKJson(),
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      todoTotalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 10,
      pageOptions: [5, 10, 25, 50, 100],
      filter: null,
      todoFilter: null,
      filterOn: [],
      todofilterOn: [],
      isBusy: false,
      sortBy: "age",
      sortDesc: false,
      fields: [
        {
          key: 'STT',
          label: 'STT',
          thStyle: {width: '50px', minWidth: '50px'},
          class: "text-center content-capso",
          thClass: "text-primary hidden-sortable text-center",
        },
        {
          key: "toChuc",
          label: "Tổ chức",
          class: "content-capso",
          thStyle: {width: '150px', minWidth: '200px'},
          thClass: "text-primary hidden-sortable text-center",
          sortable: true,
        },
        {
          key: "loaiHinh",
          label: "Loại hình",
          class: "content-capso",
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: "text-primary hidden-sortable",
        },
        {
          key: "diaChi",
          label: "Số nhà,đường",
          thStyle: {width: '100px', minWidth: '100px'},
          class: " content-capso",
          thClass: "text-primary hidden-sortable text-center",
        },
        {
          key: "donViHanhChinh",
          label: "Huyện/TP",
          thStyle: {width: '100px', minWidth: '80px'},
          class: "text-center content-capso",
          thClass: "text-primary hidden-sortable text-center",
        },
        {
          key: 'noiDungHoTro',
          label: 'Nội dung hỗ trợ',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "content-capso",
          sortable: false,
          thClass: 'text-center hidden-sortable title-capso text-primary',
        },
        {
          key: 'soTien',
          label: 'Số tiền',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'quyetDinh',
          label: 'Quyết định hỗ trợ',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'ngayKy',
          label: 'Ngày ký',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        // {
        //   key: 'process',
        //   label: 'Xử lý',
        //   thStyle: {width: '120px', minWidth: '120px'},
        //   thClass: "text-primary hidden-sortable text-center",
        //   class: "btn-process"
        // },
      ],
      optionsLoaiHinh: [],
      optionsToChuc: [],
      optionDonViHanhChinh: [],
      optionNoiDungHoTro: [],
      optionQuyetDinh: [],
      opened: [],
      tongSoTien: 0,
      soHoSo: 0,
      hidenFilter: [],
      selectHindenFilter: [],
      valueFilter: []
    };
  },
  validations: {
    model: {
      ten: {required},
      thuTu: {required}
    },
  },
  created() {
    this.getLoaiHinh();
    this.getToChuc();
    this.getDonViHanhChinh();
    this.getNoiDungHoTro();
    this.getQuyetDinh();
    this.$refs.tblList?.refresh()
    this.assignField();
  },
  watch: {
    filterModel: {
      deep: true,
      handler(val) {
        this.$refs.tblList?.refresh()
      }
    },
    selectHindenFilter: {
      deep: true,
      handler(val) {
        this.valueFilter.map((e) => {
          let check = false;
          this.selectHindenFilter.map((value) => {
            if (e.id == value) {
              check = true;
              return;
            }
          })
          e.selected = check;
        })
        this.fields.map((e) => {
          let item = this.valueFilter.find(x => x.id == e.key);
          e.class = e.class.replace("d-none", "")
          e.thClass = e.thClass.replace("d-none", "")
          if (item && !item.selected) {
            e.class += " d-none";
            e.thClass += " d-none";
          } else if (item && item.selected) {
            e.class = e.class.replace("d-none", "")
            e.thClass = e.thClass.replace("d-none", "")
          }
        })
      }
    },
    showModal(status) {
      if (status == false) this.model = hoTroDoanhNghiepModel.baseJson();
    },
    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
      }
    },
  },
  methods: {
    assignField() {
      this.fields.map((e) => {
        this.hidenFilter = [...this.hidenFilter, {id: e.key, label: e.label}]
        this.selectHindenFilter = [...this.selectHindenFilter, e.key]
        this.valueFilter = [...this.valueFilter, {id: e.key, selected: true}]
      })
    },
    async getQuyetDinh() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "QUYETDINH").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionQuyetDinh = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getNoiDungHoTro() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "NOIDUNGHOTRO").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionNoiDungHoTro = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getDonViHanhChinh() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "HUYEN").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionDonViHanhChinh = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getLoaiHinh() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "LOAIHINH").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsLoaiHinh = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getToChuc() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "TOCHUC").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsToChuc = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async fnGetList() {
      await this.onPageChange();
    },
    async onPageChange(page = 1) {
      this.pagination.currentPage = page;
      const params = {
        pageNumber: this.pagination.currentPage,
        pageSize: this.pagination.pageSize,
      }
      this.$refs.tblList?.refresh()
    },
    handleUpdate(id) {
      this.$router.push("/ho-tro-doanh-nghiep/create/" + id)
    },
    async handleDetail(id) {
      await this.$store.dispatch("hoTroDoanhNghiepStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = res.data;
          this.showDetail = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("hoTroDoanhNghiepStore/delete", this.model.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.fnGetList();
            this.showDeleteModal = false;
            this.$refs.tblList.refresh()
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          // });
        });
      }
    },
    myProvider(ctx) {
      this.filterModel.start = ctx.currentPage;
      this.filterModel.limit = ctx.perPage;
      this.loading = true
      try {
        let promise = this.$store.dispatch("hoTroDoanhNghiepStore/getPagingParamsThongKe", this.filterModel)
        return promise.then(resp => {
          if (resp.resultCode == CONSTANTS.SUCCESS) {
            let data = resp.data;
            this.totalRows = data.totalRows
            let items = data.data
            this.numberOfElement = items.length
            this.loading = false
            this.data = items || []
            this.dataThongKe = data.thongKeHTDNVMs;
            this.tongSoTien = data.money;
            this.soHoSo = data.soHoSo;
            return this.data;
          } else {
            this.data = [];
          }
        })
      } finally {
        this.loading = false
      }
    },
    toggle(id) {
      const index = this.opened.indexOf(id);
      if (index > -1) {
        this.opened.splice(index, 1)
      } else {
        this.opened.push(id)
      }
    },
    exportTableToExcel(tableId, filename) {
      this.perPage = 1000;
      this.$refs.tblList?.refresh()
      setTimeout(() => {
        if (filename == '') {
          filename = Date.now();
        }
        let dataType = 'application/vnd.ms-excel';
        let extension = '.xls';

        let base64 = function (s) {
          return window.btoa(unescape(encodeURIComponent(s)))
        };

        let template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><?xml version="1.0" encoding="UTF-8" standalone="yes"?><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>';
        let render = function (template, content) {
          return template.replace(/{(\w+)}/g, function (m, p) {
            return content[p];
          });
        };
        let tableElement;

        if ("dynamic-table3" == tableId) {
          tableElement = document.getElementById(tableId).cloneNode(true);
          tableElement.querySelectorAll(".d-none").forEach(el => el.remove());
        } else {
          tableElement = document.getElementById(tableId);
        }

        let tableExcel = render(template, {
          worksheet: filename,
          table: tableElement.innerHTML
        });

        filename = filename + extension;
        if (navigator.msSaveOrOpenBlob) {
          let blob = new Blob(
              ['\ufeff', tableExcel],
              {type: dataType}
          );

          navigator.msSaveOrOpenBlob(blob, filename);
        } else {
          let downloadLink = document.createElement("a");

          document.body.appendChild(downloadLink);

          downloadLink.href = 'data:' + dataType + ';base64,' + base64(tableExcel);

          downloadLink.download = filename;

          downloadLink.click();
        }

      }, "1000")

    },
  }
}
</script>
<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="row mb-2">
              <div class="col-lg-12 col-md-12">
                <div class="row">

                  <!--                            Loại văn bản-->
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Tổ chức</label> <span
                        class="text-danger">*</span>
                      <multiselect
                          v-model="filterModel.toChuc"
                          :options="optionsToChuc"
                          track-by="id"
                          label="name"
                          placeholder="Chọn"
                          deselect-label="Nhấn để xoá"
                          selectLabel="Nhấn enter để chọn"
                          selectedLabel="Đã chọn"
                          :multiple="true"
                      ></multiselect>
                    </div>
                  </div>
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Loại hình</label> <span
                        class="text-danger">*</span>
                      <multiselect
                          v-model="filterModel.loaiHinh"
                          :options="optionsLoaiHinh"
                          track-by="id"
                          label="name"
                          placeholder="Chọn"
                          deselect-label="Nhấn để xoá"
                          selectLabel="Nhấn enter để chọn"
                          selectedLabel="Đã chọn"
                          :multiple="true"
                      ></multiselect>
                    </div>
                  </div>

                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Huyện/TP</label> <span
                        class="text-danger">*</span>
                      <multiselect
                          v-model="filterModel.donViHanhChinh"
                          :options="optionDonViHanhChinh"
                          track-by="id"
                          label="name"
                          placeholder="Chọn"
                          deselect-label="Nhấn để xoá"
                          selectLabel="Nhấn enter để chọn"
                          selectedLabel="Đã chọn"
                          :multiple="true"
                      ></multiselect>
                    </div>
                  </div>
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Số nhà, đường</label>
                      <input
                          id="validationCustom01"
                          v-model="filterModel.diaChi"
                          type="text"
                          class="form-control"
                          placeholder=""
                      />
                    </div>
                  </div>
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Nội dung hỗ trợ</label> <span
                        class="text-danger">*</span>
                      <multiselect
                          v-model="filterModel.noiDungHoTro"
                          :options="optionNoiDungHoTro"
                          track-by="id"
                          label="name"
                          placeholder="Chọn"
                          deselect-label="Nhấn để xoá"
                          selectLabel="Nhấn enter để chọn"
                          selectedLabel="Đã chọn"
                          :multiple="true"
                      ></multiselect>
                    </div>
                  </div>
                  <!--                  <div class="col-md-6">-->
                  <!--                    <div class="mb-2">-->
                  <!--                      <label class="form-label" for="validationCustom01">Số tiền</label>-->
                  <!--                      <input-->
                  <!--                          id="validationCustom01"-->
                  <!--                          v-model="model.soTien"-->
                  <!--                          type="text"-->
                  <!--                          class="form-control"-->
                  <!--                          placeholder=""-->
                  <!--                      />-->
                  <!--                    </div>-->
                  <!--                  </div>-->
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Quyết định</label> <span
                        class="text-danger">*</span>
                      <multiselect
                          v-model="filterModel.quyetDinh"
                          :options="optionQuyetDinh"
                          track-by="id"
                          label="name"
                          placeholder="Chọn"
                          deselect-label="Nhấn để xoá"
                          selectLabel="Nhấn enter để chọn"
                          selectedLabel="Đã chọn"
                          :multiple="true"
                      ></multiselect>
                    </div>
                  </div>
                  <div class="col-md-3">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01"> Từ ngày (Ngày ký)</label>
                      <date-picker v-model="filterModel.ngayKyStart"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayKyStart"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder=""
                          />
                        </div>
                      </date-picker>
                    </div>
                  </div>
                  <div class="col-md-3">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Đến ngày (Ngày ký)</label>
                      <date-picker v-model="filterModel.ngayKyEnd"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayKyEnd"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder=""
                          />
                        </div>
                      </date-picker>
                    </div>
                  </div>
                  <div class="col-md-3">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01"> Từ ngày (Ngày nhập)</label>
                      <date-picker v-model="filterModel.ngayNhapStart"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayNhapStart"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder=""
                          />
                        </div>
                      </date-picker>
                    </div>
                  </div>
                  <div class="col-md-3">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Đến ngày (Ngày nhập)</label>
                      <date-picker v-model="filterModel.ngayNhapEnd"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayNhapEnd"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder=""
                          />
                        </div>
                      </date-picker>
                    </div>
                  </div>
                </div>

              </div>
            </div>
            <div role="tablist">
              <b-card no-body class="mb-1">
                <b-card-header header-tag="header" role="tab" style="display: flex; justify-content: space-between">
                  <h6 class="m-0 font-14">
                    <a
                        v-b-toggle.accordion-1
                        class="text-dark"
                        href="javascript: void(0);"
                    >Thống kê</a>
                  </h6>
                  <button @click="exportTableToExcel('dynamic-table2', '')" type="button"
                          class="btn btn w-md btn-primary btn-info btn-sm" style="margin-bottom: 10px;"><i
                      class="mdi mdi-plus me-1"></i> Export
                  </button>
                </b-card-header>
                <b-collapse id="accordion-1" visible accordion="my-accordion" role="tabpanel">
                  <div class="row">
                    <div class="col-12">
                      <div class="table-responsive-sm">
                        <div class="datatables custom-table table-responsive-sm">
                          <table role="table"
                                 class="table b-table table-striped table-bordered" id="dynamic-table2">
                            <thead role="rowgroup" class=""><!---->
                            <tr>
                              <td style="width: 50px; text-align: center; font-weight: bold">STT</td>
                              <td style="font-weight: bold">Tên</td>
                              <td style="width: 100px; text-align: center; font-weight: bold">Số lượng</td>
                            </tr>

                            </thead>
                            <tbody role="rowgroup"><!---->
                            <tr>
                              <td></td>
                              <td colspan="3" style="font-size: 16px; text-align: right">Tổng cộng: <strong>
                                {{ soHoSo }}</strong> hồ sơ với tổng kinh phí<strong> {{ tongSoTien }}</strong> đồng
                              </td>
                            </tr>
                            <template v-for="(item, index) in dataThongKe">
                              <tr :key="index" @click="toggle(item.id)" style="cursor: pointer">
                                <td style="text-align: center; font-weight: bold">{{ index + 1 }}</td>
                                <td style="font-weight: bold">{{ item.name }}</td>
                                <td style="text-align: center;font-weight: bold ">{{ item.count }}</td>
                              </tr>
                              <template v-if="opened.includes(item.id) && item.items && item.items.length > 0">
                                <template v-for="(value, index1) in item.items">
                                  <tr v-if="value.count > 0" :key="index + '_'+ index1 + '_1'">
                                    <td style="width: 50px; text-align: center">{{ index + 1 }}.{{ index1 + 1 }}</td>
                                    <td>{{ value.name }}</td>
                                    <td style="width: 100px; text-align: center">{{ value.count }}</td>
                                  </tr>
                                </template>

                              </template>
                            </template>


                            </tbody><!---->
                          </table>
                        </div><!----><!---->
                      </div>
                    </div>
                  </div>
                </b-collapse>
              </b-card>

              <b-card no-body class="mb-1">
                <b-card-header header-tag="header" role="tab" style="display: flex; justify-content: space-between">
                  <h6 class="m-0 font-14">
                    <a
                        v-b-toggle.accordion-2
                        class="text-dark"
                        href="javascript: void(0);"
                    >Dữ liệu</a>
                  </h6>
                  <div style="display: flex; justify-content: space-between; align-items: center">
                    <div style="max-width: 300px">
                      <treeselect
                          :multiple="true"
                          :options="hidenFilter"
                          placeholder="Hãy chọn trường dữ liệu"
                          v-model="selectHindenFilter"
                          :limit="1"
                      />
                      <treeselect-value :value="selectHindenFilter"/>
                    </div>
                    <button @click="exportTableToExcel('dynamic-table3', '')" type="button"
                            class="btn btn w-md btn-primary btn-info btn-sm" style="margin-left: 10px;"><i
                        class="mdi mdi-plus me-1"></i> Export
                    </button>
                  </div>

                </b-card-header>
                <b-collapse id="accordion-2" accordion="my-accordion" role="tabpanel">
                  <div class="row">
                    <div class="col-12">

                      <div class="table-responsive-sm">
                        <b-table
                            id="dynamic-table3"
                            class="datatables custom-table"
                            :items="myProvider"
                            :fields="fields"
                            striped
                            bordered
                            responsive="sm"
                            :per-page="perPage"
                            :current-page="currentPage"
                            :sort-by.sync="sortBy"
                            :sort-desc.sync="sortDesc"
                            :filter="filter"
                            :filter-included-fields="filterOn"
                            ref="tblList"
                            primary-key="id"
                            :busy.sync="isBusy"
                            tbody-tr-class="b-table-linhvuc"
                            label-sort-asc=""
                            label-sort-desc=""
                            label-sort-clear=""
                        >
                          <template v-slot:cell(STT)="data">
                            {{ data.index + ((currentPage - 1) * perPage) + 1 }}
                          </template>
                          <!--                    <template v-slot:cell(process)="data">-->
                          <!--                      <button-->
                          <!--                          type="button"-->
                          <!--                          size="sm"-->
                          <!--                          class="btn btn-detail btn-sm"-->
                          <!--                          data-toggle="tooltip" data-placement="bottom" title="Chi tiết"-->
                          <!--                          v-on:click="handleDetail(data.item.id)">-->
                          <!--                        <i class="fas fa-eye "></i>-->
                          <!--                      </button>-->
                          <!--                      <button-->
                          <!--                          type="button"-->
                          <!--                          size="sm"-->
                          <!--                          class="btn btn-edit btn-sm"-->
                          <!--                          data-toggle="tooltip" data-placement="bottom" title="Cập nhật"-->
                          <!--                          v-on:click="handleUpdate(data.item.id)">-->
                          <!--                        <i class="fas fa-pencil-alt"></i>-->
                          <!--                      </button>-->
                          <!--                      <button-->
                          <!--                          type="button"-->
                          <!--                          size="sm"-->
                          <!--                          class="btn btn-delete btn-sm"-->
                          <!--                          data-toggle="tooltip" data-placement="bottom" title="Xóa"-->
                          <!--                          v-on:click="handleShowDeleteModal(data.item.id)">-->
                          <!--                        <i class="fas fa-trash-alt"></i>-->
                          <!--                      </button>-->
                          <!--                    </template>-->
                          <template v-slot:cell(toChuc)="data">
                            <template v-if="data.item.toChuc">
                              <div style="font-weight: bold">
                                {{ data.item.toChuc.name }}
                              </div>

                            </template>

                          </template>
                          <template v-slot:cell(loaiHinh)="data">
                            <template v-if="data.item.loaiHinh">
                              {{ data.item.loaiHinh.name }}
                            </template>
                          </template>
                          <template v-slot:cell(donViHanhChinh)="data">
                            <template v-if="data.item.donViHanhChinh">
                              {{ data.item.donViHanhChinh.name }}
                            </template>
                          </template>
                          <template v-slot:cell(noiDungHoTro)="data">
                            <template v-if="data.item.noiDungHoTro">
                              <p style="margin: 0px" v-for="(value, index) in data.item.noiDungHoTro" :key="index">
                                - {{ value.name }}
                              </p>
                            </template>
                          </template>
                          <template v-slot:cell(quyetDinh)="data">
                            <template v-if="data.item.quyetDinh">
                              {{ data.item.quyetDinh.name }}
                            </template>
                          </template>
                        </b-table>
                        <template v-if="isBusy">
                          <div align="center">Đang tải dữ liệu</div>
                        </template>
                        <template v-if="totalRows <= 0 && !isBusy">
                          <div align="center">Không có dữ liệu</div>
                        </template>
                      </div>
                      <div class="row">
                        <b-col>
                          <div>Hiển thị {{ numberOfElement }} trên tổng số {{ totalRows }} dòng</div>
                        </b-col>
                        <div class="col">
                          <div
                              class="dataTables_paginate paging_simple_numbers float-end">
                            <ul class="pagination pagination-rounded mb-0">
                              <!-- pagination -->
                              <b-pagination
                                  v-model="currentPage"
                                  :total-rows="totalRows"
                                  :per-page="perPage"
                              ></b-pagination>
                            </ul>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </b-collapse>
              </b-card>

            </div>


          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>
<style>
.td-stt {
  text-align: center;
}

.td-xuly {
  text-align: center;
}

.table > tbody > tr > td {
  padding: 0px;
  line-height: 30px;
}

.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}

@media only screen and (max-width: 768px) {
  .b-table-linhvuc > td:nth-of-type(1):before {
    content: "STT";
    font-weight: bold;
    color: #00568c;
  }

  .b-table-linhvuc > td:nth-of-type(2):before {
    content: "Tên";
    font-weight: bold;
    color: #00568c;
  }

  .b-table-linhvuc > td:nth-of-type(3):before {
    content: "Thứ tự";
    font-weight: bold;
    color: #00568c;
  }

  .b-table-linhvuc > td:nth-of-type(4):before {
    content: "";
    font-weight: bold;
    color: #00568c;
  }
}

</style>
