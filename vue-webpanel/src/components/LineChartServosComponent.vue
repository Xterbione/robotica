<template>
    <Line :data="chartData" :options="options" />
  </template>
  
  <script>
  import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend,
  } from 'chart.js'
  import { Line } from 'vue-chartjs'
  
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
      Line,
    },
     /*using data() for reactive properties within the code. 
    vue will track the changes and update the DOM when data changes
    in this case i do so because i'd like the chart to have its data available on render. otherwise i could have technically used props
    */
    data() {
      return {
        chartData: {
          labels: [],
          datasets: [],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          scales: {
            x: {
              maxTicksLimit: 20,
            },
          },
          animation: {
            duration: 0, // Disable animation when updating the chart
          },
        },
        newDataAllowed: true, // Flag to control new data addition
      }
    },
    //props to retrieve data from parent component
    props: {
      retrievedData: {
        type: Object,
        required: true,
      },
    },
    //watching the retrievedData object for changes
    watch: {
      /*watching the retrievedData object for changes
      */
      retrievedData: {
        handler() {
          //if the newDataAllowed flag is true, the retrievedData object is filtered for the keys that contain the word voltage
          if (this.newDataAllowed) {
            //object.keys returns an array of a given object's ownd property names on which we can filter
            const voltages = Object.keys(this.retrievedData).filter(
              (key) => key.indexOf('voltage') !== -1
            );
            //the filtered keys are mapped to the retrievedData object and the values are stored in the newData array
            const newData = voltages.map((voltage) => this.retrievedData[voltage]);
            //defining the colors for the chart
            const colors = ['#f87979', '#79f879', '#7979f8', '#f8d179', '#79f8d9'];
            //the datasets array is filled with the data from the newData array
            //a lambda function is used to iterate over the array and return the data in the correct format
            const datasets = newData.map((data, index) => {
              const servoName = voltages[index].split('voltage')[1];
              return {
                label: `Servo ${servoName}`,
                backgroundColor: colors[index % colors.length],
                data: [...(this.chartData.datasets[index]?.data || []), data],
              };
            });
            //the updatedChartData object is created with the new data.
            const updatedChartData = {
              labels: [...this.chartData.labels, Date.now()],
              datasets: datasets,
            };
            //the chartData object is updated with the updatedChartData object
            this.chartData = updatedChartData;
            //ceep it readable
            if (this.chartData.labels.length > 10) {
              //removing label
              this.chartData.labels.shift();

              //removing data
              this.chartData.datasets.forEach((dataset) => {
                dataset.data.shift();
              });
            }
  
            // Disable new data addition for 3 seconds
            this.newDataAllowed = false;
            setTimeout(() => {
              this.newDataAllowed = true;
            }, 300);
          }
        },
        deep: true,
      },
    },
  }
  </script>
  