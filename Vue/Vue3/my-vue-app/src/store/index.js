
import { createStore } from 'vuex'

const store = createStore({
    state: {
        name: '前端Vue'
    },
    getters: {},
    mutations: {
        changeName(state, data) {
            state.name = data
        }
    },
    actions: {
        changeVal(state) {
            setTimeout(() => {
                state.commit('changeName', '我不是我')
            }, 2000)
        }
    },
    modules: {},
})

export default store