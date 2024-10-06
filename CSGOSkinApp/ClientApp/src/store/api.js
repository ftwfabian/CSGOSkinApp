// src/store/api.js
import { writable } from 'svelte/store';

export const skinApiUrl = writable('http://localhost:5248/api');