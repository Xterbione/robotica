<template>
<div class="container">
    <div id="app" class="m-0 p-0 row">
        <div class="card p-2 col-12 p-1 mt-1 mb-1">
            <div class="row text-center">
                <img style=" height: 60px; width: 80px; display:inline;" src="https://www.freeiconspng.com/thumbs/robot-icons/robot-icon-12.png">
                <h1 class="col-5" style="display: inline;">Telemetry TerraMedic 0.1</h1>
            </div>
        </div>
        <img class="p-0 m-0" style="max-height: 200px; object-fit:cover;" src="https://i.pinimg.com/originals/47/1a/f4/471af46519ed5bea2b0002e8c034b51b.jpg" />
        
        <div class="card col-12 mt-1 w-100">
            <HardwareDisplayComponent  />
        </div>
        <div class="card text-center col-12 mt-1 w-100">
            <RecognitionSensorDisplay/>
        </div>

        <div class="card text-center col-12 mt-1 w-100">
            <h3>battery voltage levels</h3>
        </div>
        <div style="max-height: 200px;" class="col-12 card p-1 mt-1">
            <LineChartComponent />
        </div>
        <div class="card text-center col-12 mt-1 w-100">
          <h3>Servos voltage levels</h3>
      </div>
      <div style="max-height: 200px;" class="col-12 card p-1 mt-1">
          <LineChartServosComponentVue :retrievedData="retrievedData"/>
      </div>
        <div class="col-12 p-1 card mt-1 mb-1">
            <ServosVoltageComponent :retrievedData="retrievedData"/>
        </div>
        <div class="card col-12 mt-1 p-1">
            <ServoStrengthDisplayComponent :retrievedData="retrievedData"/>
        </div>
        <div class="card text-center col-12 mt-1 w-100">
            <h3>Servos positioning</h3>
        </div>
        <ServosPositionDisplayComponentVue :retrievedData="retrievedData"/>
        <div class="card col-12 mt-1 p-1">
            <ServiceInfoComponent />
        </div>
        <div class="card col-12 mt-1 p-1"> 
            <ThreeJSComponent v-if="false" :retrievedData="retrievedData"/>
        </div>
        <div class="card col-12 mt-1 w-100">
          <TestBridgeComponent />
      </div>
    </div>

</div>
</template>

<script>
import LineChartComponent from './components/LineChartComponent.vue';
import config from '/src/config.js';
import * as ROSLIB from 'roslib';
import ThreeJSComponent from './components/ThreeJSComponent.vue';
import RecognitionSensorDisplay from './components/RecognitionSensorDisplay.vue';
import HardwareDisplayComponent from './components/HardwareDisplayComponent.vue';
import LineChartServosComponentVue from './components/LineChartServosComponent.vue';
import ServiceInfoComponent from './components/ServiceInfoComponent.vue';
import ServoStrengthDisplayComponent from './components/ServoStrengthDisplayComponent.vue';
import ServosVoltageComponent from './components/ServosVoltageComponent.vue';
import ServosPositionDisplayComponentVue from './components/ServosPositionDisplayComponent.vue';
import TestBridgeComponent from './components/TestBridgeComponent.vue';

export default {
    name: 'App',
    components: {
        LineChartServosComponentVue,
        HardwareDisplayComponent,
        ServiceInfoComponent,
        ServoStrengthDisplayComponent,
        ServosVoltageComponent,
        ServosPositionDisplayComponentVue,
        RecognitionSensorDisplay,
        ThreeJSComponent,
        LineChartComponent,
        TestBridgeComponent,
    },
    data() {
    return {
      retrievedData: {},
    };
  },
  created() {
    try {
      const ros = new ROSLIB.Ros({
        url: config.WS_URL,
      });

      const listener = new ROSLIB.Topic({
        ros,
        name: '/Telemetric',
        messageType: 'topics_services/msg/Telemetric',
      });

      listener.subscribe((message) => {
        this.retrievedData = message;
      });

      ros.on('connection', () => {
        console.log('Connected to Rosbridge server');
      });

      ros.on('error', (error) => {
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
}
</script>

<style>
body {
    background-color: #343a40 !important;
    color: white !important;
}

.card {
    background-color: #3e3f40 !important;
    color: white !important;
}
</style>
