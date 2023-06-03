<template>
    <div class="row justify-content-around text-center" v-if="info">
      <h1>Servo Strength</h1>
  
      <div class="card col-3 p-1 m-1 row align-items-center" v-for="(item, key) in info" :key="key">
        <div style="max-width: 200px" class="text-center p-1 col-4 col-sm-12">
          <gauge symbol="%" :percentage="Math.round(item/255*100)"></gauge>
          <div class="spacer"></div>
          <div class="p-1" style="width: 200px; background-color: green;">
            <h3>{{ key }}</h3>
            <p>At strength: {{ Math.round(item/255*100) }} %</p>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import gauge from './GaugeComponent.vue';
  
  export default {
    name: 'ServoSetrengthDisplayComponent',
    components: {
      gauge
    },
    /*
    using props to pass data from parent to child
    */
    props: {
      retrievedData: {
        type: Object,
        required: true
      }
    },
    //using computed to calculate the data for the chart
    //computeds are cached, meaning they only recompute when their value/dependencies change.
    computed: {
    info() {
      if (!this.retrievedData) {
        return null;
      }

      //filtering the data to only get the Load values which hare the currently applied strength of the servos
    return Object.entries(this.retrievedData).reduce((result, [key, value]) => {
      if (key.startsWith('load')) {
        
        const index = key.split('load')[1];
        result[`load ${index}`] = value;
      }
      return result;
    }, {});
  }
}
  }
  </script>
  
  <style scoped>
  .spacer {
    height: 90px;
  }
  </style>