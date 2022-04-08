import api from '../api/api';

const state = () => ({
    count: 0,
    availablePages: 0,
    currentPageNumber: 0,
    properties: [],
    property: {},
    propertyImages: [],
    propertyImageForThumbnail: {},
    searchedSuburbs: [],
    selectedSuburbs: [],
    currentMode: {}
});

const getters = {
    count: state => state.count,
    availablePages: state => state.availablePages,
    currentPageNumber: state => state.currentPageNumber,
    sortedProperties: state => state.properties,
    searchedSuburbs: state => state.searchedSuburbs,
    selectedSuburbs: state => state.selectedSuburbs,
    property: state => state.property,
    propertyImages: state => state.propertyImages,
    propertyImageForThumbnail: state => state.propertyImageForThumbnail,
    currentMode: state => {
        if (state.currentMode == 'rent')
            return 'Rent'
        if (state.currentMode == 'buy')
            return 'Sale'
    }
};

const actions = {

    
    async getPropertyImageForThumbnail({ rootState, commit }, query) {
        const response = await api.fetchPropertyImageForThumbnail(query);
        commit('setPropertyImageForThumbnail', response.data);
    },
    async getPropertyImages({ rootState, commit }, query) {
        const response = await api.fetchPropertyImages(query);
        commit('setPropertyImages', response.data);
    },
    updateProperty({state, commit}, property){
        commit('setProperty', property);
    },
    async getPropertyDetails({ commit }, query) {
        // check the store to see if we already have this properties info loaded into memory
        const response = await api.fetchProperty(query);
        commit('setProperty', response.data);
    },
    async getPropertyDescription({ commit }, query) {
        // check the store to see if we already have this properties info loaded into memory
        const response = await api.fetchPropertyDescription(query);
        commit('setPropertyDescription', response.data);
    },
    async fetchProperties({ rootState, commit }, query) {
        const response = await api.fetchProperties(query);
        commit('setProperties', response.data);
        commit('setCurrentMode', query.mode);
    },
    async searchSuburbs({ rootState, commit }, query) {
        const response = await api.searchSuburbs(query);
        commit('setSearchedSuburbs', response.data);
    },
    async updateSelectedSuburbs({commit, dispatch}, data){

        commit('setSelectedSuburbs', data);
        let queryData = "";

        data.forEach(function (value, idx, array){

            if (idx === array.length -1){
                queryData += value.suburb;
            } else {
                queryData += value.suburb + "|";
            }
            
        });

        let queryPacket = {
            key: 'suburbs',
            query: queryData
        };
        dispatch('updateQueryString', queryPacket)

    },
    async fetchSuburbBySlug({commit}, query){
        const response = await api.fetchSuburbBySlug(query);
        commit('setSelectedSuburbs', response.data);
    }

    
};

const mutations = {
    setProperty: (state, property) => {
        state.property = property;
    },
    setProperties: (state, data) => {
        state.properties = data.properties;
        if (data.total){
            state.count = data.total;
            state.availablePages = data.availablePages;
            state.currentPageNumber = data.currentPageNumber;
        } else {
            state.count = 0;
            state.availablePages = undefined;
            state.currentPageNumber = undefined;
        }
        
    },
    setSearchedSuburbs: (state, suburbs) => {
        state.searchedSuburbs = suburbs;
    },
    setSelectedSuburbs: (state, suburbs) => {
        state.selectedSuburbs = suburbs;
        
    },
    setPropertyImages: (state, data) => {
        data.forEach((img) => {
            img.active = false
        });
        state.propertyImages = data;
    },
    setPropertyImageForThumbnail: (state, data) => {
        state.propertyImageForThumbnail = data;
    },
    setCurrentMode: (state, data) => {
        state.currentMode = data;
    },
    setPropertyDescription: (state, data) => {
        state.property.description = data;
    }

    
    
};

export default {
    state,
    getters,
    actions,
    mutations
}