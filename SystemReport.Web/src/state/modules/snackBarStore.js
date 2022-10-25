import Vue from "vue";

const options = {
    timeout: 2000
}

const initialNotify =  {resultString: null, resultCode: null};

export const actions = {
    async addNotify({commit}, data = initialNotify)
    {
        if (data.resultCode === 'SUCCESS') {
            Vue.$toast.success(data.resultString, options);
        } else if (data.resultCode === 'NAME_EXIST') {
            Vue.$toast.warning(data.resultString, options);
        }
        else {
            Vue.$toast.error(data.resultString, options);
        }
    }
}