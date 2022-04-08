import Vue from 'vue'
import Router from 'vue-router'
Vue.use(Router)

const page = path => () => import(`~/pages/${path}`).then(m => m.default || m)

const routes = [
    { path: '/', name: 'index', component: page('index.vue')},
    { path: '/login', name: 'login', component: page('auth/Login.vue')},
    { path: '/register', name: 'register', component: page('auth/Register.vue')},
    { path: '/secret', name: 'secret', component: page('secret.vue')},
    { path: '/not-found', name: 'not-found', component: page('NotFound.vue')},
    { path: '/not-authenticated', name: 'not-authenticated', component: page('auth/notAuthenticated.vue')},
    { path: '/verification/verify-account', name: 'verify-account', component: page('auth/verification/VerifyAccount.vue')},
    { path: '/verification/resend-verification', name: 'resend-verification', component: page('auth/verification/ResendVerificationEmail.vue')},
    { path: '/password/reset', name: 'password.reset', component: page('auth/password/PasswordReset.vue')},
    { path: '/password/send-reset', name: 'send.password.reset', component: page('auth/password/SendPasswordReset.vue')},
    { path: '/user/profile', name: 'user.profile', component: page('user/profile.vue')},
    { path: '/user/settings/dashboard', name: 'user.dashboard', component: page('user/settomgs/dashboard.vue')},
    { path: '/user/account-details', name: 'user.account.details', component: page('user/account-details.vue')},
    { path: '/about', name: 'about', component: page('about.vue')},
    { path: '/cv', name: 'cv', component: page('cv.vue')},
    { path: '/create-ad-start', name: 'create.ad.landing', component: page('create-ad-landing.vue')},
    { path: '/manage/ads/create', name: 'manage.create.ad', component: page('manage/ads/create.vue')},
    { path: '/manage/ads/created', name: 'manage.created.ad', component: page('manage/ads/congrats.vue')},
    { path: '/manage/ads', name: 'manage.ads', component: page('manage/ads/index.vue')},
    { path: '/manage/ads/:id/manage', name: 'manage.ads.id', component: page('manage/ads/id/manage.vue')},
    { path: '/manage', name: 'manage', component: page('manage/index.vue')},
    { path: '/search', name: 'search', component: page('search/index.vue')},
    { path: '/search/results', name: 'results', component: page('search/results.vue')},
    { path: '/details/:id', name: 'details', component: page('search/details.vue')},
]

export function createRouter() {
    return new Router({
        routes,
        mode: 'history'
    })
}