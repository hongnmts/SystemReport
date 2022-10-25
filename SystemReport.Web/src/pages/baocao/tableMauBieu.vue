<template>
  <div>
    <div class="col-12 mb-2">
      <div class="row">
        <div class="col-6">
          <label class="form-label"> Loại mẫu biểu</label>
          <br />
          <multiselect
              :multiple="false"
              v-model="loaiMauBieu"
              :options="optionLoaiMauBieu"
              track-by="id"
              label="ten"
              placeholder="Chọn loại mẫu biểu"
              deselect-label="Nhấn để xoá"
              selectLabel="Nhấn enter để chọn"
              selectedLabel="Đã chọn"
              @select="handleSelectChangeLoaiMauBieu"
          ></multiselect>
        </div>
      </div>
    </div>
    <div class="col-12">
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

      <div class="table-responsive-sm">
        <b-table
            class="datatables"
            :items="myProviderMau"
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
          <template v-slot:cell(kyHieu)="data">
            <div class="px-2">    {{data.item.kyHieu}}</div>
          </template>
          <template v-slot:cell(ten)="data">
            <div style="cursor: pointer" class="px-2">    {{data.item.ten}}</div>
          </template>

          <template v-slot:cell(process)="data">
            <button
                type="button"
                size="sm"
                class="btn btn-outline btn-sm"
                data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                v-on:click="asyncHandleChooseMauBieu(data.item.id)">
              <i class="fas fa-plus text-success me-1"></i>
            </button>
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

</template>

<script>
import {CONSTANTS} from "@/helpers/constants";
import {pagingModel} from "@/models/pagingModel";
import {notifyModel} from "@/models/notifyModel";
import Multiselect from "vue-multiselect";

export default {
  name: "tableMauBieu",
  props:['modelInput', 'showModalInputMauBieu', 'refreshTableMauBieu'],
  components: { Multiselect },
  mounted() {
    this.getLoaiMauBieu();
  },
  data(){
    return{
      optionLoaiMauBieu: [],
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      todoTotalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 50,
      pageOptions: [50, 100],
      filter: null,
      todoFilter: null,
      filterOn: [],
      loaiMauBieu: null,
      todofilterOn: [],
      isBusy: false,
      sortBy: "age",
      sortDesc: false,
      fields: [
        { key: 'STT', label: 'STT', class: 'td-stt', sortable: false,thClass: 'hidden-sortable' },
        {
          key: "kyHieu",
          label: "Kỳ hiệu",
          sortable: true,
          thStyle:"text-align: center",
          thClass: 'hidden-sortable',
        },
        {
          key: "ten",
          label: "Tên mẫu biểu",
          sortable: true,
          thStyle:"text-align:left",
          thClass: 'hidden-sortable',
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: 'td-xuly',
          thClass: 'hidden-sortable',
          sortable: false
        }
      ],
      fieldsBangBieu: [
        { key: 'STT', label: 'STT', class: 'td-stt', sortable: false,thClass: 'hidden-sortable' },
        {
          key: "ten",
          label: "Tên bảng biểu",
          sortable: true,
          thStyle:"text-align:left",
          thClass: 'hidden-sortable',
        },
        {
          key: "thuTu",
          label: "Thứ tự",
          class: 'td-thuTu',
          sortable: true,
          thClass: 'hidden-sortable',
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: 'td-xuly',
          thClass: 'hidden-sortable',
          sortable: false
        }
      ],
      model: {
        mauBieuId: null,
        thang: null,
        nam: null,
        kyBaoCao: null
      }
    }
  },
  methods:{
    async getLoaiMauBieu() {
      try {
        await this.$store.dispatch("loaiMauBieuStore/getAll").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.optionLoaiMauBieu = items;
          }
          return [];
        });
      } finally {
        console.log("");
      }
    },
    handleSelectChangeLoaiMauBieu(selectedOption, id){
      this.$refs.tblList.refresh();
    },
    async asyncHandleChooseMauBieu(id){
      this.model = this.modelInput;
      this.model.mauBieuId = id;
      if(this.model.mauBieuId != null && this.model.kyBaoCao != null ){
        if(this.model.kyBaoCao.code == 'giaidoan' && (this.model.tuNgay == null || this.model.denNgay == null)){
          this.$store.dispatch("snackBarStore/addNotify", {resultString: "Hãy chọn kỳ báo cáo", resultCode: "FAILD"})
          return;
        }
        if(this.model.kyBaoCao.code == 'thang' && this.model.thang == null && this.model.nam == null){
          this.$store.dispatch("snackBarStore/addNotify", {resultString: "Hãy chọn kỳ báo cáo", resultCode: "FAILD"})
          return;
        }
        if(this.model.kyBaoCao.code == 'nam' && this.model.nam == null){
          this.$store.dispatch("snackBarStore/addNotify", {resultString: "Hãy chọn kỳ báo cáo", resultCode: "FAILD"})
          return;
        }
      }
      else if(this.model.mauBieuId == null || this.model.kyBaoCao == null){
        this.$store.dispatch("snackBarStore/addNotify", {resultString: "Hãy chọn kỳ báo cáo", resultCode: "FAILD"})
        return;
      }
      await this.$store.dispatch("mauBieuStore/generateMauBieu", this.model).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          //   this.$emit('update:showModalInputMauBieu', false)
          //   this.$emit('update:modelInput', {
          //     mauBieuId: null,
          //     thang: null,
          //     nam: null,
          //     kyBaoCao: null
          //   })
          //
          // }
          this.refreshTableMauBieu();
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },
    myProviderMau (ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        content: this.filter,
        sortBy: ctx.sortBy,
        sortDesc: ctx.sortDesc,
        loaiMauBieu: this.loaiMauBieu
      }
      this.loading = true
      try {
        let promise =  this.$store.dispatch("mauBieuStore/getPagingParamsCaNhan", params)
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
    },
  }
}
</script>

<style scoped>

</style>