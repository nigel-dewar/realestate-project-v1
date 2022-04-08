
export default class AuthApiService {

    
  constructor(_axios) {
    this.$axios = _axios
  }

  async getCount() {
    try {
        const count = await this.$axios.$get(`${process.env.apiURL}/Manage/listings/count`)
        return Promise.resolve(count)
      } catch (error) {
        return Promise.reject(error)
      }
  };

  async canCreateAddressCheck(payload) {
    try {
        const data = this.$axios.$post(`${process.env.apiURL}/Manage/Listings/CanCreateAddressCheck`, payload)
        return Promise.resolve(data)
    } catch (error) {
        return Promise.reject(error)
    }
  }

  

}
