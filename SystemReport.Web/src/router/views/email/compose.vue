<script>
import CKEditor from "@ckeditor/ckeditor5-vue";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";

import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";

/**
 * Email Compose component
 */
export default {
  page: {
    title: "Email Compose",
    meta: [{ name: "description", content: appConfig.description }],
  },
  components: { ckeditor: CKEditor.component, Layout, PageHeader },
  data() {
    return {
      title: "Email Compose",
      items: [
        {
          text: "Veltrix",
          href: "/",
        },
        {
          text: "Email",
          href: "/",
        },
        {
          text: "Email Compose",
          active: true,
        },
      ],
      editor: ClassicEditor,
      editorData: "<p>Content of the editor.</p>",
    };
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items" />
    <div class="row">
      <div class="col-12">
        <!-- Left sidebar -->
        <div class="email-leftbar card">
          <router-link
            to="/email/compose"
            class="btn btn-danger rounded btn-custom btn-block"
            >Compose</router-link
          >

          <div class="mail-list mt-4">
            <a href="javascript: void(0);" class="active">
              Inbox
              <span class="ms-1">(18)</span>
            </a>
            <a href="javascript: void(0);">Starred</a>
            <a href="javascript: void(0);">Important</a>
            <a href="javascript: void(0);">Draft</a>
            <a href="javascript: void(0);">Sent Mail</a>
            <a href="javascript: void(0);">Trash</a>
          </div>

          <h5 class="mt-4">Labels</h5>

          <div class="mail-list mt-4">
            <a href="javascript: void(0);">
              <span
                class="mdi mdi-arrow-right-drop-circle text-info float-end"
              ></span
              >Theme Support
            </a>
            <a href="javascript: void(0);">
              <span
                class="mdi mdi-arrow-right-drop-circle text-warning float-end"
              ></span
              >Freelance
            </a>
            <a href="javascript: void(0);">
              <span
                class="mdi mdi-arrow-right-drop-circle text-primary float-end"
              ></span
              >Social
            </a>
            <a href="javascript: void(0);">
              <span
                class="mdi mdi-arrow-right-drop-circle text-danger float-end"
              ></span
              >Friends
            </a>
            <a href="javascript: void(0);">
              <span
                class="mdi mdi-arrow-right-drop-circle text-success float-end"
              ></span
              >Family
            </a>
          </div>

          <h5 class="mt-4">Chat</h5>

          <div class="mt-4">
            <a href="#" class="d-flex">
              <div class="flex-shrink-0 me-3">
                <img
                  class="rounded-circle"
                  src="@/assets/images/users/user-2.jpg"
                  alt="Generic placeholder image"
                  height="36"
                />
              </div>
              <div class="flex-grow-1 chat-user-box">
                <p class="user-title m-0">Scott Median</p>
                <p class="text-muted">Hello</p>
              </div>
            </a>

            <a href="#" class="d-flex">
              <div class="flex-shrink-0 me-3">
                <img
                  class="rounded-circle"
                  src="@/assets/images/users/user-3.jpg"
                  alt="Generic placeholder image"
                  height="36"
                />
              </div>
              <div class="flex-grow-1 chat-user-box">
                <p class="user-title m-0">Julian Rosa</p>
                <p class="text-muted">What about our next..</p>
              </div>
            </a>

            <a href="#" class="d-flex">
              <div class="flex-shrink-0 me-3">
                <img
                  class="rounded-circle"
                  src="@/assets/images/users/user-4.jpg"
                  alt="Generic placeholder image"
                  height="36"
                />
              </div>
              <div class="flex-grow-1 chat-user-box">
                <p class="user-title m-0">David Medina</p>
                <p class="text-muted">Yeah everything is fine</p>
              </div>
            </a>

            <a href="#" class="d-flex">
              <div class="flex-shrink-0 me-3">
                <img
                  class="rounded-circle"
                  src="@/assets/images/users/user-6.jpg"
                  alt="Generic placeholder image"
                  height="36"
                />
              </div>
              <div class="flex-grow-1 chat-user-box">
                <p class="user-title m-0">Jay Baker</p>
                <p class="text-muted">Wow that's great</p>
              </div>
            </a>
          </div>
        </div>
        <!-- End Left sidebar -->

        <!-- Right Sidebar -->
        <div class="email-rightbar mb-3">
          <div class="card">
            <div class="card-body">
              <form>
                <div class="mb-3">
                  <input type="email" class="form-control" placeholder="To" />
                </div>

                <div class="mb-3">
                  <input
                    type="text"
                    class="form-control"
                    placeholder="Subject"
                  />
                </div>
                <div class="mb-3">
                  <!-- Editor -->
                  <ckeditor v-model="editorData" :editor="editor"></ckeditor>
                </div>

                <div class="btn-toolbar form-group mb-0">
                  <div class>
                    <button type="button" class="btn btn-success me-1">
                      <i class="far fa-save"></i>
                    </button>
                    <button type="button" class="btn btn-success me-1">
                      <i class="far fa-trash-alt"></i>
                    </button>
                    <button class="btn btn-primary">
                      <span>Send</span>
                      <i class="fab fa-telegram-plane ms-1"></i>
                    </button>
                  </div>
                </div>
              </form>
            </div>
          </div>
        </div>
        <!-- end Right Sidebar -->
      </div>
    </div>
    <!-- End row -->
  </Layout>
</template>
