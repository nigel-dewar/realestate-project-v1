<template>
  <div class="container m-t-md p-r-sm p-l-sm">
    <!-- <SearchBar /> -->
    
    <div class="columns">
      <div class="column is-3">
        <!-- <FilterBox /> -->
      </div>
      <div class="column is-6 results-container">
        <div class="results-padding">
          <div class="is-clearfix m-t-sm m-b-sm">
            <button class="button is-success is-pulled-right" @click="showFilters">Filter<i class="fas fa-sliders-h m-l-sm"></i></button>
            <ResultsInfo />
          </div>
          <div v-for="property in properties" :key="property.id">
            <ListingCard :listing="property" />
          </div>
          <div>
            <PaginationControls />
          </div>
        </div>
      </div>
      <div class="column is-3">
        <!-- <FilterBox /> -->
        <!-- Ads space -->
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import ResultsInfo from '~/components/search/results/ResultsInfo'
import PaginationControls from '~/components/search/results/PaginationControls'
import ListingCard from '~/components/search/results/ListingCard'
import SearchBar from '~/components/search/filters/SearchBar'
export default {
  watchQuery: ['page','mode','suburbs','propertyTypes','features','price','bedRooms','bathRooms','parking'],
  data() {
    return {}
  },
  components: {
    PaginationControls, ListingCard, SearchBar
  },
  computed: {
    ...mapGetters({
      properties: 'search/properties',
    }),
  },
  mounted() {
    window.scrollTo({
        top: 0,
        left: 0,
        // behavior: 'smooth'
      })
  },
  async fetch(context) {
    // console.log(context.query.page)
    // console.log(context.store.state.search.currentPageNumber)
    // if(context.store.state.search.currentPageNumber == context.query.page) {
    //   console.log('were on same page')
    // } else {
      
    // }

    const results = await context.$axios.$post(
      `${process.env.apiURL}/Search/Listings/Find`,
        context.query
      )
      console.log(results)
      context.store.dispatch('search/hyrdateStore', context.query)
      context.store.dispatch('search/updateSearchResults', results)
    
  },
  methods: {
    showFilters() {
      this.$store.commit('search/SET_SHOW_FILTERS')
    }
  },
}
</script>

<style lang="scss" scoped>
.results-padding {
  // padding: 1.2rem;
  // width: 70%;
}

.results-container {
  background-color: white;
  .results {
  }
}
</style>
