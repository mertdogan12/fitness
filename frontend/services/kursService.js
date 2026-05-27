const API_BASE_URL = import.meta.env.VITE_API_URL

export async function getKursTermineFuerWoche(wochenstart) {
  const von = wochenstart.toLocaleDateString('en-CA')

  const wochenende = new Date(wochenstart)
  wochenende.setDate(wochenende.getDate() + 7)
  const bis = wochenende.toISOString().split('T')[0]

  console.log('von:', von, 'bis:', bis)

  const response = await fetch(`${API_BASE_URL}/KursTermin?von=${von}`)

  if (!response.ok) throw new Error('Fehler beim Laden der Kurstermine')

  const daten = await response.json()


// Alle Trainer parallel laden
const termineArray = await Promise.all(daten.map(async termin => {
  const anfang = new Date(termin.anfang)
  const ende = new Date(anfang.getTime() + (termin.kurs.dauer || 60) * 60000)

  // Trainer laden
  let trainerName = `Trainer ${termin.trainerID}`
  try {
    const trainer = await fetch(`${API_BASE_URL}/Trainer/${termin.trainerID}`)
    if (trainer.ok) {
      const trainerDaten = await trainer.json()
      trainerName = `${trainerDaten.vorname} ${trainerDaten.name}`
    }
  } catch (e) {
    console.warn('Trainer konnte nicht geladen werden:', e)
  }

  return {
    terminId: termin.id,
    anfang,
    ende,
    kursId: termin.kursId,
    titel: termin.kurs.titel,
    beschreibung: termin.kurs.beschreibung,
    dauer: termin.kurs.dauer,
    minAlter: termin.kurs.minAlter,
    geschlecht: ['w'].includes(termin.kurs.geschlecht)
      ? termin.kurs.geschlecht
      : null,
    trainer: trainerName,
    teilnehmerAnzahl: termin.teilnehmerAnzahl ?? 0,
    maxTeilnehmer: termin.maxTeilnehmer ?? null,
    istVoll: termin.maxTeilnehmer
      ? (termin.teilnehmerAnzahl ?? 0) >= termin.maxTeilnehmer
      : false
    }
  

}))

return termineArray
}
export async function anmeldenZuKurs(daten) {
  const response = await fetch(`${API_BASE_URL}/buchungen`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      user: {
        vorname: daten.vorname,
        name: daten.name,
        alter: daten.alter,
        geschlecht: daten.geschlecht
      },
      kursTerminId: daten.kursTerminId
    })
  })

  if (!response.ok) {
    const err = await response.json().catch(() => ({}))
    throw new Error(err.message || 'Anmeldung fehlgeschlagen.')
  }

  const text = await response.text()
  return text ? JSON.parse(text) : { success: true }
}

export async function abmeldenVonKurs(daten) {
  const response = await fetch(`${API_BASE_URL}/Buchungen`, {
    method: 'DELETE',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      vorname: daten.vorname,
      name: daten.name,
      TerminId: daten.kursTerminId
    })
  })

  if (!response.ok) {
    const err = await response.json().catch(() => ({}))
    throw new Error(err.message || 'Abmeldung fehlgeschlagen.')
  }

  const text = await response.text()
  return text ? JSON.parse(text) : { success: true }
}

export async function erstelleKursTermin(daten) {
  const response = await fetch(`${API_BASE_URL}/KursTermine`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      anfang: daten.anfang,
      kursId: daten.kursId,
      maxTeilnehmer: daten.maxTeilnehmer
    })
  })
}
export async function loescheKursTermin(terminId) {
  const response = await fetch(`${API_BASE_URL}/KursTermine/${terminId}`, {
    method: 'DELETE'
  })
}
export async function aktualisiereKursTermin(terminId, daten) {
  const response = await fetch(`${API_BASE_URL}/KursTermine/${terminId}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      anfang: daten.anfang,
      kursId: daten.kursId,
      maxTeilnehmer: daten.maxTeilnehmer
    })
  })
}

// New API helpers matching provided endpoints
export async function createKurstermin(daten) {
  const response = await fetch(`${API_BASE_URL}/KursTermin`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      kursId: daten.kursId,
      trainerID: daten.trainerID ?? 0,
      anfang: daten.anfang,
      maxTeilnehmer: daten.maxTeilnehmer
    })
  })

  if (!response.ok) {
    const errText = await response.text().catch(() => null)
    throw new Error(errText || 'Fehler beim Erstellen des Kurstermins')
  }

  const text = await response.text()
  return text ? JSON.parse(text) : null
}

export async function deleteKurstermin(id) {
  const response = await fetch(`${API_BASE_URL}/KursTermin?id=${encodeURIComponent(id)}`, {
    method: 'DELETE'
  })

  if (!response.ok) {
    const errText = await response.text().catch(() => null)
    throw new Error(errText || 'Fehler beim Löschen des Kurstermins')
  }

  return true
}

export async function getKurse() {
  const response = await fetch(`${API_BASE_URL}/Kurs`)
  if (!response.ok) throw new Error('Fehler beim Laden der Kurse')
  return response.json()
}

export async function getTrainerList() {
  const response = await fetch(`${API_BASE_URL}/Trainer`)
  if (!response.ok) throw new Error('Fehler beim Laden der Trainer')
  return response.json()
}
