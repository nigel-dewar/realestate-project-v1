<template>
    <div class="container has-text-centered">
      <div>
        <div class="column is-4 is-offset-4">
        <h3 class="is-size-4 has-text-grey">Sign Up</h3>
        <div class="card m-t-md m-b-md">
          <div class="card-content">
            <div class="notification is-success" v-if="success">
              <button @click="closeMessageBox" class="delete"></button>
              <div>
                <p>
                  Woohoo! You're now in the system. We have sent you an email to
                  activate your account. Please Ensure you check your SPAM folder for this email.
                </p>
                <br>
                <p>Once You have confirmed your email address, you will be able to Sign In.</p>
                <br>
                <!-- <nuxt-link :to="{ name: 'resend-verification'}">Not recieve the email? Resend verification email</nuxt-link> -->
                <!-- <button class="button is-primary" @click="goSignIn">Sign In</button> -->
              </div>
            </div>
            <div class="notification is-danger" v-if="error">
              <button @click="closeMessageBox" class="delete"></button>
              {{ error.message }}
            </div>
            <form v-if="!success">
              <div class="field">
                <div class="control">
                  <input
                    v-model="form.email"
                    @blur="$v.form.email.$touch()"
                    class="input is-medium"
                    type="email"
                    placeholder="Your Email"
                  />
                  <div v-if="$v.form.email.$error" class="form-error">
                    <span v-if="!$v.form.email.required" class="help is-danger"
                      >Email is required</span
                    >
                    <span
                      v-if="!$v.form.email.emailValidator"
                      class="help is-danger"
                      >Email address is not valid</span
                    >
                  </div>
                </div>
              </div>

              <div class="field">
                <div class="control">
                  <input
                    v-model="form.password"
                    @blur="$v.form.password.$touch()"
                    class="input is-medium"
                    type="password"
                    placeholder="Your Password"
                    autocomplete="new-password"
                  />
                  <div v-if="$v.form.password.$error" class="form-error">
                    <span
                      v-if="!$v.form.password.required"
                      class="help is-danger"
                      >Password is required</span
                    >
                    <span
                      v-if="!$v.form.password.minLength"
                      class="help is-danger"
                      >Password minimum length is 6 letters</span
                    >
                  </div>
                </div>
              </div>
              <div class="field">
                <div class="control">
                  <input
                    v-model="form.passwordConfirmation"
                    @blur="$v.form.passwordConfirmation.$touch()"
                    class="input is-medium"
                    type="password"
                    placeholder="Password Confirmation"
                    autocomplete="off"
                  />
                  <div
                    v-if="$v.form.passwordConfirmation.$error"
                    class="form-error"
                  >
                    <span
                      v-if="!$v.form.passwordConfirmation.required"
                      class="help is-danger"
                      >Password is required</span
                    >
                    <span
                      v-if="!$v.form.passwordConfirmation.sameAs"
                      class="help is-danger"
                      >Password confirmation should be the same as
                      password</span
                    >
                  </div>
                </div>
              </div>
              <button
                @click.prevent="register"
                :disabled="$v.form.$invalid"
                type="submit"
                class="button is-block is-info is-large is-fullwidth"
              >
                Sign Up
              </button>
            </form>
          </div>
          <!-- <figure class="avatar">
              <img src="https://via.placeholder.com/300">
            </figure> -->
        </div>
        <p class="has-text-grey">
          Already have an account? <nuxt-link to="/login">Login</nuxt-link>
        </p>
        <p class="has-text-grey">
          <a href="../">Need Help?</a>
        </p>
      </div>
      </div>
    </div>
</template>

<script>
import {
  required,
  email,
  minLength,
  url,
  sameAs,
} from 'vuelidate/lib/validators'
export default {
  middleware: 'guest',
  data() {
    return {
      form: {
        userName: null,
        email: null,
        password: null,
        passwordConfirmation: null,
      },
      testing: false,
      success: null,
      error: null,
      showCheckEmailMessage: false,
    }
  },
  validations: {
    form: {
      email: {
        emailValidator: email,
        required,
      },
      password: {
        required,
        minLength: minLength(6),
      },
      passwordConfirmation: {
        required,
        sameAs: sameAs('password'),
      },
    },
  },
  computed: {
    isFormValid() {
      return !this.$v.$invalid
    },
  },
  methods: {
    register() {
      this.$v.form.$touch()

      if (this.isFormValid) {
        this.form.userName = this.form.email
        this.$store
          .dispatch('auth/register', this.form)
          .then((redirectPath) => {
            this.success = true

            this.resetForm()
            this.$toast.success('Register successful', { duration: 3000 })
            this.showCheckEmailMessage = true
            // this.$router.push(redirectPath)
          })
          .catch((error) => {
            this.error = error
            this.$toast.error('Wrong email or password', { duration: 3000 })
          })
      }
    },
    goSignIn() {
      this.$router.push('/login')
    },
    closeMessageBox() {
      ;(this.success = null), (this.error = null)
    },
    resetForm() {
      this.form.userName = ''
      this.form.email = ''
      this.form.password = ''
      this.form.passwordConfirmation = ''
      this.$v.$reset()
    },
  },
}
</script>

<style scoped>
.hero.is-success {
  background: #f2f6fa;
}
.hero .nav,
.hero.is-success .nav {
  -webkit-box-shadow: none;
  box-shadow: none;
}
.box {
  margin-top: 5rem;
}
.avatar {
  margin-top: -70px;
  padding-bottom: 20px;
}
.avatar img {
  height: 128px;
  width: 128px;
  padding: 5px;
  background: #fff;
  border-radius: 50%;
  -webkit-box-shadow: 0 2px 3px rgba(10, 10, 10, 0.1),
    0 0 0 1px rgba(10, 10, 10, 0.1);
  box-shadow: 0 2px 3px rgba(10, 10, 10, 0.1), 0 0 0 1px rgba(10, 10, 10, 0.1);
}
p.subtitle {
  padding-top: 1rem;
}
</style>
