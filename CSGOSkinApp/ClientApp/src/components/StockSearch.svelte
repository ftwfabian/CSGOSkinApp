<script>
    import { createEventDispatcher } from "svelte";

    let searchTicker = '';
    const dispatch = createEventDispatcher();

    async function handleSearch(){
        searchTicker = searchTicker.trim().toUpperCase();
        if(searchTicker === ""){
            dispatch('search',null);
            return;
        }
        const response = await fetch(`/api/StockApi/TickerExistsCheck?ticker=${searchTicker}`);

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const tickerExists = await response.json();

        if(tickerExists || searchTicker === ""){
            dispatch('search', searchTicker);
        } else {
            alert("Ticker does not exist");
        }
    }

    function handleKeyDown(event){
        if(event.key === 'Enter'){
            handleSearch();
        }
    }
</script>

<main>
<div class = "search-container">
    <input
        type = "text"
        bind:value={searchTicker}
        placeholder="Search for a ticker"
        on:keydown={handleKeyDown}
    />
    <button on:click={handleSearch} class="btn btn-primary">Search</button>
</div>

</main>

<style>

.search-container{
    display: flex;
    align-items: center;
    gap: 10px;
}
input{
    padding:5px;
    flex-grow: 1;
}

</style>