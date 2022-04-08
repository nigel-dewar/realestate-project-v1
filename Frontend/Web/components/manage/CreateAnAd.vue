<template>
    <div class="">
        <h4>Want to Advertise your property? It's FREE!</h4>
        <br>

        <button class="button is-success" @click="createAd">Create an Ad</button>
    </div>
</template>

<script>

import {mapState} from 'vuex';

export default {
  name: 'CreateAnAd',
  data() {
    return {
      apiData: null
    }
  },
  methods: {

    createAd() {
      
      this.$store.dispatch('manage/listings/reset').then(_ => {
        this.$router.push('/manage/ads/create')
      })

    }
  }
}
</script>

<style lang="scss" scoped>

</style>