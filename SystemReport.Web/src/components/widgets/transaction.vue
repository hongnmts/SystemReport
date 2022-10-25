<script>
/**
 * Transaction component
 */
export default {
  props: {
    transactions: {
      type: Array,
      default: function() {
        return [];
      }
    }
  }
};
</script>

<template>
  <div class="table-responsive mb-0">
    <table class="table table-hover table-centered table-nowrap">
      <thead>
        <tr>
          <th scope="col">(#)</th>
          <th scope="col"> Tên văn bản</th>
          <th scope="col"> Hạn xử lý</th>
          <th scope="col" colspan="2"> Trạng thái</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="data in transactions" :key="data.id">
          <th scope="row">{{data.id}}</th>
          <td>
            <div>
              {{data.name}}
            </div>
          </td>
          <td>{{data.date}}</td>
          <td>
            <span
              class="badge"
              :class="{
              'bg-danger': data.status === 'Từ chối',
              'bg-warning': data.status === 'Đang xử lý',
              'bg-success': data.status === 'Xử lý xong'
          }"
            >{{data.status}}</span>
          </td>
          <td>
            <div>
              <a href="javascript:void(0)" class="btn btn-primary btn-sm"> Xử lý</a>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>