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
    mounted() {
        this.getData();
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
