
export default class AuthApiService {

    
  constructor(_axios) {
    this.$axios = _axios
  }

  async getUserProfile() {
    try {
        const profile = await this.$axios.$get(
          `${process.env.apiURL}/Manage/UserProfile`
        )
        return Promise.resolve(profile)
      } catch (error) {
        return Promise.reject(error)
      }
  };

  async login(loginData) {
    try {
      // {{ $config.baseURL}}
        const login = this.$axios.$post(`${process.env.apiURL}/Identity/Login`, loginData)
        // console.log($config.apiURL)
        // const login = this.$axios.$post(`${$config.apiURL}/Identity/Login`, loginData)
        return Promise.resolve(login)
    } catch (error) {
        return Promise.reject(error)
    }
  }

  async getUserEmail(loginData) {
    try {
        const email = this.$axios.$get(`${process.env.apiURL}/Identity/Me`)
        return Promise.resolve(email)
    } catch (error) {
        return Promise.reject(error)
    }
  }

  

}
