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
import {quanLyKhoaHocModel} from "@/models/quanLyKHModel";

export default {
  page: {
    title: "Thống kê đề tài",
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
      title: "Thống kê đề tài",
      items: [
        {
          text: "Thống kê đề tài",
          href: "/quan-ly-de-tai",
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
      model: quanLyKhoaHocModel.baseJson(),
      filterModel: quanLyKhoaHocModel.baseTKJson(),
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
          key: "capQuanLy",
          label: "Cấp quản lý",
          class: "content-capso",
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: "text-primary hidden-sortable text-center",
          sortable: true,
        },
        {
          key: "tenDeTai",
          label: "Tên đề tài",
          class: "content-capso",
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: "text-primary hidden-sortable text-center",
          sortable: true,
        },
        {
          key: "chuTri",
          label: "Tổ chức chủ trì",
          class: "content-capso",
          thStyle: {width: '150px', minWidth: '200px'},
          thClass: "text-primary hidden-sortable",
        },
        {
          key: "chuNhiem",
          label: "Chủ nhiệm",
          thStyle: {width: '100px', minWidth: '100px'},
          class: " content-capso",
          thClass: "text-primary hidden-sortable text-center",
        },
        {
          key: "linhVuc",
          label: "Lĩnh vực \n nghiên cứu",
          thStyle: {width: '100px', minWidth: '80px'},
          class: "content-capso",
          thClass: "text-primary hidden-sortable text-center",
        },
        {
          key: 'pheDuyetNhiemVu',
          label: 'Quyết định \n Phê duyệt NV',
          thStyle: {width: '110px', minWidth: '110px'},
          class: " content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary text-center',
        },
        {
          key: 'ngayPheDuyetNhiemVu',
          label: 'Ngày QĐ phê duyệt NV',
          thStyle: {width: '110px', minWidth: '110px'},
          class: " content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'quyetDinhPDKQ',
          label: 'QĐ phê duyệt KP',
          thStyle: {width: '110px', minWidth: '110px'},
          class: " content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'ngayPDKQ',
          label: 'Ngày PQ quyết định',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'nguonNSNN',
          label: 'Nguồn NSNN',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'nguonKhac',
          label: 'Nguồn khác',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'ngayBatDau',
          label: 'Bắt đầu (ngày tháng năm)',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'ngayKetThuc',
          label: 'Kết thúc (ngày tháng năm)',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'ngayGiaHan',
          label: 'Gia hạn (ngày tháng năm)',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'dangThucHien',
          label: 'Đang thực hiện',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'ngayNghiemThu',
          label: 'Ngày nghiệm thu',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'xepLoai',
          label: 'Xếp loại',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'quyetDinhCQ',
          label: 'QĐ chuyển giao',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'ngayDungNghiemThu',
          label: 'Ngày dừng nghiệm thu',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'donViTiepNhan',
          label: 'Đơn vị tiếp nhận',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
      ],
      optionsDeTai: [],
      optionsChuTri: [],
      optionsChuNhiem: [],
      optionsLinhVuc: [],
      optionsQDPheDuyetKP: [],
      optionsDangThucHien: [],
      optionsXepLoai: [],
      optionsQuyetDinhCG: [],
      optionsDonViTiepNhan: [],
      optionsCapQuanLy: [],
      optionsPheDuyetNV: [],
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
    this.getDeTai();
    this.getChuNhiem();
    this.getXepLoai();
    this.getDangThucHien();
    this.getDonViTiepNhan();
    this.getChuTri();
    this.getLinhVuc();
    this.getQDChuyenGiao();
    this.getQDPheDuyet();
    this.getCapQuanLy();
    this.getPheDuyetNV();

    this.assignField();
    this.$refs.tblList?.refresh()
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
    async getCapQuanLy() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "CAPQUANLY").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsCapQuanLy = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getPheDuyetNV() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "PHEDUYETNV").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsPheDuyetNV = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getDeTai() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "DETAI").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsDeTai = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },

    async getChuNhiem() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "CHUNHIEM").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsChuNhiem = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },


    async getChuTri() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "CHUTRI").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsChuTri = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },


    async getLinhVuc() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "LINHVUC").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsLinhVuc = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },


    async getQDPheDuyet() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "QUYETDINHPHEQUYET").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsQDPheDuyetKP = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },

    async getDangThucHien() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "DANGTHUCHIEN").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsDangThucHien = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },


    async getXepLoai() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "XEPLOAI").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsXepLoai = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },


    async getQDChuyenGiao() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "QUYETDINHCHUYENGIAO").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsQuyetDinhCG = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },

    async getDonViTiepNhan() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "DONVITIEPNHAN").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsDonViTiepNhan = items;
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
        let promise = this.$store.dispatch("quanLyKhoaHocStore/getPagingParamsThongKe", this.filterModel)
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
    formatNumber(value) {
      if (value) {
        var temp = value.toString().split('.')
        temp[0] = temp[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',')
        return temp.join('.')
      }
      return 0
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
                  <div class="col-md-3">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Cấp quản lý</label>
                      <multiselect
                          v-model="filterModel.capQuanLy"
                          :options="optionsCapQuanLy"
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
                      <label class="form-label" for="validationCustom01">Tên đề tài</label>
                      <multiselect
                          v-model="filterModel.tenDeTai"
                          :options="optionsDeTai"
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
                      <label class="form-label" for="validationCustom01">Lĩnh vực</label>
                      <multiselect
                          v-model="filterModel.linhVuc"
                          :options="optionsLinhVuc"
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
                      <label class="form-label" for="validationCustom01">Tổ chức chủ trì</label>

                      <multiselect
                          v-model="filterModel.chuTri"
                          :options="optionsChuTri"
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
                      <label class="form-label" for="validationCustom01">Chủ nhiệm</label>
                      <multiselect
                          v-model="filterModel.chuNhiem"
                          :options="optionsChuNhiem"
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
                      <label class="form-label" for="validationCustom01">QĐ Phê duyệt NV</label>
                      <multiselect
                          v-model="filterModel.pheDuyetNV"
                          :options="optionsPheDuyetNV"
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
                      <label class="form-label" for="validationCustom01">Ngày QĐ Phê duyệt NV</label>
                      <multiselect
                          v-model="filterModel.quyetDinhPDKQ"
                          :options="optionsQDPheDuyetKP"
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
                      <label class="form-label" for="validationCustom01">Trạng thái</label>
                      <multiselect
                          v-model="filterModel.dangThucHien"
                          :options="optionsDangThucHien"
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
                      <label class="form-label" for="validationCustom01">Xếp loại</label>
                      <multiselect
                          v-model="filterModel.xepLoai"
                          :options="optionsXepLoai"
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
                      <label class="form-label" for="validationCustom01">QĐ chuyển giao</label>
                      <multiselect
                          v-model="filterModel.quyetDinhCQ"
                          :options="optionsQuyetDinhCG"
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
                      <label class="form-label" for="validationCustom01">Đơn vị tiếp nhận</label>
                      <multiselect
                          v-model="filterModel.donViTiepNhan"
                          :options="optionsDonViTiepNhan"
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

                  <div class="col-md-2">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01"> Từ ngày (Ngày PDKQ)</label>
                      <date-picker v-model="filterModel.ngayPDKQStart"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayPDKQStart"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder=""
                          />
                        </div>
                      </date-picker>
                    </div>
                  </div>
                  <div class="col-md-2">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Đến ngày (Ngày PDKQ)</label>
                      <date-picker v-model="filterModel.ngayPDKQEnd"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayPDKQEnd"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder=""
                          />
                        </div>
                      </date-picker>
                    </div>
                  </div>

                  <div class="col-md-2">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01"> Từ ngày (Ngày bắt đầu)</label>
                      <date-picker v-model="filterModel.ngayBatDauStart"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayBatDauStart"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder=""
                          />
                        </div>
                      </date-picker>
                    </div>
                  </div>
                  <div class="col-md-2">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Đến ngày (Ngày bắt đầu)</label>
                      <date-picker v-model="filterModel.ngayBatDauEnd"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayBatDauEnd"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder=""
                          />
                        </div>
                      </date-picker>
                    </div>
                  </div>

                  <div class="col-md-2">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01"> Từ ngày (Ngày Kết thúc)</label>
                      <date-picker v-model="filterModel.ngayKetThucStart"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayKetThucStart"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder=""
                          />
                        </div>
                      </date-picker>
                    </div>
                  </div>
                  <div class="col-md-2">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Đến ngày (Ngày Kết thúc)</label>
                      <date-picker v-model="filterModel.ngayKetThucEnd"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayKetThucEnd"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder=""
                          />
                        </div>
                      </date-picker>
                    </div>
                  </div>


                  <div class="col-md-2">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01"> Từ ngày (Ngày gia hạn)</label>
                      <date-picker v-model="filterModel.ngayGiaHanStart"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayGiaHanStart"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder=""
                          />
                        </div>
                      </date-picker>
                    </div>
                  </div>
                  <div class="col-md-2">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Đến ngày (Ngày gia hạn)</label>
                      <date-picker v-model="filterModel.ngayGiaHanEnd"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayGiaHanEnd"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder=""
                          />
                        </div>
                      </date-picker>
                    </div>
                  </div>


                  <div class="col-md-2">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01"> Từ ngày (Ngày nghiệm thu)</label>
                      <date-picker v-model="filterModel.ngayNghiemThuStart"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayNghiemThuStart"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder=""
                          />
                        </div>
                      </date-picker>
                    </div>
                  </div>
                  <div class="col-md-2">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Đến ngày (Ngày nghiệm thu)</label>
                      <date-picker v-model="filterModel.ngayNghiemThuEnd"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayNghiemThuEnd"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder=""
                          />
                        </div>
                      </date-picker>
                    </div>
                  </div>

                  <div class="col-md-2">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01"> Từ ngày (Ngày chuyển giao)</label>
                      <date-picker v-model="filterModel.ngayChuyenGiaoStart"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayChuyenGiaoStart"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder=""
                          />
                        </div>
                      </date-picker>
                    </div>
                  </div>
                  <div class="col-md-2">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Đến ngày (Ngày chuyển giao)</label>
                      <date-picker v-model="filterModel.ngayChuyenGiaoEnd"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="filterModel.ngayChuyenGiaoEnd"
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
                                {{ soHoSo }}</strong> đề tài
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

                      <div class="table-responsive-sm table-responsive">
                        <b-table
                            id="dynamic-table3"
                            sticky-header
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
                            {{ data.index + ((currentPage-1)*perPage) + 1  }}
                          </template>
                          <template v-slot:cell(nguonNSNN)="data">
                            <template v-if="data.item.nguonNSNN">
                              {{ formatNumber( data.item.nguonNSNN) }}
                            </template>
                          </template>
                          <template v-slot:cell(nguonKhac)="data">
                            <template v-if="data.item.nguonKhac">
                              {{ formatNumber( data.item.nguonNSNN) }}
                            </template>
                          </template>
                          <template v-slot:cell(capQuanLy)="data">
                            <template v-if="data.item.capQuanLy">
                              <div style="font-weight: bold">
                                {{data.item.capQuanLy.name}}
                              </div>
                            </template>

                          </template>
                          <template v-slot:cell(tenDeTai)="data">
                            <template v-if="data.item.tenDeTai">
                              <div style="font-weight: bold">
                                {{data.item.tenDeTai.name}}
                              </div>
                            </template>

                          </template>
                          <template v-slot:cell(chuTri)="data">
                            <template v-if="data.item.chuTri">
                              {{data.item.chuTri.name}}
                            </template>
                          </template>
                          <template v-slot:cell(chuNhiem)="data">
                            <template v-if="data.item.chuNhiem">
                              {{data.item.chuNhiem.name}}
                            </template>
                          </template>
                          <template v-slot:cell(linhVuc)="data">
                            <template v-if="data.item.linhVuc">
                              {{data.item.linhVuc.name}}
                            </template>
                          </template>
                          <template v-slot:cell(soTien)="data">
                            <template v-if="data.item.soTien">
                              {{formatNumber(data.item.soTien)}}
                            </template>
                          </template>
                          <template v-slot:cell(pheDuyetNhiemVu)="data">
                            <template v-if="data.item.pheDuyetNhiemVu">
                              {{data.item.pheDuyetNhiemVu.name}}
                            </template>
                          </template>
                          <template v-slot:cell(quyetDinhPDKQ)="data">
                            <template v-if="data.item.quyetDinhPDKQ">
                              {{data.item.quyetDinhPDKQ.name}}
                            </template>
                          </template>
                          <template v-slot:cell(dangThucHien)="data">
                            <template v-if="data.item.dangThucHien">
                              {{data.item.dangThucHien.name}}
                            </template>
                          </template>
                          <template v-slot:cell(xepLoai)="data">
                            <template v-if="data.item.xepLoai">
                              {{data.item.xepLoai.name}}
                            </template>
                          </template>
                          <template v-slot:cell(quyetDinhCQ)="data">
                            <template v-if="data.item.quyetDinhCQ">
                              {{data.item.quyetDinhCQ.name}}
                            </template>
                          </template>
                          <template v-slot:cell(donViTiepNhan)="data">
                            <template v-if="data.item.donViTiepNhan">
                              <p v-for="(value, index) in data.item.donViTiepNhan" :key="index">
                                - {{value.name}}
                              </p>
                            </template>
                          </template>
                          <template v-slot:cell(quyetDinh)="data">
                            <template v-if="data.item.quyetDinh">
                              {{data.item.quyetDinh.name}}
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
