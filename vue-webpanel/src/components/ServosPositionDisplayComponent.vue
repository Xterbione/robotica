<template>
    <div class="row m-0 p-0 justify-content-around text-center" v-if="info">
      <div v-for="(item, index) in info" :key="index" class="card col-4 p-1 text-center row align-items-center">
        <RadialProgressBarComponent :completedSteps="item/1025*300" />
        <p>{{ `cur_pos ${index + 1}` }}</p>
        <!--converting the numeric value to degree-->
        <p>{{ Math.round(item/1025*300) }}°</p>
      </div>
    </div>
  </template>
  
  <script>
  import RadialProgressBarComponent from './RadialProgressBarComponent.vue';
  
  export default {
    name: 'ServoSetrengthDisplayComponent',
    components: {
      RadialProgressBarComponent
    },
    //using props to pass data from parent to child 
    props: {
      retrievedData: {
        type: Object,
        required: true
      }
    },
    /* using computed to calculate the data for the chart
    we use computed so that the chart is updated when the data changes
    */
    computed: {
      info() {
        if (!this.retrievedData) {
          return null;
        }
        //filtering the data to only get the servo positions
        return Object.keys(this.retrievedData)
          .filter(key => key.startsWith('cur_pos'))
          .map(key => this.retrievedData[key]);
      }
    }
  }
  </script>
  
  <style scoped>
  .spacer {
    height: 90px;
  }
  </style>
  