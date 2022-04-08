<template>
  <div class="card manage-card">
    <div class="card-content card-section">
      <p class="is-size-4 is-size-5-mobile m-b-md">Property Details</p>
      <form @submit.prevent="">
        <div v-if="lookups">
          <b-field
            label="Property Type"
            :label-position="labelPosition"
            :message="propertyTypeErrors"
            :type="fieldFormat('propertyTypeId')"
          >
            <b-select
              placeholder="Select Type"
              expanded
              size="is-medium"
              v-model="form.propertyTypeId"
              @input="emitFormData"
              @blur="$v.form.propertyTypeId.$touch()"
            >
              <option
                v-for="option in lookups.propertyTypes"
                :value="option.id"
                :key="option.id"
              >
                {{ option.type }}
              </option>
            </b-select>
          </b-field>

          <b-field
            label="Bedrooms"
            :label-position="labelPosition"
            :message="bedroomsErrors"
            :type="fieldFormat('bedrooms')"
          >
            <b-select
              placeholder="Select Bedrooms"
              expanded
              size="is-medium"
              @blur="$v.form.bedrooms.$touch()"
              @input="emitFormData"
              v-model="form.bedrooms"
            >
              <option
                v-for="option in bedrooms"
                :value="option.id"
                :key="option.id"
              >
                {{ option.value }}
              </option>
            </b-select>
          </b-field>

          <b-field
            label="Bathrooms"
            :label-position="labelPosition"
            :message="bathroomsErrors"
            :type="fieldFormat('bathrooms')"
          >
            <b-select
              placeholder="Select Bathrooms"
              expanded
              size="is-medium"
              v-model="form.bathrooms"
              @input="emitFormData"
              @blur="$v.form.bathrooms.$touch()"
            >
              <option
                v-for="option in bathrooms"
                :value="option.id"
                :key="option.id"
              >
                {{ option.value }}
              </option>
            </b-select>
          </b-field>

          <b-field
            label="Parking Spaces"
            :label-position="labelPosition"
            :message="parkingSpacesErrors"
            :type="fieldFormat('parkingSpaces')"
          >
            <b-select
              placeholder="Select Parking"
              expanded
              size="is-medium"
              v-model="form.parkingSpaces"
              @input="emitFormData"
              @blur="$v.form.parkingSpaces.$touch()"
            >
              <option
                v-for="option in parkingSpaces"
                :value="option.id"
                :key="option.id"
              >
                {{ option.value }}
              </option>
            </b-select>
          </b-field>

          <b-field
            label="Land Size (Metre sq)"
            :label-position="labelPosition"
            :message="landSizeErrors"
            :type="fieldFormat('landSize')"
          >
            <b-input
              @input="emitFormData"
              @blur="$v.form.landSize.$touch()"
              v-model="form.landSize"
              size="is-medium"
              placeholder="Metre Sq. Estimate if you don't really know"
              type="number"
              min="0"
            >
            </b-input>
          </b-field>

          <client-only>
            <div class="blog-editor-container">
              <p class="is-size-4 is-size-5-mobile m-b-md">Property Description</p>
              <Editor
                @editorUpdated="updateDescription"
                class="editor"
                :description="this.form.description"
              />
            </div>
          </client-only>

          
        </div>
      </form>

      <div class="block m-t-md">
        <p class="is-size-4 is-size-5-mobile m-b-md">Property Features</p>
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
    </div>
  </div>
</template>

<script>
import { required } from 'vuelidate/lib/validators'
import Editor from '~/components/editor'
import { mapState } from 'vuex'

export default {
  props: {
    form: {
      type: Object,
      required: true,
    },
  },
  components: {
    Editor,
  },
  validations: {
    form: {
      propertyTypeId: {
        required,
      },
      bedrooms: {
        required,
      },
      bathrooms: {
        required,
      },
      parkingSpaces: {
        required,
      },
      landSize: {
        required,
      },
      description: {
        required,
      },
    },
  },
  data() {
    return {
      testingMode: true,
      hasError: true,
      labelPosition: 'on-border',
      features: [],
      featureGroups: [],
      propertyTypes: [
        { id: 1, value: 'House' },
        { id: 2, value: 'Apartment' },
        { id: 3, value: 'Unit' },
        { id: 4, value: 'Villa' },
        { id: 5, value: 'Townhouse' },
        { id: 6, value: 'Acreage' },
      ],
      bedrooms: [
        { id: 0, value: '0' },
        { id: 1, value: '1' },
        { id: 2, value: '2' },
        { id: 3, value: '3' },
        { id: 4, value: '4' },
        { id: 5, value: '5' },
        { id: 6, value: '6' },
        { id: 7, value: '7' },
        { id: 8, value: '8' },
        { id: 9, value: '9' },
        { id: 10, value: '10+' },
      ],
      bathrooms: [
        { id: 0, value: '0' },
        { id: 1, value: '1' },
        { id: 2, value: '2' },
        { id: 3, value: '3' },
        { id: 4, value: '4' },
        { id: 5, value: '5' },
        { id: 6, value: '6' },
        { id: 7, value: '7' },
        { id: 8, value: '8' },
        { id: 9, value: '9' },
        { id: 10, value: '10+' },
      ],
      parkingSpaces: [
        { id: 0, value: '0' },
        { id: 1, value: '1' },
        { id: 2, value: '2' },
        { id: 3, value: '3' },
        { id: 4, value: '4' },
        { id: 5, value: '5' },
        { id: 6, value: '6' },
        { id: 7, value: '7' },
        { id: 8, value: '8' },
        { id: 9, value: '9' },
        { id: 10, value: '10+' },
      ],
    }
  },
  computed: {
    isValid() {
      return !this.$v.$invalid
    },
    ...mapState({
      lookups: (state) => state.manage.listings.lookups,
      listing: (state) => state.manage.listings.listing,
    }),
    propertyTypeErrors() {
      const errors = []
      if (!this.$v.form.propertyTypeId.$dirty) return errors
      !this.$v.form.propertyTypeId.required &&
        errors.push('Property Type is required')
      return errors
    },

    bedroomsErrors() {
      const errors = []
      if (!this.$v.form.bedrooms.$dirty) return errors
      !this.$v.form.bedrooms.required && errors.push('Bedrooms is required')
      return errors
    },

    bathroomsErrors() {
      const errors = []
      if (!this.$v.form.bathrooms.$dirty) return errors
      !this.$v.form.bathrooms.required && errors.push('Bathrooms is required')
      return errors
    },

    parkingSpacesErrors() {
      const errors = []
      if (!this.$v.form.parkingSpaces.$dirty) return errors
      !this.$v.form.parkingSpaces.required && errors.push('Parking is required')
      return errors
    },

    landSizeErrors() {
      const errors = []
      if (!this.$v.form.landSize.$dirty) return errors
      !this.$v.form.landSize.required && errors.push('Land Size is required')
      return errors
    },
    checkedFeatures() {
      return this.features.filter((x) => x.checked == true)
    },
  },
  mounted() {
    this.lookups.features.map((x) => {
      let found = false
      if (this.listing.features.length > 0) {
        found = this.listing.features.find((f) => f.id == x.id) ? true : false
      }
      // let check = this.listing.features.find(f => f.id == x.id)
      var checkbox = {
        id: x.id,
        name: x.name,
        checked: found,
      }
      this.features.push(checkbox)
    })

    // this.form = { ...this.form, ...this.property }
    if (this.listing) {
      this.$v.$touch()
      this.emitFormData()
    }
  },
  methods: {
    run() {
      this.emitFormData()
    },
    fieldFormat(e) {
      if (!e) return null
      if (!this.$v.form[e].$dirty) return null
      if (this.$v.form[e].$anyError) return 'is-danger'
      return 'is-success'
    },
    emitFormData() {
      this.$emit('stepUpdated', { data: this.form, isValid: this.isValid })
    },
    updateDescription(html) {
      if (html !== '<p></p>') {
        this.form.description = html
      } else {
        this.form.description = ''
      }

     this.emitFormData()
      
    },
  },
  watch: {
    checkedFeatures: function (val) {
      this.form.features = this.features.filter((x) => x.checked == true)
      this.emitFormData()
    },
  },
}
</script>

<style lang="scss" scoped>
.editor {
  // border: 1px solid grey;
  background-color: #dddddd;
  border-radius: 2px;
  padding: 0.5rem 0.5rem;
}

.space {
  max-width: 17rem;
  min-width: 17rem;
}
</style>
