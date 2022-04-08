import { resolveConfig } from 'prettier'

export const state = () => ({
  items: []
})

export const getters = {
  images(state) {
    return state.items || null
  }
}

export const mutations = {
  SET_IMAGES(state, images) {
    state.items = images
  },
  ADD_IMAGE(state, newImage) {
    state.items.push(newImage)
  },
  SET_MAIN(state, imageId) {
 
    state.items.map(x => {
        x.isMain = false
    })

    var index = state.items.map(x => {return x.id;}).indexOf(imageId);
    state.items[index].isMain = true
 
  },
  REMOVE_IMAGE(state, imageId) {
    var index = state.items.map(x => {
        return x.id;
      }).indexOf(imageId);
    state.items.splice(index, 1);
  },
}

export const actions = {
  async getImages({ commit }, propertyId) {
    try {
      const images = await this.$axios.$get(`${process.env.apiURL}/Manage/listings/property/${propertyId}/images`)
      
      commit('SET_IMAGES', images)

      return Promise.resolve(images)
    } catch (e) {
      return Promise.reject(e)
    }
  },

  async addImage({ commit }, payload ) {

    try {
      const newImage = await this.$axios.$post(`${process.env.apiURL}/Manage/listings/property/${payload.propertyId}/addImage`, payload.data)
      
      commit('ADD_IMAGE', newImage)

      return Promise.resolve(newImage)
    } catch (e) {
        const message = e.response.errors
        const err = message[Object.keys(message)][0][0]
        return Promise.reject(err)
    }
  },

  async setMain({ commit, dispatch }, payload) {
    try {
      const mainImageUrl = await this.$axios.$post(`${process.env.apiURL}/Manage/listings/property/${payload.propertyId}/setMainImage/${payload.imageId}`)
      
      commit('SET_MAIN', payload.imageId)
      commit('manage/listings/SET_MAIN_IMAGE', mainImageUrl, {root: true})
      // dispatch('manage/listings/setMainImage', mainImageUrl)
      return Promise.resolve()
    } catch (e) {
      return Promise.reject(e)
    }
  },

  async deleteImage({ commit }, payload) {
    try {
      await this.$axios.$delete(`${process.env.apiURL}/Manage/listings/property/${payload.propertyId}/deleteImage/${payload.imageId}`)
      
      commit('REMOVE_IMAGE', payload.imageId)

      return Promise.resolve()
    } catch (e) {
      return Promise.reject(e)
    }
  },



  
}
