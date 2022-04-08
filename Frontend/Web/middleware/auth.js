// export default function({store, redirect}) {
//   const isAuth = store.getters['auth/isAuthenticated']

//   if (!isAuth) {
//       return redirect('/notAuthenticated')
//   }
// }

export default function(context) {
  const isAuth = context.store.getters['auth/isAuthenticated']

  // const hasProfile = context.store.getters['auth/profileComplete']
  context.$cookies.set('x-last-route', context.route.path)
  
  if (!isAuth) {
    
      return context.redirect('/login')
  }

  // if (!hasProfile) {
  //   return context.redirect('/user/profile')
  // }
}