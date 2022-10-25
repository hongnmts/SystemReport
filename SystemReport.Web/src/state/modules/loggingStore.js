import {apiClient} from "@/state/modules/apiClient";
const controller = "Logger";
export const actions = {
    async getPagingParams({commit}, params) {
        return apiClient.post(controller + "/get-paging-params", params);
    },
    async getById({commit}, id) {
        return apiClient.get(controller + "/get-by-id/" + id);
    }
};
