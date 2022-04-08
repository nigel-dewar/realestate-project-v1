<template>
  <div class="search">
    <div class="search-mode">
      <div class="search-controls">
        <div class="filter-radios">
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
        <div class="filter-button">
          <button class="button is-primary" @click="showFilters">Filters<i class="fas fa-sliders-h m-l-sm"></i></button>
        </div>
      </div>
    </div>

    <div class="search-box-container">
      <SearchBox class="search-box" />
      <button class="search-button button is-success m-l-sm" @click="search">
        <i class="fas fa-1x fa-search"></i>
      </button>
    </div>

    <!-- <div class="recent-searches">
      <div class="recent-searches__caption">
        <svg viewBox="0 0 24 24" class="recent-searches__icon">
          <path
            fill="none"
            stroke="currentColor"
            stroke-width="2"
            d="M17 4v4m4-4h-4m.5.9A9 9 0 1 1 12 3l1.8.2"
          />
          <path
            fill="none"
            stroke="currentColor"
            d="M3.5 12.5h2m13 0h2m-9 7.5v-1.5m0-13V3m0 5.5V12m2 1.5l-2-1.5"
          />
        </svg>
        RECENT SEARCHES
      </div>
      <div class="recent-searches__results">
        <a
          class="recent-searches__clickable-div"
          @click="goToSearch('runcorn-4113')"
        >
          <div class="recent-searches__suburbs">Runcorn</div>
          <div class="recent-searches__description-line">All Listings</div>
        </a>
        <a
          class="recent-searches__clickable-div"
          @click="goToSearch('sunnybank-hills-4109%7Cholland-park-west-4121')"
        >
          <div class="recent-searches__suburbs">
            Sunnybank Hills, Holland Pa..
          </div>
          <div class="recent-searches__description-line">All Listings</div>
        </a>
        <a
          class="recent-searches__clickable-div"
          @click="goToSearch('indooroopilly-4068')"
        >
          <div class="recent-searches__suburbs">Indooroopilly</div>
          <div class="recent-searches__description-line">All Listings</div>
        </a>
      </div>
    </div> -->
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import SearchBox from './filters/SearchBox'
export default {
  data() {
    return {
      listingType: 'Sale'
    }
  },
  components: {
    SearchBox,
  },
  methods: {
    async search() {
      try {
        const res = await this.$store.dispatch('search/updateSearch')
        console.log('search triggered')
      } catch (error) {}
    },
    updateListingType() {
      this.$store.dispatch('search/updateListingType', this.listingType)
    },
    showFilters() {
      this.$store.commit('search/SET_SHOW_FILTERS')
    }
  },
  computed: {
    ...mapGetters({
      searchedSuburbs: 'search/searchedSuburbs',
      suburbs: 'search/selectedSuburbs',
    }),
  },
}
</script>

<style lang="scss" scoped>

.search {
  background-color: white;
  padding: 10px;
}

.search-controls {
  display: flex;
  align-items: center;
}

.filter-radios {
  display: block;
}

.filter-button {
  display: block;
  margin-left: auto
}

.search-mode {
  margin-top: .5rem;
  margin-bottom: 1rem;
}
.search-box-container {
  display: flex;
  // border: 1px solid red;
  .search-box {
    flex: 1;
  }

  .search-button {
    min-height: 42px;
  }
}
</style>
