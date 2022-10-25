<script>
import {menuItems} from "@/components/menu-items";

/**
 * Horizontal-topbar component
 */
export default {
  props: {
    width: {
      type: String,
      required: true,
    },
    type: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      menuItems: menuItems,
    };
  },
  watch: {
    width: {
      immediate: true,
      handler(newVal, oldVal) {
        if (newVal !== oldVal) {
          switch (newVal) {
            case "boxed":
              document.body.setAttribute("data-layout-size", "boxed");
              break;
            case "fluid":
              document.body.removeAttribute("data-layout-size", "boxed");
              break;
            default:
              document.body.setAttribute("data-topbar", "light");
              break;
          }
        }
      },
    },
    type: {
      immediate: true,
      handler(newVal, oldVal) {
        if (newVal !== oldVal) {
          switch (newVal) {
            case "dark":
              document.body.setAttribute("data-topbar", "dark");
              break;
            case "light":
              document.body.setAttribute("data-topbar", "light");
              document.body.removeAttribute("data-layout-size", "boxed");
              break;
            default:
              document.body.setAttribute("data-topbar", "dark");
              break;
          }
        }
      },
    },
  },
  computed:{
    checkUserLogin(){
      var userLocalStorage = localStorage.getItem("user-token");
      if (userLocalStorage) {
        return true;
      }
      return false;
    }
  },
  methods: {
    initFullScreen() {
      document.body.classList.toggle("fullscreen-enable");
      if (
          !document.fullscreenElement &&
          /* alternative standard method */ !document.mozFullScreenElement &&
          !document.webkitFullscreenElement
      ) {
        // current working methods
        if (document.documentElement.requestFullscreen) {
          document.documentElement.requestFullscreen();
        } else if (document.documentElement.mozRequestFullScreen) {
          document.documentElement.mozRequestFullScreen();
        } else if (document.documentElement.webkitRequestFullscreen) {
          document.documentElement.webkitRequestFullscreen(
              Element.ALLOW_KEYBOARD_INPUT
          );
        }
      } else {
        if (document.cancelFullScreen) {
          document.cancelFullScreen();
        } else if (document.mozCancelFullScreen) {
          document.mozCancelFullScreen();
        } else if (document.webkitCancelFullScreen) {
          document.webkitCancelFullScreen();
        }
      }
    },
    toggleMenu() {
      let element = document.getElementById("topnav-menu-content");
      element.classList.toggle("show");
    },
    toggleRightSidebar() {
      this.$parent.toggleRightSidebar();
    },
    logoutUser() {
      // eslint-disable-next-line no-unused-vars
      var userLocalStorage = localStorage.getItem("user-token");
      if (userLocalStorage) {
        localStorage.removeItem("user-token");
        localStorage.removeItem("auth-user");
        localStorage.removeItem("TabData");
        // this.$router.push("/dang-nhap");
        window.location.href="/dang-nhap";
        return;
      }
    },
  },
};
</script>

<template>
  <header id="page-topbar">
    <div class="navbar-header">
      <div class="d-flex justify-content-between align-content-center w-100">
        <!-- LOGO -->
        <div class="d-flex justify-content-center align-content-center">
          <div class="navbar-brand-box">
            <a href="/" class="logo logo-dark">
            <span class="logo-sm">
              <img src="@/assets/images/logo-sm.png" alt height="22"/>
            </span>
              <span class="logo-lg">
              <img src="@/assets/images/logo-dark.png" alt height="17"/>
            </span>
            </a>
            <a href="/" class="logo logo-light">
              <span class="logo-sm">
                <img src="~@/assets/images/logo-SKHCNDT-160x160.png" alt height="50"/>
              </span>
              <span class="logo-lg d-lg-flex align-items-center">
                <img src="~@/assets/images/logo-SKHCNDT-160x160.png" alt height="50"/>
                <span class="title-logo">Sở khoa học và công nghệ<br>Đồng Tháp</span>
              </span>
            </a>
          </div>

          <button
              type="button"
              class="btn btn-sm me-2 font-size-24 d-lg-none header-item"
              @click="toggleMenu"
          >
            <i class="mdi mdi-menu"></i>
          </button>
        </div>


        <form class="app-search d-block d-lg-none">
          <div class="position-relative">
            <input type="text" class="form-control" placeholder="Tìm kiếm..."/>
            <span class="fa fa-search"></span>
          </div>
        </form>

        <b-dropdown
            class="d-inline-block d-block d-lg-none"
            right
            toggle-class="header-item"
            variant="white"
            menu-class="dropdown-menu-end"
        >
          <template v-slot:button-content>
            <img
                class="rounded-circle header-profile-user"
                src="@/assets/images/users/user-4.jpg"
                alt="Header Avatar"
            />
          </template>
          <a class="dropdown-item" href="/thong-tin-ca-nhan">
            <i
                class="mdi mdi-account-circle font-size-17 align-middle me-1"
            ></i>
            Thông tin cá nhân
          </a>
          <a class="dropdown-item d-block" href="/nhap-lieu">
            <i class="mdi mdi-cog font-size-17 align-middle me-1"></i>
            Quản lý báo báo
          </a>
          <div class="dropdown-divider"></div>
          <a class="dropdown-item text-danger" href="#" @click="logoutUser">
            <i
                class="bx bx-power-off font-size-17 align-middle me-1 text-danger"
            ></i>
            Đăng xuất
          </a>
        </b-dropdown>
      </div>
      <div class="d-flex cs-nav-hozrizontal">
        <!-- App Search-->
        <form class="app-search d-none d-lg-block" style="width: 250px !important;">
          <div class="position-relative">
            <input type="text" class="form-control" placeholder="Tìm kiếm..."/>
            <span class="fa fa-search"></span>
          </div>
        </form>

        <nav class="navbar navbar-light navbar-expand-lg topnav-menu">
          <div class="collapse navbar-collapse" id="topnav-menu-content">
            <ul class="navbar-nav d-lg-none d-inline-block">
              <li
                  v-for="(item, index) in menuItems"
                  :key="index"
                  class="nav-item dropdown"
              >
                <router-link
                    :to="item.link"
                    class="nav-link dropdown-toggle arrow-none side-nav-link"
                >
                  <i :class="`mdi ${item.icon} icon-item`"></i>
                  <span class="px-2">{{item.label}}</span>
                </router-link>
              </li>
            </ul>
            <ul class="navbar-nav">
              <li
                  class="nav-item dropdown"
              >
                <router-link
                    to="/"
                    class="nav-link dropdown-toggle arrow-none side-nav-link d-lg-inline-block d-none"
                >
                  Trang chủ
                </router-link>
              </li>
<!--              <li-->
<!--                  class="nav-item dropdown "-->
<!--              >-->
<!--                <router-link-->
<!--                    to="#"-->
<!--                    class="nav-link dropdown-toggle arrow-none side-nav-link"-->
<!--                >-->
<!--                  Giới thiệu-->
<!--                </router-link>-->
<!--              </li>-->
              <li
                  class="nav-item dropdown "
              >
                <router-link
                    to="/huong-dan-su-dung"
                    class="nav-link dropdown-toggle arrow-none side-nav-link"
                >
                  Hướng dẫn sử dụng
                </router-link>
              </li>
              <li
                  v-if="!checkUserLogin"
                  class="nav-item dropdown ms-lg-2"
              >
                <router-link
                    to="/dang-nhap"
                    class="nav-link dropdown-toggle arrow-none side-nav-link btn-login-navbar px-lg-3 ps-md-4"
                >
                  <span>Đăng nhập</span>

                </router-link>
              </li>
            </ul>


          </div>
        </nav>
        <b-dropdown
            v-if="checkUserLogin"
            class="d-inline-block d-none d-lg-block"
            right
            toggle-class="header-item"
            variant="white"
            menu-class="dropdown-menu-end"
        >
          <template v-slot:button-content>
            <img
                class="rounded-circle header-profile-user"
                src="@/assets/images/users/user-4.jpg"
                alt="Header Avatar"
            />
          </template>
          <a class="dropdown-item" href="/thong-tin-ca-nhan">
            <i
                class="mdi mdi-account-circle font-size-17 align-middle me-1"
            ></i>
            Thông tin cá nhân
          </a>
          <a class="dropdown-item d-block" href="/nhap-lieu">
            <i class="mdi mdi-cog font-size-17 align-middle me-1"></i>
            Quản lý báo báo
          </a>
          <div class="dropdown-divider"></div>
          <a class="dropdown-item text-danger" href="#" @click="logoutUser">
            <i
                class="bx bx-power-off font-size-17 align-middle me-1 text-danger"
            ></i>
            Đăng xuất
          </a>
        </b-dropdown>
      </div>
    </div>
  </header>
</template>

<style>

@media only screen and (max-width: 768px){
  .navbar{
    padding: 0px;
    background-color: #111961;
  }
  #topnav-menu-content.show{
    width: 100%;
    height: 100vh;
    padding: 10px 0px;
  }

  #topnav-menu-content .navbar-nav{
    width: 100%;
  }

  #topnav-menu-content li.nav-item .nav-link{
    color: #fff !important;
    padding: 10px 20px;

  }
  #topnav-menu-content li.nav-item .nav-link.router-link-exact-active{
    background-color: #fff;
    color: #111961 !important;
    width: 75%;
    border-top-right-radius: 50px;
    border-bottom-right-radius: 50px;
  }

  .btn-login-navbar {
    width: fit-content;
    margin-left: 18px;
  }

  .btn-login-navbar>span{
    color: #111961;
    font-weight: bold;
  }
}

#page-topbar {
  background-color: #111961 !important;
}

#page-topbar .title-logo {
  color: #fff !important;
  line-height: 1.4em;
  font-size: 15px;
  text-transform: uppercase;
  font-weight: bold;
  text-align: start;
  padding-left: 10px;
}

@media only screen and (max-width: 1024px){
  #page-topbar .title-logo {
    font-size: 12px;
  }
}

@media only screen and (max-width: 768px){
  #page-topbar .title-logo {
    display: none;
  }
}


/*#page-topbar li.nav-item .nav-link.router-link-exact-active:before {*/
/*  content: "";*/
/*  display: block;*/
/*  height: 2px;*/
/*  width: 100%;*/
/*  background-color: rgba(255, 255, 255, 0.5);*/
/*  position: absolute;*/
/*  bottom: -2px;*/
/*  left: 0px;*/
/*  border-radius: 50px;*/
/*}*/

#page-topbar li.nav-item .nav-link {
  color: #ccc;
  cursor: pointer;
}

#page-topbar li.nav-item .nav-link.router-link-exact-active {
  color: #fff;
  cursor: pointer;
  position: relative;
}

.btn-login-navbar {
  padding: 8px 10px;
  background-color: #f5f5f5;
  border-radius: 5px;
  color: #2a2a2a !important;
}

@media only screen and (max-width: 768px) {
  .navbar-header {
    align-items: flex-start;
    flex-direction: column;
    align-content: flex-start;
    padding: 0px !important;
  }

  .cs-nav-hozrizontal {
    flex-direction: column;
    width: 100%;
  }
}

</style>
