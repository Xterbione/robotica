<template>
<h3>service window</h3>
<div class="card p-1 m-1" v-if="this.info" v-html="this.info.data.value"> </div>
</template>

<script>
import axios from 'axios'
import config from '@/config';

export default {
    name: 'BarChartServoComponent',
    components: {
    },
    data() {
        return {
            info: null,
        }
    },
    //when rendered set interval
    mounted() {
        this.getData();
        this.interval = setInterval(() => {
            this.getData();
        }, 10000); 
    },
    //clearing the interval before leaving the page
    beforeUnmount() {
        clearInterval(this.interval)
    },
    //defining 
    methods: {
        //disable scrolling
        handleScroll(event) {
            event.preventDefault();
        },
        //using axios for requist and setting the response to info
        getData() {
            axios
                .get(config.API_URL + '/servicestatus')
                .then(response => {
                    this.info = response;
                    this.info.data.value = this.info.data.value.replace(/\n/g, "<br />");
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
    height: 200px;
}
</style>
