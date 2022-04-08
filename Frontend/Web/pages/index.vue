<template>
<div>
  
    <div class="container is-max-desktop m-t-sm">
    
        <div class="p-l-sm p-r-sm">
          <HomePageSearchBox />
        </div>
        <div class="p-l-sm p-r-sm m-t-sm">
          <CreateAnAd />
        </div>
    
    </div>
  </div>
</template>

<script>

import {mapState} from 'vuex';
import CreateAnAd from '~/components/manage/CreateAnAd'
import HomePageSearchBox from '~/components/search/HomePageSearchBox'


export default {
  name: 'HomePage',
  data() {
    return {
      apiData: null
    }
  },
  components: {
    CreateAnAd,
    HomePageSearchBox
  },
  computed: {
    user() {
      return this.$store.state.auth.user;
    },
    isAdmin() {
      return this.$store.getters['auth/isAuthenticated']
    },
  },
  methods: {
  },
  mounted() {
    
  }
}
</script>

<style lang="scss" scoped>
</style>
