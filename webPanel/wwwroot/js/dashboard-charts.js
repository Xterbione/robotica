//to add more gauges, simply coppy all the relevant code and change the name of the variables to something else
const ctx1 = document.getElementById("myChart");

new Chart(ctx1, {
  type: "bar",
  data: {
    labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
    datasets: [
      {
        label: "# of Votes",
        data: [12, 19, 3, 5, 2, 3],
        borderWidth: 1,
      },
    ],
  },
  options: {
    scales: {
      y: {
        beginAtZero: true,
      },
    },
  },
});

// setup
const data = {
  labels: ["left", "middle", "right"],
  datasets: [
    {
      label: "Gauge",
      data: [75, 50, 75],
      backgroundColor: [
        "rgba(75, 192, 192, 0.8)",
        "rgba(255, 206, 86, 0.8)",
        "rgba(75, 192, 192, 0.8)",
      ],
      needleValue: 100,
      borderColor: "white",
      borderWidth: 2,
      cutout: "95%",
      circumference: 180,
      rotation: 270,
      borderRadius: 5,
    },
  ],
};

// setup
const data2 = {
  labels: ["left", "middle", "right"],
  datasets: [
    {
      label: "Gauge2",
      data: [75, 50, 75],
      backgroundColor: [
        "rgba(75, 192, 192, 0.8)",
        "rgba(255, 206, 86, 0.8)",
        "rgba(75, 192, 192, 0.8)",
      ],
      needleValue: 100,
      borderColor: "white",
      borderWidth: 2,
      cutout: "95%",
      circumference: 180,
      rotation: 270,
      borderRadius: 5,
    },
  ],
};

//gaugeNeedle
const gaugeNeedle = {
  id: "gaugeNeedle",
  afterDatasetDraw(chart, args, options) {
    const {
      ctx,
      config,
      data,
      chartArea: { top, right, bottom, left, width, height },
    } = chart;
    ctx.save();
    const needleValue = data.datasets[0].needleValue;
    const dataTotal = data.datasets[0].data.reduce((a, b) => a + b, 0);

    if (needleValue <= 100) {
      var angle = Math.PI + (1 / 200) * needleValue * Math.PI;
      console.log(angle);
    } else if (needleValue <= 10000) {
      var angle =
        Math.PI +
        (1 / 200) * 100 * Math.PI +
        ((1 / 200) * needleValue * Math.PI * 65) / 10000;
    } else if (needleValue <= 1000000) {
      var angle =
        Math.PI +
        (1 / 200) * 100 * Math.PI +
        ((1 / 200) * 10000 * Math.PI * 65) / 10000 +
        ((1 / 200) * needleValue * Math.PI * 35) / 1000000;
    } else {
      var angle = 0;
    }

    const cx = width / 2;
    const cy = chart._metasets[0].data[0].y;

    //needle
    ctx.translate(cx, cy);
    ctx.rotate(angle);
    ctx.beginPath();
    ctx.moveTo(0, -2);
    ctx.lineTo(height - ctx.canvas.offsetTop - 160, 0); // change 160 value if the needle size gets changed
    ctx.lineTo(0, 2);
    ctx.fillStyle = "#444";
    ctx.fill();
    //needle dot
    ctx.translate(-cx, -cy);
    ctx.beginPath();
    ctx.arc(cx, cy, 5, 0, 10);
    ctx.fill();
    ctx.restore();

    //text
    ctx.font = "20px Ubuntu";
    ctx.fillStyle = "#444";
    ctx.fillText(needleValue + " current position(100=0)", cx, cy + 50);
    ctx.font = "10px Ubuntu";
    ctx.fillText(0, 5, cy + 20);
    ctx.fillText(100, cx, 90);
    ctx.fillText("10k", cx + 185, 200); // change values if the position gets changed
    ctx.fillText("1M", cx + 193, 320); // change values if the position gets changed
    ctx.textAlign = "center";
    ctx.restore();
  },
};

const gaugeNeedle2 = {
  id: "gaugeNeedle2",
  afterDatasetDraw(chart, args, options) {
    const {
      ctx,
      config,
      data,
      chartArea: { top, right, bottom, left, width, height },
    } = chart;
    ctx.save();
    const needleValue = data.datasets[0].needleValue;
    const dataTotal = data.datasets[0].data.reduce((a, b) => a + b, 0);

    if (needleValue <= 100) {
      var angle = Math.PI + (1 / 200) * needleValue * Math.PI;
      console.log(angle);
    } else if (needleValue <= 10000) {
      var angle =
        Math.PI +
        (1 / 200) * 100 * Math.PI +
        ((1 / 200) * needleValue * Math.PI * 65) / 10000;
    } else if (needleValue <= 1000000) {
      var angle =
        Math.PI +
        (1 / 200) * 100 * Math.PI +
        ((1 / 200) * 10000 * Math.PI * 65) / 10000 +
        ((1 / 200) * needleValue * Math.PI * 35) / 1000000;
    } else {
      var angle = 0;
    }

    const cx = width / 2;
    const cy = chart._metasets[0].data[0].y;

    //needle
    ctx.translate(cx, cy);
    ctx.rotate(angle);
    ctx.beginPath();
    ctx.moveTo(0, -2);
    ctx.lineTo(height - ctx.canvas.offsetTop - 160, 0); // change 160 value if the needle size gets changed
    ctx.lineTo(0, 2);
    ctx.fillStyle = "#444";
    ctx.fill();
    //needle dot
    ctx.translate(-cx, -cy);
    ctx.beginPath();
    ctx.arc(cx, cy, 5, 0, 10);
    ctx.fill();
    ctx.restore();

    //text
    ctx.font = "20px Ubuntu";
    ctx.fillStyle = "#444";
    ctx.fillText(needleValue + " current position(100=0)", cx, cy + 50);
    ctx.font = "10px Ubuntu";
    ctx.fillText(0, 5, cy + 20);
    ctx.fillText(100, cx, 90);
    ctx.fillText("10k", cx + 185, 200); // change values if the position gets changed
    ctx.fillText("1M", cx + 193, 320); // change values if the position gets changed
    ctx.textAlign = "center";
    ctx.restore();
  },
};

// config
const config = {
  type: "doughnut",
  data,
  options: {
    plugins: {
      legend: {
        display: false,
      },
      tooltip: {
        yAlign: "bottom",
        displayColors: false,
        callbacks: {
          label: function (tooltipItem, data, value) {
            return tooltipItem.label;
          },
        },
      },
    },
  },
  plugins: [gaugeNeedle],
};

const config2 = {
  type: "doughnut",
  data,
  options: {
    plugins: {
      legend: {
        display: false,
      },
      tooltip: {
        yAlign: "bottom",
        displayColors: false,
        callbacks: {
          label: function (tooltipItem, data, value) {
            return tooltipItem.label;
          },
        },
      },
    },
  },
  plugins: [gaugeNeedle2],
};

var ctx2 = document.getElementById("LineChart").getContext("2d");

var chart = new Chart(ctx2, {
  type: "line",
  data: {
    labels: [
      "15:00:00",
      "15:00:01",
      "15:00:02",
      "15:00:03",
      "15:00:04",
      "15:00:05",
      "15:00:06",
      "15:00:07",
      "15:00:08",
      "15:00:09",
      "15:00:10",
      "15:00:11",
    ],
    datasets: [
      {
        label: "motor1",
        data: [60, 50, 40, 40, 50, 0, 0, 0, 0, 0, 0, 0],
        fill: false,
        borderColor: "red",
      },
      {
        label: "motor2",
        data: [20, 30, 40, 50, 60, 70, 0, 0, 0, 0, 0, 0],
        fill: false,
        borderColor: "blue",
      },
    ],
  },
});

// render init block
const myChart = new Chart(document.getElementById("gauge"), config);
const myChart2 = new Chart(document.getElementById("gauge2"), config2);