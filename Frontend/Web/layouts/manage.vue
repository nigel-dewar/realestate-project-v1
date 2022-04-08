<template>
  <div>
    <navbar />
    <nuxt />
  </div>

</template>

<script>
import Navbar from '~/components/shared/Navbar'
export default {
  name: 'Manage',
  head: {
    title: 'Realestateify',
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      {
        hid: 'RealEstateify',
        name: 'RealEstateify',
        content: 'RealEstateify'
      }
    ],
    link: [{ rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }]
  },
  components: {
    Navbar
  },
  middleware: ['auth']
}
</script>

<style lang="scss">
html {
  font-family: 'Source Sans Pro', -apple-system, BlinkMacSystemFont, 'Segoe UI',
    Roboto, 'Helvetica Neue', Arial, sans-serif;
  color: #505763;;
  background-color: #f2f3f5;
  font-size: 16px;
  word-spacing: 1px;
  -ms-text-size-adjust: 100%;
  -webkit-text-size-adjust: 100%;
  -moz-osx-font-smoothing: grayscale;
  -webkit-font-smoothing: antialiased;
  box-sizing: border-box;
}
*,
*:before,
*:after {
  box-sizing: border-box;
  margin: 0;
}
.card.manage-card {
  .label {
    color: #353535;
  }
  .card-header-title {
    color: #616161;
  }
}
.full-page-takeover-header {
  background-color: #58529f;
  color: white;
  &-text {
    color: white;
    font-weight: bold;
    font-size: 22px;
  }
}
.button--green {
  display: inline-block;
  border-radius: 4px;
  border: 1px solid #3b8070;
  color: #3b8070;
  text-decoration: none;
  padding: 10px 30px;
}
.button--green:hover {
  color: #fff;
  background-color: #3b8070;
}
.button--grey {
  display: inline-block;
  border-radius: 4px;
  border: 1px solid #35495e;
  color: #35495e;
  text-decoration: none;
  padding: 10px 30px;
  margin-left: 15px;
}
.button--grey:hover {
  color: #fff;
  background-color: #35495e;
}
</style>
