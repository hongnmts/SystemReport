<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import {required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {pagingModel} from "@/models/pagingModel";
import moment from "moment";
import DatePicker from "vue2-datepicker";

export default {
  page: {
    title: "Quản Lý Logging",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, DatePicker},
  data() {
    return {
      title: "Lịch sử thao tác",
      items: [
        {
          text: "Logging",
          href: "/logging",
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 10,
      pageOptions: [5,10, 25, 50, 100],
      filter: null,
      modelFilter:{
        tuNgay: null,
        denNgay: null
      },
      fields: [
        {
          key: 'STT',
          label: 'STT',
          class : "text-center",
          thStyle: {width: '50px', minWidth: '50px' },
          tdClass: 'align-middle',
          thClass: 'hidden-sortable'
        },
        {
          key: "lastModifiedShow",
          label: "Ngày",
          class : "text-center",
          sortable: true,
          thStyle: {width: '110px', minWidth: '110px'},
        },
        {
          key: "modifiedBy",
          label: "Người thao tác",
          sortable: true,
        },
        {
          key: "collectionName",
          class : "text-center",
          label: "Tên bảng",
          sortable: true,
        },
        {
          key: "action",
          label: "Hành động",
          class : "text-center",
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: 'hidden-sortable'
        },
        {
          key: "contentLog",
          label: "Nội dung",
          thClass: 'hidden-sortable',
          class : "text-center",
          sortable: true,
        }
      ],
    };
  },
  validations: {
    model: {
      ten: {required},
      thuTu: {required}
    },
  },
  watch: {
    modelFilter: {
      deep: true,
      handler(val) {
        this.$refs.tblList?.refresh();
      }
    },
    model: {
      deep: true,
      handler(val) {
        // addCoQuanToModel()
        // this.saveValueToLocalStorage()
      }
    },

    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
      }
    },
  },
  methods: {
    choseDate(value){
      let tempTime = moment(value).format('DD/MM/YYYY');
      if (tempTime !== 'Invalid date')
        return tempTime;
      return value;
    },
    handleChoseDenNgay(val){
      this.modelFilter.denNgay = this.choseDate(val) ;
    },
    handleChoseTuNgay(val){
      this.modelFilter.tuNgay = this.choseDate(val) ;
    },
    myProvider (ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        content: this.filter,
        sortBy: ctx.sortBy,
        sortDesc: ctx.sortDesc,
        tuNgay: this.modelFilter.tuNgay,
        denNgay: this.modelFilter.denNgay,
      }
      this.loading = true
      try {
        let promise =  this.$store.dispatch("loggingStore/getPagingParams", params)
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
                <div class="mb-3">
                  <label class="text-left">Tìm kiếm</label>
                  <br />
                  <div class="search-box me-2 mb-2 d-inline-block" style="width: 100%">
                    <div class="position-relative">
                      <input
                          v-model = "filter"
                          type="text"
                          class="form-control"
                          placeholder="Người thao tác, nội dung,..."
                      />
                      <i class="bx bx-search-alt search-icon"></i>
                    </div>
                  </div>
                  </div>

              </div>
              <div  class="col-sm-4">
                <div class="mb-3">
                  <label class="text-left">Từ ngày</label>
                  <date-picker v-model="modelFilter.tuNgay"
                               format="DD/MM/YYYY"
                               v-on:change="handleChoseTuNgay"
                  >
                    <div slot="input" >
                      <input v-model="modelFilter.tuNgay" v-mask="'##/##/####'" type="text" class="form-control" />
                    </div>
                  </date-picker>
                </div>
              </div>
              <div  class="col-sm-4">
                <div class="mb-3">
                  <label class="text-left">Đến ngày</label>
                  <date-picker v-model="modelFilter.denNgay"
                               format="DD/MM/YYYY"
                               v-on:change="handleChoseDenNgay"
                  >
                    <div slot="input" >
                      <input v-model="modelFilter.denNgay" v-mask="'##/##/####'" type="text" class="form-control" />
                    </div>
                  </date-picker>
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
                          :filter="filter"
                          ref="tblList"
                          primary-key="id"
                      >
                        <template v-slot:cell(STT)="data">
                          {{ data.index + ((currentPage-1)*perPage) + 1  }}
                        </template>
                        <template v-slot:cell(ten)="data">&nbsp;&nbsp;
                          {{data.item.ten}}
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
  </Layout>
</template>
<style>
.table>tbody> tr >td{
  padding: 0px;
  line-height: 30px;
}
.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}

</style>
