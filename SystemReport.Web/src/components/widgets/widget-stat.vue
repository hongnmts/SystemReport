<script>
/**
 * Widget-stat component -- specify the widget title, image, value, subtext and color of subtext.
 */
export default {
  props: {
    title: {
      type: String,
      default: ""
    },
    image: {
      type: String,
      default: ""
    },
    value: {
      type: String,
      default: ""
    },
    subText: {
      type: String,
      default: ""
    },
    color: {
      type: String,
      default: ""
    },
    path: {
      type: String,
      default: "/"
    },
    className: {
      type: String,
      default: ""
    },
  },
  methods:{
    handleChangePage(path){
      this.$router.push(path);
    }
  }
};
</script>

<template>
  <div
      @click="handleChangePage(path)"
      class="card mini-stat"
      :class="`${className}`"
      style="cursor: pointer;"
  >
    <div class="card-body">
      <div class="mb-4">
<!--        <div class="float-start mini-stat-img me-4">-->
<!--          <img :src="`${image}`" alt />-->
<!--        </div>-->
        <h5 class="font-size-16 text-uppercase mt-0 text-white">{{title}}</h5>
<!--        <h4 class="fw-medium font-size-24">-->
<!--          {{value}}-->
<!--        </h4>-->
        <div :class="`mini-stat-label bg-mini-start bg-${color}`">
          <h4 class="fw-medium font-size-24">
            {{value}}
          </h4>
        </div>
      </div>
    </div>
  </div>
</template>
