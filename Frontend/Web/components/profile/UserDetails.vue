<template>
  <div v-if="loaded">
    <div class="card m-t-md m-r-sm m-l-sm">
      <div class="card-content">
        <p class="is-size-4 m-b-md">Your Profile</p>
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
                @input="emitFormData"
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
                @input="emitFormData"
                @blur="$v.form.lastName.$touch()"
                v-model="form.lastName"
              ></b-input>
            </b-field>
          </div>
        </div>
        <!-- DisplayName and Email -->
        <div class="columns">
          <div class="column">
            <span class="is-size-6"
              >Your Display Name is what other users will see:</span
            >
            <br /><br />
            <b-field
              label="Display Name"
              :label-position="labelPosition"
              :message="displayNameErrors"
              :type="fieldFormat('displayName')"
            >
              <b-input
                size="is-medium"
                @input="emitFormData"
                @blur="$v.form.displayName.$touch()"
                v-model="form.displayName"
              ></b-input>
            </b-field>
          </div>
          <div class="column"></div>
        </div>
        <!-- <div class="columns">
          <div class="column">
            <span class="is-size-6"
              >Email address you recieve messages to:</span
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
                @input="emitFormData"
                @blur="$v.form.email.$touch()"
                v-model="form.email"
                disabled="true"
              ></b-input>
            </b-field>
          </div>
          <div class="column"></div>
        </div> -->
        <div class="columns">
          <div class="column">
            <div class="has-text-centered">
              <p class="is-size-5">
                How would you desribe yourself here? I am a...
              </p>
              <!-- <p class="is-size-6">
                Please select at least 1. It helps us serve you better
              </p> -->
              <br />
              <form @submit.prevent="">
                <div class="block">
                  <b-checkbox
                    v-for="(item, index) in this.userTypes"
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
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { required, email, maxValue } from 'vuelidate/lib/validators'
import { mapGetters } from 'vuex'

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
      hasError: true,
      labelPosition: 'on-border',
      userTypes: [],
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
      userTypes: {},
    },
  },
  computed: {
    isValid() {
      return !this.$v.$invalid
    },
    email() {
      return this.$store.getters['auth/userEmail']
    },
    userTypesLookups() {
      return this.$store.getters['auth/userTypes']
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
    checkedUserTypes() {
      return this.userTypes.filter((x) => x.checked == true)
    },
    profile() {
      return this.$store.getters['auth/profile']
    },
  },
  mounted() {
    // ready up the checkboxes for use
    if (this.profile) {
      if (this.form.email == null) {
        this.form.email = this.email
      }

      if (this.userTypesLookups) {
        this.userTypesLookups.map((x) => {
          let found = false
          if (this.profile.userTypes.length > 0) {
            found = this.profile.userTypes.find((f) => f.id == x.id)
              ? true
              : false
          }
          // let check = this.listing.features.find(f => f.id == x.id)
          var checkbox = {
            id: x.id,
            name: x.name,
            checked: found,
          }
          this.userTypes.push(checkbox)
        })
        
      }

      this.loaded = true
      
      // this.$v.$touch()
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
      // this.$v.$touch()
      this.$emit('stepUpdated', { data: this.form, isValid: this.isValid })
    },
  },
  watch: {
    checkedUserTypes: function (val) {
      this.form.userTypes = this.userTypes.filter((x) => x.checked == true)
      this.emitFormData()
    },
  },
}
</script>

<style lang="scss" scoped></style>
