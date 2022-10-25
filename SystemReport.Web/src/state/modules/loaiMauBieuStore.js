import {apiClient} from "@/state/modules/apiClient";
const controller = "LoaiMauBieu";
export const actions = {
    async get({commit}) {
        return apiClient.get(controller +"/get");
    },
    async getAll({commit}) {
        return apiClient.get(controller +"/get-all");
    },
    async getLoaiMauBieu({commit}) {
        return apiClient.get(controller +"/get-loaimaubieu");
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
    }
};
