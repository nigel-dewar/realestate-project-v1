<template>
  <div class="card manage-card">
    <header class="card-header card-section">
      <p class="card-header-title">Property Address</p>
    </header>
    <div class="card-content card-section" v-if="form.id">
      <form>

        

        <!-- <div class="field">
          <div class="control">
            <GoogleMapsAutocomplete
              @address-response="handleAddress"
              :placeHolder="'Search for property address...'"
            />
          </div>
        </div> -->


        <div v-if="listing.canEditAddress">
          <p>Most of the time the Google address search will get the address correct. But... sometimes it just does not. You can edit this address only once so please be careful to get this correct.</p>
          <br>
          <b-checkbox v-model="editAddressManual" class="checkbox text-danger">I understand. Let me edit the address.</b-checkbox>
          
          <div v-if="editAddressManual" class="is-warning">
            <br>
            <p>Notice!: This edit will be rejected if the address you enter is already in use with a Live Advert. </p>
            <p>Need Help? Contact Realestateify: <a href="mailto:support@realestateify.com">support@realestateify.com</a></p>
          </div>

        </div>

        <div v-if="!listing.canEditAddress">
            Our records show this address has been previously edited.
            If you need this edited again, please contact support at <a href="mailto:support@realestateify.com">support@realestateify.com</a>
        </div>

        <br><br>

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

        <b-field label="Council (If known)" :label-position="labelPosition">
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
      </form>
    </div>
  </div>
</template>

<script>
import GoogleMapsAutocomplete from '~/components/google-maps/GoogleMapAutocomplete'
import { required } from 'vuelidate/lib/validators'
export default {
  props: {
    form: {
      type: Object,
      required: true,
    },
  },
  components: {
    GoogleMapsAutocomplete,
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
      council: {},
      regionOrState: {
        required,
      },
      country: {
        required,
      },
    },
  },
  data() {
    return {
      disabled: {
        streetNumber: true,
        streetName: true,
        postalCode: true,
        suburbOrCity: true,
        council: true,
        regionOrState: true,
        country: true,
      },
      editAddressManual: false,
      testingMode: true,
      hasError: true,
      labelPosition: 'on-border',
    }
  },
  computed: {
    isValid() {
      return !this.$v.$invalid
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
    listing() {
      return this.$store.getters['manage/listings/listing']
    },
  },
  mounted() {
    // this.form = { ...this.form, ...this.property }
    if (this.listing) {
      // this.form = { ...this.form, ...this.listing }
      // manually validate this form
      this.$v.$touch()
      this.emitFormData()
    //   this.emitFormData()
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
    handleAddress(data) {
      this.resetAddressFields()
      // this.form.full_address = data.formatted_address
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
    resetAddressFields() {
      this.disabled.streetNumber = true
      this.disabled.streetName = true
      this.disabled.postalCode = true
      this.disabled.suburbOrCity = true
      this.disabled.council = true
      this.disabled.regionOrState = true
    },
  },
  watch: {
    editAddressManual: function (val) {
      if (val == true && this.listing.canEditAddress == true) {
        this.disabled.streetNumber = false
        this.disabled.streetName = false
        this.disabled.postalCode = false
        this.disabled.suburbOrCity = false
        this.disabled.council = false
        this.disabled.regionOrState = false
        this.disabled.country = false
      } else {
        this.disabled.streetNumber = true
        this.disabled.streetName = true
        this.disabled.postalCode = true
        this.disabled.suburbOrCity = true
        this.disabled.council = true
        this.disabled.regionOrState = true
        this.disabled.country = true
      }
    },
  },

}
</script>

<style lang="scss" scoped></style>
