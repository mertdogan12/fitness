<template>
  <div class="kurs-karte" :style="{ borderLeft: `4px solid ${farbe}` }">
    <div class="kurs-karte-header">
      <span class="kurs-titel">{{ termin.titel }}</span>
      <span class="kurs-uhrzeit">
        {{ formatZeit(termin.anfang) }} – {{ formatZeit(termin.ende) }}
      </span>
    </div>

    <div class="kurs-karte-body">
      <p class="kurs-trainer">👤 {{ termin.trainer }}</p>
      <p class="kurs-info">⏱ {{ termin.dauer }} Min.</p>
      <p v-if="termin.minAlter" class="kurs-info">🔞 ab {{ termin.minAlter }} Jahren</p>
      <p v-if="termin.geschlecht" class="kurs-info">
        {{ termin.geschlecht === 'w' ? '♀ Nur Frauen' : '♂ Nur Männer' }}
      </p>
    </div>

    <div class="kurs-karte-footer">
      <span class="teilnehmer">👥 {{ termin.teilnehmerAnzahl }} Teilnehmer</span>
    </div>
  </div>
</template>

<script setup>
defineProps({
  termin: Object,
  farbe: {
    type: String,
    default: '#e63946'
  }
})

function formatZeit(datum) {
  return new Date(datum).toLocaleTimeString('de-DE', {
    hour: '2-digit',
    minute: '2-digit'
  })
}
</script>

<style scoped>
.kurs-karte {
  background: #16213e;
  border-radius: 10px;
  padding: 0.75rem 1rem;
  margin-bottom: 0.6rem;
  transition: transform 0.15s, box-shadow 0.15s;
  cursor: default;
}

.kurs-karte:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 16px rgba(0,0,0,0.4);
}

.kurs-karte-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.4rem;
}

.kurs-titel {
  font-weight: 700;
  font-size: 0.95rem;
  color: #f0f0f0;
}

.kurs-uhrzeit {
  font-size: 0.78rem;
  color: #aaa;
}

.kurs-karte-body p {
  margin: 0.15rem 0;
  font-size: 0.82rem;
  color: #ccc;
}

.kurs-karte-footer {
  margin-top: 0.5rem;
  font-size: 0.78rem;
  color: #888;
}
</style>