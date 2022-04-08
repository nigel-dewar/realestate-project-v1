<template>
  <client-only>
    <gmap-autocomplete
      @place_changed="handleChange"
      :selectFirstOnEnter="true"
      :options="autocompleteOptions"
    >
      <template v-slot:input="slotProps">
        <input
          class="input is-medium is-size-6-mobile is-primary"
          :value="formatted_address"
          prepend-inner-icon="place"
          :placeholder="placeHolder"
          ref="input"
          v-on:listeners="slotProps.listeners"
          v-on:attrs="slotProps.attrs"
          autofocus
        />
      </template>
    </gmap-autocomplete>
  </client-only>
</template>

<script>
export default {
  props: {
    placeHolder: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      autocompleteOptions: {
        componentRestrictions: {
          country: ['au', 'nz'],
        },
      },
      formatted_address: '',
    }
  },
  methods: {

    reset() {

    },
   
    handleChange(place) {
      if (!place || place == undefined || !place.geometry) {
        this.$emit('no-results-found', place)
        return false
      }

      if (place.address_components != undefined) {
        let returnData = {}

        returnData['latitude'] = place.geometry.location.lat()
        returnData['longitude'] = place.geometry.location.lng()

        returnData['googleAddress'] = place.formatted_address

        for (const component of place.address_components) {
          for (const value of component.types) {
            if (value == 'street_number') {
              returnData['streetNumber'] = component.long_name
            }
            if (value == 'route') {
              returnData['streetName'] = component.long_name
            }
            if (value == 'postal_code') {
              returnData['postalCode'] = component.long_name
            }
            if (value == 'locality') {
              returnData['suburbOrCity'] = component.long_name
            }
            if (value == 'administrative_area_level_2') {
              returnData['council'] = component.long_name
            }
            if (value == 'administrative_area_level_1') {
              returnData['regionOrState'] = component.long_name
            }
            if (value == 'country') {
              returnData['country'] = component.long_name
            }
          }
        }

        returnData['created-with-gmaps'] = true

        this.$emit('address-response', returnData)
      } else {
        // just in case
        this.$emit('no-results-found', place)
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.pac-container {
  //this is a fix for google autocomplete not showing up
  z-index: 10000 !important;
  display: block !important;
}
</style>
