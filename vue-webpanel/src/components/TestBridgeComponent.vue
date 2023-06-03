<template>
  <div>
    <h1>Latest EventWindow</h1>
    <json-pretty :key="retrievedData" :data="retrievedData"></json-pretty>
  </div>
</template>

<script>
import config from '../config.js'
import * as ROSLIB from 'roslib'
import JsonPretty from 'vue-json-pretty';

export default {
  name: 'TestBridgeComponent',
  components: {
    JsonPretty
  },
  data() {
    return {
      retrievedData: {} 
    };
  },
  created() {
    try {
      const ros = new ROSLIB.Ros({
        url: config.WS_URL
      });

      const listener = new ROSLIB.Topic({
        ros,
        name: '/parameter_events',
        messageType: 'rcl_interfaces/msg/ParameterEvent'
      });

      listener.subscribe(message => {
        // Check if the retrievedData has any existing data
        if (Object.keys(this.retrievedData).length === 0) {
          // If retrievedData is empty, assign the new message directly
          this.retrievedData = message;
        } else {
          // Merge the new data with the existing data
          this.retrievedData = {
            ...this.retrievedData,
            ...message
          };
        }
      });


      ros.on('connection', () => {
        console.log('Connected to Rosbridge server');
      });

      ros.on('error', error => {
        console.error('Error connecting to Rosbridge server:', error);
      });

      ros.on('close', () => {
        console.log('Connection to Rosbridge server closed');
      });

      ros.connect(); // Connect to Rosbridge server
    } catch (error) {
      console.error('An error occurred while connecting to Rosbridge server:', error);
    }
  }
};
</script>

