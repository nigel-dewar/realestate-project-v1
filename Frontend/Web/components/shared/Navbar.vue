<template>
  <nav class="navbar is-active" role="navigation" aria-label="main navigation">
    <div class="navbar-brand">
      <nuxt-link class="navbar-item" to="/">
        <h1 class="brand-title">Realestateify</h1>
      </nuxt-link>
      <!-- Adds click to open -->
      <!-- Adds active class -->
      <a @click="isActive = !isActive"
          :class="{'is-active': isActive}"
         role="button"
         class="navbar-burger burger"
         aria-label="menu"
         aria-expanded="false"
         data-target="navbarBasicExample">
        <span aria-hidden="true"></span>
        <span aria-hidden="true"></span>
        <span aria-hidden="true"></span>
      </a>
    </div>

    <!-- Adds active class -->
    <div :class="{'is-active': isActive}"
        id="navbarBasicExample"
        class="navbar-menu">
      <div class="navbar-start">

        
        <nav-link to="/" @click.native="closeMenu" class="navbar-item">
          Home
        </nav-link>
        <!-- <nav-link to="/secret" class="navbar-item">
          secret
        </nav-link> -->
        <nav-link to="/about" @click.native="closeMenu" class="navbar-item">
          About
        </nav-link>


        <nav-link to="" @click.native="createAd" class="navbar-item">
          Create an Ad
        </nav-link>
        <!-- <nuxt-link to="/instructor" class="navbar-item">
          Instructor
        </nuxt-link>
        <nuxt-link to="/secret" class="navbar-item">
          Secret
        </nuxt-link> -->
      </div>

      <div class="navbar-end">
        <div class="navbar-item">
          <div class="buttons">
            <!-- If Authenticated -->
            <template v-if="isAuth">
              <!-- <figure class="image avatar is-48x48 m-r-sm">
                <img class="is-rounded" src="https://upload.wikimedia.org/wikipedia/commons/6/67/User_Avatar.png">
              </figure> -->
              <div class="m-r-sm m-b-sm is-hidden-mobile" v-if="profile.displayName">
                Welcome {{profile.displayName}}
              </div>
              <button
                 v-if="isAuth" class="button is-link is-outlined"
                 @click="() => {$router.push('/manage/ads'); this.closeMenu();}">
                Manage
              </button>
              <a class="button is-primary" @click="() => {this.logout(); this.closeMenu(); }">
                Logout
              </a>
            </template>
            <template v-else>
              <nuxt-link to="/register" @click.native="closeMenu" class="button is-primary">
                Sign up
              </nuxt-link>
              <nuxt-link to="/login" @click.native="closeMenu" class="button is-light">
                Log in
              </nuxt-link>
            </template>
          </div>
        </div>
      </div>
    </div>
  </nav>
</template>

<script>
import {mapGetters} from 'vuex'

export default {
  data() {
    return {
      isActive: false
    }
  },
  computed: {
    ...mapGetters({
      'user': 'auth/authUser',
      'isAuth': 'auth/isAuthenticated',
      'isAdmin': 'auth/isAdmin',
      'profile': 'auth/profile'
    })
  },
  methods: {
    logout() {
      this.$store.dispatch('auth/logout').then(() => this.$router.push('/login'))
    },
    closeMenu() {
      this.isActive = !this.isActive
    },
    createAd() {
      if (this.profile) {
        this.$router.push('/manage/ads/create')
      } else {
        this.$router.push('/create-ad-start')
      }
      this.closeMenu()
    }
  }
}
</script>


<style lang="scss" scoped>
  .brand-title {
    font-size: 35px;
    font-weight: bold;

    @media screen and (max-width: 800px) {
      font-size: 25px;
    }

    @media screen and (max-width: 600px) {
      font-size: 20px;
    }
  }
  .navbar-brand {
    padding-right: 30px;

    @media screen and (max-width: 1023px) {
      padding-right: 0px;
    }
  }
  .avatar {
    margin-right: 5px;
  }
  .navbar-item:hover {
    color: #4bacff;
    font-weight: 500;
  }
</style>
