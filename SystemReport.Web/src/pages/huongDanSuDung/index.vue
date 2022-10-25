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
  components: {Header, Footer,},
  data() {
    return {
      activeIndex: [],
      isActiveIdx: null,
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
    <section id="banner" class="bg-hdsd">
      <div class="banner-box container-fluid">
        <div class="row justify-content-between align-items-center">
          <div class="col-12">
            <div class="banner-left text-center">
              <h1 class="text-white text-uppercase mb-3">Hướng dẫn sử dụng</h1>
            </div>
          </div>
        </div>
      </div>
    </section>
    <section id="content-wapper" style="padding: 30px; min-height: 400px;" >
      <div class="content-box container-fluid">
        <div class="row align-items-start">
          <div class="col-md-3 col-12 d-flex justify-content-center flex-column">
            <!--            item - 1-->
            <b-card-header
                v-b-toggle.accordion-1
                header-tag="header" role="tab" class="mb-2 card-accordion">
              <h6 class="m-0 font-14">
                <a
                    class="card-link"
                    href="javascript: void(0);"
                >Thông tin</a>
              </h6>
            </b-card-header>
            <b-collapse id="accordion-1" visible accordion="my-accordion" role="tabpanel" class="d-sm-none">
              <b-card-body>
                <b-card-text>
                  1 Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid.
                  Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident.
                </b-card-text>
              </b-card-body>
            </b-collapse>
            <!--            item - 2-->
            <b-card-header
                v-b-toggle.accordion-2
                header-tag="header" role="tab" class="mb-2 card-accordion">
              <h6 class="m-0 font-14">
                <a
                    class="card-link"
                    href="javascript: void(0);"
                >Hình ảnh</a>
              </h6>
            </b-card-header>
            <b-collapse id="accordion-2" visible accordion="my-accordion" role="tabpanel" class="d-sm-none">
              <b-card-body>
                <b-card-text>
                  2 Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid.
                  Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident.
                </b-card-text>
              </b-card-body>
            </b-collapse>
            <!--            item - 3-->
            <b-card-header
                v-b-toggle.accordion-3
                header-tag="header" role="tab" class="mb-2 card-accordion">
              <h6 class="m-0 font-14">
                <a
                    class="card-link"
                    href="javascript: void(0);"
                >Video</a>
              </h6>
            </b-card-header>
            <b-collapse id="accordion-3" visible accordion="my-accordion" role="tabpanel" class="d-sm-none">
              <b-card-body>
                <b-card-text>
                  2 Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid.
                  Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident.
                </b-card-text>
              </b-card-body>
            </b-collapse>
          </div>
          <div class="col-md-9 col-12 border content-show">
            <b-collapse id="accordion-1" visible accordion="my-accordion" role="tabpanel">
              <b-card-body>
                <b-card-text>
                  1 Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid.
                  Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident.
                </b-card-text>
              </b-card-body>
            </b-collapse>
            <b-collapse id="accordion-2" visible accordion="my-accordion" role="tabpanel">
              <b-card-body>
                <b-card-text>
                  2 Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid.
                  Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident.
                </b-card-text>
              </b-card-body>
            </b-collapse>
            <b-collapse id="accordion-3" visible accordion="my-accordion" role="tabpanel">
              <b-card-body>
                <b-card-text>
                  3 Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid.
                  Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident.
                </b-card-text>
              </b-card-body>
            </b-collapse>

          </div>
        </div>
      </div>
    </section>
    <Footer/>
  </div>
</template>

<style>

#layout-congbo {
  width: 100%;
  box-sizing: border-box;
}

#banner.bg-hdsd {
  background-color: #111961;
  padding: 50px;
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

.table-bangbieu {
  border: 1px solid #ddd;
}

.dynamic-table table {
  width: 100%;
}

tr:nth-child(even) {
  background-color: #f2f2f2
}

@media only screen and (max-width: 768px) {
  .table-bangbieu {
    border-spacing: 0;
    border-collapse: collapse;
    width: 100%;
    border: 1px solid #ddd;
  }

  .table-bangbieu thead tr th,
  .table-bangbieu tbody tr td {
    text-align: left;
    padding: 8px;
  }

  .table-bangbieu thead tr:nth-child(even) {
    background-color: #f2f2f2
  }

  .dynamic-table {
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

  tr:nth-child(even) {
    background-color: #f2f2f2
  }
}

.card-accordion {
  border-radius: 5px;
}

.card-accordion.not-collapsed {
  background-color: #111961;
  border-radius: 5px;
}

.card-accordion.not-collapsed .card-link {
  color: #fff;
}

.card-link {
  color: #2a2a2a;
}

@media only screen and (max-width: 425px){
  .content-show{
    display: none;
  }
}

</style>
