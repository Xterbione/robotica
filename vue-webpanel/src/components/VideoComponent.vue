<template>
    <div>
        <h1></h1>
      <canvas ref="canvas"></canvas>
    </div>
  </template>
  
  <script>
  import config from '/src/config.js';
    import * as ROSLIB from 'roslib';

  export default {
    mounted() {
      // Set up ROS connection
      const ros = new ROSLIB.Ros({
        url: config.WS_URL // Change this to match your ROS WebSocket server address
      });
  
      // Set up the RealSense camera subscriber
      const imageTopic = new ROSLIB.Topic({
        ros: ros,
        name: '/color/image_raw',
        messageType: 'sensor_msgs/Image'
      });
  
      // Get the canvas element
      const canvas = this.$refs.canvas;
  
      // Set up the canvas context
      const ctx = canvas.getContext('2d');
  
      // Subscribe to the image topic
      imageTopic.subscribe((message) => {
        const { width, height, data } = message;
  
        // Set the canvas dimensions to match the image dimensions
        canvas.width = width;
        canvas.height = height;
  
        // Create a new ImageData object with the image data
        const imageData = new ImageData(new Uint8ClampedArray(data), width, height);
  
        // Clear the canvas
        ctx.clearRect(0, 0, canvas.width, canvas.height);
  
        // Draw the image data onto the canvas
        ctx.putImageData(imageData, 0, 0);
      });
    }
  };
  </script>
  
  <style scoped>
  canvas {
    border: 1px solid black;
  }
  </style>