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
     /*using data() for reactive properties within the code. 
    vue will track the changes and update the DOM when data changes
    in this case i do so because i'd like the chart to have its data available on render. otherwise i could have technically used props
    */
    data () {
      // 300° is the max value 
      // 180° is the default valur
      return {
        completedSteps: 180,
        totalSteps: 300
      }
    },
  //loading in component
    components: {
      RadialProgressBar
    }
  }
  </script>