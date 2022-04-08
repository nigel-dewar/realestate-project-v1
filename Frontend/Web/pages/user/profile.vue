<template>
  <div>
    <div class="container" v-if="loaded">
      <div class="card m-t-md m-r-sm m-l-sm" v-if="!form.userProfileComplete">
        <div class="card-content">
          <p class="is-size-6 is-size-7-mobile">
            It looks like your profile is not complete. Lets get this filled out
            quickly and then we can create some Ads
          </p>
        </div>
      </div>
      <!-- can proceed: {{ canProceed }}
      <button
        v-if="profile"
        :disabled="!canProceed"
        @click="save"
        class="button is-success is-large float-right"
      >
        Save
      </button> -->

      <keep-alive>
        <component
          :is="activeComponent"
          :form="form"
          @stepUpdated="mergeFormData"
          ref="activeComponent"
        />
      </keep-alive>
    </div>

    <div class="container">
      <div class="has-text-centered p-t-md p-b-md">

        <button
          v-if="!isFirstStep"
          @click.prevent="previousStep"
          class="button is-medium"
        >
          Previous
        </button>

        <button
          v-if="!isLastStep"
          @click.prevent="nextStep"
          :disabled="!canProceed"
          class="button is-medium is-success"
        >
          Continue
        </button>
        <button
          v-else
          @click="confirm"
          :disabled="!canProceed"
          class="button is-medium is-success"
        >
          Confirm
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import UserDetails from '~/components/profile/UserDetails.vue'
import MobileNumber from '~/components/profile/Mobile.vue'

import { required, email, maxValue } from 'vuelidate/lib/validators'
export default {
  layout: 'manage',
  components: {
    UserDetails,
    MobileNumber,
  },
  data() {
    return {
      loaded: false,
      hasError: true,
      labelPosition: 'on-border',
      activeStep: 1,
      profileIsComplete: false,
      isTesting: false,
      steps: ['UserDetails', 'MobileNumber'],
      canProceed: false,
      form: {
        firstName: '',
        lastName: '',
        displayName: '',
        email: '',
        mobileNumber: '',
        mobileCountryCode: '',
        bio: '',
        userProfileComplete: false,
        userTypes: [],
      },
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
      mobileCountryCode: {
        required,
      },
    },
  },
  async mounted() {
    const profile = await this.$store.dispatch('auth/getProfile')
    const userTypes = await this.$store.dispatch('auth/getUserTypes')

    if (profile) {
      this.form = { ...profile }
      this.loaded = true
      this.checkValid()
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
  },
  methods: {
    fieldFormat(e) {
      if (!e) return null
      if (!this.$v.form[e].$dirty) return null
      if (this.$v.form[e].$anyError) return 'is-danger'
      return 'is-success'
    },
    mergeFormData({ data, isValid }) {
      this.form = { ...this.form, ...data }
      this.canProceed = isValid
    },
    checkValid() {
      this.$nextTick(() => {
        this.canProceed = this.$refs.activeComponent.isValid
      })
    },
    save() {
      const payload = {
        firstName: this.form.firstName,
        lastName: this.form.lastName,
        displayName: this.form.displayName,
        email: this.form.email,
        mobileCountryCode: this.form.mobileCountryCode,
        mobileNumber: this.form.mobileNumber,
        bio: this.form.bio,
        userTypes: this.form.userTypes,
      }

      return this.$store
        .dispatch('auth/saveProfile', payload)
        .then((_) => {
          return Promise.resolve()
        })
        .catch(({ message, code }) => {
          this.$toast.error(message, {
            duration: 10000,
            position: 'bottom-center',
          })
          return Promise.reject()
        })
    },
    confirm() {
      this.save().then(() => {
        if (this.profile.userProfileComplete) {
          this.$router.push('/manage/ads/create')
        } else {
          this.$router.push('/manage/ads')
        }
      })
    },
    nextStep() {
      this.$nextTick(() => {
        this.canProceed = this.$refs.activeComponent.isValid
      })

      // this.save()
      this.activeStep++
    },
    previousStep() {
      this.activeStep--
      // this.$router.push({
      //         name: 'manage.create.ad',
      //         query: { listing: this.listing.id, step: this.activeStep },
      //       })
      this.canProceed = true
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

<style lang="scss" scoped></style>
