<template>
  <!-- <section class="section"> -->
  <div class="card">
    <div class="card-content">
      <p class="is-size-4 is-size-5-mobile m-b-md">Ad Details</p>
      <div class="notification is-success" v-if="success">All went well</div>
      <div class="notification is-danger" v-if="error">
        {{ error }}
      </div>
      <div class="columns">
        <div class="column">
          <div class="block">
            <h4><b>Is this property 'For Sale' or 'For Rent'?</b></h4>
            <br />
            <div>
              <b-radio
                v-model="form.listingType"
                @input="emitFormData"
                @blur="$v.form.price.$touch()"
                :disabled="locked"
                name="name"
                value="Sale"
                native-value="Sale"
              >
                Sale
              </b-radio>
              <b-radio
                v-model="form.listingType"
                @input="emitFormData"
                @blur="$v.form.price.$touch()"
                :disabled="locked"
                name="name"
                value="Rent"
                native-value="Rent"
              >
                Rent
              </b-radio>
            </div>
          </div>

          <div class="prices">
            <div
              class="columns is-1-mobile is-0-tablet is-3-desktop is-8-widescreen is-2-fullhd"
            >
              <div class="column is-4">
                <div v-if="form.listingType === 'Rent'">
                  <b-field :type="fieldFormat('price')" expanded>
                    <p class="control">
                      <span class="button is-static">$</span>
                    </p>
                    <b-input
                      type="number"
                      expanded
                      min="minPrice"
                      max="maxPrice"
                      placeholder="0"
                      v-model="form.price"
                      @input="emitFormData"
                      @blur="$v.form.price.$touch()"
                    ></b-input>
                    <p class="control">
                      <span class="button is-static">Per Week</span>
                    </p>
                  </b-field>
                  <p class="help is-danger" v-if="priceErrors">
                    {{ priceErrors[0] }}
                  </p>
                </div>

                <div v-if="form.listingType === 'Sale'">
                  <b-field
                    class="control"
                    :type="fieldFormat('price')"
                    expanded
                  >
                    <p class="control">
                      <span class="button is-static">$</span>
                    </p>
                    <b-input
                      expanded
                      type="number"
                      min="minPrice"
                      max="maxPrice"
                      placeholder="0"
                      v-model="form.price"
                      @input="emitFormData"
                      @blur="$v.form.price.$touch()"
                    ></b-input>
                  </b-field>
                  <p class="help is-danger" v-if="priceErrors">
                    {{ priceErrors[0] }}
                  </p>
                </div>
              </div>
              <div class="column">
                <div class="m-l-md" v-if="form.price">
                  <span class="is-size-5 label">{{
                    form.price | currency
                  }}</span>
                </div>
              </div>
            </div>
          </div>
          <br />

          <div class="dates">
            <h1><b>When did you want your Ad to start?</b></h1>
            <br />
            <div class="columns is-gapless">
              <div class="column is-half">
                <b-field class="is-half">
                  <b-datepicker
                    expanded
                    ref="datepicker"
                    v-model="dateListingStarts"
                    @input="emitFormData"
                    @blur="$v.form.dateListingStarts.$touch()"
                    :disabled="listing && listing.confirmed"
                    placeholder="Select a date"
                    mobile-native
                    :min-date="minDate"
                  >
                  </b-datepicker>
                  <b-button
                    @click="$refs.datepicker.toggle()"
                    icon-left="calendar-today"
                    type="is-primary"
                  />
                </b-field>
              </div>
            </div>

            <div v-if="dateListingStarts">
              <p>This ad will run for <b>90 days.</b></p>
              <b>{{ formattedDateListingStarts }}</b> to
              <b>{{ formattedDateListingEnds }}</b>
              <!-- <b>{{ dateListingStarts.toLocaleDateString() }}</b> and will
                finish on <b>{{ dateListingExpires.toLocaleDateString() }}</b> -->
            </div>
          </div>

          <br /><br />

          <div class="contact">
            <h1><b>How do you wish to be contacted?</b></h1>
            <br />
            <b-field
              label="Email"
              :label-position="labelPosition"
              :message="emailErrors"
              :type="fieldFormat('contactEmail')"
            >
            <!-- replaced with debounce below -->
            <!-- @input="emitFormData" -->
              <b-input
                size="is-medium" 
                @input="emitFormData"
                @blur="$v.form.contactEmail.$touch()"
                v-model="form.contactEmail"
                placeholder="Contact Email"
              ></b-input>
            </b-field>
            <p class="is-size-7">
              {{ siteName }} <b>WILL NOT</b> show this email anywhere within
              your Advert. Enquiries will be forwarded safely to your email
              address through this site. This protects you from email spammers.
            </p>
          </div>

          <div class="contactNumber m-t-lg">
            
            
            <b-field class=""
              label="Contact Number (Landline or Mobile)"
              :label-position="labelPosition"
              :message="contactNumberErrors"
              :type="fieldFormat('contactNumber')"
            >
              <b-input
                @input="emitFormData"
                @blur="$v.form.contactNumber.$touch()"
                v-model="form.contactNumber"
                size="is-medium"
                placeholder="Contact Number"
                type="number"
                min="0"
              >
              </b-input>
            </b-field>
            <b-checkbox v-model="form.showNumber" @input="emitFormData" @blur="$v.form.showNumber.$touch()" class="checkbox text-danger m-b-md m-t-md"
              ><b>Show Contact Number in Ad?</b>
            </b-checkbox>
            <p v-if="form.showNumber" class="is-size-7">
              {{ siteName }} <b>WILL</b> show this contact number within your
              Advert. If you do not wish to be contacted by your contact number, please unselect this option.
            </p>
            <!-- <p v-if="!showNumber">
              {{ siteName }} <b>WILL NOT</b> show this contact number within your
              Advert. 
            </p> -->
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- </section> -->
</template>

<script>
import Editor from '~/components/editor'
import { required, email, maxValue, minValue } from 'vuelidate/lib/validators'
import { mapGetters, mapState } from 'vuex'

const greaterThanZero = (value) => value > 0

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
      dateListingStarts: null,
      dateListingExpires: null,
      testing: false,
      minDate: null,
      minPrice: 50,
      maxPrice: 4000
    }
  },
  components: {
    Editor,
  },
  validations() {
    return {
      form: {
        price: {
          required,
          maxValue: maxValue(this.maxPrice),
          minValue: minValue(this.minPrice),
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
        }
      },
    }
  },
  computed: {
    isValid() {
      return !this.$v.$invalid
    },

    listingType() {
      return this.form.listingType
    },

    locked() {
      if (this.listing) {
        if (this.listing) {
          return true
        }
      } else {
        return false
      }
    },

    ...mapGetters({
      listing: 'manage/listings/listing',
      userRequestOptions: 'manage/listings/userRequestOptions',
      siteName: 'manage/listings/sitName',
      profile: 'auth/profile'
    }),

    ...mapState({
      siteName: 'siteName',
    }),

    listingTypeErrors() {
      const errors = []
      if (!this.$v.form.listingType.$dirty) return errors
      !this.$v.form.listingType.required &&
        errors.push('Listing Type is required')
      return errors
    },

    priceErrors() {
      const errors = []
      if (!this.$v.form.price.$dirty) return errors
      !this.$v.form.price.required && errors.push('Price is required')
      !this.$v.form.price.maxValue &&
        errors.push(`price cannot be higher than ${this.maxPrice}`)
      !this.$v.form.price.minValue &&
        errors.push(`price cannot be lower than ${this.minPrice}`)

      return errors
    },

    emailErrors() {
      const errors = []
      if (!this.$v.form.contactEmail.$dirty) return errors
      !this.$v.form.contactEmail.required &&
        errors.push('Email address is required')
      !this.$v.form.contactEmail.email &&
        errors.push('This must be a valid email address')
      return errors
    },

    contactNumberErrors() {
      const errors = []
      if (!this.$v.form.contactNumber.$dirty) return errors
      !this.$v.form.contactNumber.required &&
        errors.push('Contact Number is required')
      return errors
    },
    // listingDate() {
    //   return this.listing.dateListingStarts || null
    // },

    formattedDateListingStarts() {
      return this.dateListingStarts != null
        ? new Date(Date.parse(this.dateListingStarts)).toLocaleDateString()
        : null
    },
    formattedDateListingEnds() {
      return this.dateListingExpires != null
        ? new Date(Date.parse(this.dateListingExpires)).toLocaleDateString()
        : null
    },
  },
  mounted() {
    this.scrollToTop()
    // ready up the checkboxes for use
    if (this.form.dateListingStarts) {
      this.dateListingStarts = new Date(this.form.dateListingStarts)
    } else {
      this.dateListingStarts = new Date()
    }

    if (this.dateListingStarts) {
      Date.prototype.addDays = function (days) {
        var date = new Date(this.valueOf())
        date.setDate(date.getDate() + days)
        return date
      }
      this.dateListingExpires = this.dateListingStarts.addDays(90)
    }

    if (!this.form.contactEmail) {
      this.form.contactEmail = this.profile.email
    }

    if (this.form.listingType == 'Rent') {
        this.minPrice = 50
        this.maxPrice = 4000
    }
    if (this.form.listingType == 'Sale') {
      this.minPrice = 50000
      this.maxPrice = 10000000
    }

    this.minDate = new Date(Date.now() - 864e5)

    // if (this.form.dateListingExpires) {
    //    this.dateListingExpires = new Date(Date.parse(this.form.dateListingExpires)).toLocaleDateString()
    // }
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
      if (this.dateListingStarts) {
        Date.prototype.addDays = function (days) {
          var date = new Date(this.valueOf())
          date.setDate(date.getDate() + days)
          return date
        }
        this.dateListingExpires = this.dateListingStarts.addDays(90)
      }

      this.form.dateListingStarts = this.dateListingStarts

      this.$emit('stepUpdated', { data: this.form, isValid: this.isValid })

      
    },
  },
  watch: {
    listingType(val) {
      if (val == 'Rent') {
        this.minPrice = 50
        this.maxPrice = 4000
        this.form.price = 50
      }
      if (val == 'Sale') {
        this.minPrice = 50000
        this.maxPrice = 10000000
        this.form.price = 50000
      }
    },
  },
}
</script>

<style lang="scss" scoped>
// .prices {
//   display: flex;
// }

.contactNumber {
  margin-top: 20px; 
}
</style>
