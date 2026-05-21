const API_BASE_URL = import.meta.env.VITE_API_URL

/**
 * Gibt alle Kurstermine einer Woche zurück
 * GET /api/kurstermine?von=2026-05-18
 */
export async function getKursTermineFuerWoche(wochenstart) {
  const von = wochenstart.toISOString().split('T')[0] // nur Datum z.B. "2026-05-18"

  const response = await fetch(`${API_BASE_URL}/kurstermine?von=${von}`)

  if (!response.ok) throw new Error('Fehler beim Laden der Kurstermine')

  const daten = await response.json()

  // Datum-Strings in echte Date-Objekte umwandeln
  return daten.map(termin => ({
    ...termin,
    anfang: new Date(termin.anfang),
    ende: new Date(termin.ende)
  }))
}

/**
 * Meldet einen User zu einem Kurstermin an
 * POST /api/anmeldung
 * Body: { vorname, name, alter, geschlecht, kursTerminId }
 */
export async function anmeldenZuKurs(daten) {
  const response = await fetch(`${API_BASE_URL}/anmeldung`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(daten)
  })

  if (!response.ok) {
    const err = await response.json().catch(() => ({}))
    throw new Error(err.message || 'Anmeldung fehlgeschlagen.')
  }

  return response.json()
}