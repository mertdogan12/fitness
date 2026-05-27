<template>
  <div class="trainer-dashboard">
    <h2 class="page-title">Trainer Dashboard</h2>

    <section class="card">
      <h3>Neuen Termin erstellen</h3>
      <div class="form-row">
        <label>Kurs: 
          <select v-model.number="kursId" :disabled="loadingKurse">
            <option disabled :value="null">-- Kurs wählen --</option>
            <option v-for="k in kurse" :key="k.id" :value="k.id">{{ k.titel }}</option>
          </select>
        </label>
        <label>Trainer:
          <select v-model.number="trainerId" :disabled="loadingTrainers">
            <option disabled :value="null">-- Trainer wählen --</option>
            <option v-for="tr in trainerList" :key="tr.id" :value="tr.id">{{ tr.vorname }} {{ tr.name }}</option>
          </select>
        </label>
      </div>
      <div class="form-row">
        <label>Anfang: <input v-model="anfang" type="datetime-local" /></label>
        <label>Max. Teiln.: <input v-model.number="maxTeilnehmer" type="number" /></label>
      </div>
      <button class="nav-btn" @click="createTermin" :disabled="creating">Termin erstellen</button>
    </section>

    <section class="card" style="margin-top:1em;">
      <h3>Termin löschen</h3>
      <div class="form-row">
        <label>Termin ID: <input v-model.number="deleteId" type="number" /></label>
        <button class="nav-btn ghost" @click="deleteTermin" :disabled="deleting">Löschen</button>
      </div>
    </section>

    <section style="margin-top:1em;">
      <h3>Vorhandene Termine</h3>
      <div class="filter-row">
        <label>Von: <input v-model="von" type="date" /></label>
        <button class="nav-btn" @click="loadTermine" :disabled="loading">Laden</button>
      </div>

      <div v-if="loading" class="loading">Lade Termine…</div>
      <ul v-else class="termin-list">
        <li v-for="t in termine" :key="t.terminId" class="termin-item card-small">
          <div class="termin-main">
            <div class="termin-title">#{{ t.terminId }} — {{ t.titel }}</div>
            <div class="termin-meta">{{ formatLocal(t.anfang) }} · {{ t.teilnehmerAnzahl }} / {{ t.maxTeilnehmer ?? '–' }}</div>
          </div>
          <div class="termin-actions">
            <button class="nav-btn ghost" @click="deleteTerminById(t.terminId)" :disabled="deletingIds.has(t.terminId)">
              {{ deletingIds.has(t.terminId) ? 'Lösche…' : 'Löschen' }}
            </button>
          </div>
        </li>
      </ul>
    </section>

    <div v-if="message" class="message-box" :class="success ? 'success' : 'error'">
      <span class="message-icon" v-if="success" aria-hidden="true">✓</span>
      <span class="message-icon" v-else aria-hidden="true">!</span>
      <div class="message-text">{{ message }}</div>
      <button class="message-close" @click="clearMessage" aria-label="Schließen">×</button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { createKurstermin, deleteKurstermin, getKursTermineFuerWoche, getKurse, getTrainerList } from '../../services/kursService.js'

const kursId = ref(null)
const trainerId = ref(null)
const anfang = ref('')
const maxTeilnehmer = ref(0)
const deleteId = ref(null)
const creating = ref(false)
const deleting = ref(false)
const message = ref('')
const success = ref(true)
const von = ref(new Date().toISOString().split('T')[0])
const termine = ref([])
const loading = ref(false)
const deletingIds = ref(new Set())
const kurse = ref([])
const loadingKurse = ref(false)
const trainerList = ref([])
const loadingTrainers = ref(false)

function isoFromLocalDatetime(local) {
  if (!local) return null
  const date = new Date(local)
  return date.toISOString()
}

async function createTermin() {
  message.value = ''
  creating.value = true
  try {
    if (!kursId.value) throw new Error('Bitte einen Kurs auswählen')
    if (!trainerId.value) throw new Error('Bitte einen Trainer auswählen')
    const payload = {
      kursId: Number(kursId.value),
      trainerID: Number(trainerId.value),
      anfang: isoFromLocalDatetime(anfang.value),
      maxTeilnehmer: Number(maxTeilnehmer.value)
    }
    await createKurstermin(payload)
    // reload list after creating
    await loadTermine()
    success.value = true
    message.value = 'Termin erfolgreich erstellt.'
  } catch (err) {
    success.value = false
    message.value = err?.message || 'Fehler beim Erstellen.'
  } finally {
    creating.value = false
  }
}

async function deleteTermin() {
  if (deleteId.value === null || deleteId.value === undefined) {
    success.value = false
    message.value = 'Bitte Termin ID angeben.'
    return
  }
  message.value = ''
  deleting.value = true
  try {
    await deleteKurstermin(deleteId.value)
    // reload list
    await loadTermine()
    success.value = true
    message.value = 'Termin erfolgreich gelöscht.'
  } catch (err) {
    success.value = false
    message.value = err?.message || 'Fehler beim Löschen.'
  } finally {
    deleting.value = false
  }
}

function formatLocal(dt) {
  if (!dt) return ''
  const d = new Date(dt)
  return d.toLocaleString()
}

async function loadTermine() {
  loading.value = true
  message.value = ''
  try {
    const start = new Date(von.value)
    const daten = await getKursTermineFuerWoche(start)
    termine.value = daten
  } catch (err) {
    message.value = err?.message || 'Fehler beim Laden der Termine.'
    success.value = false
  } finally {
    loading.value = false
  }
}

async function deleteTerminById(id) {
  deletingIds.value = new Set([...deletingIds.value, id])
  try {
    await deleteKurstermin(id)
    message.value = 'Termin erfolgreich gelöscht.'
    success.value = true
    await loadTermine()
  } catch (err) {
    message.value = err?.message || 'Fehler beim Löschen.'
    success.value = false
  } finally {
    deletingIds.value = new Set([...deletingIds.value].filter(x => x !== id))
  }
}

async function loadKurse() {
  loadingKurse.value = true
  try {
    kurse.value = await getKurse()
    // set default selected kurs if none
    if (!kursId.value && kurse.value.length) kursId.value = kurse.value[0].id
  } catch (err) {
    message.value = err?.message || 'Fehler beim Laden der Kurse.'
    success.value = false
  } finally {
    loadingKurse.value = false
  }
}

async function loadTrainers() {
  loadingTrainers.value = true
  try {
    trainerList.value = await getTrainerList()
    if (!trainerId.value && trainerList.value.length) trainerId.value = trainerList.value[0].id
  } catch (err) {
    message.value = err?.message || 'Fehler beim Laden der Trainer.'
    success.value = false
  } finally {
    loadingTrainers.value = false
  }
}

onMounted(() => {
  loadKurse()
  loadTermine()
  loadTrainers()
})

function clearMessage() {
  message.value = ''
}
</script>

<style scoped>
.trainer-dashboard { padding: 1rem }
.page-title { font-size:1.25rem; color: #e0e0e0; margin-bottom:0.75rem }
.card { background: #11121a; border-radius: 12px; padding: 1rem; margin-bottom: 0.75rem; border: 1px solid rgba(255,255,255,0.03) }
.card-small { background: #0f1116; border-radius: 8px; padding: 0.6rem; display:flex; align-items:center; justify-content:space-between; margin-bottom:0.5rem; border: 1px solid rgba(255,255,255,0.03) }
.form-row { display:flex; gap:1rem; margin-bottom:0.5rem; align-items:center }
.filter-row { display:flex; gap:1rem; align-items:center; margin-bottom:0.5rem }
.termin-list { list-style:none; padding:0; margin:0; }
.termin-item { display:flex; align-items:center; justify-content:space-between }
.termin-main { display:flex; flex-direction:column }
.termin-title { font-weight:600; color:#fff }
.termin-meta { color:#bbb; font-size:0.9rem }
.termin-actions { margin-left:1rem }
.loading { color:#bbb }
.message { margin-top:1rem }

/* improved message box */
.message-box { margin-top:1rem; display:flex; align-items:center; gap:0.75rem; padding:0.6rem 0.75rem; border-radius:8px; background: rgba(255,255,255,0.02); border: 1px solid rgba(255,255,255,0.03) }
.message-box.success { border-left: 4px solid #8fd19a; color: #dff7e6 }
.message-box.error { border-left: 4px solid #e63946; color: #ffdede }
.message-icon { font-weight:700; font-size:1.05rem; width:1.2rem; display:inline-flex; align-items:center; justify-content:center }
.message-text { flex:1 }
.message-close { background:transparent; border:none; color:inherit; font-size:1.05rem; cursor:pointer; padding:0.15rem 0.4rem; border-radius:6px }
.message-close:hover { background: rgba(255,255,255,0.02) }

.success { color: #a8d8a8 }
.error { color: #e63946 }

/* button styles align with WochenNavigator */
.nav-btn { background: #e63946; color: white; border: none; padding: 0.45rem 0.9rem; border-radius: 8px; cursor: pointer; font-size: 0.95rem; font-weight: 600; transition: background 0.15s }
.nav-btn:hover:not(.disabled) { background: #c1121f }
.nav-btn.disabled { background:#555; cursor:not-allowed; opacity:0.6 }
.nav-btn.ghost { background: transparent; border: 1px solid rgba(255,255,255,0.06); color:#fff; padding:0.35rem 0.75rem }
.nav-btn.ghost:hover:not(.disabled) { background: rgba(255,255,255,0.02) }

input[type="number"], input[type="date"], input[type="datetime-local"] { background: transparent; border: 1px solid rgba(255,255,255,0.06); padding: 0.35rem 0.5rem; border-radius: 6px; color: #e0e0e0 }
label { color:#ddd; font-size:0.95rem }

@media (max-width: 768px) {
  .trainer-dashboard {
    padding: 0.75rem;
  }

  .page-title {
    font-size: 1.1rem;
    margin-bottom: 0.6rem;
  }

  .card,
  .card-small {
    padding: 0.85rem;
  }

  .form-row,
  .filter-row {
    flex-direction: column;
    align-items: stretch;
    gap: 0.65rem;
  }

  .form-row label,
  .filter-row label {
    width: 100%;
    display: flex;
    flex-direction: column;
    gap: 0.35rem;
  }

  .card-small,
  .termin-item {
    flex-direction: column;
    align-items: stretch;
    gap: 0.75rem;
  }

  .termin-actions {
    margin-left: 0;
  }

  .nav-btn {
    width: 100%;
  }

  input[type="number"],
  input[type="date"],
  input[type="datetime-local"],
  select {
    width: 100%;
  }
}

@media (max-width: 480px) {
  .trainer-dashboard {
    padding: 0.5rem;
  }

  .card,
  .card-small {
    border-radius: 10px;
  }

  .termin-title {
    font-size: 0.95rem;
  }

  .termin-meta {
    font-size: 0.82rem;
  }
}
</style>