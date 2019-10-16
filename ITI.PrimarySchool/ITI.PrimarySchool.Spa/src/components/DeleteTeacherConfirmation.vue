<template>
  <div class="delete-teacher-confirmation">
    <p>Delete {{ firstName }} {{ lastName }}?</p>
    <button @click="onOkClicked">Ok</button><button @click="onCancelClicked">Cancel</button>
  </div>
</template>

<script>
export default {
  data() {
    return {
      firstName: 'firstName',
      lastName: 'lastName'
    }
  },

  methods: {
    onOkClicked() {
      console.log('onOkClicked()');
    },

    onCancelClicked() {
      console.log('onCancelClicked()');
    }
  }
}
</script>