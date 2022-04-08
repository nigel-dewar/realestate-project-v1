import axios from 'axios';

const ROOT_URL = 'https://localhost:5001';
// const ROOT_URL = 'http://realestatedemoapp.azurewebsites.net/data';

export const fetchFilters = () => {
    return axios.get(`${ROOT_URL}/api/filters`, {
        headers: {
        }
    });
};

export const fetchProperty = (slugAndListingId) => {

    return axios.get(`${ROOT_URL}/api/properties/GetProperty/${slugAndListingId}`, {
    });
};

export const fetchPropertyDescription = (slug) => {

    return axios.get(`${ROOT_URL}/api/properties/GetPropertyDesc/${slug}`, {
    });
};



export const fetchPropertyImages = (propertyId) => {
    return axios.get(`${ROOT_URL}/api/properties/GetPropertyImages/${propertyId}`, { 
    });
};

export const fetchPropertyImageForThumbnail = (thumbnailId) => {
    return axios.get(`${ROOT_URL}/api/properties/GetPropertyImage/${thumbnailId}`, { 
    });
};



export const fetchProperties = (query) => {
    return axios.get(`${ROOT_URL}/api/properties`, { 
        params: query,
        headers: {

        }
    });
    // return axios.get(`${ROOT_URL}/api/properties`, { params: query }).then(response => {
    // });
};

export const searchSuburbs = (query) => {

    var queryBody = {
        "query": query
    }

    return axios.post(`${ROOT_URL}/api/postcodes/FindByAny`, queryBody);
    // return axios.post(`${ROOT_URL}/api/postcodes/search`, query)
    // .then(response => {
    //     resolve(response.data);     
    // })
    // .catch(error => {
    //     
    //     //reject(error.response);
    // });
};

export const fetchSuburbBySlug = (query) => {
    
    var queryBody = {
        "query": query
    }
    return axios.post(`${ROOT_URL}/api/postcodes/FindBySlug`, queryBody);
};


export default {
    fetchFilters,
    fetchProperties,
    fetchProperty,
    fetchPropertyDescription,
    searchSuburbs,
    fetchSuburbBySlug,
    fetchPropertyImages,
    fetchPropertyImageForThumbnail
}