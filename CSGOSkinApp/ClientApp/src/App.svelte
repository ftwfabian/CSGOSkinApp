<script>
	import { apiUrl } from './store/api.js';
	import { get } from 'svelte/store';
	export let name;
	console.log(get(apiUrl));
	callApi();
	

	async function callApi()
	{
		let thing = await fetch(`${get(apiUrl)}/Skins/Night`);
		let thingone = await thing.json();
		console.log(thingone);
	}
</script>

<main>
	<h1>Hello {name}!</h1>
	<p>Visit the <a href="https://svelte.dev/tutorial">Svelte tutorial</a> to learn how to build Svelte apps.</p>
	<p>What can you do about it</p>
</main>

<style>
	main {
		text-align: center;
		padding: 1em;
		max-width: 240px;
		margin: 0 auto;
	}

	h1 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 4em;
		font-weight: 100;
	}

	@media (min-width: 640px) {
		main {
			max-width: none;
		}
	}
</style>