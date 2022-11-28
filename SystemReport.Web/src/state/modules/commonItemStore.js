import {apiClient} from "@/state/modules/apiClient";
const controller = "CommonItem";
export const actions = {
    async getByType({commit}, type) {
        return apiClient.get(controller +"/get-by-type/" + type);
    },
    async create({commit}, values) {
        return apiClient.post(controller + "/create", values);
    },
};
