<template>
  <b-notification
    v-if="hasErrors"
    type="is-danger"
  >
    <div v-for="(error, index) in activeErrorMessages" :key="index">
      <div>{{ error }}</div>
    </div>  
  </b-notification>
</template>
<script>
import { multiErrorExtractorMixin } from "vuelidate-error-extractor";
export default {
  name: "FormSummary",
  extends: multiErrorExtractorMixin
};
</script>
