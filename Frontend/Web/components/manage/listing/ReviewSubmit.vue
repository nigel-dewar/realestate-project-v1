<template>
    <div class="card">
      <div class="card-content">
        <p class="is-size-4 is-size-5-mobile p-b-md">Review and Submit</p>
        <div class="notification is-success" v-if="success">All went well</div>
        <div class="notification is-danger" v-if="error">
          {{ error }}
        </div>

        <div class="columns">
          <div class="column">
            <div class="block"><b>Name: </b> {{ listing.name }}</div>

            <div class="block">
              <b>Advert Reference: </b> {{ listing.listingRef }}
            </div>

            <div class="block">
              <b>Advert Type: </b> {{ listing.listingType }}
            </div>

            <div class="block">
              <b>Price: </b> <span>${{ listing.price }}</span>
              <span v-if="listing.listingType == 'Lease'"> Per Week</span>
            </div>

            <div class="block">
              <b>Date Listing Starts: </b>
              <span>${{ listing.dateListingStarts }}</span>
            </div>

            <div class="block">
              <b>Date Listing Ends: </b>
              <span>${{ listing.dateListingExpires }}</span>
            </div>

            <div class="block">
              <b>Contact Email:</b> <span>{{ listing.contactEmail }}</span>
              <b>Contact Number:</b> <span>{{ listing.contactNumber }}</span>
            </div>

            <div class="block">
              <b>Property Type:</b> <span>{{ listing.propertyTypeId }}</span>
            </div>

            <div class="block">
              <b>Bedrooms:</b> <span>{{ listing.bedrooms }}</span>
            </div>

            <div class="block">
              <b>Bathrooms:</b> <span>{{ listing.bathrooms }}</span>
            </div>

            <div class="block">
              <b>Parking Spaces:</b> <span>{{ listing.parkingSpaces }}</span>
            </div>

            <div class="block">
              <p><b>Features:</b></p>
              <div v-for="feature in listing.features" :key="feature.id">
                {{ feature.name }}
              </div>
            </div>

            

            <div class="block">
              <b>Description:</b>
              <p v-html="listing.description"></p>
            </div>
          </div>
        </div>

        <div class="columns">
          <div class="column">
            <div>
              <h1><b>Publish your Ad?: </b></h1>
              <b-field :message="goLiveErrors" :type="fieldFormat('userRequestAction')">
                <b-select
                  size="is-medium"
                  v-model="form.userRequestAction"
                  @input="emitFormData"
                  @blur="$v.form.userRequestAction.$touch()"
                  placeholder="Please Select"
                >
                  <option value="confirm">Yes - Confirm</option>
                  <option value="save">No, Just Save for Now</option>
                </b-select>
              </b-field>
            </div>
          </div>
        </div>
      </div>
    </div>
</template>

<script>
import Editor from '~/components/editor'
import { required } from 'vuelidate/lib/validators'

export default {
  props: {
    form: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      error: '',
      success: false,
      hasError: false,
      labelPosition: 'on-border',
      testing: true,
    }
  },
  components: {
    Editor,
  },
  validations: {
    form: {
      userRequestAction: {
        required,
      },
    },
  },
  computed: {
    isValid() {
      return !this.$v.$invalid
    },
    listing() {
      return this.$store.getters['manage/listings/listing']
    },
    listingType() {
      return this.form.listingType
    },
    goLiveErrors() {
      const errors = []
      if (!this.$v.form.userRequestAction.$dirty) return errors
      !this.$v.form.userRequestAction.required &&
        errors.push('Ad Decision is required')
      return errors
    },
  },
  mounted() {
    this.scrollToTop()
  },
  methods: {
    scrollToTop() {
      window.scrollTo({
        top: 0,
        left: 0,
        // behavior: 'smooth'
      })
    },
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
  },
}
</script>

<style lang="scss" scoped></style>
