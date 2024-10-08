<script>
import { onMount } from 'svelte';
import Chart from 'chart.js/auto';
import { skinApiUrl } from '../store/api.js';
import { get } from 'svelte/store';
import SkinSearch from './SkinSearch.svelte';

export let skinName = '';
let skinImageLink = '';
let chartCanvas;
let chart;
let error = null;
let isLoading = true;
let chartData = null;

$: {
    console.log("skinName changed:", skinName);
    if (skinName) {
        console.log("Triggering updateChart due to skinName change");
        updateChart();
    }
}

async function fetchSkinData(skinName) {
    console.log("Fetching data for:", skinName);
    const response = await fetch(`${get(skinApiUrl)}/Skins/${skinName}`);
    const responseData = await response.json();
    if (!responseData.success || !Array.isArray(responseData.data)) {
        throw new Error('Failed to fetch skin data or invalid data format');
    }
    return responseData.data;
}

function createChart() {
    if (!chartCanvas || !chartData) {
        console.log("Canvas or chart data not available");
        return;
    }
    
    console.log("Creating chart with data:", chartData);
    if (chart) {
        chart.destroy();
    }
    const ctx = chartCanvas.getContext('2d');
    chart = new Chart(ctx, {
        type: 'scatter',
        data: {
            datasets: [{
                label: `${skinName} Price vs Float`,
                data: chartData.map(item => ({
                    x: item.float,
                    y: item.marketPrice
                })),
                backgroundColor: 'rgba(240, 128, 128, 0.6)'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                x: {
                    type: 'linear',
                    position: 'bottom',
                    title: {
                        display: true,
                        text: 'Float'
                    }
                },
                y: {
                    title: {
                        display: true,
                        text: 'Market Price ($)'
                    }
                }
            }
        }
    });
}

function handleSearch(event) {
    console.log("Search event received:", event.detail);
    skinName = event.detail;
}

async function updateChart() {
    console.log("Updating chart for:", skinName);
    error = null;
    isLoading = true;
    chartData = null;
    try {
        chartData = await fetchSkinData(skinName);
        skinName = chartData[0].name;
        skinImageLink = chartData[0].url;
        createChart();
    } catch (err) {
        console.error('Error updating chart:', err);
        error = err.message;
    } finally {
        isLoading = false;
    }
}

onMount(() => {
    console.log("Component mounted, initial skinName:", skinName);
    if (skinName) {
        updateChart();
    } else {
        isLoading = false;
    }
    return () => {
        if (chart) {
            chart.destroy();
        }
    };
});

$: if (chartCanvas && chartData) {
    createChart();
}
</script>

<main>
    <div class="content-wrapper">
        <div class="chart-box">
            <SkinSearch on:search={handleSearch}/>
            <br/>
            <h1>{skinName ? `${skinName} Price vs Float` : 'Search for a skin'}</h1>
            {#if isLoading}
                <p>Loading...</p>
            {:else if error}
                <p class="error">{error}</p>
            {:else if !skinName}
                <p>...</p>
            {:else}
                <div class="chart-container">
                    <canvas bind:this={chartCanvas}></canvas>
                </div>
            {/if}
        </div>
        {#if skinImageLink}
            <div class="image-container">
                <img src={skinImageLink} alt={skinName} />
            </div>
        {/if}
    </div>
</main>

<style>
  main {
      max-width: 1200px;
      margin: 0 auto;
      padding: 20px;
      color: rgb(240, 128, 128);
  }

  .content-wrapper {
      display: flex;
      align-items: flex-start;
      gap: 20px;
  }

  .chart-box {
      background-color: white;
      border-radius: 8px;
      padding: 20px;
      box-shadow: 0 6px 6px rgba(0,0,0,0.1);
      flex: 1;
      max-width: 50%;
  }

  h1 {
      margin-top: 0;
      margin-bottom: 15px;
      text-align: center;
  }

  .chart-container {
      width: 100%;
      height: 400px;
      margin: 0 auto;
  }

  .error {
      color: red;
      text-align: center;
  }

  .image-container {
      flex: 0 0 auto;
      width: 40%;
  }

  .image-container img {
    padding-top: 35%;
      width: 100%;
      height: auto;
      transform: rotate(45deg);
      transform-origin: center center;
  }
</style>