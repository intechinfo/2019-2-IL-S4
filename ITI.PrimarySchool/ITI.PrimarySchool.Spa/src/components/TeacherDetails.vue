<template>
  <div class="teacher-details">
    <h1>Teacher</h1>
    <p v-if="teacher !== null">
      Teacher id: {{ teacher.teacherId }}<br />
      First name: {{ teacher.firstName }}<br />
      Last name: {{ teacher.lastName }}<br />
      Class: {{ teacher.className }}<br />
      Level: {{ teacher.level }}<br />
    </p>
  </div>
</template>

<script>
import { getById } from '../services/TeacherService'

export default {
  data() {
    return {
      teacher: null
    }
  },

  async created() {
    this.teacher = await getById(this.$route.params.teacherId);
  }
}
</script>