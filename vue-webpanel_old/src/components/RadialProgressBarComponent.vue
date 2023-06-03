<template>
    <radial-progress-bar :diameter="200"
                         :completed-steps="completedSteps"
                         :total-steps="totalSteps"
                         :animateSpeed="300">
     <p>Max: {{ totalSteps }}°</p>
    </radial-progress-bar>
  </template>
  
  <script>
  import RadialProgressBar from 'vue-radial-progress'
  
  export default {
    data () {
      return {
        completedSteps: 180,
        totalSteps: 359
      }
    },
  
    components: {
      RadialProgressBar
    }
  }
  </script>