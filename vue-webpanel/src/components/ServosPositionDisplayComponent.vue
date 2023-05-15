<template>
<div class="row m-0 p-0 justify-content-around text-center" v-if="this.info">
    <div v-for="(item, index) in this.info.data.value"  :key="index" class="card col-4 p-1 text-center row align-items-center">
        <RadialProgressBarComponent :completedSteps="item.data[0]" />
        <p>{{item.label}}</p>
        <p>{{item.data[0]}}°</p>
        
    </div>
</div>
</template>

<script>
import axios from 'axios'
import RadialProgressBarComponent from './RadialProgressBarComponent.vue';
import config from '@/config';
export default {
    name: 'ServoSetrengthDisplayComponent',
    components: {
        RadialProgressBarComponent
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

        }, 300);
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
                .get(config.API_URL + '/getservopos')
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
