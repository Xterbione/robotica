<template>
  <div style="container">
    <div class="container-local">
      <div class="gauge-bg"></div>
      <div class="gauge-middle"></div>
      <div class="gauge-overlay" :style="{ transform: rotate }"></div>
      <div class="gauge-data">
        <span>{{ percentage }} {{symbol}}</span>
      </div>
    </div>
  </div>
  </template>
  
  <script>
  export default {
    props: ["percentage", "symbol"],
    computed: {
      rotate() {
        const v = (this.percentage * 180) / 100;
        return `rotate(${v}deg)`;
      },
    },
  };
  </script>
  <style scoped>
  .container-local {
    width: 200px;
    height: 100px;
    position: absolute;
    top: 5px;
    overflow: hidden;
  }
  .container{
    width: 200px;
    height: 100px;
  }
  .gauge-bg {
    z-index: 1;
    width: 200px;
    height: 100px;
    position: absolute;
    background-color: #a3f6ba;
    border-radius: 100px 100px 0 0;
  }
  .gauge-middle {
    z-index: 3;
    position: absolute;
    background-color: #4d4d4d;
    width: 125px;
    height: calc(125px / 2);
    top: 37.5px;
    margin-left: 37.5px;
    margin-right: auto;
    border-radius: 125px 125px 0 0;
  }
  .gauge-overlay {
    z-index: 2;
    position: absolute;
    background-color: #5df086;
    width: 200px;
    height: 100px;
    top: 100px;
    border-radius: 0 0 100px 100px;
    transform-origin: center top;
  }
  .gauge-data {
    z-index: 4;
    color: #5df086;
    position: absolute;
    width: 200px;
    bottom: 0;
    text-align: center;
    font-size: 18px;
  }
  </style>