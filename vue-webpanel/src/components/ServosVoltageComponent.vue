<template>
<Bar id="my-chart-id" :options="chartOptions" :data="chartData" :key="chartData.datasets[0].data" name="defined" />
</template>

    
<script>
import axios from 'axios'
import config from '/src/config.js';


import {
    Bar
} from 'vue-chartjs'
import {
    Chart as ChartJS,
    Title,
    Tooltip,
    Legend,
    BarElement,
    CategoryScale,
    LinearScale
} from 'chart.js'

ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale)

export default {
    name: 'ServosVoltageComponent',
    components: {
        Bar
    },
    data() {
        return {
            chartData: {
                labels: ['no', 'data', 'yet'],
                datasets: [{
                    data: [0, 0, 0]
                }]
            },
            chartOptions: {
                animation: {
                    duration: 0
                },
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
                .get(config.API_URL + '/getservostats')
                .then(response => {
                    var pos = document.documentElement.scrollTop;
                    this.info = response;
                    this.chartData.datasets[0].data = response.data.value.data;
                    this.chartData.labels = response.data.value.labels;
                    document.documentElement.scrollTop = pos;
                })
                .catch(error => {
                    console.log(error);
                });

        },
    },
}
</script>
