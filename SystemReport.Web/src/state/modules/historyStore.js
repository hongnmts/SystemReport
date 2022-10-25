import {apiClient} from "@/state/modules/apiClient";
const controller = "history";
export const actions = {
    async mauBieu({commit}, params) {
        return apiClient.post(controller + "/get-maubieu", params);
    },
    async getBangBieuDetail({commit}, id) {
        return apiClient.get(controller + "/get-bangbieu-detail/" + id);
    },
};
