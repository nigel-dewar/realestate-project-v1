<template>
  <section class="section">
    <div class="container">
      <div class="content">
        <div class="columns">
          <div class="column"></div>
          <div class="column is-two-thirds">
            <CreateAnAd />
          </div>
          <div class="column"></div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import CreateAnAd from '~/components/manage/CreateAnAd'

export default {
  components: {
    CreateAnAd,
  }
}
</script>

<style lang="scss" scoped></style>
