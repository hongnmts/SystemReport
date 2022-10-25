<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";

/**
 * Images component
 */
export default {
  page: {
    title: "Images",
    meta: [{ name: "description", content: appConfig.description }]
  },
  components: { Layout, PageHeader },
  data() {
    return {
      title: "Images",
      items: [
        {
          text: "Veltrix",
          href: "/"
        },
        {
          text: "UI Elements",
          href: "/"
        },
        {
          text: "Images",
          active: true
        }
      ]
    };
  }
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items" />
    <div class="row">
      <div class="col-lg-6">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title">Image Thumbnails</h4>
            <p class="card-title-desc">
              In addition to our border-radius utilities, you can use
              <code
                class="highlighter-rouge"
              >thumbnail</code> to give an image a
              rounded 1px border appearance.
            </p>

            <div class>
              <b-img
                :src="require('@/assets/images/small/img-3.jpg')"
                width="200"
                thumbnail
                alt="200x200"
              ></b-img>
            </div>
          </div>
        </div>

        <div class="card">
          <div class="card-body">
            <h4 class="card-title">Image Rounded & Circle</h4>
            <p class="card-title-desc">
              Use attribute
              <code>rounded</code> and
              <code>rounded='circle'</code>.
            </p>

            <div class>
              <b-img
                :src="require('@/assets/images/small/img-4.jpg')"
                rounded
                class="me-2 mo-mb-2"
                width="200"
                alt="200x200"
              ></b-img>

              <b-img
                :src="require('@/assets/images/users/user-4.jpg')"
                rounded="circle"
                alt="200x200"
              ></b-img>
            </div>
          </div>
        </div>
      </div>

      <div class="col-lg-6">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title">Responsive images</h4>
            <p class="card-title-desc">
              Images in Bootstrap are made responsive
              with
              <code
                class="highlighter-rouge"
              >fluid</code>.
              <code class="highlighter-rouge">max-width: 100%;</code> and
              <code class="highlighter-rouge">height: auto;</code> are applied to
              the image so that it scales with the parent element.
            </p>

            <div class>
              <b-img :src="require('@/assets/images/small/img-2.jpg')" fluid alt="Responsive image"></b-img>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!--end row-->
  </Layout>
</template>