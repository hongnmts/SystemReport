<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import {required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {notifyModel} from "@/models/notifyModel";
import {pagingModel} from "@/models/pagingModel";
import Multiselect from "vue-multiselect";
import {roleModel} from "@/models/roleModel";
import {moduleModel} from "@/models/moduleModel";

export default {
  page: {
    title: "Thêm quyền vào vai trò",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader},
  data() {
    return {
      title: "Thiết lập quyền 1",
      items: [
        {
          text: "Vai trò",
          active: true,
        },
        {
          text: "Thêm quyền",
          active: true,
        }
      ],
      submitted: false,
      data: [],
      model: {},
      listModule: [],
      listVaiTro: [],
      listPermission: [],
      role: {
        id: null,
        permissions: []
      }
    };
  },
  validations: {},
  async created() {
    await this.GetListModule();
    await this.GetListVaiTro();
    this.role = Object.assign({}, this.listVaiTro[0]);
    this.InitPermission();
  },
  methods: {
    async GetListModule() {
      await this.$store.dispatch("moduleStore/getAll").then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.listModule = moduleModel.toListModel(res.data);
        }
      })
    },
    async GetListVaiTro() {
      await this.$store.dispatch("roleStore/getAll").then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.listVaiTro = res.data;
        }
      })
    },
    InitPermission() {
      this.listModule.forEach(item => {
        if (item.permissions && item.permissions.length > 0) {
          item.permissions.forEach(subItem => {
            let index = this.role?.permissions?.findIndex(x => x.id == subItem.id);
            if (index != undefined && index != -1) {
              subItem.checked = true;
            } else {
              subItem.checked = false
            }
          })
        }
        return item;
      })
    },
    handleAddPermissionsToRole(permission) {
      let check = this.role.permissions?.findIndex(x => x.id == permission.id);
      if (check == undefined || check == -1) {
        if (!this.role.permissions) {
          this.role.permissions = [];
        }
        this.role.permissions.push({id: permission.id, name: permission.name, action: permission.action});
      } else {
        this.role.permissions = this.role.permissions?.filter(x => {
          if (x.id != permission.id)
            return x;
        });
      }
    },
    handleRoleChange() {
      this.InitPermission();
    },
    async handleSubmitRole() {
      await this.$store.dispatch("roleStore/update", this.role).then((res) => {
        if (res.resultCode === 'SUCCESS') {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      })
    }
  },
  computed: {},
  watch: {
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="mb-3">
              <label class="col-md-2 col-form-label">Vai trò</label>
              <div class="row">
                <div class="col-md-10">
                  <select class="form-control" @change="handleRoleChange" v-model="role">
                    <template v-if="listVaiTro && listVaiTro.length > 0">
                      <option v-for="(item, index) in listVaiTro" :key="index" :value="item"
                              :selected="item.id === role.id">{{ item.code + " - " + item.ten }}
                      </option>
                    </template>
                    <template v-else>
                      <option>Không có dữ liệu</option>
                    </template>
                  </select>
                </div>
                <div class="col-md-2">
                  <button class="btn btn-success" style="width: 100%" @click="handleSubmitRole">Lưu</button>
                </div>
              </div>
            </div>
            <div v-for="(item , index) in listModule"
                 :key="item.id"
            >
              <div style="font-size: 14px; margin-bottom: 8px; font-weight: bold"> {{ ++index }}. {{ item.name }}</div>
              <div style="display: flex; column-gap: 15px; justify-content: space-around">
                <template v-if="item.permissions && item.permissions.length > 0">
                  <div class="form-check form-check-success mb-3" v-for="(subItem, subIndex) in item.permissions"
                       :key="subItem.id + 'abc'">
                    <b-form-checkbox v-model="subItem.checked" @change="handleAddPermissionsToRole(subItem)" switch
                                     size="sm">
                      <div style="display: flex; flex-direction: column">
                        <label class="form-check-label"> {{ index }}.{{ ++subIndex }} {{ subItem.name }} </label>
                        <span class="form-check-label" style="font-size: 10px"> {{ subItem.action }}</span>
                      </div>
                    </b-form-checkbox>
                  </div>
                </template>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>
