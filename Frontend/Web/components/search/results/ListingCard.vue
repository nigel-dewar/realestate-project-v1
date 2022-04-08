<template>
  <div class="card card-padding">
    <div @click="go()">
    <div class="card-image">
      <figure class="image is-4by3">
        <!-- {{listing.mainImage}} -->
        <!-- :src="listing.mainImage" -->
        <img loading="lazy"
          :src="listing.mainImage"
          alt="Listing Image"
          width="200" height="200"
        />
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
            <div class="specs__item__text">{{ listing.bathrooms }}</div>
            <i class="fas fa-bath fa-md"></i>
          </div>
          <div class="specs__item">
            <div class="specs__item__text">{{ listing.parkingSpaces }}</div>
            <i class="fas fa-car fa-md"></i>
          </div>
          <div class="specs__item">
            <div class="specs__item__text">{{ listing.landSize }}&#13217;</div>
            <i class="fa fa-ruler fa-md"></i>
          </div>
          <div class="specs__item">
            |
          </div>
          <div class="specs__item">
            {{ listing.propertyType }}
          </div>
        </div>
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
  methods: {
    go() {
      this.$router.push(`/details/${this.listing.id}`)
    }
  },
}
</script>

<style lang="scss" scoped>
.card-padding {
  margin-bottom: 20px;
  cursor: pointer;
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
