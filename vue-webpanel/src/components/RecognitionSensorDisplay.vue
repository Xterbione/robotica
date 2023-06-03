<template>
    <div>
        <h3>Object recognition and Sensor information</h3>
        <div class="align-items-center">
            <!-- <img v-if="this.streamlink != null" style="height: 300px; width: 400px;" :key="this.streamlink" :src="`${this.streamlink}?${Date.now()}`" />
       -->
        </div>
        <table class="table table-dark">
            <tr>
                <th scope="col">QR reader Value</th>
                <td>{{ this.retrievedData.gs1 }}</td>
            </tr>
            <tr>
                <th scope="row">weight:</th>
                <td>{{ this.retrievedData.weight }} in grams</td>
            </tr>
            <tr>
                <th scope="row">amplitude lowband:</th>
                <td>{{ this.retrievedData.lowband }}</td>
            </tr>
            <tr>
                <th scope="row">amplitude midband:</th>
                <td>{{ this.retrievedData.midband }}</td>
            </tr>
            <tr>
                <th scope="row">amplitude highband:</th>
                <td>{{ this.retrievedData.highband }}</td>
            </tr>
            <tr>
                <th scope="row">battery percentage:</th>
                <td>{{ Math.round(((this.retrievedData.batteryvoltage-90)/30)*100,2) }}%</td>
            </tr>
            <tr>
                <th scope="row">battery voltage:</th>
                <td> {{(this.retrievedData.batteryvoltage/10) }} Volt</td>
            </tr>
            
        </table>
    </div>
</template>

<script>
import * as ROSLIB from 'roslib'
import config from '/src/config.js';

export default {
    name: 'RecognitionSensorDisplay',
    components: {},
         /*using data() for reactive properties within the code. 
    vue will track the changes and update the DOM when data changes
    in this case i do so because i'd like the chart to have its data available on render. otherwise i could have technically used props
    in this case we don't need a prop because the component handles its own requists
    */
    data() {
        return {
            retrievedData: {},
        };
    },
    //set up websocket for dynamic data display
    mounted() {
        try {
            const ros = new ROSLIB.Ros({
                url: config.WS_URL,
            });

            const listener = new ROSLIB.Topic({
                ros,
                name: '/RandomData',
                messageType: 'topics_services/msg/RandomData',
            });
            //listener event defenition
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

<style></style>
