<template>
<div class="row justify-content-around text-center">
    <h1>System info</h1>
    <div class="card col-3 p-1 m-1 row align-items-center" v-if="this.info">
        <div style="max-width: 200px" class="text-center p-1 col-4 col-sm-12">
            <gauge :key="this.info.data.value.cpuUsage" symbol="%" max="100" :percentage="this.info.data.value.cpuUsage"></gauge>
            <div class="spacer"></div>
            <div class="p-1" style="width: 200px;  background-color: green;">
                <h3>CPU Usage</h3>
                <p> CPU Cores: {{this.info.data.value.cpuCores}}</p>
            </div>
        </div>

    </div>
    <div class="card col-3 p-1 m-1 row align-items-center" v-if="this.info">
        <div style="max-width: 200px" class="text-center p-1 col-4 col-sm-12">
            <gauge :key="this.info.data.value.usedMemory" symbol="%" :percentage="(Math.round((this.info.data.value.usedMemory/this.info.data.value.totalMemory)*100))"></gauge>
            <div class="spacer"></div>
            <div class="p-1" style="width: 200px;  background-color: green;">
                <h3>Memory usage</h3>
                <p>installed Memory: {{Math.round((this.info.data.value.totalMemory/1024))}} Gib</p>
                <p>used memory: {{this.info.data.value.usedMemory}}Mb</p>
            </div>
        </div>
    </div>
    <div class="card col-3 p-1 m-1 row align-items-center" v-if="this.info">
        <div style="max-width: 200px" class="text-center p-1 col-4 col-sm-12">
            <gauge :key="this.info.data.value.usedMemory" symbol="%" :percentage="Math.abs(Math.round(this.info.data.value.diskFree/this.info.data.value.totalDisk*100)-100)"></gauge>
            <div class="spacer"></div>
            <div class="p-1" style="width: 200px;  background-color: green;">
                <h3>Disk usage</h3>
                <p>free disk space: {{Math.round((this.info.data.value.diskFree/1024))}} Gib</p>
                <p>installed disk space: {{Math.round(this.info.data.value.totalDisk/1024)}}Gib</p>
                <p>disk input: {{this.info.data.diskInput}}</p>
            </div>
        </div>
    </div>
    <div class="card col-3 p-1 m-1 row align-items-center" v-if="this.info">
        <table class="table table-dark">
            <thead>
                <tr>
                    <th scope="col">Description</th>
                    <th scope="col">Value</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th scope="row">Object Detection:</th>
                    <td>Running: {{ this.info.data.value.objectDetection }}</td>
                </tr>
                <tr>
                    <th scope="row">Text to Speech:</th>
                    <td>Running: {{ this.info.data.value.textToSpeech }}</td>
                </tr>
                <tr>
                    <th scope="row">Runtime:</th>
                    <td>{{ this.info.data.value.runtimeMinutes }}</td>
                </tr>
            </tbody>
        </table>
    </div>
    
</div>
</template>

<script>
import axios from 'axios'
import gauge from './GaugeComponent.vue';
import config from '../config.js';






export default {
    name: 'HardwareDisplayComponent',
    components: {
        gauge
    },
    /*using data() for reactive properties within the code. 
    vue will track the changes and update the DOM when data changes
    */
    data() {
        //configuring the chart..s
        return {
            chartData: {
                labels: ['cpu usage in %'],
                datasets: [{
                    //mock data
                    data: [100]
                }]
            },
            //customize the chart
            chartOptions: {
                responsive: true,
                scales: {
                    y: {
                        //min max horizontally
                        min: 0,
                        max: 100,
                    }
                }
            },
            //the object that will be filled with the data from the api
            info: null,
        }
    },
    //using mounted to get the first data from the api and set the interval
    mounted() {
        this.getData();
        setInterval(() => {

            this.getData();

        }, 2000);
    },
    //clearing the interval when the component is destroyed
    beforeUnmount() {
        clearInterval(this.interval)
    },
    //declaring methods i can use troughout the lifecycle of the component
    methods: {
        handleScroll(event) {
            event.preventDefault();
        },
        //using axios to do a http requist to the api to  retrieve the data
        getData() {
            axios
                .get(config.API_URL + '/hwstats')
                .then(response => {
                    this.info = response;
                })
                .catch(error => {
                    console.log(error);
                });
        },
    },
}
</script>

<style scoped>
.spacer {
    height: 90px;
}
</style>
