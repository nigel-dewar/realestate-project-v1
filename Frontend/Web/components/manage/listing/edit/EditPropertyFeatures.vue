<template>
  <div class="card manage-card">
    <header class="card-header card-section">
      <p class="card-header-title">Property Features</p>
    </header>
    <div class="card-content card-section">
      <form>
        <div class="block">
          <b-checkbox
            v-for="(item, index) in this.features"
            :key="index"
            :id="item.name"
            v-model="item.checked"
            class="space"
          >
            {{ item.name }}
          </b-checkbox>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
export default {
  props: {
    form: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      features: [],
      featureGroups: [],
    }
  },
  computed: {

    ...mapState({
      lookups: state => state.manage.listings.lookups,
      listing: state => state.manage.listings.listing
    }),

    filters() {
      return this.$store.getters['manage/listings/filters']
    },
    checkedFeatures() {
      return this.features.filter((x) => x.checked == true)
    },
  },
  mounted() {

    this.lookups.features.map((x) => {
      let found = false
      if (this.listing.features.length > 0) {
        found = this.listing.features.find(f => f.id == x.id) ? true : false
      }
      // let check = this.listing.features.find(f => f.id == x.id)
        var checkbox = {
          id: x.id,
          name: x.name,
          checked: found,
        }
        this.features.push(checkbox)
      })

  },
  methods: {
    run() {
      this.emitFormData()
    },
    checked(e) {

    },
    emitFormData() {
      this.$emit('stepUpdated', { data: this.form, isValid: true })
    },
  },
  watch: {
    checkedFeatures: function (val) {
      this.form.features = this.features.filter(x=>x.checked == true)
      this.emitFormData()
    },
  },
}
</script>

<style lang="scss" scoped>
.space {
  max-width: 17rem;
  min-width: 17rem;
}

</style>
