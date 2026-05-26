<template>
  <div
    class="kurs-karte"
    :class="{ voll: termin.istVoll }"
    @click="!termin.istVoll && $emit('kursGeklickt', termin)"
  >
    <div class="kurs-karte-top" :style="{ background: farbe }">
      <span class="kurs-titel">{{ termin.titel }}</span>
      <span class="kurs-uhrzeit">{{ formatZeit(termin.anfang) }} – {{ formatZeit(termin.ende) }}</span>
    </div>

    <div class="kurs-karte-body">
      <div class="kurs-row">
        <span class="label">Trainer</span>
        <span>{{ termin.trainer }}</span>
      </div>
      <div class="kurs-row">
        <span class="label">Dauer</span>
        <span>{{ termin.dauer }} Min.</span>
      </div>
      <div class="kurs-row" v-if="termin.minAlter">
        <span class="label">Mindestalter</span>
        <span>{{ termin.minAlter }} Jahre</span>
      </div>
      <div class="kurs-row" v-if="termin.geschlecht === 'w'">
        <span class="label">Zielgruppe</span>
        <span>Nur Frauen</span>
      </div>
      <div class="kurs-row">
        <span class="label">Plätze</span>
        <span :class="{ 'text-voll': termin.istVoll, 'text-frei': !termin.istVoll }">
          {{ termin.istVoll ? 'Ausgebucht' : `${termin.teilnehmerAnzahl} / ${termin.maxTeilnehmer ?? '?'}` }}
        </span>
      </div>
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

defineEmits(['kursGeklickt'])

function formatZeit(datum) {
  return new Date(datum).toLocaleTimeString('de-DE', {
    hour: '2-digit',
    minute: '2-digit'
  })
}
</script>

<style scoped>
.kurs-karte {
  background: #ffffff08;
  border-radius: 8px;
  overflow: hidden;
  margin-bottom: 0.5rem;
  cursor: pointer;
  transition: transform 0.15s, box-shadow 0.15s;
  border: 1px solid rgba(255,255,255,0.06);
}

.kurs-karte:hover:not(.voll) {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0,0,0,0.3);
}

.kurs-karte.voll {
  opacity: 0.45;
  cursor: not-allowed;
}

.kurs-karte-top {
  padding: 0.5rem 0.75rem;
  display: flex;
  flex-direction: column;
  gap: 0.15rem;
}

.kurs-titel {
  font-weight: 700;
  font-size: 0.88rem;
  color: #fff;
}

.kurs-uhrzeit {
  font-size: 0.75rem;
  color: rgba(255,255,255,0.8);
}

.kurs-karte-body {
  padding: 0.5rem 0.75rem;
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.kurs-row {
  display: flex;
  justify-content: space-between;
  font-size: 0.78rem;
  color: #ccc;
}

.label {
  color: #888;
}

.text-voll {
  color: #e63946;
  font-weight: 600;
}

.text-frei {
  color: #a8d8a8;
}

@media (max-width: 768px) {
  .kurs-karte {
    margin-bottom: 0.75rem;
  }

  .kurs-karte-top,
  .kurs-karte-body {
    padding-left: 0.85rem;
    padding-right: 0.85rem;
  }

  .kurs-row {
    gap: 0.5rem;
  }
}

@media (max-width: 480px) {
  .kurs-row {
    flex-direction: column;
    align-items: flex-start;
  }

  .kurs-titel {
    font-size: 0.95rem;
  }

  .kurs-uhrzeit {
    font-size: 0.72rem;
  }
}
</style>