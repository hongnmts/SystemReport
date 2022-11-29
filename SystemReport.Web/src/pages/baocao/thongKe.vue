<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="row">
              <div class="col-3">
                <label class="form-label">Năm</label>
                <br/>
                <date-picker
                    v-model="paramMauBieu.nam"
                    type="year"
                    lang="vi"
                ></date-picker>

              </div>
              <div class="col-3">
                <label class="form-label">Loại báo cáo</label>
                <br/>
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
              </div>
            </div>

          </div>
        </div>
      </div>
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div style="display: flex; justify-content: space-between" class="mb-2">
              <h5>Danh sách mẫu biểu</h5>
              <!--              <b-button-->
              <!--                  variant="primary"-->
              <!--                  type="button"-->
              <!--                  class="btn w-md btn-primary"-->
              <!--                  @click="showModalPhanMauBieu = true"-->
              <!--                  size="sm"-->
              <!--              >-->
              <!--                <i class="mdi mdi-plus me-1"></i> Mẫu biểu-->
              <!--              </b-button>-->
            </div>
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
                      tbody-tr-class="b-table-tonghop"
                  >
                    <template v-slot:cell(STT)="data">
                      {{ data.index + ((currentPage - 1) * perPage) + 1 }}
                    </template>
                    <template v-slot:cell(kyHieu)="data">
                      <div class="px-2"> {{ data.item.kyHieu }}</div>
                    </template>
                    <template v-slot:cell(ten)="data">
                      <div style="cursor: pointer" class="px-2"> {{ data.item.ten }}</div>
                    </template>
                    <template v-slot:cell(loaiMauBieu)="data">
                      <div style="text-align: center;cursor: pointer" class="px-2" v-if="data.item.loaiMauBieu">
                        {{ data.item.loaiMauBieu.tenNgan }}
                      </div>
                    </template>
                    <template v-slot:cell(process)="data">
                      <!--                      <button-->
                      <!--                          type="button"-->
                      <!--                          size="sm"-->
                      <!--                          class="btn btn-outline btn-sm"-->
                      <!--                          data-toggle="tooltip" data-placement="bottom" title="Cập nhật"-->
                      <!--                          v-on:click="handleUpdate(data.item.id)">-->
                      <!--                        <i class="fas fa-pencil-alt text-success me-1"></i>-->
                      <!--                      </button>-->
                      <button
                          type="button"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Chi tiết"
                          v-b-toggle="'collapse-' + data.item.id"
                          @click="toggleDetails(data.item)"
                      >
                        <i class="fas fa-eye text-primary me-1"></i>
                      </button>
                      <!--                      <button-->
                      <!--                          v-if="data.item.lastStatus && (data.item.lastStatus.code == 'NL')"-->
                      <!--                          type="button"-->
                      <!--                          size="sm"-->
                      <!--                          class="btn btn-outline btn-sm"-->
                      <!--                          data-toggle="tooltip" data-placement="bottom" title="Xóa"-->
                      <!--                          v-on:click="handleShowDeleteModal(data.item.id)">-->
                      <!--                        <i class="fas fa-trash-alt text-danger me-1"></i>-->
                      <!--                      </button>-->

                    </template>
                    <template #row-details="data">
                      <b-collapse :visible="activeId == data.item.id">
                        <b-card class="show-detail-vbd mt-1">
                          <b-tabs content-class="p-3 bg-white">
                            <b-tab active class="border-0">
                              <template v-slot:title>
                                <span class="d-inline-block d-sm-none">
                                  <i class="fas fa-file-alt"></i>
                                </span>
                                <span class="d-none d-sm-inline-block">Bảng biểu</span>
                              </template>
                              <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                  <div class="header-thiet-ke-mau-bieu mb-1"
                                       style="display: flex; justify-content: space-between">
                                    <h5 class="pe-1">Biểu mẫu: <i> {{ data.item.ten }} </i></h5>
<!--                                    <div-->
<!--                                        style="display: flex; flex-direction: column; justify-content: center; align-items: end">-->
<!--                                      <b-button-->
<!--                                          variant="info"-->
<!--                                          type="button"-->
<!--                                          class="btn w-lg btn-primary mb-2"-->
<!--                                          @click="handleTongHop(data.item.id)"-->
<!--                                          size="sm"-->
<!--                                      >-->
<!--                                        <i class="mdi mdi-eye-check me-1"></i> Tổng hợp-->
<!--                                      </b-button>-->
<!--                                    </div>-->
                                  </div>
                                  <div class="row" v-if="data.item.bangBieus.length > 0">
                                    <div class="col-12 ">
                                      <div class="card p-2" style="margin-bottom: 4px" v-for="(value, index) in data.item.bangBieus"
                                           :key="index">
                                        <div class="card-body">
                                          <h4 class="card-title"><h4 class="card-title">#{{ index + 1 }}.
                                            {{ value.ten }}</h4></h4>
                                          <p>
                                            <b-button
                                                variant="info"
                                                type="button"
                                                class="btn w-lg btn-primary mb-2"
                                                @click="handleTongHop(data.item.id)"
                                                size="sm"
                                            >
                                              <i class="mdi mdi-eye-check me-1"></i> Tổng hợp
                                            </b-button>
                                          </p>
                                          <table class="table b-table table-striped table-bordered">
                                            <thead>
                                            <tr>
                                              <td style="width: 50px; text-align: center">
                                              </td>
                                              <td style="width: 50px; text-align: center">
                                                STT
                                              </td>
                                              <td style="text-align: center">
                                                Loại báo cáo
                                              </td>
                                              <td style="text-align: center">
                                                Từ ngày
                                              </td>
                                              <td style="text-align: center">
                                                Đến ngày
                                              </td>
                                              <td style="text-align: center">
                                                Trạng thái
                                              </td>
                                              <td>

                                              </td>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            <tr v-for="(item, index) in value.kyBaos" :key="index">
                                              <td style="text-align: center">
                                                <input type="checkbox" :value="item.id" v-model="selected" />
                                              </td>
                                              <td style="text-align: center">{{ index + 1 }}</td>
                                              <td style="text-align: center">
                                                <template v-if="item.kyBaoCao">
                                                  {{ item.kyBaoCao.ten }}
                                                </template>
                                                <template v-else>
                                                  Tổng hợp
                                                </template>
                                              </td>
                                              <td style="text-align: center">{{ item.tuNgay }}</td>
                                              <td style="text-align: center">{{ item.denNgay }}</td>
                                              <td>
                                                <div
                                                    style="display: flex; justify-content: center; align-items: center; flex-direction: column">
                                                  <div>
                                                    <span v-if="item.trangThai" class="badge" :class="'bg-' + item.trangThai.bgColor"> {{
                                                        item.trangThai.ten
                                                      }}</span>
                                                  </div>
                                                </div>
                                              </td>
                                              <td style="text-align: center">
                                                <button
                                                    type="button"
                                                    size="sm"
                                                    class="btn btn-edit btn-sm"
                                                    data-toggle="tooltip" data-placement="bottom" title="Xem bảng biểu"
                                                    v-on:click="handleXemBangBieu(item.id)">
                                                  <i class="mdi mdi-eye me-1"></i>
                                                </button>
                                                <button
                                                    type="button"
                                                    size="sm"
                                                    class="btn btn-edit btn-sm"
                                                    data-toggle="tooltip" data-placement="bottom" title="Chỉnh sửa"
                                                    v-on:click="$router.push('/thuc-hien-nhap-lieu/' + item.id)">
                                                  <i class="fas fa-pencil-alt text-success me-1"></i>
                                                </button>
                                              </td>
                                            </tr>
                                            </tbody>
                                          </table>
                                        </div>
                                      </div>
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </b-tab>
                            <b-tab class="border-0">
                              <template v-slot:title>
                                <div @click="getLichSu">
                                                                  <span class="d-inline-block d-sm-none">
                                  <i class="fas fa-file-alt"></i>
                                </span>
                                  <span class="d-none d-sm-inline-block">Lịch sử</span>
                                </div>

                              </template>
                              <div class="row">
                                <div class="col-12">
                                  <div class="row mb-2">
                                    <div class="col-sm-12 col-md-6">
                                      <div id="tickets-table_length" class="dataTables_length">
                                        <label class="d-inline-flex align-items-center">
                                          Hiện
                                          <b-form-select
                                              class="form-select form-select-sm"
                                              v-model="historyOption.perPage"
                                              size="sm"
                                              :options="historyOption.pageOptions"
                                          ></b-form-select
                                          >&nbsp;dòng
                                        </label>
                                      </div>
                                    </div>
                                  </div>
                                  <div class="table-responsive-sm">
                                    <b-table
                                        class="datatables custom-table"
                                        :items="myProviderHistory"
                                        :fields="fieldsHistory"
                                        striped
                                        bordered
                                        responsive="sm"
                                        :per-page="historyOption.perPage"
                                        :current-page="historyOption.currentPage"
                                        :sort-by.sync="historyOption.sortBy"
                                        :sort-desc.sync="historyOption.sortDesc"
                                        :filter="historyOption.filter"
                                        :filter-included-fields="historyOption.filterOn"
                                        ref="tblList"
                                        primary-key="id"
                                        :busy.sync="historyOption.isBusy"
                                        tbody-tr-class="b-table-kiemtra"
                                    >
                                      <template v-slot:cell(STT)="data">
                                        {{ data.index + ((historyOption.currentPage - 1) * historyOption.perPage) + 1 }}
                                      </template>

                                      <template v-slot:cell(status)="data">
                                        <div
                                            style="display: flex; justify-content: center; align-items: center; flex-direction: column">
                                          <div>
                                      <span v-if="data.item.status" class="badge"
                                            :class="'bg-' + data.item.status.bgColor"> {{
                                          data.item.status.ten
                                        }}</span>
                                          </div>
                                        </div>


                                      </template>
                                      <template v-slot:cell(process)="data">
                                        <button
                                            v-if="data.item.jsonData"
                                            type="button"
                                            class="btn btn-outline btn-sm"
                                            data-toggle="tooltip" data-placement="bottom" title="Chi tiết"
                                            v-b-toggle="'collapse-' + data.item.id"
                                            @click="showBangBieuHistory(data.item.id)"
                                        >
                                          <i class="fas fa-eye text-primary me-1"></i>
                                        </button>
                                      </template>
                                    </b-table>
                                    <template v-if="historyOption.isBusy">
                                      <div align="center">Đang tải dữ liệu</div>
                                    </template>
                                    <template v-if="historyOption.totalRows <= 0 && !historyOption.isBusy">
                                      <div align="center">Không có dữ liệu</div>
                                    </template>
                                  </div>
                                  <div class="row">
                                    <b-col>
                                      <div>Hiển thị {{ historyOption.numberOfElement }} trên tổng số
                                        {{ historyOption.totalRows }} dòng
                                      </div>
                                    </b-col>
                                    <div class="col">
                                      <div
                                          class="dataTables_paginate paging_simple_numbers float-end">
                                        <ul class="pagination pagination-rounded mb-0">
                                          <!-- pagination -->
                                          <b-pagination
                                              v-model="historyOption.currentPage"
                                              :total-rows="historyOption.totalRows"
                                              :per-page="historyOption.perPage"
                                          ></b-pagination>
                                        </ul>
                                      </div>
                                    </div>
                                  </div>

                                </div>
                              </div>
                            </b-tab>
                          </b-tabs>
                        </b-card>
                      </b-collapse>

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
          <hr/>
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
          <hr/>
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
              <br/>
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
        title="Thông tin bảng biểu"
        title-class="text-black font-18"
        body-class="p-3"
        hide-footer
        centered
        no-close-on-backdrop
        size="xl"
    >
      <b-button
          variant="info"
          type="button"
          class="btn w-md btn-primary"
          @click="exportTableToExcel('dynamic-table2', '')"
          size="sm"
          style="margin-bottom: 10px"
      >
        <i class="mdi mdi-plus me-1"></i> Export
      </b-button>
      <div id="dynamic-table2">
        <br/>
        <table class="dynamic-table">
          <thead v-if="headers">
          <tr v-for="(row, indexRow) in headers" :key="indexRow">
            <td v-for="(data, indexData) in row.tHeaderVms" :key="indexData" :rowspan="data.rowSpan"
                :colspan="data.colSpan"
                :style="{ 'width': data.width == 0?'auto':  data.width + 'px',
                     'text-align': data.fontHorizontalAlign?data.fontHorizontalAlign.id: '',
                     'font-weight': data.fontWeight?data.fontWeight.id: '',
                     'font-style': data.fontStyle?data.fontStyle.id: '',
                     }"
                style=" border: 1px solid #a1a0a0"
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
                :style="{ 'width': item.width == 0?'auto':  item.width + 'px',
                     'text-align': item.fontHorizontalAlign?item.fontHorizontalAlign.id: '',
                     'font-weight': item.fontWeight?item.fontWeight.id: '',
                     'font-style': item.fontStyle?item.fontStyle.id: '',
                     }"
                style="padding: 0px 5px; border: 1px solid #a1a0a0"
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

    <!--    Xem mẫu biểu-->
    <b-modal
        v-model="showModalXemMauBieu"
        title="Thông tin mẫu biểu"
        title-class="text-black font-18"
        body-class="p-3"
        hide-footer
        centered
        no-close-on-backdrop
        size="xl"
    >
      <b-button
          variant="info"
          type="button"
          class="btn w-md btn-primary mb-2"
          @click="exportTableToExcel('dynamic-table1', model.kyHieu+'_' +model.ten)"
          size="sm"
      >
        <i class="mdi mdi-plus me-1"></i> Export
      </b-button>
      <div id="dynamic-table1">
        <table>
          <tr>
            <td>
              <strong> Biểu số </strong>
            </td>
            <td colspan="4" style="text-align: center">
              <strong style="font-size: 18px">{{ model.ten }}</strong>


            </td>
            <td colspan="2">
              <div>
                <div style="font-weight: bold">Đơn vị báo cáo:</div>
                <div v-if="model.donViBaoCao">{{ model.donViBaoCao.ten }}</div>
              </div>

            </td>
          </tr>
          <tr>
            <td>{{ model.kyHieu }}</td>
            <td style="text-align: center" colspan="4">
              <div v-if="model.nam" style="font-style: italic">Kỳ báo cáo: Năm {{ model.nam }}</div>
              <div v-if="model.tuNgayDenNgay" style="font-style: italic">({{ model.tuNgayDenNgay }})</div>
            </td>
            <td colspan="2">
              <div>
                <div style="font-weight: bold">Đơn vị nhận báo cáo:</div>
                <div v-if="model.donViNhan">{{ model.donViNhan.ten }}</div>
              </div>
            </td>
          </tr>
        </table>
        <!--        <div class="row mb-4" >-->
        <!--          <div class="col-3">-->
        <!--            <strong> Biểu số {{model.kyHieu}}</strong>-->
        <!--          </div>-->
        <!--          <div class="col-6 text-center">-->
        <!--            <strong style="font-size: 18px">{{model.ten}}</strong>-->

        <!--            <div v-if="model.nam" style="font-style: italic">Kỳ báo cáo: Năm {{model.nam}}</div>-->
        <!--            <div v-if="model.tuNgayDenNgay" style="font-style: italic">({{model.tuNgayDenNgay}})</div>-->
        <!--          </div>-->
        <!--          <div class="col-3" style="display: flex; flex-direction: column">-->
        <!--            <div>-->
        <!--              <div style="font-weight: bold">Đơn vị báo cáo:</div>-->
        <!--              <div v-if="model.donViBaoCao">{{model.donViBaoCao.ten}}</div>-->
        <!--            </div>-->
        <!--            <div>-->
        <!--              <div  style="font-weight: bold">Đơn vị nhận báo cáo: </div>-->
        <!--              <div v-if="model.donViNhan">{{model.donViNhan.ten}}</div>-->
        <!--            </div>-->
        <!--          </div>-->
        <!--        </div>-->
        <div v-for="(val, index) in listTableMauBieu" :key="index" class="mb-4">
          <h6 style="margin-bottom: 15px" v-if="val.isHienThiTen">{{ val.tenBangBieu }}</h6>
          <table class="dynamic-table">
            <thead v-if="val.headers">
            <tr v-for="(row, indexRow) in val.headers" :key="indexRow">
              <td v-for="(data, indexData) in row.tHeaderVms" :key="indexData" :rowspan="data.rowSpan"
                  :colspan="data.colSpan"
                  style=" border: 1px solid #a1a0a0"
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
                  style="padding: 0px 5px; border: 1px solid #a1a0a0"
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

      </div>

    </b-modal>

    <!--    Danh sách mẫu biểu được phân quyền-->
    <b-modal
        v-model="showModalPhanMauBieu"
        centered
        title="Danh sách mẫu biểu"
        title-class="font-18"
        no-close-on-backdrop
        hide-footer
        size="xl"
    >
      <div class="row">

        <div class="col-3">
          <label class="form-label"> Kỳ báo cáo</label>
          <br/>
          <multiselect
              :multiple="false"
              v-model="modelInputMauBieu.kyBaoCao"
              :options="optionKyBaoCao"
              track-by="id"
              label="ten"
              placeholder="Chọn loại kỳ báo cáo"
              deselect-label="Nhấn để xoá"
              selectLabel="Nhấn enter để chọn"
              selectedLabel="Đã chọn"
              class="mb-2"
          ></multiselect>

        </div>
        <template
            v-if="modelInputMauBieu.kyBaoCao != '' && modelInputMauBieu.kyBaoCao != null && modelInputMauBieu.kyBaoCao != undefined">
          <div class="col-3 " v-if="modelInputMauBieu.kyBaoCao && modelInputMauBieu.kyBaoCao.loaiKyBaoCao == 'nam'">
            <label class="form-label"> Thời gian</label>
            <br/>
            <date-picker
                v-model="modelInputMauBieu.nam"
                type="year"
                lang="vi"
                placeholder="Chọn năm"
            ></date-picker>
          </div>
          <div v-else class="col-6">
            <label class="form-label"> Thời gian</label>
            <br/>
            <div style="display: flex;">
              <multiselect
                  :multiple="false"
                  v-model="modelInputMauBieu.thang"
                  :options="thangModel"
                  track-by="id"
                  label="name"
                  placeholder="Chọn tháng"
                  deselect-label="Nhấn để xoá"
                  selectLabel="Nhấn enter để chọn"
                  selectedLabel="Đã chọn"
                  class="mb-2"
                  style="margin-right: 10px"
              ></multiselect>
              <date-picker
                  v-model="modelInputMauBieu.nam"
                  type="year"
                  lang="en"
                  placeholder="Chọn tháng năm"
              ></date-picker>

            </div>

          </div>
        </template>
        <table-mau-bieu :refreshTableMauBieu="refreshTableMauBieu" :showModalInputMauBieu.sync="showModalPhanMauBieu"
                        :modelInput.sync="modelInputMauBieu"/>
      </div>
    </b-modal>


    <b-modal
        v-model="showTrangThaiModal"
        centered
        title="Chi tiết VB"
        title-class="fw-bold fs-5 text-dark"
        no-close-on-backdrop
        size="lg"
        ref="refshowCheckVanBanModal"
        hide-footer
    >
      <div class="row" style="width: 100%; margin: 0">
        <div class="col-md-12 capso-container">
          <div class="title-capso">Mẫu biểu:</div>
          <div class="content-capso">{{ model.ten }}</div>
        </div>
        <div class="col-md-12 capso-container">
          <div class="title-capso">Kỳ báo cáo:</div>
          <div class="content-capso" v-if="model.kyBaoCao">{{ model.showKyBaoCao }}</div>
        </div>
        <div class="col-md-6 capso-container">
          <div class="title-capso">Từ ngày:</div>
          <div class="content-capso" v-if="model.tuNgay">{{ model.tuNgay }}</div>
        </div>
        <div class="col-md-6 mb-3 capso-container">
          <div class="title-capso">Đến ngày:</div>
          <div class="content-capso" v-if="model.denNgay">{{ model.denNgay }}</div>
        </div>
        <hr/>
        <div class="mb-3" style="padding: 0px">
          <label class="form-label title-capso">Lý do</label>
          <div>
                  <textarea
                      v-model="modelTrangThai.noiDung"
                      class="form-control"
                      name="textarea"

                  ></textarea>
          </div>
        </div>
      </div>
      <template #modal-header="{  }">
        <!-- Emulate built in modal header close button action -->
        <h5 style="min-width: 200px">Xử lý mẫu biểu</h5>
        <div style="width: 100%; display: flex; justify-content: flex-end" class="text-end">
          <div v-if="optionsTrangThai && optionsTrangThai.length" style="display: flex">
            <div v-for="(value, index) in optionsTrangThai" :key="index">

              <b-button type="button" :class="'btn-' + value.bgColor" class="ms-1" style="min-width: 80px;"
                        size="sm"
                        @click="handleChuyenTrangThaiVanBan(value)"
              >
                {{ value.ten }}
              </b-button>
            </div>

          </div>

          <b-button variant="light" class="ms-1" size="sm" style="width: 80px"
                    @click="showTrangThaiModal = false">
            Đóng
          </b-button>
        </div>
      </template>
    </b-modal>
    <history :historyId.sync="historyId"></history>

    <!--       Nội dung từ chối-->
    <b-modal
        v-model="showModalNoiDungTuChoi"
        centered
        title="Lý do không duyệt văn bản"
        title-class="fw-bold fs-5 text-dark"
        hide-header
        hide-footer
        no-close-on-backdrop
    >
      <div class="d-flex justify-content-between align-items-center mb-3">
        <p class="fw-bold">Lý do</p>
        <b-button pill v-b-modal.modal-close_visit size="sm" class=""
                  v-on:click="showModalNoiDungTuChoi = false">
          <i class="fas fa-times mx-1"></i>
        </b-button>
      </div>
      <div class="row">
        <div class="col-4 text-center">
          <img src="@/assets/images/icon-tu-choi.png" class="w-100" alt="">
        </div>
        <div v-if="dataNoiDungTuChoi" class="col-8">
          <div class="text-black d-flex justify-content-between">
            <p class="fw-medium">Ngày xử lý:</p>
            <p
                class="text-primary px-2"
                style="background-color: rgb(66 153 225 / 10%); border-radius: 3px;"
            >{{ dataNoiDungTuChoi.lastModifiedShow }}</p>
          </div>
          <div class="text-black">
            <p class="fw-medium">Nội dung:</p>
            <p class="">
              {{ dataNoiDungTuChoi.lastStatus.content }}
            </p>
          </div>
        </div>
      </div>

      <template #modal-footer>
        <b-button v-b-modal.modal-close_visit size="sm" class="btn btn-outline-info w-md"
                  v-on:click="showModalNoiDungTuChoi = false">
          Đóng
        </b-button>
      </template>
    </b-modal>
    <b-modal
        v-model="showModalThongKe"
        centered
        title="Tổng hợp dữ liệu"
        title-class="font-18"
        no-close-on-backdrop
    >
      <p>
        Tổng hợp sẽ tạo ra dữ liệu mới từ những bảng biểu có sẳn!
      </p>
      <template #modal-footer>
        <b-button v-b-modal.modal-close_visit
                  size="sm"
                  class="btn btn-outline-info w-md"
                  v-on:click="showModalThongKe = false">
          Đóng
        </b-button>
        <b-button v-b-modal.modal-close_visit
                  size="sm"
                  variant="danger"
                  type="button"
                  class="w-md"
                  v-on:click="handleSubmitTongHop">
          Tổng hợp
        </b-button>
      </template>
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
import DatePicker from "vue2-datepicker";
import {thangModel} from "@/models/thangModel";
import TableMauBieu from "@/pages/baocao/tableMauBieu";
import {trangThaiModel} from "@/models/trangThaiModel";
import History from "@/pages/baocao/history";

export default {
  page: {
    title: "Tổng hợp",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {TableMauBieu, Layout, PageHeader, Switches, Multiselect, DatePicker, History},
  data() {
    return {
      title: "Tổng hợp",
      items: [
        {
          text: "Tổng hợp",
          href: "/tong-hop",
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
        {key: 'STT', label: 'STT', class: '', sortable: false, thClass: 'hidden-sortable'},
        {
          key: "kyHieu",
          label: "Ký hiệu",
          sortable: true,
          thClass: 'hidden-sortable',
          thStyle: {width: '100px', minWidth: '100px', 'text-align': 'center'},
        },
        {
          key: "ten",
          label: "Tên mẫu biểu",
          sortable: true,
          thStyle: "text-align:left",
          thClass: 'hidden-sortable',
        },
        {
          key: "loaiMauBieu",
          label: "Loại mẫu biểu",
          sortable: true,
          thClass: 'hidden-sortable',
          thStyle: {width: '150px', minWidth: '100px', 'text-align': 'center'},
        },
        // {
        //   key: "showKyBaoCao",
        //   label: "Kỳ báo cáo",
        //   sortable: true,
        //   thStyle: {width: '150px', minWidth: '150px', 'text-align': 'center'},
        //   thClass: 'hidden-sortable',
        // },
        // {
        //   key: "lastStatus",
        //   label: "Trạng thái",
        //   sortable: true,
        //   thClass: 'hidden-sortable',
        //   thStyle: {width: '100px', minWidth: '100px', 'text-align': 'center'},
        // },
        // {
        //   key: 'status',
        //   label: '',
        //   class: '',
        //   thClass: 'hidden-sortable',
        //   sortable: false
        // },
        {
          key: 'process',
          label: '',
          class: 'btn-process',
          thClass: 'hidden-sortable',
          sortable: false
        }
      ],
      fieldsBangBieu: [
        {key: 'STT', label: 'STT', class: 'td-stt', sortable: false, thClass: 'hidden-sortable'},
        {
          key: "ten",
          label: "Tên bảng biểu",
          sortable: true,
          thStyle: "text-align:left",
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
      optionCoQuans: [],
      showModalPhanMauBieu: false,
      optionKyBaoCao: [],
      thangModel: thangModel.calThang,
      modelInputMauBieu: {
        mauBieuId: null,
        thang: null,
        nam: null,
        kyBaoCao: null
      },
      strIdMauBieu: null,
      optionsTrangThai: [],
      showTrangThaiModal: false,
      modelTrangThai: trangThaiModel.currentBaseJson(),
      paramMauBieu: {
        mauBieuId: null,
        thang: null,
        nam: new Date(),
        kyBaoCao: null
      },
      submittedBangBieu: false,
      activeId: null,
      historyId: null,
      historyOption: {
        totalRows: 1,
        todoTotalRows: 1,
        currentPage: 1,
        numberOfElement: 1,
        perPage: 10,
        pageOptions: [10, 100],
        filter: null,
        isBusy: false,
      },
      fieldsHistory: [
        {key: 'STT', label: 'STT', class: 'td-stt', sortable: false, thClass: 'hidden-sortable'},
        {
          key: "createdAtFullShow",
          label: "Thời gian",
          sortable: true,
          thStyle: "text-align:center",
          thClass: 'hidden-sortable',

        },
        {
          key: "showFullName",
          label: "Người thao tác",
          sortable: true,
          thStyle: "text-align:center",
          thClass: 'hidden-sortable',
          class: "td-xuly-history"
        },
        {
          key: "title",
          label: "Hành động",
          sortable: true,
          thStyle: "text-align:center",
          thClass: 'hidden-sortable',
          class: "td-xuly-history"
        },
        {
          key: "status",
          label: "Trạng thái",
          sortable: true,
          thStyle: {width: '100px', minWidth: '100px', 'text-align': 'center'},
          thClass: 'hidden-sortable',
        },
        {
          key: "content",
          label: "Nội dung",
          sortable: true,
          thStyle: "text-align:center",
          thClass: 'hidden-sortable',
        },
        // {
        //   key: "collectionName",
        //   label: "Bảng dữ liệu",
        //   sortable: true,
        //   thStyle:"text-align:center",
        //   thClass: 'hidden-sortable',
        //   class: "td-xuly-history"
        // },

        {
          key: 'process',
          label: '',
          class: 'td-xuly',
          thClass: 'hidden-sortable',
          sortable: false
        }
        // {
        //   key: "thuTu",
        //   label: "Thứ tự",
        //   class: 'td-thuTu',
        //   sortable: true,
        //   thClass: 'hidden-sortable',
        // },
      ],
      dataNoiDungTuChoi: null,
      showModalNoiDungTuChoi: false,
      selected: [],
      showModalThongKe: false,
      modelMB:{
        mauBieuId: null,
        bangBieuIds: null
      }
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
    this.getKyBaoCao();
  },
  watch: {
    modelInputMauBieu: {
      deep: true,
      handler(val) {
        if (val.kyBaoCao == '' || val.kyBaoCao == null || val.kyBaoCao == undefined) {
          this.modelInputMauBieu.nam = null;
          this.modelInputMauBieu.thang = null;
        }
      }
    },
    paramMauBieu: {
      deep: true,
      handler(val) {
        this.$refs.tblList?.refresh();
      }
    },
    selected: {
      deep: true,
      handler(val) {
       console.log("selected", val)
      }
    }
  },
  methods: {
   async handleSubmitTongHop(){
     if(this.modelMB.bangBieuIds && this.modelMB.bangBieuIds.length <=1)
       this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage({resultString: "Bảng tổng hợp không bé hơn 1", resultCode: "FAIL"}))

     await this.$store.dispatch("mauBieuStore/generateMauBieuTongHop", this.modelMB).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.showModalThongKe = false;
          this.$refs.tblList?.refresh()
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },
    handleTongHop(mauBieuId){
      this.modelMB.mauBieuId = mauBieuId;
      this.modelMB.bangBieuIds = this.selected;
      this.showModalThongKe = true;
    },
    isEmpty(str) {
      return (!str || str.length === 0);
    },
    checkErrorStatus(code) {
      var arrays = ['CBKTTC', 'CBTHTC', 'LDSTC', 'THO'];

      if (arrays.includes(code)) {
        return true;
      }
      return false;
    },
    async handleShowNoiDungTuChoi(value) {
      this.dataNoiDungTuChoi = value;
      this.showModalNoiDungTuChoi = true;
    },
    exportTableToExcel(tableId, filename) {
      if (filename == '') {
        filename = Date.now();
      }
      let dataType = 'application/vnd.ms-excel';
      let extension = '.xls';

      let base64 = function (s) {
        return window.btoa(unescape(encodeURIComponent(s)))
      };

      let template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>';
      let render = function (template, content) {
        return template.replace(/{(\w+)}/g, function (m, p) {
          return content[p];
        });
      };

      let tableElement = document.getElementById(tableId);

      let tableExcel = render(template, {
        worksheet: filename,
        table: tableElement.innerHTML
      });

      filename = filename + extension;

      if (navigator.msSaveOrOpenBlob) {
        let blob = new Blob(
            ['\ufeff', tableExcel],
            {type: dataType}
        );

        navigator.msSaveOrOpenBlob(blob, filename);
      } else {
        let downloadLink = document.createElement("a");

        document.body.appendChild(downloadLink);

        downloadLink.href = 'data:' + dataType + ';base64,' + base64(tableExcel);

        downloadLink.download = filename;

        downloadLink.click();
      }
    },
    async handleChuyenTrangThaiVanBan(value) {
      if (value) {
        this.modelTrangThai.newTrangThai = value;
      }
      this.submitted = true;
      let loader = this.$loading.show({
        container: this.$refs.formContainerTrangThai,
      });
      if (
          this.modelTrangThai
          && this.modelTrangThai.mauBieuId != null
      ) {
        //Update model
        await this.$store.dispatch("mauBieuStore/changeStatus", this.modelTrangThai).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showTrangThaiModal = false;
            this.model = trangThaiModel.currentBaseJson()
            this.$refs.tblList?.refresh();
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        })
      }
      loader.hide()
      this.submitted = false;
    },
    handleChuyenTrangThai: function (currentStatus, mauBieuId, mauBieu) {
      this.getTrangThai(currentStatus)
      this.modelTrangThai.currentTrangThai = currentStatus;
      this.modelTrangThai.nextTrangThai = null;
      this.modelTrangThai.mauBieuId = mauBieuId;
      this.showTrangThaiModal = true;
      this.model = mauBieu;
    },
    async getTrangThai(currentTrangThai) {
      try {
        await this.$store.dispatch("trangThaiStore/getNextTrangThai", {
          currentTrangThai: currentTrangThai
        }).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data;
            this.optionsTrangThai = items;
            return items || []
          }
          return [];
        })
      } finally {
        this.loading = false
      }
    },
    handleThucHienNhapLieu(id) {
      this.$router.push('/thuc-hien-nhap-lieu/' + id);
    },
    refreshTableMauBieu() {
      this.$refs.tblList?.refresh();
    },
    async handleChooseMauBieu(id) {
      this.modelInputMauBieu.mauBieuId = id;
      await this.$store.dispatch("mauBieuStore/generateMauBieu", this.modelInputMauBieu).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.showModalPhanMauBieu = false;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },
    async getKyBaoCao() {
      try {
        await this.$store.dispatch("kyBaoCaoStore/get").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.optionKyBaoCao = items;
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
    async handleXemMauBieu(id) {
      this.showModalXemMauBieu = true;
      this.handleGetMauBieuId(id);
      this.renderTableMauBieu(id);
      // console.log(this.listBangBieu);
      // if( this.listBangBieu && this.listBangBieu.length > 0){
      //   this.listBangBieu.map(async (value) => {
      //     await this.renderTable(value.id);
      //   })
      // }
      //
      // if(this.listTable && this.listTable.length > 0){
      //   this.listTable
      // }
    },
    checkIsStar(value) {
      return value && value.length > 0 && value.includes('*');
    },
    checkIsEmpty(value) {
      return value && value.length > 0;
    },
    async renderTable(id) {
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
    async renderTableMauBieu(id) {
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
        await this.$store.dispatch("bangBieuStore/renderHeader", id).then(resp => {
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
    handleXemBangBieu(id) {
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
    handleCreate() {
      this.model = mauBieuModel.baseJson();
      this.showModalMauBieu = true;
    },
    handleCloseModalMauBieu() {
      this.model = mauBieuModel.baseJson();
      this.showModalMauBieu = false;
    },
    async handleGetMauBieuId(id) {
      await this.$store.dispatch("mauBieuStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = res.data
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
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
        await this.$store.dispatch("mauBieuStore/deleteMauBieu", this.model.id).then((res) => {
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
    handleChangeKyBaoCao() {

    },
    handleSelectChangeLoaiMauBieu(selectedOption, id) {
      this.$refs.tblList?.refresh();
    },
    myProvider(ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        content: this.filter,
        sortBy: ctx.sortBy,
        sortDesc: ctx.sortDesc,
        loaiMauBieu: this.loaiMauBieu,
        paramMauBieu: this.paramMauBieu
      }
      this.loading = true
      try {
        let promise = this.$store.dispatch("mauBieuStore/getMauBieuPaging", params)
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

    ///BangBieu
    handlePushThietKe(id) {
      this.$router.push("/thiet-ke-mau-bieu/thiet-ke/" + id);
    },
    handleCreateBangBieu() {
      this.modelBangBieu = bangBieuModel.baseJson();
      this.modelBangBieu.mauBieuId = this.model.id;
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
    showBangBieuHistory(id) {
      this.historyId = id;
    },
    getLichSu() {
      this.$refs.tblList?.refresh();
    },
    toggleDetails(item) {
      if (item._showDetails) { // if details are open, close them
        item._showDetails = false
        this.activeId = null;
      } else if (item) { // if details already exists, show the details
        this.$set(item, '_showDetails', true)
        this.activeId = item.id;
      }
    },
    myProviderHistory(ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        sortBy: ctx.sortBy,
        sortDesc: ctx.sortDesc,
        formKey: this.activeId
      }
      try {
        let promise = this.$store.dispatch("historyStore/mauBieu", params)
        return promise.then(resp => {
          if (resp.resultCode == CONSTANTS.SUCCESS) {
            let data = resp.data;
            this.historyOption.totalRows = data.totalRows
            let items = data.data
            this.historyOption.numberOfElement = items.length
            return items || []
          } else {
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

.td-thuTu {
  text-align: center;
  width: 70px;
}

.table > tbody > tr > td {
  padding: 0px;
  line-height: 30px;
}

.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}


.show-detail-vbd {
  background-color: #eda910;
  border: 1px solid #eda910;
  border-radius: 0px;
}

@media only screen and (max-width: 768px) {
  .show-detail-vbd {
    margin: 0px;
  }
}

.show-detail-vbd .card-body {
  padding: 0;
}

.show-detail-vbd .card-body .nav-tabs .nav-link {
  border-top-left-radius: 0px;
  border-top-right-radius: 0px;
}

.show-detail-vbd .card-body .nav-tabs .nav-link.active {
  color: #055E8F;
}

.show-detail-vbd .card-body .nav-tabs .nav-link {
  color: #fff;
}

.text-black-2a {
  color: #2a2a2a;
}

.text-black {
  color: #000;
}

.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}

@media only screen and (max-width: 768px) {
  /*
  Label the data
  */
  .b-table-tonghop > td:nth-of-type(1):before {
    content: "STT";
    font-weight: bold;
    color: #00568c;
  }

  .b-table-vbd > td:nth-of-type(2):before {
    content: "Ký hiệu";
    font-weight: bold;
    color: #00568c;
  }

  .b-table-vbd > td:nth-of-type(3):before {
    content: "Trích yếu";
    font-weight: bold;
    color: #00568c;
  }

  .b-table-tonghop > td:nth-of-type(4):before {
    content: "Tên biễu mẫu";
    font-weight: bold;
    color: #00568c;
  }

  .b-table-tonghop > td:nth-of-type(5):before {
    content: "Loại biễu mẫu";
    font-weight: bold;
    color: #00568c;
  }

  .b-table-tonghop > td:nth-of-type(6):before {
    content: "Kỳ báo cáo";
    font-weight: bold;
    color: #00568c;
  }

  .b-table-tonghop > td:nth-of-type(7):before {
    content: "Trạng thái";
    font-weight: bold;
    color: #00568c;
  }

  .b-table-tonghop > td:nth-of-type(8):before {
    content: "";
    font-weight: bold;
    color: #00568c;
  }

  .b-table-tonghop > td:nth-of-type(9):before {
    content: "";
    font-weight: bold;
    color: #00568c;
  }
}

.b-table-tonghop:hover {
  background: #ddd !important;
  position: relative;
  z-index: 99;
  --bs-table-accent-bg: #ddd;
}

.table-striped > tbody > .b-table-tonghop:nth-of-type(odd):hover {
  --bs-table-accent-bg: #ddd;
}

.dynamic-table table {
  width: 100%;
}

#dynamic-table1 table {
  width: 100%;
}

#dynamic-table2 table {
  width: 100%;
}

tr:nth-child(even) {
  background-color: #f2f2f2
}

@media only screen and (max-width: 768px) {
  #dynamic-table1 {
    overflow-x: auto;
  }

  #dynamic-table2 {
    overflow-x: auto;
  }

  #dynamic-table1 table {
    border-spacing: 0;
    border-collapse: collapse;
    width: 1440px;
    border: 1px solid #ddd;
  }

  #dynamic-table2 table {
    border-spacing: 0;
    border-collapse: collapse;
    width: 1440px;
    border: 1px solid #ddd;
  }

  #dynamic-table1 table tr th, #dynamic-table1 table tr td {
    text-align: left;
    padding: 8px;
  }

  #dynamic-table2 table tr th, #dynamic-table2 table tr td {
    text-align: left;
    padding: 8px;
  }

  #dynamic-table1 thead tr:nth-child(even) {
    background-color: #f2f2f2
  }

  #dynamic-table2 thead tr:nth-child(even) {
    background-color: #f2f2f2
  }

  .dynamic-table table {
    width: 1440px;
  }
}
</style>
