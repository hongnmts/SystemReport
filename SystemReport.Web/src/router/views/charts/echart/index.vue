<script>
import ECharts from "vue-echarts";
import "echarts/lib/component/polar";
import "echarts/lib/chart/line";
import "echarts/lib/chart/pie";
import "echarts/lib/chart/scatter";
import "echarts/lib/component/legend";
import "echarts/lib/component/title";
import "echarts/lib/component/toolbox";
import "echarts/lib/chart/bar";

import Layout from "../../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";

import {
  polarchartdata,
  lineChart,
  barChart,
  pieChart,
  donutChart,
  areaChart
} from "./data";

/**
 * E - chart component
 */
export default {
  page: {
    title: "E - chart",
    meta: [{ name: "description", content: appConfig.description }]
  },
  components: {
    Layout,
    PageHeader,
    "v-chart": ECharts
  },
  data() {
    return {
      polarchartdata: polarchartdata,
      lineChart: lineChart,
      barChart: barChart,
      pieChart: pieChart,
      donutChart: donutChart,
      areaChart: areaChart,
      title: "E - Charts",
      items: [
        {
          text: "Veltrix",
          href: "/"
        },
        {
          text: "Charts",
          href: "/"
        },
        {
          text: "E - Charts",
          active: true
        }
      ]
    };
  }
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items" />

    <div class="row">
      <div class="col-lg-6">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title mb-4">Polar Chart</h4>
            <v-chart :options="polarchartdata" :autoresize="true" />
          </div>
        </div>
      </div>
      <div class="col-lg-6">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title mb-4">Line Chart</h4>
            <v-chart :options="lineChart" :autoresize="true" />
          </div>
        </div>
      </div>
    </div>
    <div class="row">
      <div class="col-lg-6">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title mb-4">Bar Chart</h4>
            <v-chart :options="barChart" :autoresize="true" />
          </div>
        </div>
      </div>
      <div class="col-lg-6">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title mb-4">Pie Chart</h4>
            <v-chart :options="pieChart" :autoresize="true" />
          </div>
        </div>
      </div>
    </div>
    <div class="row">
      <div class="col-lg-6">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title mb-4">Donut Chart</h4>
            <v-chart :options="donutChart" :autoresize="true" />
          </div>
        </div>
      </div>
      <div class="col-lg-6">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title mb-4">Area Chart</h4>
            <v-chart :options="areaChart" :autoresize="true" />
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>

<style>
.e-charts{
    height: 300px;
}
</style>