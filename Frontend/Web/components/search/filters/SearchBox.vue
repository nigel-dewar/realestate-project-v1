<template>
  <div>

    <multiselect
      :value="selectedSuburbs"
      :options="searchedSuburbs"
      label="label"
      track-by="id"
      placeholder="Search Suburb or Post Code"
      :multiple="true"
      :loading="isLoading"
      :hide-selected="true"
      select-label="Select"
      :internal-search="false"
      :close-on-select="true"
      @search-change="asyncFind"
      @input="selectSuburb"
    >

    </multiselect>
    <!-- <multiselect
      v-model="selectedSuburbs"
      id="ajax"
      label="locality"
      track-by="id"
      placeholder="Suburb, region, postcode or address"
      open-direction="bottom"
      :options="searchedSuburbs"
      :multiple="true"
      :searchable="true"
      :loading="isLoading"
      :internal-search="false"
      :clear-on-select="true"
      :close-on-select="true"
      :options-limit="300"
      :limit="10"
      :limit-text="limitText"
      :max-height="600"
      :show-no-results="false"
      :hide-selected="true"
      @search-change="asyncFind"
    >
      <template slot="properties_tag" slot-scope="{ option, remove }">
        <span class="custom__tag">
          <div class="suburb-button">
            {{ option.locality }}
            <span class="custom__remove" @click="remove(option)">
              <button type="button" class="close" aria-label="Close">
                <span aria-hidden="true">&nbsp; <b>&times;</b></span>
              </button>
            </span>
          </div>
        </span>
      </template>
      <template slot="clear" slot-scope="props">
        <div
          class="multiselect__clear"
          v-if="selectedSuburbs.length"
          @mousedown.prevent.stop="clearAll(props.search)"
        ></div> </template
      ><span slot="noResult"
        >Oops! No elements found. Consider changing the search query.</span
      >
    </multiselect> -->
  </div>
</template>

<script>
import Multiselect from 'vue-multiselect'
import { mapGetters } from 'vuex'
export default {
  name: 'search-box',
  components: {
    Multiselect,
  },
  data() {
    return {
      isLoading: false,
      // selectedSuburbs: [],
      value: '',
      option: '',
      options: [],
    }
  },
  computed: {
    ...mapGetters({
      searchedSuburbs: 'search/searchedSuburbs',
      selectedSuburbs: 'search/selectedSuburbs'
    }),
    queryString() {
      return this.$route.query
    },
  },
  watch: {
    // whenever changes, this function will run
    selectedSuburbs: function (newValue, oldValue) {
      this.$store
        .dispatch('search/updateSelectedSuburbs', newValue)
        .then(() => {
          // this.$store.dispatch("updateQueryString", "suburbs", )
        })
    },
  },
  mounted() {
    this.setSearchBox()
  },
  methods: {
    updateQueryString() {},
    selectSuburb(val) {
      this.$store
        .dispatch('search/updateSelectedSuburbs', val)
        .then(() => {
          // this.$store.dispatch("updateQueryString", "suburbs", )
        })
    },
    setSearchBox() {
      // this runs on first time loadup
      // if (this.filteredSuburbs.length != 0) {
      //   this.selectedSuburbs = this.filteredSuburbs
      // } else {
      //   // we have no data so we need to check query string and see if there is anything on it for suburbs

      //   let query = Object.assign({}, this.$route.query)

      //   let split = query.suburbs ? query.suburbs.split('|') : []

      //   if (split.length != 0) {
      //     this.$store
      //       .dispatch('properties/fetchSuburbBySlug', query.suburbs)
      //       .then(() => {
      //         this.selectedSuburbs = this.filteredSuburbs
      //       })
      //   }
      // }
    },
    asyncFind(query) {
      if (query.length >= 3) {
        this.isLoading = true
        this.$store
          .dispatch('search/searchSuburbs', query)
          .then(() => {
            this.isLoading = false
          })
      }
      this.isLoading = false
    },
    remove(item) {
      let suburbs = ''

      this.selectedSuburbs.forEach(function (value, idx, array) {
        if (idx === array.length - 1) {
          suburbs += value.suburb
        } else {
          suburbs += value.suburb + '|'
        }
      })

      // this.filter(suburbs);
    },
    limitText(count) {
      return `and ${count} other suburbs`
    },
    clearAll() {
      this.selectedSuburbs = []
    },
  },
}
</script>

<style src="vue-multiselect/dist/vue-multiselect.min.css"></style>
