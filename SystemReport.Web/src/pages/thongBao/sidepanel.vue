<script>

export default {
  data() {
    return {
      showModal: false,
      listNotify: [],
    };
  },
  methods:{
    getListNotify() {
      this.$store.dispatch("notifyStore/getListNotify").then((res) => {
        this.listNotify = res.data || [];
      });
    },
  },
  created() {
    this.getListNotify();
  }
};
</script>
<template>
  <div class="email-leftbar card">
    <div class="mail-list mt-4">
      <router-link to="/thong-bao" class="active">
        <i class="mdi mdi-email-outline me-2"></i> Tất cả
        <span class="ms-1 float-end">&lpar;{{ listNotify.number }}&rpar;</span>
      </router-link>
      <router-link to="/thong-bao">
        <i class="mdi mdi-star-outline me-2"></i>Chưa xem
      </router-link>
      <router-link to="/thong-bao">
        <i class="mdi mdi-diamond-stone me-2"></i>Đã xem
      </router-link>
    </div>
  </div>
</template>
