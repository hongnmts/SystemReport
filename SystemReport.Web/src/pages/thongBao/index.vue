<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";
import {pagingModel} from "@/models/pagingModel";
import Sidepanel from "./sidepanel";
import {userModel} from "@/models/userModel";
import {thongBaoModel} from "@/models/thongBaoModel";
import {CONSTANTS} from "@/helpers/constants";
import {vanBanDenModel} from "@/models/vanBanDenModel";
import Multiselect from "vue-multiselect";
import CKEditor from "@ckeditor/ckeditor5-vue";
import Switches from "vue-switches";
import DatePicker from "vue2-datepicker";
import vue2Dropzone from "vue2-dropzone";
import Treeselect from "@riophae/vue-treeselect";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import {vanBanDiModel} from "@/models/vanBanDiModel";
import {notifyModel} from "@/models/notifyModel";

export default {
  page: {
    title: "Thông báo",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {
    Layout, PageHeader,
    Multiselect,
    ckeditor: CKEditor.component,
    DatePicker,
  },
  data() {
    return {
      url: process.env.VUE_APP_API_URL + "files/view/",
      title: "Thông báo",
      items: [
        {
          text: "Thông báo",
          href: '/thong-bao'
        },
        {
          text: "Danh sách thông báo",
          active: true,
        }
      ],
      editor: ClassicEditor,
      editorConfig: {
        height: '200px'
      },
      data: [],
      currentPage: 1,
      perPage: 50,
      pageOptions: [ 50, 80, 100],
      showModal: false,
      showDeleteModal: false,
      submitted: false,
      sortBy: "age",
      isBusy: false,
      filter: null,
      sortDesc: false,
      filterOn: [],
      todoFilter: null,
      todofilterOn: [],
      numberOfElement: 1,
      totalRows: 1,
      model: thongBaoModel.baseJson(),
      modelVBDen: vanBanDenModel.baseJson(),
      modelVBDi: vanBanDiModel.baseJson(),
      modelUser: userModel.baseJson(),
      pagination: pagingModel.baseJson(),
      fields: [
        {
          key: 'STT',
          label: 'STT',
          thStyle: {width: '50px', minWidth: '50px'},
          class: "text-center text-primary",
          thClass: 'hidden-sortable text-primary bold',
        },
        {
          key: "title",
          label: "Tiêu đề",
          class: "px-2 text-primary",
          sortable: true,
          thClass: 'hidden-sortable text-primary bold',
        },
        {
          key: "sender",
          label: "Người tạo",
          thStyle: {width: '160px', minWidth: '160px'},
          class: "text-center px-1 text-primary",
          thClass: 'hidden-sortable text-primary bold',
        },
        {
          key: "createdAtShow",
          label: "Ngày tạo",
          thStyle: {width: '100px', minWidth: '100px'},
          class: "text-center px-1 text-primary",
          thClass: 'hidden-sortable text-primary bold',
        },
        {
          key: "read",
          label: "Trạng thái",
          thStyle: {width: '100px', minWidth: '100px'},
          class: "text-center px-1 text-primary ",
          thClass: 'hidden-sortable text-primary bold',
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: "text-center text-primary",
          thStyle: {width: '110px', minWidth: '110px'},
          thClass: 'hidden-sortable text-primary bold',
        }
      ],
      showModalVBDen: false,
      showModalVBDi: false,
      optionsLoaiVanBan: [],
      optionsDonVi: [],
      optionsTreeDonVi: [],
      optionsKhoiCoQuan: [],
      optionsLinhVuc: [],
      optionsUser: [],
      optionsHinhThucNhan: [],
      optionsMucDo: [],
      optionsTrangThai: [],
      apiUrl: process.env.VUE_APP_API_URL,
      url1: `${process.env.VUE_APP_API_URL}files/upload`,
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
    };
  },
  validations: {},
  created() {
    this.$refs.tblList?.refresh()
  },
  watch: {
    currentPage: {
      deep: true,
      handler(val) {
        this.currentPage = val;
      }
    }
  },
  methods: {
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
    async handleChangeStatus(id) {
      this.model.id = id;
      await this.$store.dispatch("notifyStore/changeStatus", this.model).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.$refs.tblList?.refresh()
        }
      });
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
        let promise = this.$store.dispatch("notificationStore/getPagingParams", params)
        return promise.then(resp => {
          if (resp.resultCode == CONSTANTS.SUCCESS) {
            let data = resp.data;
            this.totalRows = data.totalRows
            let items = data.data
            this.numberOfElement = items.length
            this.loading = false
            return items || []
          } else {
            return [];
          }
        })
      } finally {
        this.loading = false
      }
    },
    async handleDetail(id) {
      await this.$store.dispatch("notificationStore/getById", id).then((res) => {
        this.model = res;
        this.showModal = true;
      });
    },
    async handleDetailUser(id) {
      await this.$store.dispatch("userStore/getById", id).then((res) => {
        this.modelUser = userModel.fromJson(res.data);
        this.showModal = true;
      });
    },
    handleLuuCV(loaiCongVan, congVanId) {
      if (loaiCongVan && congVanId) {
        if (loaiCongVan == 2) {
          this.handleGetCongVanDi(congVanId);
        } else if (loaiCongVan == 1) {
          this.handleGetCongVanDen(congVanId);
        }
      }
    },
    async handleGetCongVanDi(id) {
      await this.$store.dispatch("vanBanDiStore/getById", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.modelVBDi = res.data;
          this.showModalVBDi = true;
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleGetCongVanDen(id) {
      await this.$store.dispatch("vanBanDenStore/getById", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.modelVBDen = res.data;
          this.showModalVBDen = true;
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
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
    async handleSubmitLuuCV(id) {
      let loader = this.$loading.show({
        container: this.$refs.formContainer,
      });
      await this.$store.dispatch("notifyStore/luuCVNoiBo", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.showModalVBDi = false;
          this.showModalVBDen = false;
          this.modelVBDi = vanBanDiModel;
          this.modelVBDen = vanBanDenModel;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
      });
      loader.hide();
    }
  }
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <!-- Right Sidebar -->
      <div class="col-12">
        <!--        <Sidepanel />-->
        <div class=" mb-3">
          <div class="card">
            <div class="card-body">
<!--              <div class="row mb-2">-->
<!--                <div class="col-sm-4">-->
<!--                  <div class="search-box me-2 mb-2 d-inline-block">-->
<!--                    <div class="position-relative">-->
<!--                      <input-->
<!--                          v-model="filter"-->
<!--                          type="text"-->
<!--                          class="form-control"-->
<!--                          placeholder="Tìm kiếm ..."-->
<!--                      />-->
<!--                      <i class="bx bx-search-alt search-icon"></i>-->
<!--                    </div>-->
<!--                  </div>-->
<!--                </div>-->
<!--              </div>-->
              <div class="row">
                <div class="col-12">
<!--                  <div class="row mb-2">-->
<!--                    <div class="col-sm-12 col-md-6">-->
<!--                      <div id="tickets-table_length" class="dataTables_length">-->
<!--                        <label class="d-inline-flex align-items-center">-->
<!--                          Hiện-->
<!--                          <b-form-select-->
<!--                              class="form-select form-select-sm"-->
<!--                              v-model="perPage"-->
<!--                              size="sm"-->
<!--                              :options="pageOptions"-->
<!--                          ></b-form-select-->
<!--                          >&nbsp;dòng-->
<!--                        </label>-->
<!--                      </div>-->
<!--                    </div>-->
<!--                  </div>-->
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
                      <template v-slot:cell(title)="data">
                        <div @click="handleDetail(data.item.id), handleChangeStatus(data.item.id)"
                             style="cursor: pointer">
                          {{ data.item.title }}
                        </div>

                      </template>
                      <template v-slot:cell(read)="data">
                        <span class="badge bg-success" v-if="data.item.read">Đã đọc</span>
                        <span class="badge bg-warning" v-else>Chưa đọc</span>
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
        </div>
      </div>
    </div>
    <b-modal
        v-model="showModal"
        id="modal-lg"
        size="lg"
        body-class="p-0"
        hide-footer
        no-close-on-backdrop
    >
      <template #modal-header="{ }">
        <!-- Emulate built in modal header close button action -->
        <h5> Thông báo</h5>
        <div class="text-end">
          <b-button v-if="model.loaiCongVan && model.congVanId" variant="info" size="sm"
                    style="width: 120px; margin-right: 10px" @click="handleLuuCV(model.loaiCongVan, model.congVanId)">
            Lưu CV nội bộ
          </b-button>
          <b-button variant="light" size="sm" style="width: 80px" @click="showModal = false">
            Đóng
          </b-button>
        </div>
      </template>
      <div class="card-body pb-0 pt-0">
        <div class="row">
          <div class="col-12">
            <div class="bg-notify position-relative d-flex justify-content-center">
              <img src="~@/assets/images/bg-notify.png" alt="bg notify" width="150px">
              <div class="icon-notify position-absolute d-flex justify-content-center align-items-center">
                <img src="~@/assets/images/icon-bell.png" alt="icon notify" width="100px">
              </div>
            </div>
          </div>
        </div>
        <div class="row info-box">
          <div class="col-md-4 col-12 d-flex align-items-center sender-box">
            <div>
              <span v-if="modelUser.avatar">
                <img
                    :src="url + `${modelUser.avatar.fileId}`"
                    alt="Avatar"
                    class="d-flex me-3 rounded-circle avatar-sm"
                />

              </span>
              <span v-else>
                <img
                    class="d-flex me-2 rounded-circle avatar-sm"
                    src="@/assets/images/4.png"
                    alt="Avatar"
                />
              </span>
            </div>
            <div class="d-flex">
              <div class="flex-grow-1">
                <h5 class="font-size-14 opacity-50">NGƯỜI GỬI: </h5>
                <p class="m-0 text-dark">{{ model.sender }}</p>
              </div>
            </div>
          </div>
          <div class="col-md-4 col-12 recipient-box">
            <div class="d-flex">
              <div class="flex-grow-1">
                <h5 class="font-size-14 opacity-50">CC: </h5>
                <p class="m-0 badge text-success px-1 font-size-13"
                   style="background-color: rgba(2,164,153,0.2); border-radius: 3px;"
                >{{ model.recipient }}</p>
              </div>
            </div>
          </div>
          <div class="col-md-4 col-12">
            <div class="flex-grow-2 p-1">
              <small class="text-muted" style="float: right"><i>Ngày gửi: {{ model.createdAtShow }}
                {{ model.createdAtTimeShow }}</i></small>
            </div>
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-12">
            <h4 class="mt-0 font-size-16" style="font-weight: bold;color: #00568C">{{ model.title }}</h4>
            <p
                style="color: #00568C; text-align: justify;"
                v-if="model.content"
                :inner-html.prop="model.content"
                class="m-0"
            >
            </p>
          </div>
        </div>
        <div class="row mb-3">
          <div class="col-md-12"
               style="display: flex; flex-direction: column; padding: 0px"
          >
            <div
                v-if="model.files && model.files.length > 0"
            >
              <div
                  v-for="(value, index) in model.files" :key="index"
                  style="background-color: rgba(47,147,224,0.2); border-radius: 3px; padding: 5px; margin-bottom: 5px"
              >
                <a
                    :href="`${apiUrl}files/view/${value.fileId}`"
                    class=" fw-medium"
                ><i
                    class="fas fa-cloud-download-alt"
                ></i>
                  [Tải về]</a
                >
                <span style="padding-left: 20px; font-weight: 500; color: #2F93E0">{{ value.fileName }}</span>
              </div>
            </div>
          </div>
        </div>

      </div>
    </b-modal>

    <!--    Van ban den-->
    <b-modal
        v-model="showModalVBDen"
        title="Lưu công văn nội bộ"
        title-class="text-black font-18"
        body-class="p-3"
        hide-footer
        centered
        no-close-on-backdrop
        size="xl"
    >
      <template #modal-header="{  }">
        <!-- Emulate built in modal header close button action -->
        <h5> Lưu công văn nội bộ</h5>
        <div class="text-end">
          <b-button variant="light" size="sm" style="width: 80px" @click="showModalVBDen = false">
            Đóng
          </b-button>
          <b-button type="submit" variant="primary" class="ms-1" style="width: 80px"
                    size="sm"
                    @click="handleSubmitLuuCV(model.id)"
          >
            Lưu
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
                      v-model="modelVBDen.soLuuCV"
                      type="text"
                      class="form-control"
                      placeholder="eg: 123-dthu"
                  />

                </div>
              </div>
              <!--                            Số VB đến -->
              <div class="col-md-5">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Số văn bản đến</label> <span
                    class="text-danger">*</span>
                  <input
                      id="validationSoVBDen"
                      v-model="modelVBDen.soVBDen"
                      type="text"
                      class="form-control"
                      placeholder="eg: 123-dthu"
                  />
                </div>
              </div>
              <!--                            Loại văn bản-->
              <div class="col-md-4">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Loại văn bản</label> <span
                    class="text-danger">*</span>
                  <multiselect
                      v-model="modelVBDen.loaiVanBan"
                      :options="optionsLoaiVanBan"
                      track-by="id"
                      label="ten"
                      placeholder="Chọn trạng thái"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                  ></multiselect>
                </div>
              </div>
              <!--                            Trích yếu -->
              <div class="col-md-12">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Trích yếu</label> <span
                    class="text-danger">*</span>
                  <ckeditor
                      v-model="modelVBDen.trichYeu"
                      :editor="editor"
                      :config="editorConfig"
                  ></ckeditor>
                </div>
              </div>
              <!--                            Ngày ban hành -->
              <div class="col-md-6">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Ngày ban hành</label>
                  <date-picker v-model="modelVBDen.ngayBanHanh"
                               format="DD/MM/YYYY"
                               value-type="format"
                  >
                    <div slot="input">
                      <input v-model="modelVBDen.ngayBanHanh"
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
                  <date-picker v-model="modelVBDen.ngayNhan"
                               format="DD/MM/YYYY"
                               value-type="format"
                  >
                    <div slot="input">
                      <input v-model="modelVBDen.ngayNhan"
                             v-mask="'##/##/####'" type="text" class="form-control"
                             placeholder="Nhập ngày nhận"/>
                    </div>
                  </date-picker>
                </div>
              </div>
              <div v-if="modelVBDen.id">
                <div v-if="modelVBDen.file != null  && modelVBDen.file.length > 0" class="col-md-12">
                  <label for="">Danh sách đã ký (Nhấn vào để tải xuống)</label>
                  <div
                      class=" p-1"
                  >
                    <template>
                      <div v-for="(file, index) in modelVBDen.file" :key="index">
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
                  <date-picker v-model="modelVBDen.ngayKy"
                               format="DD/MM/YYYY"
                               value-type="format"
                  >
                    <div slot="input">
                      <input v-model="modelVBDen.ngayKy"
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
                      v-model="modelVBDen.nguoiKy"
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
              <!--                            Khối cơ quan gửi-->
              <div class="col-md-12">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Khối cơ quan gửi</label>
                  <multiselect
                      v-model="modelVBDen.khoiCoQuanGui"
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
                      v-model="modelVBDen.coQuanGui"
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
                      v-model="modelVBDen.hinhThucNhan"
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
                      v-model="modelVBDen.linhVuc"
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
                      v-model="modelVBDen.mucDoTinhChat"
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
                      v-model="modelVBDen.mucDoBaoMat"
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
                      v-model="modelVBDen.hoSoDonVi"
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
                      v-model="modelVBDen.noiLuuTru"
                      type="text"
                      class="form-control"
                      placeholder="Nhập nơi lưu trữ"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </form>
    </b-modal>

    <b-modal
        v-model="showModalVBDi"
        title="Lưu công văn nội bộ"
        title-class="text-black font-18"
        body-class="p-3"
        hide-footer
        centered
        no-close-on-backdrop
        size="xl"
    >
      <template #modal-header="{  }">
        <!-- Emulate built in modal header close button action -->
        <h5>Lưu công văn nội bộ</h5>
        <div class="text-end">
          <b-button variant="light" size="sm" style="width: 80px" @click="showModalVBDi = false">
            Đóng
          </b-button>

          <b-button type="submit" variant="primary" class="ms-1" style="width: 80px"
                    size="sm"
                    @click="handleSubmitLuuCV(model.id)"
          >
            Lưu
          </b-button>
        </div>
      </template>
      <form
          ref="formContainer">
        <div class="row">
          <div class="col-md-7">
            <div class="row">
              <!--                            Loại văn bản-->
              <div class="col-md-6">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Loại văn bản</label> <span
                    class="text-danger">*</span>
                  <multiselect
                      v-model="modelVBDi.loaiVanBan"
                      :options="optionsLoaiVanBan"
                      track-by="id"
                      label="ten"
                      placeholder="Chọn loại văn bản"
                  ></multiselect>
                </div>
              </div>
              <!--                            Trạng thái-->
              <div class="col-md-6">

                <div v-if="modelVBDi.trangThai && modelVBDi.id" class="mb-2">
                  <label class="form-label" for="validationCustom01">Trạng thái</label> <span
                    class="text-danger">*</span>
                  <input
                      id="validationCustom01"
                      :value="modelVBDi.trangThai.ten"
                      type="text"
                      class="form-control"
                      placeholder=""
                      disabled
                  />
                </div>

                <div v-else class="mb-2">
                  <label class="form-label" for="validationCustom01">Trạng thái</label> <span
                    class="text-danger">*</span>
                  <multiselect
                      v-model="modelVBDi.trangThai"
                      :options="optionsTrangThai"
                      track-by="id"
                      label="ten"
                      placeholder="Chọn trạng thái"
                  ></multiselect>
                </div>
              </div>
              <!--                              Số lưu -->
              <div class="col-md-4">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Số lưu CV</label>
                  <!--                                <span-->
                  <!--                                  class="text-danger">*</span>-->
                  <input
                      id="validationCustom01"
                      v-model="modelVBDi.soLuuCV"
                      type="text"
                      class="form-control"
                      placeholder=""
                  />
                </div>
              </div>
              <!--                            Số VB đến -->
              <div class="col-md-4">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Số CV đi</label>
                  <!--                                <span-->
                  <!--                                  class="text-danger">*</span>-->
                  <input
                      id="validationSoVBDen"
                      v-model="modelVBDi.soVBDi"
                      type="text"
                      class="form-control"
                      placeholder=""

                  />
                </div>
              </div>
              <!--                            Ngày nhận -->
              <div class="col-md-4">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Ngày nhập</label>
                  <!--                                <date-picker-->
                  <!--                                    v-model="model.ngayNhap"-->
                  <!--                                    format="DD/MM/YYYY"-->
                  <!--                                    :first-day-of-week="1"-->
                  <!--                                    lang="en"-->
                  <!--                                    placeholder="Chọn ngày nhập"-->
                  <!--                                ></date-picker>-->
                  <date-picker v-model="modelVBDi.ngayNhap"
                               format="DD/MM/YYYY"
                               value-type="format"
                  >
                    <div slot="input">
                      <input v-model="modelVBDi.ngayNhap"
                             v-mask="'##/##/####'" type="text" class="form-control"
                             placeholder="Nhập ngày nhập"/>
                    </div>
                  </date-picker>
                </div>
              </div>
              <!--                              Số lưu -->
              <div class="col-md-4">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01"> Trả lời CV số</label>
                  <!--                                <span-->
                  <!--                                  class="text-danger">*</span>-->
                  <input
                      id="validationCustom01"
                      v-model="modelVBDi.traLoiCVSo"
                      type="text"
                      class="form-control"
                      placeholder=""
                  />
                </div>
              </div>
              <!--                            Ngày nhận -->
              <div class="col-md-4">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01"> Ngày trả lời</label>
                  <date-picker v-model="modelVBDi.ngayTraLoi"
                               format="DD/MM/YYYY"
                               value-type="format"
                  >
                    <div slot="input">
                      <input v-model="modelVBDi.ngayTraLoi"
                             v-mask="'##/##/####'" type="text" class="form-control"
                             placeholder="Nhập ngày trả lời"/>
                    </div>
                  </date-picker>
                </div>
              </div>
              <!--                            Số VB đến -->
              <div class="col-md-4">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01"> Số bản</label>
                  <!--                                <span-->
                  <!--                                  class="text-danger">*</span>-->
                  <input
                      id="validationSoVBDen"
                      v-model="modelVBDi.soBan"
                      type="number"
                      class="form-control"
                      placeholder=""

                  />
                </div>
              </div>
              <div class="col-md-6">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01"> Người ký</label>
                  <multiselect
                      v-model="modelVBDi.nguoiKy"
                      :options="optionsUser"
                      track-by="id"
                      label="fullName"
                      placeholder="Chọn cán bộ soạn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                  >
                    <template slot="singleLabel" slot-scope="{ option }">
                      <strong>{{ option.fullName }}</strong>

                      <span v-if="option.donVi" style="color:red">&nbsp;{{ option.donVi.ten }}</span>
                    </template>
                    <template slot="option" slot-scope="{ option }">
                      <div class="option__desc">
          <span class="option__title">
            <strong>{{ option.fullName }}&nbsp;</strong>
          </span>
                        <span v-if="option.donVi" class="option__small"
                              style="color:green">{{ option.donVi.ten }}</span>
                      </div>
                    </template>
                  </multiselect>
                </div>
              </div>
              <!--                            Ngày Ký-->
              <div class="col-md-6">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Ngày ký</label>
                  <date-picker v-model="modelVBDi.ngayKy"
                               format="DD/MM/YYYY"
                               value-type="format"
                  >
                    <div slot="input">
                      <input v-model="modelVBDi.ngayKy"
                             v-mask="'##/##/####'" type="text" class="form-control"
                             placeholder="Nhập ngày ký"/>
                    </div>
                  </date-picker>
                </div>
              </div>
              <!--                            Trích yếu -->
              <div class="col-md-12">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Trích yếu</label>
                  <!--                                <span-->
                  <!--                                  class="text-danger">*</span>-->
                  <ckeditor
                      v-model="modelVBDi.trichYeu"
                      :editor="editor"
                      :config="editorConfig"
                  ></ckeditor>
                </div>
              </div>
              <div v-if="modelVBDi.filePDF != null  && modelVBDi.filePDF.length > 0" class="col-md-12">
                <label for="">Danh sách đã ký (Nhấn vào để tải xuống)</label>
                <template>
                  <div v-for="(file, index) in model.filePDF" :key="index">
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
              <div class="col-md-12">
                <label for="">Danh sách tệp tin (Nhấn vào để tải xuống)</label>
                <template v-if="modelVBDi.file == null || (modelVBDi.file != null &&modelVBDi.file.length <= 0)">
                  <div>Không có tệp tin</div>
                </template>
                <template v-else>
                  <div v-for="(file, index) in modelVBDi.file" :key="index">
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

          <div class="col-md-5">
            <div class="row">
              <div class="col-md-12">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01"> Đơn vị soạn</label>
                  <multiselect
                      v-model="modelVBDi.donViSoan"
                      :options="optionsDonVi"
                      track-by="id"
                      label="ten"
                      placeholder="Chọn đơn vị soạn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                  ></multiselect>

                </div>
              </div>
              <!--                            Cơ quan gửi-->
              <div class="col-md-12">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01"> Cán bộ soạn</label>
                  <multiselect
                      v-model="modelVBDi.canBoSoan"
                      :options="optionsUser"
                      track-by="id"
                      label="fullName"
                      placeholder="Chọn cán bộ soạn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                  >
                    <template slot="singleLabel" slot-scope="{ option }">
                      <strong>{{ option.fullName }}</strong>

                      <span v-if="option.donVi" style="color:red">&nbsp;{{ option.donVi.ten }}</span>
                    </template>
                    <template slot="option" slot-scope="{ option }">
                      <div class="option__desc">
          <span class="option__title">
            <strong>{{ option.fullName }}&nbsp;</strong>
          </span>
                        <span v-if="option.donVi" class="option__small"
                              style="color:green">{{ option.donVi.ten }}</span>
                      </div>
                    </template>
                  </multiselect>
                </div>
              </div>
              <!--                            Hình thức nhận -->
              <div class="col-md-6">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Hình thức nhận</label>
                  <multiselect
                      v-model="modelVBDi.hinhThucGui"
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
                      v-model="modelVBDi.linhVuc"
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
                      v-model="modelVBDi.mucDoTinhChat"
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
                      v-model="modelVBDi.mucDoBaoMat"
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
                      v-model="modelVBDi.hoSoDonVi"
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
                      v-model="modelVBDi.noiLuuTru"
                      type="text"
                      class="form-control"
                      placeholder="Nhập nơi lưu trữ"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </form>
    </b-modal>
  </Layout>

</template>
<style scoped>
.td-stttt {
  text-align: center;
  width: 10px;
}

.td-tb {
  text-align: left;
  width: 250px;
}

.td-content {
  text-align: center;
  width: 250px;
}

.td-nguoitao {
  text-align: center;
  width: 80px;
}

.td-trangthai {
  text-align: center;
  width: 50px;
}

.td-xuly {
  text-align: center;
  width: 50px;
}

.table > tbody > tr > td {
  padding: 0px;
  line-height: 30px;
}

.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}

.title-thongbao {
  color: #00568C;
  margin-right: 10px;

}

.icon-notify {
  top: 20%;
  animation: ring 2s infinite;
}

@keyframes ring {
  0% {
    transform: rotate(0);
  }
  1% {
    transform: rotate(30deg);
  }
  3% {
    transform: rotate(-28deg);
  }
  5% {
    transform: rotate(34deg);
  }
  7% {
    transform: rotate(-32deg);
  }
  9% {
    transform: rotate(30deg);
  }
  11% {
    transform: rotate(-28deg);
  }
  13% {
    transform: rotate(26deg);
  }
  15% {
    transform: rotate(-24deg);
  }
  17% {
    transform: rotate(22deg);
  }
  19% {
    transform: rotate(-20deg);
  }
  21% {
    transform: rotate(18deg);
  }
  23% {
    transform: rotate(-16deg);
  }
  25% {
    transform: rotate(14deg);
  }
  27% {
    transform: rotate(-12deg);
  }
  29% {
    transform: rotate(10deg);
  }
  31% {
    transform: rotate(-8deg);
  }
  33% {
    transform: rotate(6deg);
  }
  35% {
    transform: rotate(-4deg);
  }
  37% {
    transform: rotate(2deg);
  }
  39% {
    transform: rotate(-1deg);
  }
  41% {
    transform: rotate(1deg);
  }

  43% {
    transform: rotate(0);
  }
  100% {
    transform: rotate(0);
  }
}
/*Thông báo*/

.info-box {
  background-color: #f5f5f5;
  padding: 5px;
  border-radius: 5px;
  border: 0.5px solid #f1f1f1;
  margin-bottom: 5px;
}

@media only screen and (max-width: 425px){
  .sender-box,
  .recipient-box{
    background-color: #f5f5f5;
    border: 0.5px solid #f5f5f5;
    border-radius: 5px;
    padding: 5px;
    margin-bottom: 2px;
  }
}

</style>
