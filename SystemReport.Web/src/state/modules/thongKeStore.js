import {apiClient} from "@/state/modules/apiClient";
const controller = "ThongKe";
export const actions = {

    async getTieuChi({commit}, values) {
        return apiClient.get(controller + "/get-tieu-chi/" + values);
    },
    async getBangBieu({commit}, {id = null, value=null}) {
        console.log(id, value)
        return apiClient.post(controller + "/get-bangbieu" , value);
    },
};
