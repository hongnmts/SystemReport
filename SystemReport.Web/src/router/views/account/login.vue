<script>
import { required, email } from "vuelidate/lib/validators";
import { mapState } from "vuex";

import {
  authMethods,
  authFackMethods,
  notificationMethods,
} from "@/state/helpers";
import appConfig from "@/app.config";

/**
 * Login component
 */
export default {
  page: {
    title: "Login",
    meta: [{ name: "description", content: appConfig.description }],
  },
  components: {},
  validations: {
    email: {
      required,
      email,
    },
    password: {
      required,
    },
  },
  data() {
    return {
      email: "admin@themesbrand.com",
      password: "123456",
      submitted: false,
      authError: null,
      tryingToLogIn: false,
      isAuthError: false,
    };
  },
  computed: {
    ...mapState("authfack", ["status"]),
    notification() {
      return this.$store ? this.$store.state.notification : null;
    },
  },
  methods: {
    ...authMethods,
    ...authFackMethods,
    ...notificationMethods,
    // Try to log the user in with the username
    // and password they provided.
    tryToLogIn() {
      this.submitted = true;
      // stop here if form is invalid
      this.$v.$touch();
      if (this.$v.$invalid) {
        return;
      } else {
        if (process.env.VUE_APP_DEFAULT_AUTH === "firebase") {
          this.tryingToLogIn = true;
          // Reset the authError if it existed.
          this.authError = null;
          return (
            this.logIn({
              email: this.email,
              password: this.password,
            })
              // eslint-disable-next-line no-unused-vars
              .then((token) => {
                this.tryingToLogIn = false;
                this.isAuthError = false;
                // Redirect to the originally requested page, or to the home page
                this.$router.push(
                  this.$route.query.redirectFrom || { name: "home" }
                );
              })
              .catch((error) => {
                this.tryingToLogIn = false;
                this.authError = error ? error : "";
                this.isAuthError = true;
              })
          );
        } else if (process.env.VUE_APP_DEFAULT_AUTH === "fakebackend") {
          const { email, password } = this;
          if (email && password) {
            this.login({
              email,
              password,
            });
          }
        }
      }
    },
  },
};
</script>

<template>
  <div class="account-pages my-5 pt-5">
    <div class="container">
      <div class="row justify-content-center">
        <div class="col-md-8 col-lg-6 col-xl-4">
          <div class="card overflow-hidden">
            <div class="bg-primary">
              <div class="text-primary text-center p-4">
                <h5 class="text-white font-size-20">Welcome Back !</h5>
                <p class="text-white-50">Sign in to continue to Veltrix.</p>
                <a href="/" class="logo logo-admin">
                  <img
                    src="@/assets/images/logo-sm.png"
                    height="24"
                    alt="logo"
                  />
                </a>
              </div>
            </div>
            <div class="card-body p-4">
              <div class="p-3">
                <b-alert
                  v-model="isAuthError"
                  variant="danger"
                  class="mt-3"
                  dismissible
                  >{{ authError }}</b-alert
                >
                <div
                  v-if="notification.message"
                  :class="'alert ' + notification.type"
                >
                  {{ notification.message }}
                </div>

                <b-form
                  @submit.prevent="tryToLogIn"
                  class="form-horizontal mt-4"
                >
                  <b-form-group
                    id="input-group-1"
                    label="Email"
                    label-for="input-1"
                    class="mb-3"
                    label-class="form-label"
                  >
                    <b-form-input
                      id="input-1"
                      :class="{ 'is-invalid': submitted && $v.email.$error }"
                      v-model="email"
                      type="email"
                      placeholder="Enter email"
                    ></b-form-input>
                    <div
                      v-if="submitted && $v.email.$error"
                      class="invalid-feedback"
                    >
                      <span v-if="!$v.email.required">Email is required.</span>
                      <span v-if="!$v.email.email"
                        >Please enter valid email.</span
                      >
                    </div>
                  </b-form-group>

                  <b-form-group
                    id="input-group-2"
                    label="Password"
                    label-for="input-2"
                    class="mb-3"
                    label-class="form-label"
                  >
                    <b-form-input
                      id="input-2"
                      v-model="password"
                      type="password"
                      placeholder="Enter password"
                      :class="{ 'is-invalid': submitted && $v.password.$error }"
                    ></b-form-input>
                    <div
                      v-if="submitted && !$v.password.required"
                      class="invalid-feedback"
                    >
                      Password is required.
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
                          >Remember me</label
                        >
                      </div>
                    </div>
                    <div class="col-sm-6 text-end">
                      <b-button type="submit" variant="primary" class="w-md"
                        >Log In</b-button
                      >
                    </div>
                  </div>

                  <div class="mt-2 mb-0 row">
                    <div class="col-12 mt-4">
                      <router-link to="/forgot-password">
                        <i class="mdi mdi-lock"></i> Forgot your password?
                      </router-link>
                    </div>
                  </div>
                </b-form>
              </div>
            </div>
            <!-- end card-body -->
          </div>
          <!-- end card -->
          <div class="mt-5 text-center">
            <p>
              Don't have an account ?
              <router-link to="/register" class="fw-medium text-primary"
                >Signup now</router-link
              >
            </p>
            <p class="mb-0">
              ©
              {{ new Date().getFullYear() }} Veltrix. Crafted with
              <i class="mdi mdi-heart text-danger"></i> by Themesbrand
            </p>
          </div>
        </div>
        <!-- end col -->
      </div>
      <!-- end row -->
    </div>
  </div>
</template>

<style lang="scss" module></style>
