<script>
/**
 * Page-header component
 */
export default {
  components: {},
  props: {
    title: {
      type: String,
      default: "",
    },
    items: {
      type: Array,
      default: () => {
        return [];
      },
    },
  },
};
</script>

<template>
  <!-- start page title -->
  <div class="row align-items-center">
    <div class="col-sm-12">
      <div class="page-title-box" style="display: flex; justify-content: space-between; padding-top: 10px; padding-bottom: 10px">
        <h4 class="font-size-18">{{ title }}</h4>
        <div class="page-title-right">
          <b-breadcrumb :items="items" class="m-0"></b-breadcrumb>
        </div>
      </div>
    </div>
<!--    <div class="col-sm-6">-->
<!--      <div class="float-end d-none d-md-block">-->
<!--        <b-dropdown right variant="primary" menu-class="dropdown-menu-end">-->
<!--          <template v-slot:button-content>-->
<!--            <i class="mdi mdi-cog me-2"></i> Settings-->
<!--          </template>-->
<!--          <b-dropdown-item>Action</b-dropdown-item>-->
<!--          <b-dropdown-item>Another action</b-dropdown-item>-->
<!--          <b-dropdown-item>Something else here</b-dropdown-item>-->
<!--          <b-dropdown-divider></b-dropdown-divider>-->
<!--          <b-dropdown-item>Separated link</b-dropdown-item>-->
<!--        </b-dropdown>-->
<!--      </div>-->
<!--    </div>-->
  </div>
  <!-- end page title -->
</template>
