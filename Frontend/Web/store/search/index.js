import { fetchProperties } from '~/api/api'

const errorHandler = (error) => {
  if (error.response) {
    return {
      message: error.response.data.errors.Errors,
      code: error.response.data.errors.Code,
    }
  } else if (error.request) {
    return {
      message: 'Unknown server error. Please contact support',
      code: '500',
    }
    //
  } else {
    return {
      message: 'Unknown server error. Please contact support',
      code: '500',
    }
  }
}

const capitalize = (str) => {
  var splitStr = str.toLowerCase().split(' ')
  for (var i = 0; i < splitStr.length; i++) {
    // You do not need to check if i is larger than splitStr length, as your for does that for you
    // Assign it back to the array
    splitStr[i] = splitStr[i].charAt(0).toUpperCase() + splitStr[i].substring(1)
  }
  // Directly return the joined string
  return splitStr.join(' ')
}

const calcQueryString = async (query) => {
  try {
    const q = Object.keys(query)
      .map((k) => `${k}=${query[k]}`)
      .join('&')
    return Promise.resolve(q)
  } catch (error) {
    return Promise.reject(errorHandler(error))
  }
}

const rentPrices = [
  { id: 0, label: '$50', value: 50 },
  { id: 1, label: '$100', value: 100 },
  { id: 2, label: '$150', value: 150 },
  { id: 3, label: '$200', value: 200 },
  { id: 4, label: '$250', value: 250 },
  { id: 5, label: '$300', value: 300 },
  { id: 6, label: '$350', value: 350 },
  { id: 7, label: '$400', value: 400 },
  { id: 8, label: '$450', value: 450 },
  { id: 9, label: '$500', value: 500 },
  { id: 10, label: '$550', value: 550 },
  { id: 11, label: '$600', value: 600 },
  { id: 12, label: '$650', value: 650 },
  { id: 13, label: '$700', value: 700 },
  { id: 14, label: '$750', value: 750 },
  { id: 15, label: '$800', value: 800 },
  { id: 16, label: '$850', value: 850 },
  { id: 17, label: '$900', value: 900 },
  { id: 18, label: '$950', value: 950 },
  { id: 19, label: '$1000', value: 1000 },
  { id: 20, label: '$1100', value: 1100 },
  { id: 21, label: '$1200', value: 1200 },
  { id: 22, label: '$1300', value: 1300 },
  { id: 23, label: '$1400', value: 1400 },
  { id: 24, label: '$1500', value: 1500 },
  { id: 25, label: '$1600', value: 1600 },
  { id: 26, label: '$1700', value: 1700 },
  { id: 27, label: '$1800', value: 1800 },
  { id: 28, label: '$1900', value: 1900 },
  { id: 29, label: '$2000', value: 2000 },
  { id: 30, label: '$2500', value: 2500 },
  { id: 31, label: '$3000', value: 3000 },
  { id: 32, label: '$3500', value: 3500 },
  { id: 33, label: '$4000', value: 4000 }
]

const buyPrices = [
  { id: 0, label: '$50,000', value: 50000 },
  { id: 1, label: '$150,000', value: 150000 },
  { id: 2, label: '$200,000', value: 200000 },
  { id: 3, label: '$250,000', value: 250000 },
  { id: 4, label: '$300,000', value: 300000 },
  { id: 5, label: '$300,000', value: 350000 },
  { id: 6, label: '$400,000', value: 400000 },
  { id: 7, label: '$450,000', value: 450000 },
  { id: 8, label: '$500,000', value: 500000 },
  { id: 9, label: '$550,000', value: 550000 },
  { id: 10, label: '$600,000', value: 600000 },
  { id: 11, label: '$650,000', value: 650000 },
  { id: 12, label: '$700,000', value: 700000 },
  { id: 13, label: '$750,000', value: 750000 },
  { id: 14, label: '$800,000', value: 800000 },
  { id: 15, label: '$850,000', value: 850000 },
  { id: 16, label: '$900,000', value: 900000 },
  { id: 17, label: '$950,000', value: 950000 },
  { id: 18, label: '$1,000,000', value: 1000000 },
  { id: 19, label: '$1,100,000', value: 1100000 },
  { id: 20, label: '$1,200,000', value: 1200000 },
  { id: 21, label: '$1,300,000', value: 1300000 },
  { id: 22, label: '$1,400,000', value: 1400000 },
  { id: 23, label: '$1,500,000', value: 1500000 },
  { id: 24, label: '$1,600,000', value: 1600000 },
  { id: 25, label: '$2,000,000', value: 2000000 },
  { id: 26, label: '$3,000,000', value: 3000000 },
  { id: 27, label: '$4,000,000', value: 4000000 },
  { id: 28, label: '$5,000,000', value: 5000000 },
  { id: 29, label: '$6,000,000', value: 6000000 },
  { id: 30, label: '$7,000,000', value: 7000000 },
  { id: 31, label: '$8,000,000', value: 8000000 },
  { id: 32, label: '$9,000,000', value: 9000000 },
  { id: 33, label: '$10,000,000', value: 10000000 },
]

export const state = () => ({
  searchedSuburbs: [],
  uiUpdate: false, // important variable that determines how the Page and Components will be loaded
  queryString: '',
  queryParams: {},
  filtersLoaded: false,
  showFilters: false,
  showImages: false,
  // search params
  page: 1, // Feeds into Pagination Component
  mode: 'Sale', // 'Sale', 'Rent' - Equates to ListingType on backend
  price: [50000, 10000000],
  suburbs: {
    formatted: [],
    slugs: [],
    selected: [],
  }, // ['runcorn-4113', 'bondi-3565']
  bedRooms: [0, 10], // ['0','10+']
  bathRooms: [0, 10], // ['0','10+']
  parking: [0, 10], // ['0','10+']
  //
  property: {}, // current listing loaded into state
  propertyImages: [], // list of current listings Images loaded into state
  properties: [], // current list of Listings loaded into state
  //
  availablePages: 0,
  currentPageNumber: 0,
  currentPageResultsCount: 0,
  totalResultsCount: 0,

  lookups: {
    propertyTypes: [],
    features: [],
  },
})

export const getters = {
  showFilters: (state) => state.showFilters,
  showImages: (state) => state.showImages,
  images: (state) => state.propertyImages,
  address: (state) => state.property.address,
  mode: (state) => state.mode,
  searchedSuburbs: (state) => state.searchedSuburbs,
  selectedSuburbs: (state) => state.suburbs.selected,
  suburbsFormatted: (state) => state.suburbs.formatted,
  suburbsFormattedFirst: (state) => {
    return state.suburbs.formatted[0]
  },
  suburbsSlugs: (state) => state.suburbs.slugs,
  suburbsCount: (state) => state.suburbs.slugs.length,
  currentPageResultsCount: (state) => state.currentPageResultsCount,
  totalResultsCount: (state) => state.totalResultsCount,
  availablePages: (state) => state.availablePages,
  currentPageNumber: (state) => state.currentPageNumber,
  properties: (state) => state.properties,
  property: (state) => state.property,
  propertyImages: (state) => state.propertyImages,
  currentMode: (state) => {
    if (state.mode == 'rent') return 'Rent'
    if (state.mode == 'buy') return 'Sale'
  },
  lookups: (state) => state.lookups,
  selectedFeatures: (state) =>
    state.lookups.features.filter((x) => x.checked == true),
  selectedFeaturesNames: (state) =>
    state.lookups.features
      .filter((x) => x.checked == true)
      .map((x) => {
        return x.name
      }),
  selectedPropertyTypes: (state) =>
    state.lookups.propertyTypes.filter((x) => x.checked == true),
  selectedPropertyTypesNames: (state) =>
    state.lookups.propertyTypes
      .filter((x) => x.checked == true)
      .map((x) => {
        return x.name
      }),
  rentPrices: () => rentPrices,
  buyPrices: () => buyPrices
}

export const mutations = {
  UI_UPDATE(state, data) {
    state.uiUpdate = data
  },
  SET_SHOW_FILTERS(state) {
    state.showFilters = !state.showFilters
  },
  SET_SHOW_IMAGES(state) {
    state.showImages = !state.showImages
  },
  SET_QUERY_STRING(state, data) {
    state.queryString = data
  },
  SET_QUERY_PARAMS(state, data) {
    state.queryParams = data
  },
  SET_SEARCHED_SUBURBS(state, data) {
    state.searchedSuburbs = data
  },
  SET_SELECTED_SUBURBS(state, data) {
    state.suburbs.formatted = data.formatted
    state.suburbs.slugs = data.slugs
    state.suburbs.selected = data.selected
  },
  SET_PROPERTIES(state, data) {
    state.properties = data
  },
  SET_PROPERTY(state, data) {
    state.property = data
  },
  SET_PROPERTY_IMAGES(state, data) {
    state.propertyImages = data
  },
  SET_PAGE(state, data) {
    state.page = data
  },
  SET_SEARCH_RESULTS(state, data) {
    state.page = 1
    state.properties = data.properties
    state.availablePages = data.availablePages
    state.currentPageNumber = data.currentPageNumber
    state.currentPageResultsCount = data.currentPageNumber
    state.totalResultsCount = data.total
  },
  SET_LOOKUPS(state, data) {
    state.lookups = data
  },
  SET_LISTING_TYPE(state, data) {
    state.mode = data
  },
  SET_FEATURES(state, data) {
    state.lookups.features.find((x) => x.name == data.name).checked =
      data.checked
  },
  SET_PROPERTY_TYPES(state, data) {
    state.lookups.propertyTypes.find((x) => x.name == data.name).checked =
      data.checked
  },
  SET_MIN_PRICE(state, data) {
    state.price[0] = data
  },
  SET_MAX_PRICE(state, data) {
    state.price[1] = data
  },
  SET_MIN_BEDS(state, data) {
    state.bedRooms[0] = data
  },
  SET_MAX_BEDS(state, data) {
    state.bedRooms[1] = data
  },
  SET_MIN_BATHROOMS(state, data) {
    state.bathRooms[0] = data
  },
  SET_MAX_BATHROOMS(state, data) {
    state.bathRooms[1] = data
  },
  SET_MIN_PARKING_SPACES(state, data) {
    state.parking[0] = data
  },
  SET_MAX_PARKING_SPACES(state, data) {
    state.parking[1] = data
  },
}

export const actions = {
  async hyrdateStore({ commit, getters, state }, payload) {
    try {
      if (!state.uiUpdate) {
        commit('SET_QUERY_PARAMS', payload)
      } else {
        // console.log('no need to update params')
      }
      // commit('SET_SEARCHED_SUBURBS', res)
      return Promise.resolve()
    } catch (error) {
      return Promise.reject(errorHandler(error))
    }
  },
  async searchSuburbs({ commit }, query) {
    try {
      var queryBody = {
        query: query,
      }

      const res = await this.$axios.$post(
        `${process.env.apiURL}/Search/PostCodes/Search`,
        queryBody
      )

      commit('SET_SEARCHED_SUBURBS', res)
      return Promise.resolve(res)
    } catch (error) {
      return Promise.reject(errorHandler(error))
    }
  },
  async updateSelectedSuburbs({ commit }, data) {
    let formatted = []
    let slugs = []

    data.forEach((suburb) => {
      formatted.push(suburb.label)
      slugs.push(suburb.suburb)
    })

    const payload = {
      formatted,
      slugs,
      selected: data,
    }

    commit('SET_SELECTED_SUBURBS', payload)
    return Promise.resolve(true)
  },
  async updateSearch({ commit, state, getters }) {
    try {
      commit('UI_UPDATE', true)

      var query = {
        page: state.page, // Feeds into Pagination Component
        mode: state.mode, // 'Sale', 'Rent' - Equates to ListingType on backend
        suburbs: state.suburbs.slugs, // ['runcorn-4113', 'bondi-3565']
        propertyTypes: getters.selectedPropertyTypesNames,
        features: getters.selectedFeaturesNames,
        price: state.price,
        bedRooms: state.bedRooms, // ['0','10+']
        bathRooms: state.bathRooms, // ['0','10+']
        parking: state.parking, // ['0','10+']
      }

      const params = await calcQueryString(query)

      if (state.queryString != params) {
        commit('SET_QUERY_STRING', params)
        commit('SET_QUERY_PARAMS', query)
        // Only update route if queryString has actually changed
        this.$router.push(`/search/results?${params}`)
      } else {
        commit('SET_QUERY_STRING', params)
        commit('SET_QUERY_PARAMS', query)
        this.$router.push(`/search/results?${params}`)
      }
    } catch (error) {}
  },
  updateSearchResults({ commit }, payload) {
    try {
      commit('SET_SEARCH_RESULTS', payload)
    } catch (error) {
      return Promise.reject(errorHandler(error))
    }
  },
  updateListingType({ commit }, payload) {
    try {
      if (payload == 'Sale') {
        commit('SET_MIN_PRICE', buyPrices[0].value)
        commit('SET_MAX_PRICE', buyPrices.slice(-1)[0].value)
      } else if (payload == 'Rent') {
        commit('SET_MIN_PRICE', rentPrices[0].value)
        commit('SET_MAX_PRICE', rentPrices.slice(-1)[0].value)
      }
      commit('SET_LISTING_TYPE', payload)
      return Promise.resolve(true)
    } catch (error) {
      return Promise.reject(errorHandler(error))
    }
  },
  async fetchImages({ commit,state }) {
    try {

      const res = await this.$axios.$get(
        `${process.env.apiURL}/Search/Listings/Images/${state.property.id}`)

      commit('SET_PROPERTY_IMAGES', res)
      return Promise.resolve(res)
    } catch (error) {
      return Promise.reject(errorHandler(error))
    }
  },
  async getLookups({ commit, state }) {
    try {
      // check to see if lookups already loaded

      if (state.lookups.features.length == 0) {
        const res = await this.$axios.$get(
          `${process.env.apiURL}/Search/Lookups`
        )
        var payload = {
          features: [],
          propertyTypes: [],
        }

        res.features.map((x) => {
          var checkbox = {
            name: x,
            checked: false,
          }
          payload.features.push(checkbox)
        })

        res.propertyTypes.map((x) => {
          var checkbox = {
            name: x,
            checked: false,
          }
          payload.propertyTypes.push(checkbox)
        })

        commit('SET_LOOKUPS', payload)
        return Promise.resolve(payload)
      } else {
        return Promise.resolve(state.lookups)
      }
    } catch (error) {
      return Promise.reject(errorHandler(error))
    }
  },
}
