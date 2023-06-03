<template>
    <div>
      <Line :data="chartData" :options="options" />
    </div>
  </template>
  
  <script>
  import { Chart as ChartJS, LineElement, Title, Tooltip, Legend } from 'chart.js';
  import { Line } from 'vue-chartjs';
  import ROSLIB from 'roslib';
  import config from '/src/config.js';
  
  ChartJS.register(LineElement, Title, Tooltip, Legend);
  
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
          datasets: [
            {
              label: 'Battery Voltage',
              backgroundColor: '#f87979',
              data: [],
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          scales: {
            x: {
              //maxTicksLimit is the maximum number of ticks and gridlines allowed for the x axis
              //this means that the chart will only show 20 ticks and gridlines at a time
              //ticks are the numbers on the x axis
              //gridlines are the lines that go from the x axis to the top of the chart
              maxTicksLimit: 20,
            },
          },
          animation: {
            duration: 0, // Disable animation when updating the chart
          },
        },
        newDataAllowed: true, // Flag to control new data addition
        retrievedData: {},
      };
    },
    /*using mounted to connect to the rosbridge server and subscribe to the topic    
    */
    mounted() {
      //try catch so vue won't be angry
      try {
        //const cant be reassigned
        //initializing roslib
        const ros = new ROSLIB.Ros({
          url: config.WS_URL,
        });
        //const cant be reassigned
        //initializing the topic listener from roslib
        const listener = new ROSLIB.Topic({
          ros,
          name: '/RandomData',
          messageType: 'topics_services/msg/RandomData',
        });
        //this function will be executed when a message is received
        listener.subscribe((message) => {
          this.retrievedData = message;
        });
        //executes when the connection is established
        ros.on('connection', () => {
          console.log('Connected to Rosbridge server');
        });
        //executes when there is an error
        ros.on('error', (error) => {
          console.error('Error connecting to Rosbridge server:', error);
        });
        //executes when the connection is closed
        ros.on('close', () => {
          console.log('Connection to Rosbridge server closed');
        });
  
        ros.connect(); // Connect to Rosbridge server
      } catch (error) {
        console.error('An error occurred while connecting to Rosbridge server:', error);
      }
    },
    //The watch option is used to watch for changes in retrievedData.
    // when the data changes the function will be executed
    watch: {
      /*
      for the watcher i specify the property to watch and make a handler function that will be called whenever the property changes.
       */
      retrievedData: {
        handler() {
          //checking if data is allowed, we get more data then we show to ceep things readable
          if (this.newDataAllowed) {
            //getting the data from the api and dividing it by 10 to get the voltage  
            const newData = this.retrievedData.batteryvoltage / 10;
            //updating the chart data
            const updatedChartData = {
              labels: [...this.chartData.labels, Date.now()],
              datasets: [
                {
                  /* the spread syntax (...arr1) expands the elements of arr1 and incorporates them into a new array arr2.
                  The result is that the elements of arr1 are expanded and added individually to arr2.
                  in short: ... inserts the properties as seperate elements*/
                  ...this.chartData.datasets[0],
                  data: [...this.chartData.datasets[0].data, newData],
                },
              ],
            };
            
            this.chartData = updatedChartData;
  
            if (this.chartData.labels.length > 50) {
              //remove first element to ceep stuff readable
              this.chartData.labels.shift();
              this.chartData.datasets[0].data.shift();
            }
  
            // Disable new data addition for x amojunt seconds
            this.newDataAllowed = false;
            setTimeout(() => {
              this.newDataAllowed = true;
            }, 300);
          }
        },
        deep: true,
      },
    },
  };
  </script>
  