<template>
  <!--    Xem bảng biểu-->
  <b-modal
      v-model="showModalXemBangBieu"
      title="Thông tin bảng biểu"
      title-class="text-black font-18"
      body-class="p-3"
      hide-footer
      centered
      no-close-on-backdrop
      size="xl"
  >
    <template #modal-header="{  }">
      <!-- Emulate built in modal header close button action -->
      <h5 style="min-width: 200px">Lịch sử bảng biểu</h5>
      <div style="width: 100%; display: flex; justify-content: flex-end" class="text-end">

        <b-button variant="light" class="ms-1" size="sm" style="width: 80px"
                  @click="handleClose">
          Đóng
        </b-button>
      </div>
    </template>
    <b-button
        variant="info"
        type="button"
        class="btn w-md btn-primary"
        @click="exportTableToExcel('dynamic-table2', '')"
        size="sm"
        style="margin-bottom: 10px"
    >
      <i class="mdi mdi-plus me-1"></i> Export
    </b-button>
    <div id="dynamic-table2">
      <br />
      <table class="dynamic-table"  style="width: 100%;  border-collapse: collapse;">
        <thead v-if="headers">
        <tr v-for="(row, indexRow) in headers" :key="indexRow">
          <td v-for="(data, indexData) in row.tHeaderVms" :key="indexData" :rowspan="data.rowSpan"
              :colspan="data.colSpan"
              :style="{ 'width': data.width == 0?'auto':  data.width + 'px',
                     'text-align': data.fontHorizontalAlign?data.fontHorizontalAlign.id: '',
                     'font-weight': data.fontWeight?data.fontWeight.id: '',
                     'font-style': data.fontStyle?data.fontStyle.id: '',
                     }"
              style=" border: 1px solid #a1a0a0"
          >
            {{ data.tenThuocTinh }}
            <template v-if="data.ghiChu">
                               <span v-if="checkIsEmpty(data.ghiChu.kyHieu)">
                                 <template v-if="checkIsStar(data.ghiChu.kyHieu)">
                                        ({{ data.ghiChu.kyHieu }})
                                 </template>
                                       <template v-else>
                                       <sup>{{ data.ghiChu.kyHieu }}</sup>
                                 </template>

                  </span>
            </template>

            <template v-if="data.donViTinh">

              <div style="font-weight: normal; font-style: italic">
                ({{ data.donViTinh.ten }})
              </div>
            </template>
          </td>
        </tr>
        </thead>
        <tbody>
        <tr v-for="(value, index) in renderMainRowValue" :key="index" style="min-height: 50px; height: 50px">
          <td v-for="(item, index1) in value.rowValues" :key="index1"
              :style="{ 'width': item.width == 0?'auto':  item.width + 'px',
                     'text-align': item.fontHorizontalAlign?item.fontHorizontalAlign.id: '',
                     'font-weight': item.fontWeight?item.fontWeight.id: '',
                     'font-style': item.fontStyle?item.fontStyle.id: '',
                     }"
              style="padding: 0px 5px; border: 1px solid #a1a0a0"
          >
            {{ item.value }}
            <template v-if="item.ghiChu">
                               <span v-if="checkIsEmpty(item.ghiChu.kyHieu)">
                           <sup>{{ item.ghiChu.kyHieu }}</sup>

                  </span>
            </template>
          </td>
        </tr>
        </tbody>
      </table>
    </div>

  </b-modal>
</template>

<script>
export default {
  props:['historyId'],
  data(){
    return{
      showModalXemBangBieu:false,
      headers: [],
      renderMainRowValue: []
    }
  },
  watch:{
    historyId:{
      deep: true,
          handler(val){
        if(val != "" && val != null){
          this.renderHeader();
        }
      }
    },
  },
  methods: {
    handleClose(){
      this.$emit("update:historyId", null);
      this.showModalXemBangBieu = false;
    },
    async renderHeader(id) {
      try {
        await this.$store.dispatch("historyStore/getBangBieuDetail",this.historyId).then(resp => {
          if (resp.resultCode == "SUCCESS") {
            let items = resp.data
            this.headers = items.headers;
            this.renderMainRowValue = items.body;
            this.showModalXemBangBieu = true;
          }
        });
      } finally {
        console.log("");
      }
    },
    checkIsStar(value) {
      return value && value.length > 0 && value.includes('*');
    },
    checkIsEmpty(value) {
      return value && value.length > 0;
    },
    exportTableToExcel(tableId, filename) {
      if(filename == ''){
        filename = Date.now();
      }
      let dataType = 'application/vnd.ms-excel';
      let extension = '.xls';

      let base64 = function(s) {
        return window.btoa(unescape(encodeURIComponent(s)))
      };

      let template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>';
      let render = function(template, content) {
        return template.replace(/{(\w+)}/g, function(m, p) { return content[p]; });
      };

      let tableElement = document.getElementById(tableId);

      let tableExcel = render(template, {
        worksheet: filename,
        table: tableElement.innerHTML
      });

      filename = filename + extension;

      if (navigator.msSaveOrOpenBlob)
      {
        let blob = new Blob(
            [ '\ufeff', tableExcel ],
            { type: dataType }
        );

        navigator.msSaveOrOpenBlob(blob, filename);
      } else {
        let downloadLink = document.createElement("a");

        document.body.appendChild(downloadLink);

        downloadLink.href = 'data:' + dataType + ';base64,' + base64(tableExcel);

        downloadLink.download = filename;

        downloadLink.click();
      }
    },
  }
}
</script>

<style scoped>

</style>