<script>/**
 * Topbar component
 */
import simplebar from "simplebar-vue";
import {thongBaoModel} from "@/models/thongBaoModel";
import {userModel} from "@/models/userModel";

export default {
  data() {
    return {
      currentUserAuth: null,
      url:  process.env.VUE_APP_API_URL + 'files/view/' ,
      model: thongBaoModel.baseJson(),
      modelUser: userModel.baseJson(),
      listNotify: [],
      showModal: false
    };
  },
  components:{simplebar},
  mounted() {
    this.getListNotify();
  },

  created() {
    let authUser = localStorage.getItem("auth-user");
    if(authUser){
      let jsonUserCurrent = JSON.parse(authUser);
      this.currentUserAuth = jsonUserCurrent.user;
    }
  },
  methods: {
    handleXemTatCa(){
      if(this.$route.path != '/thong-bao')
       this.$router.push('/thong-bao')
    },
    handleThongTinCaNhan(){
      if (this.$route.path != '/thong-tin-ca-nhan') {
        this.$router.push("/thong-tin-ca-nhan");
      }
    },
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
      this.$parent.toggleMenu();
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
        this.$router.push("/dang-nhap");
        return;
      }
    },
    async handleNotifyDetail(id) {
      await this.$store
          .dispatch("notificationStore/getById", id)
          .then((res) => {
            this.model = res;
            this.showModal = true;
          });
    },
    async handleDetailUser(id) {
      await this.$store.dispatch("userStore/getById", id).then((res) => {
        this.modelUser = userModel.fromJson(res.data);
        this.showModal = true;
      });
    },
    async handleChangeStatus(id) {
      this.model.id = id;
      await this.$store.dispatch("notifyStore/changeStatus", this.model).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.getListNotify();
        }
      });
    },
    getListNotify() {
      this.$store.dispatch("notifyStore/getListNotify").then((res) => {
        this.listNotify = res.data || [];
      });
    },
  },
};
</script>
<template>
  <header id="page-topbar">
    <div class="navbar-header" style="background-color: #111961">
      <div class="d-flex">
        <!-- LOGO -->
        <div class="navbar-brand-box d-flex align-items-center" style="">
          <router-link to="/" class="logo logo-dark">
            <span class="logo-sm">
              <img src="@/assets/images/logo-sm.png" alt height="22" />
            </span>
            <span class="logo-lg">
              <img src="@/assets/images/logo-dark.png" alt height="17" />
            </span>
          </router-link>

          <router-link to="/" class="logo logo-light d-flex align-items-center" style="width: 100%; display: flex; justify-content: center">
            <span class="logo-sm">
              <img src="@/assets/images/DTHU.png" class="logo-dthu" alt="logo dthu" />
            </span>
            <span class="logo-lg" style="width: 80%">
              <img src="@/assets/images/d-office.png" alt  style="width: 100%"/>
            </span>
          </router-link>
        </div>

        <button
          type="button"
          class="btn btn-sm px-3 font-size-24 header-item"
          id="vertical-menu-btn"
          @click="toggleMenu()"
        >
          <i class="mdi mdi-menu" style="color: white"></i>
        </button>
        <div class="d-none d-sm-block">
          <h5 class="pt-4 d-inline-block text-white">HỆ THỐNG THÔNG TIN KHOA HỌC VÀ CÔNG NGHỆ</h5>
        </div>

<!--        <div class="d-none d-sm-block">-->
<!--          <b-dropdown variant="white" class="pt-3 d-inline-block">-->
<!--            <template v-slot:button-content>-->
<!--              Create-->
<!--              <i class="mdi mdi-chevron-down"></i>-->
<!--            </template>-->
<!--            <b-dropdown-item>Action</b-dropdown-item>-->
<!--            <b-dropdown-item>Another action</b-dropdown-item>-->
<!--            <b-dropdown-item>Something else here</b-dropdown-item>-->
<!--            <b-dropdown-divider></b-dropdown-divider>-->
<!--            <b-dropdown-item>Separated link</b-dropdown-item>-->
<!--          </b-dropdown>-->
<!--        </div>-->
      </div>

      <div class="d-flex">

<!--        <b-dropdown-->
<!--          variant="white"-->
<!--          class="d-inline-block"-->
<!--          toggle-class="header-item noti-icon"-->
<!--          menu-class="dropdown-menu-lg dropdown-menu-end"-->
<!--        >-->
<!--          <template v-slot:button-content>-->
<!--            <i class="mdi mdi-magnify"></i>-->
<!--          </template>-->
<!--          <form class="p-3">-->
<!--            <div class="form-group m-0">-->
<!--              <div class="input-group">-->
<!--                <input-->
<!--                  type="text"-->
<!--                  class="form-control"-->
<!--                  placeholder="Search ..."-->
<!--                  aria-label="Recipient's username"-->
<!--                />-->
<!--                <div class="input-group-append">-->
<!--                  <button class="btn btn-primary" type="submit">-->
<!--                    <i class="mdi mdi-magnify"></i>-->
<!--                  </button>-->
<!--                </div>-->
<!--              </div>-->
<!--            </div>-->
<!--          </form>-->
<!--        </b-dropdown>-->
        <!-- App Search-->

<!--        <b-dropdown-->
<!--          class="d-none d-md-block ms-2"-->
<!--          toggle-class="header-item"-->
<!--          menu-class="dropdown-menu-end"-->
<!--          right-->
<!--          variant="white"-->
<!--        >-->
<!--          <template v-slot:button-content>-->
<!--            <img-->
<!--              class="me-2"-->
<!--              src="@/assets/images/flags/us_flag.jpg"-->
<!--              alt="Header Language"-->
<!--              height="16"-->
<!--            />-->
<!--            English-->
<!--            <span class="mdi mdi-chevron-down"></span>-->
<!--          </template>-->
<!--          <a href="javascript:void(0);" class="dropdown-item notify-item">-->
<!--            <img-->
<!--              src="@/assets/images/flags/germany_flag.jpg"-->
<!--              alt="user-image"-->
<!--              class="me-1"-->
<!--              height="12"-->
<!--            />-->
<!--            <span class="align-middle">German</span>-->
<!--          </a>-->

<!--          &lt;!&ndash; item&ndash;&gt;-->
<!--          <a href="javascript:void(0);" class="dropdown-item notify-item">-->
<!--            <img-->
<!--              src="@/assets/images/flags/italy_flag.jpg"-->
<!--              alt="user-image"-->
<!--              class="me-1"-->
<!--              height="12"-->
<!--            />-->
<!--            <span class="align-middle">Italian</span>-->
<!--          </a>-->

<!--          &lt;!&ndash; item&ndash;&gt;-->
<!--          <a href="javascript:void(0);" class="dropdown-item notify-item">-->
<!--            <img-->
<!--              src="@/assets/images/flags/french_flag.jpg"-->
<!--              alt="user-image"-->
<!--              class="me-1"-->
<!--              height="12"-->
<!--            />-->
<!--            <span class="align-middle">French</span>-->
<!--          </a>-->

<!--          &lt;!&ndash; item&ndash;&gt;-->
<!--          <a href="javascript:void(0);" class="dropdown-item notify-item">-->
<!--            <img-->
<!--              src="@/assets/images/flags/spain_flag.jpg"-->
<!--              alt="user-image"-->
<!--              class="me-1"-->
<!--              height="12"-->
<!--            />-->
<!--            <span class="align-middle">Spanish</span>-->
<!--          </a>-->

<!--          &lt;!&ndash; item&ndash;&gt;-->
<!--          <a href="javascript:void(0);" class="dropdown-item notify-item">-->
<!--            <img-->
<!--              src="@/assets/images/flags/russia_flag.jpg"-->
<!--              alt="user-image"-->
<!--              class="me-1"-->
<!--              height="12"-->
<!--            />-->
<!--            <span class="align-middle">Russian</span>-->
<!--          </a>-->
<!--        </b-dropdown>-->

        <div class="dropdown d-none d-lg-inline-block">
          <button
            type="button"
            class="btn header-item noti-icon"
            @click="initFullScreen"
          >
            <i class="mdi mdi-fullscreen text-white"></i>
          </button>
        </div>
        <!-- ./start Notify -->
        <b-dropdown
            class="d-inline-block"
            toggle-class="header-item noti-icon"
            menu-class="dropdown-menu-lg p-0 dropdown-menu-end"
            right
            variant="white"
        >
          <template v-slot:button-content>
            <i class="mdi mdi-bell-outline text-white"></i>
            <span v-if="listNotify.number > 0" class="badge bg-danger rounded-pill"
            >{{ listNotify.number }}
            </span>
          </template>
          <div class="p-3">
            <div class="row align-items-center">
              <div class="col">
                <h6 class="m-0">Thông báo</h6>
              </div>
              <div class="col-auto">
                <a href="/thong-bao" class="small"> Tất cả </a>
              </div>
            </div>
          </div>
          <simplebar style="max-height: 300px">
            <a
                href="javascript: void(0);"
                class="text-reset notification-item"
                v-for="items in listNotify.listNotify"
                :key="items.id"
                @click="handleNotifyDetail(items.id),handleDetailUser(items.senderId),handleChangeStatus(items.id)"
            >
              <div>
                <div class="d-flex align-items-center">
                  <div class="avatar-xs me-3">
                    <span
                        class="bg-primary rounded-circle font-size-16 text-light p-2 mt-2"
                    >
                      <i class="fas fa-scroll"></i>
                    </span>
                  </div>
                  <div class="flex-grow-1 notify-content-box px-2">
                    <h6 class="mt-0 mb-1  title-capso">
                      {{ items.title }}
                    </h6>
                    <div class="font-size-12 text-muted ntf-content">
                      <p class="mb-1" :inner-html.prop="items.content | truncate(60)">
                      </p>
                    </div>
                  </div>
                  <div class="icon-box p-2">
                    <i v-if="items.read == true" class="bx bxs-circle text-light"></i>
                    <i v-else-if="items.read == false" class="bx bxs-circle text-primary"></i>
                  </div>
                </div>
              </div>
            </a>

          </simplebar>
          <div class="p-2 border-top">
            <div class="d-grid">
              <a
                  class="btn btn-sm btn-link font-size-14 text-center"
                  href="javascript:void(0)"
                  @click="handleXemTatCa"
              >
                Xem tất cả
              </a>
            </div>
          </div>
        </b-dropdown>
        <!-- ./ end Notify -->
        <b-dropdown
          class="d-inline-block"
          right
          toggle-class="header-item"
          variant="white"
          menu-class="dropdown-menu-end"

        >
<!--          <template v-slot:button-content>-->
<!--            <img-->
<!--              class="rounded-circle header-profile-user"-->
<!--              src="@/assets/images/users/user-4.jpg"-->
<!--              alt="Header Avatar"-->
<!--            />-->

<!--          </template>-->
          <template v-slot:button-content >
           <div style="display:flex; justify-content:center; align-content:center">
                         <span>
              <span v-if="currentUserAuth.avatar" >
                <b-img
                    :src=" url + `${currentUserAuth.avatar.fileId}`"
                    alt="Avatar"
                    class="rounded-circle header-profile-user"
                >
                </b-img>
              </span>
              <span v-else>
                <img
                    class="rounded-circle header-profile-user"
                    src="@/assets/images/users/user-4.jpg"
                    alt="Avatar"
                />
              </span>
            </span>&nbsp;
             <span class="d-none d-xl-inline-block ms-1">
              <div v-if="currentUserAuth" style="font-size: 10px;">
                <div style="font-size: 12px; color: white">
                  {{currentUserAuth.fullName}}
                  <i class="mdi mdi-chevron-down d-none d-xl-inline "></i>
                </div>
                <div class="text-start text-white">
                  @{{currentUserAuth.userName}}
                </div>
              </div>
              <div v-else>
               AnhDev99
              </div>
              </span
              >
           </div>

          </template>
          <b-dropdown-item>
            <a v-on:click="handleThongTinCaNhan">
              <i
                  class="mdi mdi-account-circle font-size-17 align-middle me-1"
              ></i>
              Thông tin cá nhân
            </a>
          </b-dropdown-item>
          <div class="dropdown-divider"></div>
          <a class="dropdown-item text-danger" v-on:click="logoutUser">
            <i
              class="bx bx-power-off font-size-17 align-middle me-1 text-danger"
            ></i>
            Đăng xuất
          </a>
        </b-dropdown>
      </div>
    </div>
    <b-modal
        v-model="showModal"
        id="modal-lg"
        size="lg"
        body-class="p-0"
        hide-footer
    >
      <template #modal-header="{ }">
        <!-- Emulate built in modal header close button action -->
        <h5> Thông báo</h5>
        <div class="text-end">
          <b-button variant="light" size="sm" style="width: 80px" @click="showModal = false">
            Đóng
          </b-button>
        </div>
      </template>
      <div class="card-body p-3">
        <h4 class="mt-0 font-size-16 title-capso" style="font-weight: bold">{{model.title}}</h4>
        <div class="d-flex mb-2  mt-2">
          <span>
              <span v-if="modelUser.avatar">
                <img
                    :src="url + `${modelUser.avatar.fileId}`"
                    alt="Avatar"
                    class="d-flex me-3 rounded-circle avatar-sm"
                />

              </span>
              <span v-else>
                <img
                    class="d-flex me-3 rounded-circle avatar-sm"
                    src="@/assets/images/4.png"
                    alt="Avatar"
                />
              </span></span>&nbsp;
          <div class="flex-grow-1">
            <h5 class="font-size-14 mt-1">{{model.sender}}</h5>
            <small class="text-muted">tới {{model.recipient}}</small>

          </div>
          <div class="flex-grow-2">
            <small class="text-muted" style="float: right"><i>Ngày gửi: {{ model.createdAtShow }} {{model.createdAtTimeShow}}</i></small>
          </div>
        </div>
        <p v-if="model.content" :inner-html.prop="model.content">
        </p>
      </div>
    </b-modal>
  </header>
</template>
<style>
.title-capso{
  font-weight: bold; color: #00568C; margin-right: 10px;

}
</style>

<style>
  .logo-dthu{
    width: 50px;
  }
  @media only screen only (max-width: 768px){
    .logo-dthu{
      height: 100%;
    }
  }
  .navbar-brand-box{
    background-color: transparent;
  }
</style>