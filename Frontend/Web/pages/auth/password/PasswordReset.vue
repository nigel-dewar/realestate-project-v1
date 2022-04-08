<template>
  <section class="hero is-success is-fullheight">
    <div class="hero-body">
      <div class="container has-text-centered">
        <div class="column is-4 is-offset-4">
          <h3 class="title has-text-grey">Password Reset</h3>
          <p class="subtitle has-text-grey">Please enter your email, and new password.</p>
          <div class="box">
            <!-- <figure class="avatar">
              <img src="https://via.placeholder.com/300">
            </figure> -->
            <div class="notification is-success" v-if="success">
              <button @click="closeMessageBox" class="delete"></button>
              <div>
                <p>Woohoo! You're now in the system. We have sent you an email to activate your account.</p>
                <!-- <nuxt-link :to="{ name: 'resend-verification'}">Not recieve the email? Resend verification email</nuxt-link> -->
              </div>
            </div>
            <div class="notification is-danger" v-if="error">
              <button @click="closeMessageBox" class="delete"></button>
              {{error.message}}
            </div>
            <form>

              <div class="field">
                <div class="control">
                  <input
                    v-model="form.password"
                    @blur="$v.form.password.$touch()"
                    class="input is-medium"
                    type="password"
                    placeholder="Your Password"
                    autocomplete="new-password">
                  <div v-if="$v.form.password.$error" class="form-error">
                    <span v-if="!$v.form.password.required" class="help is-danger">Password is required</span>
                    <span v-if="!$v.form.password.minLength" class="help is-danger">Password minimum length is 6 letters</span>
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
                    autocomplete="off">
                  <div v-if="$v.form.passwordConfirmation.$error" class="form-error">
                    <span v-if="!$v.form.passwordConfirmation.required" class="help is-danger">Password is required</span>
                    <span v-if="!$v.form.passwordConfirmation.sameAs" class="help is-danger">Password confirmation should be the same as password</span>
                  </div>
                </div>
              </div>
              <button 
                @click.prevent="reset"
                :disabled="$v.form.$invalid"
                type="submit"
                class="button is-block is-info is-large is-fullwidth">Reset Password</button>
            </form>
          </div>
          <p class="has-text-grey">
            <nuxt-link to="/login">Login</nuxt-link> 
          </p>
          <p class="has-text-grey">
            <a href="../">Need Help?</a>
          </p>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import {required, email, minLength, url, sameAs} from 'vuelidate/lib/validators'
export default {
  middleware: 'guest',
  data(){
    return {
      form: {
        password: null,
        passwordConfirmation: null,
        token: '',
        userId: ''
      },
      success: null,
      error: null
    }
  },
  mounted(){
    this.form.userId = this.$route.query.userId
    this.form.token = this.$route.query.token
    this.form.password = 'Austina40!!!'
    this.form.passwordConfirmation = 'Austina40!!!'
  },
  validations: {
    form: {
      token: {
        required
      },
      userId: {
        required
      },
      password: {
        required,
        minLength: minLength(6)
      },
      passwordConfirmation: {
        required,
        sameAs: sameAs('password')
      }
    }
  },
  computed: {
    isFormValid(){
      return !this.$v.$invalid
    }
  },
  methods: {
    reset(){
      this.$v.form.$touch()
      if (this.isFormValid) {
        this.$store.dispatch('auth/resetPassword', this.form)
          .then(() => {
            this.success = true
            // 
            this.resetForm()
            this.$toast.success('Password reset successful', {duration: 3000})
            this.$router.push('/login')
          })
          .catch((error)=> {
            this.error = error
            this.$toast.error('Wrong email or password', {duration: 3000})
          })
      }
    },
    closeMessageBox() {
      this.success = null,
      this.error = null
    },
    resetForm() {
      this.form.token = ''
      this.form.userId = ''
      this.form.password = ''
      this.form.passwordConfirmation = ''
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
