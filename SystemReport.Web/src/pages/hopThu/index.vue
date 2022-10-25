<script>
import Layout from "../../layouts/main";

import PageHeader from "@/components/page-header";
import {required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {notifyModel} from "@/models/notifyModel";
import {pagingModel} from "@/models/pagingModel";
import {linhVucModel} from "@/models/linhVucModel";
import {CONSTANTS} from "@/helpers/constants";
import {hopThuModel} from "@/models/hopThuModel";
import CKEditor from "@ckeditor/ckeditor5-vue";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import Treeselect from "@riophae/vue-treeselect";
import vue2Dropzone from "vue2-dropzone";

export default {
  page: {
    title: "Thư đến",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, ckeditor: CKEditor.component, Treeselect, vueDropzone: vue2Dropzone,},
  data() {
    return {
      title: "Thư đến",
      items: [
        {
          text: "Hộp thư",
          href: "/hop-thu",
          // active: true,
        },
        {
          text: "Thư đến",
          active: true,
        }
      ],
      data: [],
      showModal: false,
      showDetail: false,
      showDeleteModal: false,
      submitted: false,
      model: hopThuModel.baseJson(),
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      todoTotalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 50,
      pageOptions: [50, 80, 100],
      filter: null,
      filterOn: [],
      isBusy: false,
      sortBy: "age",
      sortDesc: false,
      fields: [
        {
          key: 'STT',
          label: 'STT',
          class: 'td-stt title-capso',
          sortable: false,
          thClass: 'hidden-sortable',
          thStyle: {width: '80px', minWidth: '50px'},
        },
        {
          key: "tieuDe",
          label: "Tiêu đề",
          class: 'px-3 title-capso',
          sortable: true,
          thStyle: "text-align:left",
          thClass: 'hidden-sortable',
        },
        {
          key: "nguoiGui",
          label: "Người gửi",
          class: 'td-xuly px-1 title-capso',
          thStyle: {width: '200px', minWidth: '100px'},
          sortable: true,
          thClass: 'hidden-sortable',
        },
        {
          key: "ngayNhan",
          label: "Ngày nhận",
          class: 'td-xuly title-capso',
          sortable: true,
          thStyle: {width: '150px', minWidth: '100px'},
          thClass: 'hidden-sortable',
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: 'td-xuly',
          thStyle: {width: '80px', minWidth: '50px'},
          thClass: 'hidden-sortable',
          sortable: false
        }
      ],
      apiUrl: process.env.VUE_APP_API_URL,
      url: `${process.env.VUE_APP_API_URL}files/upload`,
      urlView: `${process.env.VUE_APP_API_URL}files/view/`,
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}files/upload`,
        thumbnailWidth: 300,
        thumbnailHeight: 160,
        maxFiles: 4,
        maxFilesize: 30,
        headers: {"My-Awesome-Header": "header value"},
        addRemoveLinks: true,
        acceptedFiles: ".doc,.docx,.pdf",
        dropzoneClassName:"dropzonevue-box"
      },
      editor: ClassicEditor,
      editorConfig: {
        height: '200px'
      },
      optionsUserTree: [],
      valueConsistsOf: 'LEAF_PRIORITY',
      showChiTietModal: false,
      showChuyenTiepModal: false
    };
  },
  validations: {
    model: {
      tieuDe: {required},
      nguoiNhans: {required},
    },
  },
  created() {
    this.fnGetList();
    this.getUserTree();
  },
  watch: {
    model: {
      deep: true,
      handler(val) {
      }
    },
  },
  methods: {
    async fnGetList() {
      this.$refs.tblList?.refresh()
    },
    async handleUpdate(id) {
      await this.$store.dispatch("dmLinhVucStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = linhVucModel.fromJson(res.data);
          this.showModal = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          this.$refs.tblList.refresh()
        }
      });
    },
    async handleDetail(id) {
      await this.$store.dispatch("dmLinhVucStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = linhVucModel.fromJson(res.data);
          this.showDetail = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("hopThuStore/delete", this.model.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showDeleteModal = false;
            this.$refs.tblList.refresh()
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        });
      }
    },
    handleShowDeleteModal(id) {
      this.model.id = id;
      this.showDeleteModal = true;
    },
    async handleSubmit(e) {
      e.preventDefault();
      this.submitted = true;
      this.$v.$touch();
      if (this.$v.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        if (
            this.model.id != 0 &&
            this.model.id != null &&
            this.model.id
        ) {
          // Update model
          await this.$store.dispatch("hopThuStore/createSend", this.model).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.showModal = false;
              this.showChuyenTiepModal = false;
              this.$refs.tblList.refresh()
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        } else {
          // Create model
          await this.$store.dispatch("hopThuStore/createSend", this.model).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.showModal = false;
              this.showChuyenTiepModal = false;
              this.$refs.tblList.refresh()
              this.model = hopThuModel.baseJson();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        }
        loader.hide();
      }
      this.submitted = false;
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
        let promise = this.$store.dispatch("hopThuStore/getPagingParams", params)
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
    async getUserTree() {
      try {
        await this.$store.dispatch("userStore/userTreeForDonVi").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsUserTree = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    normalizer(node) {
      if (node.children == null || node.children == 'null') {
        delete node.children;
      }
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
      if (this.model) {
        if (this.model.uploadFiles == null || this.model.uploadFiles.length <= 0) {
          this.model.uploadFiles = [];
        }
        let fileSuccess = response.data;
        this.model.uploadFiles.push({fileId: fileSuccess.id, fileName: fileSuccess.fileName, ext: fileSuccess.ext})
      }
    },
    async handleShowChiTietModal(id) {
      await this.$store.dispatch("hopThuStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = hopThuModel.fromJson(res.data);
          this.showChiTietModal = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    handleCreate() {
      this.model = hopThuModel.baseJson();
      this.showModal = true;
    },
    handleTraLoi(tatca) {
      this.model.tieuDe = "Re: " + this.model.tieuDe;
      this.model.noiDung = null;
      if (tatca) {
        this.model.cc = null;
      }
      this.showChiTietModal = false;
      this.showModal = true;
    },
    handleChuyenTiep() {
      this.model.cc = null
      this.model.nguoiNhans = null;
      this.showChiTietModal = false;
      this.showChuyenTiepModal = true;
    }
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
              <div class="col-sm-4">
                <div class="search-box me-2 mb-2 d-inline-block">
                  <!--                  <div class="position-relative">-->
                  <!--                    <input-->
                  <!--                        v-model="filter"-->
                  <!--                        type="text"-->
                  <!--                        class="form-control"-->
                  <!--                        placeholder="Tìm kiếm ..."-->
                  <!--                    />-->
                  <!--                    <i class="bx bx-search-alt search-icon"></i>-->
                  <!--                  </div>-->
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
                    <i class="mdi mdi-plus me-1"></i> Soạn thư
                  </b-button>
                  <!--                  Modal create -->
                  <b-modal
                      v-model="showModal"
                      title="Thêm thư mới"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="xl"
                  >
                    <template #modal-header="{  }">
                      <!-- Emulate built in modal header close button action -->
                      <h5> Thêm mới thư</h5>
                      <div class="text-end">
                        <b-button variant="light" class="w-md" size="sm" @click="showModal = false">
                          Đóng
                        </b-button>
                        <!--                        <b-button type="submit" variant="primary" size="sm" class="ms-1 w-md">-->
                        <!--                          Lưu nháp-->
                        <!--                        </b-button>-->
                        <b-button type="button" @click="handleSubmit" variant="primary" size="sm" class="ms-1 w-md">
                          Gửi thư
                        </b-button>

                      </div>
                    </template>
                    <form ref="formContainer">
                      <b-form-group
                          label-cols="10"
                          label-cols-sm="2"
                          label="Tiêu đề"
                          label-for="example-text-input"
                          label-class="form-label"
                          class="mb-3"
                      >
                        <b-form-input id="example-text-input"
                                      v-model="model.tieuDe"
                                      :class="{
                                'is-invalid':
                                  submitted && $v.model.tieuDe.$error,
                                }"
                        ></b-form-input>
                        <div
                            v-if="submitted && !$v.model.tieuDe.required"
                            class="invalid-feedback"
                        >
                          Tiêu đề thư không được để trống.
                        </div>
                      </b-form-group>

                      <b-form-group
                          label-cols="10"
                          label-cols-sm="2"
                          label="Người nhận"
                          label-for="example-email-input"
                          label-class="form-label"
                          class="mb-3"
                      >
                        <treeselect
                            v-model="model.nguoiNhans"
                            :multiple="true"
                            :options="optionsUserTree"
                            :searchable="true"
                            :value-consists-of="valueConsistsOf"
                            :normalizer="normalizer"
                            value-format="object"
                            :class="{
                                'is-invalid':
                                  submitted && $v.model.nguoiNhans.$error,
                                }"
                        >

                          <label slot="option-label"
                                 slot-scope="{ node, shouldShowCount, count, labelClassName, countClassName }"
                                 :class="labelClassName">
                            {{ node.label }}
                            <span v-if="shouldShowCount" :class="countClassName">({{ count }})</span>
                          </label>
                        </treeselect>
                        <div
                            v-if="submitted && !$v.model.nguoiNhans.required"
                            class="invalid-feedback"
                        >
                          Người nhận không được để trống.
                        </div>
                      </b-form-group>

                      <b-form-group
                          label-cols="10"
                          label-cols-sm="2"
                          label="Cc"
                          label-for="example-email-input"
                          label-class="form-label"
                          class="mb-3"
                      >
                        <treeselect
                            v-model="model.cc"
                            :multiple="true"
                            :options="optionsUserTree"

                            :searchable="true"
                            :value-consists-of="valueConsistsOf"
                            :normalizer="normalizer"
                            value-format="object"
                        >

                          <label slot="option-label"
                                 slot-scope="{ node, shouldShowCount, count, labelClassName, countClassName }"
                                 :class="labelClassName">
                            {{ node.label }}
                            <span v-if="shouldShowCount" :class="countClassName">({{ count }})</span>
                          </label>
                        </treeselect>
                      </b-form-group>

                      <b-form-group
                          label-cols="10"
                          label-cols-sm="2"
                          label="Nội dung"
                          label-for="example-url-input"
                          label-class="form-label"
                          class="mb-3"
                      >
                        <ckeditor
                            v-model="model.noiDung"
                            :editor="editor"
                            :config="editorConfig"
                        ></ckeditor>
                      </b-form-group>
                      <b-form-group
                          label-cols="10"
                          label-cols-sm="2"
                          label="Tệp tin đính kèm"
                          label-for="example-url-input"
                          label-class="form-label"
                          class="mb-3"
                      >
                        <vue-dropzone
                            id="dropzone"
                            ref="myVueDropzone"
                            :use-custom-slot="true"
                            :options="dropzoneOptions"
                            class="dropzonevue-box"
                            v-on:vdropzone-removed-file="removeThisFile"
                            v-on:vdropzone-success="addThisFile"
                        >
                          <div class="dropzone-custom-content">
                            <i
                                class="display-1 text-warning fas fa-cloud-upload-alt fs-3"
                                style="font-size: 70px"
                            ></i>
                            <h4>
                              Kéo thả tệp tin hoặc bấm vào để tải tệp tin
                            </h4>
                          </div>
                        </vue-dropzone>
                      </b-form-group>

                    </form>
                  </b-modal>
                  <!--                  Modal chuyển tiếp -->
                  <b-modal
                      v-model="showChuyenTiepModal"
                      title="Thêm thư mới"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="xl"
                  >
                    <template #modal-header="{  }">
                      <h5> Chuyển tiếp thư</h5>
                      <div class="text-end">
                        <b-button variant="light" class="w-md" size="sm" @click="showChuyenTiepModal = false">
                          Đóng
                        </b-button>
                        <b-button type="button" @click="handleSubmit" variant="primary" size="sm" class="ms-1 w-md">
                          Chuyển tiếp
                        </b-button>
                      </div>
                    </template>
                    <form
                        ref="formContainer">
                      <b-form-group
                          label-cols="10"
                          label-cols-sm="2"
                          label="Người nhận"
                          label-for="example-email-input"
                          label-class="form-label"
                          class="mb-3"
                      >
                        <treeselect
                            v-model="model.nguoiNhans"
                            :multiple="true"
                            :options="optionsUserTree"
                            :searchable="true"
                            :value-consists-of="valueConsistsOf"
                            :normalizer="normalizer"
                            value-format="object"
                            :class="{
                                'is-invalid':
                                  submitted && $v.model.nguoiNhans.$error,
                                }"
                        >

                          <label slot="option-label"
                                 slot-scope="{ node, shouldShowCount, count, labelClassName, countClassName }"
                                 :class="labelClassName">
                            {{ node.label }}
                            <span v-if="shouldShowCount" :class="countClassName">({{ count }})</span>
                          </label>
                        </treeselect>
                        <div
                            v-if="submitted && !$v.model.nguoiNhans.required"
                            class="invalid-feedback"
                        >
                          Người nhận không được để trống.
                        </div>
                      </b-form-group>
                      <b-form-group
                          label-cols="10"
                          label-cols-sm="2"
                          label="Cc"
                          label-for="example-email-input"
                          label-class="form-label"
                          class="mb-3"
                      >
                        <treeselect
                            v-model="model.cc"
                            :multiple="true"
                            :options="optionsUserTree"

                            :searchable="true"
                            :value-consists-of="valueConsistsOf"
                            :normalizer="normalizer"
                            value-format="object"
                        >

                          <label slot="option-label"
                                 slot-scope="{ node, shouldShowCount, count, labelClassName, countClassName }"
                                 :class="labelClassName">
                            {{ node.label }}
                            <span v-if="shouldShowCount" :class="countClassName">({{ count }})</span>
                          </label>
                        </treeselect>
                      </b-form-group>
                      <b-form-group
                          label-cols="10"
                          label-cols-sm="2"
                          label="Tiêu đề"
                          label-for="example-email-input"
                          label-class="form-label"
                      >
                        <div class="title-hopthu-main">
                          {{ model.tieuDe }}
                        </div>
                      </b-form-group>
                      <b-form-group
                          label-cols="10"
                          label-cols-sm="2"
                          label="Thời gian"
                          label-for="example-email-input"
                          label-class="form-label"
                      >
                        <div style="display: flex">
                          <div v-if="model.ngayNhanFull" style="margin-right: 10px; font-size: 14px"><span
                              class="title-hopthu"> Ngày gửi: </span> {{ model.ngayNhanFull }}
                          </div>
                          <div v-if="model.nguoiGui" style="margin-right: 10px; font-size: 14px">
                            <span class="title-hopthu"> Người tạo: </span> {{ model.nguoiGui.fullName }}
                            <template v-if="model.nguoiGui.donVi">
                              -{{ model.nguoiGui.donVi.ten }}
                            </template>
                          </div>
                        </div>
                      </b-form-group>
                      <b-form-group
                          label-cols="10"
                          label-cols-sm="2"
                          label="Nội dung"
                          label-for="example-email-input"
                          label-class="form-label"
                      >
                        <div v-if="model.noiDung">
                          <div :inner-html.prop="model.noiDung">
                          </div>
                        </div>
                      </b-form-group>
                      <b-form-group
                          label-cols="10"
                          label-cols-sm="2"
                          label="Danh sách tệp tin"
                          label-for="example-email-input"
                          label-class="form-label"
                      >
                        <div class="col-md-12">

                          <template v-if="model.files == null || (model.files != null &&model.files.length <= 0)">
                          </template>
                          <template v-else>
                            <div v-for="(file, index) in model.files" :key="index">
                              <a
                                  :href="`${apiUrl}files/view/${file.fileId}`"
                                  class=" fw-medium"
                              ><i
                                  :class="`mdi font-size-16 align-middle me-2`"
                              ></i>
                                {{ index + 1 }}. {{ file.fileName }}</a
                              >
                            </div>
                          </template>

                        </div>
                      </b-form-group>


                    </form>
                  </b-modal>
                </div>
              </div>
            </div>
            <!--            Pagination -->
            <div class="row">
              <div class="col-12">
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
                    <template v-slot:cell(process)="data">
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Xóa"
                          v-on:click="handleShowDeleteModal(data.item.id)">
                        <i class="fas fa-trash-alt text-danger me-1"></i>
                      </button>
                    </template>
                    <template v-slot:cell(tieuDe)="data">
                      <div @click="handleShowChiTietModal(data.item.id)" style="cursor: pointer">
                        {{ data.item.tieuDe }}
                      </div>

                    </template>
                    <template v-slot:cell(ngayNhan)="data">
                      {{ data.item.ngayNhanFull }}
                    </template>
                    <template v-slot:cell(nguoiGui)="data">
                      <div v-if="data.item.nguoiGui">
                        {{ data.item.nguoiGui.fullName }}
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
                    <div class="col-sm-12 col-md-6">
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
            title="Xóa dữ liệu thư"
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

        <b-modal
            v-model="showChiTietModal"
            centered
            title="Thông tin"
            title-class="font-12"
            no-close-on-backdrop
            size="lg"
            hide-footer
        >
          <div class="title-hopthu-main" style="text-align: justify">
            {{ model.tieuDe }}
          </div>
          <hr>
          <div class="row">
            <div class="col-md-6 col-m-12">
              <div style="margin-bottom: 5px">
                <div v-if="model.ngayNhanFull" style="margin-right: 10px; font-size: 14px"
                     class="d-flex flex-column text-success">
                  <label class="text-black-50"> NGÀY GỬI: </label>
                  <span>{{ model.ngayNhanFull }}</span>

                </div>
                <div v-if="model.nguoiGui" style="margin-right: 10px; font-size: 14px"
                     class="d-flex flex-column text-success">
                  <label class="text-black-50"> NGƯỜI TẠO: </label>
                  {{ model.nguoiGui.fullName }}
                  <template v-if="model.nguoiGui.donVi">
                    -{{ model.nguoiGui.donVi.ten }}
                  </template>
                </div>
              </div>
            </div>
            <!--            <div class="col-md-6 col-sm-12 d-flex" style="margin-bottom: 5px">-->
            <!--              <template v-if="model.nguoiNhans == null || (model.nguoiNhans != null &&model.nguoiNhans.length <= 0)">-->
            <!--              </template>-->
            <!--              <template v-else>-->
            <!--                <div class="d-flex flex-column">-->
            <!--                  <label for="" class="text-black-50">NGƯỜI NHẬN: </label>-->
            <!--                  <span v-for="(value, index) in model.nguoiNhans" :key="index" class="mb-1">-->
            <!--                    <span class="p-1 text-info font-size-12" style="background-color: rgba(56,164,248,0.2)">-->
            <!--                      {{ value.fullName }}({{ value.userName }})-->
            <!--                    </span>-->
            <!--                  </span>-->
            <!--                </div>-->

            <!--              </template>-->
            <!--            </div>-->
          </div>

          <!--          <div class="col-md-12" style="margin-bottom: 5px">-->
          <!--            <template v-if="model.cc == null || (model.cc != null &&model.cc.length <= 0)">-->
          <!--            </template>-->
          <!--            <template v-else>-->
          <!--              <label for="" class="title-hopthu">Cc: </label>-->
          <!--              <span v-for="(value, index) in model.cc" :key="index">-->
          <!--                <span>{{ value.fullName }}({{ value.userName }}), </span>-->
          <!--              </span>-->
          <!--            </template>-->
          <!--          </div>-->
          <div v-if="model.noiDung" class="mt-2">
            <div class="text-black-50">NỘI DUNG</div>
            <div :inner-html.prop="model.noiDung" style="text-align: justify">
            </div>
          </div>

          <div class="col-md-12">

            <template v-if="model.files == null || (model.files != null &&model.files.length <= 0)">
            </template>
            <template v-else>
              <label for="">Danh sách tệp tin (Nhấn vào để tải xuống)</label>
              <div v-for="(file, index) in model.files" :key="index">
                <a
                    :href="`${apiUrl}files/view/${file.fileId}`"
                    class=" fw-medium"
                ><i
                    :class="`mdi font-size-16 align-middle me-2`"
                ></i>
                  {{ index + 1 }}. {{ file.fileName }}</a
                >
              </div>
            </template>

          </div>
          <template #modal-header="{  }">
            <!-- Emulate built in modal header close button action -->
            <h5>Thông tin chi tiết</h5>
            <div class="text-end">
              <b-button variant="info" class="w-md me-1" size="sm" @click="handleChuyenTiep">
                Chuyển tiếp
              </b-button>
              <b-button variant="primary" class="w-md me-1" size="sm" @click="handleTraLoi(true)">
                Trả lời
              </b-button>
              <b-button variant="success" class="w-md me-1" size="sm" @click="handleTraLoi()">
                Trả lời tất cả
              </b-button>
              <b-button variant="light" class="w-md me-1" size="sm" @click="showChiTietModal = false">
                Đóng
              </b-button>
            </div>
          </template>
        </b-modal>
      </div>
    </div>
  </Layout>
</template>
<style>
.td-stt {
  text-align: center;
  width: 30px;
}

.td-xuly {
  text-align: center;
  width: 100px;
}

.table > tbody > tr > td {
  padding: 0px;
  line-height: 30px;
}

.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}

.title-capso {
  font-weight: 500;
  color: #00568C;
  margin-right: 10px;

}

.title-hopthu {
  font-weight: bold;
  color: #00568C;
  margin-right: 5px;
}

.title-hopthu-main {
  font-weight: bold;
  color: #00568C;
  margin-right: 10px;
  font-size: 16px;
  margin-bottom: 10px;
}
</style>
