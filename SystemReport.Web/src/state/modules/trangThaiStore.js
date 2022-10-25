import {apiClient} from "@/state/modules/apiClient";
const controller = "TrangThai";
export const actions = {
    async get({commit}) {
        return apiClient.get(controller +"/get");
    },
    async getPagingParams({commit}, params) {
        return apiClient.post(controller + "/get-paging-params", params);
    },
    async getNextTrangThai({commit}, params) {
        return apiClient.post(controller + "/get-next-trangthai", params);
    },
    async getTrangThai({commit}) {
        return apiClient.get(controller +"/get-all");
    },
    async getTree({commit}) {
        return apiClient.get(controller +"/getTree");
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
    }
};
