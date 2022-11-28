<script>
import { required } from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import Vue from "vue";

import {VueRecaptcha} from "vue-recaptcha";
import LetterCube from "@/components/LetterCube";

/**
 * Login component
 */
export default {
  page: {
    title: "Đăng nhập",
    meta: [{ name: "description", content: appConfig.description }],
  },
  // eslint-disable-next-line vue/no-unused-components
  components: {VueRecaptcha, LetterCube},
  validations: {
    model:{
      userName: {
        required,
      },
      password: {
        required
      }
    }
  },
  data() {
    return {
      capcha: null,
      email: "admin@themesbrand.com",
      password: "123456",
      submitted: false,
      authError: null,
      tryingToLogIn: false,
      isAuthError: false,
      modelAuth:{
        isAuthError: false,
        message: null
      },
      model:{
        userName: "",
        password: ""
      },
    };
  },
  methods: {
    submit(response){
      this.capcha = response;
    },
    async Login(e) {
      e.preventDefault();
      if(!this.capcha && process.env.VUE_APP_ENV != 'development'){
        this.modelAuth.isAuthError = true;
        this.modelAuth.message = "Xác nhận đã nhập mã captcha";
        return;
      }
      this.submitted = true;
      this.$v.$touch();
      if (this.$v.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        },{
          default: this.$createElement(LetterCube),
        });
        // await setTimeout(function () {
        //  }, 5000);
        await this.$store.dispatch("authStore/login", this.model).then(async (res) => {
          if (res.resultCode === 'SUCCESS') {
            await new Promise(resolve => setTimeout(resolve, 1000));
            localStorage.setItem('auth-user', JSON.stringify(res.data));
            localStorage.setItem("user-token", JSON.stringify(res.data.token));

            if (res.data.user) {
              if (res.data.user.menuItems) {
                localStorage.setItem("menuItems", JSON.stringify(res.data.user.menuItems));
              }
            }
            Vue.prototype.$auth_token = res.data.token;
            this.showModal = false;
            this.model = {};
            this.modelAuth.isAuthError = false;
            window.location.href = '/bang-dieu-khien'
          } else {
            if (res.code == 400) {
              this.modelAuth.isAuthError = true;
              this.modelAuth.message = "Lỗi! Hãy kiểm tra kết nối mạng!";
            } else {
              this.modelAuth.isAuthError = true;
              this.modelAuth.message = res.resultString;
            }
            loader.hide();
          }

        })
            .finally(() => {
              loader.hide();
            });
      }
      this.submitted = false;
    },
  }
};
</script>

<template>
  <div class="account-pages">
    <div class="row justify-content-center align-content-center">
      <div class="col-12 col-md-6 col-lg-8 cs-dnone">
        <div class="cs-login-left">
          <img src="@/assets/images/img_SKHCN_DT.png" width="100%" alt="SKH&CN-DT">
        </div>
      </div>
      <div class="col-12 col-md-6 col-lg-4" ref="formContainer">
        <div class="cs-login-right">
          <div class="py-0 py-md-2 py-lg-5 px-0 py-md-3 px-lg-5">
            <div class="text-center">
              <div class="text-center login-img">
                <a href="/" class="">
                  <img
                      src="@/assets/images/DTHU.png"
                      alt="logo"
                  />
                </a>
              </div>
              <h5
                  class="login-title"
              > Hệ thống thông tin <br /> Khoa học và Công nghệ </h5>
              <p class="text-primary mb-0" style="color: #111961">Sở Khoa học và Công nghệ tỉnh Đồng Tháp</p>
            </div>
            <div class="p-2">
              <b-alert
                  v-model="modelAuth.isAuthError"
                  variant="danger"
                  class="mt-4"
                  dismissible
              >{{ modelAuth.message }}</b-alert
              >
              <b-form
                  @submit.prevent="Login"
                  class="form-horizontal mt-4"
              >
                <b-form-group
                    id="input-group-1"
                    label="Tài khoản"
                    label-for="input-1"
                    class="mb-3"
                    label-class="form-label"
                >
                  <b-form-input
                      id="input-1"
                      v-model="model.userName"
                      type="text"
                      placeholder="Nhập tài khoản"
                      :class="{ 'is-invalid': submitted && $v.model.userName.$error }"
                  ></b-form-input>
                  <div
                      v-if="submitted && $v.model.userName.$error"
                      class="invalid-feedback"
                  >
                    <span v-if="!$v.model.userName.required">Tài khoản không được trống.</span>
                  </div>
                </b-form-group>

                <b-form-group
                    id="input-group-2"
                    label="Mật khẩu"
                    label-for="input-2"
                    class="mb-3"
                    label-class="form-label"
                >
                  <b-form-input
                      id="input-2"
                      v-model="model.password"
                      type="password"
                      placeholder="Nhập mật khẩu"
                      :class="{ 'is-invalid': submitted && $v.model.password.$error }"
                  ></b-form-input>
                  <div
                      v-if="submitted && !$v.model.password.required"
                      class="invalid-feedback"
                  >
                    Mật khẩu không được trống.
                  </div>
                </b-form-group>

                <div class="form-group row">
                  <div class="col-sm-6">
                    <div class="form-check">
                      <input
                          type="checkbox"
                          class="form-check-input"
                          id="customControlInline"
                      />
                      <label
                          class="form-check-label"
                          for="customControlInline"
                      >Ghi nhớ tài khoản</label
                      >
                    </div>
                  </div>
                  <div class="my-2 d-flex justify-content-center">
                    <vue-recaptcha  @verify="submit" sitekey="6LcnfWwhAAAAAIW-zBI9Cg5s5LNcYepeevhybx9o"></vue-recaptcha>
                  </div>
                  <div class="col-sm-12 text-end">
                    <b-button
                        type="submit"
                        variant="primary"
                        class="w-100 text-uppercase fw-bold py-2"
                        style="background: #111961"
                    > Đăng nhập</b-button
                    >
                  </div>
                </div>
              </b-form>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" >
.account-pages{
  background-color: #111961;
  //background-image: url("~@/assets/images/img_SKHCN_DT.png");
  //background-repeat: no-repeat;
  //background-size: cover;
  height: 100vh;
  overflow: hidden;
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
}
.login-img img{
  padding: 10px;
  height: 150px;
}
.cs-login-left{
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
}
.cs-login-right{
  background-color: #fff;
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
}

.login-title{
  font-size: 28px;
  font-weight: bold;
  text-transform: uppercase;
  background: linear-gradient(90deg, #111961, #01b0c9);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

@media only screen and (max-width: 1024px){
  .login-title{
    font-size: 20px;
  }
  .login-img img{
    height: 100px;
  }
}
@media only screen and (max-width: 425px){
  .cs-dnone{
    display: none;
  }
}
</style>
