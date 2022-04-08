<template>
  <div class="filterbox-container" v-if="loaded">
    <div class="filterbox">
      <div class="listingType filterbox-section">
        <div class="level is-mobile">
          <div class="level-left">
            <b-radio
              v-model="listingType"
              @input="updateListingType"
              name="name"
              value="Sale"
              native-value="Sale"
            >
              Sale
            </b-radio>
            <b-radio
              v-model="listingType"
              @input="updateListingType"
              name="name"
              value="Rent"
              native-value="Rent"
            >
              Rent
            </b-radio>
          </div>
          <div class="level-right">
            <button class="button is-primary m-r-sm m-l-sm" @click="triggerFilters">
              back
            </button>
            <button class="button is-success" @click="update">Update</button>
          </div>
        </div>
      </div>
      <div class="search-box filterbox-section">
        <SearchBox />
      </div>
      <div class="property-types filterbox-section">
        Property Types:
        <div class="block">
          <b-checkbox
            v-for="(item, index) in this.propertyTypes"
            :key="index"
            :id="item.name"
            v-model="item.checked"
            class="space"
            @input="checkPropertyType(item)"
          >
            {{ item.name }}
          </b-checkbox>
        </div>
      </div>
      <div class="prices filterbox-section">
        <div class="columns">
          <div class="column">
            <b-field label="Min Price" :label-position="labelPosition">
              <b-select
                placeholder="Select Min Price"
                expanded
                size="is-medium"
                v-model="minPrice"
                @input="updateMinPrice"
              >
                <option
                  v-for="option in priceOptions"
                  :value="option.value"
                  :key="option.id"
                >
                  {{ option.label }}
                </option>
              </b-select>
            </b-field>
          </div>
          <div class="column">
            <b-field label="Max Price" :label-position="labelPosition">
              <b-select
                placeholder="Select Max Price"
                expanded
                size="is-medium"
                v-model="maxPrice"
                @input="updateMaxPrice"
              >
                <option
                  v-for="option in priceOptions"
                  :value="option.value"
                  :key="option.id"
                >
                  {{ option.label }}
                </option>
              </b-select>
            </b-field>
          </div>
        </div>
      </div>
      <div class="beds filterbox-section">
        <div class="columns">
          <div class="column">
            <b-field label="Min Beds" :label-position="labelPosition">
              <b-select
                placeholder="Select Min Beds"
                expanded
                size="is-medium"
                v-model="minBeds"
                @input="updateMinBeds"
              >
                <option
                  v-for="option in bedOptions"
                  :value="option"
                  :key="option"
                >
                  {{ option }}
                </option>
              </b-select>
            </b-field>
          </div>
          <div class="column">
            <b-field label="Max Beds" :label-position="labelPosition">
              <b-select
                placeholder="Select Max Beds"
                expanded
                size="is-medium"
                v-model="maxBeds"
                @input="updateMaxBeds"
              >
                <option
                  v-for="option in bedOptions"
                  :value="option"
                  :key="option"
                >
                  {{ option }}
                </option>
              </b-select>
            </b-field>
          </div>
        </div>
      </div>
      <div class="bathrooms filterbox-section">
        <div class="columns">
          <div class="column">
            <b-field label="Min Bathrooms" :label-position="labelPosition">
              <b-select
                placeholder="Select Min Bathrooms"
                expanded
                size="is-medium"
                v-model="minBathrooms"
                @input="updateMinBathrooms"
              >
                <option
                  v-for="option in bathroomOptions"
                  :value="option"
                  :key="option"
                >
                  {{ option }}
                </option>
              </b-select>
            </b-field>
          </div>
          <div class="column">
            <b-field label="Max Bathrooms" :label-position="labelPosition">
              <b-select
                placeholder="Select Max Bathrooms"
                expanded
                size="is-medium"
                v-model="maxBathrooms"
                @input="updateMaxBathrooms"
              >
                <option
                  v-for="option in bathroomOptions"
                  :value="option"
                  :key="option"
                >
                  {{ option }}
                </option>
              </b-select>
            </b-field>
          </div>
        </div>
      </div>
      <div class="parkings filterbox-section">
        <div class="columns">
          <div class="column">
            <b-field label="Min Parking Spaces" :label-position="labelPosition">
              <b-select
                placeholder="Select Min Parking Spaces"
                expanded
                size="is-medium"
                v-model="minParkingSpaces"
                @input="updateMinParkingSpaces"
              >
                <option
                  v-for="option in parkingSpacesOptions"
                  :value="option"
                  :key="option"
                >
                  {{ option }}
                </option>
              </b-select>
            </b-field>
          </div>
          <div class="column">
            <b-field label="Max Parking Spaces" :label-position="labelPosition">
              <b-select
                placeholder="Select Max Parking Spaces"
                expanded
                size="is-medium"
                v-model="maxParkingSpaces"
                @input="updateMaxParkingSpaces"
              >
                <option
                  v-for="option in parkingSpacesOptions"
                  :value="option"
                  :key="option"
                >
                  {{ option }}
                </option>
              </b-select>
            </b-field>
          </div>
        </div>
      </div>
      <div class="features filterbox-section">
        Features:
        <div class="block">
          <b-checkbox
            v-for="(item, index) in this.features"
            :key="index"
            :id="item.name"
            v-model="item.checked"
            class="space"
            @input="checkFeature(item)"
          >
            {{ item.name }}
          </b-checkbox>
        </div>
      </div>
      <div class="is-clearfix">
        <button
          class="button is-success is-pulled-right m-r-sm m-l-sm"
          @click="update"
        >
          Update
        </button>
        <button
          class="button is-primary is-pulled-right"
          @click="triggerFilters"
        >
          back
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'
import SearchBox from '~/components/search/filters/SearchBox'
export default {
  data() {
    return {
      loaded: false,
      listingType: 'Sale',
      features: [],
      propertyTypes: [],
      labelPosition: 'on-border',
      priceOptions: [],
      minPrice: 0,
      maxPrice: 0,
      bedOptions: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
      minBeds: 0,
      maxBeds: 0,
      bathroomOptions: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
      minBathrooms: 0,
      maxBathrooms: 0,
      parkingSpacesOptions: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
      minParkingSpaces: 0,
      maxParkingSpaces: 0,
    }
  },
  components: {
    SearchBox,
  },
  computed: {
    ...mapGetters({
      lookups: 'search/lookups',
      selectedFeatures: 'search/selectedFeatures',
      selectedPropertyTypes: 'search/selectedPropertyTypes',
      rentPrices: 'search/rentPrices',
      buyPrices: 'search/buyPrices',
    }),
    ...mapState({
      prices: (state) => state.search.price,
      parkingSpaces: (state) => state.search.parking,
      beds: (state) => state.search.bedRooms,
      bathrooms: (state) => state.search.bathRooms,
      mode: (state) => state.search.mode,
    }),

    checkedFeatures() {
      return this.features.filter((x) => x.checked == true)
    },
    checkedPropertyTypes() {
      return this.propertyTypes.filter((x) => x.checked == true)
    },
  },
  methods: {
    switchPriceTypes() {
      if (this.listingType == 'Sale') {
        this.priceOptions = this.buyPrices
      } else if (this.listingType == 'Rent') {
        this.priceOptions = this.rentPrices
      }

      this.minPrice = this.prices[0]
      this.maxPrice = this.prices[1]
    },
    updateListingType() {
      this.$store.dispatch('search/updateListingType', this.listingType)
      this.switchPriceTypes()
    },
    ...mapMutations({
      triggerFilters: 'search/SET_SHOW_FILTERS', // map `this.add()` to `this.$store.commit('increment')`
    }),
    updateMinPrice() {
      this.$store.commit('search/SET_MIN_PRICE', this.minPrice)
    },
    updateMaxPrice() {
      this.$store.commit('search/SET_MAX_PRICE', this.maxPrice)
    },
    updateMinBeds() {
      this.$store.commit('search/SET_MIN_BEDS', this.minBeds)
    },
    updateMaxBeds() {
      this.$store.commit('search/SET_MAX_BEDS', this.maxBeds)
    },
    updateMinBathrooms() {
      this.$store.commit('search/SET_MIN_BATHROOMS', this.minBathrooms)
    },
    updateMaxBathrooms() {
      this.$store.commit('search/SET_MAX_BATHROOMS', this.maxBathrooms)
    },
    updateMinParkingSpaces() {
      this.$store.commit('search/SET_MIN_PARKING_SPACES', this.minParkingSpaces)
    },
    updateMaxParkingSpaces() {
      this.$store.commit('search/SET_MAX_PARKING_SPACES', this.maxParkingSpaces)
    },
    checkFeature(item) {
      this.$store.commit('search/SET_FEATURES', item)
    },
    checkPropertyType(item) {
      this.$store.commit('search/SET_PROPERTY_TYPES', item)
    },

    update() {
      this.$store.dispatch('search/updateSearch')
      this.triggerFilters()
    },
    async resetFilters() {
      try {
        this.listingType = 'Sale'
        this.features = []
        this.propertyTypes = []
        this.minPrice = 0
        this.maxPrice = 1000000
        this.minBeds = 0
        this.maxBeds = 10
        this.minBathrooms = 0
        this.maxBathrooms = 10
        this.minParkingSpaces = 0
        this.maxParkingSpaces = 10

        this.mountOptions()
      } catch (error) {}
    },
    mountOptions() {
      this.listingType = this.mode

      this.lookups.features.map((x) => {
        var checkbox = {
          name: x.name,
          checked: x.checked,
        }
        this.features.push(checkbox)
      })

      this.lookups.propertyTypes.map((x) => {
        var checkbox = {
          name: x.name,
          checked: x.checked,
        }
        this.propertyTypes.push(checkbox)
      })

      this.switchPriceTypes()

      this.minBeds = this.beds[0]
      this.maxBeds = this.beds[1]

      this.minBathrooms = this.bathrooms[0]
      this.maxBathrooms = this.bathrooms[1]

      this.minParkingSpaces = this.parkingSpaces[0]
      this.maxParkingSpaces = this.parkingSpaces[1]

      this.loaded = true
    },
  },
  async mounted() {
    await this.$store.dispatch('search/getLookups')

    this.mountOptions()
  },
  watch: {
    // features: function (val) {
    //   debugger
    //   this.$store.commit('search/SET_FEATURES', val)
    // },
    // checkedPropertyTypes: function (val) {
    //   this.$store.commit('search/SET_PROPERTY_TYPES', val)
    // },
  },
}
</script>

<style lang="scss" scoped>
.filterbox-container {
  z-index: 2000;
  opacity: 100;
}

.filterbox {
  background-color: white;
  padding: 1rem;

  &-section {
    margin-bottom: 20px;
  }

  .property-types {
  }

  .features {
  }

  .search-box {
  }

  .beds {
  }

  .bathRooms {
  }

  .parkings {
  }
}

.space {
  max-width: 17rem;
  min-width: 17rem;
}
</style>
