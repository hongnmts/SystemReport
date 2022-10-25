<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";
import {pagingModel} from "@/models/pagingModel";
import {required} from "vuelidate/lib/validators";
import {vanBanDenModel} from "@/models/vanBanDenModel";
import Multiselect from "vue-multiselect";
import DatePicker from "vue2-datepicker";
/**
 * Form editor
 */
import CKEditor from "@ckeditor/ckeditor5-vue";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import vue2Dropzone from "vue2-dropzone";
import {butPheModel} from "@/models/butPheModel";
import {phanCongModel} from "@/models/phanCongModel";
import {notifyModel} from "@/models/notifyModel";
import Swal from "sweetalert2";
import login from "@/router/views/account/login";
import {trangThaiModel} from "@/models/trangThaiModel";
import {CURRENT_USER} from "@/helpers/currentUser";

/**
 * Advanced table component
 */
export default {
  page: {
    title: "Lưu công văn đến",
    meta: [{name: "description", content: appConfig.description}]
  },
  components: {
    Layout,
    PageHeader,
    Multiselect,
    ckeditor: CKEditor.component,
    DatePicker,
  },
  data() {
    return {
      title: "Lưu công văn đến",
      items: [
        {
          text: "E-Office",
          href: "/"
        },
        {
          text: "Văn bản đến",
          href: "/van-ban-den"
        },
        {
          text: "Danh sách",
          active: true
        }
      ],
      data: [],
      showModal: false,
      showDetail: false,
      showDeleteModal: false,
      submitted: false,
      listCoQuan: [],
      listRole: [],
      model: vanBanDenModel.baseJson(),
      modelButPhe: butPheModel.baseJson(),
      modelPhanCong: phanCongModel.baseJson(),
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      todoTotalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 50,
      pageOptions: [50, 80, 100],
      filter: null,
      todoFilter: null,
      filterOn: [],
      todofilterOn: [],
      optionsDonViTree: [],
      isBusy: false,
      sortBy: "age",
      sortDesc: false,
      fields: [
        {
          key: 'STT',
          label: 'STT',
          thStyle: {width: '50px', minWidth: '50px'},
          class: "text-center content-capso",
          thClass: "text-primary hidden-sortable",
        },
        {
          key: "soLuuCV",
          label: "Số lưu CV",
          class: "px-1 text-center content-capso",
          thStyle: {width: '130px', minWidth: '100px'},
          thClass: "text-primary hidden-sortable",
          sortable: true,
        },
        {
          key: "soVBDen",
          label: "Số CV đến",
          class: "px-1 text-center content-capso",
          thStyle: {width: '130px', minWidth: '100px'},
          thClass: "text-primary hidden-sortable",
        },
        {
          key: "trichYeu",
          label: "Trích yếu",
          class: "px-1 content-capso",
          thClass: "text-primary hidden-sortable",
        },
        {
          key: "donViSoan",
          label: "Đơn vị soạn",
          class: "px-1 text-center content-capso",
          thStyle: { width:'200px', maxWidth: '200px'},
          thClass: "text-primary hidden-sortable",
        },
        {
          key: "nguoiKy",
          label: "Người ký",
          class: "px-1 text-center content-capso",
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: "text-primary hidden-sortable",
        },
        {
          key: "ngayKy",
          label: "Ngày ký",
          thStyle: {width: '100px', minWidth: '100px'},
          class: "px-1 text-center content-capso",
          thClass: "text-primary hidden-sortable",
        },
        {
          key: 'process',
          label: 'Xử lý',
          thStyle: {width: '80px', minWidth: '110px'},
          thClass: "text-primary hidden-sortable",
        }
      ],
      optionsLoaiVanBan: [],
      optionsDonVi: [],
      optionsTreeDonVi: [],
      optionsKhoiCoQuan: [],
      optionsLinhVuc: [],
      optionsUser: [],
      optionsHinhThucNhan: [],
      optionsMucDo: [],
      optionsTrangThai: [],
      editor: ClassicEditor,
      editorConfig: {
        height: '200px'
      },
      apiUrl: process.env.VUE_APP_API_URL,
      url: `${process.env.VUE_APP_API_URL}files/upload`,
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}files/upload`,
        thumbnailWidth: 300,
        thumbnailHeight: 160,
        maxFiles: 4,
        maxFilesize: 30,
        headers: {"My-Awesome-Header": "header value"},
        addRemoveLinks: true,
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.doc,.docx,.xlsx,.pptx,.pdf",
      },
      showModalButPhe: false,
      showModalPhanCong: false,
      phanCong: [],
      modelTrangThai: trangThaiModel.currentVBDBaseJson(),
      currentUserName: CURRENT_USER.USERNAME,
      showCheckVanBanModal: false,
      showTrangThaiModal: false,
      currentStatus: null,
      valueConsistsOf: 'ALL_WITH_INDETERMINATE',
      listHistoryQuestion: [],
      showHistoryModal: false
    };
  },
  validations: {
    model: {
      soLuuCV: {required},
      soVBDen: {required},
      loaiVanBan: {required},
      trichYeu: {required},
      ngayBanHanh: {required},
      ngayNhan: {required},
    },
    modelTrangThai: {
      currentTrangThai: {required},
      newTrangThai: {required},
      userName: {required},
    }
  },
  computed: {
    /**
     * Total no. of records
     */
    // rows() {
    //   return this.data.length;
    // },
  },
  watch: {
    showModalPhanCong() {
      if (this.showModalPhanCong == false) {
        this.phanCong = [];
      }
    },
    modelTrangThai: {
      deep: true,
      async handler(val) {

      }
    },
    modelButPhe: {
      deep: true,
      async handler(val) {

      }
    }
  },
  created() {
    this.getLoaiVanBan();
    this.getTrangThai();
    this.getDonVi();
    this.getUser();
    this.getLinhVuc();
    this.getHinhThuc();
    this.getMucDo();
    this.getKhoiCoQuan();
    this.getTreeDonVi();
    this.getDonViTree();
    this.$refs.tblList?.refresh;
  },
  // mounted() {
  //   // Set the initial number of items
  //   this.totalRows = this.items.length;
  // },

  methods: {
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
    onFiltered(filteredItems) {
      // Trigger pagination to update the number of buttons/pages due to filtering
      this.totalRows = filteredItems.length;
      this.currentPage = 1;
    },
    handleShowDeleteModal(id) {
      this.model.id = id;
      this.showDeleteModal = true;
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("vanBanDenStore/delete", this.model.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showDeleteModal = false;
            this.model = vanBanDenModel.baseJson();
            this.$refs.tblList.refresh();
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        });
      }
    },
  async  handleDetail(id) {
      await this.$store.dispatch("congVanStore/getByIdLuuCVDen", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.showModal = true;
          this.model = res.data;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
      });
    },
    HandleShowPhanCong(id) {
      this.modelPhanCong.vanBanDenId = id;
      this.phanCong.push({yKienChiDao: "", nguoiButPhe: "", NguoiNhanXuLy: "", vanBanDenId: id,});
      this.showModalPhanCong = true;
    },
    async handleShowButPhe(id) {
      await this.$store.dispatch("vanBanDenStore/getById", id).then(resp => {
        if (resp.resultCode == "SUCCESS") {
          this.loading = false
          this.model = resp.data;
          this.showModalButPhe = true;
          if (this.model.butPhe) {
            this.modelButPhe = this.model.butPhe;
          }
          this.modelButPhe.vanBanDenId = id;
          return [];
        }
        return [];
      })
    },
    async getLoaiVanBan() {
      await this.$store.dispatch("loaiVanBanStore/getAll").then((res) => {
        if (res.resultCode === "SUCCESS") {
          this.optionsLoaiVanBan = res.data;
        } else {
          this.optionsLoaiVanBan = [];
        }
      });
    },
    handleCreate() {
      this.model = vanBanDenModel.baseJson();
      this.showModal = true;
      this.getTrangThai(null);
    },
    async getDonVi() {
      try {
        await this.$store.dispatch("donViStore/get").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsDonVi = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getTreeDonVi() {
      try {
        await this.$store.dispatch("donViStore/getDonViCha").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsTreeDonVi = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getKhoiCoQuan() {
      try {
        await this.$store.dispatch("khoiCoQuanStore/get").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsKhoiCoQuan = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getUser() {
      try {
        await this.$store.dispatch("userStore/getAll").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsUser = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getLinhVuc() {
      try {
        await this.$store.dispatch("dmLinhVucStore/get").then(resp => {
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
    async getHinhThuc() {
      try {
        let promise = this.$store.dispatch("hinhThucGuiStore/get")
        return promise.then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsHinhThucNhan = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getMucDo() {
      await this.$store.dispatch("enumStore/getMucDo").then((res) => {
        if (res.resultCode === "SUCCESS") {
          this.optionsMucDo = res.data;
        } else {
          this.optionsMucDo = [];
        }
      });
    },
    removeThisFile(file, error, xhr) {
      let fileCongViec = JSON.parse(file.xhr.response);
      if (fileCongViec.data && fileCongViec.data.id) {
        let idFile = fileCongViec.data.id;
        let resultData = this.model.uploadFiles.filter(x => {
          return x.fileId != idFile;
        })
        this.model.uploadFiles = resultData;
      }
    },
    addThisFile(file, response) {
      if (this.showModalButPhe == true) {
        if (this.modelButPhe.uploadFiles == null || this.modelButPhe.uploadFiles <= 0) {
          this.modelButPhe.uploadFiles = [];
        }
        let fileSuccess = response.data;
        this.modelButPhe.uploadFiles.push({
          fileId: fileSuccess.id,
          fileName: fileSuccess.fileName,
          ext: fileSuccess.ext
        })
      }
      if (this.model) {
        if (this.model.uploadFiles == null || this.model.uploadFiles.length <= 0) {
          this.model.uploadFiles = [];
        }
        let fileSuccess = response.data;
        this.model.uploadFiles.push({fileId: fileSuccess.id, fileName: fileSuccess.fileName, ext: fileSuccess.ext})
      }
    },
    removeCommentFile(file, error, xhr) {
      let fileCongViec = JSON.parse(file.xhr.response);
      if (fileCongViec.data && fileCongViec.data.id) {
        let idFile = fileCongViec.data.id;
        let resultData = this.model.uploadFiles = null;
        this.model.uploadFiles = resultData;
      }
    },
    addCommentFile(file, response) {
      if (this.model) {
        if (this.model.uploadFiles == null || this.model.length <= 0) {
          this.model.uploadFiles = [];
        }
        let fileSuccess = response.data;
        this.model.uploadFiles = {fileId: fileSuccess.id, fileName: fileSuccess.fileName, ext: fileSuccess.ext}
      }
    },
    normalizer(node) {
      if (node.children == null || node.children == 'null') {
        delete node.children;
      }
    },
    AddformData(id) {
      this.phanCong.push({yKienChiDao: "", nguoiButPhe: "", NguoiNhanXuLy: "", vanBanDenId: id,});
    },
    deleteRow(index) {
      Swal.fire({
        title: "Bạn có chắc muốn xoá không?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#34c38f",
        cancelButtonColor: "#f46a6a",
        confirmButtonText: "Đồng ý"
      }).then(result => {
        if (result.value) {
          this.phanCong.splice(index, 1);
          Swal.fire({
            position: 'top-center',
            icon: 'success',
            title: 'Thành công',
            showConfirmButton: false,
            timer: 1500
          });
        }
      });
    },
    formatRemoveDonVi1(node, instanceId) {
      let value = this.modelTrangThai.donVi?.find(x => x.id == node.id);
      if (value != null) {
        this.modelTrangThai.donVi = this.modelTrangThai.donVi.filter(x => x.id != value.id);
      }
    },
    formatDonVi1(node, instanceId) {
      let index = this.modelTrangThai.donVi?.findIndex(x => x.id == node.id);
      if (index == -1 || index == undefined) {
        if (!this.modelTrangThai.donVi) {
          this.modelTrangThai.donVi = [];
        }
        this.modelTrangThai.donVi.push({id: node.id, ten: node.label, code: node.code});

      }
    },
    formatDonVi(node, instanceId) {
      let index = this.optionsTreeDonVi?.findIndex(x => x.id == node.id);
      if (index == -1 || index == undefined) {
        if (!this.modelButPhe.donViPhoiHop) {
          this.modelButPhe.donViPhoiHop = [];
        }
        this.modelButPhe.donViPhoiHop.push({id: node.id, ten: node.label, code: node.code});

      }
    },
    formatRemoveDonVi(node, instanceId) {
      let value = this.optionsTreeDonVi?.find(x => x.id == node.id);
      if (value != null) {
        this.modelButPhe.donViPhoiHop = this.optionsTreeDonVi.children.filter(x => x.id != value.id);
      }
    },
    myProvider(ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        content: this.filter,
        sortBy: ctx.sortBy,
        sortDesc: ctx.sortDesc,
      }
      this.loading = true

      try {
        let promise = this.$store.dispatch("congVanStore/getPagingParamsLuuCVDen", params)
        return promise.then(resp => {
          let items = resp.data.data
          this.totalRows = resp.data.totalRows
          this.numberOfElement = resp.data.data.length
          this.loading = false
          return items || []
        })
      } finally {
        this.loading = false
      }
    },

    // Trang thai van ban
    async getTrangThai(currentTrangThai) {
      try {
        await this.$store.dispatch("trangThaiStore/getNextTrangThai", {
          loaiTrangThai: "VBDen",
          currentTrangThai: currentTrangThai,
        }).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data;
            this.loading = false
            this.optionsTrangThai = items;
            return items || []
          }
          return [];
        })
      } finally {
        this.loading = false
      }
    },
    async getDonViTree() {
      try {
        await this.$store.dispatch("donViStore/getTree").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsDonViTree = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    handleChuyenTrangThai: function (currentStatus, vanBanDenId) {
      this.getTrangThai(currentStatus)
      this.modelTrangThai.currentTrangThai = currentStatus;
      this.modelTrangThai.newTrangThai = null;
      this.modelTrangThai.vanBanDenId = vanBanDenId;
      this.modelTrangThai.userName = this.currentUserName;
      this.showTrangThaiModal = true;
    },
    async handleCheckVanBanModal(id) {
      await this.$store.dispatch("vanBanDenStore/getById", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.model = res.data;
          this.getTrangThai(this.model.trangThai)
          this.modelTrangThai.currentTrangThai = this.model.trangThai;
          this.modelTrangThai.newTrangThai = null;
          this.modelTrangThai.vanBanDenId = this.model.id;
          this.modelTrangThai.userName = this.currentUserName;
          this.showCheckVanBanModal = true;
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleChuyenTrangThaiVanBan(value){
      if(value){
        this.modelTrangThai.newTrangThai = value;
      }
      this.submitted = true;
      this.$v.$touch();
      if (this.$v.modelTrangThai.$invalid) {
        return;
      } else {

        if (
            this.modelTrangThai
            && this.modelTrangThai.vanBanDenId != null
        ) {
          //Update model
          await this.$store.dispatch("vanBanDenStore/chuyenTrangThaiVanBan", this.modelTrangThai).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.showTrangThaiModal = false;
              this.showCheckVanBanModal = false;
              this.modelTrangThai = trangThaiModel.currentVBDBaseJson()
              this.$refs.tblList.refresh();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          })
        }
      }
      this.submitted = false;


    },
    async handleHistory(id) {
      let loader = this.$loading.show({
        container: this.$refs.formContainer,
      });
      await this.$store.dispatch("historyVanBanStore/getHistoryVanBanDi", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.listHistoryQuestion = res.data;
          this.showHistoryModal = true;

          loader.hide();
        }
      });
    },
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="row">
              <div class="col-12">
                <!-- Table -->
                <div class="table-responsive-sm">
                  <b-table
                      class="datatables"
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
                  >
                    <template v-slot:cell(STT)="data">
                      {{ data.index + ((currentPage - 1) * perPage) + 1 }}
                    </template>
                    <template v-slot:cell(ngayKy)="data">
                      <span v-if="data.item.ngayKy"> {{ data.item.ngayKy }}</span>
                    </template>
                    <template v-slot:cell(nguoiKy)="data">
                      <span v-if="data.item.nguoiKy"> {{ data.item.nguoiKy.fullName }}</span>
                    </template>
                    <template v-slot:cell(donViSoan)="data">
                      <span v-if="data.item.donViSoan"> {{ data.item.donViSoan.ten }}</span>
                    </template>
                    <template v-slot:cell(trichYeu)="data">
                      <div v-if="data.item.trichYeu" :inner-html.prop="data.item.trichYeu | truncate(150)">
                      </div>
                    </template>
                    <template v-slot:cell(process)="data">
                      <div class="d-flex justify-content-around">
                                                <button
                                                    type="button"
                                                    size="sm"
                                                    class="btn btn-outline btn-sm p-0"
                                                    data-toggle="tooltip" data-placement="bottom" title="Chi tiết"
                                                    v-on:click="handleDetail(data.item.id)">
                                                  <i class="fas fa-eye  text-warning me-1"></i>
                                                </button>
                      </div>
                    </template>
                  </b-table>
                  <template v-if="isBusy">
                    <div align="center">Đang tải dữ liệu</div>
                  </template>
                  <template v-if="totalRows <= 0 && !isBusy">
                    <div align="center">Không có dữ liệu</div>
                  </template>
                </div>
                <!-- Paginnation -->
                <div>
                  <div class="row mb-2">
                    <div class="col-sm-12 col-md-6">
                      <div style="display: flex; justify-content: start; align-items: center">
                        <div style="margin-right: 20px" class="">
                          <div id="tickets-table_length" class="dataTables_length">
                            <label class="d-inline-flex align-items-center">
                              Hiện
                              <b-form-select
                                  class="form-select form-select-sm"
                                  v-model="perPage"
                                  size="sm"
                                  :options="pageOptions"
                              ></b-form-select
                              >&nbsp;dòng
                            </label>
                          </div>

                        </div>
                        <div class="">
                          <div>Hiển thị {{ numberOfElement }} trên tổng số {{ totalRows }} dòng</div>
                        </div>
                      </div>
                    </div>
                    <div  class="col-sm-12 col-md-6">
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
            </div>
          </div>
        </div>

        <!--        Modal delete -->
        <b-modal
            v-model="showDeleteModal"
            centered
            title="Xóa dữ liệu"
            title-class="font-18"
            no-close-on-backdrop
        >
          <p>
            Dữ liệu xóa sẽ không được phục hồi!
          </p>
          <template #modal-footer>
            <b-button v-b-modal.modal-close_visit
                      size="sm"
                      class="btn btn-outline-info w-md"
                      v-on:click="showDeleteModal = false">
              Đóng
            </b-button>
            <b-button v-b-modal.modal-close_visit
                      size="sm"
                      variant="danger"
                      type="button"
                      class="w-md"
                      v-on:click="handleDelete">
              Xóa
            </b-button>
          </template>
        </b-modal>
        <!-- Model create -->
        <b-modal
            v-model="showModal"
            title-class="text-black font-18"
            body-class="p-3"
            hide-footer
            centered
            no-close-on-backdrop
            size="xl"
        >
          <template #modal-header="{  }">
            <!-- Emulate built in modal header close button action -->
            <h5> Thông tin Lưu CV</h5>
            <div class="text-end">
              <b-button variant="light" size="sm" style="width: 80px" @click="showModal = false">
                Đóng
              </b-button>
            </div>
          </template>
          <form
              ref="formContainer">
            <div class="row">
              <div class="col-lg-7 col-md-12">
                <div class="row">
                  <!--                              Số lưu -->
                  <div class="col-md-3">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Số lưu CV</label> <span
                        class="text-danger">*</span>
                      <input
                          id="validationCustom01"
                          v-model="model.soLuuCV"
                          type="text"
                          class="form-control"
                          placeholder="eg: 123-dthu"
                          :class="{
                                      'is-invalid': submitted && $v.model.soLuuCV.$error,
                                    }"
                      />
                      <div
                          v-if="submitted && $v.model.soLuuCV.$error"
                          class="invalid-feedback"
                      >
                                <span v-if="!$v.model.soLuuCV.required"
                                >Vui lòng thêm số lưu CV.</span
                                >
                      </div>
                    </div>
                  </div>
                  <!--                            Số VB đến -->
                  <div class="col-md-5">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Số văn bản đến</label> <span
                        class="text-danger">*</span>
                      <input
                          id="validationSoVBDen"
                          v-model="model.soVBDen"
                          type="text"
                          class="form-control"
                          placeholder="eg: 123-dthu"
                          :class="{
                                      'is-invalid': submitted && $v.model.soVBDen.$error,
                                    }"
                      />
                      <div
                          v-if="submitted && $v.model.soVBDen.$error"
                          class="invalid-feedback"
                      >
                                <span v-if="!$v.model.soVBDen.required"
                                >Vui lòng thêm số văn bản đến.</span
                                >
                      </div>
                    </div>
                  </div>
                  <!--                            Loại văn bản-->
                  <div class="col-md-4">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Loại văn bản</label> <span
                        class="text-danger">*</span>
                      <multiselect
                          v-model="model.loaiVanBan"
                          :options="optionsLoaiVanBan"
                          track-by="id"
                          label="ten"
                          placeholder="Chọn trạng thái"
                          deselect-label="Nhấn để xoá"
                          selectLabel="Nhấn enter để chọn"
                          selectedLabel="Đã chọn"
                      ></multiselect>
                      <div
                          v-if="submitted && $v.model.loaiVanBan.$error"
                          class="invalid-feedback"
                      >
                                <span v-if="!$v.model.loaiVanBan.required"
                                >Vui lòng thêm số văn bản đến.</span
                                >
                      </div>
                    </div>
                  </div>
                  <!--                            Trích yếu -->
                  <div class="col-md-12">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Trích yếu</label> <span
                        class="text-danger">*</span>
                      <ckeditor
                          v-model="model.trichYeu"
                          :editor="editor"
                          :config="editorConfig"
                      ></ckeditor>
                    </div>
                  </div>
                  <!--                            Ngày ban hành -->
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Ngày ban hành</label>
                      <date-picker v-model="model.ngayBanHanh"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="model.ngayBanHanh"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder="Nhập ngày ban hành"/>
                        </div>
                      </date-picker>
                    </div>
                  </div>
                  <!--                            Ngày nhận -->
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Ngày nhận</label>
                      <date-picker v-model="model.ngayNhan"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="model.ngayNhan"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder="Nhập ngày nhận"/>
                        </div>
                      </date-picker>
                    </div>
                  </div>
                  <div v-if="model.id">
                    <div v-if="model.file != null  && model.file.length > 0" class="col-md-12">
                      <label for="">Danh sách đã ký (Nhấn vào để tải xuống)</label>
                      <div
                          class=" p-1"
                      >
                        <template>
                          <div v-for="(file, index) in model.file" :key="index">
                            <a
                                :href="`${apiUrl}files/view/${file.fileId}`"
                                class=" fw-medium"
                            ><i
                                :class="`mdi font-size-16 align-middle me-2`"
                            ></i>
                              {{ index + 1 }}: {{ file.fileName }}</a
                            >
                          </div>
                        </template>
                      </div>

                    </div>
                  </div>

                </div>

              </div>
              <div class="col-lg-5 col-md-12">
                <div class="row">
                  <!--                            Ngày Ký-->
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Ngày ký</label>
                      <date-picker v-model="model.ngayKy"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="model.ngayKy"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder="Nhập ngày ký"/>
                        </div>
                      </date-picker>
                    </div>
                  </div>
                  <!--                            Người ký-->
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Người ký</label>
                      <multiselect
                          v-model="model.nguoiKy"
                          :options="optionsUser"
                          track-by="id"
                          label="fullName"
                          placeholder="Chọn người ký"
                          deselect-label="Nhấn để xoá"
                          selectLabel="Nhấn enter để chọn"
                          selectedLabel="Đã chọn"
                      ></multiselect>
                    </div>
                  </div>
                  <!--                            Thời hạn xử lý-->
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Thời hạn xử lý</label>
                      <date-picker v-model="model.hanXuLy"
                                   format="DD/MM/YYYY"
                                   value-type="format"
                      >
                        <div slot="input">
                          <input v-model="model.hanXuLy"
                                 v-mask="'##/##/####'" type="text" class="form-control"
                                 placeholder="Nhập hạn xử lý"/>
                        </div>
                      </date-picker>
                    </div>
                  </div>
                  <!--                            Trạng thái-->
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Trạng thái</label>
                      <multiselect
                          v-model="model.trangThai"
                          :options="optionsTrangThai"
                          track-by="id"
                          label="ten"
                          placeholder="Chọn trạng thái"
                          deselect-label="Nhấn để xoá"
                          selectLabel="Nhấn enter để chọn"
                          selectedLabel="Đã chọn"
                      ></multiselect>
                    </div>
                  </div>
                  <!--                            Khối cơ quan gửi-->
                  <div class="col-md-12">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Khối cơ quan gửi</label>
                      <multiselect
                          v-model="model.khoiCoQuanGui"
                          :options="optionsKhoiCoQuan"
                          track-by="id"
                          label="ten"
                          placeholder="Chọn trạng thái"
                          deselect-label="Nhấn để xoá"
                          selectLabel="Nhấn enter để chọn"
                          selectedLabel="Đã chọn"
                      ></multiselect>
                    </div>
                  </div>
                  <!--                            Cơ quan gửi-->
                  <div class="col-md-12">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Cơ quan gửi</label>
                      <multiselect
                          v-model="model.coQuanGui"
                          :options="optionsDonVi"
                          track-by="id"
                          label="ten"
                          placeholder="Chọn trạng thái"
                          deselect-label="Nhấn để xoá"
                          selectLabel="Nhấn enter để chọn"
                          selectedLabel="Đã chọn"
                      ></multiselect>
                    </div>
                  </div>
                  <!--                            Hình thức nhận -->
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Hình thức nhận</label>
                      <multiselect
                          v-model="model.hinhThucNhan"
                          :options="optionsHinhThucNhan"
                          track-by="id"
                          label="ten"
                          placeholder="Chọn hình thức nhận"
                          deselect-label="Nhấn để xoá"
                          selectLabel="Nhấn enter để chọn"
                          selectedLabel="Đã chọn"
                      ></multiselect>
                    </div>
                  </div>
                  <!--                            Lĩnh vực-->
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Lĩnh vực</label>
                      <multiselect
                          v-model="model.linhVuc"
                          :options="optionsLinhVuc"
                          track-by="id"
                          label="ten"
                          placeholder="Chọn lĩnh vực"
                          deselect-label="Nhấn để xoá"
                          selectLabel="Nhấn enter để chọn"
                          selectedLabel="Đã chọn"
                      ></multiselect>
                    </div>
                  </div>
                  <!--                            Mức độ tính chất-->
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Mức độ tính chất</label>
                      <multiselect
                          v-model="model.mucDoTinhChat"
                          :options="optionsMucDo"
                          track-by="id"
                          label="name"
                          placeholder="Chọn mức độ tính chất"
                          deselect-label="Nhấn để xoá"
                          selectLabel="Nhấn enter để chọn"
                          selectedLabel="Đã chọn"
                      ></multiselect>
                    </div>
                  </div>
                  <!--                            Mức độ bảo mật-->
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Mức độ bảo mật</label>
                      <multiselect
                          v-model="model.mucDoBaoMat"
                          :options="optionsMucDo"
                          track-by="id"
                          label="name"
                          placeholder="Chọn mức độ bảo mật"
                          deselect-label="Nhấn để xoá"
                          selectLabel="Nhấn enter để chọn"
                          selectedLabel="Đã chọn"
                      ></multiselect>
                    </div>
                  </div>
                  <!--                            Hồ sơ đơn vị-->
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Hồ sơ đơn vị</label>
                      <input
                          v-model="model.hoSoDonVi"
                          type="text"
                          class="form-control"
                          placeholder=""
                      />
                    </div>
                  </div>
                  <!--                            Nơi lưu trữ-->
                  <div class="col-md-6">
                    <div class="mb-2">
                      <label class="form-label" for="validationCustom01">Nơi lưu trữ</label>
                      <input
                          v-model="model.noiLuuTru"
                          type="text"
                          class="form-control"
                          placeholder="Nhập nơi lưu trữ"
                      />
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="text-end pt-2 mt-3">
            </div>
          </form>
        </b-modal>
      </div>
    </div>
  </Layout>
</template>
<style>
.td-stt {
  text-align: center;
  width: 50px;
}

.td-xuly {
  text-align: center;
  width: 130px;
}

.table > tbody > tr > td {
  padding: 0px;
  line-height: 30px;
}

.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}

.ck-editor__editable {
  min-height: 100px !important;
}

.ck-content {
  height: 100px !important;
}

.customAddElement {
  position: absolute;
  top: 8px;
  left: -20px;
}

.custom-ribon {
  position: absolute;
  width: 130px;
  top: 15px;
  left: -15px;
}

.custom-ribon > div {
  border-radius: 3px;
}

.title-capso {
  font-weight: bold;
  color: #00568C;

}

.content-capso {
  color: #00568C;
}

.capso-container {
  margin-top: 10px;
  display: flex;
  padding: 0px;
}

.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}
</style>