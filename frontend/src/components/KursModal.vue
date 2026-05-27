<template>
  <div class="modal-overlay" @click.self="$emit('schliessen')">
    <div class="modal">

      <!-- Header -->
      <div class="modal-header" :style="{ borderBottom: `3px solid ${farbe}` }">
        <h2>{{ termin.titel }}</h2>
        <button class="close-btn" @click="$emit('schliessen')">✕</button>
      </div>

      <!-- Kursinfos -->
      <div class="modal-body">
        <div class="kurs-details">
          <p class="beschreibung">{{ termin.beschreibung }}</p>

          <div class="detail-grid">
            <div class="detail-item">
              <span class="detail-label">🕐 Uhrzeit</span>
              <span>{{ formatZeit(termin.anfang) }} – {{ formatZeit(termin.ende) }}</span>
            </div>
            <div class="detail-item">
              <span class="detail-label">⏱ Dauer</span>
              <span>{{ termin.dauer }} Minuten</span>
            </div>
            <div class="detail-item">
              <span class="detail-label">👤 Trainer</span>
              <span>{{ termin.trainer }}</span>
            </div>
            <div class="detail-item">
              <span class="detail-label">👥 Teilnehmer</span>
              <span>{{ termin.teilnehmerAnzahl }} / {{ termin.maxTeilnehmer }}</span>
            </div>
            <div class="detail-item" v-if="termin.minAlter">
              <span class="detail-label">🔞 Mindestalter</span>
              <span>{{ termin.minAlter }} Jahre</span>
            </div>
            <div class="detail-item" v-if="termin.geschlecht === 'w'">
              <span class="detail-label">⚧ Zielgruppe</span>
              <span>Nur Frauen</span>
            </div>
          </div>
        </div>

        <!-- Trennlinie -->
        <hr class="divider" />

        <!-- Anmeldeformular -->
        <div class="anmeldung">
          <h3>Zum Kurs anmelden</h3>

          <div v-if="termin.istVoll" class="fehler">
            ❌ Dieser Kurs ist leider ausgebucht.
          </div>

          <div v-if="anmeldungErfolgreich" class="erfolg">
            ✅ Anmeldung erfolgreich! Wir freuen uns auf dich.
          </div>

          <div v-if="fehler" class="fehler">
            ⚠️ {{ fehler }}
          </div>

          <div v-if="!anmeldungErfolgreich && !termin.istVoll">
            <div class="form-gruppe">
              <label>Vorname *</label>
              <input
                v-model="formular.vorname"
                type="text"
                placeholder="z.B. Leon"
                :class="{ invalid: validierung.vorname }"
              />
              <span class="fehler-text" v-if="validierung.vorname">
                {{ validierung.vorname }}
              </span>
            </div>

            <div class="form-gruppe">
              <label>Nachname *</label>
              <input
                v-model="formular.name"
                type="text"
                placeholder="z.B. Bauer"
                :class="{ invalid: validierung.name }"
              />
              <span class="fehler-text" v-if="validierung.name">
                {{ validierung.name }}
              </span>
            </div>

            <div class="form-gruppe">
              <label>Alter *</label>
              <input
                v-model.number="formular.alter"
                type="number"
                placeholder="z.B. 25"
                min="1"
                max="120"
                :class="{ invalid: validierung.alter }"
              />
              <span class="fehler-text" v-if="validierung.alter">
                {{ validierung.alter }}
              </span>
            </div>

            <div class="form-gruppe">
              <label>Geschlecht *</label>
              <select
                v-model="formular.geschlecht"
                :class="{ invalid: validierung.geschlecht }"
              >
                <option value="">– bitte wählen –</option>
                <option value="m">Männlich</option>
                <option value="w">Weiblich</option>
                <option value="d">Divers</option>
              </select>
              <span class="fehler-text" v-if="validierung.geschlecht">
                {{ validierung.geschlecht }}
              </span>
            </div>

            <button class="anmelden-btn" @click="anmelden" :disabled="laden">
              {{ laden ? 'Wird angemeldet...' : 'Jetzt anmelden' }}
            </button>
          </div>
        </div>

        <!-- Trennlinie -->
        <hr class="divider" />

        <!-- Abmeldeformular -->
        <div class="abmeldung">
          <h3>Vom Kurs abmelden</h3>

          <div v-if="abmeldungErfolgreich" class="erfolg">
            ✅ Abmeldung erfolgreich!
          </div>

          <div v-if="abmeldeFehler" class="fehler">
            ⚠️ {{ abmeldeFehler }}
          </div>

          <div v-if="!abmeldungErfolgreich">
            <div class="form-gruppe">
              <label>Vorname *</label>
              <input
                v-model="abmeldeFormular.vorname"
                type="text"
                placeholder="z.B. Leon"
                :class="{ invalid: abmeldeValidierung.vorname }"
              />
              <span class="fehler-text" v-if="abmeldeValidierung.vorname">
                {{ abmeldeValidierung.vorname }}
              </span>
            </div>

            <div class="form-gruppe">
              <label>Nachname *</label>
              <input
                v-model="abmeldeFormular.name"
                type="text"
                placeholder="z.B. Bauer"
                :class="{ invalid: abmeldeValidierung.name }"
              />
              <span class="fehler-text" v-if="abmeldeValidierung.name">
                {{ abmeldeValidierung.name }}
              </span>
            </div>

            <button class="abmelden-btn" @click="abmelden" :disabled="abmeldenLaden">
              {{ abmeldenLaden ? 'Wird abgemeldet...' : 'Vom Kurs abmelden' }}
            </button>
          </div>
        </div>

      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'
import { anmeldenZuKurs, abmeldenVonKurs } from '../../services/kursService.js'

const props = defineProps({
  termin: Object,
  farbe: {
    type: String,
    default: '#e63946'
  }
})

const emit =defineEmits(['schliessen', 'aktualisieren'])

// ── Anmeldung ──────────────────────────────────────
const formular = reactive({
  vorname: '',
  name: '',
  alter: '',
  geschlecht: ''
})

const validierung = reactive({
  vorname: '',
  name: '',
  alter: '',
  geschlecht: ''
})

const laden = ref(false)
const fehler = ref('')
const anmeldungErfolgreich = ref(false)

function formularValidieren() {
  validierung.vorname = ''
  validierung.name = ''
  validierung.alter = ''
  validierung.geschlecht = ''

  let gueltig = true

  if (!formular.vorname.trim()) {
    validierung.vorname = 'Vorname ist erforderlich.'
    gueltig = false
  }
  if (!formular.name.trim()) {
    validierung.name = 'Nachname ist erforderlich.'
    gueltig = false
  }
  if (!formular.alter || formular.alter < 1 || formular.alter > 120) {
    validierung.alter = 'Bitte ein gültiges Alter eingeben.'
    gueltig = false
  }
  if (props.termin.minAlter && formular.alter < props.termin.minAlter) {
    validierung.alter = `Mindestalter für diesen Kurs: ${props.termin.minAlter} Jahre.`
    gueltig = false
  }
  if (!formular.geschlecht) {
    validierung.geschlecht = 'Bitte Geschlecht auswählen.'
    gueltig = false
  }
  if (props.termin.geschlecht === 'w' && formular.geschlecht !== 'w') {
    validierung.geschlecht = 'Dieser Kurs ist nur für Frauen.'
    gueltig = false
  }

  return gueltig
}

async function anmelden() {
  fehler.value = ''
  if (!formularValidieren()) return

  laden.value = true
  try {
    await anmeldenZuKurs({
      vorname: formular.vorname,
      name: formular.name,
      alter: formular.alter,
      geschlecht: formular.geschlecht,
      kursTerminId: props.termin.terminId
    })
    anmeldungErfolgreich.value = true
    emit('aktualisieren')
  } catch (e) {
    fehler.value = e.message || 'Ein Fehler ist aufgetreten.'
  } finally {
    laden.value = false
  }
}

// ── Abmeldung ──────────────────────────────────────
const abmeldeFormular = reactive({
  vorname: '',
  name: ''
})

const abmeldeValidierung = reactive({
  vorname: '',
  name: ''
})

const abmeldenLaden = ref(false)
const abmeldungErfolgreich = ref(false)
const abmeldeFehler = ref('')

function abmeldeFormularValidieren() {
  abmeldeValidierung.vorname = ''
  abmeldeValidierung.name = ''
  let gueltig = true

  if (!abmeldeFormular.vorname.trim()) {
    abmeldeValidierung.vorname = 'Vorname ist erforderlich.'
    gueltig = false
  }
  if (!abmeldeFormular.name.trim()) {
    abmeldeValidierung.name = 'Nachname ist erforderlich.'
    gueltig = false
  }
  return gueltig
}

async function abmelden() {
  abmeldeFehler.value = ''
  if (!abmeldeFormularValidieren()) return

  abmeldenLaden.value = true
  try {
    await abmeldenVonKurs({
      vorname: abmeldeFormular.vorname,
      name: abmeldeFormular.name,
      kursTerminId: props.termin.terminId
    })
    abmeldungErfolgreich.value = true
    emit('aktualisieren')
  } catch (e) {
    abmeldeFehler.value = e.message || 'Abmeldung fehlgeschlagen.'
  } finally {
    abmeldenLaden.value = false
  }
}

function formatZeit(datum) {
  return new Date(datum).toLocaleTimeString('de-DE', {
    hour: '2-digit',
    minute: '2-digit'
  })
}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.7);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
}

.modal {
  background: #1a1a2e;
  border-radius: 16px;
  width: 90%;
  max-width: 540px;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 8px 40px rgba(0, 0, 0, 0.6);
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.2rem 1.5rem;
}

.modal-header h2 {
  font-size: 1.3rem;
  color: #fff;
}

.close-btn {
  background: none;
  border: none;
  color: #aaa;
  font-size: 1.2rem;
  cursor: pointer;
  transition: color 0.2s;
}

.close-btn:hover {
  color: #e63946;
}

.modal-body {
  padding: 1.2rem 1.5rem 1.5rem;
}

.beschreibung {
  color: #ccc;
  margin-bottom: 1rem;
  line-height: 1.6;
}

.detail-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 0.6rem;
  margin-bottom: 1rem;
}

.detail-item {
  background: #0f3460;
  border-radius: 8px;
  padding: 0.5rem 0.75rem;
  display: flex;
  flex-direction: column;
  gap: 0.2rem;
}

.detail-label {
  font-size: 0.72rem;
  color: #888;
}

.detail-item span:last-child {
  font-size: 0.88rem;
  color: #e0e0e0;
}

.divider {
  border: none;
  border-top: 1px solid rgba(255,255,255,0.1);
  margin: 1.2rem 0;
}

.anmeldung h3,
.abmeldung h3 {
  font-size: 1rem;
  color: #fff;
  margin-bottom: 1rem;
}

.form-gruppe {
  display: flex;
  flex-direction: column;
  margin-bottom: 0.85rem;
}

.form-gruppe label {
  font-size: 0.8rem;
  color: #aaa;
  margin-bottom: 0.3rem;
}

.form-gruppe input,
.form-gruppe select {
  background: #0f3460;
  border: 1px solid #2a2a5a;
  border-radius: 8px;
  color: #e0e0e0;
  padding: 0.5rem 0.75rem;
  font-size: 0.9rem;
  outline: none;
  transition: border 0.2s;
}

.form-gruppe input:focus,
.form-gruppe select:focus {
  border-color: #e63946;
}

.form-gruppe input.invalid,
.form-gruppe select.invalid {
  border-color: #e63946;
}

.fehler-text {
  color: #e63946;
  font-size: 0.75rem;
  margin-top: 0.25rem;
}

.anmelden-btn {
  width: 100%;
  background: #e63946;
  color: white;
  border: none;
  padding: 0.75rem;
  border-radius: 10px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  margin-top: 0.5rem;
  transition: background 0.2s;
}

.anmelden-btn:hover:not(:disabled) {
  background: #c1121f;
}

.anmelden-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.abmelden-btn {
  width: 100%;
  background: #444;
  color: white;
  border: none;
  padding: 0.75rem;
  border-radius: 10px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  margin-top: 0.5rem;
  transition: background 0.2s;
}

.abmelden-btn:hover:not(:disabled) {
  background: #666;
}

.abmelden-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.erfolg {
  background: #1e4d2b;
  color: #a8d8a8;
  border-radius: 10px;
  padding: 1rem;
  text-align: center;
  font-weight: 600;
  margin-bottom: 1rem;
}

.fehler {
  background: #4d1e1e;
  color: #f4a261;
  border-radius: 10px;
  padding: 0.75rem;
  margin-bottom: 1rem;
  font-size: 0.85rem;
}
</style>