<template>
<div>
    <h3>Object recognition and Sensor information</h3>
    <div class="align-items-center">
       <!-- <img v-if="this.streamlink != null" style="height: 300px; width: 400px;" :key="this.streamlink" :src="`${this.streamlink}?${Date.now()}`" />
       -->
    </div>
    <table class="table table-dark">
        <thead>
            <tr>
                <th scope="col">Sensor</th>
                <th scope="col">Value</th>
            </tr>
        </thead>
        <tbody v-if="this.info != null" :key="this.info">
            <tr>
                <th scope="row">Currently detected object:</th>
                <td>{{this.info.data.value.currentObject}}</td>
            </tr>
            <tr>
                <th scope="row">Detected QR Value:</th>
                <td>{{this.info.data.value.qrValue}}</td>
            </tr>
            <tr>
                <th scope="row">weight:</th>
                <td>{{this.info.data.value.weight}} in grams</td>
            </tr>
            <tr>
                <th scope="row">Temperature:</th>
                <td>{{this.info.data.value.temperature}} Celcius</td>
            </tr>
            <tr>
                <th scope="row">frequentie categorie:</th>
                <td>{{this.info.data.value.category}}</td>
            </tr>
            <tr>
                <th scope="row">huidige geluidsfrequentie:</th>
                <td>{{ this.info.data.value.frequentie }} Hz</td>
            </tr>
        </tbody>
    </table>
</div>
</template>

<script>
import axios from 'axios';
import config from '/src/config.js';

export default {
    name: 'RecognitionSensorDisplay',
    components: {},
    data() {
        return {
            info: null,
            streamlink:  config.API_URL+"/webcamframe",
        }
    },
    mounted() {
        this.getData();
        setInterval(() => {

            this.getData();

        }, 3000);
        setInterval(() => {

            this.updateframe();

        }, 3000);
    },
    beforeUnmount() {
        clearInterval(this.interval)
    },
    methods: {
        handleScroll(event) {
            event.preventDefault();
        },
        updateframe() {
            this.streamlink = config.API_URL+"/webcamframe"
        },
        getData() {
            axios
                .get(config.API_URL+'/getobjectsensor')
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

<style>

</style>
