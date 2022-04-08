<template>
  <editor-menu-bar :editor="editor" v-slot="{ commands, isActive }">
    <div class="menubar">
      <button
        class="menubar__button"
        :class="{ 'is-active': isActive.bold() }"
        @click="commands.bold()"
      >
        <icon name="bold" size="small" />
      </button>
      <button
        class="menubar__button"
        :class="{ 'is-active': isActive.italic() }"
        @click="commands.italic()"
      >
        <icon name="italic" size="small" />
      </button>
      <button
        class="menubar__button"
        :class="{ 'is-active': isActive.code() }"
        @click="commands.code()"
      >
        <icon name="code" size="small" />
      </button>
      <button
        class="menubar__button"
        :class="{ 'is-active': isActive.strike() }"
        @click="commands.strike()"
      >
        <icon name="strikethrough" size="small" />
      </button>
      <button
        class="menubar__button"
        :class="{ 'is-active': isActive.underline() }"
        @click="commands.underline()"
      >
        <icon name="underline" size="small" />
      </button>
      <button
        class="menubar__button"
        :class="{ 'is-active': isActive.heading({ level: 1 }) }"
        @click="commands.heading({ level: 1 })"
      >
        H1
      </button>

      <button
        class="menubar__button"
        :class="{ 'is-active': isActive.heading({ level: 2 }) }"
        @click="commands.heading({ level: 2 })"
      >
        H2
      </button>

      <button
        class="menubar__button"
        :class="{ 'is-active': isActive.heading({ level: 3 }) }"
        @click="commands.heading({ level: 3 })"
      >
        H3
      </button>
      <button
        class="menubar__button"
        :class="{ 'is-active': isActive.paragraph() }"
        @click="commands.paragraph()"
      >
        <icon name="paragraph" size="small" />
      </button>
    </div>
  </editor-menu-bar>
</template>
<script>
import { EditorMenuBar } from 'tiptap'
import Icon from '~/components/shared/Icon'
export default {
  components: { EditorMenuBar, Icon },
  props: {
    editor: Object,
  },
}
</script>
<style scoped lang="scss">
.button-save {
  float: right;
  background-color: #23d160;
  &:hover {
    background-color: #2bc76c;
  }
  &:disabled {
    cursor: not-allowed;
  }
}
</style>
