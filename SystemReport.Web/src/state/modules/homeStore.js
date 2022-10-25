import {apiClient} from "@/state/modules/apiClient";
const controller = "home";
export const actions = {
    async homeValue({commit}) {
        return apiClient.get(controller +"/home-value");
    },
    async homeCongbo({commit}, values) {
        return apiClient.post(controller + "/congbo", values);
    },
};
