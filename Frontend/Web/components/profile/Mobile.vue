<template>
  <div class="card m-t-md m-r-sm m-l-sm" v-if="profile">
    <div class="card-content">
      <!-- isValid {{isValid}} -->
      <div class="confirm-mobile-section" v-if="!profile.mobileConfirmed">
        <div class="mobile-number">
          <p class="is-size-5 is-size-5-mobile">Let's double check your mobile number</p>
          <b-field class="m-t-md"
            label="Country"
            :label-position="labelPosition"
            
          >
            <b-select placeholder="Country Code" v-model="form.mobileCountryCode">
              <option
                v-for="(item, index) in this.countryCodes"
                :key="index"
                :id="item.name"
                :value="item.value"
              >
                {{ item.name }}
              </option>
            </b-select>
          </b-field>

          <b-field label="Mobile Number" class="m-t-md"
            :label-position="labelPosition">
            <p class="control" v-if="form.mobileCountryCode">
              <span class="button is-static">{{ form.mobileCountryCode }}</span>
            </p>
            <b-input
              type="tel"
              placeholder="Mobile Number"
              v-mask="phoneMask"
              v-model="form.mobileNumber"
              :disabled="form.mobileCountryCode == ''"
            >
            </b-input>
            <p class="control">
              <button
                v-if="!form.mobileCountryCode == ''"
                class="button is-success"
                :disabled="!mobileLengthValid"
                @click="submitMobile"
              >
                Get Code
              </button>
            </p>
          </b-field>

          <div class="notification is-success" v-if="submittingMobile">
            <i class="fas fa-spinner fa-spin"></i>
            Submitting Number...
          </div>
          <div class="notification is-danger" v-if="submitMobileErrorMsg">
            {{ submitMobileErrorMsg }}
          </div>
        </div>

        <div class="enter-code" v-if="mobileSubmitted">
          <br />
          <h3 class="is-size-5 is-size-6-mobile">
            Great! We have have just sent you a code to your phone. Please enter
            your code received.
          </h3>

          <b-field class="m-t-md">
            <b-input
              type="number"
              placeholder="####"
              v-mask="'####'"
              v-model="code"
            >
            </b-input>
            <p class="control">
              <button
                class="button is-success"
                :disabled="!code"
                @click="submitCode"
              >
                GO
              </button>
            </p>
          </b-field>
        </div>
      </div>
      <div v-else>
        <h3 class="is-size-5 is-size-6-mobile m-b-md">Great! Your mobile number is confirmed.</h3>
        <b-field label="Mobile Number" :label-position="labelPosition">
          <b-input
            disabled="true"
            size="is-medium"
            :value="form.mobileCountryCode + ' ' + form.mobileNumber"
          ></b-input>
        </b-field>
      </div>
    </div>
  </div>
</template>

<script>
import { required, email, maxValue } from 'vuelidate/lib/validators'
import { mapState, mapGetters } from 'vuex'

export default {
  props: {
    form: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      loaded: false,
      submitMobileErrorMsg: null,
      hasError: true,
      submittingMobile: false,
      mobileSubmitted: false,
      submittingCode: false,
      codeSubmitted: false,
      labelPosition: 'on-border',
      countryCodes: [
        { name: 'AU', value: '+61' },
        { name: 'NZ', value: '+64' },
      ],
      phoneMask: '',
      code: '',
    }
  },
  validations: {
    form: {
      mobileNumber: {
        required,
      },
    },
  },
  computed: {
    isValid() {
      return this.confirmed ? true : false
    },
    mobileNumberErrors() {
      const errors = []
      if (!this.$v.form.mobileNumber.$dirty) return errors
      !this.$v.form.mobileNumber.required &&
        errors.push('Mobile number is required')
      return errors
    },
    mobileLengthValid() {
      
      return this.form.mobileNumber && this.form.mobileNumber.length > 7 ? true : false
    },

    ...mapState({
      profile: (state) => state.auth.profile,
      confirmed: (state) => state.auth.profile.mobileConfirmed
    }),
    countryCode() {
      return this.form.mobileCountryCode
    }
  },
  mounted() {
    if (this.profile) {
      this.loaded = true
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
    async submitMobile() {

      this.submitMobileErrorMsg = null
      try {
        this.submittingMobile = true
        
        var payload = {
          mobileNumber: this.form.mobileNumber,
          countryCode: this.form.mobileCountryCode
        }
        const validationResult = await this.$store.dispatch(
          'auth/submitMobile',
          payload
        )

        if (validationResult) {
          this.mobileSubmitted = true
        }
      } catch (error) {
        this.submitMobileErrorMsg = 'There was an issue validating your mobile number'
      } finally {
        this.submittingMobile = false
      }
    },
    async submitCode() {
      try {
        var payload = {
          code: this.code,
        }
        this.submittingCode = true
        const codeResult = await this.$store.dispatch(
          'auth/submitCode',
          payload
        )
      } catch (error) {
        this.error = 'There was an issue validating your mobile number'
      } finally {
        this.submittingCode = false
      }
    },
  },
  watch: {
    countryCode: function (val) {
      if (!this.form.mobileNumber) {
        // this.form.mobileNumber = val
      }

      if (val === '+61') {
        this.phoneMask = '### ### ####'
      } else {
        this.phoneMask = '### ### ####'
      }
    },
    isValid(val) {
      if (val) {
        this.emitFormData()
      }
    }
  },
}
</script>

<style lang="scss" scoped></style>
