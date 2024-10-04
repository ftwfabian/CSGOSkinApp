// src/store/api.js
import { writable } from 'svelte/store';

export const apiUrl = writable('http://localhost:5248/api');