import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'

export default defineConfig({
  plugins: [react()],
  base: "/",
  server: {
    port: 5176,
    allowedHosts: true,
    host: true,
    hmr: {
      overlay: true
    }
  },
  optimizeDeps: {
    exclude: ['@fortawesome/fontawesome-svg-core'],
    include: ['react', 'react-dom', 'react-router-dom']
  },
  build: {
    sourcemap: false,
    rollupOptions: {
      output: {
        manualChunks: {
          'react-vendor': ['react', 'react-dom', 'react-router-dom'],
          'ui-vendor': ['framer-motion']
        }
      }
    }
  },
  test: {
    globals: true,
    environment: 'jsdom',
    setupFiles: './src/__tests__/setup.js',
    coverage: {
      reporter: ['text', 'json', 'html'],
    },
  },
})
