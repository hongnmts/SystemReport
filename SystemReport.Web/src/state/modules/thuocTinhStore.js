import {apiClient} from "@/state/modules/apiClient";
const controller = "ThuocTinh";
export const actions = {
    async getAll({commit}) {
        return apiClient.get(controller +"/get");
    },
    async getPagingParams({commit}, params) {
        return apiClient.post(controller + "/get-paging-params", params);
    },
    async create({commit}, values) {
        return apiClient.post(controller + "/create", values);
    },
    async update({commit, dispatch}, values) {
        return apiClient.put(controller + "/update", values);
    },
    async delete({commit}, id) {
        return await apiClient.delete(controller + "/delete/" + id);
    },
    async getById({commit}, id) {
        return apiClient.get(controller + "/get-by-id/" + id);
    },
    async getThuocTinhByBangBieuId({commit}, id) {
        return apiClient.get(controller + "/get-thuoc-tinh-by-bang-bieu-id/" + id);
    },
    async getTreeThuocTinhByBangBieuId({commit}, id) {
        return apiClient.get(controller + "/get-tree-thuoc-tinh-by-bang-bieu-id/" + id);
    },
    async getListTreeThuocTinhByBangBieuId({commit}, id) {
        return apiClient.get(controller + "/get-list-tree-thuoc-tinh-by-bang-bieu-id/" + id);
    },
    async getThuocTinhChiTieu({commit}, id) {
        return apiClient.get(controller + "/get-thuoc-tinh-chitieu/" + id);
    },
    async getThuocTinhHasChildren({commit}, id) {
        return apiClient.get(controller + "/get-thuoc-tinh-haschildren/" + id);
    },
};
