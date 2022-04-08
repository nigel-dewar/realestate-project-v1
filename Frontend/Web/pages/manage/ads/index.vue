<template>
    <div class="courses-page m-t-md">
      <div class="container">
        <div class="columns">
          <div class="column is-8 is-offset-2">
            <div class="level">
              <h1 class="is-size-4 is-size-5-mobile level-left">Your Ads</h1>
              <nuxt-link class="button is-primary level-right" to="/manage/ads/create">Create New Ad</nuxt-link>
            </div>
            <!-- Iterate Courses -->
            <div v-if="ads">
              <div v-for="ad in ads" :key="ad.id" class="tile is-ancestor">
              
              <div class="tile is-parent is-12">
                <!-- Navigate to course manage page -->
                <nuxt-link :to="`/manage/ads/${ad.id}/manage`" class="tile tile-overlay-container is-child box">
                  <!-- <div class="tile-overlay">
                    <span class="tile-overlay-text">
                      Update Ad
                    </span>
                  </div> -->
                  <div class="columns">
                    <div class="column">
                      <figure class="image is-4by2">
                        <img :src="ad.mainImage">
                      </figure>
                    </div>
                    <div class="column">
                      <p class="is-size-5"><b>{{ad.name}}</b></p>
                      <p class="is-size-6">Ad Ref: {{ad.listingRef}}</p>
                      <!-- {{ad.status}} -->
                      <span class="tag" :class="statusClass(ad.status)">{{ad.status}}</span>
                      <span class="tag" :class="confirmedClass(ad.confirmed)">{{confirmedText(ad.confirmed)}}</span>
                    </div>
                    <div class="column is-narrow flex-centered">
                      <div class="is-size-4 is-size-5-mobile">
                        <!-- {{course.price || 0}} $ -->
                        {{ad.price | currency}}
                      </div>
                    </div>
                  </div>
                </nuxt-link>
              </div>
            </div>
            </div>
          </div>
        </div>
      </div>
    </div>
</template>

<script>
import ManageHeader from '~/components/shared/Header'
export default {
  // layout: 'manage',
  middleware: 'auth',
  components: {
    ManageHeader
  },
  computed: {
    ads() {
      return this.$store.state.manage.listings.items
    }
  },
  mounted() {
    return this.$store.dispatch('manage/listings/fetchListings')
  },
  methods: {
    statusClass(status) {
      if (!status) return 'is-danger'
      if (status == 'Incomplete') return 'is-danger'
      if (status == 'Complete') return 'is-success'
    },
    confirmedClass(confirmed) {
      if (!confirmed) return 'is-warning'
      if (confirmed) return 'is-success'
    },
    confirmedText(confirmed) {
      if (!confirmed) return 'Not confirmed'
      if (confirmed) return 'Confirmed'
    }
  }
}
</script>
<style scoped lang="scss">
  
  .courses-page {
    // padding-top: 60px;
    &-title {
      // font-size: 40px;
      // font-weight: bold;
      // padding-bottom: 20px;
    }
  }
</style>
