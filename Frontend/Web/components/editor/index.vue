<template>
  <div class="editor">
    <basic-menu :editor="editor" />
    <!-- <bubble-menu :editor="editor" /> -->
    <editor-content class="editor__content" :editor="editor" />
  </div>
</template>

<script>
import { Editor, EditorContent } from 'tiptap'
import { 
  Heading,
  Bold,
  Code,
  Italic,
  Strike,
  Underline,
  History,
  Blockquote,
  HorizontalRule,
  OrderedList,
  BulletList,
  ListItem,
  CodeBlockHighlight,
  Placeholder
 } from 'tiptap-extensions'
import BubbleMenu from '~/components/editor/BubbleMenu'
import BasicMenu from '~/components/editor/BasicMenu'

export default {
  props: ['description'],
  components: {
    EditorContent,
    BubbleMenu,
    BasicMenu
  },
  data() {
    return {
      editor: null,
    }
  },
  mounted() {

    this.editor = new Editor({
      extensions: [
        new Heading({ levels: [1, 2, 3] }),
        new Bold(),
        new Placeholder({
            emptyEditorClass: 'is-editor-empty',
            emptyNodeClass: 'is-empty',
            emptyNodeText: 'Write something...',
            showOnlyWhenEditable: true,
            showOnlyCurrent: true,
          }),
        new Code(),
        new Italic(),
        new Strike(),
        new Underline(),
        new History(),
        new Blockquote(),
        new HorizontalRule(),
        new OrderedList(),
        new BulletList(),
        new ListItem(),
        new CodeBlockHighlight()
      ],
      content: this.description,
      autoFocus: 'end'
    })
  },
  computed: {
    html() {
      return this.editor && this.editor.getHTML()
    }
  },
  methods: {
    emitUpdate() {
      const html = this.editor.getHTML()
    },
  },
  watch: {
    html: function(val) {
      this.$emit('editorUpdated', this.html)
    }
  },
  beforeDestroy() {
    this.editor.destroy()
  },
}
</script>

// <style lang="scss">
// .editor p.is-editor-empty:first-child::before {
//   content: attr(data-empty-text);
//   float: left;
//   color: #aaa;
//   pointer-events: none;
//   height: 0;
//   font-style: italic;
// }
// </style>
