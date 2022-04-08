<template>
  <section class="section">
    <div class="m-b-lg" v-if="!listing">
      <h1 class="title">Let's add your Property to this Ad...</h1>
      <h2 class="subtitle">
        By the way, we can always change this info later before we make it live.
      </h2>
    </div>
    <div v-if="listing">
      <h1 class="title">Property for Ad</h1>
    </div>
    <!-- Select Use Existing or Create New Property -->
    <div v-if="count > 0">
      <section v-if="!listing">
        <div class="block">
          {{ form.existingProperty }}
          <h4>Would you like to add an existing Property or Setup a new one?</h4>
          <br />
          <b-radio
            v-model="form.existingProperty"
            @input="emitFormData"
            name="name"
            :value="true"
            :native-value="true"
          >
            An existing Property
          </b-radio>
          <b-radio
            v-model="form.existingProperty"
            @input="emitFormData"
            name="name"
            :value="false"
            :native-value="false"
          >
            Create a new Property in Realestateify
          </b-radio>
        </div>
      </section>

      <!-- Use Existing Property -->
      <section v-if="form.existingProperty">

        <div>
          <br>
          
          <p>You CAN'T have 2x 'For Sale' Ads for the same Property at same time. But you CAN have 2x Ads on a Property where 1 Ad is'For Sale' and 1 is 'For Rent' at the same time.</p>
          <br>

          <div v-if="propertiesCanListFresh.length > 0">
            <p>We're only showing you Properties here that are available to <b>{{form.listingType}}</b>.</p>
          <br>
          <!-- <div v-for="item in listings" :key="item.id">
            {{item.name}}
          </div> -->

          <b-field
            :label-position="labelPosition"
          >
            <b-select
              placeholder="Select Property"
              expanded
              size="is-medium"
              v-model="existingPropertySelected"
            >
              <option
                v-for="item in propertiesCanListFresh"
                :value="item.id"
                :key="item.id"
              >
                {{ item.name }}
              </option>
            </b-select>
          </b-field>

          <button class="button is-success" @click="existingPropertySubmit()">Use this Property</button>
          </div>

          <div v-if="propertiesCanReList.length > 0">
            <p>You have some Properties advertised here to {{form.listingType}} that have expired. You can opt to Re-List these</p>
            <br>
            <b-field
            :label-position="labelPosition"
          >
            <b-select
              placeholder="Select Property"
              expanded
              size="is-medium"
              v-model="existingPropertySelected"
            >
              <option
                v-for="item in propertiesCanReList"
                :value="item.id"
                :key="item.id"
              >
                {{ item.name }}
              </option>
            </b-select>
          </b-field>
          <br>
            <button class="button is-success">Relist this Property</button>
          </div>
        </div>
        
          

      </section>
    </div>

    <!-- Create a New Property -->
    <section v-if="!form.existingProperty || this.listing">
      <form-wrapper :validator="$v.form">
        <div style="margin-bottom: 20px">
          <form-summary />
        </div>
        <div class="columns">
          <div class="column">
            <div class="field" v-if="!listing">
              <div class="control">
                <GoogleMapsAutocomplete
                  @address-response="handleAddress"
                  :placeHolder="'Search for property address...'"
                />
                <label for=""
                  >We'll try to get as much info as possible and you can fill in
                  the missing details below.</label
                >
              </div>
            </div>

            <div class="notification is-success" v-if="checkingPropertyStatus">
              <i class="fas fa-spinner fa-spin"></i>
              Checking to see if this Property can be created...
            </div>
            <div class="notification is-danger" v-if="hasError">
              {{ errorMessage }}
            </div>

            <div v-if="listing">
              <div v-if="listing.canEditAddress">
                <p>
                  Most of the time the Google address search will get the
                  address correct. But... sometimes it just does not. You can
                  edit this address only once so please be careful to get this
                  correct.
                </p>
                <br />
                <b-checkbox
                  v-model="editAddressManual"
                  class="checkbox text-danger"
                  >I understand. Let me edit the address.</b-checkbox
                >

                <div v-if="editAddressManual" class="is-warning">
                  <br />
                  <p>
                    Notice!: This edit will be rejected if the address you enter
                    is already in use with a Live Advert.
                  </p>
                  <p>
                    Need Help? Contact Realestateify:
                    <a href="mailto:support@realestateify.com"
                      >support@realestateify.com</a
                    >
                  </p>
                </div>
              </div>

              <div v-if="!listing.canEditAddress">
                Our records show this address has been previously edited. If you
                need this edited again, please contact support at
                <a href="mailto:support@realestateify.com"
                  >support@realestateify.com</a
                >
              </div>
            </div>

            <div class="columns">
              <div class="column">
                <b-field
                  label="Street Number"
                  :label-position="labelPosition"
                  :message="streetNumberErrors"
                  :type="fieldFormat('streetNumber')"
                >
                  <b-input
                    size="is-medium"
                    @input="emitFormData"
                    v-model="form.streetNumber"
                    :disabled="disabled.streetNumber"
                  ></b-input>
                </b-field>

                <b-field
                  label="Street Name"
                  :label-position="labelPosition"
                  :message="streetNameErrors"
                  :type="fieldFormat('streetName')"
                >
                  <b-input
                    size="is-medium"
                    @input="emitFormData"
                    v-model="form.streetName"
                    :disabled="disabled.streetName"
                  ></b-input>
                </b-field>

                <b-field
                  label="Post Code"
                  :label-position="labelPosition"
                  :message="postalCodeErrors"
                  :type="fieldFormat('postalCode')"
                >
                  <b-input
                    size="is-medium"
                    @input="emitFormData"
                    v-model="form.postalCode"
                    :disabled="disabled.postalCode"
                  ></b-input>
                </b-field>

                <b-field
                  label="Suburb or City"
                  :label-position="labelPosition"
                  :message="suburbOrCityErrors"
                  :type="fieldFormat('suburbOrCity')"
                >
                  <b-input
                    @blur="$v.form.suburbOrCity.$touch()"
                    size="is-medium"
                    @input="emitFormData"
                    v-model="form.suburbOrCity"
                    :disabled="disabled.suburbOrCity"
                  ></b-input>
                </b-field>

                <b-field
                  label="Council (If known)"
                  :label-position="labelPosition"
                >
                  <b-input
                    size="is-medium"
                    @input="emitFormData"
                    v-model="form.council"
                    :disabled="disabled.council"
                  ></b-input>
                </b-field>

                <b-field
                  label="Region or State (eg. NSW or Canterbury)"
                  :label-position="labelPosition"
                  :message="regionOrStateErrors"
                  :type="fieldFormat('regionOrState')"
                >
                  <b-input
                    size="is-medium"
                    @input="emitFormData"
                    v-model="form.regionOrState"
                    :disabled="disabled.regionOrState"
                  ></b-input>
                </b-field>

                <b-field
                  label="Country"
                  :label-position="labelPosition"
                  :message="countryErrors"
                  :type="fieldFormat('country')"
                >
                  <b-input
                    size="is-medium"
                    @input="emitFormData"
                    v-model="form.country"
                    :disabled="disabled.country"
                  ></b-input>
                </b-field>
              </div>

              <div class="column">
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
                      @blur="$v.form.propertyTypeId.$touch()"
                      @input="emitFormData"
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
                      v-model="form.bedrooms"
                      @input="emitFormData"
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
                </div>
              </div>
            </div>
          </div>
        </div>
      </form-wrapper>
    </section>
  </section>
</template>

<script>
import GoogleMapsAutocomplete from '~/components/google-maps/GoogleMapAutocomplete'
import { required } from 'vuelidate/lib/validators'
import Editor from '~/components/editor'
import { mapState, mapGetters } from 'vuex'
export default {
  props: {
    form: {
      type: Object,
      required: true,
    },
  },
  components: {
    GoogleMapsAutocomplete,
    Editor,
  },
  data() {
    return {
      testingMode: true,
      hasError: false,
      errorMessage: '',
      labelPosition: 'on-border',
      checkingPropertyStatus: false,
      existingPropertySelected: '',
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
      disabled: {
        streetNumber: false,
        streetName: false,
        postalCode: false,
        suburbOrCity: false,
        council: false,
        regionOrState: false,
        country: false,
      },
      editAddressManual: false,
      manualSearch: false,
    }
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
      council: {},
      regionOrState: {
        required,
      },
      country: {
        required,
      },
    },
  },
  computed: {
    isValid() {
      return !this.$v.$invalid
    },
    nameErrors() {
      const errors = []
      if (!this.$v.form.name.$dirty) return errors
      !this.$v.form.name.required && errors.push('Name is required')
      !this.$v.form.name.email && errors.push('Email is required')
      return errors
    },
    testErrors() {
      const errors = []
      if (!this.$v.form.test.$dirty) return errors
      !this.$v.form.test.required && errors.push('Test is required')
      return errors
    },

    streetNumberErrors() {
      const errors = []
      if (!this.$v.form.streetNumber.$dirty) return errors
      !this.$v.form.streetNumber.required &&
        errors.push('Street Number is required')
      return errors
    },
    streetNameErrors() {
      const errors = []
      if (!this.$v.form.streetName.$dirty) return errors
      !this.$v.form.streetName.required &&
        errors.push('Street Name is required')
      return errors
    },
    postalCodeErrors() {
      const errors = []
      if (!this.$v.form.postalCode.$dirty) return errors
      !this.$v.form.postalCode.required &&
        errors.push('Postal Code is required')
      return errors
    },
    suburbOrCityErrors() {
      const errors = []
      if (!this.$v.form.suburbOrCity.$dirty) return errors
      !this.$v.form.suburbOrCity.required &&
        errors.push('Suburb or City is required')
      return errors
    },
    regionOrStateErrors() {
      const errors = []
      if (!this.$v.form.regionOrState.$dirty) return errors
      !this.$v.form.regionOrState.required &&
        errors.push('Region or State is required')
      return errors
    },

    countryErrors() {
      const errors = []
      if (!this.$v.form.country.$dirty) return errors
      !this.$v.form.country.required && errors.push('Country is required')
      return errors
    },

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

    // property() {
    //   return this.$store.getters['manage/listings/property']
    // },

    listing() {
      return this.$store.getters['manage/listings/listing']
    },

    lookups() {
      return this.$store.getters['manage/listings/lookups']
    },

    ...mapState({
      count: (state) => state.manage.listings.count
    }),

    ...mapGetters({
      'propertiesCanListFresh': 'manage/listings/propertiesCanListFresh',
      'propertiesCanReList': 'manage/listings/propertiesCanReList'
    })
  },
  mounted() {
    if (this.count == 0) {
      this.form.existingProperty == true
    }

    if (this.testingMode) {
      this.form.propertyTypeId = 1
      this.form.bedrooms = 1
      this.form.bathrooms = 1
      this.form.parkingSpaces = 1
      this.form.landSize = 1
    }

    if (this.listing) {
      // this.form.propertyTypeId = this.property.propertyTypeId
      // this.form.bedrooms = this.property.bedrooms
      // this.form.bathrooms = this.property.bathrooms
      // this.form.parkingSpaces = this.property.parkingSpaces
      // this.form.landSize = this.property.landSize

      // this.form = {...this.form, ...this.property}
      // this.form.property = {...this.listing.property}
      // manually validate this form
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
    getMyListings() {

    },
    emitFormData() {
      if (this.form.existingProperty === true) {
          this.$store.dispatch('manage/listings/fetchPropertiesToList', this.form.listingType)
      }
      this.$emit('stepUpdated', { data: this.form, isValid: this.isValid })
    },
    setAddressFeilds(data) {
      this.resetAddressFields()

      this.form.googleAddress = data.googleAddress
      this.form.latitude = data.latitude
      this.form.longitude = data.longitude

      this.form.streetNumber = data.streetNumber
      this.$v.form.streetNumber.$touch()
      if (!this.form.streetNumber) {
        this.disabled.streetNumber = false
      }
      this.form.streetName = data.streetName
      this.$v.form.streetName.$touch()
      if (!this.form.streetName) {
        this.disabled.streetName = false
      }
      this.form.postalCode = data.postalCode
      this.$v.form.postalCode.$touch()
      if (!this.form.postalCode) {
        this.disabled.postalCode = false
      }
      this.form.suburbOrCity = data.suburbOrCity
      this.$v.form.suburbOrCity.$touch()
      if (!this.form.suburbOrCity) {
        this.disabled.suburbOrCity = false
      }
      this.form.council = data.council
      if (!this.form.council) {
        this.disabled.council = false
      }
      this.form.regionOrState = data.regionOrState
      this.$v.form.regionOrState.$touch()
      if (!this.form.regionOrState) {
        this.disabled.regionOrState = false
      }
      this.form.country = data.country
      this.$v.form.country.$touch()

      this.$emit('stepUpdated', { data: this.form, isValid: this.isValid })
    },
    async handleAddress(data) {
      this.checkingPropertyStatus = true
      this.hasError = false
      this.errorMessage = ''
      var payload = {
        googleAddress: data.googleAddress,
      }
      // check to see if this address can be created or not
      try {
        const res = await this.$store.dispatch(
          'manage/listings/canCreateAddressCheck',
          payload
        )

        if (res.canCreate == true) {
          this.form.canCreateProperty = true
          this.setAddressFeilds(data)
        } else {
          // address is not good to go
          this.hasError = true
          this.errorMessage =
            'This property cannot be created because it already exists'
          this.form.canCreateProperty = false
        }
      } catch (error) {
        this.hasError = true
        this.errorMessage = error
      } finally {
        this.checkingPropertyStatus = false
      }
    },
    resetAddressFields() {
      this.disabled.streetNumber = true
      this.disabled.streetName = true
      this.disabled.postalCode = true
      this.disabled.suburbOrCity = true
      this.disabled.council = true
      this.disabled.regionOrState = true
    },
    existingPropertySubmit() {
      
    }
  },
  watch: {
    // editAddressManual: function (val) {
    //   if (val == true && this.listing.canEditAddress == true) {
    //     this.disabled.streetNumber = false
    //     this.disabled.streetName = false
    //     this.disabled.postalCode = false
    //     this.disabled.suburbOrCity = false
    //     this.disabled.council = false
    //     this.disabled.regionOrState = false
    //     this.disabled.country = false
    //   } else {
    //     this.disabled.streetNumber = true
    //     this.disabled.streetName = true
    //     this.disabled.postalCode = true
    //     this.disabled.suburbOrCity = true
    //     this.disabled.council = true
    //     this.disabled.regionOrState = true
    //     this.disabled.country = true
    //   }
    // },
    listing(val) {
      if (this.listing.canEditAddress == true) {
        this.disabled.streetNumber = false
        this.disabled.streetName = false
        this.disabled.postalCode = false
        this.disabled.suburbOrCity = false
        this.disabled.council = false
        this.disabled.regionOrState = false
        this.disabled.country = false
      } else {
        // this.disabled.streetNumber = false
        // this.disabled.streetName = false
        // this.disabled.postalCode = false
        // this.disabled.suburbOrCity = false
        // this.disabled.council = false
        // this.disabled.regionOrState = false
        // this.disabled.country = false
      }
    },
  },
}
</script>
