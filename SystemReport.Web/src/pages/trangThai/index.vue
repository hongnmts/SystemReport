<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import {required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {notifyModel} from "@/models/notifyModel";
import {pagingModel} from "@/models/pagingModel";
import {CONSTANTS} from "@/helpers/constants";
import {trangThaiModel} from "@/models/trangThaiModel";
import Treeselect from "@riophae/vue-treeselect";

export default {
  page: {
    title: "Trạng thái",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, Treeselect},
  data() {
    return {
      title: "Trạng thái",
      items: [
        {
          text: "Trạng thái",
          href: "/trang-thai",
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
      submitted: false,
      model: trangThaiModel.baseJson(),
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      todoTotalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 50,
      pageOptions: [5,10, 25, 50, 100],
      filter: null,
      todoFilter: null,
      filterOn: [],
      todofilterOn: [],
      isBusy: false,
      sortBy: "code",
      sortDesc: false,
      fields: [
        { key: 'STT', label: 'STT', class: 'td-stt', sortable: false,thClass: 'hidden-sortable' },
        {
          key: "code",
          label: "Code",
          class: 'td-xuly',
          sortable: true,
        },
        {
          key: "ten",
          label: "Tên",
          sortable: true,
          thStyle:"text-align:left",
        },
        {
          key: "nextTrangThai",
          label: "Trạng thái tiếp theo",
          sortable: true,
          thStyle:"text-align:left",
        },
        {
          key: "actions",
          label: "Hành động",
          sortable: true,
          thStyle:"text-align:left",
        },
        {
          key: "thuTu",
          label: "Thứ tự",
          class: 'td-xuly',
          sortable: true,
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: 'td-xuly',
          thClass: 'hidden-sortable',
          sortable: false
        }
      ],
      optionTrangThai: [],
      optionActions: []
    };
  },
  validations: {
    model: {
      code: {required},
      ten: {required},
      thuTu: {required}
    },
  },
  created() {
    this.fnGetList();
    this.getAllTrangThai();
    this.getActions();
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
      if (status == false) this.model = trangThaiModel.baseJson();
    },
    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
      }
    },
  },
  methods: {
    async getActions() {
      await this.$store.dispatch("moduleStore/getTreeModule").then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionActions = res.data
        }
      });
    },
    async getAllTrangThai() {
      await this.$store.dispatch("trangThaiStore/getTree").then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionTrangThai = res.data
        }
      });
    },
    normalizer(node) {
      if (node.children == null || node.children == 'null') {
        delete node.children;
      }
    },
    formatRemoveAction(node, instanceId) {
      let value = this.model.actions?.find(x => x.id == node.id);
      if (value != null) {
        this.model.actions = this.model.actions.filter(x => x.id != value.id);
      }
    },
    formatAction(node, instanceId) {
      let index = this.model.actions?.findIndex(x => x.id == node.id);
      if (index == -1 || index == undefined) {
        if (!this.model.actions) {
          this.model.actions = [];
        }
        this.model.actions.push({id: node.id, action: node.label, code: node.code});
      }
    },
    formatRemoveTrangThai(node, instanceId) {
      let value = this.model.nextTrangThai?.find(x => x.id == node.id);
      if (value != null) {
        this.model.nextTrangThai = this.model.nextTrangThai.filter(x => x.id != value.id);
      }
    },
    formatTrangThai(node, instanceId) {
      let index = this.model.nextTrangThai?.findIndex(x => x.id == node.id);
      if (index == -1 || index == undefined) {
        if (!this.model.nextTrangThai) {
          this.model.nextTrangThai = [];
        }
        this.model.nextTrangThai.push({id: node.id, ten: node.label, code: node.code});

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
    async handleUpdate(id) {
      await this.$store.dispatch("trangThaiStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {

          this.model = trangThaiModel.fromJson(res.data);
          this.showModal = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          this.$refs.tblList.refresh()
        }
      });
    },
    async handleDetail(id) {
      await this.$store.dispatch("trangThaiStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = trangThaiModel.fromJson(res.data);

          this.showDetail = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("trangThaiStore/delete", this.model.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.fnGetList();
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
          await this.$store.dispatch("trangThaiStore/update", this.model).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.showModal = false;
              this.$refs.tblList.refresh()
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        } else {
          // Create model
          await this.$store.dispatch("trangThaiStore/create", trangThaiModel.toJson(this.model)).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.fnGetList();
              this.showModal = false;
              this.$refs.tblList.refresh()
              this.model = {};
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
        let promise =  this.$store.dispatch("trangThaiStore/getPagingParams", params)
        return promise.then(resp => {
          if(resp.resultCode == CONSTANTS.SUCCESS){
            let data = resp.data;
            this.totalRows = data.totalRows
            let items = data.data
            this.numberOfElement = items.length
            this.loading = false
            return items || []
          }else{
            return [];
          }
        })
      } finally {
        this.loading = false
      }
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
                  <b-button
                      variant="primary"
                      type="button"
                      class="btn w-md btn-primary"
                      @click="showModal = true"
                      size="sm"
                  >
                    <i class="mdi mdi-plus me-1"></i> Tạo trạng thái
                  </b-button>
                  <b-modal
                      v-model="showModal"
                      title="Thông tin trạng thái"
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
                        <div class="col-6">
                          <div class="mb-3">
                            <label class="text-left"> Code</label>
                            <span style="color: red">&nbsp;*</span>
                            <input
                                id="ten"
                                v-model.trim="model.code"
                                type="text"
                                class="form-control"
                                placeholder="Nhập code"
                                style="text-transform:uppercase"
                                :class="{
                                'is-invalid':
                                  submitted && $v.model.code.$error,
                              }"
                            />
                            <div
                                v-if="submitted && !$v.model.code.required"
                                class="invalid-feedback"
                            >
                              Tên trạng thái không được để trống.
                            </div>
                          </div>
                        </div>
                        <div class="col-6">
                          <div class="mb-3">
                            <label class="text-left">Tên trạng thái</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="model.id"/>
                            <input
                                id="ten"
                                v-model.trim="model.ten"
                                type="text"
                                class="form-control"
                                placeholder="Nhập tên trạng thái"
                                :class="{
                                'is-invalid':
                                  submitted && $v.model.ten.$error,
                                }"
                            />
                            <div
                                v-if="submitted && !$v.model.ten.required"
                                class="invalid-feedback"
                            >
                              Tên trạng thái không được để trống.
                            </div>
                          </div>
                        </div>
                        <div class="col-6">
                          <div class="mb-3">
                            <label class="text-left">Background</label>
                            <input
                                id="bgColor"
                                v-model.trim="model.bgColor"
                                type="text"
                                class="form-control"
                                placeholder="Nhập background"
                            />
                          </div>
                        </div>
                        <div class="col-6">
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
                        <div class="col-6">
                          <div class="mb-3">
                            <label class="text-left">Màu sắc</label>
                            <span style="color: red">&nbsp;*</span>
                            <b-form-input
                                style="width: 100%; max-width: 100%"
                                class="form-control-color"
                                id="color"
                                type="color"
                                name="color"
                                v-model="model.color"
                                value="#556ee6"
                            ></b-form-input>
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Trạng thái tiếp theo</label>
                            <treeselect
                                v-on:select="formatTrangThai"
                                v-on:deselect="formatRemoveTrangThai"
                                :multiple="true"
                                :options="optionTrangThai"
                                :value="model.nextTrangThai"
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
                            <label class="text-left">Hành động</label>
                            <treeselect
                                v-on:select="formatAction"
                                v-on:deselect="formatRemoveAction"
                                :multiple="true"
                                :options="optionActions"
                                :value="model.actions"
                                :searchable="true"
                                :flat="true"
                                :show-count="true"
                                :default-expand-level="1"
                                :normalizer="normalizer"
                                value-format="object"
                                :disable-branch-nodes="true"
                                search-nested
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
                  <b-modal
                      v-model="showDetail"
                      title="Thông tin chi tiết trạng thái"
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
                            <label class="text-left">Tên trạng thái: </label>
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
                                v-model="model.createdAtShow"
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
                                v-model="model.lastModifiedShow"
                                type="text"
                                class="form-control"
                            />
                          </div>
                        </div>
                      </div>
                      <div class="text-end pt-2 mt-3">
                        <b-button variant="light" @click="showDetail = false">
                          Đóng
                        </b-button>
                      </div>
                    </form>
                  </b-modal>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-12">
                <div class="row mb-3">
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
                      {{ data.index + ((currentPage-1)*perPage) + 1  }}
                    </template>
                    <template v-slot:cell(process)="data">
<!--                      <button-->
<!--                          type="button"-->
<!--                          size="sm"-->
<!--                          class="btn btn-outline btn-sm"-->
<!--                          data-toggle="tooltip" data-placement="bottom" title="Chi tiết"-->
<!--                          v-on:click="handleDetail(data.item.id)">-->
<!--                        <i class="fas fa-eye  text-warning me-1"></i>-->
<!--                      </button>-->
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
                    <template v-slot:cell(ten)="data">
                      {{data.item.ten}}
                    </template>
                    <template v-slot:cell(nextTrangThai)="data">
                      <template v-if="data.item.nextTrangThai != null && data.item.nextTrangThai.length > 0">
                        <div  v-for="(value, index) in data.item.nextTrangThai" :key="index">
                          {{value.ten}}
                        </div>
                      </template>
                    </template>
                    <template v-slot:cell(actions)="data">
                      <template v-if="data.item.actions != null && data.item.actions.length > 0">
                        <div  v-for="(value, index) in data.item.actions" :key="index">
                          {{value.action}}
                        </div>
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
                        ></b-pagination>
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
.table>tbody> tr >td{
  padding: 0px;
  line-height: 30px;
}
.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}
</style>
