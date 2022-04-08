import Vue from 'vue'

import * as VueGoogleMaps from 'vue2-google-maps-withscopedautocomp'

Vue.use(VueGoogleMaps, {
    load: {
        key: 'AIzaSyAfgtEYfYQgItIvFwzfG4rxk9U6hpgwMTk',
        libraries: 'places'
    }
})