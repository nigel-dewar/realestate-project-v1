<template>
  <div class="card">
    <div class="card-content">
      <div>
        <p class="is-size-4 is-size-5-mobile">Add Pictures</p>
        <p class="is-size-6 is-size-7-mobile m-b-md">
          You can add up to 20 pictures
        </p>
        <p v-if="!mainImageExists">
          You will need to add at least 1 picture to proceed
        </p>
      </div>

      <div class="m-t-md">
        <div class="inputImage" v-if="!displaySlim">
          <button class="button is-success" @click="displaySlimControl">
            Upload or Take Photo
          </button>
        </div>

        <div class="slim-container" v-if="displaySlim">
          <slim-cropper :options="slimOptions">
            <input
              type="file"
              accept="image/*"
              id="image-picker"
              ref="slimFilePicker"
            />
          </slim-cropper>

          <div class="notification is-success" v-if="uploading">
            <i class="fas fa-spinner fa-spin"></i>
            Uploading...
          </div>
          <div class="notification is-danger" v-if="error">
            {{ error }}
          </div>
        </div>
      </div>
      <div class="card-images m-t-md" v-if="images">
        <div
          class="images-container"
          v-for="(image, index) in images"
          :key="image.id"
        >
          <img :src="image.url" alt="" class="images-container__image" />
          <div class="images-container__menu">
            <div v-if="image.isMain" class="main-image">
              <span class="m-r-xs">Main</span>
              <icon name="heart" size="small" class="has-text-danger" />
            </div>

            <button
              class="button is-small is-primary"
              @click="setMain(image, index)"
              v-if="!image.isMain"
            >
              <span class="m-r-xs">Main</span>
              <icon name="heart" size="small" />
            </button>
            <button
              class="button is-small is-danger"
              @click="deleteImage(image, index)"
              v-if="!image.isMain"
            >
              <!-- <span class="m-r-xs"></span> -->
              <icon name="trash-alt" size="small" />
            </button>
            <i
              v-if="editLoading && target == index"
              class="fas fa-spinner fa-spin"
            ></i>
            <!-- <b-checkbox class="checkbox"> Main </b-checkbox> -->
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Slim from '~/components/manage/listing/images/slim'
import Icon from '~/components/shared/Icon'
import { required } from 'vuelidate/lib/validators'
import { mapGetters } from 'vuex'

export default {
  components: {
    'slim-cropper': Slim,
    Icon,
  },
  data() {
    return {
      slim: null,
      slimData: null,
      slimOptions: {
        service: this.slimService,
        didInit: this.slimInit,
        didUpload: this.imageUploaded,
        serviceFormat: 'file',
        post: 'output',
        ratio: '16:9',
        defaultInputName: 'image',
        uploadMethod: this.uploadMyImage,
        forceType: 'jpg',
        jpgCompression: '90',
        instantEdit: true,
        push: true,
        // minSize: '800,600',
        size: '1280,720',
        // size: '400,300',
        label: 'Drop your image here or take a photo',
        maxFileSize: 20, // value is 2MB limit
      },
      uploading: false,
      editLoading: false,
      error: '',
      displaySlim: false,
      editMode: false,
      editingImage: null,
      target: null,
      form: {
        mainImage: '',
      },
    }
  },
  validations: {
    form: {
      mainImage: {
        required,
      },
    },
  },
  computed: {
    isValid() {
      return !this.$v.$invalid
    },
    listing() {
      return this.$store.getters['manage/listings/listing']

      // return props.sort(x=>x.isMain)
    },
    images() {
      return this.$store.getters['manage/images/images']
    },
    mainImageExists() {
      return this.images.filter((x) => x.isMain == true).length > 0
        ? true
        : false
    },
    mainImage() {
      return this.images.find((x) => x.isMain == true)
    },
    currentImageExists() {
      return this.slim
    }
  },
  mounted() {
    this.scrollToTop()
    this.$store.dispatch('manage/images/getImages', this.listing.propertyId)
  },
  methods: {
    uploadMyImage() {
      console.log('I got pressed')
    },
    scrollToTop() {
      window.scrollTo({
        top: 0,
        left: 0,
        // behavior: 'smooth'
      })
    },
    run() {
      this.emitFormData()
    },
    displaySlimControl() {
      this.displaySlim = !this.displaySlim
    },
    setMain(image, index) {
      this.target = index
      this.editLoading = true
      this.$store
        .dispatch('manage/images/setMain', {
          propertyId: this.listing.propertyId,
          imageId: image.id,
        })
        .then((res) => {
          // this.uploading = false
          this.$toast.success('Image set as main', {
            duration: 3000,
            position: 'bottom-center',
          })
        })
        .catch((e) => {
          this.$toast.error('Could not set image as main', {
            duration: 3000,
            position: 'bottom-center',
          })
        })
        .finally((_) => (this.editLoading = false))
    },
    deleteImage(image, index) {
      this.target = index
      this.editLoading = true
      this.$store
        .dispatch('manage/images/deleteImage', {
          propertyId: this.listing.propertyId,
          imageId: image.id,
        })
        .then((res) => {
          // this.uploading = false
          this.$toast.success('Image deleted', {
            duration: 3000,
            position: 'bottom-center',
          })
        })
        .catch((e) => {
          this.$toast.error('Could not delete image', {
            duration: 3000,
            position: 'bottom-center',
          })
        })
        .finally((_) => (this.editLoading = false))
    },
    slimService(formdata, progress, success, failure, slim) {
      this.uploading = true

      const formData = new FormData()
      formData.append('upload_preset', 'wyfbdzud')
      // formData.append('file', slim.data.input.file)
      formData.append('file', formdata[0])

      this.$store
        .dispatch('manage/images/addImage', {
          propertyId: this.listing.propertyId,
          data: formData,
        })
        .then((res) => {
          // this.uploading = false
          this.$toast.success('Image upload success', {
            duration: 3000,
            position: 'bottom-center',
          })
        })
        .catch((e) => {
          this.$toast.error('Image upload error', {
            duration: 3000,
            position: 'bottom-center',
          })
        })
        .finally((_) => {
          this.uploading = false
          this.displaySlim = false
        })
    },
    slimInit(data, slim) {
      // slim instance reference
      this.slim = slim
      this.slimData = data
    },
    imageUploaded(error, data, response) {
      this.$refs.slimFilePicker.value = null
    },
    emitFormData() {
      this.$v.$touch()
      this.$emit('stepUpdated', { data: this.form, isValid: this.isValid })
    },
  },
  watch: {
    mainImageExists: function (val) {
      this.form.mainImage = this.mainImage
      this.emitFormData()

      // this.$v.form.mainImage.$touch()
      // if (val.length > 0) {
      //     this.form.mainImage = val[0]
      // }
      // this.$v.$touch()
      // this.emitFormData()
    },
    currentImageExists: function(val) {
      console.log(val)
    }
  },
}
</script>

<style lang="scss" scoped>
.slim-container {
  // height: 300px;
  // min-width: 100%;
  max-width: 80vh;
  margin: auto auto;
  border: 1px solid lightgray;
  padding: 5px;
}

.main-image {
  display: inline-flex;
}

.image-editor {
  &__image {
    width: 400px;
    height: 100%;
  }
}

.images {
  // padding: 10px;
  // display: inline-flex;
  flex-wrap: wrap;
  margin: 2rem;
  // border: 1px solid lightgray;

  &-container {
    display: inline-flex;
    width: 200px;
    height: 150px;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    margin: 0.1rem 0.1rem;
    border: 1px solid lightgray;

    &__image {
      border: 1px solid gray;
    }

    &__menu {
      padding: 3px;
    }
  }

  &-container:hover {
    // background-color: lightgreen;
  }
}
</style>
