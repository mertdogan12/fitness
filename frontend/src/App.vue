<template>
  <div class="app">
    <header class="app-header">
      <h1>🏋️ Fitti Exempel – Kurskalender</h1>
      <button 
        class="trainer-btn" 
        @click="route.path === '/trainer/dashboard' ? $router.push('/') : $router.push('/trainer/dashboard')"
      >
        {{ route.path === '/trainer/dashboard' ? '← Zurück zum Kalender' : '🔐 Trainer-Bereich' }}
      </button>
    </header>
    <main>
      <RouterView />
    </main>
  </div>
</template>

<script setup>
import { RouterView } from 'vue-router'
import { useRoute } from 'vue-router'

const route = useRoute()
</script>

<style>
* {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

body {
  background-color: #0a0a1a;
  background-image: 
    linear-gradient(rgba(0, 0, 0, 0.75), rgba(0, 0, 0, 0.85)),
    url('https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=1920&q=80');
  background-size: cover;
  background-position: center;
  background-attachment: fixed;
  color: #e0e0e0;
  font-family: 'Segoe UI', sans-serif;
  min-height: 100vh;
}

.app-header {
  background: rgba(10, 10, 26, 0.85);
  backdrop-filter: blur(10px);
  padding: 1.2rem 2rem;
  border-bottom: 2px solid #e63946;
  display: flex;
  justify-content: space-between;
  align-items: center;
  position: sticky;
  top: 0;
  z-index: 50;
}

.app-header h1 {
  font-size: 1.4rem;
  color: #fff;
  font-weight: 700;
}

.trainer-btn {
  background: transparent;
  border: 1px solid #e63946;
  color: #e63946;
  padding: 0.4rem 1rem;
  border-radius: 8px;
  cursor: pointer;
  font-size: 0.85rem;
  font-weight: 600;
  transition: all 0.2s;
}

.trainer-btn:hover {
  background: #e63946;
  color: white;
}
</style>