import { defineConfig } from 'astro/config';

import react from '@astrojs/react';

import tailwind from '@astrojs/tailwind';

export default defineConfig({
  integrations: [react(), tailwind()],
  site: "https://wrobirson.github.io/",
  base: 'twitch-lm-chatbot',
  outDir: './../docs',
});