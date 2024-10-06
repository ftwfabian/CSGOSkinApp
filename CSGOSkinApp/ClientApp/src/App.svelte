<script>
    import { Router, Route, Link } from "svelte-routing";
    import { onMount, beforeUpdate } from "svelte";
    import { navigate } from "svelte-routing";
    import SkinStatChart from './components/SkinStatChart.svelte';
	import HomePage from "./pages/HomePage.svelte";

    
    export let url = "";

    let isFirstLoad = true;

    function handleVisibilityChange() {
        if (document.visibilityState === 'visible') {
            const lastPageHide = sessionStorage.getItem('lastPageHide');
            const now = Date.now();
            
            if (lastPageHide && (now - parseInt(lastPageHide) < 50)) {
                // This is likely a page refresh
                navigate("/", { replace: true });
            }
        }
    }

    beforeUpdate(() => {
        if (isFirstLoad) {
            document.addEventListener('visibilitychange', handleVisibilityChange);
            isFirstLoad = false;
        }
    });

    onMount(() => {
        return () => {
            document.removeEventListener('visibilitychange', handleVisibilityChange);
        };
    });
</script>

<svelte:window on:pagehide={() => sessionStorage.setItem('lastPageHide', Date.now().toString())} />

<Router {url}>
<main>
    <nav>
        <Link to="/">Home</Link>
		<Link to="/Skin">Skin</Link>
    </nav>

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

    nav {
        margin-bottom: 1em;
    }

    nav :global(a) {
        margin-right: 1em;
    }

    @media (min-width: 640px) {
        main {
            max-width: none;
        }
    }
</style>