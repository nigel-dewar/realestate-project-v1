import api from '../api/api';

const state = () => ({
    filtersLoaded: false,
    turnOffSwitch: false,
    prices: {
        minPrice: "150000",
        maxPrice: "10000000"
    },
    filters: {
        propertyTypes: [],
        features: [],
        bedRooms: [],
        bathRooms: [],
        parking: []
    },
    selectedFilters: {
        propertyTypes: [],
        features: [],
        bedRooms: [],
        bathRooms: [],
        parking: []
    },
    mode: "",
    queryString: {}
});

const getters = {
    allFilters: state => state.filters,
    selectedFilters: state => state.selectedFilters,
    prices: state => state.prices,
    getMode: state => state.mode,
    queryString: state => state.queryString,
    selectedPropertyTypes: state => state.selectedFilters.propertyTypes,
    selectedFeatureTypes: state => state.selectedFilters.features,
    turnOffSwitch: state => state.turnOffSwitch
    
};

const actions = {
    
    async fetchFilters({ rootState, commit, state }, query) {

        // This is run on a first time load up
        // It will check the query string and it will update the vuex store with these values
        // We do this because the first time load up might be a direct url with params - loaded from an email or shared link etc
        let split = [];
        let params = {}
    
        for (let [key, value] of Object.entries(query)) {
            split = query[key] ? query[key].split("|") : [];
            params[key] = split;
        }
        
        
        const filters = {
            propertyTypes: [],
            features: []
        }

        const selectedFilters = {
            propertyTypes: [],
            features: []
        }

        let data = null;
        // check to see if we already have filter data - if not - get it
        if (state.filtersLoaded == false){

            let response = await api.fetchFilters(); 
            data = response.data;
            
            prepCheckBoxes('propertyTypes', data);
            prepCheckBoxes('features', data);

            commit("setLoaded");
        } else {
            prepCheckBoxes('propertyTypes', null);
            prepCheckBoxes('features', null);
        }

        // runs only for the checkbox filters that come from the database
        function prepCheckBoxes(key, data){
            if (data != null){
                data[key].forEach(item => {
                    let status = false;
                    if (params[key]){
                        const foundItem = params[key].find(a => a === item);
                        if (foundItem) selectedFilters[key].push(foundItem);
                        status = !!params[key].find(a => a === item);
                    }
                    let s = {"name": item, "checked": status};
                    filters[key].push(s);
                });
                commit('setFilters', filters);
            } else {
                // just adjust the checkboxes - not reset them
                state.filters[key].forEach(item => {
                    let status = false;
                    if (params[key]){
                        const foundItem = params[key].find(a => a === item);
                        if (foundItem) {
                            selectedFilters[key].push(foundItem);
                            status = !!params[key].find(a => a === item);
                            foundItem.key = key;
                            updateFilter(s);
                        }     
                    }
                });
                
            }
            
        };

        commit('setSelectedFilters', selectedFilters);

        // set price boxes
        if (params.minPrice){
            let minPrice = { "name": "minPrice", "value": params.minPrice[0] };
            commit('setPrices', minPrice);
        }
        
        if (params.maxPrice){
            let maxPrice = { "name": "maxPrice", "value": params.maxPrice[0] };
            commit('setPrices', maxPrice);
        }

        if (params.bedRooms){
            commit("setBedrooms", params.bedRooms);
        } else {
            commit("setBedrooms", ["0","5+"]);
        }

        if (params.bathRooms){
            commit("setBathrooms", params.bathRooms);
        } else {
            commit("setBathrooms", ["0","5+"]);
        }

        if (params.parking){
            commit("setParking", params.parking);
        } else {
            commit("setParking", ["0","5+"]);
        }

        if (params.page) {
            commit("setPage", params.page);
        }
        
    },
    turnOffSwitch({state, commit}, data){
        commit('setTurnOffSwitch', data);
    },
    updateQueryString({rootState, commit}, data) {
        commit('setQueryString', data);
    },
    updateFilter({commit}, data) {
        
        const index = state.selectedFilters[data.key].findIndex(
            i => i == data.name
        );

        if (index >= 0) {
            data.index = index;
            commit("removeFromSelectedFilter", data);
        } else {
            commit("addToSelectedFilter", data);
        }
    },
    updateBedrooms({commit}, data) {
        commit("setBedrooms", data);
    },
    updateBathrooms({commit}, data) {
        commit("setBathrooms", data);
    },
    updateParking({commit}, data) {
        commit("setParking", data);
    },
    updatePropertyType({rootState, commit}, data){
        const index = state.propertyTypes.findIndex(
            i => i == data
        );
        if (index >= 0) {
            commit("removePropertyType", index);

          //  commit("updateProductQuantity", index);
        } else {
            commit("addPropertyType", data);
            // commit("addProductToCart", product);
        }
    },
    calcQueryString({commit}){
        // loop through all filters, selected suburbs, and formulate a new querystring
        for (let [key] of Object.entries(state.selectedFilters)) {
            let query = "";
            state.selectedFilters[key].forEach(function (value, idx, array){

                if (value === "propertyTypes" || "features" || "bedRooms"){
                    if (idx === array.length -1){
                        query += value;
                    } else {
                        query += value + "|";
                    }
                }

            });

            if (query != ""){
                state.queryString[key] = query;
            } else {
               delete state.queryString[key];
            }
        }

        // set price data on query string
        let min = { 'key': 'minPrice', 'query': state.prices.minPrice };
        commit("setQueryString", min);

        let max = { 'key': 'maxPrice', 'query': state.prices.maxPrice };
        commit("setQueryString", max);
        // state.queryString.priceMin = state.prices.priceMin;
        // state.queryString.priceMax = state.prices.priceMax;
        

    },
    updatePrices({commit}, data){
        commit('setPrices', data);
    },
    updateMode({commit}, data){
        commit('setMode', data);
        commit('setPage', 0);
    },
    updatePage({commit}, data){
        commit('setPage', data);
    }

};

const mutations = {
    setLoaded(state){
        state.loaded = true;
    },
    setFilters: (state, filters) => {
        state.filters = filters;
    },
    setFilter: (state, data) => {
        state.filters[data.key] = data.payload
    },
    setSelectedFilters(state, filters){
        state.selectedFilters = filters;
    },
    addToSelectedFilter(state, data){
        state.selectedFilters[data.key].push(data.name);
    },
    removeFromSelectedFilter(state, data){
        state.selectedFilters[data.key].splice(data.index, 1);
    },
    setMode: (state, mode) => {
        state.mode = mode;
        state.queryString['mode'] = mode;
    },
    setQueryString: (state, data) => {
        state.queryString[data.key] = data.query;
    },
    setPrices: (state, data) => {
        state.prices[data.name] = data.value;
    },
    setBedrooms: (state, data) => {
        state.selectedFilters.bedRooms = data;
    },
    setBathrooms: (state, data) => {
        state.selectedFilters.bathRooms = data;
    },
    setParking: (state, data) => {
        state.selectedFilters.parking = data;
    },
    setPage: (state, data) => {
        state.queryString['page'] = data;
        // rootState.currentPageNumber = 0;
    },
    setTurnOffSwitch: (state, data) => {
        state.turnOffSwitch = data;
    }
};

export default {
    state,
    getters,
    actions,
    mutations
}