<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";

/**
 * Form Repeater component
 */
export default {
  page: {
    title: "Form Repeater",
    meta: [{ name: "description", content: appConfig.description }],
  },
  components: { Layout, PageHeader },
  data() {
    return {
      title: "Form Repeater",
      items: [
        {
          text: "Veltrix",
          href: "/",
        },
        {
          text: "Forms",
          href: "/",
        },
        {
          text: "Form Repeater",
          active: true,
        },
      ],
      fields: [{ id: 1 }],
      phoneData: [{ id: 1 }],
    };
  },
  methods: {
    /**
     * Push the row in form
     */
    AddformData() {
      this.fields.push({ name: "", email: "", subject: "", message: "" });
    },
    /**
     * Delete the row
     */
    deleteRow(index) {
      if (confirm("Are you sure you want to delete this element?"))
        this.fields.splice(index, 1);
    },
    /**
     * Add the phone number in form
     */
    AddPhoneNo() {
      this.phoneData.push({ phone: "" });
    },
    /**
     * Delete the row from form
     */
    deletePhone(index) {
      if (confirm("Are you sure you want to delete this element?")) {
        this.phoneData.splice(index, 1);
      }
    },
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items" />
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title">Example</h4>
            <form class="repeater" enctype="multipart/form-data">
              <div data-repeater-list="group-a">
                <div
                  v-for="(field, index) in fields"
                  :key="field.id"
                  class="row"
                >
                  <div class="mb-3 col-lg-2">
                    <label class="form-label" for="name">Name</label>
                    <input
                      type="text"
                      v-model="field.name"
                      id="name"
                      name="untyped-input"
                      class="form-control"
                    />
                  </div>

                  <div class="mb-3 col-lg-2">
                    <label class="form-label" for="email">Email</label>
                    <input
                      type="email"
                      v-model="field.email"
                      id="email"
                      class="form-control"
                    />
                  </div>

                  <div class="mb-3 col-lg-2">
                    <label class="form-label" for="subject">Subject</label>
                    <input
                      type="text"
                      v-model="field.subject"
                      id="subject"
                      class="form-control"
                    />
                  </div>

                  <div class="mb-3 col-lg-2">
                    <label class="form-label" for="resume">Resume</label>
                    <input type="file" class="form-control-file" id="resume" />
                  </div>

                  <div class="mb-3 col-lg-2">
                    <label class="form-label" for="message">Message</label>
                    <textarea
                      id="message"
                      v-model="field.message"
                      class="form-control"
                    ></textarea>
                  </div>

                  <div class="col-lg-2 align-self-center">
                    <div class="d-grid">
                      <input
                        type="button"
                        class="btn btn-primary"
                        value="Delete"
                        @click="deleteRow(index)"
                      />
                    </div>
                  </div>
                </div>
              </div>
              <input
                type="button"
                class="btn btn-success mo-mt-2"
                value="Add"
                @click="AddformData"
              />
            </form>
          </div>
        </div>
      </div>
    </div>
    <!-- end row -->

    <div class="row">
      <div class="col-lg-12">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title">Nested</h4>
            <form class="outer-repeater">
              <div data-repeater-list="outer-group" class="outer">
                <div data-repeater-item class="outer">
                  <div class="mb-3">
                    <label class="form-label" for="formname">Name :</label>
                    <input
                      type="text"
                      class="form-control"
                      id="formname"
                      placeholder="Enter your Name..."
                    />
                  </div>

                  <div class="mb-3">
                    <label class="form-label" for="formemail">Email :</label>
                    <input
                      type="email"
                      class="form-control"
                      id="formemail"
                      placeholder="Enter your Email..."
                    />
                  </div>

                  <div class="inner-repeater mb-4">
                    <div
                      data-repeater-list="inner-group"
                      class="inner form-group"
                    >
                      <label class="form-label">Phone no :</label>
                      <div
                        v-for="(data, index) in phoneData"
                        :key="data.id"
                        class="inner mb-3 row"
                      >
                        <div class="col-md-10 col-8">
                          <input
                            v-model="data.phone"
                            type="text"
                            class="inner form-control"
                            placeholder="Enter your phone no..."
                          />
                        </div>
                        <div class="col-md-2 col-4">
                          <div class="d-grid">
                          <input
                            type="button"
                            class="btn btn-primary btn-block inner"
                            value="Delete"
                            @click="deletePhone(index)"
                          />
                          </div>
                        </div>
                      </div>
                    </div>
                    <input
                      data-repeater-create
                      type="button"
                      class="btn btn-success inner"
                      value="Add Number"
                      @click="AddPhoneNo"
                    />
                  </div>

                  <div class="mb-3">
                    <label class="form-label mb-3 d-flex">Gender :</label>
                    <div class="form-check form-check-inline">
                      <input
                        type="radio"
                        id="customRadioInline1"
                        name="outer-group[0][customRadioInline1]"
                        class="form-check-input"
                      />
                      <label class="form-check-label" for="customRadioInline1"
                        >Male</label
                      >
                    </div>
                    <div class="form-check form-check-inline">
                      <input
                        type="radio"
                        id="customRadioInline2"
                        name="outer-group[0][customRadioInline1]"
                        class="form-check-input"
                      />
                      <label class="form-check-label" for="customRadioInline2"
                        >Female</label
                      >
                    </div>
                  </div>

                  <div class="mb-3">
                    <label for="formmessage" class="form-label"
                      >Message :</label
                    >
                    <textarea
                      id="formmessage"
                      class="form-control"
                      rows="3"
                    ></textarea>
                  </div>
                  <button type="submit" class="btn btn-primary">Submit</button>
                </div>
              </div>
            </form>
          </div>
        </div>
      </div>
      <!-- end col -->
    </div>
    <!-- end row -->
  </Layout>
</template>
