import {apiClient} from "@/state/modules/apiClient";
const controller = "dashboard";
export const actions = {
    async getDashboard({commit}, params) {
        return apiClient.get(controller + "/getDashboard");
    },
};
