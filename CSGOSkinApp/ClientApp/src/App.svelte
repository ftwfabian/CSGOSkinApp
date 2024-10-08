<script>
    import { Router, Route, Link } from "svelte-routing";
    import { onMount, beforeUpdate } from "svelte";
    import { navigate } from "svelte-routing";
    import SkinStatChart from './components/SkinStatChart.svelte';
	import HomePage from "./pages/HomePage.svelte";

    
    export let url = "";

</script>

<svelte:window on:pagehide={() => sessionStorage.setItem('lastPageHide', Date.now().toString())} />

<Router {url}>
<main>
	<div class = "navbar-box">
		<nav>
			<Link to="/">Home</Link>
			<Link to="/Skin">Skin</Link>
		</nav>
	</div>
	<br/>

    <Route path="/">
        <HomePage/>
    </Route>
	<Route path="/Skin">
		<SkinStatChart/>
	</Route>
</main>
</Router>

<style>
    main {
        text-align: center;
        padding: 1em;
        max-width: 1000px;
        margin: 0 auto;
    }

	.navbar-box {
        background-color: white;
        border-radius: 3px;
        box-shadow: 0 6px 6px rgba(0,0,0,0.1);
        margin: 0 auto 1em auto;
        width: 20%;
        height: 30px; /* Adjust this value as needed */
    }

    nav {
        display: flex;
        justify-content: center;
        align-items: center;
        height: 100%;
    }

    nav :global(a) {
        margin-right: 1em;
    }

	.navbar-box {
      background-color: white;
      border-radius: 8px;
      padding: 1px;
      box-shadow: 0 2px 6px rgba(0,0,0,0.1);
	  margin-left: auto;
	  margin-right: auto;
	  margin: 0 auto;
	  width: 20%;
	  
  	}

    @media (min-width: 640px) {
        main {
            max-width: none;
        }
    }
</style>