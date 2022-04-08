<template>
      <div class="container has-text-centered">
        <div class="column is-4 is-offset-4">
          <h3 class="is-size-4 has-text-grey">Login</h3>
          <nuxt-link to="/register">Or Sign Up</nuxt-link>
          <div class="card m-t-md m-b-md">
            <div class="card-content">
              <div class="notification is-danger" v-if="error">
              <button @click="closeErrorBox" class="delete"></button>
              <div v-if="error.code == 555">
                <p>{{error.message}}</p>
              </div>

              <div v-if="error.code == 556">
                <p>{{error.message}}</p>
                <nuxt-link :to="{name: 'resend-verification'}">Resend verification email</nuxt-link>
              </div>
              <div v-else>
                {{error.message}}
              </div>
            </div>
            <form>
              <div class="field">
                <div class="control">
                  <input 
                    v-model="form.email"
                    @blur="$v.form.email.$touch()"
                    class="input is-medium"
                    type="email"
                    placeholder="Your Email"
                    autofocus=""
                    autocomplete="email">
                  <div v-if="$v.form.email.$error" class="form-error">
                    <span v-if="!$v.form.email.required" class="help is-danger">Email is required</span>
                    <span v-if="!$v.form.email.emailValidator" class="help is-danger">Email address is not valid</span>
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
                    autocomplete="current-password">
                  <div v-if="$v.form.password.$error" class="form-error">
                    <span v-if="!$v.form.password.required" class="help is-danger">Password is required</span>
                  </div>
                </div>
              </div>
              <!-- Login Button -->
              <button
                @click.prevent="login"
                :disabled="$v.form.$invalid"
                class="button is-block is-info is-large is-fullwidth">
                Login
              </button>
            </form>
            </div>
          </div>
          <p class="has-text-grey">
            <nuxt-link :to="{name: 'send.password.reset'}">Forgot Password?</nuxt-link> &nbsp;·&nbsp;
            <nuxt-link to="/register">Sign Up</nuxt-link> &nbsp;·&nbsp;

            <a href="../">Need Help?</a>
          </p>
        </div>
      </div>
</template>

<script>
import {required, email} from 'vuelidate/lib/validators'
export default {
  middleware: 'guest',
  data(){
    return {
      testing: false,
      form: {
        email: '',
        password: ''
      },
      error: null
    }
  },
  validations: {
    form: {
      email: {
        emailValidator: email,
        required
      },
      password: {
        required
      }
    }
  },
  computed: {
    isFormValid(){
      return !this.$v.$invalid
    }
  },
  methods: {
    login() {
      this.$v.form.$touch()

      if (this.isFormValid) {
        this.$store.dispatch('auth/login', this.form)
          .then((redirectPath) => {
            // this.$router.push(redirectPath)
            this.$router.push(redirectPath)
          })
          .catch((error) => {
            this.error = error
            this.$toast.error(error.message, {duration: 3000})
          })
      }
    },
    closeErrorBox() {
      this.error = null
    }
  },
}
</script>

<style scoped>
  .hero.is-success {
    background: #F2F6FA;
  }
  .hero .nav, .hero.is-success .nav {
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
    -webkit-box-shadow: 0 2px 3px rgba(10,10,10,.1), 0 0 0 1px rgba(10,10,10,.1);
    box-shadow: 0 2px 3px rgba(10,10,10,.1), 0 0 0 1px rgba(10,10,10,.1);
  }
  p.subtitle {
    padding-top: 1rem;
  }
</style>
