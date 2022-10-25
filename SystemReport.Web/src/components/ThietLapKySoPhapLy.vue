<template>
  <div class="row">
    <div class="col-md-13">
      <PdfEditor  :fileInfo="data"/>
    </div>
  </div>
</template>

<script>
import PdfEditor from "@/components/pdfEditor/PdfEditor";
import appConfig from "@/app.config.json";

export default {
  name: "thietLapKySo",
  page: {
    title: "Ký số",
    meta: [{name: "description", content: appConfig.description}],
  },
  components:{
    PdfEditor,
  },
  props:{
    data: {required: true}
  },
  computed:{
  },
  data() {
    return {
      apiUrl: process.env.VUE_APP_API_URL,
      url: `${process.env.VUE_APP_API_URL}files/upload`,
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}files/upload`,
        thumbnailWidth: 300,
        thumbnailHeight: 160,
        maxFiles: 1,
        maxFilesize: 30,
        headers: {"My-Awesome-Header": "header value"},
        addRemoveLinks: true,
        acceptedFiles: ".pdf",
      },
      files: null,
    };
  },
  methods:{
    removeThisFile(file, error, xhr) {
      this.files = null;
    },
    addThisFile(file, response) {
      if (this.files == null || this.files.length <= 0) {
        this.files = [] ;
      }
      let fileSuccess = response.data;

      this.files.push(
          {
            fileId: fileSuccess.id,
            fileName: fileSuccess.fileName,
            ext: fileSuccess.ext,
            size: fileSuccess.size,
          }
      );
    },

  }
}
</script>

<style scoped>

</style>