import {apiClient} from "@/state/modules/apiClient";
const controller = "Enum";
export const actions = {
    async getMucDo({commit}) {
        return apiClient.get(controller + "/get-muc-do");
    }
};
