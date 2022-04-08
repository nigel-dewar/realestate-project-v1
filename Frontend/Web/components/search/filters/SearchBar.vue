<template>
  <div>
    <div class="search-bar">
      <div class="search-bar__search-box">
        <SearchBox />
      </div>
      <div class="search-bar__type">Prop Type</div>
      <div class="search-bar__beds">
        <span @click="selectedMenu('beds')">Beds</span>
        <div class="showMenu" v-if="showBeds">This is the beds menu</div>
      </div>
      <div class="search-bar__price">
        <span @click="selectedMenu('price')">Price</span>
      </div>
      <div class="search-bar__filters">Filters</div>
      <div class="search-bar__update">
        <button class="button is-success">Update</button>
      </div>
      
    </div>
    <div>
        
    </div>
  </div>
</template>

<script>
import SearchBox from './SearchBox'
export default {
  data() {
    return {
      showBeds: false,
      showPrice: false,
    }
  },
  components: {
    SearchBox,
  },
  methods: {
    selectedMenu(val) {
      if (val == 'beds') {
        this.showBeds = !this.showBeds
      }
    },
  },
}
</script>

<style lang="scss" scoped>

.multiselect__single {
  white-space: nowrap;
  overflow: hidden; 
}

.showMenu {
    position: absolute;
    width: 150px;
    background-color: white;
    bottom: -50px;
    transition: 5s all;
}
.search-bar {
  display: flex;
  width: 100%;
  background-color: white;

  &__search-box {
    border: 1px solid red;
    width: 25rem;
  }
  &__type {
    border: 1px solid blue;
    cursor: pointer;
  }
  &__beds {
      position: relative;
    border: 1px solid lightblue;
    cursor: pointer;
  }
  &__price {
    border: 1px solid purple;
    cursor: pointer;
  }
  &__fitlers {
    border: 1px solid orange;
    cursor: pointer;
  }
  &__update {
    border: 1px solid green;
    cursor: pointer;
  }


}
</style>
