<template>
  <div class="wochen-navigator">
    <button @click="$emit('vorherige')" class="nav-btn">‹ Vorherige</button>

    <div class="wochen-titel">
      <span>{{ formatDatum(wochenstart) }} – {{ formatDatum(wochenende) }}</span>
    </div>

    <button
      @click="$emit('naechste')"
      :disabled="istMaxWoche"
      :class="['nav-btn', { disabled: istMaxWoche }]"
    >
      Nächste ›
    </button>
  </div>
</template>

<script setup>
const props = defineProps({
  wochenstart: Date,
  wochenende: Date,
  istMaxWoche: Boolean
})

defineEmits(['vorherige', 'naechste'])

function formatDatum(datum) {
  return datum?.toLocaleDateString('de-DE', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
  })
}
</script>

<style scoped>
.wochen-navigator {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1rem 2rem;
  background: #1a1a2e;
  border-radius: 12px;
  margin-bottom: 1.5rem;
}

.wochen-titel {
  font-size: 1.1rem;
  font-weight: 600;
  color: #e0e0e0;
}

.nav-btn {
  background: #e63946;
  color: white;
  border: none;
  padding: 0.5rem 1.2rem;
  border-radius: 8px;
  cursor: pointer;
  font-size: 0.95rem;
  font-weight: 600;
  transition: background 0.2s;
}

.nav-btn:hover:not(.disabled) {
  background: #c1121f;
}

.nav-btn.disabled {
  background: #555;
  cursor: not-allowed;
  opacity: 0.5;
}
</style>