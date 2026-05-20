import {
  mockKurse,
  mockKursTermine,
  mockTrainer,
  mockNimmtTeil
} from '../data/mockKurse.js'

const USE_MOCK = true
const API_BASE_URL = 'http://localhost:8080/api'

/**
 * Gibt alle Kurstermine einer Woche zurück,
 * angereichert mit Kurs- und Trainerdaten (JOIN-Logik im Frontend)
 */
export async function getKursTermineFuerWoche(wochenstart) {
  if (USE_MOCK) {
    await new Promise(r => setTimeout(r, 200))
    return buildKalenderDaten(wochenstart)
  }

  // Echter API-Call – Backend gibt idealerweise bereits gejointen Datensatz zurück
  const von = wochenstart.toISOString()
  const response = await fetch(`${API_BASE_URL}/kurstermine?von=${von}`)
  if (!response.ok) throw new Error('Fehler beim Laden der Kurstermine')
  return response.json()
}

/**
 * Simuliert den DB-JOIN aus:
 * Kurs-Termin → Kurs, Trainer, Nimmt-Teil
 * Filtert auf die aktuelle Woche (Mo–So)
 */
function buildKalenderDaten(wochenstart) {
  const wochenende = new Date(wochenstart)
  wochenende.setDate(wochenende.getDate() + 6)
  wochenende.setHours(23, 59, 59)

  return mockKursTermine
    .filter(termin => {
      const datum = new Date(termin.anfang)
      return datum >= wochenstart && datum <= wochenende
    })
    .map(termin => {
      const kurs = mockKurse.find(k => k.id === termin.kursId)
      const trainer = mockTrainer.find(t => t.id === termin.trainerId)
      const teilnehmerAnzahl = mockNimmtTeil.filter(
        n => n.kursTerminId === termin.id
      ).length

      const anfang = new Date(termin.anfang)
      const ende = new Date(anfang.getTime() + kurs.dauer * 60000)

      return {
        terminId: termin.id,
        anfang,
        ende,
        kursId: kurs.id,
        titel: kurs.titel,
        beschreibung: kurs.beschreibung,
        dauer: kurs.dauer,
        minAlter: kurs.minAlter,
        geschlecht: kurs.geschlecht,
        trainer: `${trainer.vorname} ${trainer.name}`,
        teilnehmerAnzahl
      }
    })
}