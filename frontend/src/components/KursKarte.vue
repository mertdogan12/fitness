<template>
  <div
    class="kurs-karte"
    :style="{ borderLeft: `4px solid ${farbe}` }"
    :class="{ voll: termin.istVoll }"
    @click="!termin.istVoll && $emit('kursGeklickt', termin)"
  >
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
      <!-- Auslastungsbalken -->
      <div class="auslastung-balken-hintergrund">
        <div
          class="auslastung-balken-fuell"
          :style="{
            width: auslastungProzent + '%',
            background: auslastungFarbe
          }"
        ></div>
      </div>

      <div class="auslastung-text">
        <span>👥 {{ termin.teilnehmerAnzahl }} / {{ termin.maxTeilnehmer }}</span>
        <span v-if="termin.istVoll" class="voll-label">Ausgebucht</span>
        <span v-else class="frei-label">{{ termin.maxTeilnehmer - termin.teilnehmerAnzahl }} Plätze frei</span>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  termin: Object,
  farbe: {
    type: String,
    default: '#e63946'
  }
})

defineEmits(['kursGeklickt'])

const auslastungProzent = computed(() =>
  Math.min((props.termin.teilnehmerAnzahl / props.termin.maxTeilnehmer) * 100, 100)
)

const auslastungFarbe = computed(() => {
  if (auslastungProzent.value >= 100) return '#e63946'  // rot = voll
  if (auslastungProzent.value >= 75) return '#f9c784'   // orange = fast voll
  return '#a8d8a8'                                       // grün = Plätze frei
})

function formatZeit(datum) {
  return new Date(datum).toLocaleTimeString('de-DE', {
    hour: '2-digit',
    minute: '2-digit'
  })
}
</script>

<style scoped>
/* bestehende styles bleiben, folgendes NEU hinzufügen: */

.kurs-karte.voll {
  opacity: 0.55;
  cursor: not-allowed;
}

.kurs-karte.voll:hover {
  transform: none;
  box-shadow: none;
}

.auslastung-balken-hintergrund {
  height: 5px;
  background: #2a2a5a;
  border-radius: 99px;
  margin-bottom: 0.4rem;
  overflow: hidden;
}

.auslastung-balken-fuell {
  height: 100%;
  border-radius: 99px;
  transition: width 0.4s ease;
}

.auslastung-text {
  display: flex;
  justify-content: space-between;
  font-size: 0.75rem;
  color: #888;
}

.voll-label {
  color: #e63946;
  font-weight: 700;
}

.frei-label {
  color: #a8d8a8;
}
</style>