<template>
    <Bar
      id="my-chart-id"
      :options="chartOptions"
      :data="chartData"
      :key="chartData.datasets[0].data"
      name="defined"
    />
  </template>
  
  <script>
    /* eslint-disable */
    import config from '/src/config.js';
  
    import { Bar } from 'vue-chartjs';
    import {
      Chart as ChartJS,
      Title,
      Tooltip,
      Legend,
      BarElement,
      CategoryScale,
      LinearScale
    } from 'chart.js';
  
    ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale);
  
    export default {
      name: 'ServosVoltageComponent',
      components: {
        Bar
      },
      //have to do this according to chartjs to register the plugins
      mounted() {
        ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale);
      },
      //using props to get the data from the parent component
      props: {
        retrievedData: {
          type: Object,
          required: true,
        },
      },
           /*using data() for reactive properties within the code. 
    vue will track the changes and update the DOM when data changes
    in this case i do so because i'd like the chart to have its data available on render. otherwise i could have technically used props
    */
      data() {
        return {
          chartData: {
            labels: ['no', 'data', 'yet'],
            datasets: [
              {
                data: [0, 0, 0],
                backgroundColor: ['rgba(54, 162, 235, 0.5)', 'rgba(75, 192, 192, 0.5)', 'rgba(255, 99, 132, 0.5)'],
              }
            ]
          },
          chartOptions: {
            animation: {
              duration: 0
            },
            responsive: true,
            scales: {
              y: {
                min: 8,
                max: 13,
              }
            }
          },
          info: null,
        };
      },
      //using computed to update the chart data
      //computeds are cached, meaning they only recompute when their value/dependencies change.
      computed: {
        chartData() {
          const voltages = Object.keys(this.retrievedData).filter(key => key.indexOf('voltage') !== -1);
          const labels = voltages.map(voltage => voltage);
          return {
            labels,
            datasets: [
              {
                //using map to get the data from the object and set it to the chart
                data: voltages.map(voltage => this.retrievedData[voltage]),
                backgroundColor: [
                  'rgba(54, 162, 235, 0.5)',
                  'rgba(75, 192, 192, 0.5)',
                  'rgba(255, 99, 132, 0.5)',
                  // Add more colors if needed
                ],
              }
            ]
          };
        }
      },
    };
  </script>
  
  <style>
  #my-chart-id {
    height: 600px !important;
    width: 100% !important;
    display: block !important;
    box-sizing: border-box !important;
  }
  </style>
  