<template>
  <section class="section" v-if="loaded">
    <div class="card">
      <header class="card-header">
        <p class="card-header-title">
          Coolio, let's fill our your profile and then we can create your Ad
        </p>
      </header>
      <div class="card-content">
        <br />
        <div class="columns">
          <div class="column">
            <b-field
              label="First Name"
              :label-position="labelPosition"
              :message="firstNameErrors"
              :type="fieldFormat('firstName')"
            >
              <b-input
                size="is-medium"
                @blur="$v.form.firstName.$touch()"
                v-model="form.firstName"
              ></b-input>
            </b-field>
          </div>
          <div class="column">
            <b-field
              label="Last Name"
              :label-position="labelPosition"
              :message="lastNameErrors"
              :type="fieldFormat('lastName')"
            >
              <b-input
                size="is-medium"
                @blur="$v.form.lastName.$touch()"
                v-model="form.lastName"
              ></b-input>
            </b-field>
          </div>
        </div>
        <!-- DisplayName and Email -->
        <div class="columns">
          <div class="column">
            <span>Your Display Name is what other users will see.</span>
            <br /><br />
            <b-field
              label="Display Name"
              :label-position="labelPosition"
              :message="displayNameErrors"
              :type="fieldFormat('displayName')"
            >
              <b-input
                placeholder="Display Name"
                size="is-medium"
                @blur="$v.form.displayName.$touch()"
                v-model="form.displayName"
              ></b-input>
            </b-field>
          </div>
          <div class="column">
            <span
              >We wont show this Email anywhere on the site. This email address
              is how</span
            >
            <br /><br />
            <b-field
              label="Email"
              :label-position="labelPosition"
              :message="emailErrors"
              :type="fieldFormat('email')"
            >
              <b-input
                size="is-medium"
                @blur="$v.form.email.$touch()"
                v-model="form.email"
              ></b-input>
            </b-field>
          </div>
        </div>
        <!-- Mobile Number -->
        <div class="columns">
          <div class="column">
            <b-field
              label="Mobile Number"
              :label-position="labelPosition"
              :message="mobileNumberErrors"
              :type="fieldFormat('mobileNumber')"
            >
              <b-input
                size="is-medium"
                @blur="$v.form.mobileNumber.$touch()"
                v-model="form.mobileNumber"
              ></b-input>
            </b-field>

            <b-field>
              {{countryCode}}
              <b-select 
                  placeholder="Country" 
                  v-model="countryCode"
                >
                <option v-for="(item, index) in this.countryCodes"
                  :key="index"
                  :id="item.name"
                  :value="item.value"
                  >{{item.name}}</option>
              </b-select>
              <b-input 
                type="tel"
    
                v-mask="'+## ### ### ###'"
                v-model="form.mobileNumber"
                >
                </b-input>
              <p class="control">
                <button class="button is-success">Validate</button>
              </p>
            </b-field>

          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import { required, email } from 'vuelidate/lib/validators'
import { mask } from 'vue-the-mask'
export default {
  data() {
    return {
      loaded: false,
      hasError: true,
      labelPosition: 'on-border',
      form: {
        firstName: '',
        lastName: '',
        displayName: '',
        email: '',
        mobileNumber: '',
        bio: '',
        userTypes: [],
        userProfileComplete: false,
      },
      countryCodes: [
        { name: 'AU', value: '+61' },
        { name: 'NZ', value: '+64' },
      ],
      countryCode: ''
    }
  },
  validations: {
    form: {
      firstName: {
        required,
      },
      lastName: {
        required,
      },
      displayName: {
        required,
      },
      email: {
        required,
        email,
      },
      mobileNumber: {
        required,
      },
    },
  },
  computed: {
    isValid() {
      return !this.$v.$invalid
    },
    profile() {
      return this.$store.getters['auth/profile']
    },
    formInValid() {
      return this.$v.form.$invalid
    },
    formInfo() {
      return this.$v.form
    },

    firstNameErrors() {
      const errors = []
      if (!this.$v.form.firstName.$dirty) return errors
      !this.$v.form.firstName.required && errors.push('First Name is required')
      return errors
    },
    lastNameErrors() {
      const errors = []
      if (!this.$v.form.lastName.$dirty) return errors
      !this.$v.form.lastName.required && errors.push('Last Name is required')
      return errors
    },
    displayNameErrors() {
      const errors = []
      if (!this.$v.form.displayName.$dirty) return errors
      !this.$v.form.displayName.required &&
        errors.push('Display Name is required')
      return errors
    },
    emailErrors() {
      const errors = []
      if (!this.$v.form.email.$dirty) return errors
      !this.$v.form.email.required && errors.push('Email is required')
      return errors
    },
    mobileNumberErrors() {
      const errors = []
      if (!this.$v.form.mobileNumber.$dirty) return errors
      !this.$v.form.mobileNumber.required &&
        errors.push('Mobile number is required')
      return errors
    },
  },
  mounted() {
    this.loaded = true
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
    }
  },
  watch: {
    countryCode: function(val) {
      this.form.mobileNumber = val
    }
  }
}
</script>

<style lang="scss" scoped></style>
