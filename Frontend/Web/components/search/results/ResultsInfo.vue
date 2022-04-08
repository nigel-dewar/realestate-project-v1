<template>
  <div>
    <p class="is-size-6" >For {{mode}}<span v-if="firstSuburb"> in {{firstSuburb}}</span> <span v-if="suburbsCount > 1"> and other suburbs</span></p>
    <div> <span v-if="suburbsCount > 1">{{start}}-{{end}} of</span> {{totalResultsCount}} result(s)</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  computed: {
    ...mapGetters({
      totalResultsCount: 'search/totalResultsCount',
      firstSuburb: 'search/suburbsFormattedFirst',
      suburbsCount: 'search/suburbsCount',
      mode: 'search/mode',
      availablePages: 'search/availablePages',
      currentPageNumber: 'search/currentPageNumber',
    }),
    start() {
      if (this.isPageOne){
        return 1
      } else {
        return (this.currentPageNumber - 1) * 10 + 1
      }
    },
    end() {
      if (this.isLastPage) {
        return this.totalResultsCount
      } else {
        return this.currentPageNumber == 1 ? 10 : (this.currentPageNumber * 10) + 10
      }
      
    },
    isPageOne() {
      if (this.currentPageNumber == 0 || this.currentPageNumber == 1) {
        return true
      } else {
        return false
      }
    },
    isLastPage() {
      if (this.availablePages == this.currentPageNumber) {
        return true
      } else {
        return false
      }
    },
  }
}
</script>

<style lang="scss" scoped>
</style>
