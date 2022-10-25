<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";
import {pagingModel} from "@/models/pagingModel";
import {congVanModel} from "@/models/congVanModel";
import {required} from "vuelidate/lib/validators";
import Multiselect from "vue-multiselect";
import DatePicker from "vue2-datepicker";
import Switches from "vue-switches";
/**
 * Form editor
 */
import CKEditor from "@ckeditor/ckeditor5-vue";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import vue2Dropzone from "vue2-dropzone";
import {notifyModel} from "@/models/notifyModel";
import Swal from "sweetalert2";
import login from "@/router/views/account/login";
import {trangThaiModel} from "@/models/trangThaiModel";
import {CURRENT_USER} from "@/helpers/currentUser";
import {phanCongModel} from "@/models/phanCongModel";

/**
 * Advanced table component
 */
export default {
  page: {
    title: "Công văn",
    meta: [{name: "description", content: appConfig.description}]
  },
  components: {
    Layout,
    PageHeader,
    Multiselect,
    ckeditor: CKEditor.component,
    DatePicker,
    vueDropzone: vue2Dropzone,
  },
  data() {
    return {
      title: "Công văn",
      items: [
        {
          text: "E-Office",
          href: "/"
        },
        {
          text: "Công Văn",
          href: "/cong-van"
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
      model: congVanModel.baseJson(),
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
          thClass: "text-primary",
        },
        {
          key: "soLuuCV",
          label: "Số lưu CV",
          class: "px-1 text-center content-capso",
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: "text-primary",
          sortable: true,
        },
        {
          key: "soVBDen",
          label: "Số CV đến",
          class: "px-1 text-center content-capso",
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: "text-primary",
        },
        {
          key: "trichYeu",
          label: "Trích yếu",
          class: "px-1 content-capso",
          thStyle: {width: '100px', minWidth: '200px'},
          thClass: "text-primary",
        },
        {
          key: "loaiVanBan",
          label: "Loại văn bản",
          class: "px-1 text-center content-capso",
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: "text-primary",
        },
        {
          key: "trangThai",
          label: "Trạng thái",
          thStyle: {width: '100px', minWidth: '100px'},
          class: "px-1 text-center content-capso",
          thClass: "text-primary",
        },
        {
          key: "ngayNhap",
          label: "Ngày nhập",
          thStyle: {width: '100px', minWidth: '100px'},
          class: "px-1 text-center content-capso",
          thClass: "text-primary",
        },
        {
          key: 'chuyenTrangThai',
          label: '',
          thStyle: {width: '110px', minWidth: '110px'},
          class: "px-1 text-center content-capso",
          sortable: false,
          thClass: 'hidden-sortable title-capso text-primary',
        },
        {
          key: 'process',
          label: 'Xử lý',
          thStyle: {width: '80px', minWidth: '110px'},
          thClass: "text-primary",
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
  async created() {
    this.getLoaiVanBan();
    this.getTrangThai();
    this.getDonVi();
    this.getUser();
    this.getLinhVuc();
    this.getHinhThuc();
    this.getMucDo();
    this.getKhoiCoQuan();
    this.getTreeDonVi();
  },
  // mounted() {
  //   // Set the initial number of items
  //   this.totalRows = this.items.length;
  // },

  methods: {
    /**
     * Search the table data with search input
     */
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
    async handleSubmit(e) {
      e.preventDefault();
      if (
          this.model.id != 0 &&
          this.model.id != null &&
          this.model.id
      ) {
        //Update model
        await this.$store.dispatch("vanBanDenStore/update", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = phanCongModel.baseJson();
            this.$refs.tblList.refresh()
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));

        })
      } else {
        //Create modelhandleSubmit
        this.model.version = 1;
        await this.$store.dispatch("vanBanDenStore/create", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = phanCongModel.baseJson()
            // this.$refs.myVueDropzone.removeAllFiles();
            this.$refs.tblList.refresh();
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        });
      }
    },
    async handleUpdate(id) {
      await this.$store.dispatch("vanBanDenStore/getById", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.model = res.data;
          this.showModal = true;
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
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
            this.model = congVanModel.baseJson();
            this.$refs.tblList.refresh();
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        });
      }
    },
    handleDetail(id) {

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
      this.model = congVanModel.baseJson();
      this.showModal = true;
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
        let promise = this.$store.dispatch("vanBanDenStore/getPagingParams", params)
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
          this.modelTrangThai.vanBanDiId = this.model.id;
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
        let loader = this.$loading.show({
          container: this.$refs.formContainerTrangThai,
        });
        if (
            this.modelTrangThai
            && this.modelTrangThai.vanBanDenId != null
        ) {
          //Update model
          await this.$store.dispatch("vanBanDenStore/chuyenTrangThaiVanBan", this.modelTrangThai).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.showTrangThaiModal = false;
              this.showCheckVanBanModal = false;
              this.model = trangThaiModel.currentVBDBaseJson()
              this.$refs.tblList.refresh();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          })
        }
        loader.hide();
      }
      this.submitted = false;


    }
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
            <div class="row mb-2">
              <div class="col-sm-4">
                <div class="search-box me-2 mb-2 d-inline-block">
                  <div class="position-relative">
                    <input
                        v-model="filter"
                        type="text"
                        class="form-control"
                        placeholder="Tìm kiếm ..."
                    />
                    <i class="bx bx-search-alt search-icon"></i>
                  </div>
                </div>
              </div>
              <div class="col-sm-8">
                <div class="text-sm-end">
                  <b-button
                      variant="primary"
                      type="button"
                      class="btn w-md btn-primary"
                      @click="handleCreate"
                      size="sm"
                  >
                    <i class="mdi mdi-plus me-1"></i> Thêm mới
                  </b-button>
                  <!-- Model create -->
                  <b-modal
                      v-model="showModal"
                      title="Tiếp nhận công văn"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="xl"
                  >
                    <form @submit.prevent="handleSubmit"
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
                                <label class="form-label" for="validationCustom01">Số công văn</label> <span
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
                                >Vui lòng thêm số công văn.</span
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
                                >Vui lòng thêm số công văn.</span
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

                            <!--                            file đính kèm-->
                            <div class="col-md-12">
                              <label for="">File đính kèm</label>
                              <vue-dropzone
                                  id="dropzone"
                                  ref="myVueDropzone"
                                  :use-custom-slot="true"
                                  :options="dropzoneOptions"
                                  v-on:vdropzone-removed-file="removeThisFile"
                                  v-on:vdropzone-success="addThisFile"
                              >
                                <div class="dropzone-custom-content">
                                  <i
                                      class="display-1 text-muted bx bxs-cloud-upload"
                                      style="font-size: 70px"
                                  ></i>
                                  <h4>
                                    Kéo thả tệp tin hoặc bấm vào để tải tệp tin
                                  </h4>
                                </div>
                              </vue-dropzone>
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
                        <b-button variant="light" class="w-md" size="sm" @click="showModal = false">
                          Đóng
                        </b-button>
                        <b-button type="submit" variant="primary" size="sm" class="ms-1 w-md">
                          Lưu
                        </b-button>
                      </div>
                    </form>
                  </b-modal>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-12">
                <!--            Hiển thị dòng-->
                <div class="row mb-2">
                  <div class="col-sm-12 col-md-6">
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
                </div>
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
                    <template v-slot:cell(loaiVanBan)="data">
                      <span v-if="data.item.loaiVanBan"> {{ data.item.loaiVanBan.ten }}</span>
                    </template>
                    <template v-slot:cell(trangThai)="data">
                      <span v-if="data.item.trangThai" class="badge bg-success"> {{ data.item.trangThai.ten }}</span>
                    </template>
                    <template v-slot:cell(trichYeu)="data">
                      <div v-if="data.item.trichYeu" :inner-html.prop="data.item.trichYeu | truncate(150)">
                      </div>
                    </template>
                    <template v-slot:cell(chuyenTrangThai)="data">

                      <b-button
                          v-if="data.item.ower && data.item.ower.userName == currentUserName
                          && (data.item.trangThai.code == 'KTVBD')"
                          type="button"
                          size="sm"
                          class="btn btn-light btn-danger"
                          data-toggle="tooltip" data-placement="bottom" title="Xử lý văn bản"
                          v-on:click="handleChuyenTrangThai(data.item.trangThai,data.item.id)">
                        <i class="fas fa-exchange-alt  me-1"></i>
                        Xử lý VB

                      </b-button>
                    </template>
                    <template v-slot:cell(process)="data">
                      <div class="d-flex justify-content-around">
                        <!--                        <button-->
                        <!--                            type="button"-->
                        <!--                            size="sm"-->
                        <!--                            class="btn btn-outline btn-sm p-0"-->
                        <!--                            data-toggle="tooltip" data-placement="bottom" title="Chi tiết"-->
                        <!--                            v-on:click="handleDetail(data.item.id)">-->
                        <!--                          <i class="fas fa-eye  text-warning me-1"></i>-->
                        <!--                        </button>-->
                        <button
                            type="button"
                            size="sm"
                            class="btn btn-outline btn-sm p-0"
                            data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                            v-on:click="handleUpdate(data.item.id)">
                          <i class="fas fa-pencil-alt text-success me-1"></i>
                        </button>
                        <!--                        <button-->
                        <!--                            type="button"-->
                        <!--                            size="sm"-->
                        <!--                            class="btn btn-outline btn-sm p-0"-->
                        <!--                            data-toggle="tooltip" data-placement="bottom" title="Cập nhật"-->
                        <!--                            v-on:click="HandleShowPhanCong(data.item.id)">-->
                        <!--                          <i class="fas fa-user-plus text-info me-1"></i>-->
                        <!--                        </button>-->
                        <button
                            type="button"
                            size="sm"
                            class="btn btn-outline btn-sm p-0"
                            data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                            v-on:click="handleShowButPhe(data.item.id)">
                          <i class="fas fa-feather-alt text-primary me-1"></i>
                        </button>
                        <button
                            type="button"
                            size="sm"
                            class="btn btn-outline btn-sm p-0"
                            data-toggle="tooltip" data-placement="bottom" title="Xóa"
                            v-on:click="handleShowDeleteModal(data.item.id)">
                          <i class="fas fa-trash-alt text-danger me-1"></i>
                        </button>
                      </div>
                    </template>
                    <template v-slot:cell(ten)="data">&nbsp;&nbsp;
                      {{ data.item.ten }}
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
          </div>
        </div>
        <!--        Trạng thái -->
        <b-modal
            v-model="showTrangThaiModal"
            centered
            title="Xử lý văn bản"
            title-class="font-18"
            no-close-on-backdrop
            size="lg"
            ref="formContainerTrangThai"
        >
          <div class="row">
            <div class="col-md-6">

              <div v-if="modelTrangThai.currentTrangThai" class="mb-2">
                <label class="form-label" for="validationCustom01">TT Hiện tại</label> <span
                  class="text-danger">*</span>
                <input
                    id="validationCustom01"
                    :value="modelTrangThai.currentTrangThai.ten"
                    type="text"
                    class="form-control"
                    placeholder=""
                    disabled
                />
              </div>

            </div>
            <div class="col-md-6">

              <div class="mb-2">
                <label class="form-label" for="validationCustom01">TT Tiếp theo</label> <span
                  class="text-danger">*</span>
                <multiselect
                    v-model="modelTrangThai.newTrangThai"
                    :options="optionsTrangThai"
                    track-by="code"
                    label="ten"
                    placeholder="Chọn trạng thái"
                    :class="{
                                'is-invalid':
                                  submitted && $v.modelTrangThai.newTrangThai.$error,
                                }"
                ></multiselect>
                <div
                    v-if="submitted && !$v.modelTrangThai.newTrangThai.required"
                    class="invalid-feedback"
                >
                  Trạng thái không được để trống.
                </div>
              </div>
            </div>
          </div>
          <template #modal-footer>
            <b-button v-b-modal.modal-close_visit
                      size="sm"
                      class="btn btn-outline-info w-md"
                      v-on:click="showTrangThaiModal = false">
              Đóng
            </b-button>
            <b-button v-if="modelTrangThai.newTrangThai && modelTrangThai.newTrangThai.code == 'tlddv'"
                      v-b-modal.modal-close_visit
                      size="sm"
                      variant="primary"
                      type="button"
                      class="w-md"
                      :disabled="modelTrangThai.lanhDaoDonVi == null"
                      v-on:click="handleChuyenTrangThaiVanBan(null)">
              Trình lãnh đạo đơn vị
            </b-button>
            <b-button v-else v-b-modal.modal-close_visit
                      size="sm"
                      variant="primary"
                      type="button"
                      class="w-md"
                      v-on:click="handleChuyenTrangThaiVanBan(null)">
              Chuyển trạng thái
            </b-button>
          </template>
        </b-modal>
        <!--        modal tran thai van ban -->
        <b-modal
            v-model="showCheckVanBanModal"
            centered
            title="Chi tiết VB"
            title-class="font-18"
            no-close-on-backdrop
            size="lg"
            ref="refshowCheckVanBanModal"
            hide-footer
        >
          <!--          <div class="row" style="width: 100%; margin: 0">-->
          <!--            <div class="col-md-6">-->
          <!--              <div class="row">-->
          <!--                <div class="col-md-12 capso-container">-->
          <!--                  <div class="title-capso">Số lưu CV</div>-->
          <!--                  <div class="content-capso">{{ model.soLuuCV }}</div>-->
          <!--                </div>-->
          <!--                <div class="col-md-12 capso-container">-->
          <!--                  <div class="title-capso"> Ngày nhập công văn</div>-->
          <!--                  <div class="content-capso">{{ model.ngayNhap }}</div>-->
          <!--                </div>-->

          <!--                <div class="col-md-12 capso-container">-->
          <!--                  <div class="title-capso"> Ngày ký</div>-->
          <!--                  <div class="content-capso">{{ model.ngayKy }}</div>-->
          <!--                </div>-->
          <!--                <div class="col-md-12 capso-container">-->
          <!--                  <div class="title-capso"> Trích yếu</div>-->
          <!--                  <div class="content-capso">-->
          <!--                    <div v-if="model.trichYeu" :inner-html.prop="model.trichYeu | truncate(150)">-->
          <!--                    </div>-->
          <!--                  </div>-->
          <!--                  &lt;!&ndash;                <div class="col-md-12 capso-container">&ndash;&gt;-->
          <!--                  &lt;!&ndash;                  <div class="title-capso"> Ngày ký</div>&ndash;&gt;-->
          <!--                  &lt;!&ndash;                  <div class="content-capso">{{model.ngayKy}}</div>&ndash;&gt;-->
          <!--                  &lt;!&ndash;                </div>&ndash;&gt;-->
          <!--                </div>-->
          <!--              </div>-->
          <!--            </div>-->
          <!--            <div class="col-md-6">-->
          <!--              <div class="row">-->
          <!--                <div class="col-md-12 capso-container">-->
          <!--                  <div class="title-capso"> Loại văn bản</div>-->
          <!--                  <template v-if="model.loaiVanBan">-->
          <!--                    <div class="content-capso">{{ model.loaiVanBan.ten }}</div>-->
          <!--                  </template>-->
          <!--                </div>-->
          <!--                <div class="col-md-12 capso-container">-->
          <!--                  <div class="title-capso"> Trạng thái</div>-->
          <!--                  <template v-if="model.trangThai">-->
          <!--                    <div class="content-capso">{{ model.trangThai.ten }}</div>-->
          <!--                  </template>-->
          <!--                </div>-->

          <!--              </div>-->

          <!--            </div>-->
          <!--            <div class="col-md-12" style="padding: 0px">-->
          <!--              <h5 style="font-weight: bold">Duyệt thể thức văn bản</h5>-->
          <!--              <div class="col-md-12 capso-container" style="display: flex; flex-direction: column">-->
          <!--                <div class="title-capso">Tện tin dính kèm của người soạn</div>-->
          <!--                <ul v-if="model.file && model.file.length > 0" style="padding-left: 30px">-->
          <!--                  <li class="title-capso" style="font-weight: normal" v-for="(value, index) in model.file" :key="index">-->

          <!--                    <a-->
          <!--                        :href="`${apiUrl}files/view/${value.fileId}`"-->
          <!--                        class=" fw-medium"-->
          <!--                    ><i-->
          <!--                        :class="`mdi font-size-16 align-middle me-2`"-->
          <!--                    ></i>-->
          <!--                      [Tải về]</a-->
          <!--                    >-->
          <!--                    <span style="padding-left: 20px">{{ value.fileName }}</span>-->
          <!--                  </li>-->
          <!--                </ul>-->

          <!--              </div>-->
          <!--              <div class="mb-3">-->
          <!--                <label class="form-label title-capso">Ghi chú</label>-->
          <!--                <div>-->
          <!--                  <textarea-->
          <!--                      v-model="modelTrangThai.noiDung"-->
          <!--                      class="form-control"-->
          <!--                      name="textarea"-->

          <!--                  ></textarea>-->
          <!--                </div>-->
          <!--              </div>-->

          <!--            </div>-->

          <!--          </div>-->
          <!--          <template #modal-header="{  }">-->
          <!--            &lt;!&ndash; Emulate built in modal header close button action &ndash;&gt;-->
          <!--            <h5 style="min-width: 200px"> Văn bản đi</h5>-->
          <!--            <div style="width: 100%; display: flex; justify-content: flex-end" class="text-end">-->
          <!--              <div v-if="optionsTrangThai && optionsTrangThai.length" style="display: flex">-->
          <!--                <div v-for="(value, index) in optionsTrangThai" :key="index" >-->

          <!--                  <b-button v-if="value.code == 'TLKSPL'" type="button" :class="'btn-' + value.bgColor" class="ms-1"-->
          <!--                            style="min-width: 80px;" size="sm"-->
          <!--                            @click="handleKySoPhapLy(model.id, true)"-->
          <!--                  >-->
          <!--                    {{ value.ten }}-->
          <!--                  </b-button>-->
          <!--                  <b-button v-else type="button" :class="'btn-' + value.bgColor" class="ms-1" style="min-width: 80px;"-->
          <!--                            size="sm"-->
          <!--                            @click="handleChuyenTrangThaiVanBan(value)"-->
          <!--                  >-->
          <!--                    {{ value.ten }}-->
          <!--                  </b-button>-->
          <!--                </div>-->

          <!--              </div>-->

          <!--              <b-button variant="light" class="ms-1" size="sm" style="width: 80px"-->
          <!--                        @click="showCheckVanBanModal = false">-->
          <!--                Đóng-->
          <!--              </b-button>-->
          <!--            </div>-->
          <!--          </template>-->
        </b-modal>

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