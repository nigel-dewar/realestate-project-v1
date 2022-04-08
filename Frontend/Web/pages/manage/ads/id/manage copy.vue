<template>
  <div class="manage-page">
    <div class="course-manage" v-if="loaded">
      <div class="container">
        <div class="columns">
          <div class="column">
            <h1 class="title">Advert Id: {{ listing.listingRef }}</h1>
            <h3 class="subtitle">{{ listing.name }}</h3>

            <div>
              <span class="tag" :class="listingStatus.class">{{
                listingStatus.text
              }}</span>
              <span class="tag" :class="listingConfirmed.class">{{
                listingConfirmed.text
              }}</span>
              <small
                v-if="
                  listing.confirmed && !listing.cancelled && !listing.active
                "
                >Ad will be run from: {{ dateListingStarts }} to
                {{ dateListingEnds }}</small
              >
              <small v-if="listing.active"
                >Ad is Live. Running from: {{ dateListingStarts }} to
                {{ dateListingEnds }}</small
              >
            </div>

            <div v-if="listing.active">
              <span class="tag is-info">Note: Changes made to an Active Ad can take up to 2 hours to display</span>
            </div> 

            <div
              v-if="listing.checker && listing.checker.errorCount > 0"
              style="border: 1px solid grey; padding: 5px"
            >
              {{ listing.checker.errorMessages.length }} Items required your
              attention
              <ul>
                <li
                  v-for="item in listing.checker.errorMessages"
                  :key="item.code"
                >
                  {{ item.error }}
                </li>
              </ul>
            </div>
            
          </div>
        </div>
        <div class="columns">
          <div class="column is-3 p-lg">
            <!-- InValid? {{ formInValid }} -->
            <!-- <aside class="menu is-hidden-mobile"> -->
            <aside class="menu">
              <p class="menu-label">Advert</p>
              <ul class="menu-list">
                <li>
                  <a
                    @click.prevent="navigateTo(1)"
                    :class="activeComponentClass(1)"
                  >
                    Details
                  </a>
                </li>
              </ul>
              <p class="menu-label">Property</p>
              <ul class="menu-list">
                <li>
                  <!-- display Address -->
                  <a
                    @click.prevent="navigateTo(2)"
                    :class="activeComponentClass(2)"
                    >Address
                  </a>
                </li>
                <li>
                  <!-- display Property Details-->
                  <a
                    @click.prevent="navigateTo(3)"
                    :class="activeComponentClass(3)"
                  >
                    Details
                  </a>
                </li>
                <li>
                  <!-- display Property Features -->
                  <a
                    @click.prevent="navigateTo(4)"
                    :class="activeComponentClass(4)"
                  >
                    Features
                  </a>
                </li>
              </ul>
              <p class="menu-label">Pictures</p>
              <ul class="menu-list">
                <li>
                  <!-- display Price -->
                  <a
                    @click.prevent="navigateTo(5)"
                    :class="activeComponentClass(5)"
                  >
                    Manage
                  </a>
                </li>
              </ul>
            </aside>
          </div>
          <div class="column">
            <keep-alive>
              <component
                :is="activeComponent"
                :form="form"
                @stepUpdated="mergeFormData"
                ref="activeComponent"
              />
            </keep-alive>
            <div class="level p-t-md">
              <div class="level-left"></div>
              <button
                v-if="listing"
                :disabled="!canProceed || !changes"
                @click="save"
                class="button is-success is-large level-left"
              >
                Save & Check
              </button>

              <Modal
                v-if="userRequestOptions.canConfirm"
                @submitted="confirm($event, 'confirm')"
                openTitle="Confirm"
                openBtnClass="button is-success is-large is-outlined level-left"
                title="Confirm Ad?"
                :showButton="true"
                actionTitle="Confirm"
              >
                <div>
                  Confirming your Ad will lock it in so it appears to the
                  public. You will not be able to edit the Start and End dates
                  of the Ad after confirming.
                </div>
              </Modal>

              <Modal
                v-if="userRequestOptions.canCancel"
                @submitted="cancel($event, 'cancel')"
                openTitle="Cancel"
                openBtnClass="button is-danger is-medium is-outlined"
                title="Cancel Ad?"
                :showButton="true"
                actionTitle="Confirm"
              >
                <div>
                  When you cancel your Ad, it will ensure it will not appear to
                  the public. To Re-list your Ad, you can use the Re-list button
                  to re-activate it, and set new dates for the Ad to run.
                </div>
              </Modal>

              <Modal
                v-if="userRequestOptions.canRelist"
                @submitted="relist($event, 'relist')"
                openTitle="Relist"
                openBtnClass="button is-success is-medium is-outlined"
                title="Relist Ad?"
                :showButton="true"
                actionTitle="Confirm"
              >
                <div>
                  Relisting your add will get it back up and running again.
                </div>
              </Modal>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import AdDetails from '~/components/manage/listing/AdDetails.vue'
import Address from '~/components/manage/listing/edit/EditAddress.vue'
import Property from '~/components/manage/listing/edit/EditProperty.vue'
import PropertyFeatures from '~/components/manage/listing/edit/EditPropertyFeatures.vue'
import Images from '~/components/manage/listing/AddImages.vue'
import Modal from '~/components/shared/Modal'

const greaterThanZero = (value) => value > 0

import { mapState, mapGetters } from 'vuex'
import { required, email, maxValue } from 'vuelidate/lib/validators'
export default {
  layout: 'manage',
  name: 'manage-ad',
  components: {
    Address,
    Property,
    PropertyFeatures,
    Images,
    AdDetails,
    Modal,
  },
  validations: {
    form: {
      streetNumber: {
        required,
      },
      streetName: {
        required,
      },
      postalCode: {
        required,
      },
      suburbOrCity: {
        required,
      },
      regionOrState: {
        required,
      },
      country: {
        required,
      },
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
      mainImage: {
        required,
      },
      price: {
        required,
        maxValue: greaterThanZero,
      },
      listingType: {
        required,
      },
      dateListingStarts: {
        required,
      },
      contactEmail: {
        required,
        email,
      },
      contactNumber: {
        required,
      },
    },
  },
  data() {
    return {
      changes: false,
      steps: ['AdDetails', 'Address', 'Property', 'PropertyFeatures', 'Images'],
      activeStep: 1,
      loaded: false,
      canProceed: false,
      form: {
        name: '',
        canEditAddress: false,
        propertyTypeId: '',
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
        price: '',
        listingType: '',
        isPremium: false,
        dateListingStarts: null,
        dateListingExpires: null,
        contactEmail: '',
        contactNumber: '',
        confirmed: null,
        userRequestAction: '',
      },
    }
  },
  // async fetch({ store, params }) {
  //   const list = await store.dispatch('manage/listings/getListing', params.id)
  //   const lookup = await store.dispatch('manage/listings/getLookups')
  //   return list
  // },
  mounted() {
    // check if propertyId is listed in params. If it is, load its details from API, and hydrate this listing

    this.$store.dispatch('manage/listings/getLookups')

    // if we dont have a property in the store
    this.$store
      .dispatch('manage/listings/getListing', this.$route.params.id)
      .then((listingId) => {
        this.form = { ...this.listing }
        this.loaded = true
        // this.checkValid()
        this.checkValid()

        // this.activeStep = this.$route.query.step
      })
      .catch((error) => {})
  },
  computed: {
    activeComponent() {
      return this.steps[this.activeStep - 1]
    },
    ...mapState({
      listing: ({ manage }) => manage.listings.listing,
    }),
    ...mapGetters({
      listingStatus: 'manage/listings/listingStatus',
      listingConfirmed: 'manage/listings/listingConfirmed',
      userRequestOptions: 'manage/listings/userRequestOptions',
    }),
    formInValid() {
      return this.$v.form.$invalid
    },
    formInfo() {
      return this.$v.form
    },
    dateListingStarts() {
      return this.listing.dateListingStarts != null
        ? new Date(
            Date.parse(this.listing.dateListingStarts)
          ).toLocaleDateString()
        : null
    },
    dateListingEnds() {
      return this.listing.dateListingExpires != null
        ? new Date(
            Date.parse(this.listing.dateListingExpires)
          ).toLocaleDateString()
        : null
    },
  },
  methods: {
    confirm({ closeModal }) {
      this.form.userRequestAction = 'confirm'
      this.save()
      closeModal()
    },

    cancel({ closeModal }) {
      this.form.userRequestAction = 'cancel'
      this.save()
      closeModal()
    },

    relist({ closeModal }) {
      this.form.userRequestAction = 'relist'
      this.save()
      closeModal()
    },
    checkTotalComplete() {},
    checkValid() {
      this.$nextTick(() => {
        this.canProceed = this.$refs.activeComponent.isValid
      })
    },
    navigateTo(step) {
      this.activeStep = step
    },
    activeComponentClass(step) {
      return this.activeStep === step ? 'is-active' : ''
    },
    mergeFormData({ data, isValid }) {
      this.form = { ...this.form, ...data }

      var errors = this.$v.form.$anyError
      this.changes = true
      this.canProceed = isValid
    },
    save() {
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

      const payload = {
        id: this.listing.id,
        propertyId: this.listing.propertyId,
        active: this.form.active,
        price: this.form.price,
        dateListingStarts: utc_timestamp,
        listingType: this.form.listingType,
        isPremium: this.form.isPremium,
        contactEmail: this.form.contactEmail,
        contactNumber: this.form.contactNumber,
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
        userRequestAction: this.form.userRequestAction,
      }

      this.$store
        .dispatch('manage/listings/save', payload)
        .then((_) => {
          this.changes = false
        })
        .catch(({ message, code }) => {
          this.$toast.error(message, {
            duration: 10000,
            position: 'bottom-center',
          })
        })
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

<style lang="scss">
.manage-page {
  .label-info {
    font-size: 13px;
    color: gray;
    font-style: italic;
  }
  .course-manage {
    padding-top: 40px;
    .menu {
      padding-top: 30px;
      &-label {
        font-size: 20px;
        color: black;
      }
      &-list {
        > li {
          font-size: 18px;
          margin-top: 10px;
          > a {
            &.is-active {
              border-left: 3px solid #58529f;
              background-color: transparent;
              color: inherit;
            }
          }
        }
      }
    }
    .card {
      &-section {
        padding: 40px;
      }
      &-header {
        &-title {
          padding: 0;
          color: #8f99a3;
          font-weight: 400;
          font-size: 25px;
        }
      }
    }
  }
}
</style>
