<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-lg-6 col-md-12">
        <div class="card">
          <div class="card-body">
            <div class="header-thiet-ke-mau-bieu mb-3" style="display: flex; justify-content: space-between">
              <h5>Danh sách mẫu biểu</h5>
              <div style="display: flex">
                <b-button
                    variant="primary"
                    type="button"
                    class="btn w-md btn-primary"
                    @click="handleCreate"
                    size="sm"
                >
                  <i class="mdi mdi-plus me-1"></i> Tạo mẫu biểu
                </b-button>
              </div>

            </div>
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
                class="mb-2"
                @select="handleSelectChangeLoaiMauBieu"
            ></multiselect>

            <div class="row">
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
                    <template v-slot:cell(kyHieu)="data">
                      <div class="px-2">    {{data.item.kyHieu}}</div>
                    </template>
                    <template v-slot:cell(ten)="data">
                      <div style="cursor: pointer" @click="handleGetBangBieu(data.item.id, data.item)" class="px-2">    {{data.item.ten}}</div>
                    </template>

                    <template v-slot:cell(process)="data">
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
      </div>
      <div class="col-6" v-if="model.id">
        <div class="card">
          <div class="card-body">
            <div class="header-thiet-ke-mau-bieu mb-3" style="display: flex; justify-content: space-between">
              <h5 class="pe-1">Biểu mẫu: <i> {{model.ten}} </i></h5>
              <div style="display: flex; flex-direction: column; justify-content: center; align-items: end">
                <b-button
                    variant="info"
                    type="button"
                    class="btn w-md btn-primary mb-2"
                    @click="handleXemMauBieu"
                    size="sm"
                >
                  <i class="mdi mdi-eye-check me-1"></i> Xem mẫu biểu
                </b-button>
                <b-button
                    style="width: 140px"
                    variant="primary"
                    type="button"
                    class="btn w-md btn-primary"
                    @click="handleCreateBangBieu"
                    size="sm"
                >
                  <i class="mdi mdi-plus me-1"></i> Tạo bảng biểu
                </b-button>
              </div>
            </div>
            <div class="row" v-if="listBangBieu.length > 0">
              <div class="col-12">
                <div class="card" v-for="(value, index) in listBangBieu" :key="index">
                  <div class="card-body">
                    <h4 class="card-title"><h4 class="card-title">#{{index + 1}}. {{value.ten}}</h4></h4>
                    <p> </p>
                    <a href="javascript:void(0);" @click="handlePushThietKe(value.id)" class="btn btn-primary btn-block me-1" size="sm">   <i class="mdi mdi-apps me-1"></i> Thiết kế bảng biểu</a>
                    <a href="javascript:void(0);" @click="handleXemBangBieu(value.id)" class="btn btn-info btn-block" size="sm"> <i class="mdi mdi-eye me-1"></i>Xem bảng biểu</a>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <b-modal
        v-model="showModalMauBieu"
        title="Mẫu biểu"
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
              <label class="text-left">Tên mẫu biểu</label>
              <span style="color: red">&nbsp;*</span>
              <input type="hidden" v-model="model.id"/>
              <input
                  id="ten"
                  v-model.trim="model.ten"
                  type="text"
                  class="form-control"
                  placeholder=""
                  :class="{
                                'is-invalid':
                                  submitted && $v.model.ten.$error,
                              }"
              />
              <div
                  v-if="submitted && !$v.model.ten.required"
                  class="invalid-feedback"
              >
                Tên mẫu biểu không được để trống.
              </div>
            </div>
          </div>
<!--          <div class="col-12">-->
<!--            <div class="mb-3">-->
<!--              <label class="text-left">Tên ngắn</label>-->
<!--              <span style="color: red">&nbsp;*</span>-->
<!--              <input-->
<!--                  id="ten"-->
<!--                  v-model.trim="model.tenNgan"-->
<!--                  type="text"-->
<!--                  class="form-control"-->
<!--                  placeholder=""-->
<!--                  :class="{-->
<!--                                'is-invalid':-->
<!--                                  submitted && $v.model.tenNgan.$error,-->
<!--                              }"-->
<!--              />-->
<!--              <div-->
<!--                  v-if="submitted && !$v.model.tenNgan.required"-->
<!--                  class="invalid-feedback"-->
<!--              >-->
<!--                Tên ngắn không được để trống.-->
<!--              </div>-->
<!--            </div>-->
<!--          </div>-->
          <div class="col-6">
            <div class="mb-3">
              <label class="text-left">Ký hiệu</label>
              <span style="color: red">&nbsp;*</span>
              <input
                  id="ten"
                  v-model.trim="model.kyHieu"
                  type="text"
                  class="form-control"
                  placeholder=""
                  :class="{
                                'is-invalid':
                                  submitted && $v.model.kyHieu.$error,
                              }"
              />
              <div
                  v-if="submitted && !$v.model.kyHieu.required"
                  class="invalid-feedback"
              >
                Ký hiệu không được để trống.
              </div>
            </div>
          </div>
          <div class="col-6">
            <div class="mb-3">
              <label class="text-left">Loại mẫu biểu</label>
              <multiselect
                  :multiple="false"
                  v-model="model.loaiMauBieu"
                  :options="optionLoaiMauBieu"
                  track-by="id"
                  label="ten"
                  placeholder="Chọn loại mẫu biểu"
                  deselect-label="Nhấn để xoá"
                  selectLabel="Nhấn enter để chọn"
                  selectedLabel="Đã chọn"
              ></multiselect>
            </div>
          </div>
          <div class="col-6">
            <div class="mb-3">
              <label class="text-left">Nhập số thứ tự</label>
              <span style="color: red">&nbsp;*</span>
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
          <hr />
          <div class="col-6">
            <div class="mb-3">
              <label class="text-left">Đơn vị báo cáo</label>
              <multiselect
                  :multiple="false"
                  v-model="model.donViBaoCao"
                  :options="optionCoQuans"
                  track-by="id"
                  label="ten"
                  placeholder="Chọn cơ quan"
                  deselect-label="Nhấn để xoá"
                  selectLabel="Nhấn enter để chọn"
                  selectedLabel="Đã chọn"
              ></multiselect>
            </div>
          </div>
          <div class="col-6">
            <div class="mb-3">
              <label class="text-left">Đơn vị nhận</label>
              <multiselect
                  :multiple="false"
                  v-model="model.donViNhan"
                  :options="optionCoQuans"
                  track-by="id"
                  label="ten"
                  placeholder="Chọn cơ quan"
                  deselect-label="Nhấn để xoá"
                  selectLabel="Nhấn enter để chọn"
                  selectedLabel="Đã chọn"
              ></multiselect>
            </div>
          </div>
          <hr />
          <div class="col-12">
            <div class="mb-3">
              <label class="text-left">Phân quyền đơn vị</label>
              <multiselect
                  :multiple="true"
                  v-model="model.phanQuyenDonVi"
                  :options="optionDonVis"
                  track-by="id"
                  label="ten"
                  placeholder="Chọn đơn vị"
                  deselect-label="Nhấn để xoá"
                  selectLabel="Nhấn enter để chọn"
                  selectedLabel="Đã chọn"
              ></multiselect>
            </div>
          </div>
        </div>
        <div class="text-end pt-2 mt-3">
          <b-button variant="light" class="w-md" size="sm" @click="handleCloseModalMauBieu">
            Đóng
          </b-button>
          <b-button type="submit" variant="primary" size="sm" class="ms-1 w-md">
            Lưu
          </b-button>
        </div>
      </form>
    </b-modal>
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

<!--    Bảng biểu-->
    <b-modal
        v-model="showModalBangBieu"
        title="Bảng biểu"
        title-class="text-black font-18"
        body-class="p-3"
        hide-footer
        centered
        no-close-on-backdrop
        size="lg"
    >
      <form @submit.prevent="handleSubmitBangBieu"
            ref="formContainer">
        <div class="row">
          <div class="col-12">
            <div class="mb-3">
              <label class="text-left">Tên bảng biểu</label>
              <span style="color: red">&nbsp;*</span>
              <input type="hidden" v-model="modelBangBieu.id"/>
              <input
                  id="ten"
                  v-model.trim="modelBangBieu.ten"
                  type="text"
                  class="form-control"
                  placeholder=""
                  :class="{
                                'is-invalid':
                                  submitted && $v.modelBangBieu.ten.$error,
                              }"
              />
              <div
                  v-if="submittedBangBieu && !$v.modelBangBieu.ten.required"
                  class="invalid-feedback"
              >
                Tên bảng biểu không được để trống.
              </div>
            </div>
          </div>
          <div class="col-3">
            <div class="mb-3">
              <label class="text-left">Hiển thị tên</label>
              <br />
              <switches v-model="modelBangBieu.hienThiTen" type-bold="false" color="info" class="mt-2"></switches>
            </div>
          </div>
          <div class="col-6">
            <div class="mb-3">
              <label class="text-left">Đơn vị tính</label>
              <multiselect
                  :multiple="false"
                  v-model="modelBangBieu.donViTinh"
                  :options="optionDonViTinh"
                  track-by="id"
                  label="ten"
                  placeholder="Chọn đơn vị"
                  deselect-label="Nhấn để xoá"
                  selectLabel="Nhấn enter để chọn"
                  selectedLabel="Đã chọn"
              ></multiselect>
            </div>
          </div>
          <div class="col-3">
            <div class="mb-3">
              <label class="text-left">Nhập số thứ tự</label>
              <span style="color: red">&nbsp;*</span>
              <input
                  id="thuTu"
                  v-model="modelBangBieu.thuTu"
                  type="number"
                  min="0"
                  oninput="validity.valid||(value='');"
                  class="form-control"
                  placeholder="Nhập số thứ tự"
                  :class="{
                                'is-invalid':
                                  submittedBangBieu && $v.modelBangBieu.thuTu.$error,
                              }"
              />
              <div
                  v-if="submittedBangBieu && !$v.modelBangBieu.thuTu.required"
                  class="invalid-feedback"
              >
                Thứ tự không được để trống.
              </div>
            </div>
          </div>
        </div>
        <div class="text-end pt-2 mt-3">
          <b-button variant="light" class="w-md" size="sm" @click="handleCloseModalMauBieu">
            Đóng
          </b-button>
          <b-button type="submit" variant="primary" size="sm" class="ms-1 w-md">
            Lưu
          </b-button>
        </div>
      </form>
    </b-modal>
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


<!--    Xem bảng biểu-->
    <b-modal
        v-model="showModalXemBangBieu"
        title="Xem bảng biểu"
        title-class="text-black font-18"
        body-class="p-3"
        hide-footer
        centered
        no-close-on-backdrop
        size="xl"
    >
      <table class="dynamic-table" style="width: 100%">
        <thead v-if="headers">
        <tr v-for="(row, indexRow) in headers" :key="indexRow">
          <td v-for="(data, indexData) in row.tHeaderVms" :key="indexData" :rowspan="data.rowSpan"
              :colspan="data.colSpan"
              :style="{ 'width': data.width == 0?'auto':  data.width + 'px',
                     'text-align': data.fontHorizontalAlign?data.fontHorizontalAlign.id: '',
                     'font-weight': data.fontWeight?data.fontWeight.id: '',
                     'font-style': data.fontStyle?data.fontStyle.id: '',
                     }"
          >
            {{ data.tenThuocTinh }}
            <template v-if="data.ghiChu">
                               <span v-if="checkIsEmpty(data.ghiChu.kyHieu)">
                                 <template v-if="checkIsStar(data.ghiChu.kyHieu)">
                                        ({{ data.ghiChu.kyHieu }})
                                 </template>
                                       <template v-else>
                                       <sup>{{ data.ghiChu.kyHieu }}</sup>
                                 </template>

                  </span>
            </template>

            <template v-if="data.donViTinh">

              <div style="font-weight: normal; font-style: italic">
                ({{ data.donViTinh.ten }})
              </div>
            </template>
          </td>
        </tr>
        </thead>
        <tbody>
        <tr v-for="(value, index) in renderMainRowValue" :key="index" style="min-height: 50px; height: 50px"
            @click="getRowValueByKeyRow(value.keyRow)">
          <td v-for="(item, index1) in value.rowValues" :key="index1"
              style="padding: 0px 5px"
              :style="{ 'width': item.width == 0?'auto':  item.width + 'px',
                     'text-align': item.fontHorizontalAlign?item.fontHorizontalAlign.id: '',
                     'font-weight': item.fontWeight?item.fontWeight.id: '',
                     'font-style': item.fontStyle?item.fontStyle.id: '',
                     }"
          >
            {{ item.value }}
            <template v-if="item.ghiChu">
                               <span v-if="checkIsEmpty(item.ghiChu.kyHieu)">
                           <sup>{{ item.ghiChu.kyHieu }}</sup>

                  </span>
            </template>
          </td>
        </tr>
        </tbody>
      </table>
    </b-modal>

    <!--    Xem mẫu biểu-->
    <b-modal
        v-model="showModalXemMauBieu"
        title="Xem bảng biểu"
        title-class="text-black font-18"
        body-class="p-3"
        hide-footer
        centered
        no-close-on-backdrop
        size="xl"
    >
      <div class="row mb-5">
        <div class="col-3">
        <strong> Biểu số {{model.kyHieu}}</strong>
        </div>
        <div class="col-6 text-center">
         <strong style="font-size: 18px">{{model.ten}}</strong>
          <div style="font-style: italic">Kỳ báo cáo: Năm...</div>
          <div style="font-style: italic">(Từ ngày... tháng... năm... đến ngày... tháng... năm...)</div>
        </div>
        <div class="col-3" style="display: flex; flex-direction: column">
          <div>
            <div style="font-weight: bold">Đơn vị báo cáo:</div>
            <div v-if="model.donViBaoCao">{{model.donViBaoCao.ten}}</div>
          </div>
          <div>
            <div  style="font-weight: bold">Đơn vị nhận báo cáo: </div>
            <div v-if="model.donViNhan">{{model.donViNhan.ten}}</div>
          </div>
        </div>
      </div>
      <div v-for="(val, index) in listTableMauBieu" :key="index" class="mb-4">
        <h6 style="margin-bottom: 15px" v-if="val.isHienThiTen">{{val.tenBangBieu}}</h6>
        <table class="dynamic-table" style="width: 100%">
          <thead v-if="val.headers">
          <tr v-for="(row, indexRow) in val.headers" :key="indexRow">
            <td v-for="(data, indexData) in row.tHeaderVms" :key="indexData" :rowspan="data.rowSpan"
                :colspan="data.colSpan"
                :style="{ 'width': data.width == 0?'auto':  data.width + 'px',
                     'text-align': data.fontHorizontalAlign?data.fontHorizontalAlign.id: '',
                     'font-weight': data.fontWeight?data.fontWeight.id: '',
                     'font-style': data.fontStyle?data.fontStyle.id: '',
                     }"
            >
              {{ data.tenThuocTinh }}
              <template v-if="data.ghiChu">
                               <span v-if="checkIsEmpty(data.ghiChu.kyHieu)">
                                 <template v-if="checkIsStar(data.ghiChu.kyHieu)">
                                        ({{ data.ghiChu.kyHieu }})
                                 </template>
                                       <template v-else>
                                       <sup>{{ data.ghiChu.kyHieu }}</sup>
                                 </template>

                  </span>
              </template>

              <template v-if="data.donViTinh">

                <div style="font-weight: normal; font-style: italic">
                  ({{ data.donViTinh.ten }})
                </div>
              </template>
            </td>
          </tr>
          </thead>
          <tbody>
          <tr v-for="(value, index) in val.body" :key="index" style="min-height: 50px; height: 50px"
              @click="getRowValueByKeyRow(value.keyRow)"
          >
            <td v-for="(item, index1) in value.rowValues" :key="index1"
                style="padding: 0px 5px"
                :style="{ 'width': item.width == 0?'auto':  item.width + 'px',
                     'text-align': item.fontHorizontalAlign?item.fontHorizontalAlign.id: '',
                     'font-weight': item.fontWeight?item.fontWeight.id: '',
                     'font-style': item.fontStyle?item.fontStyle.id: '',
                     }"
            >
              {{ item.value }}
              <template v-if="item.ghiChu">
                               <span v-if="checkIsEmpty(item.ghiChu.kyHieu)">
                           <sup>{{ item.ghiChu.kyHieu }}</sup>

                  </span>
              </template>
            </td>
          </tr>
          </tbody>
        </table>
      </div>

    </b-modal>
  </Layout>
</template>

<script>
import appConfig from "@/app.config.json";
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import {mauBieuModel} from "@/models/mauBieuModel";
import {required} from "vuelidate/lib/validators";
import {notifyModel} from "@/models/notifyModel";
import {CONSTANTS} from "@/helpers/constants";
import {pagingModel} from "@/models/pagingModel";
import {bangBieuModel} from "@/models/bangBieuModel";
import Switches from "vue-switches";
import Multiselect from "vue-multiselect";

export default {
  page: {
    title: "Thiết kế mẫu biểu",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, Switches, Multiselect},
  data(){
    return{
      title: "Thiết kế mẫu biểu",
      items: [
        {
          text: "Thiết kế mẫu biểu",
          href: "/thiet-ke-mau-bieu",
          // active: true,
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
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
          label: "Ký hiệu",
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
      showModalMauBieu: false,
      showDeleteModal: false,
      showModalBangBieu: false,
      showDeleteModalBangBieu: false,
      model: mauBieuModel.baseJson(),
      modelBangBieu: bangBieuModel.baseJson(),
      listBangBieu: [],
      submitted: false,
      optionDonViTinh: [],
      optionLoaiMauBieu: [],
      optionDonVis: [],
      showModalXemBangBieu: false,
      showModalXemMauBieu: false,
      renderMainRowValue: [],
      headers: [],
      listTable: [],
      listTableMauBieu: [],
      optionCoQuans: []
    }
  },
  validations: {
    model: {
      ten: {required},
      kyHieu: {required},
      thuTu: {required}
    },
    modelBangBieu: {
      ten: {required},
      thuTu: {required}
    },
  },
  created() {
    this.getDonViTinh();
    this.getLoaiMauBieu();
    this.getDonVi();
    this.getCoQuan();
  },
  methods: {
   async handleXemMauBieu(){
      this.showModalXemMauBieu = true;
      this.renderTableMauBieu(this.model.id);
    },
    checkIsStar(value) {
      return value && value.length > 0 && value.includes('*');
    },
    checkIsEmpty(value) {
      return value && value.length > 0;
    },
    async renderTable(id){
      try {
        await this.$store.dispatch("mauBieuStore/renderTable", id).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.listTable.push(items);
          }
          return [];
        });
      } finally {
        console.log("");
      }
    },
    async renderTableMauBieu(id){
      try {
        await this.$store.dispatch("mauBieuStore/renderTableMauBieu", id).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.listTableMauBieu = items;
          }
          return [];
        });
      } finally {
        console.log("");
      }
    },
    async renderBodyMainByBangBieuId(id) {
      try {
        await this.$store.dispatch("rowValueStore/renderBodyMainByBangBieuId", id).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.renderMainRowValue = items;
          }
          return [];
        });
      } finally {
        console.log("");
      }
    },
    async renderHeader(id) {
      try {
        await this.$store.dispatch("bangBieuStore/renderHeader",id).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.headers = items;
            console.log("header", this.headers);
          }
        });
      } finally {
        console.log("");
      }
    },
    handleXemBangBieu(id){
      this.showModalXemBangBieu = true;
      this.renderBodyMainByBangBieuId(id);
      this.renderHeader(id);
    },
    async getDonVi() {
      try {
        await this.$store.dispatch("donViStore/get").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.optionDonVis = items;
          }
          return [];
        });
      } finally {
        console.log("");
      }
    },
    async getCoQuan() {
      try {
        await this.$store.dispatch("coQuanStore/get").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.optionCoQuans = items;
          }
          return [];
        });
      } finally {
        console.log("");
      }
    },
    async getDonViTinh() {
      try {
        await this.$store.dispatch("donViTinhStore/getAll").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.optionDonViTinh = items;
          }
          return [];
        });
      } finally {
       console.log("");
      }
    },
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
    handleCreate(){
      this.model = mauBieuModel.baseJson();
      this.showModalMauBieu = true;
    },
    handleCloseModalMauBieu(){
      this.model = mauBieuModel.baseJson();
      this.showModalMauBieu = false;
    },

    async handleUpdate(id) {
      await this.$store.dispatch("mauBieuStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = mauBieuModel.fromJson(res.data);
          this.showModalMauBieu = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          this.$refs.tblList.refresh()
        }
      });
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("mauBieuStore/delete", this.model.id).then((res) => {
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
    async handleSubmit(e) {
      e.preventDefault();
      this.submitted = true;
      this.$v.$touch();
      console.log(this.$v.model.$invalid);
      if (this.$v.model.$invalid) {
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
          await this.$store.dispatch("mauBieuStore/update", this.model).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.fnGetList();
              this.showModalMauBieu = false;
              this.$refs.tblList.refresh()
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        } else {
          this.model.isTemplate = true;
          // Create model
          await this.$store.dispatch("mauBieuStore/create", this.model).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.fnGetList();
              this.showModalMauBieu = false;
              this.$refs.tblList.refresh()
              this.model = mauBieuModel.baseJson();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        }
        loader.hide();
      }
      this.submitted = false;
    },
    handleSelectChangeLoaiMauBieu(selectedOption, id){
      this.$refs.tblList.refresh();
    },
    myProvider (ctx) {
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
        let promise =  this.$store.dispatch("mauBieuStore/getPagingParams", params)
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
    ///BangBieu
    handlePushThietKe(id){
      this.$router.push("/thiet-ke-mau-bieu/thiet-ke/" + id);
    },
    handleCreateBangBieu(){
      this.modelBangBieu = bangBieuModel.baseJson();
      this.modelBangBieu.mauBieuId = this.model.id ;
      this.showModalBangBieu = true;
    },
    async handleGetBangBieu(id, data) {
      await this.$store.dispatch("bangBieuStore/getBangBieuByMauBieuId", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = data;
          this.listBangBieu = res.data;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          this.$refs.tblList.refresh()
        }
      });
    },
    async handleSubmitBangBieu(e) {
      e.preventDefault();
      this.submittedBangBieu = true;
      this.$v.$touch();
      if (this.$v.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        if (
            this.modelBangBieu.id != 0 &&
            this.modelBangBieu.id != null &&
            this.modelBangBieu.id
        ) {
          // Update model
          await this.$store.dispatch("bangBieuStore/update", this.modelBangBieu).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              // this.fnGetList();
              this.showModalBangBieu = false;
              console.log(this.model.id, this.model)
              this.handleGetBangBieu(this.model.id, this.model);
              // this.$refs.tblList.refresh()
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        } else {
          this.model.isTemplate = true;
          // Create model
          await this.$store.dispatch("bangBieuStore/create", this.modelBangBieu).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.handleGetBangBieu(this.model.id, this.model);
              this.showModalBangBieu = false;
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        }
        loader.hide();
      }
      this.submitted = false;
    },
  }

}
</script>

<style>
.dynamic-table table {
  border-collapse: collapse;

  width: 100%;
}

.dynamic-table td {
  border: 1px solid;
}
.td-stt {
  text-align: center;
  width: 50px;
}
.td-xuly {
  text-align: center;
  width: 100px;
}
.td-thuTu{
  text-align: center;
  width: 70px;
}
.table>tbody> tr >td{
  padding: 0px;
  line-height: 30px;
}
.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}
</style>
