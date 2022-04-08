<template>
  <div class="instructor-page">
    <!-- <ManageHeader title="Choose your manage page" exitLink="/" >
        <template #actionMenu>
            <div class="full-page-takeover-header-button">
                <nuxt-link
                to="/manage/ads/create"
                class="button is-medium is-light">
                New Ad
                </nuxt-link>
                <nuxt-link
                to="/"
                class="button is-danger is-medium is-inverted is-outlined">
                Student
                </nuxt-link>
            </div>
        </template>
    </ManageHeader> -->
    <div class="centered">
      <div class="columns">
        <!-- Go to /instructor/courses -->
        <div class="box" @click="() => $router.push('/manage/ads')">
          <div>
            My Ads
          </div>
        </div>
        <!-- Go to /instructor/blogs -->
        <!-- <div class="box" @click="() => {}">
          <div>
            Blogs
          </div>
        </div> -->
        <!-- Go to /instructor/heroes -->
        <!-- <div class="box" @click="() => {}">
          <div>
            Heroes
          </div>
        </div> -->
      </div>
    </div>-
  </div>
</template>
<script>
import ManageHeader from '~/components/shared/Header'

export default {
    // middleware: 'admin',
    // layout: 'manage',
    components: {
      ManageHeader
    }
}
</script>
<style scoped lang="scss">
  .instructor-page {
    .centered {
      margin-top: 100px;
      display: flex;
      flex-direction: row;     /* make main axis horizontal (default setting) */
      justify-content: center; /* center items horizontally, in this case */
      align-items: center;     /* center items vertically, in this case */
    }
  }
  .box {
    height: 300px;
    width: 300px;
    display: flex;
    margin: 5px;
    justify-content: center;
    &:hover {
      cursor: pointer;
      background-color: #58529f;
      color: white;
      transition: background-color 0.3s ease-out;
    }
    > div {
      align-self: center;
      font-size: 50px;
      font-weight: bold;
    }
  }
</style>
