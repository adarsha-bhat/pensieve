import { svelte } from '@sveltejs/vite-plugin-svelte'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [svelte()],
  server: {
    // During development the API runs separately; forward /api calls to it.
    proxy: {
      '/api': 'http://localhost:5141',
    },
  },
  build: {
    // The API serves the built frontend from its wwwroot folder.
    outDir: '../Pensieve.Api/wwwroot',
    emptyOutDir: true,
  },
})
