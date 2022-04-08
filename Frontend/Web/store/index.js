
export const state = () => ({
  callingAction: '',
  payload: {},
  siteName: 'Realestateify'
})

export const mutations = {
  setCurrentState(state, {callingAction, payload}) {
      state.callingAction = callingAction
      state.payload = payload
  }
}

export const actions = {


  async nuxtServerInit({commit, dispatch}, context) {

    try {
      const auth = await dispatch('auth/initAuth', context)
      if (auth) {
        await dispatch('auth/getProfileSSR', context)
      }
      return Promise.resolve(true)
    } catch (error) {
      return Promise.reject(error)
    } finally {
      return Promise.resolve()
    }
  }

}