import {apiClient} from "@/state/modules/apiClient";
const controller = "RowValue";
export const actions = {
    async create({commit}, values) {
        return apiClient.post(controller + "/create", values);
    },
    async createSub({commit}, values) {
        return apiClient.post(controller + "/create-sub", values);
    },
    async update({commit, dispatch}, values) {
        return apiClient.put(controller + "/update", values);
    },
    async delete({commit}, id) {
        return await apiClient.delete(controller + "/delete/" + id);
    },
    async deleteRowValue({commit}, values) {
        return apiClient.post(controller + "/delete-rowvalue", values);
    },
    async getById({commit}, id) {
        return apiClient.get(controller + "/get-by-id/" + id);
    },
    async getTreeRowValueByBangBieuId({commit}, id) {
        return apiClient.get(controller + "/get-tree-row-value-by-bang-bieu-id/" + id);
    },
    async renderBodyByBangBieuId({commit}, id) {
        return apiClient.get(controller + "/render-body-by-bang-bieu-id/" + id);
    },
    async renderBodyMainByBangBieuId({commit}, id) {
        return apiClient.get(controller + "/render-body-main-by-bang-bieu-id/" + id);
    },
    async getTreeParentRowValue({commit}, id) {
        return apiClient.get(controller + "/get-parent-row-value-by-bang-bieu-id/" + id);
    },
    async getRowValueByKeyRow({commit}, id) {
        return apiClient.get(controller + "/get-row-value-by-key-row/" + id);
    },
    async addRowTong({commit}, id) {
        return apiClient.get(controller + "/add-row-tong/" + id);
    },
};
