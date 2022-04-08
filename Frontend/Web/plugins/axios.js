
export default function ({store, $axios, redirect, app}) {

  if (store.state.auth.token) {
      $axios.defaults.headers['Authorization'] = 'Bearer ' + store.state.auth.token
  }

  // $axios.interceptors.request.use(function (config) {

  //   if (store.state.auth.token)
  //     config.headers.Authorization = store.state.auth.token
  //     // $axios.defaults.headers['Authorization'] = 'Bearer ' + store.state.auth.token

  //   return config;
  //   // if (store.state.auth.token) {
  //   //   config.headers.Authorization = store.state.auth.token
  //   // }

  //   // return config
  //     // $axios.defaults.headers.common['Authorization'] = 'Bearer ' + store.state.auth.token
  //   // const token = store.getState().session.token;
      

  //   // return config;
  // });

  $axios.interceptors.response.use(undefined, function(error) {

    const originalRequest = error.config;
    if (
      error.response.status === 401 &&
      !originalRequest._retry &&
      store.state.auth.refresh_token
    ) {
      originalRequest._retry = true;
  
      const payload = {
        refreshToken: store.state.auth.refresh_token
      };
  
      return $axios
        .post(`${process.env.idsURL}/api/auth/refresh`, payload)
        .then(response => {
          const auth = response.data;
          $axios.defaults.headers["Authorization"] = `Bearer ${
            auth.access_token
          }`;
          originalRequest.headers["Authorization"] = `Bearer ${
            auth.access_token
          }`;

          store.commit('auth/setToken', { token: response.data.access_token, refresh_token: response.data.refresh_token})
          store.commit('auth/setAuthUser', response.data)

          // app.$cookies.set('x-user', response.data)
          app.$cookies.set('x-access-token', response.data.access_token)
          app.$cookies.set('x-refresh-token', response.data.refresh_token)
          app.$cookies.set('x-expires-in', new Date().getTime() + +response.data.expiresIn * 1000)

          // $axios.defaults.headers.common['Authorization'] = 'Bearer ' + response.data.access_token
          // $axios.setToken(store.state.auth.token, 'Bearer')
          // store.commit("loginSuccess", auth);
          return $axios(originalRequest)
        })
        .catch(error => {
          // store.dispatch('auth/logout')
          // router.push({ path: "/" });
          delete $axios.defaults.headers["Authorization"];
          return Promise.reject(error);
        });
    }
  
    return Promise.reject(error);
  });

}
