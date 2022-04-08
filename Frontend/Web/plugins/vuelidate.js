import Vue from 'vue'
import Vuelidate from 'vuelidate'
import vuelidateErrorExtractor from 'vuelidate-error-extractor'

const messages = {
    required: "Field {attribute} is required",
    email: "Field {attribute} is not a proper email address"
  };

Vue.use(Vuelidate)
Vue.use(vuelidateErrorExtractor, {
    messages
  })

  import { templates } from 'vuelidate-error-extractor'


// This will register the component globally
Vue.component('FormWrapper', templates.FormWrapper)
