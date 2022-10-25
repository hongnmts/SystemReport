<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";
import {pagingModel} from "@/models/pagingModel";
import {required} from "vuelidate/lib/validators";
import {vanBanDenModel} from "@/models/vanBanDenModel";
import Multiselect from "vue-multiselect";
import DatePicker from "vue2-datepicker";
import CKEditor from "@ckeditor/ckeditor5-vue";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import vue2Dropzone from "vue2-dropzone";
import {butPheModel} from "@/models/butPheModel";
import {phanCongModel} from "@/models/phanCongModel";
import {notifyModel} from "@/models/notifyModel";
import {vanBanDiModel} from "@/models/vanBanDiModel";
import {trangThaiModel} from "@/models/trangThaiModel";
import {CURRENT_USER} from "@/helpers/currentUser";

import moment from "moment";

/**
 * Advanced table component
 */
export default {
  page: {
    title: "Lưu công văn đi",
    meta: [{name: "description", content: appConfig.description}]
  },
  components: {
    Layout,
    PageHeader,
    Multiselect,
    ckeditor: CKEditor.component,
    DatePicker
  },
  data() {
    return {
      title: "Lưu công văn đi",
      items: [
        {
          text: "E-Office",
          href: "/"
        },
        {
          text: "Văn bản đi",
          href: "/van-ban-den"
        },
        {
          text: "Danh sách",
          active: true
        }
      ],
      data: [],
      showModal: false,
      showDetail: false,
      showDeleteModal: false,
      submitted: false,
      listCoQuan: [],
      listRole: [],
      showModalMembers: false,
      showTrangThaiModal: false,
      showCheckVanBanModal: false,
      model: vanBanDiModel.baseJson(),
      modelKySo: {
        choPhepKy: true,
        nguoiKy: null,
        vanBanDiId: null,
        thuTu: 0
      },
      modelButPhe: butPheModel.baseJson(),
      modelPhanCong: phanCongModel.baseJson(),
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      todoTotalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 50,
      pageOptions: [50, 80, 100],
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
          thClass: "text-primary hidden-sortable",
        },
        {
          key: "soLuuCV",
          label: "Số lưu CV",
          class: "px-1 text-center content-capso",
          thStyle: {width: '130px', minWidth: '100px'},
          thClass: "text-primary hidden-sortable",
          sortable: true,
        },
        {
          key: "soVBDi",
          label: "Số CV đi",
          class: "px-1 text-center content-capso",
          thStyle: {width: '130px', minWidth: '100px'},
          thClass: "text-primary hidden-sortable",
        },
        {
          key: "trichYeu",
          label: "Trích yếu",
          class: "px-1 content-capso",
          thClass: "text-primary hidden-sortable",
        },
        {
          key: "donViSoan",
          label: "Đơn vị soạn",
          class: "px-1 text-center content-capso",
          thStyle: { width:'200px', maxWidth: '200px'},
          thClass: "text-primary hidden-sortable",
        },
        {
          key: "nguoiKy",
          label: "Người ký",
          class: "px-1 text-center content-capso",
          thStyle: {width: '100px', minWidth: '100px'},
          thClass: "text-primary hidden-sortable",
        },
        {
          key: "ngayKy",
          label: "Ngày ký",
          thStyle: {width: '100px', minWidth: '100px'},
          class: "px-1 text-center content-capso",
          thClass: "text-primary hidden-sortable",
        },
        {
          key: 'process',
          label: 'Xử lý',
          thStyle: {width: '80px', minWidth: '110px'},
          thClass: "text-primary hidden-sortable",
        }
      ],
      optionsLoaiVanBan: [],
      optionsDonVi: [],
      optionsDonViTree: [],
      optionsLinhVuc: [],
      optionsUser: [],
      optionsHinhThucNhan: [],
      optionsMucDo: [],
      optionsTrangThai: [],
      listPhanCongKySo: [],
      editor: ClassicEditor,
      editorConfig: {
        height: '200px'
      },
      apiUrl: process.env.VUE_APP_API_URL,
      url: `${process.env.VUE_APP_API_URL}files/upload`,
      urlView: `${process.env.VUE_APP_API_URL}files/view/`,
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}files/upload`,
        thumbnailWidth: 300,
        thumbnailHeight: 160,
        maxFiles: 4,
        maxFilesize: 30,
        headers: {"My-Awesome-Header": "header value"},
        addRemoveLinks: true,
        acceptedFiles: ".doc,.docx,.pdf",
      },
      showModalButPhe: false,
      showModalPhanCong: false,
      phanCong: [{id: 1}],
      currentStatus: null,
      modelTrangThai: trangThaiModel.currentBaseJson(),
      currentUserName: CURRENT_USER.USERNAME,
      showHistoryModal: false,
      listHistoryQuestion: [],
      noiDungTuChoi: null,
      showModalNoiDungTuChoi: false,
      valueConsistsOf: 'ALL_WITH_INDETERMINATE',
      showButtonSave: false,
      showButtonKySoCurrent: false,
      showModelAcceptKySo: false,
      showModalKySoPhapLy: false,
      showModalThietLapKySoPhapLy: false
    };
  },
  validations: {
  },

  async created() {
    this.getLoaiVanBan();

    this.getDonVi();
    this.getUser();
    this.getLinhVuc();
    this.getHinhThuc();
    this.getMucDo();
    this.getDonViTree();
  },
  watch:{
    modelTrangThai: {
      deep: true,
      async handler(val) {
        if(val.newTrangThai && val.newTrangThai.code == "VSDM"){
          await this.handleKySoPhapLyDM(val.vanBanDiId)
        }
      }
    },
  },
  mounted() {
    // Set the initial number of items
    this.totalRows = this.items.length;
  },
  methods: {
    async  handleDetail(id) {
      await this.$store.dispatch("congVanStore/getByIdLuuCVDi", id).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.showModal = true;
          this.model = res.data;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
      });
    },
    async getPhanCongKySoByVanBanId(id) {
      this.modelKySo.vanBanDiId = id;
      try {
        await this.$store.dispatch("vanBanDiStore/getPhanCongKySoByVanBanId", id).then(resp => {
          if (resp.resultCode == "SUCCESS") {

            let items = resp.data;
            this.loading = false
            this.listPhanCongKySo = items || [];
            let checkExistPhanCongKySo = this.listPhanCongKySo.find(x => x.userName == this.currentUserName && (x.ngayKyString != null && x.ngayKyString != ""));
            if (checkExistPhanCongKySo != null) {
              this.showButtonKySoCurrent = true;
            } else {
              this.showButtonKySoCurrent = false;
            }
            this.showModalMembers = true;
            return items || []
          }
          return [];
        })
      } finally {
        this.loading = false
      }
    },
    async fnGetList() {
      await this.onPageChange();
    },
    async onPageChange(page = 1) {
      // this.pagination.currentPage = page;
      // const params = {
      //   pageNumber: this.pagination.currentPage,
      //   pageSize: this.pagination.pageSize,
      // }
      this.$refs.tblList?.refresh()
    },
    onFiltered(filteredItems) {
      // Trigger pagination to update the number of buttons/pages due to filtering
      this.totalRows = filteredItems.length;
      this.currentPage = 1;
    },

    async handleSubmit(e) {
      e.preventDefault();
      this.submitted = true;
      this.$v.$touch();
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
          //Update model
          await this.$store.dispatch("vanBanDiStore/update", this.model).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.showModal = false;
              this.model = vanBanDenModel.baseJson()
              this.$refs.tblList.refresh();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          })
        } else {
          //Create modelhandleSubmit
          this.model.version = 1;
          await this.$store.dispatch("vanBanDiStore/create", this.model).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.showModal = false;
              this.model = vanBanDenModel.baseJson()
              this.$refs.tblList.refresh();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          });
        }
        loader.hide();
      }
      this.submitted = false;

    },
    async handleHistory(id) {
      let loader = this.$loading.show({
        container: this.$refs.formContainer,
      });
      await this.$store.dispatch("historyVanBanStore/getHistoryVanBanDi", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.listHistoryQuestion = res.data;
          this.showHistoryModal = true;

          loader.hide();
        }
      });
    },
    async handleUpdate(id) {
      let loader = this.$loading.show({
        container: this.$refs.formContainer,
      });
      await this.$store.dispatch("vanBanDiStore/getById", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.model = res.data;
          this.showModal = true;
          this.getTrangThai(this.model.trangThai)
          loader.hide();
          this.showButtonSave = true;
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleView(id) {
      let loader = this.$loading.show({
        container: this.$refs.formContainer,
      });
      await this.$store.dispatch("vanBanDiStore/getById", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.model = res.data;
          this.showModal = true;
          this.getTrangThai(this.model.trangThai)
          loader.hide();
          this.showButtonSave = false;
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleShowNoiDungTuChoi(value) {
      this.noiDungTuChoi = value;
      this.showModalNoiDungTuChoi = true;

    },
    async handleCreate() {
      await this.$store.dispatch("vanBanDiStore/getVaCapSo").then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.model = res.data;
          this.showModal = true;
          this.showButtonSave = true;
        }
      });
      this.getTrangThai(null);
    },
    handleShowDeleteModal(id) {
      this.model.id = id;
      this.showDeleteModal = true;
    },
    handleDeleteFile(index) {
      if (this.model) {
        if (this.model.file) {
          this.model.file.splice(index, 1);
        }

      }
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("vanBanDiStore/delete", this.model.id).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showDeleteModal = false;
            this.$refs.tblList.refresh();
          }
        });
      }
    },
    HandleShowPhanCong(id) {
      this.model.id = id;
      this.showModalPhanCong = true;

    },
    handleShowButPhe(id) {
      this.model.id = id;
      this.showModalButPhe = true;
    },
    async getLoaiVanBan() {
      await this.$store.dispatch("loaiVanBanStore/getAll").then((res) => {
        if (res.resultCode === "SUCCESS") {
          this.optionsLoaiVanBan = res.data;
        } else {
          this.optionsLoaiVanBan = [];
        }
      });
    },
    async getTrangThai(currentTrangThai) {
      try {
        await this.$store.dispatch("trangThaiStore/getNextTrangThai", {
          loaiTrangThai: "VBDi",
          currentTrangThai: currentTrangThai
        }).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data;
            this.loading = false
            this.optionsTrangThai = items;
            return items || []
          }
          return [];
        })
      } finally {
        this.loading = false
      }
    },
    // async getPhanCongKySoByVanBanId(id) {
    //   // this.modelKySo = null;
    //   try {
    //     await this.$store.dispatch("vanBanDiStore/getPhanCongKySoByVanBanId", id).then(resp => {
    //       if (resp.resultCode == "SUCCESS") {
    //         let items = resp.data;
    //         this.loading = false
    //         this.listPhanCongKySo = items || [];
    //         return items || []
    //       }
    //       return [];
    //     })
    //   } finally {
    //     this.loading = false
    //   }
    // },
    async getDonVi() {
      try {
        await this.$store.dispatch("donViStore/get").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsDonVi = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getDonViTree() {
      try {
        await this.$store.dispatch("donViStore/getTree").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsDonViTree = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getUser() {
      try {
        await this.$store.dispatch("userStore/getAll").then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsUser = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getLinhVuc() {
      try {
        await this.$store.dispatch("dmLinhVucStore/get").then(resp => {
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
    async getHinhThuc() {
      try {
        let promise = this.$store.dispatch("hinhThucGuiStore/get")
        return promise.then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.loading = false
            this.optionsHinhThucNhan = items;
          }
          return [];
        });
      } finally {
        this.loading = false
      }
    },
    async getMucDo() {
      await this.$store.dispatch("enumStore/getMucDo").then((res) => {
        if (res.resultCode === "SUCCESS") {
          this.optionsMucDo = res.data;
        } else {
          this.optionsMucDo = [];
        }
      });
    },
    removeThisFile(file, error, xhr) {
      let fileCongViec = JSON.parse(file.xhr.response);
      if (fileCongViec.data && fileCongViec.data.id) {
        let idFile = fileCongViec.data.id;
        let resultData = this.model.uploadFiles.filter(x => {
          return x.fileId != idFile;
        })
        this.model.uploadFiles = resultData;
      }
    },
    addThisFile(file, response) {
      if (this.model) {
        if (this.model.uploadFiles == null || this.model.uploadFiles.length <= 0) {
          this.model.uploadFiles = [];
        }
        let fileSuccess = response.data;
        this.model.uploadFiles.push({fileId: fileSuccess.id, fileName: fileSuccess.fileName, ext: fileSuccess.ext})
      }
    },
    removeCommentFile(file, error, xhr) {
      let fileCongViec = JSON.parse(file.xhr.response);
      if (fileCongViec.data && fileCongViec.data.id) {
        let idFile = fileCongViec.data.id;
        let resultData = this.model.uploadFiles = null;
        this.model.uploadFiles = resultData;
      }
    },
    addCommentFile(file, response) {
      if (this.model) {
        if (this.model.uploadFiles == null || this.model.length <= 0) {
          this.model.uploadFiles = [];
        }
        let fileSuccess = response.data;
        this.model.uploadFiles = {fileId: fileSuccess.id, fileName: fileSuccess.fileName, ext: fileSuccess.ext}
      }
    },
    normalizer(node) {
      if (node.children == null || node.children == 'null') {
        delete node.children;
      }
    },
    AddformData() {
      this.phanCong.push({yKienChiDao: null, nguoiButPhe: null, nguoiNhanXuLy: null, file: null});
    },
    deleteRow(index) {
      if (confirm("Bạn có chắc muốn xoá không?"))
        this.phanCong.splice(index, 1);
    },
    myProvider(ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        // content: this.filter,
        sortBy: ctx.sortBy,
        sortDesc: ctx.sortDesc,
      }
      this.loading = true

      try {
        let promise = this.$store.dispatch("congVanStore/getPagingParamsLuuCVDi", params)
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
    async handleAssignSign(e) {
      e.preventDefault();
      if (
          this.modelKySo.vanBanDiId != 0 &&
          this.modelKySo.vanBanDiId != null &&
          this.modelKySo.vanBanDiId
      ) {
        //Update model
        await this.$store.dispatch("vanBanDiStore/assignSign", this.modelKySo).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.getPhanCongKySoByVanBanId(this.modelKySo.vanBanDiId);
            this.modelKySo.nguoiKy = null
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        })
      }
    },
    async handleAssignSignTemp(e) {
      if (this.modelTrangThai.listPhanCongKySo == null)
        this.modelTrangThai.listPhanCongKySo = [];
      if (this.modelKySo && this.modelKySo.nguoiKy) {
        let index = this.modelTrangThai.listPhanCongKySo.findIndex(x => x.userName == this.modelKySo.nguoiKy.userName);
        if (index != null && index == -1) {
          this.modelTrangThai.listPhanCongKySo.push({
            userName: this.modelKySo.nguoiKy.userName,
            fullName: this.modelKySo.nguoiKy.fullName,
            choPhepKy: this.modelKySo.choPhepKy
          })
        }
      }
    },
    async handleRemoveSignTemp(value) {
      if (this.listPhanCongKySo == null)
        this.modelTrangThai.listPhanCongKySo = [];
      if (this.modelKySo && this.modelKySo.nguoiKy) {
        let index = this.modelTrangThai.listPhanCongKySo.findIndex(x => x.userName == value);
        if (index != null && index != -1) {
          this.modelTrangThai.listPhanCongKySo = this.modelTrangThai.listPhanCongKySo.filter(x => x.userName != value)
        }
      }
    },
    async handleRemoveAssignSign(userName) {
      if (
          this.modelKySo.vanBanDiId != 0 &&
          this.modelKySo.vanBanDiId != null &&
          this.modelKySo.vanBanDiId
      ) {
        this.modelKySo.nguoiKy = {userName: userName};
        //Update model
        await this.$store.dispatch("vanBanDiStore/removeAssignSign", this.modelKySo).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModal = false;
            this.getPhanCongKySoByVanBanId(this.modelKySo.vanBanDiId);
            this.modelKySo.nguoiKy = null
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        })
      }
    },
    handleChuyenTrangThai: function (currentStatus, vanBanDiId) {
      this.getTrangThai(currentStatus)
      this.modelTrangThai.currentTrangThai = currentStatus;
      this.modelTrangThai.newTrangThai = null;
      this.modelTrangThai.vanBanDiId = vanBanDiId;
      this.modelTrangThai.userName = this.currentUserName;
      this.showTrangThaiModal = true;
    },
    async handleChuyenTrangThaiVanBan(value) {
      if (value) {
        this.modelTrangThai.newTrangThai = value;
      }
      this.submitted = true;
      this.$v.$touch();
      if (this.$v.modelTrangThai.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainerTrangThai,
        });
        if (
            this.modelTrangThai
            && this.modelTrangThai.vanBanDiId != null
        ) {
          //Update model
          await this.$store.dispatch("vanBanDiStore/chuyenTrangThaiVanBan", this.modelTrangThai).then((res) => {
            if (res.resultCode === 'SUCCESS') {
              this.showTrangThaiModal = false;
              this.showCheckVanBanModal = false;
              this.model = trangThaiModel.currentBaseJson()
              this.$refs.tblList.refresh();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
          })
        }
        loader.hide();
      }
      this.submitted = false;

    },
    async handleCheckVanBanModal(id) {
      await this.$store.dispatch("vanBanDiStore/getById", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.model = res.data;
          this.getTrangThai(this.model.trangThai)
          this.modelTrangThai.currentTrangThai = this.model.trangThai;
          this.modelTrangThai.newTrangThai = null;
          this.modelTrangThai.vanBanDiId = this.model.id;
          this.modelTrangThai.userName = this.currentUserName;
          this.showCheckVanBanModal = true;
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    formatRemoveDonVi(node, instanceId) {
      let value = this.modelTrangThai.donVi?.find(x => x.id == node.id);
      if (value != null) {
        this.modelTrangThai.donVi = this.modelTrangThai.donVi.filter(x => x.id != value.id);
      }
    },
    formatDonVi(node, instanceId) {
      let index = this.modelTrangThai.donVi?.findIndex(x => x.id == node.id);
      if (index == -1 || index == undefined) {
        if (!this.modelTrangThai.donVi) {
          this.modelTrangThai.donVi = [];
        }
        this.modelTrangThai.donVi.push({id: node.id, ten: node.label, code: node.code});

      }
    },
    checkUserQuyenKySo(data) {
      if (data) {
        let index = data.findIndex(x => x.userName == this.currentUserName);
        if (index != -1)
          return true;
        return false
      }
      return false;
    },
    handleShowModelAcceptKySo() {
      this.showModelAcceptKySo = true;
    },
    async handleAssignOrReject() {
      let loader = this.$loading.show({
        container: this.$refs.modalAcceptKySo,
      });
      if (this.modelKySo.vanBanDiId != null) {
        this.modelKySo.ngayKyString = moment().format();
        await this.$store.dispatch("vanBanDiStore/assignOrReject", this.modelKySo).then((res) => {
          if (res.resultCode === 'SUCCESS') {
            this.showModelAcceptKySo = false;
            this.showModalMembers = false;
            loader.hide();
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }).finally(() => {
          loader.hide();
        });
      }
    },
    async handleKySoPhapLyDM(id) {

      await this.$store.dispatch("vanBanDiStore/getById", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.modelKySo.vanBanDiId = res.data.id;
          if (res.data.filePDF) {
            this.modelKySo.fileName = res.data.filePDF[0].fileName;
            this.modelKySo.path = this.urlView + res.data.filePDF[0].fileId;
          } else if (res.data.file) {
            this.modelKySo.fileName = res.data.file[0].fileName;
            this.modelKySo.path = this.urlView + res.data.file[0].fileId;
          }

          this.modelKySo.userName = CURRENT_USER.USER_KY_SO.userNameKySo;
          this.modelKySo.password = CURRENT_USER.USER_KY_SO.userNameKySo;
        }
      });
    },
    async handleKySoPhapLy(id, value) {
      await this.$store.dispatch("vanBanDiStore/getById", id).then((res) => {
        if (res.resultCode == "SUCCESS") {
          this.modelKySo.vanBanDiId = res.data.id;
          if (res.data.filePDF) {
            this.modelKySo.fileName = res.data.filePDF[0].fileName;
            this.modelKySo.path = this.urlView + res.data.filePDF[0].fileId;
          } else if (res.data.file) {
            this.modelKySo.fileName = res.data.file[0].fileName;
            this.modelKySo.path = this.urlView + res.data.file[0].fileId;
          }

          if (value) {
            this.showModalThietLapKySoPhapLy = true;
            localStorage.setItem("kysophaply", JSON.stringify(this.modelKySo));
          } else {
            this.showModalKySoPhapLy = true;
            this.modelKySo.userName = CURRENT_USER.USER_KY_SO.userNameKySo;
            this.modelKySo.password = CURRENT_USER.USER_KY_SO.userNameKySo;
          }
        } else {
          // this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    async handleAssignOrRejectPhapLy() {
      let loader = this.$loading.show({
        container: this.$refs.modalKySoPhapLy,
      });
      if (this.modelKySo.vanBanDiId != null) {
        localStorage.setItem("kysophaply", JSON.stringify(this.modelKySo));
        this.$router.push("/ky-so")
        // this.modelKySo.ngayKyString = moment().format();
        // await this.$store.dispatch("vanBanDiStore/assignOrRejectPhapLy", this.modelKySo).then((res) => {
        //   if (res.resultCode === 'SUCCESS') {
        //     this.showModelAcceptKySo = false;
        //     loader.hide();
        //   }
        //   this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        // }).finally(() => {
        //   loader.hide();
        // });
      }
    },
    handleCloseThietLapKySoPhapLy(){
      this.showCheckVanBanModal = false;
      this.showModalKySoPhapLy = false;
      this.showModalThietLapKySoPhapLy = false;
      this.showTrangThaiModal = false;
      this.$refs.tblList?.refresh()
    }
  }
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="row mb-2">
<!--              <div class="col-sm-4">-->
<!--                <div class="search-box me-2 mb-2 d-inline-block">-->
<!--                  <div class="position-relative">-->
<!--                    <input-->

<!--                        type="text"-->
<!--                        class="form-control"-->
<!--                        placeholder="Tìm kiếm ..."-->
<!--                    />-->
<!--                    <i class="bx bx-search-alt search-icon"></i>-->
<!--                  </div>-->
<!--                </div>-->
<!--              </div>-->
              <div class="col-sm-8">
                <div class="text-sm-end">
                  <!-- Model create -->
                  <b-modal
                      v-model="showModal"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="xl"
                  >
                    <template #modal-header="{  }">
                      <!-- Emulate built in modal header close button action -->
                      <h5> Thông tin Lưu CV</h5>
                      <div class="text-end">
                        <b-button variant="light" size="sm" style="width: 80px" @click="showModal = false">
                          Đóng
                        </b-button>
                      </div>
                    </template>
                    <form
                          ref="formContainer">
                      <div class="row">
                        <div class="col-md-7">
                          <div class="row">
                            <!--                            Loại văn bản-->
                            <div class="col-md-6" v-if="model.loaiVanBan">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Loại văn bản</label> <span
                                  class="text-danger">*</span>
                                <multiselect
                                    v-model="model.loaiVanBan"
                                    :options="optionsLoaiVanBan"
                                    track-by="id"
                                    label="ten"
                                    placeholder="Chọn loại văn bản"
                                ></multiselect>
                              </div>
                            </div>
                            <!--                            Trạng thái-->
                            <div class="col-md-6">

                              <div v-if="model.trangThai && model.id" class="mb-2">
                                <label class="form-label" for="validationCustom01">Trạng thái</label> <span
                                  class="text-danger">*</span>
                                <input
                                    id="validationCustom01"
                                    :value="model.trangThai.ten"
                                    type="text"
                                    class="form-control"
                                    placeholder=""
                                    disabled
                                />
                              </div>

                              <div v-else class="mb-2">
                                <label class="form-label" for="validationCustom01">Trạng thái</label> <span
                                  class="text-danger">*</span>
                                <multiselect
                                    v-model="model.trangThai"
                                    :options="optionsTrangThai"
                                    track-by="id"
                                    label="ten"
                                    placeholder="Chọn trạng thái"
                                ></multiselect>
                              </div>
                            </div>
                            <!--                              Số lưu -->
                            <div class="col-md-4">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Số lưu CV</label>
                                <!--                                <span-->
                                <!--                                  class="text-danger">*</span>-->
                                <input
                                    id="validationCustom01"
                                    v-model="model.soLuuCV"
                                    type="text"
                                    class="form-control"
                                    placeholder=""
                                />
                              </div>
                            </div>
                            <!--                            Số VB đến -->
                            <div class="col-md-4">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Số CV đi</label>
                                <!--                                <span-->
                                <!--                                  class="text-danger">*</span>-->
                                <input
                                    id="validationSoVBDen"
                                    v-model="model.soVBDi"
                                    type="text"
                                    class="form-control"
                                    placeholder=""

                                />
                              </div>
                            </div>
                            <!--                            Ngày nhận -->
                            <div class="col-md-4">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Ngày nhập</label>
                                <!--                                <date-picker-->
                                <!--                                    v-model="model.ngayNhap"-->
                                <!--                                    format="DD/MM/YYYY"-->
                                <!--                                    :first-day-of-week="1"-->
                                <!--                                    lang="en"-->
                                <!--                                    placeholder="Chọn ngày nhập"-->
                                <!--                                ></date-picker>-->
                                <date-picker v-model="model.ngayNhap"
                                             format="DD/MM/YYYY"
                                             value-type="format"
                                >
                                  <div slot="input">
                                    <input v-model="model.ngayNhap"
                                           v-mask="'##/##/####'" type="text" class="form-control"
                                           placeholder="Nhập ngày nhập"/>
                                  </div>
                                </date-picker>
                              </div>
                            </div>
                            <!--                              Số lưu -->
                            <div class="col-md-4">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01"> Trả lời CV số</label>
                                <!--                                <span-->
                                <!--                                  class="text-danger">*</span>-->
                                <input
                                    id="validationCustom01"
                                    v-model="model.traLoiCVSo"
                                    type="text"
                                    class="form-control"
                                    placeholder=""
                                />
                              </div>
                            </div>
                            <!--                            Ngày nhận -->
                            <div class="col-md-4">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01"> Ngày trả lời</label>
                                <date-picker v-model="model.ngayTraLoi"
                                             format="DD/MM/YYYY"
                                             value-type="format"
                                >
                                  <div slot="input">
                                    <input v-model="model.ngayTraLoi"
                                           v-mask="'##/##/####'" type="text" class="form-control"
                                           placeholder="Nhập ngày trả lời"/>
                                  </div>
                                </date-picker>
                              </div>
                            </div>
                            <!--                            Số VB đến -->
                            <div class="col-md-4">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01"> Số bản</label>
                                <!--                                <span-->
                                <!--                                  class="text-danger">*</span>-->
                                <input
                                    id="validationSoVBDen"
                                    v-model="model.soBan"
                                    type="number"
                                    class="form-control"
                                    placeholder=""

                                />
                              </div>
                            </div>
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01"> Người ký</label>
                                <multiselect
                                    v-model="model.nguoiKy"
                                    :options="optionsUser"
                                    track-by="id"
                                    label="fullName"
                                    placeholder="Chọn cán bộ soạn"
                                    deselect-label="Nhấn để xoá"
                                    selectLabel="Nhấn enter để chọn"
                                    selectedLabel="Đã chọn"
                                >
                                  <template slot="singleLabel" slot-scope="{ option }">
                                    <strong>{{ option.fullName }}</strong>

                                    <span v-if="option.donVi" style="color:red">&nbsp;{{ option.donVi.ten }}</span>
                                  </template>
                                  <template slot="option" slot-scope="{ option }">
                                    <div class="option__desc">
          <span class="option__title">
            <strong>{{ option.fullName }}&nbsp;</strong>
          </span>
                                      <span v-if="option.donVi" class="option__small"
                                            style="color:green">{{ option.donVi.ten }}</span>
                                    </div>
                                  </template>
                                </multiselect>
                              </div>
                            </div>
                            <!--                            Ngày Ký-->
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
                                           placeholder="Nhập ngày ký"/>
                                  </div>
                                </date-picker>
                              </div>
                            </div>
                            <!--                            Trích yếu -->
                            <div class="col-md-12">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Trích yếu</label>
                                <!--                                <span-->
                                <!--                                  class="text-danger">*</span>-->
                                <ckeditor
                                    v-model="model.trichYeu"
                                    :editor="editor"
                                    :config="editorConfig"
                                ></ckeditor>
                              </div>
                            </div>
                            <div v-if="model.filePDF != null  && model.filePDF.length > 0" class="col-md-12">
                              <label for="">Danh sách đã ký (Nhấn vào để tải xuống)</label>
                              <template>
                                <div v-for="(file, index) in model.filePDF" :key="index">
                                  <a
                                      :href="`${apiUrl}files/view/${file.fileId}`"
                                      class=" fw-medium"
                                  ><i
                                      :class="`mdi font-size-16 align-middle me-2`"
                                  ></i>
                                    {{ index + 1 }}: {{ file.fileName }}</a
                                  >
                                </div>
                              </template>

                            </div>
                            <div class="col-md-12">
                              <label for="">Danh sách tệp tin (Nhấn vào để tải xuống)</label>
                              <template v-if="model.file == null || (model.file != null &&model.file.length <= 0)">
                                <div>Không có tệp tin</div>
                              </template>
                              <template v-else>
                                <div v-for="(file, index) in model.file" :key="index">
                                  <a
                                      :href="`${apiUrl}files/view/${file.fileId}`"
                                      class=" fw-medium"
                                  ><i
                                      :class="`mdi font-size-16 align-middle me-2`"
                                  ></i>
                                    {{ index + 1 }}: {{ file.fileName }}</a
                                  >
                                </div>
                              </template>

                            </div>

                          </div>

                        </div>

                        <div class="col-md-5">
                          <div class="row">
                            <div class="col-md-12">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01"> Đơn vị soạn</label>
                                <multiselect
                                    v-model="model.donViSoan"
                                    :options="optionsDonVi"
                                    track-by="id"
                                    label="ten"
                                    placeholder="Chọn đơn vị soạn"
                                    deselect-label="Nhấn để xoá"
                                    selectLabel="Nhấn enter để chọn"
                                    selectedLabel="Đã chọn"
                                ></multiselect>

                              </div>
                            </div>
                            <!--                            Cơ quan gửi-->
                            <div class="col-md-12">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01"> Cán bộ soạn</label>
                                <multiselect
                                    v-model="model.canBoSoan"
                                    :options="optionsUser"
                                    track-by="id"
                                    label="fullName"
                                    placeholder="Chọn cán bộ soạn"
                                    deselect-label="Nhấn để xoá"
                                    selectLabel="Nhấn enter để chọn"
                                    selectedLabel="Đã chọn"
                                >
                                  <template slot="singleLabel" slot-scope="{ option }">
                                    <strong>{{ option.fullName }}</strong>

                                    <span v-if="option.donVi" style="color:red">&nbsp;{{ option.donVi.ten }}</span>
                                  </template>
                                  <template slot="option" slot-scope="{ option }">
                                    <div class="option__desc">
          <span class="option__title">
            <strong>{{ option.fullName }}&nbsp;</strong>
          </span>
                                      <span v-if="option.donVi" class="option__small"
                                            style="color:green">{{ option.donVi.ten }}</span>
                                    </div>
                                  </template>
                                </multiselect>
                              </div>
                            </div>
                            <!--                            Hình thức nhận -->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Hình thức nhận</label>
                                <multiselect
                                    v-model="model.hinhThucGui"
                                    :options="optionsHinhThucNhan"
                                    track-by="id"
                                    label="ten"
                                    placeholder="Chọn hình thức nhận"
                                    deselect-label="Nhấn để xoá"
                                    selectLabel="Nhấn enter để chọn"
                                    selectedLabel="Đã chọn"
                                ></multiselect>
                              </div>
                            </div>
                            <!--                            Lĩnh vực-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Lĩnh vực</label>
                                <multiselect
                                    v-model="model.linhVuc"
                                    :options="optionsLinhVuc"
                                    track-by="id"
                                    label="ten"
                                    placeholder="Chọn lĩnh vực"
                                    deselect-label="Nhấn để xoá"
                                    selectLabel="Nhấn enter để chọn"
                                    selectedLabel="Đã chọn"
                                ></multiselect>
                              </div>
                            </div>
                            <!--                            Mức độ tính chất-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Mức độ tính chất</label>
                                <multiselect
                                    v-model="model.mucDoTinhChat"
                                    :options="optionsMucDo"
                                    track-by="id"
                                    label="name"
                                    placeholder="Chọn mức độ tính chất"
                                    deselect-label="Nhấn để xoá"
                                    selectLabel="Nhấn enter để chọn"
                                    selectedLabel="Đã chọn"
                                ></multiselect>
                              </div>
                            </div>
                            <!--                            Mức độ bảo mật-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Mức độ bảo mật</label>
                                <multiselect
                                    v-model="model.mucDoBaoMat"
                                    :options="optionsMucDo"
                                    track-by="id"
                                    label="name"
                                    placeholder="Chọn mức độ bảo mật"
                                    deselect-label="Nhấn để xoá"
                                    selectLabel="Nhấn enter để chọn"
                                    selectedLabel="Đã chọn"
                                ></multiselect>
                              </div>
                            </div>
                            <!--                            Hồ sơ đơn vị-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Hồ sơ đơn vị</label>
                                <input
                                    v-model="model.hoSoDonVi"
                                    type="text"
                                    class="form-control"
                                    placeholder=""
                                />
                              </div>
                            </div>
                            <!--                            Nơi lưu trữ-->
                            <div class="col-md-6">
                              <div class="mb-2">
                                <label class="form-label" for="validationCustom01">Nơi lưu trữ</label>
                                <input
                                    v-model="model.noiLuuTru"
                                    type="text"
                                    class="form-control"
                                    placeholder="Nhập nơi lưu trữ"
                                />
                              </div>
                            </div>
                            <!--                            Điều kiện-->
                            <!--                            <div class="col-md-12">-->
                            <!--                              <div class="mb-2 d-flex align-items-center">-->
                            <!--                                <switches v-model="model.trinhLanhDaoTruong" color="primary"-->
                            <!--                                          class="ml-1 mx-2"></switches>-->
                            <!--                                <label for=""> Trình lãnh đạo trường</label>-->
                            <!--                              </div>-->
                            <!--                              <div class="mb-2 d-flex align-items-center">-->
                            <!--                                <switches v-model="model.congVanChiDoc" color="primary" class="ml-1 mx-2"></switches>-->
                            <!--                                <label for="">Là công văn chỉ đọc</label>-->
                            <!--                              </div>-->
                            <!--                              <div class="mb-2 d-flex align-items-center">-->
                            <!--                                <switches v-model="model.banChinh" color="primary" class="ml-1 mx-2"></switches>-->
                            <!--                                <label for=""> Là bản chính</label>-->
                            <!--                              </div>-->
                            <!--                              <div class="mb-2 d-flex align-items-center">-->
                            <!--                                <switches v-model="model.hienThiThongBao" color="primary" class="ml-1 mx-2"></switches>-->
                            <!--                                <label for="">Hiển thị mục thông báo</label>-->
                            <!--                              </div>-->

                            <!--                            </div>-->
                          </div>
                        </div>
                      </div>
                    </form>
                  </b-modal>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-12">
                <!-- Table -->
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
                      {{ data.index + ((currentPage - 1) * perPage) + 1 }}
                    </template>
                    <template v-slot:cell(ngayKy)="data">
                      <span v-if="data.item.ngayKy"> {{ data.item.ngayKy }}</span>
                    </template>
                    <template v-slot:cell(nguoiKy)="data">
                      <span v-if="data.item.nguoiKy"> {{ data.item.nguoiKy.fullName }}</span>
                    </template>
                    <template v-slot:cell(donViSoan)="data">
                      <span v-if="data.item.donViSoan"> {{ data.item.donViSoan.ten }}</span>
                    </template>
                    <template v-slot:cell(trichYeu)="data">
                      <div v-if="data.item.trichYeu" :inner-html.prop="data.item.trichYeu | truncate(150)">
                      </div>
                    </template>
                    <template v-slot:cell(process)="data">
                      <div class="d-flex justify-content-around">
                        <button
                            type="button"
                            size="sm"
                            class="btn btn-outline btn-sm p-0"
                            data-toggle="tooltip" data-placement="bottom" title="Chi tiết"
                            v-on:click="handleDetail(data.item.id)">
                          <i class="fas fa-eye  text-warning me-1"></i>
                        </button>
                      </div>
                    </template>
                  </b-table>
                  <template v-if="isBusy">
                    <div align="center">Đang tải dữ liệu</div>
                  </template>
                  <template v-if="totalRows <= 0 && !isBusy">
                    <div align="center">Không có dữ liệu</div>
                  </template>
                </div>
                <!-- Paginnation -->
                <div>
                  <div class="row mb-2">
                    <div class="col-sm-12 col-md-6">
                      <div style="display: flex; justify-content: start; align-items: center">
                        <div style="margin-right: 20px" class="">
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
                        <div class="">
                          <div>Hiển thị {{ numberOfElement }} trên tổng số {{ totalRows }} dòng</div>
                        </div>
                      </div>
                    </div>
                    <div  class="col-sm-12 col-md-6">
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
        <!--        Modal delete -->
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
<style scoped>
.title-capso {
  font-weight: bold;
  color: #00568C;

}

.content-capso {
  color: #00568C;
}

.capso-container {
  margin-top: 10px;
  display: flex;
  padding: 0px;
}

.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}

</style>