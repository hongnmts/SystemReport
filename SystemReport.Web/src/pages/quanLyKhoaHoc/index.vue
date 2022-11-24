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
import {quanLyKhoaHocModel} from "@/models/quanLyKHModel";
export default {
  page: {
    title: "Quản lý đề tài",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader},
  data() {
    return {
      title: "Quản lý đề tài",
      items: [
        {
          text: "Quản lý đề tài",
          href: "/quan-ly-de-tai",
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
      model: quanLyKhoaHocModel.baseJson(),
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      todoTotalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 10,
      pageOptions: [5,10, 25, 50, 100],
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
          key: 'ngayChuyenGiao',
          label: 'Ngày chuyển giao',
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
        {
          key: 'process',
          label: 'Xử lý',
          thStyle: {width: '120px', minWidth: '120px'},
          thClass: "text-primary hidden-sortable text-center",
          class: "btn-process"
        },
      ],
    };
  },
  validations: {
    model: {
      ten: {required},
      thuTu: {required}
    },
  },
  created() {
    // this.fnGetList();
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
      if (status == false) this.model = hoTroDoanhNghiepModel.baseJson();
    },
    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
      }
    },
  },
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
    handleUpdate(id) {
      this.$router.push("/quan-ly-de-tai/create/" + id)
    },
    async handleDetail(id) {
      await this.$store.dispatch("quanLyKhoaHocStore/getById", id).then((res) => {
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
        await this.$store.dispatch("quanLyKhoaHocStore/delete", this.model.id).then((res) => {
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
    handleShowDeleteModal(id) {
      this.model.id = id;
      this.showDeleteModal = true;
    },
    // async handleSubmit(e) {
    //   e.preventDefault();
    //   this.submitted = true;
    //   this.$v.$touch();
    //   if (this.$v.$invalid) {
    //     return;
    //   } else {
    //     let loader = this.$loading.show({
    //       container: this.$refs.formContainer,
    //     });
    //     if (
    //         this.model.id != 0 &&
    //         this.model.id != null &&
    //         this.model.id
    //     ) {
    //       // Update model
    //       await this.$store.dispatch("hoTroDoanhNghiepStore/update", this.model).then((res) => {
    //         if (res.resultCode === 'SUCCESS') {
    //           this.fnGetList();
    //           this.showModal = false;
    //           this.$refs.tblList.refresh()
    //         }
    //         this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
    //       });
    //     } else {
    //       // Create model
    //       await this.$store.dispatch("hoTroDoanhNghiepStore/create", linhVucModel.toJson(this.model)).then((res) => {
    //         if (res.resultCode === 'SUCCESS') {
    //           this.fnGetList();
    //           this.showModal = false;
    //           this.$refs.tblList.refresh()
    //           this.model = {};
    //         }
    //         this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
    //       });
    //     }
    //     loader.hide();
    //   }
    //   this.submitted = false;
    // },
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
        let promise =  this.$store.dispatch("quanLyKhoaHocStore/getPagingParams", params)
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
                      @click="$router.push('/quan-ly-de-tai/create')"
                      size="sm"
                  >
                    <i class="mdi mdi-plus me-1"></i> Thêm đề tài
                  </b-button>
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
                  >
                    <template v-slot:cell(STT)="data">
                      {{ data.index + ((currentPage-1)*perPage) + 1  }}
                    </template>
                    <template v-slot:cell(process)="data">
<!--                      <button-->
<!--                          type="button"-->
<!--                          size="sm"-->
<!--                          class="btn btn-detail btn-sm"-->
<!--                          data-toggle="tooltip" data-placement="bottom" title="Chi tiết"-->
<!--                          v-on:click="handleDetail(data.item.id)">-->
<!--                        <i class="fas fa-eye "></i>-->
<!--                      </button>-->
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-edit btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                          v-on:click="handleUpdate(data.item.id)">
                        <i class="fas fa-pencil-alt"></i>
                      </button>
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-delete btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Xóa"
                          v-on:click="handleShowDeleteModal(data.item.id)">
                        <i class="fas fa-trash-alt"></i>
                      </button>
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
}
.td-xuly {
  text-align: center;
}
.table>tbody> tr >td{
  padding: 0px;
  line-height: 30px;
}
.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}
@media only screen and (max-width: 768px) {
  .b-table-linhvuc>td:nth-of-type(1):before{
    content: "STT";
    font-weight: bold;
    color: #00568c;
  }
  .b-table-linhvuc>td:nth-of-type(2):before {
    content: "Tên";
    font-weight: bold;
    color: #00568c;
  }
  .b-table-linhvuc>td:nth-of-type(3):before {
    content: "Thứ tự";
    font-weight: bold;
    color: #00568c;
  }
  .b-table-linhvuc>td:nth-of-type(4):before {
    content: "";
    font-weight: bold;
    color: #00568c;
  }
}

</style>
