// const env = require('dotenv').config()

// console.log(env)

export default {
  /*
  ** Nuxt rendering mode
  ** See https://nuxtjs.org/api/configuration-mode
  */
  mode: 'universal',
  /*
  ** Nuxt target
  ** See https://nuxtjs.org/api/configuration-target
  */
  target: 'server',
  /*
  ** Headers of the page
  ** See https://nuxtjs.org/api/configuration-head
  */
  head: {
    title: process.env.npm_package_name || '',
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      { hid: 'description', name: 'description', content: process.env.npm_package_description || '' }
    ],
    link: [
      { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }
    ],
    script: [
      {
        src: 'https://kit.fontawesome.com/d33a83f69c.js'
      }
    ]
  },
  /*
  ** Global CSS
  */
  css: [
    '@/assets/scss/main.scss'
  ],
  /*
  ** Plugins to load before mounting the App
  ** https://nuxtjs.org/guide/plugins
  */
  plugins: [
    {src: '~/plugins/filters'},
    {src: '~/plugins/vuelidate'},
    {src: '~/plugins/axios', mode: 'client'},
    {src: '~/plugins/components'},
    {src: '~/plugins/gmaps', ssr: false},
    {src: '~/plugins/masks', ssr: false}
  ],
  /*
  ** Auto import components
  ** See https://nuxtjs.org/api/configuration-components
  */
  components: true,
  /*
  ** Nuxt.js dev-modules
  */
  buildModules: [
    '@nuxtjs/router',
    // '@nuxtjs/dotenv',
  ],
  /*
  ** Nuxt.js modules
  */
  modules: [
    // Doc: https://buefy.github.io/#/documentation
    'nuxt-buefy',
    // Doc: https://axios.nuxtjs.org/usage
    '@nuxtjs/axios',
    ['cookie-universal-nuxt', { parseJSON: false }],
    '@nuxtjs/toast',
    
  ],

  /*
  ** Axios module configuration
  ** See https://axios.nuxtjs.org/options
  */
  axios: {
    retry: { retries: 3 }
  },
  /*
  ** Build configuration
  ** See https://nuxtjs.org/api/configuration-build/
  */
  build: {
    vendor: ['lodash']
  },


  env: {
    baseURL: process.env.BASE_URL || "http://localhost:3000",
    apiURL: process.env.API_URL || "http://localhost:5000"
  },

  // publicRuntimeConfig: {
  //   baseURL: process.env.BASE_URL,
  //   apiURL: process.env.API_URL
  // },

  server: {     
    port: 3000, // default: 3000     
    host: '0.0.0.0', // default: localhost
  },
  
}
