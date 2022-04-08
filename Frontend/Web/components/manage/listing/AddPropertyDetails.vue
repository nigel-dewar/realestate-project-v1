<template>
  <div class="card">

    <div class="card-content">
      <h2 class="is-size-4 is-size-5-mobile m-b-md">Tell us all about this property</h2>
      <form @submit.prevent="">

        <div class="editor-area">
          <client-only>
          <div class="blog-editor-container">
            
            <Editor
              @editorUpdated="updateDescription"
              class="editor"
              :description="this.form.description"
            />
          </div>
        </client-only>
        </div>

        <div class="block">
          <b-checkbox
            v-for="(item, index) in this.features"
            :key="index"
            :id="item.name"
            v-model="item.checked"
            class="space"
          >
            {{ item.name }}
          </b-checkbox>
        </div>

        
      </form>
    </div>
  </div>
</template>

<script>
import Editor from '~/components/editor'
import { required } from 'vuelidate/lib/validators'
export default {
  props: {
    form: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      features: [],
      featureGroups: [],
      // form: {
      //   property: {
      //     features: [],
      //     description: null
      //   }
      // },
    }
  },
  components: {
    Editor,
  },
  validations: {
    form: {
      description: {
        required,
      },
    },
  },
  computed: {
    isValid() {
      return !this.$v.$invalid
    },
    lookups() {
      return this.$store.getters['manage/listings/lookups']
    },
    checkedFeatures() {
      return this.features.filter((x) => x.checked == true)
    },
    listing() {
      return this.$store.getters['manage/listings/listing']
    },
  },
  mounted() {
    // ready up the checkboxes for use

    this.lookups.features.map((x) => {
      let found = false
      if (this.listing.features.length > 0) {
        found = this.listing.features.find((f) => f.id == x.id) ? true : false
      }
      // let check = this.listing.features.find(f => f.id == x.id)
      var checkbox = {
        id: x.id,
        name: x.name,
        checked: found,
      }
      this.features.push(checkbox)
    })
  },
  methods: {
    run() {
      this.emitFormData()
    },
    checked(e) {},
    emitFormData() {
      this.$emit('stepUpdated', { data: this.form, isValid: this.isValid })
    },
    updateDescription(html) {
      if (html !== '<p></p>') {
        this.form.description = html
      } else {
        this.form.description = ''
      }

      this.emitFormData()
    },
  },
  watch: {
    checkedFeatures: function (val) {
      this.form.features = this.features.filter((x) => x.checked == true)
      this.emitFormData()
    },
  },
}
</script>

<style lang="scss" scoped>

.editor-area {
  margin-bottom: 15px;
}

.editor {
  // border: 1px solid grey;
  // background-color: #dddddd;
  border-radius: 2px;
  padding: 0.3rem 0.3rem;
}


.space {
  max-width: 17rem;
  min-width: 17rem;
}
</style>
