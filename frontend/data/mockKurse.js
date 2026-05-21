// Entspricht Tabelle: Trainer
export const mockTrainer = [
  { id: 1, name: "Müller", vorname: "Anna", geschlecht: "w" },
  { id: 2, name: "Klein", vorname: "Ben", geschlecht: "m" },
  { id: 3, name: "Peters", vorname: "Chris", geschlecht: "m" },
  { id: 4, name: "Schmidt", vorname: "Dana", geschlecht: "w" }
]

// Entspricht Tabelle: Kurs
export const mockKurse = [
  {
    id: 1,
    titel: "Yoga",
    beschreibung: "Sanftes Yoga für Körper und Geist.",
    minAlter: 16,
    geschlecht: null, // null = alle willkommen
    dauer: 60 // Minuten
  },
  {
    id: 2,
    titel: "Cycling",
    beschreibung: "Intensives Fahrradtraining im Kursraum.",
    minAlter: 18,
    geschlecht: null,
    dauer: 60
  },
  {
    id: 3,
    titel: "Functional Training",
    beschreibung: "Ganzkörpertraining mit dem eigenen Körpergewicht.",
    minAlter: 16,
    geschlecht: null,
    dauer: 60
  },
  {
    id: 4,
    titel: "Rückenkurs",
    beschreibung: "Gezieltes Training zur Stärkung der Rückenmuskulatur.",
    minAlter: 14,
    geschlecht: null,
    dauer: 60
  }
]

// Entspricht Tabelle: Kurs-Termin
// "anfang" = Datum + Uhrzeit, Endzeit wird aus Kurs.dauer berechnet
export const mockKursTermine = [
  { id: 1, kursId: 2, anfang: "2026-05-18T07:00:00", trainerId: 2, maxTeilnehmer: 20 },
  { id: 2, kursId: 1, anfang: "2026-05-19T08:00:00", trainerId: 1, maxTeilnehmer: 15 },
  { id: 3, kursId: 3, anfang: "2026-05-20T18:00:00", trainerId: 3, maxTeilnehmer: 12 },
  { id: 4, kursId: 4, anfang: "2026-05-21T10:00:00", trainerId: 4, maxTeilnehmer: 10 },
  { id: 5, kursId: 1, anfang: "2026-05-22T17:00:00", trainerId: 1, maxTeilnehmer: 15 },
  { id: 6, kursId: 2, anfang: "2026-05-23T09:00:00", trainerId: 2, maxTeilnehmer: 20 },
]

// Entspricht Tabelle: Nimmt-Teil
export const mockNimmtTeil = [
  { userId: 1, kursTerminId: 1 },
  { userId: 2, kursTerminId: 1 },
  { userId: 3, kursTerminId: 2 },
  { userId: 1, kursTerminId: 3 },
]

// Entspricht Tabelle: User
export const mockUser = [
  { id: 1, name: "Bauer", vorname: "Leon", alter: 25, geschlecht: "m" },
  { id: 2, name: "Hoffmann", vorname: "Sarah", alter: 30, geschlecht: "w" },
  { id: 3, name: "Wagner", vorname: "Tim", alter: 22, geschlecht: "m" },
]