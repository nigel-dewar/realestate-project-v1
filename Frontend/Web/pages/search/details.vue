<template>
  <div class="container m-t-lg">
    <div>
      
    </div>
    <div class="columns">
      <div class="column is-2"></div>
      <div class="column">
        <BreadCrumbs :listing="property"/>
        <ListingDetail :listing="property" /></div>
      <div class="column is-2"></div>
    </div>
    
    <!-- {{property}} -->
  </div>
</template>

<script>
import BreadCrumbs from '~/components/search/BreadCrumbs'
import ListingDetail from '~/components/search/detail/ListingDetail'
import { mapGetters } from 'vuex'
export default {
  components: {
    BreadCrumbs,
    ListingDetail
  },
  data() {
    return {
      
    }
  },

  computed: {
    ...mapGetters({
      property: 'search/property',
    }),
  },

  async fetch(context) {
    // console.log(context.params)
    const property = await context.$axios.$get(`${process.env.apiURL}/Search/Listings/${context.params.id}`)

    // console.log(property)
    // console.log(properties)
    // context.store.dispatch('search/hyrdateStore', context.query)
    context.store.commit('search/SET_PROPERTY', property)
  },
}
</script>

<style lang="scss" scoped></style>
