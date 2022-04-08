<template>
  <div class="card">
    <header class="card-header">
      <button class="button" @click="back">Back</button>
      <p class="center is-size-4">
          {{address}}
      </p>
      
    </header>

    

    <div class="card-content">

      <div class="property-images" v-for="image in images" :key="image">
        <figure class="image">
          <!-- {{listing.mainImage}} -->
          <!-- :src="listing.mainImage" -->
          <img
            loading="lazy"
            :src="image"
            alt="Listing Image"
            class="property-images__image"
          />
        </figure>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
    data() {
        return {
            loaded: false
        }
    },
  methods: {
    back() {
      this.$store.commit('search/SET_SHOW_IMAGES')
    },
  },
  computed: {
    ...mapGetters({
      images: 'search/images',
      address: 'search/address'
    }),
  },
  async mounted() {
      await this.$store.dispatch('search/fetchImages')

      this.loaded = true
  }
}
</script>

<style lang="scss" scoped>
.card-header {
  display: flex;

  .center {
      margin: auto auto;
  }
}

.property-images {
//   display: flex;
  width: 60%;
  margin: auto auto;
//   flex-direction: column;

  &__image {
    border: 1px solid lightslategrey;
    // padding: 1rem;
    margin-bottom: 10px;
  }
}
</style>
