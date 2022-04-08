<template>
  <!-- <div class="field">
      <div class="control">
          <GoogleMapsAutocomplete />
      </div>
  </div> -->
  <client-only>
    <div class="blog-editor-container">
      <Editor />
    </div>
  </client-only>
</template>

<script>
// import GoogleMapsAutocomplete from '~/components/google-maps/GoogleMapAutocomplete'
import Editor from '~/components/editor'
export default {
  // middleware: ['check-auth', 'auth'],
  components: {
    Editor,
  },
}
</script>
