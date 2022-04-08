<template>
  <div class="container m-t-md">
    <div class="card">
      <div class="card-content">
        <div class="has-text-centered">
          <div class="m-b-md">
            <p class="is-size-4 is-size-5-mobile">
              Congrats! Your Ad is created
            </p>
          </div>
          <div>
            <button class="button is-success" @click="go()">
              View my Ad in Management Console
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  computed: {
    ...mapGetters({
      listing: 'manage/listings/listing'
    }),
  },
  methods: {
    go() {
      this.$router.push(`/manage/ads/${this.listing.id}/manage`)
    },
  },
}
</script>

<style lang="scss" scoped></style>
