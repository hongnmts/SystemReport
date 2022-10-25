<script>
import {layoutComputed} from "@/state/helpers";

import Header from "@/components/header";
import Navbar from "@/components/navbar";
import Footer from "@/components/footer";
import RightBar from "@/components/right-bar";
import VueSlickCarousel from 'vue-slick-carousel'
import 'vue-slick-carousel/dist/vue-slick-carousel.css'
// optional style for arrows & dots
import 'vue-slick-carousel/dist/vue-slick-carousel-theme.css'
import {pagingModel} from "@/models/pagingModel";
import {CONSTANTS} from "@/helpers/constants";
import {mauBieuModel} from "@/models/mauBieuModel";
import {notifyModel} from "@/models/notifyModel";


/**
 * Horizontal-layout component
 */
export default {
  components: {Header, Navbar, Footer,},
  data() {
    return {
      activeIndex: [],
      isActiveIdx: null,
      contentFilter: [
        {
          id: '1',
          content: "Tài nguyên và môi trường",
          number: "10",
          isActive: false,
        },
        {
          id: '2',
          content: "Khoa học và Công nghệ",
          number: "5",
          isActive: false,
        },
        {
          id: '3',
          content: "Giao thông vận tải",
          number: "4",
          isActive: false,
        },
        {
          id: '4',
          content: "Ngân hàng",
          number: "0",
          isActive: false,
        },
        {
          id: '6',
          content: "Xây dựng",
          number: "0",
          isActive: false,
        }
      ],
      contentFilter1: [
        {
          id: '1',
          content: "PHÒNG HÀNH CHÍNH - TỔNG HỢP",
          number: "5",
          isActive: false,
        },
        {
          id: '2',
          content: "PHÒNG PHÂN TÍCH THỬ NGHIỆM",
          number: "9",
          isActive: false,
        },
        {
          id: '3',
          content: "PHÒNG THÔNG TIN VÀ ỨNG DỤNG KHOA HỌC CÔNG NGHỆ",
          number: "6",
          isActive: false,
        },
        {
          id: '4',
          content: "PHÒNG TƯ VẤN VÀ DỊCH VỤ KỸ THUẬT",
          number: "0",
          isActive: false,
        },
        {
          id: '6',
          content: "PHÒNG KIỂM ĐỊNH - HIỆU CHUẨN",
          number: "0",
          isActive: false,
        },
        {
          id: '6',
          content: "PHÒNG HÀNH CHÍNH, TỔNG HỢP VÀ QUẢN LÝ ĐO LƯỜNG",
          number: "3",
          isActive: false,
        },
        {
          id: '6',
          content: "PHÒNG QUẢN LÝ TIÊU CHUẨN CHẤT LƯỢNG",
          number: "1",
          isActive: false,
        }
      ],
      tableDanhSach: true,
      tableBangBieu: false,
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      todoTotalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 10,
      pageOptions: [5, 10, 25, 50, 100],
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
          class: 'td-stt',
          sortable: false,
          thStyle: {width: '70px', minWidth: '70px'},
          thClass: 'hidden-sortable text-center',
          tdClass: 'text-center'
        },
        {
          key: "ten",
          label: "Kết quả tìm kiếm",
          sortable: true,
          thStyle: "text-align:left",
        },
      ],
      data: [
        {
          id: 1,
          content: 'Báo cáo tình hình xây dựng cơ chế, chính sách pháp luật và kết quả công tác thanh tra, kiểm tra trong lĩnh vực khoa học và công nghệ'
        },
        {
          id: 2,
          content: 'Báo cáo kết quả phân bổ và sử dụng kinh phí khoa học và công nghệ '
        },
        {
          id: 3,
          content: 'Báo cáo kết quả nghiên cứu khoa học, phát triển công nghệ và kết quả hoạt động chuyển giao, ứng dụng, đổi mới công nghệ, thông tin khoa học và công nghệ'
        },
        {
          id: 4,
          content: 'Báo cáo tình hình hoạt động thẩm định cơ sở khoa học chương trình phát triển kinh tế - xã hội và thẩm định công nghệ dự án đầu tư; Thẩm định kết quả thực hiện nhiệm vụ khoa học và công nghệ không sử dụng ngân sách nhà nước'
        }
        ,
        {
          id: 5,
          content: 'Báo cáo về phát triển nhân lực khoa học và công nghệ; phát triển hạ tầng khoa học và công nghệ'
        },
        {
          id: 6,
          content: 'Báo cáo kết quả cấp Giấy chứng nhận trong hoạt động khoa học và công nghệ'
        },
        {
          id: 7,
          content: 'Báo cáo tình hình triển khai phát triển hệ sinh thái khởi nghiệp đổi mới sáng tạo quốc gia, thị trường khoa học và công nghệ'
        },
        {
          id: 8,
          content: 'Kết quả công tác quản lý về sở hữu trí tuệ và triển khai các biện pháp thúc đẩy hoạt động sáng kiến tại địa phương'
        },
        {
          id: 9,
          content: 'Tình hình xây dựng, ban hành quy chuẩn kỹ thuật quốc gia và quy chuẩn kỹ thuật địa phương'
        },
        {
          id: 10,
          content: 'Tình hình quản lý, xét tặng giải thưởng chất lượng sản phẩm, hàng hóa của tổ chức, cá nhân trên địa bàn tỉnh, thành phố'
        },
      ],
      model: mauBieuModel.baseJson(),
      listTableMauBieu: [],
      optionLoaiMauBieu: [],
      optionNamXuatBan: [],
      loaiMauBieuFilter: [],
      namXuatBanFilter: []
    }
  },
  computed: {
    ...layoutComputed
  },
  watch: {
    contentFilter: {
      deep: true,
      handler(value, oldValue) {
      },
    }
  },
  created: () => {
    document.body.setAttribute("data-layout", "horizontal");
    document.body.setAttribute("data-topbar", "dark");
    document.body.removeAttribute('data-sidebar', 'dark');
  },
  mounted() {
    this.getLoaiMauBieu();
    this.listNamMauBieu();
  },
  methods: {
    async getLoaiMauBieu() {
      try {
        await this.$store.dispatch("loaiMauBieuStore/getLoaiMauBieu").then(resp => {
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
    checkIsStar(value) {
      return value && value.length > 0 && value.includes('*');
    },
    checkIsEmpty(value) {
      return value && value.length > 0;
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
    async handleXemMauBieu(id) {
      this.tableBangBieu = true;
      this.tableDanhSach = false;
      this.handleGetMauBieuId(id);
      this.renderTableMauBieu(id);
    },
    myProvider(ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        content: this.filter,
        sortBy: ctx.sortBy,
        sortDesc: ctx.sortDesc,
        loaiMauBieuIds: this.loaiMauBieuFilter,
        namXuatBanFilter: this.namXuatBanFilter,
        // loaiMauBieu: this.loaiMauBieu,
        // paramMauBieu: this.paramMauBieu
      }
      this.loading = true
      try {
        let promise = this.$store.dispatch("homeStore/homeCongbo", params)
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
    toggleRightSidebar() {
      document.body.classList.toggle("right-bar-enabled");
    },
    hideRightSidebar() {
      document.body.classList.remove("right-bar-enabled");
    },
    handleClickFilter(event, id) {
      this.optionLoaiMauBieu.map(value => {
        if (value.id == id) {
          if (value.isActive)
            value.isActive = false;
          else
            value.isActive = true;
        }
      })
      var check = this.loaiMauBieuFilter.find(x => x == id);
      if (check) {
        this.loaiMauBieuFilter = this.loaiMauBieuFilter.filter(x => x != id);
      } else {
        this.loaiMauBieuFilter.push(id)
      }
      if (this.tableBangBieu) {
        this.model = mauBieuModel.baseJson();
        this.listTableMauBieu = [];
        this.tableDanhSach = true;
        this.tableBangBieu = false;
      }

      this.$refs.tblList?.refresh()
    },
    handleClickNamFilter(event, id) {
      this.optionNamXuatBan.map(value => {
        if (value.nam == id) {
          if (value.isActive)
            value.isActive = false;
          else
            value.isActive = true;
        }
      })
      var check = this.namXuatBanFilter.find(x => x == id);
      if (check) {
        this.namXuatBanFilter = this.namXuatBanFilter.filter(x => x != id);
      } else {
        this.namXuatBanFilter.push(id)
      }
      console.log(this.namXuatBanFilter);
      if (this.tableBangBieu) {
        this.model = mauBieuModel.baseJson();
        this.listTableMauBieu = [];
        this.tableDanhSach = true;
        this.tableBangBieu = false;
      }
      this.$refs.tblList?.refresh()
    },
    HandleClickTab(value) {
      if (value === 'DANHSACH') {
        this.model = mauBieuModel.baseJson();
        this.listTableMauBieu = [];
        this.tableDanhSach = true;
        this.tableBangBieu = false;
      } else {
        this.tableBangBieu = true
        this.tableDanhSach = false
      }
    },
    getHome() {
      this.$store.dispatch("homeStore/congBo").then(resp => {
        if (resp.resultCode == CONSTANTS.SUCCESS) {
          if (resp.data) {
            this.loaiMauBieus = resp.data.loaiMauBieus;
            this.countHome = resp.data.countHomes;
          }

        }
      })
    },
    listNamMauBieu() {
      this.$store.dispatch("mauBieuStore/listNamMauBieu").then(resp => {
        if (resp.resultCode == CONSTANTS.SUCCESS) {
          this.optionNamXuatBan = resp.data;

        }
      })
    },
  }
};
</script>

<template>
  <div id="layout-congbo">
    <Header :width="layoutWidth" :type="topbar"/>
    <section id="banner" class="bg-congbo">
      <div class="banner-box container-fluid">
        <div class="row justify-content-between align-items-center">
          <div class="col-6 ">
            <div class="banner-left d-flex flex-wrap">
              <h1 class="text-white text-uppercase mb-3">Hệ thống thông tin <br> khoa học và công nghệ</h1>
              <!--              <span class="text-blue-grey mb-3">-->
              <!--                  It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages,-->
              <!--                  and more recently with desktop.-->
              <!--                </span>-->
              <!--              <button-->
              <!--                  class="btn btn-login"-->
              <!--              >-->
              <!--                Đăng nhập-->
              <!--              </button>-->

            </div>
          </div>
          <div class="col-6">
            <div class="banner-right">
              <img src="~@/assets/images/img_SKHCN_DT.png" alt="KH&CN DT" width="100%">
            </div>
          </div>
        </div>
      </div>
    </section>
    <Navbar/>
    <div class="pages-congbo">
      <div class="container-fluid content-page">
        <section id="content" class="">
          <div class="row">
            <div class="col-lg-3 col-md-12">
              <!-- Accordin - 1 -->
              <div role="tablist" class="mb-2">
                <b-card no-body class="mb-1">
                  <b-card-header header-tag="header" role="tab" class="bg-primary">
                    <h6 class="m-0 font-14 d-flex justify-content-between">
                      <div>
                        <i class="fas fa-filter text-white me-2"></i>
                        <a
                            class="text-white"
                            href="javascript: void(0);"
                        >Loại báo cáo</a>
                      </div>
                      <i
                          v-b-toggle.accordion-1
                          class="mdi mdi-chevron-double-down text-white"
                      ></i>
                    </h6>
                  </b-card-header>
                  <b-collapse id="accordion-1" visible accordion="my-accordion" role="tabpanel">
                    <b-card-body class="p-0">
                      <div
                          v-for="(contentItem, idx) in optionLoaiMauBieu"
                          :key="idx"
                      >
                        <div
                            class="list-item d-flex justify-content-between align-items-center pointer p-2"
                            v-bind:class="{active: (contentItem.isActive === true) }"
                            @click="handleClickFilter($event, contentItem.id)"
                        >
                          <div class="d-flex align-items-center">
                            <input type="checkbox" v-model="contentItem.isActive" class="me-2"
                            >
                            <span>{{ contentItem.tenNgan }}</span>
                          </div>
                          <div>
                            {{ contentItem.countMauBieu }}
                          </div>
                        </div>
                      </div>
                    </b-card-body>
                  </b-collapse>
                </b-card>
              </div>
              <div role="tablist" class="mb-2">
                <b-card no-body class="mb-1">
                  <b-card-header header-tag="header" role="tab" class="bg-primary">
                    <h6 class="m-0 font-14 d-flex justify-content-between">
                      <div>
                        <i class="fas fa-filter text-white me-2"></i>
                        <a
                            class="text-white"
                            href="javascript: void(0);"
                        >Năm xuất bản</a>
                      </div>
                      <i
                          v-b-toggle.accordion-1
                          class="mdi mdi-chevron-double-down text-white"
                      ></i>
                    </h6>
                  </b-card-header>
                  <b-collapse id="accordion-1" visible accordion="my-accordion" role="tabpanel">
                    <b-card-body class="p-0">
                      <div
                          v-for="(contentItem, idx) in optionNamXuatBan"
                          :key="idx"
                      >
                        <div
                            class="list-item d-flex justify-content-between align-items-center pointer p-2"
                            v-bind:class="{active: (contentItem.isActive === true) }"
                            @click="handleClickNamFilter($event, contentItem.nam)"
                        >
                          <div class="d-flex align-items-center">
                            <input type="checkbox" v-model="contentItem.isActive" class="me-2"
                            >
                            <span>Năm {{ contentItem.nam }}</span>
                          </div>
                          <div>
                            {{ contentItem.count }}
                          </div>
                        </div>
                      </div>
                    </b-card-body>
                  </b-collapse>
                </b-card>
              </div>
            </div>
            <div class="col-lg-9 col-md-12" style="background-color: white; padding-top: 10px; padding-bottom: 10px">
              <!--              <div-->
              <!--                  class="d-flex align-items-center gap-2 mb-3 fillter-box"-->
              <!--              >-->
              <!--                <div class="d-flex fil-search-box">-->
              <!--                  <form-->
              <!--                      class="app-search d-lg-block p-0 w-75 me-2"-->
              <!--                  >-->
              <!--                    <div class="position-relative">-->
              <!--                      <input type="text" class="form-control ip-search bg-white text-primary" placeholder="Search..."/>-->
              <!--                      <span class="fa fa-search text-primary"></span>-->
              <!--                    </div>-->
              <!--                  </form>-->
              <!--                  <button-->
              <!--                      class="btn-item btn btn-primary fw-bold d-inline-block w-25"-->
              <!--                  >-->
              <!--                    <i class="fa fa-search me-2"></i>-->
              <!--                    <span class="d-sm-inline-block d-none">Tìm kiếm</span>-->
              <!--                  </button>-->

              <!--                </div>-->
              <!--                <div class="d-flex fil-cs-box">-->
              <!--                  <button-->
              <!--                      class="btn-item btn btn-outline-primary fw-bold d-inline-block w-75 me-2"-->
              <!--                  >-->
              <!--                    Nâng cao-->
              <!--                  </button>-->
              <!--                  <button-->
              <!--                      class="btn-item-fullscreen fw-bold d-inline-block w-25"-->
              <!--                  >-->
              <!--                    <i class="mdi mdi-arrow-expand-all text-primary fs-3"></i>-->
              <!--                  </button>-->
              <!--                </div>-->
              <!--              </div>-->
              <div class="d-flex justify-content-between align-items-center gap-2 mb-3 item-fil-tab">
                <div class="item-filter-left d-flex align-content-between">
                  <button
                      class="btn-item btn btn-primary fw-bold d-inline-block me-2 w-50"
                      @click="HandleClickTab('DANHSACH')"
                  >
                    <i class="mdi mdi-format-list-numbered text-white"></i>
                    Danh sách
                  </button>
                  <button
                      v-if="model.id != null"
                      class="btn-item btn btn-outline-primary fw-bold d-inline-block me-2 w-50"
                      style="width: 120px"
                      @click="HandleClickTab('BANGBIEU')"
                  >
                    <i class="mdi mdi-file-table-outline text-primary"></i>
                    Bảng biểu
                  </button>
                </div>
                <div v-if="tableBangBieu" class="item-filter-right d-flex align-content-end "
                     style="justify-content: end">
                  <a
                      class="d-inline-block me-2 text-dark w-50"
                      style="width: 100px; cursor: pointer"
                      @click="exportTableToExcel('dynamic-table1', model.kyHieu+'_' +model.ten)"
                  >
                    <i class="mdi mdi-file-excel-box text-dark"></i>
                    Xuất Excel
                  </a>
                  <!--                  <a-->
                  <!--                      class="d-inline-block me-2 text-dark w-50"-->
                  <!--                      style="width: 100px"-->
                  <!--                  >-->
                  <!--                    <i class="mdi mdi-file-pdf-box text-dark"></i>-->
                  <!--                    Xuất PDF-->
                  <!--                  </a>-->
                </div>
              </div>
              <div v-if="tableDanhSach">
                <!--                <div class="row mb-2">-->
                <!--                  <div class="col-sm-12 col-md-6">-->
                <!--                    <div id="tickets-table_length" class="dataTables_length">-->
                <!--                      <label class="d-inline-flex align-items-center">-->
                <!--                        Hiện-->
                <!--                        <b-form-select-->
                <!--                            class="form-select form-select-sm"-->
                <!--                            v-model="perPage"-->
                <!--                            size="sm"-->
                <!--                            :options="pageOptions"-->
                <!--                        ></b-form-select-->
                <!--                        >&nbsp;dòng-->
                <!--                      </label>-->
                <!--                    </div>-->
                <!--                  </div>-->
                <!--                </div>-->
                <b-table
                    class="datatables"
                    :items="myProvider"
                    :fields="fields"
                    striped
                    bordered
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
                  <template v-slot:cell(ten)="data">
                    <div style="cursor: pointer" @click="handleXemMauBieu(data.item.id)">
                      {{ data.item.ten }}
                    </div>
                  </template>
                </b-table>

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
              <div id="dynamic-table1" v-if="tableBangBieu">
                <div style="overflow-x:auto;" class="dynamic-table">
                  <table>
                    <tr>
                      <th>
                        <strong> Biểu số </strong>
                      </th>
                      <th colspan="4" style="text-align: center">
                        <strong style="font-size: 18px">{{ model.ten }}</strong>
                      </th>
                      <th style="width: 200px">
                        <div>
                          <div style="font-weight: bold">Đơn vị báo cáo:</div>
                          <div v-if="model.donViBaoCao">{{ model.donViBaoCao.ten }}</div>
                        </div>

                      </th>
                    </tr>
                    <tr>
                      <td>{{ model.kyHieu }}</td>
                      <td style="text-align: center" colspan="4">
                        <div v-if="model.nam" style="font-style: italic">Kỳ báo cáo: Năm {{ model.nam }}</div>
                        <div v-if="model.tuNgayDenNgay" style="font-style: italic">({{ model.tuNgayDenNgay }})</div>
                      </td>
                      <td style="width: 200px">
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
                  <div v-for="(val, index) in listTableMauBieu" :key="index" class="mb-4 mt-4">
                    <h6 style="margin-bottom: 15px" v-if="val.isHienThiTen">{{ val.tenBangBieu }}</h6>
                    <div>
                      <table class="table-bangbieu">
                        <thead v-if="val.headers">
                        <tr v-for="(row, indexRow) in val.headers" :key="indexRow">
                          <th v-for="(data, indexData) in row.tHeaderVms" :key="indexData" :rowspan="data.rowSpan"
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
                          </th>
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

                </div>

              </div>

            </div>
          </div>
        </section>
      </div>

    </div>
    <Footer/>
  </div>
</template>

<style>

#layout-congbo {
  width: 100%;
  box-sizing: border-box;
}

#banner.bg-congbo {
  background-color: #111961;
  margin-top: 70px;
}

.pages-congbo .content-page {
  margin-top: 30px;
  margin-bottom: 20px;
}

@media only screen and (max-width: 768px) {
  .pages-congbo .content-page {
    margin-top: 10px;
  }
}

.list-item.active {
  background-color: rgba(5, 94, 143, 0.1);
}

.pointer {
  cursor: pointer;
}

.pages-congbo .app-search .ip-search {
  border: 1px solid #ccc !important;
  border-radius: 0px !important;
  height: 38px;
  padding-left: 20px;
  padding-right: 40px;
  background-color: #e0e0ea;
  -webkit-box-shadow: none;
  box-shadow: none;
}

.pages-congbo .btn-item {
  height: 38px;
  border-radius: 0px;
  width: 150px;
  position: relative;
}

.pages-congbo .btn-item-fullscreen {
  height: 38px;
  border-radius: 0px;
  background-color: #fff;
  color: #055E8F;
  width: 38px;
  border: 1px solid #055E8F;
  position: relative;
}

.fillter-box {
  width: 100%;
}

.fillter-box .fil-search-box {
  width: 75%;
}

.fillter-box .fil-cs-box {
  width: 25%;
}

.item-fil-tab .item-filter-left {
  width: 240px;
}

.item-fil-tab .item-filter-right {
  width: 240px;
}

@media only screen and (max-width: 570px) {
  .fillter-box {
    flex-direction: column;
  }

  .fillter-box .fil-search-box {
    width: 100%;
  }

  .fillter-box .fil-cs-box {
    width: 100%;
  }

  .item-fil-tab {
    flex-direction: column-reverse;
  }

  .item-fil-tab .item-filter-left {
    width: 100%;
  }

  .item-fil-tab .item-filter-right {
    width: 100%;
  }

}

.banner-box {
  padding: 30px 10px;
}

#banner .btn-login {
  margin-top: 50px;
  text-transform: uppercase;
  font-weight: bold;
  padding: 10px 20px;
  background-color: #fff;
  color: #111961 !important;
  box-shadow: rgb(204, 219, 232) 3px 3px 6px 0px inset, rgba(255, 255, 255, 0.5) -3px -3px 6px 1px inset;
}
.table-bangbieu{
  border: 1px solid #ddd;
}

.dynamic-table table {
  width: 100%;
}
tr:nth-child(even){background-color: #f2f2f2}

@media only screen and (max-width: 768px){
  .table-bangbieu{
    border-spacing: 0;
    border-collapse: collapse;
    width: 100%;
    border: 1px solid #ddd;
  }
  .table-bangbieu thead tr th,
  .table-bangbieu tbody tr td{
    text-align: left;
    padding: 8px;
  }
  .table-bangbieu thead tr:nth-child(even){background-color: #f2f2f2}

  .dynamic-table{
    overflow-x: auto;
  }
  .dynamic-table table {
    border-collapse: collapse;
    border-spacing: 0;
    width: 1440px;
    border: 1px solid #ddd;
  }

  .dynamic-table table th, .dynamic-table table td {
    text-align: left;
    padding: 8px;
  }


  tr:nth-child(even){background-color: #f2f2f2}
}
</style>
