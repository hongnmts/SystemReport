import {apiClient} from "@/state/modules/apiClient";
const controller = "MauBieu";
export const actions = {
    async get({commit}) {
        return apiClient.get(controller +"/get");
    },
    async listNamMauBieu({commit}) {
        return apiClient.get(controller +"/ListNamMauBieu");
    },
    async getPagingParams({commit}, params) {
        return apiClient.post(controller + "/get-paging-params", params);
    },
    async getPagingParamsNhapLieu({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-nhaplieu", params);
    },
    async getPagingParamsKiemTra({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-kiemtra", params);
    },
    async getPagingParamsTongHop({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-tonghop", params);
    },
    async getPagingParamsXuatBan({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-xuatban", params);
    },
    async getPagingParamsCaNhan({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-canhan", params);
    },
    async getPagingParamsThongTinXuatBan({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-thongtinxuatban", params);
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
    async deleteMauBieu({commit}, id) {
        return await apiClient.delete(controller + "/deleted-maubieu/" + id);
    },
    async getById({commit}, id) {
        return apiClient.get(controller + "/get-by-id/" + id);
    },
    async renderTable({commit}, id) {
        return apiClient.get(controller + "/render-table/" + id);
    },
    async renderTableMauBieu({commit}, id) {
        return apiClient.get(controller + "/render-table-maubieu/" + id);
    },
    async generateMauBieu({commit}, values) {
        return apiClient.post(controller + "/generate-maubieu", values);
    },
    async changeStatus({commit}, values) {
        return apiClient.post(controller + "/change-status", values);
    },
};
