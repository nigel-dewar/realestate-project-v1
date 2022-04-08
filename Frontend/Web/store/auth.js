import AuthApi from "~/api/api-auth";

export const state = () => ({
  user: null,
  token: null,
  refresh_token: null,
  profile: null,
  userTypes: null
})

export const getters = {
  authUser(state) {
    return state.user || null
  },
  // isAuthenticated(state) {
  //     return state.user &&
  //       state.token !== null
  // },
  isAuthenticated(state) {
    return state.token !== null
  },
  isAdmin(state) {
    return state.user && state.user.roles && state.user.roles.includes('Admin')
  },
  profileComplete(state) {
    return state.profile && state.profile.userProfileComplete
  },
  profile(state) {
    return state.profile
  },
  mobileConfirmed(state) {
    return state.profile.mobileConfirmed !== null
  },
  userEmail(state) {
    return state.user.email || null
  },
  userTypes(state) {
    return state.userTypes || null
  }
}

export const mutations = {
  setAuthUser(state, user) {
    state.user = user
  },
  clearAuthUser() {
    state.user = null
    state.profile = null
  },
  setToken(state, payload) {
    state.token = payload.token
    state.refresh_token = payload.refresh_token
  },
  clearToken(state) {
    state.token = null
    state.refresh_token = null
    state.user = null
    state.profile = null
    // delete this.$axios.defaults.headers.common['Authorization']
  },
  SET_PROFILE(state, payload) {
    state.profile = payload
  },
  SET_USER_TYPES(state, payload) {
    state.userTypes = payload
  },
  SET_MOBILE_NUMBER(state, payload) {
    state.profile.mobileNumber = payload.mobileNumber
    state.profile.mobileCountryCode = payload.countryCode
  },
  SET_MOBILE_CONFIRMED(state, payload) {
    state.profile.mobileConfirmed = payload
  },
  SET_EMAIL(state, payload) {
    state.user.email = payload
  },
  
}

export const actions = {
  setLogoutTimer({ commit, dispatch }, duration) {
    setTimeout(() => {
      dispatch('refreshToken')
    }, duration - 1000)
  },
  logout({ commit }) {
    this.$cookies.removeAll()
    commit('setAuthUser', null)
    commit('clearToken')
    localStorage.clear()
    return Promise.resolve()
  },

  sendPasswordReset({ commit, state }, payload) {
    return this.$axios
      .$post(`${process.env.apiURL}/Identity/ForgotPassword`, payload)
      .then((response) => {
        return Promise.resolve(response.data)
      })
      .catch((error) => Promise.reject(error))
  },
  resetPassword({ commit, state }, payload) {
    return this.$axios
      .$post(`${process.env.apiURL}/Identity/ResetPassword`, payload)
      .then((response) => {})
      .catch((error) => Promise.reject(error))
  },
  async login({ commit, state, dispatch }, loginData) {
    try {
      // let _authApi = new AuthApi(this.$axios);
      // const response = await _authApi.login(loginData)
      // console.log($config.apiURL)
      const response = await this.$axios.$post(`${process.env.apiURL}/Identity/Login`, loginData)

      this.$axios.defaults.headers['Authorization'] =
          'Bearer ' + response.access_token

      // let _authApi = new AuthApi(this.$axios);

      // const profile = await _authApi.getUserProfile()
      const profile = await this.$axios.$get(
        `${process.env.apiURL}/Manage/UserProfile`
      )
      commit('SET_PROFILE', profile)
      
      commit('setToken', {
        token: response.access_token,
        refresh_token: response.refresh_token,
      })
      commit('setAuthUser', response)

        this.$cookies.set('x-user', response)
        this.$cookies.set('x-access-token', response.access_token)
        this.$cookies.set('x-refresh-token', response.refresh_token)
        this.$cookies.set(
          'x-expires-in',
          new Date().getTime() + +response.expiresIn * 1000
        )
        let redirectPath = '/'
        let lastKnownRoute = this.$cookies.get('x-last-route')

        if (lastKnownRoute) redirectPath = lastKnownRoute

      return Promise.resolve(redirectPath)
    } catch (error) {
      if (error.response) {
        return Promise.reject(error.response.data.errors)
      } else if (error.request) {
        // The request was made but no response was received
        return Promise.reject({
          message: 'Unknown server error. Please contact support',
          code: '500',
        })
        //
      } else {
        // Something happened in setting up the request that triggered an Error
        return Promise.reject({
          message: 'Unknown server error. Please contact support',
          code: '500',
        })
      }
    }
  },
  register({ commit, state }, data) {
    return this.$axios
      .$post(`${process.env.apiURL}/Identity/Register`, data)
      .then((response) => {
        // TODO: put a toast here
        let redirectPath = '/'

        let lastKnownRoute = this.$cookies.get('x-last-route')

        if (lastKnownRoute) redirectPath = lastKnownRoute
        return redirectPath
        // return Promise.resolve({message: 'successful', code: 200, redirect: redirectPath})
      })
      .catch((error) => {
        if (error.response) {
          return Promise.reject(error.response.data.errors)
        } else if (error.request) {
          // The request was made but no response was received
          return Promise.reject({
            message: 'Unknown server error. Please contact support',
            code: '500',
          })
          //
        } else {
          // Something happened in setting up the request that triggered an Error
          return Promise.reject({
            message: 'Unknown server error. Please contact support',
            code: '500',
          })
        }
      })
  },
  // runs from middleware
  checkAuth(vuexContext, context) {
    let token
    let refreshToken
    let tokenExpires
    let user

    const isClient = process.client

    user = context.$cookies.get('x-user')
    token = context.$cookies.get('x-access-token')
    refreshToken = context.$cookies.get('x-refresh-token')
    tokenExpires = context.$cookies.get('x-expires-in')

    if (!token) {
      return
    }

    if (isClient) {
      if (new Date().getTime() > +tokenExpires || !token) {
        if (refreshToken) {
          // attempt to get refresh token
          return this.$axios({
            method: 'post',
            url: `${process.env.apiURL}/Identity/TokenRefresh`,
            data: {
              refreshToken: refreshToken,
            },
          })
            .then((response) => {
              vuexContext.commit('setToken', {
                token: response.data.access_token,
                refresh_token: response.data.refresh_token,
              })
              vuexContext.commit('setAuthUser', response.data)

              if (process.server) {
                context.$cookies.set(
                  'x-access-token',
                  response.data.access_token
                )
                context.$cookies.set(
                  'x-refresh-token',
                  response.data.refresh_token
                )
                context.$cookies.set(
                  'x-expires-in',
                  new Date().getTime() + +response.data.expiresIn * 1000
                )
              } else {
                this.$cookies.set('x-access-token', response.data.access_token)
                this.$cookies.set(
                  'x-refresh-token',
                  response.data.refresh_token
                )
                this.$cookies.set(
                  'x-expires-in',
                  new Date().getTime() + +response.data.expiresIn * 1000
                )
              }

              return state.user
            })
            .catch((error) => {
              vuexContext.commit('setAuthUser', null)
              vuexContext.commit('clearToken')
              return Promise.reject(error)
            })
        }
        vuexContext.commit('setAuthUser', null)
        vuexContext.commit('clearToken')
        return
      }
    }
    return state.user
  },
  // runs from the index.js store action - root
  initAuth(vuexContext, context) {

    let token
    let refreshToken
    let tokenExpires
    let user

    var userToken = context.$cookies.get('x-user')
    if (userToken) {
      user = JSON.parse(userToken)
    }
    token = context.$cookies.get('x-access-token')
    refreshToken = context.$cookies.get('x-refresh-token')
    tokenExpires = context.$cookies.get('x-expires-in')

    if (!token) {
      return
    }

    if (new Date().getTime() > +tokenExpires || !token) {
      return this.$axios({
        method: 'post',
        url: `${process.env.apiURL}/Identity/TokenRefresh`,
        data: {
          refreshToken: refreshToken,
        },
      })
        .then((response) => {
          vuexContext.commit('setToken', {
            token: response.data.access_token,
            refresh_token: response.data.refresh_token,
          })

          // vuexContext.commit('setAuthUser', null)
          // vuexContext.commit('clearToken')
          vuexContext.commit('setAuthUser', response.data)

          if (process.server) {
            context.$cookies.set('x-user', response.data)
            context.$cookies.set('x-access-token', response.data.access_token)
            context.$cookies.set('x-refresh-token', response.data.refresh_token)
            context.$cookies.set(
              'x-expires-in',
              new Date().getTime() + +response.data.expiresIn * 1000
            )
          } else {
            this.$cookies.set('x-user', response.data)
            this.$cookies.set('x-access-token', response.data.access_token)
            this.$cookies.set('x-refresh-token', response.data.refresh_token)
            this.$cookies.set(
              'x-expires-in',
              new Date().getTime() + +response.data.expiresIn * 1000
            )
          }

          return response.data.access_token
        })
        .catch((error) => {
          vuexContext.commit('setAuthUser', null)
          vuexContext.commit('clearToken')
          return error
        })
    } else {
      vuexContext.commit('setToken', {
        token: token,
        refresh_token: refreshToken,
      })
      vuexContext.commit('setAuthUser', user)
      return token
    }
  },

  async getMe({ commit, getters, state }) {
    try {
      await this.$axios.$get(`${process.env.apiURL}/Identity/Me`)
      return Promise.resolve(state.user)
    } catch (error) {
      return Promise.reject(error)
    }
  },

  async getProfile({ commit }) {
    try {

      const profile = await this.$axios.$get(
        `${process.env.apiURL}/Manage/UserProfile`
      )
      commit('SET_PROFILE', profile)
      return Promise.resolve(profile)
    } catch (error) {
      return Promise.reject(error)
    }
  },

  async getProfileSSR(vuexContext, context) {

    try {
      context.$axios.defaults.headers["Authorization"] = `Bearer ${
        vuexContext.state.token
      }`;
      // let _authApi = new AuthApi(context.$axios);
      // const profile = await _authApi.getUserProfile()
      const profile = await this.$axios.$get(
        `${process.env.apiURL}/Manage/UserProfile`
      )
      vuexContext.commit('SET_PROFILE', profile)
      return Promise.resolve(profile)
    } catch (error) {
      return Promise.reject(error)
    }
  },

  async saveProfile({ commit }, payload) {
    try {
      const profile = await this.$axios.$post(
        `${process.env.apiURL}/Manage/UserProfile`,
        payload
      )
      commit('SET_PROFILE', profile)

      return Promise.resolve()
    } catch (error) {
    }
  },

  async submitMobile({ commit }, payload) {
    try {
      const res = await this.$axios.$post(
        `${process.env.apiURL}/Manage/UserProfile/check-mobile-number`,
        payload
      )

      commit('SET_MOBILE_NUMBER', res)

      return Promise.resolve(res)
    } catch (error) {
      return Promise.reject(error)
    }
  },

  async submitCode({ commit }, payload) {
    try {
      const res = await this.$axios.$post(
        `${process.env.apiURL}/Manage/UserProfile/submit-mobile-code`,
        payload
      )

      commit('SET_MOBILE_CONFIRMED', res)
      return Promise.resolve(res)
    } catch (error) {
      return Promise.reject(error)
    }
  },

  async getUserTypes({ commit }) {
    try {
      const userTypes = await this.$axios.$get(
        `${process.env.apiURL}/Manage/UserProfile/user-types`
      )
      commit('SET_USER_TYPES', userTypes)

      return Promise.resolve(userTypes)
    } catch (error) {
      return Promise.reject(error)
    }
  },

  testApi() {
    return this.$axios({
      method: 'GET',
      url: `${process.env.apiURL}/api/values`,
    })
      .then((response) => {
        return Promise.resolve(response)
      })
      .catch((error) => {
        // commit('setAuthUser', null)
        return Promise.reject(error)
      })
  },

  // async getMe({commit}) {
  //   try {
  //     let _authApi = new AuthApi(context.$axios);
  //     const email = await _authApi.getUserEmail()
  //     commit('SET_EMAIL', email)
  //     return Promise.resolve(email)
  //   } catch (error) {
  //     return Promise.reject(error)
  //   }
  // }
}
