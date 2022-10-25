import {apiClient} from "@/state/modules/apiClient";
const controller = "auth";
export const state = {
    currentUser: localStorage.getItem('userToken'),
    authUser: localStorage.getItem('auth-user') ? JSON.parse(localStorage.getItem('auth-user')): {},
}

export const mutations = {
    SET_CURRENT_USER(state, newValue) {
        state.currentUser = newValue
    },
    SET_AUTH(state, newValue) {
        state.authUser = newValue
    }
}

export const getters = {
    loggedIn(state) {
        return !!state.currentUser
    },
        getAuthUser(state){
        return state.authUser
    }
}

export const actions = {
    async login({commit}, values) {
        return apiClient.post(controller +"/login", values);
    },
    async setAuth({commit}, auth) {
        commit('SET_AUTH', auth)
    }
};