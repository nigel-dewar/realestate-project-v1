<template>
  <div class="pagination-wrapper" v-if="totalResultsCount > 0">
    <!-- <div>IsPageOne: {{ isPageOne }}</div>
    <div>availablePages: {{ availablePages }}</div>

    <div>inBetween: {{ inBetween }}</div>
    <div>currentPageNumber: {{ currentPageNumber }}</div>
    <div>nextPageNumber: {{ nextPageNumber }}</div> -->
    <span> {{start}} - {{end}} of {{totalResultsCount}} result(s)</span>

    <span v-if="availablePages">

      <!-- Left nav arrow button -->
      <span class="arrow-button" v-if="!isPageOne" @click="back()">
        <i class="fas fa-chevron-left"></i>
      </span>
      <span class="arrow-button-disabled" v-if="isPageOne">
        <i class="fas fa-chevron-left"></i>
      </span>
      <!-- Left nav arrow button -->

      <!-- first circle button -->
      <!-- <span class="pages-buttons"  @click="goToPage(0)" :class="{ 'active': currentPageNumber == 0 }">1</span> -->
      <span
        class="pages-buttons"
        @click="goToPage(1)"
        :class="{ active: isPageOne }"
        >1</span
      >

      <!-- middle buttons -->
      <!-- <span
        class="pages-buttons"
        v-if="!inBetween"
        @click="goToPage(nextPageNumber)"
        :class="{ active: inBetween }"
        >{{ nextPageNumber }} - not</span
      > -->

      <span
        class="pages-buttons"
        v-if="inBetween"
        :class="{ active: inBetween }"
        >{{ currentPageNumber }}</span
      >

      <!-- end circle button -->
      <span
        class="pages-buttons"
        v-if="availablePages"
        @click="goToPage(availablePages)"
        :class="{ active: currentPageNumber == availablePages }"
        >{{ availablePages }}</span
      >

      <span
        class="arrow-button"
        v-if="currentPageNumber != availablePages"
        @click="forward()"
      >
        <i class="fas fa-chevron-right"></i>
      </span>

      <span
        class="arrow-button-disabled"
        v-if="currentPageNumber == availablePages"
      >
        <i class="fas fa-chevron-right"></i>
      </span>
    </span>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  computed: {
    ...mapGetters({
      currentPageResultsCount: 'search/currentPageResultsCount',
      totalResultsCount: 'search/totalResultsCount',
      properties: 'search/properties',
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
    inBetween() {
      if (!this.isPageOne && !this.isLastPage) {
        return true
      } else {
        return false
      }
    },
    nextPageNumber() {
      if (this.isLastPage == true) {
        return this.currentPageNumber - 1
      } else if (this.isPageOne == true) {
        return this.currentPageNumber + 1
      } else {
        return 0
      }
    },
  },
  methods: {
    back(val) {
      this.$store.commit('search/SET_PAGE', this.currentPageNumber - 1)
      this.updateSearch()
      if (val) {
        this.scrollToTop()
      }
    },
    forward(val) {
      this.$store.commit('search/SET_PAGE', this.currentPageNumber + 1)
      this.updateSearch()
      if (val) {
        this.scrollToTop()
      }
    },
    updateSearch() {
      this.$store.dispatch('search/updateSearch')
    },
    goToPage(page, val) {
      this.$store.commit('search/SET_PAGE', page)

      this.updateSearch()
      if (val) {
        this.scrollToTop()
      }
    },
    scrollToTop() {
      window.scrollTo({
        top: 0,
        left: 0,
        // behavior: 'smooth'
      })
    },
  },
}
</script>

<style lang="scss" scoped>
.pagination-wrapper {
  .arrow-button {
    cursor: pointer;
    outline-width: 0;
    z-index: 0;
    border: 2px solid grey;
    padding: 10px;
    border-radius: 5px;
    margin: 10px 10px;
    &:hover {
      background-color: #d0d3d9;
    }
  }

  .arrow-button-disabled {
    cursor: default;
    outline-width: 0;
    z-index: 0;
    border: 2px solid #d0d3d9;
    padding: 10px;
    border-radius: 5px;
    margin: 10px 10px;
    color: #d0d3d9;
  }

  .pages-buttons {
    // font-size: .8rem;
    // font-weight: bold;
    display: inline-block;
    text-align: center;
    cursor: pointer;
    outline-width: 0;
    z-index: 0;
    border: 1px solid #d0d3d9;
    padding: 10px;
    border-radius: 50%;
    margin: 10px 10px;
    width: 50px;
    height: 50px;

    &:hover:not(.active) {
      background-color: #d0d3d9;
    }

    &.active {
      background-color: lightseagreen;
      color: white;
    }

  }

//   .pages-buttons:active {
//       color: red;
//   }
}
</style>
