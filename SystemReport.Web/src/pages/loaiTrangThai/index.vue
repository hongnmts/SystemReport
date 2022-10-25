<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import {required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {notifyModel} from "@/models/notifyModel";
import {pagingModel} from "@/models/pagingModel";
import Treeselect from "@riophae/vue-treeselect";
import {loaiTrangThaiModel} from "@/models/loaiTrangThaiModel";

export default {
  page: {
    title: "Loại trạng thái",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, Treeselect},
  data() {
    return {
      title: "Loại trạng thái",
      items: [
        {
          text: "Loại trạng thái",
          href: "/ma-thong-ke",
          // active: true,
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      data: [],
      showModal: false,
      showDetail: false,
      showDeleteModal: false,
      listMaThongKe : [] ,
      submitted: false,
      model: loaiTrangThaiModel.baseJson(),
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 10,
      pageOptions: [5,10, 25, 50, 100],
      filter: null,
      fields: [
        { key: 'STT', label: 'STT',
          thStyle: {width: '80px', minWidth: '60px' },
          class: "text-center",
          thClass: 'hidden-sortable' },
        {
          key: "code",
          label: "Mã loại TT",
          class:  "text-left",
          thClass: 'hidden-sortable'
        },
        {
          key: "Ten",
          label: "Tên loại TT",
          class:  "text-left",
          thClass: 'hidden-sortable'
        },
        {
          key: "listTrangThai",
          label: "Trạng thái",
          class:  "text-left",
          thClass: 'hidden-sortable'
        },
        {
          key: "thuTu",
          label: "Thứ tự",
          class : "text-center",
          thStyle: {width: '110px', minWidth: '110px'},
          thClass: 'hidden-sortable'
        },
        {
          key: 'process',
          label: 'Xử lý',
          class : "text-center",
          thStyle: {width: '110px', minWidth: '110px'},
          thClass: 'hidden-sortable'
        }
      ],
      optionTrangThai: []
    };
  },
  validations: {
    model: {
      code : {required},
      ten: {required},
      thuTu: {required}
    },
  },
  created() {
    this.getAllTrangThai();
  },
  watch: {
    model: {
      deep: true,
      handler(val) {
        // addCoQuanToModel()
        // this.saveValueToLocalStorage()
      }
    },
    showModal(status) {
      if (status == false) this.model = loaiTrangThaiModel.baseJson();
    },
    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
      }
    },
  },

  methods: {
    async handleUpdate(id) {
      await this.$store.dispatch("loaiTrangThaiStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = loaiTrangThaiModel.fromJson(res.data);
          this.showModal = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          this.$refs.tblList.refresh()
        }
      });
    },
    async handleDetail(id) {
      await this.$store.dispatch("loaiTrangThaiStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = loaiTrangThaiModel.fromJson(res.data);
          this.showDetail = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("loaiTrangThaiStore/delete", this.model.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showDeleteModal = false;
            this.$refs.tblList.refresh()
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          // });
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
          await this.$store.dispatch("loaiTrangThaiStore/update", this.model).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.showModal = false;
              this.model = loaiTrangThaiModel.baseJson()
              this.$refs.tblList.refresh()
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        } else {
          // Create model
          await this.$store.dispatch("loaiTrangThaiStore/create", loaiTrangThaiModel.toJson(this.model)).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.showModal = false;
              this.model = loaiTrangThaiModel.baseJson()
              this.$refs.tblList.refresh()
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        }
        loader.hide();
      }
      this.submitted = false;
    },
    myProvider (ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        content: this.filter,
        sortBy: ctx.sortBy,
        sortDesc: ctx.sortDesc,
      }
      this.loading = true

      try {
        let promise =  this.$store.dispatch("loaiTrangThaiStore/getPagingParams", params)
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
    normalizer(node) {
      if (node.children == null || node.children == 'null') {
        delete node.children;
      }
    },
    formatRemoveTrangThai(node, instanceId) {
      let value = this.model.listTrangThai?.find(x => x.id == node.id);
      if (value != null) {
        this.model.listTrangThai = this.model.listTrangThai.filter(x => x.id != value.id);
      }
    },
    formatTrangThai(node, instanceId) {
      let index = this.model.listTrangThai?.findIndex(x => x.id == node.id);
      if (index == -1 || index == undefined) {
        if (!this.model.listTrangThai) {
          this.model.listTrangThai = [];
        }
        this.model.listTrangThai.push({id: node.id, ten: node.label, code: node.code});
      }
    },
    async getAllTrangThai() {
      await this.$store.dispatch("trangThaiStore/getTree").then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionTrangThai = res.data
        } else {
          console.log("")
        }
      });
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
              <div class="col-sm-4">
                <div class="search-box me-2 mb-2 d-inline-block">
                  <div class="position-relative">
                    <input
                        v-model = "filter"
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
                  <b-button type="button" class="btn-label  btn-success mb-2 me-2" @click="showModal = true" size="sm"

                  >
                    <i class="mdi mdi-plus me-1 label-icon"></i> Thêm
                  </b-button>
                  <b-modal
                      v-model="showModal"
                      title="Thông tin loại trạng thái"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="lg"
                  >
                    <form @submit.prevent="handleSubmit"
                          ref="formContainer">
                      <div class="row">
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Loại trạng thái</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="model.code"/>
                            <input
                                v-model.trim="model.code"
                                type="text"
                                class="form-control"
                                placeholder="Nhập Loại trạng thái"
                                :class="{
                                'is-invalid':
                                  submitted && $v.model.code.$error,
                              }"
                            />
                            <div
                                v-if="submitted && !$v.model.code.required"
                                class="invalid-feedback"
                            >
                              Tên chức vụ không được để trống.
                            </div>
                          </div>
                        </div>

                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Tên Loại trạng thái</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="model.ten"/>
                            <input
                                id="ten"
                                v-model.trim="model.ten"
                                type="text"
                                class="form-control"
                                placeholder="Nhập tên Loại trạng thái"
                                :class="{
                                'is-invalid':
                                  submitted && $v.model.ten.$error,
                              }"
                            />
                            <div
                                v-if="submitted && !$v.model.ten.required"
                                class="invalid-feedback"
                            >
                              Tên chức vụ không được để trống.
                            </div>
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Trạng thái</label>
                            <treeselect
                                v-on:select="formatTrangThai"
                                v-on:deselect="formatRemoveTrangThai"
                                :multiple="true"
                                :options="optionTrangThai"
                                :value="model.listTrangThai"
                                :searchable="true"
                                :flat="true"
                                :show-count="true"
                                :default-expand-level="1"
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
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Nhập số thứ tự</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="model.id"/>
                            <input
                                id="thuTu"
                                v-model="model.thuTu"
                                type="number"
                                min="0"
                                oninput="validity.valid||(value='');"
                                class="form-control"
                                placeholder="Nhập số thứ tự"
                                :class="{
                                'is-invalid':
                                  submitted && $v.model.thuTu.$error,
                              }"
                            />
                            <div
                                v-if="submitted && !$v.model.thuTu.required"
                                class="invalid-feedback"
                            >
                              Thứ tự không được để trống.
                            </div>
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Nhập mô tả </label>
                            <input type="hidden" v-model="model.moTa"/>
                            <textarea
                                id="ghichu"
                                v-model.trim="model.moTa"
                                type="text"
                                class="form-control"
                                placeholder="Nhập ghi chú"
                            />
                          </div>
                        </div>
                      </div>
                      <div class="text-end pt-2 mt-3">
                        <b-button variant="light" @click="showModal = false">
                          Đóng
                        </b-button>
                        <b-button type="submit" variant="success" class="ms-1">
                          Lưu
                        </b-button>
                      </div>
                    </form>
                  </b-modal>
                  <b-modal
                      v-model="showDetail"
                      title="Thông tin chi tiết chức vụ"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="lg"
                  >
                    <form @submit.prevent="handleSubmit"
                          ref="formContainer">
                      <div class="row">
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Tên chức vụ : </label>
                            <input
                                v-model="model.ten"
                                type="text"
                                class="form-control"
                            />
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Thứ tự : </label>
                            <input
                                v-model="model.thuTu"
                                type="number"
                                min="0"
                                oninput="validity.valid||(value='');"
                                class="form-control"
                            />
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Người tạo : </label>
                            <input
                                v-model="model.createdBy"
                                type="text"
                                class="form-control"
                            />
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Ngày tạo: </label>
                            <input
                                v-model="model.createdAt"
                                type="text"
                                class="form-control"
                            />
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Người cập nhật : </label>
                            <input
                                v-model="model.modifiedBy"
                                type="text"
                                class="form-control"
                            />
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Ngày cập nhật : </label>
                            <input
                                v-model="model.modifiedAt"
                                type="text"
                                class="form-control"
                            />
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Mô tả: </label>
                            <textarea
                                id="ghichu01"
                                v-model.trim="model.moTa"
                                type="text"
                                class="form-control"
                                placeholder="Nhập ghi chú"
                            />
                          </div>
                        </div>
                      </div>
                      <div class="text-end">
                        <b-button variant="light" @click="showDetail = false">
                          Đóng
                        </b-button>
                      </div>
                    </form>
                  </b-modal>
                </div>
              </div>
            </div>
            <div class="row" style="margin-top: -20px">
              <div class="col-12">
                <div class="row mt-4">
                  <div class="col-sm-12 col-md-6">
                    <div id="tickets-table_length" class="dataTables_length">
                      <label class="d-inline-flex align-items-center">
                        Show&nbsp;
                        <b-form-select
                            class="form-select form-select-sm"
                            v-model="perPage"
                            size="sm"
                            :options="pageOptions"
                        ></b-form-select
                        >&nbsp;entries
                      </label>
                    </div>
                  </div>
                </div>
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
                      :filter="filter"
                      ref="tblList"
                      primary-key="id"
                  >
                    <template v-slot:cell(STT)="data">
                      {{ data.index + ((currentPage-1)*perPage) + 1  }}
                    </template>
                    <template v-slot:cell(code)="data">
                      {{data.item.code}}
                    </template>
                    <template v-slot:cell(ten)="data">
                      {{data.item.ten}}
                    </template>
                    <template v-slot:cell(listTrangThai)="data">
                      <template v-if="data.item.listTrangThai != null && data.item.listTrangThai.length > 0">
                        <div  v-for="(value, index) in data.item.listTrangThai" :key="index">
                          {{value.ten}}
                        </div>
                      </template>
                    </template>
                    <template v-slot:cell(process)="data">
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Chi tiết"
                          v-on:click="handleDetail(data.item.id)">
                        <i class="fas fa-eye  text-warning me-1"></i>
                      </button>
                      <button

                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                          v-on:click="handleUpdate(data.item.id)">
                        <i class="fas fa-pencil-alt text-success me-1"></i>
                      </button>
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Xóa"
                          v-on:click="handleShowDeleteModal(data.item.id)">
                        <i class="fas fa-trash-alt text-danger me-1"></i>
                      </button>
                    </template>
                  </b-table>
                </div>
                <div class="row">
                  <b-col>
                    <div>Hiển thị {{numberOfElement}} trên tổng số {{totalRows}} dòng</div>
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
                        >
                        </b-pagination>
                      </ul>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
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
            <button v-b-modal.modal-close_visit
                    class="btn btn-outline-info m-1"
                    v-on:click="showDeleteModal = false">
              Đóng
            </button>
            <button v-b-modal.modal-close_visit
                    class="btn btn-danger btn m-1"
                    v-on:click="handleDelete">
              Xóa
            </button>
          </template>
        </b-modal>
      </div>
    </div>
  </Layout>
</template>
<style>
.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}
.table>tbody> tr >td{
  padding: 0px;
  line-height: 30px;
}
</style>
