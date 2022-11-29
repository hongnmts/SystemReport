<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";
import Multiselect from "vue-multiselect";
import {hoTroDoanhNghiepModel} from "@/models/hoTroDoanhNghiepModel";
import {notifyModel} from "@/models/notifyModel";
import DatePicker from "vue2-datepicker";
import {quanLyKhoaHocModel} from "@/models/quanLyKHModel";
import CKEditor from "@ckeditor/ckeditor5-vue";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import {Money} from "v-money"
/**
 * Advanced table component
 */
export default {
  page: {
    title: "Quản lý đề tài",
    meta: [{name: "description", content: appConfig.description}]
  },
  components: {
    Layout,
    PageHeader,
    Multiselect,
    DatePicker,
    ckeditor: CKEditor.component,
    Money
  },
  data() {
    return {
      title: "Thêm hoặc chỉnh sửa",
      items: [
        {
          text: "Quản lý đề tài",
          href: "/"
        }
      ],
      submitted: false,
      model: quanLyKhoaHocModel.baseJson(),
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
      editor: ClassicEditor,
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
      optionsDeTai: [],
      optionsChuTri: [],
      optionsChuNhiem: [],
      optionsLinhVuc: [],
      optionsQDPheDuyetKP: [],
      optionsPheDuyetNV: [],
      optionsCapQuanLy: [],
      optionsDangThucHien: [],
      optionsXepLoai: [],
      optionsQuyetDinhCG: [],
      optionsDonViTiepNhan: [],
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
    this.getDeTai();
    this.getChuNhiem();
    this.getXepLoai();
    this.getDangThucHien();
    this.getDonViTiepNhan();
    this.getChuTri();
    this.getLinhVuc();
    this.getQDChuyenGiao();
    this.getQDPheDuyet();
    this.getCapQuanLy();
    this.getPheDuyetNV();
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
    async getDeTai() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "DETAI").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsDeTai = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async addTagDeTai(newTag) {
      const parts = newTag;
      const tag = {
        name: parts,
        type: "DETAI"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionsDeTai = [res.data, ...this.optionsDeTai];
          this.model.tenDeTai = res.data;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },

    async getChuNhiem() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "CHUNHIEM").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsChuNhiem = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async addTagChuNhiem(newTag) {
      const parts = newTag;
      const tag = {
        name: parts,
        type: "CHUNHIEM"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionsChuNhiem = [res.data, ...this.optionsChuNhiem];
          this.model.chuNhiem = res.data;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },

    async getChuTri() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "CHUTRI").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsChuTri = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async addTagChuTri(newTag) {

      const parts = newTag;
      const tag = {
        name: parts,
        type: "CHUTRI"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionsChuTri = [res.data, ...this.optionsChuTri];
          this.model.chuTri = res.data;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },

    async getLinhVuc() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "LINHVUC").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsLinhVuc = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async addTagLinhVuc(newTag) {
      const parts = newTag;
      const tag = {
        name: parts,
        type: "LINHVUC"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionsLinhVuc = [res.data, ...this.optionsLinhVuc];
          this.model.linhVuc = res.data;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },

    async getQDPheDuyet() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "QUYETDINHPHEQUYET").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsQDPheDuyetKP = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async addTagQDPheDuyet(newTag) {
      const parts = newTag;
      const tag = {
        name: parts,
        type: "QUYETDINHPHEQUYET"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionsQDPheDuyetKP = [res.data, ...this.optionsQDPheDuyetKP];
          this.model.quyetDinhPDKQ = res.data;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },

    async getDangThucHien() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "DANGTHUCHIEN").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsDangThucHien = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async addTagDangThucHien(newTag) {
      const parts = newTag;
      const tag = {
        name: parts,
        type: "DANGTHUCHIEN"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionsDangThucHien = [res.data, ...this.optionsDangThucHien];
          this.model.dangThucHien = res.data;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },

    async getXepLoai() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "XEPLOAI").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsXepLoai = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async addTagXepLoai(newTag) {
      const parts = newTag;
      const tag = {
        name: parts,
        type: "XEPLOAI"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionsXepLoai = [res.data, ...this.optionsXepLoai];
          this.model.xepLoai = res.data;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },

    async getQDChuyenGiao() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "QUYETDINHCHUYENGIAO").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsQuyetDinhCG = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async addTagQDChuyenGiao(newTag) {
      const parts = newTag;
      const tag = {
        name: parts,
        type: "QUYETDINHCHUYENGIAO"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionsQuyetDinhCG = [res.data, ...this.optionsQuyetDinhCG];
          this.model.quyetDinhCQ = res.data;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },
    async getPheDuyetNV() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "PHEDUYETNV").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsPheDuyetNV = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async addTagPheDuyetNV(newTag) {
      const parts = newTag;
      const tag = {
        name: parts,
        type: "PHEDUYETNV"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionsPheDuyetNV = [res.data, ...this.optionsPheDuyetNV];
          this.model.pheDuyetNhiemVu = res.data;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },
    async getCapQuanLy() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "CAPQUANLY").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsCapQuanLy = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async addTagCapQuanLy(newTag) {
      const parts = newTag;
      const tag = {
        name: parts,
        type: "CAPQUANLY"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionsCapQuanLy = [res.data, ...this.optionsCapQuanLy];
          this.model.capQuanLy = res.data;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
    },
    async getDonViTiepNhan() {
      try {
        await this.$store.dispatch("commonItemStore/getByType", "DONVITIEPNHAN").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsDonViTiepNhan = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async addTagDonViTiepNhan(newTag) {
      const parts = newTag;
      const tag = {
        name: parts,
        type: "DONVITIEPNHAN"
      }
      await this.$store.dispatch("commonItemStore/create", tag).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.optionsDonViTiepNhan = [res.data, ...this.optionsDonViTiepNhan];
          if(this.model.donViTiepNhan == null)
            this.model.donViTiepNhan = []
          this.model.donViTiepNhan = [...this.model.donViTiepNhan, res.data];
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
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
        await this.$store.dispatch("quanLyKhoaHocStore/update", this.model).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.model = res.data;
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        })
      } else {
        //Create modelhandleSubmit
        await this.$store.dispatch("quanLyKhoaHocStore/create", this.model).then((res) => {
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
      await this.$store.dispatch("quanLyKhoaHocStore/getById", this.$route.params.id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.model = res.data;
          loader.hide();
          //this.showButtonSave = false;
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });

    },
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
          <b-button type="button" variant="danger" size="sm" class="ms-1 w-md" @click="$router.push('/quan-ly-de-tai')">
            Trở về
          </b-button>
        </div>
        <div class="row">
          <div class="col-lg-12 col-md-12">
            <div class="row">
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Cấp quản lý</label>
                  <multiselect
                      v-model="model.capQuanLy"
                      :options="optionsCapQuanLy"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagCapQuanLy"
                  ></multiselect>
                </div>
              </div>
              <div class="col-md-6">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Tên đề tài/ dự án</label>
                  <multiselect
                      v-model="model.tenDeTai"
                      :options="optionsDeTai"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagDeTai"
                  ></multiselect>
                </div>
              </div>
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Lĩnh vực</label>
                  <multiselect
                      v-model="model.linhVuc"
                      :options="optionsLinhVuc"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagLinhVuc"
                  ></multiselect>
                </div>
              </div>
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Tổ chức chủ trì</label>
                  <multiselect
                      v-model="model.chuTri"
                      :options="optionsChuTri"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagChuTri"
                  ></multiselect>
                </div>
              </div>
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Chủ nhiệm</label>
                  <multiselect
                      v-model="model.chuNhiem"
                      :options="optionsChuNhiem"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagChuNhiem"
                  ></multiselect>
                </div>
              </div>
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">QĐ Phê duyệt NV</label>
                  <multiselect
                      v-model="model.pheDuyetNhiemVu"
                      :options="optionsPheDuyetNV"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagPheDuyetNV"
                  ></multiselect>
                </div>
              </div>
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Ngày QĐ Phê duyệt NV</label>
                  <date-picker v-model="model.ngayPheDuyetNhiemVu"
                               format="DD/MM/YYYY"
                               value-type="format"
                  >
                    <div slot="input">
                      <input v-model="model.ngayPheDuyetNhiemVu"
                             v-mask="'##/##/####'" type="text" class="form-control"
                             placeholder=""
                      />
                    </div>
                  </date-picker>
                </div>
              </div>
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">QĐ phê duyệt KP</label>
                  <multiselect
                      v-model="model.quyetDinhPDKQ"
                      :options="optionsQDPheDuyetKP"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagQDPheDuyet"
                  ></multiselect>
                </div>
              </div>
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Ngày phê duyệt KP</label>
                  <date-picker v-model="model.ngayPDKQ"
                               format="DD/MM/YYYY"
                               value-type="format"
                  >
                    <div slot="input">
                      <input v-model="model.ngayPDKQ"
                             v-mask="'##/##/####'" type="text" class="form-control"
                             placeholder=""
                      />
                    </div>
                  </date-picker>
                </div>
              </div>
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Nguồn NSNN (ngàn đồng)</label>
                  <money
                      v-model="model.nguonNSNN"
                      v-bind="money"
                      class="form-control text-right"
                      placeholder="Kinh phí từ ngân sách nhà nước"
                  />
                </div>
              </div>
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Nguồn khác  (ngàn đồng)</label>
                  <money
                      v-model="model.nguonKhac"
                      v-bind="money"
                      class="form-control text-right"
                      placeholder="Kinh phí từ nguồn khác"
                  />
                </div>
              </div>
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Ngày bắt đầu hợp đồng</label>
                  <date-picker v-model="model.ngayBatDau"
                               format="DD/MM/YYYY"
                               value-type="format"
                  >
                    <div slot="input">
                      <input v-model="model.ngayBatDau"
                             v-mask="'##/##/####'" type="text" class="form-control"
                             placeholder=""
                      />
                    </div>
                  </date-picker>
                </div>
              </div>
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Ngày kết thúc hợp đồng</label>
                  <date-picker v-model="model.ngayKetThuc"
                               format="DD/MM/YYYY"
                               value-type="format"
                  >
                    <div slot="input">
                      <input v-model="model.ngayKetThuc"
                             v-mask="'##/##/####'" type="text" class="form-control"
                             placeholder=""
                      />
                    </div>
                  </date-picker>
                </div>
              </div>

              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Ngày gia hạn</label>
                  <date-picker v-model="model.ngayGiaHan"
                               format="DD/MM/YYYY"
                               value-type="format"
                  >
                    <div slot="input">
                      <input v-model="model.ngayGiaHan"
                             v-mask="'##/##/####'" type="text" class="form-control"
                             placeholder=""
                      />
                    </div>
                  </date-picker>
                </div>
              </div>

              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Trạng thái</label>
                  <multiselect
                      v-model="model.dangThucHien"
                      :options="optionsDangThucHien"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagDangThucHien"
                  ></multiselect>
                </div>
              </div>
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Ngày nghiệm thu</label>
                  <date-picker v-model="model.ngayNghiemThu"
                               format="DD/MM/YYYY"
                               value-type="format"
                  >
                    <div slot="input">
                      <input v-model="model.ngayNghiemThu"
                             v-mask="'##/##/####'" type="text" class="form-control"
                             placeholder=""
                      />
                    </div>
                  </date-picker>
                </div>
              </div>
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Xếp loại</label>
                  <multiselect
                      v-model="model.xepLoai"
                      :options="optionsXepLoai"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagXepLoai"
                  ></multiselect>
                </div>
              </div>

              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">QĐ chuyển giao</label>
                  <multiselect
                      v-model="model.quyetDinhCQ"
                      :options="optionsQuyetDinhCG"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagQDChuyenGiao"
                  ></multiselect>
                </div>
              </div>
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Ngày chuyển giao</label>
                  <date-picker v-model="model.ngayChuyenGiao"
                               format="DD/MM/YYYY"
                               value-type="format"
                  >
                    <div slot="input">
                      <input v-model="model.ngayChuyenGiao"
                             v-mask="'##/##/####'" type="text" class="form-control"
                             placeholder=""
                      />
                    </div>
                  </date-picker>
                </div>
              </div>

              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Đơn vị tiếp nhận</label>
                  <multiselect
                      v-model="model.donViTiepNhan"
                      :options="optionsDonViTiepNhan"
                      track-by="id"
                      label="name"
                      placeholder="Chọn"
                      deselect-label="Nhấn để xoá"
                      selectLabel="Nhấn enter để chọn"
                      selectedLabel="Đã chọn"
                      :taggable="true" @tag="addTagDonViTiepNhan"
                      :multiple="true"
                  ></multiselect>
                </div>
              </div>
              <div class="col-md-3">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Ngày dừng thực hiện</label>
                  <date-picker v-model="model.ngayDungNghiemThu"
                               format="DD/MM/YYYY"
                               value-type="format"
                  >
                    <div slot="input">
                      <input v-model="model.ngayDungNghiemThu"
                             v-mask="'##/##/####'" type="text" class="form-control"
                             placeholder=""
                      />
                    </div>
                  </date-picker>
                </div>
              </div>
              <div class="col-md-12">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Mục tiêu</label>
                  <ckeditor
                      v-model="model.mucTieu"
                      :editor="editor"
                      :config="editorConfig"
                  ></ckeditor>
                </div>
              </div>
              <div class="col-md-12">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Nội dung</label>
                  <ckeditor
                      v-model="model.noiDung"
                      :editor="editor"
                      :config="editorConfig"
                  ></ckeditor>
                </div>
              </div>
              <div class="col-md-12">
                <div class="mb-2">
                  <label class="form-label" for="validationCustom01">Sản phẩm</label>
                  <ckeditor
                      v-model="model.sanPham"
                      :editor="editor"
                      :config="editorConfig"
                  ></ckeditor>
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
