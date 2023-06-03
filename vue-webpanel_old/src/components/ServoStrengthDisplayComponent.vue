<template>
<div class="row justify-content-around text-center" v-if="this.info">
    <h1>Servo Strength</h1>
      
    <div v-for="(item, index) in this.info.data.value.data" :key="index" class="card col-3 p-1 m-1 row align-items-center" >
        <div style="max-width: 200px" class="text-center p-1 col-4 col-sm-12">
            <gauge :key="this.info.data.value.cpuUsage" symbol="%" :percentage="item "></gauge>
            <div class="spacer"></div>
            <div class="p-1" style="width: 200px;  background-color: green;">
                <h3>{{ this.info.data.value.labels[index] }} </h3>
                <p>At strength: {{item}} %</p>
                
            </div>
        </div>
    </div>
</div>
</template>

<script>
import axios from 'axios'
import gauge from './GaugeComponent.vue';
import config from '@/config';

export default {
    name: 'ServoSetrengthDisplayComponent',
    components: {
        gauge
    },
    data() {
        return {
            chartData: {
                labels: ['cpu usage in %'],
                datasets: [{
                    data: [100]
                }]
            },
            chartOptions: {
                responsive: true,
                scales: {
                    y: {
                        min: 0,
                        max: 100,
                    }
                }
            },
            info: null,
        }
    },
    mounted() {
        this.getData();
        setInterval(() => {

            this.getData();

        }, 1000);
    },
    beforeUnmount() {
        clearInterval(this.interval)
    },
    methods: {
        handleScroll(event) {
            event.preventDefault();
        },
        getData() {
            axios
                .get(config.API_URL+'/getservostats')
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
