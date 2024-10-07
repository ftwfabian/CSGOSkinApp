<script>
    import { createEventDispatcher } from "svelte";

    let searchSkinName = '';
    const dispatch = createEventDispatcher();

    async function handleSearch(){
        searchSkinName = searchSkinName.trim();
        if(searchSkinName !== ""){
            dispatch('search', searchSkinName);
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
        bind:value={searchSkinName}
        placeholder="Use keywords like 'stat fac night' to get results for StatTrak Factory New AK-47 Nightwish"
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