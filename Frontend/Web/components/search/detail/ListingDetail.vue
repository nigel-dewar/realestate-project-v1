<template>
    <div class="card">
      <div class="card-image" @click="showImages">
        <figure class="image is-4by3">
          <!-- {{listing.mainImage}} -->
          <img :src="listing.mainImage" class="clickable-image" alt="Listing Image" />
        </figure>
      </div>
      <div class="card-content">
        <p class="card-title">
          {{ listing.title }}
        </p>
        <p class="subtitle">{{ listing.address }}</p>
        <div class="content">
          <div class="specs">
            <div class="specs__item">
              <div class="specs__item__text">{{ listing.bedrooms }}</div>
              <i class="fas fa-bed fa-md"></i>
            </div>
            <div class="specs__item">
              <div class="specs__item__text">{{ listing.bedrooms }}</div>
              <i class="fas fa-bath fa-md"></i>
            </div>
            <div class="specs__item">
              <div class="specs__item__text">{{ listing.parkingSpaces }}</div>
              <i class="fas fa-car fa-md"></i>
            </div>
            <div class="specs__item">
              <div class="specs__item__text">
                {{ listing.landSize }}&#13217;
              </div>
              <i class="fa fa-ruler fa-md"></i>
            </div>
            <div class="specs__item">|</div>
            <div class="specs__item">
              {{ listing.propertyType }}
            </div>
          </div>
          <hr />
          <div class="features">
            <p class="header">Features</p>
            
            <div
              class="features-items"
              v-for="(item, index) in listing.features"
              :key="index"
            >
              <div class="space" :id="item.name">
                {{ item }}
              </div>
            </div>
          </div>
          <hr />
          <div class="description">
            <p class="header">Description</p>
            <div v-html="listing.description"></div>
          </div>
        </div>
      </div>
    </div>
</template>

<script>
export default {
  props: {
    listing: {
      type: Object,
    },
  },
  mounted() {
    window.scrollTo({
      top: 0,
      left: 0,
      // behavior: 'smooth'
    })
  },
  methods: {
    showImages() {
      this.$store.commit('search/SET_SHOW_IMAGES')
    }
  },
}
</script>

<style lang="scss" scoped>

.clickable-image {
  cursor: pointer;
}

.images {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: yellow;
}
.card-padding {
  margin-bottom: 20px;
  cursor: pointer;
}

.header {
  font-size: 1.5rem;
  font-weight: 600;
}

.features {
  &-items {
    display: inline-block;

    .space {
      max-width: 17rem;
      min-width: 17rem;
    }
  }
}

.card-title {
  font-size: 1.5rem;
  font-weight: 600;
}

.specs {
  display: flex;
  // border: 1px solid green;

  &__item {
    display: flex;
    align-items: center;
    // border: 1px solid red;
    margin-right: 12px;

    &__text {
      margin-right: 5px;
      font-weight: 600;
    }
  }
}
</style>
