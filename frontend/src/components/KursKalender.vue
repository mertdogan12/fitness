<template>
  <div class="kalender-wrapper">
    <WochenNavigator
      :wochenstart="wochenstart"
      :wochenende="wochenende"
      :istMaxWoche="istMaxWoche"
      @vorherige="wocheWechseln(-1)"
      @naechste="wocheWechseln(1)"
    />

    <div v-if="laden" class="laden">Kurse werden geladen...</div>
    <div v-if="apiFehler" class="api-fehler">
      ⚠️ {{ apiFehler }}
    </div>
    <div v-else class="kalender-grid">
      <div
        v-for="tag in wochentage"
        :key="tag.datum"
        class="tag-spalte"
        :class="{ heute: istHeute(tag.datum) }"
      >
        <!-- Spalten-Header -->
        <div class="tag-header">
          <span class="tag-name">{{ tag.name }}</span>
          <span class="tag-datum">{{ formatTagDatum(tag.datum) }}</span>
        </div>

        <!-- Kurskarten -->
        <div class="tag-kurse">
          <KursKarte
            v-for="termin in termineProTag(tag.datum)"
            :key="termin.terminId"
            :termin="termin"
            :farbe="kursfarbe(termin.kursId)"
            @kursGeklickt="modalOeffnen"
          />
          <p v-if="termineProTag(tag.datum).length === 0" class="keine-kurse">
            Keine Kurse
          </p>
          <KursModal
            v-if="ausgewaehlterTermin"
            :termin="ausgewaehlterTermin"
            :farbe="kursfarbe(ausgewaehlterTermin.kursId)"
            @schliessen="modalSchliessen"
            />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch, onMounted } from 'vue'
import WochenNavigator from './WochenNavigator.vue'
import KursKarte from './KursKarte.vue'
import { getKursTermineFuerWoche } from '../../services/kursService.js'
import KursModal from './KursModal.vue'

// Farbpalette je Kurs-ID
const KURS_FARBEN = {
  1: '#a8d8a8', // Yoga → grün
  2: '#f9c784', // Cycling → orange
  3: '#a0c4ff', // Functional → blau
  4: '#f4a261', // Rücken → lachs
}

function kursfarbe(kursId) {
  return KURS_FARBEN[kursId] ?? '#e63946'
}

// Aktuellen Montag berechnen
function getMontag(datum) {
  const d = new Date(datum)
  const tag = d.getDay()
  const diff = tag === 0 ? -6 : 1 - tag // Montag = 0
  d.setDate(d.getDate() + diff)
  d.setHours(0, 0, 0, 0)
  return d
}

const heute = new Date()
const wochenOffsetStart = getMontag(heute)

const wochenOffset = ref(0)

const wochenstart = computed(() => {
  const d = new Date(wochenOffsetStart)
  d.setDate(d.getDate() + wochenOffset.value * 7)
  return d
})

const wochenende = computed(() => {
  const d = new Date(wochenstart.value)
  d.setDate(d.getDate() + 6)
  d.setHours(23, 59, 59)
  return d
})

// Nur aktuelle + nächste Woche erlaubt (1 Woche voraus)
const istMaxWoche = computed(() => wochenOffset.value >= 1)

function wocheWechseln(richtung) {
  const neuerOffset = wochenOffset.value + richtung
  if (neuerOffset < 0 || neuerOffset > 1) return
  wochenOffset.value = neuerOffset
}

// Wochentage Mo–So generieren
const TAGNAMEN = ['Mo', 'Di', 'Mi', 'Do', 'Fr', 'Sa', 'So']

const wochentage = computed(() => {
  return TAGNAMEN.map((name, i) => {
    const datum = new Date(wochenstart.value)
    datum.setDate(datum.getDate() + i)
    return { name, datum }
  })
})

// Kurstermine laden
const termine = ref([])
const laden = ref(false)

const ausgewaehlterTermin = ref(null)

async function ladeTermine() {
  laden.value = true
  try {
    termine.value = await getKursTermineFuerWoche(wochenstart.value)
  } catch (e) {
    console.error('Fehler beim Laden:', e)
  } finally {
    laden.value = false
  }
}

function termineProTag(datum) {
  return termine.value
    .filter(t => {
      const d = new Date(t.anfang)
      return (
        d.getFullYear() === datum.getFullYear() &&
        d.getMonth() === datum.getMonth() &&
        d.getDate() === datum.getDate()
      )
    })
    .sort((a, b) => new Date(a.anfang) - new Date(b.anfang))
}

function istHeute(datum) {
  return (
    datum.getDate() === heute.getDate() &&
    datum.getMonth() === heute.getMonth() &&
    datum.getFullYear() === heute.getFullYear()
  )
}

function formatTagDatum(datum) {
  return datum.toLocaleDateString('de-DE', { day: '2-digit', month: '2-digit' })
}

function modalOeffnen(termin) {
  ausgewaehlterTermin.value = termin
}

function modalSchliessen() {
  ausgewaehlterTermin.value = null
}

// Script: apiFehler ref hinzufügen
const apiFehler = ref('')

async function ladeTermine() {
  laden.value = true
  apiFehler.value = ''
  try {
    termine.value = await getKursTermineFuerWoche(wochenstart.value)
  } catch (e) {
    console.error('Fehler beim Laden:', e)
    apiFehler.value = 'Kurse konnten nicht geladen werden. Bitte später erneut versuchen.'
  } finally {
    laden.value = false
  }
}

watch(wochenstart, ladeTermine)
onMounted(ladeTermine)
</script>

<style scoped>
.kalender-wrapper {
  padding: 1.5rem;
}

.laden {
  text-align: center;
  color: #aaa;
  padding: 3rem;
  font-size: 1rem;
}

.kalender-grid {
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  gap: 0.75rem;
}

.tag-spalte {
  background: #0f3460;
  border-radius: 12px;
  padding: 0.75rem;
  min-height: 300px;
}

.tag-spalte.heute {
  background: #1a1a4e;
  box-shadow: 0 0 0 2px #e63946;
}

.tag-header {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-bottom: 0.75rem;
  padding-bottom: 0.5rem;
  border-bottom: 1px solid rgba(255,255,255,0.1);
}

.tag-name {
  font-weight: 700;
  font-size: 1rem;
  color: #e63946;
}

.tag-datum {
  font-size: 0.75rem;
  color: #aaa;
}

.tag-kurse {
  display: flex;
  flex-direction: column;
}

.keine-kurse {
  color: #555;
  font-size: 0.78rem;
  text-align: center;
  margin-top: 1rem;
}

.api-fehler {
  text-align: center;
  color: #f4a261;
  background: #4d2e1e;
  border-radius: 10px;
  padding: 1.5rem;
  margin-top: 1rem;
}
</style>