<template>
  <div>
    <div class="container" v-if="loaded">

      <div v-if="listing">
        <div class="card m-t-md m-b-md">
          <div class="card-content">
            <p class="is-size-5 is-size-7-mobile"><b>{{ listing.name }}</b></p>
            <p class="is-size-6 is-size-7-mobile"><b>Ad Ref: {{ listing.listingRef }}</b></p>
          </div>
        </div>
      </div>
      <!-- <button
        v-if="listing"
        :disabled="!canProceed"
        @click="saveListing"
        class="button is-success is-large float-right"
      >
        Save
      </button> -->

      <div class="m-t-md">
        <keep-alive>
        <component
          :is="activeComponent"
          :form="form"
          @stepUpdated="mergeFormData"
          ref="activeComponent"
        />
      </keep-alive>
      </div>
    </div>

    <div class="container">
      <!-- <div v-if="!isFirstStep">
        <a @click.prevent="previousStep" class="button is-large float-left"> Previous </a>
      </div>
      <div v-else class="empty-container"></div> -->
      <div class="has-text-centered p-t-md p-b-md">
        <button
          v-if="!isFirstStep"
          @click.prevent="previousStep"
          class="button is-medium"
        >
          Previous
        </button>
        <!-- <div v-if="!isLastStep" class="level-left"></div> -->
        <button
          v-if="!isLastStep"
          @click.prevent="nextStep"
          :disabled="!canProceed"
          class="button is-success is-medium"
        >
          Continue
        </button>
        <button
          v-else
          @click.prevent="confirm"
          :disabled="!canProceed"
          class="button is-success is-medium"
        >
          Confirm
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import AddProperty from '~/components/manage/listing/AddProperty.vue'
import AddPropertyDetails from '~/components/manage/listing/AddPropertyDetails.vue'
import AddImages from '~/components/manage/listing/AddImages.vue'
import CreateListing from '~/components/manage/listing/AdDetails.vue'
import ReviewSubmit from '~/components/manage/listing/ReviewSubmit.vue'
import { mapState } from 'vuex'
export default {
  layout: 'manage',
  name: 'create-ad',
  components: {
    AddProperty,
    AddPropertyDetails,
    AddImages,
    CreateListing,
    ReviewSubmit,
  },
  data() {
    return {
      profileComplete: false,
      activeStep: 1,
      loaded: false,
      propertyCreated: false,
      steps: [
        'CreateListing',
        'AddProperty',
        'AddPropertyDetails',
        'AddImages',
        'ReviewSubmit',
      ],
      canProceed: false,
      form: {
        name: '',
        propertyId: '',
        propertyTypeId: '',
        existingProperty: false,
        canCreateProperty: false,
        bedrooms: '',
        bathrooms: '',
        parkingSpaces: '',
        landSize: '',
        streetNumber: '',
        streetName: '',
        postalCode: '',
        suburbOrCity: '',
        council: '',
        regionOrState: '',
        country: '',
        longitude: '',
        latitude: '',
        googleAddress: '',
        description: '',
        mainImage: '',
        features: [],
        active: false,
        status: 0,
        price: "",
        listingType: '',
        isPremium: false,
        dateListingStarts: '',
        contactEmail: '',
        contactNumber: '',
        showNumber: false,
        userRequestAction: null
      },
    }
  },
  computed: {
    stepsLength() {
      return this.steps.length
    },
    isLastStep() {
      return this.activeStep === this.stepsLength
    },
    isFirstStep() {
      return this.activeStep === 1
    },
    progress() {
      return `${(100 / this.stepsLength) * this.activeStep}%`
    },
    activeComponent() {
      return this.steps[this.activeStep - 1]
    },

    profileCompleted() {
      return this.$store.getters['auth/profileComplete']
    },

    listing() {
      return this.$store.getters['manage/listings/listing']
    },

    ...mapState({
      count: (state) => state.manage.listings.count,
    }),
  },
  mounted() {

    if (!this.profileCompleted) {
      this.$router.push('/user/profile')
    } 
    // check if propertyId is listed in params. If it is, load its details from API, and hydrate this listing
    // just a comment
    this.$store.dispatch('manage/listings/getLookups')
    this.$store.dispatch('manage/listings/getCount')

    if (this.$route.query.listing) {
      if (!this.listing) {
        // if we dont have a property in the store
        this.$store
          .dispatch('manage/listings/getListing', this.$route.query.listing)
          .then((listingId) => {
            this.form = { ...this.listing }
            this.loaded = true
            
            this.activeStep = +this.$route.query.step
            this.checkValid()
          })
          .catch((_) => this.$router.push({ name: 'manage.create.ad' }))
      }
    } else {
      this.loaded = true
    }
  },
  methods: {
    scrollToTop() {
      window.scrollTo({
        top: 0,
        left: 0,
        // behavior: 'smooth'
      })
    },
    async createNewProperty() {
      try {
        // Create property and send back id because we have required info we need.
        const payload = {
          propertyTypeId: this.form.propertyTypeId,
          bedrooms: this.form.bedrooms,
          bathrooms: this.form.bathrooms,
          parkingSpaces: this.form.parkingSpaces,
          landSize: this.form.landSize,
          streetNumber: this.form.streetNumber,
          streetName: this.form.streetName,
          postalCode: this.form.postalCode,
          suburbOrCity: this.form.suburbOrCity,
          council: this.form.council,
          regionOrState: this.form.regionOrState,
          country: this.form.country,
          longitude: this.form.longitude,
          latitude: this.form.latitude,
          googleAddress: this.form.googleAddress,
          listingType: this.form.listingType
        }

        const prop = await this.$store.dispatch(
          'manage/listings/createProperty',
          payload
        )

        return Promise.resolve(prop)
      } catch (error) {
        return Promise.reject(error)
      }
    },
    async createNewListing() {

      var now = new Date(this.form.dateListingStarts)

      var utc_timestamp = Date.UTC(
        now.getFullYear(),
        now.getMonth(),
        now.getDate(),
        now.getHours(),
        now.getMinutes(),
        now.getSeconds(),
        now.getMilliseconds()
      )

      try {
        const payload = {
          propertyId: this.form.propertyId,
          price: this.form.price,
          listingType: this.form.listingType,
          dateListingStarts: utc_timestamp,
          contactEmail: this.form.contactEmail,
          contactNumber: this.form.contactNumber,
          showNumber: this.form.showNumber
        }

        const res = await this.$store.dispatch(
          'manage/listings/createListing',
          payload
        )

        return Promise.resolve(res)
      } catch (error) {
        return Promise.reject(error)
      }
    },
    async saveListing() {

      var now = new Date(this.form.dateListingStarts)

      var utc_timestamp = Date.UTC(
        now.getFullYear(),
        now.getMonth(),
        now.getDate(),
        now.getHours(),
        now.getMinutes(),
        now.getSeconds(),
        now.getMilliseconds()
      )

      try {
        const payload = {
          id: this.listing.id,
          propertyId: this.listing.propertyId,
          active: this.form.active,
          dateListingStarts: utc_timestamp,
          price: this.form.price,
          listingType: this.form.listingType,
          isPremium: this.form.isPremium,
          contactEmail: this.form.contactEmail,
          contactNumber: this.form.contactNumber,
          showNumber: this.form.showNumber,
          name: this.form.name,
          description: this.form.description,
          propertyTypeId: this.form.propertyTypeId,
          bedrooms: this.form.bedrooms,
          bathrooms: this.form.bathrooms,
          parkingSpaces: this.form.parkingSpaces,
          landSize: this.form.landSize,
          streetNumber: this.form.streetNumber,
          streetName: this.form.streetName,
          postalCode: this.form.postalCode,
          suburbOrCity: this.form.suburbOrCity,
          council: this.form.council,
          regionOrState: this.form.regionOrState,
          country: this.form.country,
          longitude: this.form.longitude,
          latitude: this.form.latitude,
          googleAddress: this.form.googleAddress,
          features: this.form.features,
          userRequestAction: this.form.userRequestAction
        }
        const res = await this.$store.dispatch('manage/listings/save', payload)
        return Promise.resolve(res)
      } catch ({message, code}) {
        this.$toast.error(message, {
            duration: 10000,
            position: 'bottom-center',
          })
        return Promise.reject(error)
      }

    },
    checkValid() {
      this.$nextTick(() => {
        this.canProceed = this.$refs.activeComponent.isValid
      })
    },
    async nextStep() {
      this.$nextTick(() => {
        this.canProceed = this.$refs.activeComponent.isValid
      })

      try {
        if (this.listing == null) {
        // here we create new stuff
        
        if (this.activeStep == 2) {
          
          // double check that we CAN create a property, and that user has selected NOT to use an existing property
          if (this.form.canCreateProperty) {
            const property = await this.createNewProperty()

            this.form.propertyId = property.id

            const listing = await this.createNewListing()

            this.activeStep++
            this.$router.push({
              name: 'manage.create.ad',
              query: { listing: listing.id, step: this.activeStep },
            })

          }
        } else {
          this.activeStep++
        }
      } else {
        // We have a Listing, so we now need to save any changes to it
        const res = await this.saveListing()
        this.activeStep++
        this.$router.push({
          name: 'manage.create.ad',
          query: { listing: this.listing.id, step: this.activeStep },
        })
      }
      } catch (error) {
        this.$toast.error(error.message, {
            duration: 3000,
            position: 'bottom-center',
          })
      }
      this.scrollToTop()
    },
    async confirm() {
      try {
        const res = await this.saveListing()
        this.$router.push('/manage/ads/created')
      } catch (error) {
        
      }
    },
    previousStep() {
      this.activeStep--
      if (this.listing) {
        this.$router.push({
        name: 'manage.create.ad',
        query: { listing: this.listing.id, step: this.activeStep },
      })
      }
      
      this.canProceed = true
      this.scrollToTop()
    },
    mergeFormData({ data, isValid }) {
      this.form = { ...this.form, ...data }
      this.canProceed = isValid
    },
  },
  watch: {
    activeComponent(val) {
      this.$nextTick(() => {
        this.$refs.activeComponent.run()
      })
    },
  },
}
</script>
