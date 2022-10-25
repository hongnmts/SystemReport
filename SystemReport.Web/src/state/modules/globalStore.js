export const getters ={
    auth(state){
        return state.chiTieus
    }
}

export const state = () =>({
    chiTieus: []
})

export const mutations ={
    SET_CHITIEUS(state, item){
        state.chiTieus = item;
    },
    CLEAR_CHITIEUS(state){
        state.chiTieus = []
    }
}

export const actions = {
    async setChiTieus({commit}, item){
        await commit('SET_CHITIEUS', item)
    },
    async clearChiTieus({commit}){
        await commit('CLEAR_CHITIEUS')
    }
}

