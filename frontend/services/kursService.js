const API_BASE_URL = import.meta.env.VITE_API_URL

export async function getKursTermineFuerWoche(wochenstart) {
  const von = wochenstart.toISOString().split('T')[0]

  const wochenende = new Date(wochenstart)
  wochenende.setDate(wochenende.getDate() + 6)
  const bis = wochenende.toISOString().split('T')[0]

  const response = await fetch(`${API_BASE_URL}/KursTermin?von=${von}&bis=${bis}`)

  if (!response.ok) throw new Error('Fehler beim Laden der Kurstermine')

  const daten = await response.json()
  return daten.map(termin => {
    const anfang = new Date(termin.anfang)
    const ende = new Date(anfang.getTime() + (termin.kurs.dauer || 60) * 60000)

    return {
      terminId: termin.id,
      anfang,
      ende,
      kursId: termin.kursId,
      titel: termin.kurs.titel,
      beschreibung: termin.kurs.beschreibung,
      dauer: termin.kurs.dauer,
      minAlter: termin.kurs.minAlter,
      geschlecht: termin.kurs.geschlecht,
      trainer: `Trainer ${termin.trainerID}`,
      teilnehmerAnzahl: termin.teilnehmerAnzahl ?? 0,
      maxTeilnehmer: termin.maxTeilnehmer,
      istVoll: termin.maxTeilnehmer
        ? (termin.teilnehmerAnzahl ?? 0) >= termin.maxTeilnehmer
        : false
    }
  })
}

export async function anmeldenZuKurs(daten) {
  const buchungResponse = await fetch(`${API_BASE_URL}/Buchungen/buchen`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      vorname: daten.vorname,
      name: daten.name,
      alter: daten.alter,
      geschlecht: daten.geschlecht,
      kursTerminId: daten.kursTerminId
    })
  })

  if (!buchungResponse.ok) {
    const err = await buchungResponse.json().catch(() => ({}))
    throw new Error(err.message || 'Buchung fehlgeschlagen.')
  }

  return buchungResponse.json()
}