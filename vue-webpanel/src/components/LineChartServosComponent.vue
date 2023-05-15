<template>
<Line :key="this.data" :data="data" :options="options" />
</template>

<script>
const data = {
    labels: [],
    datasets: [{
        label: 'no data',
        backgroundColor: '#f87979',
        data: [
            [0],
            [0],
            [0],
            [0],
            [0],
            [0]
        ]
    }]
}

const options = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
        x: {
            maxTicksLimit: 20,
        }
    },
    animation: {
        duration: 0
    }
}
import axios from 'axios'
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend
} from 'chart.js'
import config from '@/config'
import {
    Line
} from 'vue-chartjs'

ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend
)

export default {
    name: 'LineChartServosComponent',
    components: {
        Line
    },
    data() {
        return {
            data,
            options
        }
    },
    mounted() {
      this.getfirstData();
     setInterval(() => {

          this.getfirstData();

        }, 500);
    },
    beforeUnmount() {
        clearInterval(this.interval)
    },
    methods: {

        handleScroll(event) {
            event.preventDefault();
        },

        getfirstData() {

            axios
                .get(config.API_URL+'/getservostatsgraph')
                .then(response => {
                    console.log(response);

                    var insdata = [];
                    response.data.value.records.forEach(record => {
                        var ins = [];
                        record.data.forEach(element => {
                            ins.push(element[0]);
                        });
                        insdata.push({
                            label: record.label,
                            backgroundColor: record.backgroundcolor,
                            data: ins
                        });
                    });

                    const newData = {
                        labels: response.data.value.labels.slice(1),
                        datasets: insdata
                    }

                    this.data = newData;
                    console.log(this.data);
                })
                .catch(error => {
                    console.log("ERROR");
                    console.log(error);
                });

        },

    },
}
</script>
