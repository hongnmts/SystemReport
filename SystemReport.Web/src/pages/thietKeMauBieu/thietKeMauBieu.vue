<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title"> Cấu hình bảng biểu</h4>
            <p class="card-title-desc"></p>
            <b-tabs nav-class="nav-tabs-custom" content-class="p-3">
              <b-tab active>
                <template v-slot:title>
                  <span class="d-inline-block d-sm-none">
                    <i class="fas fa-home"></i>
                  </span>
                  <span class="d-none d-sm-inline-block">Thuộc tính</span>
                </template>
                <div class="row">
                  <div class="col-6">
                    <div class="header-thiet-ke-mau-bieu mb-3" style="display: flex; justify-content: space-between">
                      <h5>Danh sách thuộc tính</h5>
                      <!--                      <b-button-->
                      <!--                          variant="primary"-->
                      <!--                          type="button"-->
                      <!--                          class="btn w-md btn-primary"-->
                      <!--                          @click="handleCreate"-->
                      <!--                          size="sm"-->
                      <!--                      >-->
                      <!--                        <i class="mdi mdi-plus me-1"></i> Thêm thuộc tính-->
                      <!--                      </b-button>-->
                      <!--                      @item-click="itemClick"   :item-events="itemEvents"?-->

                    </div>
                    <v-jstree :data="listTreeThuocTinh" @item-click="itemClick" :item-events="itemEvents"></v-jstree>
                  </div>
                  <div class="col-6">
                    <form @submit.prevent="handleSubmitThuocTinh"
                          ref="formContainer">
                      <div class="text-end pt-2 mt-3">
                        <b-button variant="warning" class="w-md mx-1" size="sm" @click="handleResetThuocTinh">
                          Làm mới
                        </b-button>
                        <b-button :disabled="!modelThuocTinh.ten" type="submit" variant="primary" size="sm"
                                  class="ms-1 w-md">
                          Lưu
                        </b-button>
                        <b-button v-if="modelThuocTinh.id" variant="danger" class="w-md mx-1" size="sm"
                                  @click="handleShowModalDeleteThuocTinh">
                          Xóa
                        </b-button>
                      </div>
                      <div class="row">
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Tên thuộc tính</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="modelThuocTinh.id"/>
                            <input
                                id="ten"
                                v-model.trim="modelThuocTinh.ten"
                                type="text"
                                class="form-control"
                                placeholder=""
                                :class="{
                                'is-invalid':
                                  submitted && $v.modelThuocTinh.ten.$error,
                              }"
                            />
                            <div
                                v-if="submitted && !$v.modelThuocTinh.ten.required"
                                class="invalid-feedback"
                            >
                              Tên thuộc tính không được để trống.
                            </div>
                          </div>
                        </div>

                        <div class="col-4">
                          <div class="mb-3">
                            <label class="text-left">Thuộc tính cha</label>
                            <treeselect
                                :normalizer="normalizer"
                                v-model="modelThuocTinh.parentId"
                                :options="optionsThuocTinhFilter"
                                :searchable="false"
                                :show-count="true"
                                :default-expand-level="1"
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
                        <div class="col-4">
                          <div class="mb-3">
                            <label class="text-left">Nhập số thứ tự</label>
                            <span style="color: red">&nbsp;*</span>
                            <input
                                id="thuTu"
                                v-model="modelThuocTinh.thuTu"
                                type="number"
                                min="0"
                                oninput="validity.valid||(value='');"
                                class="form-control"
                                placeholder="Nhập số thứ tự"
                                :class="{
                                'is-invalid':
                                  submitted && $v.modelThuocTinh.thuTu.$error,
                              }"
                            />
                            <div
                                v-if="submitted && !$v.modelThuocTinh.thuTu.required"
                                class="invalid-feedback"
                            >
                              Thứ tự không được để trống.
                            </div>
                          </div>
                        </div>
                        <div class="col-4">
                          <div class="mb-3">
                            <label class="text-left">Đơn vị tính</label>
                            <multiselect
                                :multiple="false"
                                v-model="modelThuocTinh.donViTinh"
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
                        <div class="col-4">
                          <div class="mb-3">
                            <label class="text-left">Loại thuộc tính</label>
                            <multiselect v-model="modelThuocTinh.styleInput" :options="styleInput" label="name"
                                         track="id" :searchable="false" :show-labels="false"
                                         placeholder="Chọn loại thuộc tính"
                                         :class="{
                                'is-invalid':
                                  submitted && $v.modelThuocTinh.styleInput.$error,
                              }"
                            ></multiselect>
                          </div>
                        </div>
                        <div class="col-4">
                          <div class="mb-3">
                            <label class="text-left">Chỉ tiêu</label>
                            <br/>
                            <switches v-model="modelThuocTinh.isChiTieu"
                                      type-bold="false" color="info" class="mt-2"></switches>
                          </div>
                        </div>
                        <div class="col-4">
                          <div class="mb-3">
                            <label class="text-left">Tính chỉ tiêu con</label>
                            <br/>
                            <switches  v-model="modelThuocTinh.tinhChiTieuCon"
                                      type-bold="false" color="primary" class="mt-2"></switches>
                          </div>
                        </div>
                        <div class="col-6" v-if="modelThuocTinh.styleInput && modelThuocTinh.styleInput.id == 'formula'">
                          <div class="mb-3">
                            <label class="text-left">Nhập công thức</label>
                            <input class="form-control" v-model="modelThuocTinh.stringCongThuc" />
                          </div>
                        </div>
                        <hr/>
                        <h6>Ghi chú</h6>
                        <div class="col-2">
                          <div class="mb-3">
                            <label class="text-left">Ký hiệu</label>
                            <input
                                v-model.trim="modelThuocTinh.ghiChu.kyHieu"
                                type="text"
                                class="form-control"
                                placeholder=""
                            />
                          </div>
                        </div>
                        <div class="col-10">
                          <div class="mb-3">
                            <label class="text-left">Nội dung</label>
                            <textarea
                                v-model.trim="modelThuocTinh.ghiChu.noiDung"
                                type="text"
                                class="form-control"
                                placeholder=""
                            />
                          </div>
                        </div>
                        <hr/>
                        <div class="col-3">
                          <div class="mb-3">
                            <label class="text-left">Chiều rộng</label>
                            <!--                            <span style="color: red">&nbsp;px * (Mặc định bằng 0 thì chiều rộng sẽ là auto)</span>-->
                            <input
                                v-model="modelThuocTinh.width"
                                type="number"
                                min="0"
                                oninput="validity.valid||(value='');"
                                class="form-control"
                                placeholder="Nhập số thứ tự"
                                :class="{
                                'is-invalid':
                                  submitted && $v.modelThuocTinh.width.$error,
                              }"
                            />
                            <div
                                v-if="submitted && !$v.modelThuocTinh.width.required"
                                class="invalid-feedback"
                            >
                              Chiều rộng không được để trống.
                            </div>
                          </div>
                        </div>
                        <div class="col-3">
                          <div class="mb-3">
                            <label class="text-left">Kiểu chữ</label>
                            <multiselect v-model="modelThuocTinh.fontStyle" :options="fontStyle" label="name" track="id"
                                         :searchable="false" :show-labels="false"
                                         placeholder="Chọn kiểu chữ"></multiselect>
                          </div>
                        </div>
                        <div class="col-3">
                          <div class="mb-3">
                            <label class="text-left">Phông chữ</label>
                            <multiselect v-model="modelThuocTinh.fontWeight" :options="fontWeight" label="name"
                                         track="id" :searchable="false" :show-labels="false"
                                         placeholder="Chọn phông chữ"></multiselect>
                          </div>
                        </div>
                        <div class="col-3">
                          <div class="mb-3">
                            <label class="text-left">Canh chiều ngang</label>

                            <multiselect v-model="modelThuocTinh.fontHorizontalAlign" :options="fontHorizontalAlign"
                                         label="name"
                                         :class="{
                                'is-invalid':
                                  submitted && $v.modelThuocTinh.fontHorizontalAlign.$error,
                              }"
                            ></multiselect>
                            <div
                                v-if="submitted && !$v.modelThuocTinh.fontHorizontalAlign.required"
                                class="invalid-feedback"
                            >
                              Canh ngang không được để trống.
                            </div>
                          </div>
                        </div>
                      </div>

                    </form>
                  </div>
                </div>
              </b-tab>
              <b-tab>
                <template v-slot:title>
                  <span class="d-inline-block d-sm-none">
                    <i class="far fa-user"></i>
                  </span>
                  <span class="d-none d-sm-inline-block">Chỉ tiêu</span>
                </template>
                <div class="row">
                  <div class="col-6">
                    <div style="display: flex; justify-content: space-between" class="pb-2">
                      <h5>Danh sách chỉ tiêu</h5>
                      <div>
                        <!--                        <b-button-->
                        <!--                            variant="primary"-->
                        <!--                            type="button"-->
                        <!--                            class="btn w-md btn-primary"-->
                        <!--                            @click="handleCreateChiTieu"-->
                        <!--                            size="sm"-->
                        <!--                        >-->
                        <!--                          <i class="mdi mdi-plus me-1"></i> Thêm chỉ tiêu-->
                        <!--                        </b-button>-->
                      </div>
                    </div>
                    <table class="dynamic-table" v-if="thuocTinhIsChiTieu && thuocTinhIsChiTieu.length > 0"
                           style="width: 100%">
                      <thead>
                      <td v-for="(value, index) in thuocTinhIsChiTieu" :key="index"
                          style="padding: 0px 5px"
                          :style="{ 'width': value.width == 0?'auto':  value.width + 'px',
                     'text-align': value.fontHorizontalAlign?value.fontHorizontalAlign.id: '',
                     'font-weight': value.fontWeight?value.fontWeight.id: '',
                     'font-style': value.fontStyle?value.fontStyle.id: '',
                     }"
                      >
                        {{ value.ten }}
                      </td>
                      <td style="text-align: center; font-weight: bold; width: 50px">Xử lý</td>
                      </thead>
                      <tbody>
                      <tr v-for="(value, index) in renderRowValue" :key="index" style="min-height: 50px; height: 50px"
                          @click="getRowValueByKeyRow(value.keyRow)">
                        <td v-for="(item, index1) in value.rowValues" :key="index1"
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
                        <td style="text-align: center">
                          <button
                              type="button"
                              size="sm"
                              class="btn btn-outline btn-sm"
                              data-toggle="tooltip" data-placement="bottom" title="Cập nhật"
                              v-on:click="handleUpdate(item.id)">
                            <i class="fas fa-pencil-alt text-success me-1"></i>
                          </button>
                        </td>

                      </tr>
                      </tbody>
                    </table>
                  </div>

                  <div class="col-6">
                    <div class="row">
                      <div class="col-12">
                        <div class="text-start mb-2">
                          <b-button variant="warning" class="w-md mx-1" size="sm"
                                    @click="handleResetThuocTinhIsChiTieu">
                            Làm mới
                          </b-button>
                          <b-button @click="handleSubmitRowValue" type="submit" variant="primary" size="sm"
                                    class="ms-1 w-md">
                            Lưu
                          </b-button>
                          <b-button v-if="checkValueChiTieu()" variant="danger" class="w-md mx-1" size="sm"
                                    @click="handleSubmitDeleteRowValue">
                            Xóa
                          </b-button>
                        </div>
                      </div>
                      <div class="col-8">
                        <div class="mb-3">
                          <label class="text-left">Chỉ tiêu cha</label>
                          <treeselect
                              :normalizer="normalizer"
                              v-model="configChiTieu.parentId"
                              :options="optionsRowValueFilter"
                              :searchable="false"
                              :show-count="true"
                              :default-expand-level="1"
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
                      <div class="col-4">
                        <div class="mb-3">
                          <label class="text-left">Nhập số thứ tự</label>
                          <input
                              id="thuTu"
                              v-model="configChiTieu.thuTu"
                              type="number"
                              min="0"
                              oninput="validity.valid||(value='');"
                              class="form-control"
                              placeholder="Nhập số thứ tự"

                          />
                        </div>
                      </div>

                      <div class="col-12 card" v-for="(value, index) in chiTieus" :key="index">
                        <div class="card-body">
                          <div style="font-weight: bold; font-size: 18px">#{{ ++index }} - {{ value.tenThuocTinh }}
                          </div>
                          <form @submit.prevent="handleSubmitThuocTinh"
                                ref="formContainer">
                            <div class="row">
                              <div class="col-12">
                                <div class="mb-3">
                                  <label class="text-left">Tên chỉ tiêu</label>
                                  <input type="hidden" v-model="value.id"/>
                                  <input
                                      id="ten"
                                      v-model.trim="value.value"
                                      type="text"
                                      class="form-control"
                                      placeholder=""
                                  />
                                </div>
                              </div>
                              <hr/>
                              <h6>Ghi chú</h6>
                              <div class="col-2">
                                <div class="mb-3">
                                  <label class="text-left">Ký hiệu</label>
                                  <input
                                      v-model.trim="value.ghiChu.kyHieu"
                                      type="text"
                                      class="form-control"
                                      placeholder=""
                                  />
                                </div>
                              </div>
                              <div class="col-10">
                                <div class="mb-3">
                                  <label class="text-left">Nội dung</label>
                                  <textarea
                                      v-model.trim="value.ghiChu.noiDung"
                                      type="text"
                                      class="form-control"
                                      placeholder=""
                                  />
                                </div>
                              </div>
                              <hr/>
                              <div class="col-4">
                                <div class="mb-3">
                                  <label class="text-left">Kiểu chữ</label>
                                  <multiselect v-model="value.fontStyle" :options="fontStyle" label="name" track="id"
                                               :searchable="false" :show-labels="false"
                                               placeholder="Chọn kiểu chữ"></multiselect>
                                </div>
                              </div>
                              <div class="col-4">
                                <div class="mb-3">
                                  <label class="text-left">Phông chữ</label>
                                  <multiselect v-model="value.fontWeight" :options="fontWeight" label="name" track="id"
                                               :searchable="false" :show-labels="false"
                                               placeholder="Chọn phông chữ"></multiselect>
                                </div>
                              </div>
                              <div class="col-4">
                                <div class="mb-3">
                                  <label class="text-left">Canh chiều ngang</label>

                                  <multiselect v-model="value.fontHorizontalAlign" :options="fontHorizontalAlign"
                                               label="name"
                                  ></multiselect>
                                </div>
                              </div>
                            </div>

                          </form>
                        </div>

                        <hr/>
                      </div>
                    </div>
                  </div>
                </div>
              </b-tab>
              <b-tab>
                <template v-slot:title>
                  <span class="d-inline-block d-sm-none">
                    <i class="far fa-envelope"></i>
                  </span>
                  <span class="d-none d-sm-inline-block">Thông tin bảng biểu</span>
                </template>
                <div class="row">
                  <div class="col-12">
                    <b-button
                        variant="primary"
                        type="button"
                        class="btn w-md btn-primary mx-2"
                        @click="handleUpdateBangBieu()"
                        size="sm"
                    >
                      Sửa bảng biểu
                    </b-button>
                                          <b-button
                                              variant="primary"
                                              type="button"
                                              class="btn w-md btn-danger mx-2"
                                              @click="handleShowDeleteModal($route.params.id)"
                                              size="sm"
                                          >
                                           Xóa bảng biểu
                                          </b-button>
                  </div>
                </div>
              </b-tab>
            </b-tabs>
          </div>
        </div>
      </div>
    </div>
    <div class="row ">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
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
          </div>
        </div>
      </div>
    </div>
    <b-modal
        v-model="showModalThuocTnh"
        title="Thuộc tin thuộc tính"
        title-class="text-black font-18"
        body-class="p-3"
        hide-footer
        centered
        no-close-on-backdrop
        size="lg"
    >
      <form @submit.prevent="handleSubmitThuocTinh"
            ref="formContainer">
        <div class="row">
          <div class="col-12">
            <div class="mb-3">
              <label class="text-left">Tên thuộc tính</label>
              <span style="color: red">&nbsp;*</span>
              <input type="hidden" v-model="modelThuocTinh.id"/>
              <input
                  id="ten"
                  v-model.trim="modelThuocTinh.ten"
                  type="text"
                  class="form-control"
                  placeholder=""
                  :class="{
                                'is-invalid':
                                  submitted && $v.modelThuocTinh.ten.$error,
                              }"
              />
              <div
                  v-if="submitted && !$v.modelThuocTinh.ten.required"
                  class="invalid-feedback"
              >
                Tên thuộc tính không được để trống.
              </div>
            </div>
          </div>
          <div class="col-6">
            <div class="mb-3">
              <label class="text-left">Nhập số thứ tự</label>
              <span style="color: red">&nbsp;*</span>
              <input
                  id="thuTu"
                  v-model="modelThuocTinh.thuTu"
                  type="number"
                  min="0"
                  oninput="validity.valid||(value='');"
                  class="form-control"
                  placeholder="Nhập số thứ tự"
                  :class="{
                                'is-invalid':
                                  submitted && $v.modelThuocTinh.thuTu.$error,
                              }"
              />
              <div
                  v-if="submitted && !$v.modelThuocTinh.thuTu.required"
                  class="invalid-feedback"
              >
                Thứ tự không được để trống.
              </div>
            </div>
          </div>
        </div>
<!--        <div class="text-end pt-2 mt-3">-->
<!--          <b-button variant="light" class="w-md" size="sm" @click="handleCloseModalMauBieu">-->
<!--            Đóng-->
<!--          </b-button>-->
<!--          <b-button type="submit" variant="primary" size="sm" class="ms-1 w-md">-->
<!--            Lưu-->
<!--          </b-button>-->
<!--        </div>-->
      </form>
    </b-modal>
    <b-modal
        v-model="showDeleteModalThuocTinh"
        centered
        title="Xóa thuộc tính"
        title-class="font-18"
        no-close-on-backdrop
    >
      <p>
        Khi xóa thuộc tính những dữ liệu liên quan sẽ bị xóa theo.
        <br/>
        Dữ liệu xóa sẽ không được phục hồi!
      </p>
      <template #modal-footer>
        <b-button v-b-modal.modal-close_visit
                  size="sm"
                  class="btn btn-outline-info w-md"
                  v-on:click="showDeleteModalThuocTinh = false">
          Đóng
        </b-button>
        <b-button v-b-modal.modal-close_visit
                  size="sm"
                  variant="danger"
                  type="button"
                  class="w-md"
                  v-on:click="handleDeleteThuocTinh">
          Xóa
        </b-button>
      </template>
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
<!--          <b-button variant="light" class="w-md" size="sm" @click="handleCloseModalMauBieu">-->
<!--            Đóng-->
<!--          </b-button>-->
<!--          <b-button type="submit" variant="primary" size="sm" class="ms-1 w-md">-->
<!--            Lưu-->
<!--          </b-button>-->
        </div>
      </form>
    </b-modal>
  </Layout>

</template>

<script>
import PageHeader from "@/components/page-header";
import Layout from "@/layouts/main";
import {notifyModel} from "@/models/notifyModel";
import {thuocTinhModel} from "@/models/thuocTinhModel";
import {required} from "vuelidate/lib/validators";
import Treeselect from "@riophae/vue-treeselect";
import {styleModel} from "@/models/StyleModel";
import Multiselect from 'vue-multiselect'
import {formulaModel} from "@/models/FormulaModel";
import Switches from "vue-switches";
import {rowValueModel} from "@/models/rowValueModel";
import {bangBieuModel} from "@/models/bangBieuModel";


export default {
  components: {Layout, PageHeader, Treeselect, Multiselect, Switches},
  data() {
    return {
      title: "Thiết kế bảng biểu",
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
      thuocTinhs: [],
      modelThuocTinh: thuocTinhModel.baseJson(),
      showModalThuocTnh: false,
      showDeleteModalThuocTinh: false,
      submitted: false,
      optionsThuocTinh: [],
      optionsThuocTinhFilter: [],
      optionsRowValueFilter: [],
      listTreeThuocTinh: [],
      itemEvents: {
        mouseover: function () {
        },
        contextmenu: function () {
          arguments[2].preventDefault()
        }
      },
      headers: [],
      fontStyle: styleModel.FontStyle,
      fontWeight: styleModel.FontWeight,
      fontHorizontalAlign: styleModel.FontHorizontalAlign,
      fontVerticalAlign: styleModel.FontVerticalAlign,
      formula: formulaModel.FormulaModel,
      styleInput: formulaModel.StyleInputModel,
      optionDonViTinh: [],
      thuocTinhIsChiTieu: [],
      chiTieus: [],
      configChiTieu: {
        parentId: null,
        thuTu: 0
      },
      renderRowValue: [],
      renderMainRowValue: [],
      defaultThuocTinhIsChiTieu: [],
      modelBangBieu: bangBieuModel.baseJson(),
      showModalBangBieu: false,
      submittedBangBieu: false,
      showDeleteModal: false,
      model: bangBieuModel.baseJson()
    }
  },
  validations: {
    modelThuocTinh: {
      ten: {required},
      thuTu: {required},
      width: {required},
      fontHorizontalAlign: {required},
      fontStyle: {required},
      styleInput: {required},
    },
    modelBangBieu: {
      ten: {required},
      thuTu: {required}
    },
  },
  mounted() {
    this.getParentThuocTinh();
    this.getTreeThuocTinh();
    this.renderHeader();
    this.getDonViTinh();
    ///
    this.getThuocTinhChiTieu();
    this.renderBodyByBangBieuId();
    this.getTreeParentRowValue();
    this.renderBodyMainByBangBieuId();
  },
  computed: {},
  watch: {
    thuocTinhIsChiTieu: {
      deep: true,
      handler(val) {
        this.chiTieus = [];
        val.map((value, index) => {
          var ct = rowValueModel.baseJson();
          ct.bangBieuId = value.bangBieuId;
          ct.thuocTinhId = value.id;
          ct.tenThuocTinh = value.ten;
          ct.styleInput = value.styleInput;
          ct.order = value.order;

          this.chiTieus.push(ct);
        })
      }
    },
    configChiTieu: {
      deep: true,
      handler(val) {
        if (this.chiTieus && this.chiTieus.length > 0) {
          this.chiTieus.map(value => {
            value.thuTu = val.thuTu;
            value.parentId = val.parentId;
          })
        }
      }
    },
  },
  methods: {
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("bangBieuStore/delete", this.model.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showDeleteModal = false;
            this.$router.push("/thiet-ke-mau-bieu")
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
    async handleUpdateBangBieu() {
      await this.$store.dispatch("bangBieuStore/getById", this.$route.params.id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.modelBangBieu = bangBieuModel.fromJson(res.data);
          this.showModalBangBieu = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleSubmitBangBieu(e) {
      e.preventDefault();
      this.submittedBangBieu = true;
      this.$v.modelBangBieu.$touch();
      if (this.$v.modelBangBieu.$invalid) {
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
              this.showModalBangBieu = false;
              this.modelBangBieu = bangBieuModel.baseJson();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        } else {
          this.model.isTemplate = true;
          // Create model
          await this.$store.dispatch("bangBieuStore/create", this.modelBangBieu).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.modelBangBieu = bangBieuModel.baseJson();
              this.showModalBangBieu = false;
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        }
        loader.hide();
      }
      this.submitted = false;
    },
    async clearRowValue() {
      this.chiTieus = [];
      this.thuocTinhIsChiTieu.map((value, index) => {
        var ct = rowValueModel.baseJson();
        ct.bangBieuId = value.bangBieuId;
        ct.thuocTinhId = value.id;
        ct.tenThuocTinh = value.ten;
        ct.styleInput = value.styleInput;

        this.chiTieus.push(ct);
      })
    },
    checkValueChiTieu() {
      let check = false;
      this.chiTieus && this.chiTieus.map(value => {
        if (value.id)
          check = true;
      })
      return check;
    },
    async handleSubmitDeleteRowValue() {
      this.submitted = true;
      await this.$store.dispatch("rowValueStore/deleteRowValue", this.chiTieus).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.getThuocTinhChiTieu();
          this.renderBodyByBangBieuId();
          this.getTreeParentRowValue();
          this.clearRowValue();
          this.renderBodyMainByBangBieuId();
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
      this.submitted = false;
    },
    async handleSubmitRowValue() {
      this.submitted = true;
      await this.$store.dispatch("rowValueStore/create", this.chiTieus).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.chiTieus = [];
          this.getThuocTinhChiTieu();
          this.renderBodyByBangBieuId();
          this.getTreeParentRowValue();
          this.clearRowValue();
          this.renderHeader();
          this.renderBodyMainByBangBieuId();
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
      this.submitted = false;
    },
    handleCreateChiTieu() {

    },
    async handleResetThuocTinhIsChiTieu() {
      await this.getThuocTinhChiTieu();
      this.chiTieus = [];
      this.thuocTinhIsChiTieu.map((value, index) => {
        var ct = rowValueModel.baseJson();
        ct.bangBieuId = value.bangBieuId;
        ct.thuocTinhId = value.id;
        ct.tenThuocTinh = value.ten;
        ct.styleInput = value.styleInput;

        this.chiTieus.push(ct);
      })
    },
    handleResetThuocTinh() {
      this.modelThuocTinh = thuocTinhModel.baseJson()
    },
    handleShowModalDeleteThuocTinh() {
      this.showDeleteModalThuocTinh = true;
    },
    async handleDeleteThuocTinh() {
      if (this.modelThuocTinh.id != 0 && this.modelThuocTinh.id != null && this.modelThuocTinh.id) {
        await this.$store.dispatch("thuocTinhStore/delete", this.modelThuocTinh.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.modelThuocTinh = thuocTinhModel.baseJson();
            this.getTreeThuocTinh();
            this.showDeleteModalThuocTinh = false;
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          // });
        });
      }
    },
    checkIsStar(value) {
      return value && value.length > 0 && value.includes('*');
    },
    checkIsEmpty(value) {
      return value && value.length > 0;
    },
    async renderBodyMainByBangBieuId() {
      try {
        await this.$store.dispatch("rowValueStore/renderBodyMainByBangBieuId", this.$route.params.id).then(resp => {
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
    async renderBodyByBangBieuId() {
      try {
        await this.$store.dispatch("rowValueStore/renderBodyByBangBieuId", this.$route.params.id).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.renderRowValue = items;
          }
          return [];
        });
      } finally {
        console.log("");
      }
    },
    async getRowValueByKeyRow(keyRow) {
      try {
        await this.$store.dispatch("rowValueStore/getRowValueByKeyRow", keyRow).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            // this.optionsRowValueFilter = items;
            let temp = [];
            this.chiTieus.map(value => {
              var val = items.find(x => x.thuocTinhId == value.thuocTinhId);
              if (val != null) {
                // value = val;
                val.tenThuocTinh = value.tenThuocTinh;
                this.configChiTieu.parentId = val.parentId;
                this.configChiTieu.thuTu = val.thuTu;
                temp.push(val)
              }
            })
            this.chiTieus = temp;
          }
          return [];
        });
      } finally {
        console.log("");
      }
    },
    async getTreeParentRowValue() {
      try {
        await this.$store.dispatch("rowValueStore/getTreeParentRowValue", this.$route.params.id).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.optionsRowValueFilter = items;
          }
          return [];
        });
      } finally {
        console.log("");
      }
    },
    async getThuocTinhChiTieu() {
      try {
        await this.$store.dispatch("thuocTinhStore/getThuocTinhChiTieu", this.$route.params.id).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.thuocTinhIsChiTieu = items;
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
    normalizer(node) {
      if (node.children == null || node.children == 'null') {
        delete node.children;
      }
    },
    async handleUpdate(id) {
      await this.$store.dispatch("thuocTinhStore/getById", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.modelThuocTinh = thuocTinhModel.toJson(res.data);
          this.optionsThuocTinhFilter = this.optionsThuocTinh.filter(x => x.id != id);
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async itemClick(node) {
      console.log(node);
      await this.handleUpdate(node.model.id);
    },
    async renderHeader() {
      try {
        await this.$store.dispatch("bangBieuStore/renderHeader", this.$route.params.id).then(resp => {
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
    async getParentThuocTinh() {
      try {
        await this.$store.dispatch("thuocTinhStore/getTreeThuocTinhByBangBieuId", this.$route.params.id).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.optionsThuocTinh = items;
            this.optionsThuocTinhFilter = this.optionsThuocTinh;
          }
          return [];
        });
      } finally {
        console.log("");
      }
    },
    async getParentRowValue() {
      try {
        await this.$store.dispatch("thuocTinhStore/getTreeRowValueByBangBieuId", this.$route.params.id).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.optionsThuocTinh = items;
            this.optionsThuocTinhFilter = this.optionsThuocTinh;
          }
          return [];
        });
      } finally {
        console.log("");
      }
    },
    async getTreeThuocTinh() {
      try {
        await this.$store.dispatch("thuocTinhStore/getListTreeThuocTinhByBangBieuId", this.$route.params.id).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.listTreeThuocTinh = items;
          }
          return [];
        });
      } finally {
        console.log("");
      }
    },
    handleCreate() {
      this.showModalThuocTnh = true;
      this.modelThuocTinh.bangBieuId = this.$route.params.id;
    },
    async handleSubmitThuocTinh(e) {
      console.log(this.modelThuocTinh)
      e.preventDefault();
      this.modelThuocTinh.bangBieuId = this.$route.params.id;
      this.submitted = true;
      this.$v.modelThuocTinh.$touch();
      if (this.$v.modelThuocTinh.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        if (
            this.modelThuocTinh.id != 0 &&
            this.modelThuocTinh.id != null &&
            this.modelThuocTinh.id
        ) {
          // Update model
          await this.$store.dispatch("thuocTinhStore/update", this.modelThuocTinh).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.modelThuocTinh = thuocTinhModel.baseJson();
              this.renderHeader();
              this.getTreeThuocTinh();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        } else {
          // Create model
          await this.$store.dispatch("thuocTinhStore/create", this.modelThuocTinh).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.modelThuocTinh = thuocTinhModel.baseJson();
              this.renderHeader();
              this.getTreeThuocTinh();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        }
        loader.hide();

      }
      this.submitted = false;
    }
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

.vertical-style {
  display: flex;
  align-items: center;
}
</style>