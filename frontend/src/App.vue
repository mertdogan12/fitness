<template>
  <div class="app">
    <header class="app-header">
      <h1>🏋️ Fitti Exempel – Kurskalender</h1>
    </header>
    <main>
      <KursKalender />
    </main>
  </div>
</template>

<script setup>
import KursKalender from './components/KursKalender.vue'
</script>

<style>
* {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

body {
  background: #0a0a1a;
  color: #e0e0e0;
  font-family: 'Segoe UI', sans-serif;
  min-height: 100vh;
}

.app-header {
  background: #1a1a2e;
  padding: 1.2rem 2rem;
  border-bottom: 2px solid #e63946;
}

.app-header h1 {
  font-size: 1.4rem;
  color: #fff;
  font-weight: 700;
}
</style>