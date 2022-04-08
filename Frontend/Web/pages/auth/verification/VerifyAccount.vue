<template>
  <section class="hero">

    <div class="hero-body">
      <div class="container has-text-centered">
        <div class="column is-4 is-offset-4">
          <h3 class="title has-text-grey">Email Verification</h3>
          <div class="notification is-danger" v-if="!success">
              <div>
                Error Message
              </div>
            </div>
            <div class="notification is-success" v-if="success">
              <p>Congrats!, your account is good to go</p>
              <!-- <nuxt-link to="/login">Login</nuxt-link> -->
              <button class="button is-primary" @click="signin">Sign In</button>
            </div>

          <p class="has-text-grey">
            <!-- <nuxt-link to="/login">Proceed to Login</nuxt-link> &nbsp;·&nbsp; -->

            <a href="../">Need Help?</a>
          </p>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
  export default {
   
    async asyncData({params, query, app}){

      const q = Object.keys(query).map(k => `${k}=${query[k]}`)
        .join('&')

        try {
          const {data} = await app.$axios.$post(
            `${process.env.apiURL}/Identity/ConfirmEmail?${q}`
          )
        
          return {success: true}

        } catch(e) {
          console.log(e)
          return {success: false}
        } finally {

        }
    },
    methods: {
      signin() {
        this.$router.push('/login')
      }
    }
  }
</script>

<style lang="scss" scoped>
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