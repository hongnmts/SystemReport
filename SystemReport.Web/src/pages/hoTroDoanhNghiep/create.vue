<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";
import Multiselect from "vue-multiselect";
import {hoTroDoanhNghiepModel} from "@/models/hoTroDoanhNghiepModel";
import {notifyModel} from "@/models/notifyModel";
import DatePicker from "vue2-datepicker";
import {Money} from "v-money"
import {quanLyKhoaHocModel} from "@/models/quanLyKHModel";
/**
 * Advanced table component
 */
export default {
  page: {
    title: "Xử lý văn bản đến",
    meta: [{name: "description", content: appConfig.description}]
  },
  components: {
    Layout,
    PageHeader,
    Multiselect,
    DatePicker,
    Money
  },
  data() {
    return {
      title: "Thêm hoặc chỉnh sửa",
      items: [
        {
          text: "Hỗ trợ doanh nghiệp",
          href: "/"
        }
      ],
      submitted: false,
      model: hoTroDoanhNghiepModel.baseJson(),
      optionsLoaiVanBan: [],
      editorConfig: {
        height: '200px',
        toolbar: {
          items: [
            'heading',
            'bold',
            'italic',
            'bulletedList',
            'numberedList',
            'undo',
            'redo',
          ]
        },
      },
      apiUrl: process.env.VUE_APP_API_URL,
      url: `${process.env.VUE_APP_API_URL}files/upload`,
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}files/upload`,
        thumbnailWidth: 300,
        thumbnailHeight: 160,
        maxFiles: 1,
        maxFilesize: 30,
        headers: {"My-Awesome-Header": "header value"},
        addRemoveLinks: true,
        acceptedFiles: ".jpeg,.jpg,.png,.gif,.doc,.docx,.xlsx,.pptx,.pdf",
      },
      optionsLoaiHinh: [],
      optionsToChuc: [],
      optionDonViHanhChinh: [],
      optionNoiDungHoTro: [],
      optionQuyetDinh: [],
      action: null,
      money: {
        decimal: '.',
        thousands: ',',
        prefix: '',
        suffix: '',
        precision: 0,
        masked: false,
      },
    };
  },
  validations: {},
  mounted() {
  },
  watch: {},
  async created() {
    this.getLoaiHinh();
    this.getToChuc();
    this.getDonViHanhChinh();
    this.getNoiDungHoTro();
    this.getQuyetDinh();
    if (this.$route.params.id) {
      this.handleDetail();
    } else {
      this.handleCreate();
    }
  },
  methods: {
    resetForm(){
      this.model = quanLyKhoaHocModel.baseJson();
    },
    async getQuyetDinh() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "QUYETDINH").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionQuyetDinh = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getNoiDungHoTro() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "NOIDUNGHOTRO").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionNoiDungHoTro = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getDonViHanhChinh() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "HUYEN").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionDonViHanhChinh = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getLoaiHinh() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "LOAIHINH").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsLoaiHinh = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getToChuc() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "TOCHUC").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsToChuc = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    handleCreate() {
      this.model = hoTroDoanhNghiepModel.baseJson();
    },
    async handleSubmit(e) {
      e.preventDefault();
      this.submitted = true;
      console.log(this.model);
      // this.$v.$touch();
      // if (this.$v.model.$invalid) {
      //   return;
      // } else {
      //
      // }
      let loader = this.$loading.show({
        container: this.$refs.formContainer,
      });
      if (
          this.model.id != 0 &&
          this.model.id != null &&
          this.model.id
      ) {
        //Update model
        await this.$store.dispatch("hoTroDoanhNghiepStore/update", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = res.data;
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        })
      } else {
        //Create modelhandleSubmit
        await this.$store.dispatch("hoTroDoanhNghiepStore/create", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = res.data;
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        });
      }
      loader.hide()
    },

    async handleDetail() {
      let loader = this.$loading.show({
        container: this.$refs.formContainer,
      });
      await this.$store.dispatch("hoTroDoanhNghiepStore/getById", this.$route.params.id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.model = res.data;
          loader.hide();
          //this.showButtonSave = false;
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });

    },
    async addTagToChuc(newTag) {
      const parts = newTag;
      const tag = {
        name: parts,
        type: "TOCHUC"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionsToChuc = [res.data, ...this.optionsToChuc];
          this.model.toChuc = res.data;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },
    async addTagQuyetDinh(newTag) {
      const parts = newTag;
      const tag = {
        name: parts,
        type: "QUYETDINH"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionQuyetDinh = [res.data, ...this.optionQuyetDinh];
          this.model.quyetDinh = res.data;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },
    async addTagLoaiHinh(newTag) {
      const parts = newTag;
      const tag = {
        name: parts,
        type: "LOAIHINH"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionsLoaiHinh = [res.data, ...this.optionsLoaiHinh];
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },
    async addTagDonViHanhChinh(newTag) {
      const parts = newTag;
      const tag = {
        name: parts,
        type: "HUYEN"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionDonViHanhChinh = [res.data, ...this.optionDonViHanhChinh];
          this.model.donViHanhChinh = res.data;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },
    async addTagNoiDung(newTag) {
      const parts = newTag;
      const tag = {
        name: parts,
        type: "NOIDUNGHOTRO"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionNoiDungHoTro = [res.data, ...this.optionNoiDungHoTro];
          if(this.model.noiDungHoTro == null)
            this.model.noiDungHoTro = []
          this.model.noiDungHoTro = [ ...this.model.noiDungHoTro, res.data];
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    }
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <b-card>
      <form @submit.prevent="handleSubmit"
            ref="formContainer">
        <div class="text-end pt-2 mb-2">
          <!--          <b-button variant="light" class="w-md" size="sm" @click="showModal = false">-->
          <!--            Đóng-->
          <!--          </b-button>-->
          <b-button type="submit" variant="primary" size="sm" class="ms-1 w-md">
            Lưu
          </b-button>
          <b-button type="button" variant="warning" size="sm" class="ms-1 w-md" @click="resetForm">
            Thêm mới
          </b-button>
          <b-button type="submit" variant="danger" size="sm" class="ms-1 w-md" @click="$router.push('/ho-tro-doanh-nghiep')">
            Trở về
          </b-button>
        </div>
        <div class="row">
          <div class="col-lg-12 col-md-12">
            <div class="row">

              <!--                            Loại văn bản-->
              <div class="col-md-6">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Tổ chức</label>
                  <multiselect
                      v-model="model.toChuc"
                      :options="optionsToChuc"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagToChuc"
                  ></multiselect>
                </div>
              </div>
              <div class="col-md-6">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Loại hình</label>

                  <multiselect
                      v-model="model.loaiHinh"
                      :options="optionsLoaiHinh"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagLoaiHinh"
                  ></multiselect>
                </div>
              </div>

              <div class="col-md-6">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Huyện/TP</label>

                  <multiselect
                      v-model="model.donViHanhChinh"
                      :options="optionDonViHanhChinh"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagDonViHanhChinh"
                  ></multiselect>
                </div>
              </div>
              <div class="col-md-6">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Số nhà, đường</label>
                  <input
                      id="validationCustom01"
                      v-model="model.diaChi"
                      type="text"
                      class="form-control"
                      placeholder=""
                  />
                </div>
              </div>
              <div class="col-md-6">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Nội dung hỗ trợ</label>

                  <multiselect
                      v-model="model.noiDungHoTro"
                      :options="optionNoiDungHoTro"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagNoiDung"
                      :multiple="true"
                  ></multiselect>
                </div>
              </div>
              <div class="col-md-6">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Số tiền</label>

                  <money
                      v-model="model.soTien"
                      v-bind="money"
                      class="form-control text-right"
                      placeholder=""
                  />
                </div>
              </div>
              <div class="col-md-6">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Quyết định</label>
                  <multiselect
                      v-model="model.quyetDinh"
                      :options="optionQuyetDinh"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagQuyetDinh"
                  ></multiselect>
                </div>
              </div>
              <div class="col-md-6">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Ngày ký</label>
                  <date-picker v-model="model.ngayKy"
                               format="DD/MM/YYYY"
                               value-type="format"
                  >
                    <div slot="input">
                      <input v-model="model.ngayKy"
                             v-mask="'##/##/####'" type="text" class="form-control"
                             placeholder="Nhập ngày ký"
                      />
                    </div>
                  </date-picker>
                </div>
              </div>
            </div>

          </div>
        </div>
      </form>
    </b-card>
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

.table > tbody > tr > td {
  padding: 0px;
  line-height: 30px;
}

.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}

.ck-editor__editable {
  min-height: 100px !important;
}

.ck-content {
  height: 100px !important;
}

.customAddElement {
  position: absolute;
  top: 8px;
  left: -20px;
}

.custom-ribon {
  position: absolute;
  width: 130px;
  top: 15px;
  left: -15px;
}

.custom-ribon > div {
  border-radius: 3px;
}

.title-capso {
  font-weight: bold;
  color: #00568C;

}

.content-capso {
  padding: 5px;
  color: #00568C;
}

.capso-container {
  margin-top: 10px;
  display: flex;
  padding: 0px;
}

.show-detail-vbd {
  background-color: #eda910;
  border: 1px solid #eda910;
  border-radius: 0px;
  margin: 10px;
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

.file-box {
  justify-content: start;
}

</style>
<style lang="scss">
.success-checkmark {
  width: 80px;
  height: 115px;
  margin: 0 auto;

  .check-icon {
    width: 80px;
    height: 80px;
    position: relative;
    border-radius: 50%;
    box-sizing: content-box;
    border: 4px solid #4CAF50;

    &::before {
      top: 3px;
      left: -2px;
      width: 30px;
      transform-origin: 100% 50%;
      border-radius: 100px 0 0 100px;
    }

    &::after {
      top: 0;
      left: 30px;
      width: 60px;
      transform-origin: 0 50%;
      border-radius: 0 100px 100px 0;
      animation: rotate-circle 4.25s ease-in;
    }

    &::before, &::after {
      content: '';
      height: 100px;
      position: absolute;
      background: #FFFFFF;
      transform: rotate(-45deg);
    }

    .icon-line {
      height: 5px;
      background-color: #4CAF50;
      display: block;
      border-radius: 2px;
      position: absolute;
      z-index: 10;

      &.line-tip {
        top: 46px;
        left: 14px;
        width: 25px;
        transform: rotate(45deg);
        animation: icon-line-tip 0.75s;
      }

      &.line-long {
        top: 38px;
        right: 8px;
        width: 47px;
        transform: rotate(-45deg);
        animation: icon-line-long 0.75s;
      }
    }

    .icon-circle {
      top: -4px;
      left: -4px;
      z-index: 10;
      width: 80px;
      height: 80px;
      border-radius: 50%;
      position: absolute;
      box-sizing: content-box;
      border: 4px solid rgba(76, 175, 80, .5);
    }

    .icon-fix {
      top: 8px;
      width: 5px;
      left: 26px;
      z-index: 1;
      height: 85px;
      position: absolute;
      transform: rotate(-45deg);
      background-color: #FFFFFF;
    }
  }
}

@keyframes rotate-circle {
  0% {
    transform: rotate(-45deg);
  }
  5% {
    transform: rotate(-45deg);
  }
  12% {
    transform: rotate(-405deg);
  }
  100% {
    transform: rotate(-405deg);
  }
}

@keyframes icon-line-tip {
  0% {
    width: 0;
    left: 1px;
    top: 19px;
  }
  54% {
    width: 0;
    left: 1px;
    top: 19px;
  }
  70% {
    width: 50px;
    left: -8px;
    top: 37px;
  }
  84% {
    width: 17px;
    left: 21px;
    top: 48px;
  }
  100% {
    width: 25px;
    left: 14px;
    top: 45px;
  }
}

@keyframes icon-line-long {
  0% {
    width: 0;
    right: 46px;
    top: 54px;
  }
  65% {
    width: 0;
    right: 46px;
    top: 54px;
  }
  84% {
    width: 55px;
    right: 0px;
    top: 35px;
  }
  100% {
    width: 47px;
    right: 8px;
    top: 38px;
  }
}

.btn-modal {
  background: #00568C;
  border: none;
  color: #fff;
}

.btn-modal:hover {
  background: #00568C;
  border: none;
  color: #fff;
  box-shadow: rgba(50, 50, 93, 0.25) 0px 13px 27px -5px, rgba(0, 0, 0, 0.3) 0px 8px 16px -8px;
}

</style>
