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
import {menuItems} from "@/components/menu-items";
import {CONSTANTS} from "@/helpers/constants";


/**
 * Horizontal-layout component
 */
export default {
  components: {Header, RightBar, Navbar, Footer},
  data() {
    return {
      menuItems: menuItems,
      settings: {
        "dots": true,
        "infinite": false,
        "speed": 500,
        "slidesToShow": 6,
        "slidesToScroll": 6,
        "initialSlide": 0,
        "responsive": [
          {
            "breakpoint": 1024,
            "settings": {
              "slidesToShow": 4,
              "slidesToScroll": 4,
              "infinite": true,
              "dots": true
            }
          },
          {
            "breakpoint": 600,
            "settings": {
              "slidesToShow": 3,
              "slidesToScroll": 3,
              "initialSlide": 2
            }
          },
          {
            "breakpoint": 480,
            "settings": {
              "slidesToShow": 2,
              "slidesToScroll": 2
            }
          }
        ]
      },
      loaiMauBieus: [],
      countHome: []
    }
  },
  computed: {
    ...layoutComputed
  },
  created: () => {
    document.body.setAttribute("data-layout", "horizontal");
    document.body.setAttribute("data-topbar", "dark");
    document.body.removeAttribute('data-sidebar', 'dark');

  },
  mounted() {
    this.getHome();
  },
  methods: {
    toggleRightSidebar() {
      document.body.classList.toggle("right-bar-enabled");
    },
    hideRightSidebar() {
      document.body.classList.remove("right-bar-enabled");
    },
    handleClickIsActive(event, id) {
      this.contentFilter.map((value, index) => {
        if (value.id == id) {
          value.isActve = event.target.checked;
        }
      })
    },
    getHome() {
      this.$store.dispatch("homeStore/homeValue").then(resp => {
        if (resp.resultCode == CONSTANTS.SUCCESS) {
          if(resp.data){
            this.loaiMauBieus = resp.data.loaiMauBieus;
            this.countHome = resp.data.countHomes;
          }

        }
      })
    },
  }
};
</script>

<template>
  <div class="background-primary">
    <div id="layout-wrapper" class="bg-layout">
      <Header :width="layoutWidth" :type="topbar"/>
      <div class="main-content">
        <!--    Banner -->
        <section id="banner" class="container-fluid">
          <div class="banner-box">
            <div class="row justify-content-between align-items-center">
              <div class="col-6 ">
                <div class="banner-left d-flex flex-column flex-wrap">
                  <h1 class="title-banner text-white text-uppercase mb-3">Hệ thống thông tin <br> khoa học và công nghệ</h1>
                  <span class="text-blue-grey mb-3">
<!--                  It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages,-->
<!--                  and more recently with desktop.-->
                </span>
<!--                  <button-->
<!--                      class="btn btn-login"-->
<!--                      style="width: fit-content"-->
<!--                  >-->
<!--                    Đăng nhập-->
<!--                  </button>-->

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
        <section id="categories" class="container-fluid" v-if="countHome && countHome.length > 0">
          <div class="row list-cat">
            <div class="col-6 col-md-4 mb-4" v-for="(item, index) in countHome" :key="index">
              <div class="card-item text-center">
                <router-link
                    to="/cong-bo"
                >
                  <div class="card-icon">
                    <i :class="item.icon"></i>
                  </div>
                  <div class="card-number text-center">
                    <span>{{item.count}}</span>
                  </div>
                  <div class="card-text text-center">
                    <span>{{item.name}}</span>
                  </div>
                </router-link>
              </div>
            </div >
<!--            <div class="col-6 col-md-3 mb-4">-->
<!--              <div class="card-item text-center">-->
<!--                <router-link-->
<!--                    to="/cong-bo"-->
<!--                >-->
<!--                  <div class="card-icon">-->
<!--                    <i class="mdi mdi-clipboard-check"></i>-->
<!--                  </div>-->
<!--                  <div class="card-number text-center">-->
<!--                    <span>136759</span>-->
<!--                  </div>-->
<!--                  <div class="card-text text-center">-->
<!--                    <span>CSDL Nhiệm vụ</span>-->
<!--                  </div>-->
<!--                </router-link>-->
<!--              </div>-->
<!--            </div>-->
<!--            <div class="col-6 col-md-3 mb-4">-->
<!--              <div class="card-item text-center">-->
<!--                <router-link-->
<!--                    to="/cong-bo"-->
<!--                >-->
<!--                  <div class="card-icon">-->
<!--                    <i class="mdi mdi-account-multiple"></i>-->
<!--                  </div>-->
<!--                  <div class="card-number text-center">-->
<!--                    <span>136759</span>-->
<!--                  </div>-->
<!--                  <div class="card-text text-center">-->
<!--                    <span>CSDL Chuyên gia</span>-->
<!--                  </div>-->
<!--                </router-link>-->
<!--              </div>-->
<!--            </div>-->
<!--            <div class="col-6 col-md-3 mb-4">-->
<!--              <div class="card-item text-center">-->
<!--                <router-link-->
<!--                    to="/cong-bo"-->
<!--                >-->
<!--                  <div class="card-icon">-->
<!--                    <i class="mdi mdi-bank"></i>-->
<!--                  </div>-->
<!--                  <div class="card-number text-center">-->
<!--                    <span>136759</span>-->
<!--                  </div>-->
<!--                  <div class="card-text text-center">-->
<!--                    <span>CSDL Tổ chức</span>-->
<!--                  </div>-->
<!--                </router-link>-->
<!--              </div>-->
<!--            </div>-->
<!--            <div class="col-6 col-md-3 mb-4">-->
<!--              <div class="card-item text-center">-->
<!--                <router-link-->
<!--                    to="/cong-bo"-->
<!--                >-->
<!--                  <div class="card-icon">-->
<!--                    <i class="mdi mdi-memory"></i>-->
<!--                  </div>-->
<!--                  <div class="card-number text-center">-->
<!--                    <span>136759</span>-->
<!--                  </div>-->
<!--                  <div class="card-text text-center">-->
<!--                    <span>CSDL Công nghệ</span>-->
<!--                  </div>-->
<!--                </router-link>-->
<!--              </div>-->
<!--            </div>-->
<!--            <div class="col-6 col-md-3 mb-4">-->
<!--              <div class="card-item text-center">-->
<!--                <router-link-->
<!--                    to="/cong-bo"-->
<!--                >-->
<!--                  <div class="card-icon">-->
<!--                    <i class="mdi mdi-lightbulb-on-outline"></i>-->
<!--                  </div>-->
<!--                  <div class="card-number text-center">-->
<!--                    <span>136759</span>-->
<!--                  </div>-->
<!--                  <div class="card-text text-center">-->
<!--                    <span>CSDL Sáng chế</span>-->
<!--                  </div>-->
<!--                </router-link>-->
<!--              </div>-->
<!--            </div>-->
<!--            <div class="col-6 col-md-3 mb-4">-->
<!--              <div class="card-item text-center">-->
<!--                <router-link-->
<!--                    to="/cong-bo"-->
<!--                >-->
<!--                  <div class="card-icon">-->
<!--                    <i class="mdi mdi-book-variant"></i>-->
<!--                  </div>-->
<!--                  <div class="card-number text-center">-->
<!--                    <span>136759</span>-->
<!--                  </div>-->
<!--                  <div class="card-text text-center">-->
<!--                    <span>CSDL Tiêu chuẩn</span>-->
<!--                  </div>-->
<!--                </router-link>-->
<!--              </div>-->
<!--            </div>-->
<!--            <div class="col-6 col-md-3 mb-4">-->
<!--              <div class="card-item text-center">-->
<!--                <router-link-->
<!--                    to="/cong-bo"-->
<!--                >-->
<!--                  <div class="card-icon">-->
<!--                    <i class="mdi mdi-layers"></i>-->
<!--                  </div>-->
<!--                  <div class="card-number text-center">-->
<!--                    <span>136759</span>-->
<!--                  </div>-->
<!--                  <div class="card-text text-center">-->
<!--                    <span>Dữ liệu mở</span>-->
<!--                  </div>-->
<!--                </router-link>-->
<!--              </div>-->
<!--            </div>-->
          </div>
        </section>
        <!--      Danh mục -->
        <section id="categories-box" class="container-fluid" v-if="loaiMauBieus && loaiMauBieus.length > 0">
          <div class="line-gradient"></div>
          <div class="row list-content">
            <div class="col-12 col-md-6 mb-4" v-for="(lmb, index) in loaiMauBieus" :key="index">
              <div class="item-content">
                <div class="item-title d-flex align-items-center">
                  <i class="mdi mdi-folder-open-outline fs-3 me-2"></i>
                  <p class="mb-0">{{lmb.name}}</p>
                </div>
                <div class="item-content-box" v-if="lmb.mauBieus && lmb.mauBieus.length > 0">
                  <div class="item-1 d-flex align-items-stretch" v-for="(mb, indexMB) in lmb.mauBieus" :key="indexMB">
                    <i class="mdi mdi-checkbox-blank-circle text-secondary py-1 me-2" style="font-size: 8px;"></i>
                    <span class="text-secondary ">
                   {{mb.ten}}
                  </span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>
        <!--      Danh sách các vị đơn vị -->
<!--        <section id="brand-box" class="container-fluid">-->
<!--          <div class="slide-title text-white">DANH SÁCH CÁC ĐƠN VỊ TÍCH HỢP DỮ LIỆU</div>-->
<!--          <div class="list-brand">-->
<!--            <VueSlickCarousel v-bind="settings">-->
<!--              <div class="text-center"><img src="@/assets/images/logo-SKHCNDT-160x160.png" alt="brand" height="120"></div>-->
<!--              <div><img src="@/assets/images/logo-SKHCNDT-160x160.png" alt="brand" height="120"></div>-->
<!--              <div><img src="@/assets/images/logo-SKHCNDT-160x160.png" alt="brand" height="120"></div>-->
<!--              <div><img src="@/assets/images/logo-SKHCNDT-160x160.png" alt="brand" height="120"></div>-->
<!--              <div><img src="@/assets/images/logo-SKHCNDT-160x160.png" alt="brand" height="120"></div>-->
<!--              <div><img src="@/assets/images/logo-SKHCNDT-160x160.png" alt="brand" height="120"></div>-->
<!--              <div><img src="@/assets/images/logo-SKHCNDT-160x160.png" alt="brand" height="120"></div>-->
<!--              <div><img src="@/assets/images/logo-SKHCNDT-160x160.png" alt="brand" height="120"></div>-->
<!--              <div><img src="@/assets/images/logo-SKHCNDT-160x160.png" alt="brand" height="120"></div>-->
<!--              <div><img src="@/assets/images/logo-SKHCNDT-160x160.png" alt="brand" height="120"></div>-->
<!--            </VueSlickCarousel>-->
<!--          </div>-->
<!--        </section>-->
      </div>
      <RightBar/>
    </div>
    <br />
    <Footer/>
  </div>
</template>

<style>
.background-primary{
  background-color: #111961 !important;
}

.bg-layout {
  background-color: #111961 !important;
}

@media only screen and (max-width: 1024px){
  .bg-layout {
    padding: 20px;
  }
}

.banner-box {
  /*background: linear-gradient(135d);*/
  /*background-image: url("~@/assets/images/banner.jpeg");*/
  /*background-repeat: no-repeat;*/
  /*background-size: cover;*/
  /*background-position: center;*/
  height: 100%;
  width: 100%;
  margin-top: 70px;
  position: relative;
}

.search-box {
  position: absolute;
  top: 50%;
  left: 40%;
}

.ip-search {
  background-color: rgba(255, 255, 255, 0.8) !important;
  color: #3e4094;
}

/* ====== Category ====== */
#categories {
  padding: 60px 0px;
}

.card-item {
  background-color: rgba(149, 167, 225, 0.1);
  border-radius: 10px;
  padding: 20px;
  box-sizing: border-box;
}

.card-item .card-icon {
  font-size: 50px;
  color: #fff;
  padding-bottom: 5px;
}

.card-item .card-number {
  font-size: 24px;
  color: #fff;
  font-weight: bold;
}

.card-item .card-text {
  font-size: 14px;
  color: #fff;
}

.card-item:hover {
  background-color: #f5f5f5;
  filter: drop-shadow(-4px -4px 20px rgba(48, 78, 124, 0.6)) drop-shadow(4px 4px 20px rgba(42, 35, 35, 0.6));
  backdrop-filter: blur(2px);
}

.card-item:hover .card-icon {
  color: #111961;
}

.card-item:hover .card-number {
  color: #111961;
}

.card-item:hover .card-text {
  color: #111961;
}

.line-gradient:after{
  content: "";
  display: block;
  width: 100%;
  height: 2px;
  background-image: linear-gradient(90deg, rgba(255, 255, 255, 0), #fff, rgba(255, 255, 255, 0));
  position: absolute;
  top: 0px;
  right: 0px;
}
.line-gradient{
  position: relative;
  width: 100%;
  display: flex;
  justify-content: center;
  /*box-shadow: rgb(125, 196, 211) 0px 50px 100px -20px, rgba(255, 255, 255, 0.41) 0px 30px 60px 30px;*/
  margin-bottom: 100px
}

.item-content {
  background-color: rgba(149, 167, 225, 0.1);
  border-radius: 10px;
  padding: 20px;
  box-sizing: border-box;
}

.item-content .item-title {
  color: #fff;
  font-weight: bold;
  position: relative;
  margin-bottom: 20px;
}

.item-content .item-title:before {
  content: "";
  display: block;
  position: absolute;
  left: 0;
  bottom: -10px;
  width: 50px;
  height: 2px;
  background-color: rgba(255, 255, 255, 0.5);

}


.mdi:before, .mdi-set {
  display: inline-block;
  font: normal normal normal 24px/1 "Material Design Icons";
  font-size: inherit;
  text-rendering: auto;
  line-height: inherit;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

section#brand-box {
  padding: 30px;
  height: 350px;
}

#brand-box .slide-title {
  text-align: center;
  font-size: 16px;
  text-transform: uppercase;
  position: relative;
  font-weight: bold;
  margin: 30px auto 45px !important;
  padding-bottom: 10px !important;
}

#brand-box .slide-title:before {
  content: "";
  position: absolute;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);
  width: 100px;
  height: 4px;
  border-radius: 50px;
  background-color: rgba(255, 255, 255, 0.5);
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

@media only screen and (max-width: 425px){
  .title-banner{
    font-size: 13px;
  }
  #banner .btn-login {
    margin-top: 0px;
  }
  .line-gradient{
    margin-bottom: 30px
  }
  #categories{
    padding: 30px 0px;
  }
}
</style>
