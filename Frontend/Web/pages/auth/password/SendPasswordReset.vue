<template>
  <section class="hero is-success is-fullheight">

    <div class="hero-body">
      <div class="container has-text-centered">
        <div class="column is-4 is-offset-4">
          <h3 class="title has-text-grey">Password Reset</h3>
          <p class="subtitle has-text-grey">Enter your email address. We will send you an email to reset your password if you have an account with us.</p>
          <div class="box">
            <!-- <figure class="avatar">
              <img src="https://via.placeholder.com/300">
            </figure> -->
             <div class="notification is-success" v-if="success">
              <button @click="closeMessageBox" class="delete"></button>
              <div>
                <p>We have sent you an email to activate your account.</p>
                <!-- <nuxt-link :to="{ name: 'resend-verification'}">Not recieve the email? Resend verification email</nuxt-link> -->
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
              <!-- Login Button -->
              <button
                @click.prevent="send"
                :disabled="$v.form.$invalid"
                class="button is-block is-info is-large is-fullwidth">
                Send
              </button>
            </form>
          </div>
          <p class="has-text-grey">
            <nuxt-link to="/login">Login</nuxt-link> &nbsp;·&nbsp;
            <a href="../">Need Help?</a>
          </p>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import {required, email} from 'vuelidate/lib/validators'
export default {
  middleware: 'guest',
  data(){
    return {
      form: {
        email: 'nigel.dewar@protonmail.com'
      },
      error: null,
      success: null
    }
  },
  validations: {
    form: {
      email: {
        emailValidator: email,
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
    send() {
      this.$v.form.$touch()

      if (this.isFormValid) {

        this.$store.dispatch('auth/sendPasswordReset', this.form)
          .then(() => {
            this.$toast.success('Password Reset Sent', {duration: 3000})
            this.success = true
            this.resetForm()
            // this.$router.push('/')
          })
          .catch((error) => {
            this.error = error
            this.$toast.error(error.message, {duration: 3000})
          })
      }
    },
    closeMessageBox() {
      this.success = null,
      this.error = null
    },
    resetForm() {
      this.form.email = ''
      this.$v.$reset()
    }
  }
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
