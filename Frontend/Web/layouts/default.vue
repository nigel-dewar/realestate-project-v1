<template>
  <div>
    <div class="filters" v-if="showFilters">
      <div class="filterbox">
        <FilterBox />
      </div>
    </div>
    <div class="images" v-if="showImages">
      <ImagesBox />
    </div>
    <navbar />
    <nuxt />
  </div>

</template>

<script>
import Navbar from '~/components/shared/Navbar'
import FilterBox from '~/components/search/filters/FilterBox'
import ImagesBox from '~/components/search/detail/ImagesBox'
import {mapGetters, mapMutations} from 'vuex'
export default {
  head: {
    title: 'Realestateify',
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      {
        hid: 'RealEstateify',
        name: 'RealEstateify',
        content: 'RealEstateify'
      }
    ],
    link: [{ rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }]
  },
  name: 'Default',
  components: {
    Navbar,
    FilterBox,
    ImagesBox
  },
  computed: {
    ...mapGetters({
      showFilters: 'search/showFilters',
      showImages: 'search/showImages'
    }),
  }
}
</script>

<style lang="scss">

.images {
  position: absolute;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  // width: 100%;
  // height: 100%;
  z-index: 1000;
  background-color: white;
}

.filters {
  position: absolute;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  // width: 100%;
  // height: 100%;
  z-index: 1000;
  background-color: rgba(0,0,0,0.8);

  .filterbox {
    position: relative;
    margin: auto auto;
    min-width: 90%;
    max-width: 500px;
    top: 2rem;
  }
}


</style>
