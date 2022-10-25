import {apiClient} from "@/state/modules/apiClient";
const controller = "BangBieu";
export const actions = {
    async get({commit}) {
        return apiClient.get(controller +"/get");
    },
    async getPagingParams({commit}, params) {
        return apiClient.post(controller + "/get-paging-params", params);
    },
    async create({commit}, values) {
        return apiClient.post(controller + "/create", values);
    },
    async saveDataBangBieu({commit}, values) {
        return apiClient.post(controller + "/save-data-bang-bieu", values);
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
    async getBangBieuByMauBieuId({commit}, id) {
        return apiClient.get(controller + "/get-bangbieu-by-maubieuid/" + id);
    },
    async renderHeader({commit}, id) {
        return apiClient.get(controller + "/render-header/" + id);
    },
    async renderNhapLieuBangBieu({commit}, id) {
        return apiClient.get(controller + "/render-nhaplieu-bangbieu/" + id);
    },
};
