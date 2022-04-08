import ApiListing from "~/api/api-listing";
export const state = () => ({
  items: [],
  listing: null,
  property: null,
  propertiesToList: [],
  lookups: null,
  count: 0
})

const errorHandler = (error) => {
  if (error.response) {
    return {message: error.response.data.errors.Errors, code: error.response.data.errors.Code }
  } else if (error.request) {
    return {message: "Unknown server error. Please contact support", code: "500"}
    // 
  } else {
    return {message: "Unknown server error. Please contact support", code: "500"}
  }
}

export const getters = {
  listings(state) {
    return state.items || null
  },
  listing(state) {
    return state.listing || null
  },
  lookups(state) {
    return state.lookups || null
  },
  propertiesCanListFresh(state) {
    return state.propertiesToList.filter(x=>x.reList == false) || []
  },
  propertiesCanReList(state) {
    return state.propertiesToList.filter(x=>x.reList == true) || []
  },
  listingComplete(state) {
    if (!state.listing.complete) 
      return { text: 'Incomplete', class: 'is-warning', desc: 'This Ad is missing some essential information before it can be started' }

    if (state.listing.complete) 
      return { text: 'Complete', class: 'is-success', desc: 'This Ad is complete and can be started started' }  
    
  },
  listingStatus(state) {

    if (state.listing.cancelled) 
      return { text: 'Cancelled', class: 'is-dark', desc: 'This Ad was cancelled. You can Relist this Ad' }

    if (state.listing.expired) 
      return { text: 'Expired', class: 'is-danger', desc: 'This Ad has expired past its End Date. You can Relist this Ad to run it again' }

    if (!state.listing.complete) 
      return { text: 'Stopped', class: 'is-danger', desc: 'This Ad is missing some essential information. It cannot be started until the Ad is 100% complete' }

    if (state.listing.complete && !state.listing.active) 
      return { text: 'Not Started', class: 'is-info', desc: 'This Ad is complete. You can start this Ad when you are ready' }

    if (state.listing.isPublishedLive) 
      return { text: 'Started', class: 'is-success', desc: 'This Ad is now Live. You can search for your Ad or View directly here by clicking the View Live Ad button' }  

    if (state.listing.active) 
      return { text: 'Started', class: 'is-success', desc: 'This Ad is now Active. You will recieve an email when the Ad is processed and appears in search results. NOTE: Changes made to an Active Ad can take up to 15 mins to display' }
    
    
    // return {text: 'Incomplete', class: 'is-warning'}
  },
  listingConfirmed(state) {

    if (state.listing.cancelled) return { text: 'Confirmed - But Cancelled', class: 'is-dark' }
    if (state.listing.confirmed) return { text: 'Confirmed', class: 'is-success' }

    return {text: 'Not Confirmed', class: 'is-primary'}
  },
  userRequestOptions(state) {
    var opts = {
      canConfirm: false,
      canRelist: false,
      canCancel: false,
      canRelist: false
    }

    if (state.listing) {
      if (state.listing.cancelled) {
        opts.canRelist = true
      }

      if (state.listing.confirmed && !state.listing.cancelled) {
        opts.canCancel = true
      }
  
      if (state.listing.complete && !state.listing.confirmed) {
        opts.canConfirm = true
      }
    }

    return opts
  }
}

export const mutations = {
  RESET(state) {
    state.items = []
    state.listing = null
    state.property = null
    state.propertiesToList = []
    state.lookups = null
    state.count = 0
  },
  SET_LISTINGS(state, listings) {
    state.items = listings
  },
  SET_LISTING(state, listing) {
    state.listing = {...listing}
  },
  SET_PROPERTY(state, property) {
    state.property = {...property}
  },
  SET_LOOKUPS(state, lookups) {
    state.lookups = lookups
  },
  SET_MAIN_IMAGE(state, imageUrl) {
    state.listing.mainImage = imageUrl
  },
  SET_COUNT(state, count) {
    state.count = count
  },
  SET_PROPERTIES_TO_LIST(state, payload) {
    state.propertiesToList = payload
  }
}

export const actions = {

  reset({commit}) {
    try {

      commit('RESET')

      return Promise.resolve(true)
      
    } catch (error) {
      return Promise.reject(errorHandler(error))
    }
    
  },

  setMainImage({commit}, mainImageUrl) {
    commit('SET_MAIN_IMAGE', mainImageUrl)
  },

  async fetchListings({ commit }) {

    try {
      const listings = await this.$axios.$get(`${process.env.apiURL}/Manage/listings`)
      commit('SET_LISTINGS', listings)

      return Promise.resolve(listings)
      
    } catch (error) {
      return Promise.reject(errorHandler(error))
    }
  },

  async fetchPropertiesToList({ commit }, listingType) {

    try {
      const propertiesToList = await this.$axios.$get(`${process.env.apiURL}/Manage/Property/CanList/${listingType}`)
      commit('SET_PROPERTIES_TO_LIST', propertiesToList)

      return Promise.resolve(propertiesToList)
      
    } catch (error) {
      return Promise.reject(errorHandler(error))
    }
  },

  

  async createProperty({ commit }, payload) {

    try {
      const property = await this.$axios.$post(`${process.env.apiURL}/Manage/Property/Create`, payload)

      commit('SET_PROPERTY', property)

      return Promise.resolve(property)
    } catch (error) {
      return Promise.reject(errorHandler(error))
    }
  },

  async createListing({ commit }, payload) {

    try {

      const listing = await this.$axios.$post(`${process.env.apiURL}/Manage/Listings/Create`, payload)
      commit('SET_LISTING', listing)

      return Promise.resolve(listing)
    } catch (error) {
      
      return Promise.reject(errorHandler(error))
    }
  },

  async getListing({ commit }, listingId) {
    
    try {
      const listing = await this.$axios.$get(`${process.env.apiURL}/Manage/listings/${listingId}`)
      commit('SET_LISTING', listing)
      // commit('SET_PROPERTY', listing.property)

      // const lookups = await this.$axios.$get(`${process.env.apiURL}/api/v1/listings/lookups`)
      // commit('SET_LOOKUPS', lookups)

      return Promise.resolve(listing.id)
      
    } catch (error) {
      
      return Promise.reject(errorHandler(error))
    }
  },

  async save({ commit }, payload) {

    try {
      const listing = await this.$axios.$post(`${process.env.apiURL}/Manage/listings`, payload)
      commit('SET_LISTING', listing)

      this.$toast.success('Progress Saved', {
        duration: 3000,
        position: 'top-center',
      })

      return Promise.resolve(listing)
      
    } catch (error) {

      return Promise.reject(errorHandler(error))
      
    }
  },
  

  async getLookups({ commit, rootState }) {
    
    try {
      const lookups = await this.$axios.$get(`${process.env.apiURL}/Manage/lookups`, { 
        // headers: {
        //   'Authorization': 'Bearer ' + rootState.auth.token || null
        // }
      });

      commit('SET_LOOKUPS', lookups)
      return Promise.resolve(lookups)

    } catch (error) {
      return Promise.reject(error)
    }
    
  },

  async getCount({ commit }) {
    
    try {
      let _api = new ApiListing(this.$axios)
      const res = await _api.getCount()
      commit('SET_COUNT', res)
      return Promise.resolve(res)

    } catch (error) {
      return Promise.reject(errorHandler(error))
    }
    
  },

  async canCreateAddressCheck({ commit }, payload) {
    
    try {
      let _api = new ApiListing(this.$axios)
      const res = await _api.canCreateAddressCheck(payload)
      commit('SET_COUNT', res)
      return Promise.resolve(res)

    } catch (error) {
      return Promise.reject(errorHandler(error))
    }
    
  },


}
