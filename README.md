# Hospital Project - Documentazione Tecnica

## Introduzione
Hospital Project è un'applicazione web basata su **ASP.NET Core MVC** per la gestione di un sistema ospedaliero.  
Il sistema supporta operazioni amministrative e cliniche come gestione pazienti, appuntamenti e fatturazione.

## Obiettivi
- Gestione dati di pazienti, medici, infermieri e reparti
- Pianificazione e tracciamento appuntamenti
- Gestione delle fatturazioni e dei pagamenti
- Gestione cartelle cliniche elettroniche
- Sistema di autenticazione e ruoli

## Architettura del sistema
- **Controller**: Appointment, Bill, Doctor, Nurse, Patient, Record, User, Home
- **Models**: entità e ViewModel
- **Helpers**: gestione token, ruoli, dipartimenti, specialità
- **Configurazione**: `Program.cs`, `appsettings.json`

## Database
Il database è basato su **SQL Server**.  
La cartella `Database/` contiene `Hospital.zip` con schema e dati iniziali.

## Tecnologie utilizzate
- C# .NET
- ASP.NET Core MVC
- Visual Studio
- Microsoft SQL Server
- Git

## Funzionalità principali
- Registrazione e login con ruoli
- CRUD pazienti, medici, infermieri
- Gestione appuntamenti
- Gestione fatturazione
- Cartelle cliniche e record medici

## Installazione ed esecuzione
1. Estrarre i file del progetto e aprire `Hospital.sln` in Visual Studio
2. Configurare SQL Server con il dump in `Database/Hospital.zip`
3. Aggiornare `appsettings.json` con le credenziali DB
4. Compilare ed eseguire il progetto da Visual Studio o CLI .NET
5. Accedere via browser a `https://localhost:5001`

## Conclusioni
Hospital Project fornisce una base per un sistema gestionale ospedaliero.  
Sviluppi futuri:
- Integrazione con sistemi esterni
- Adozione standard HL7/FHIR
- Interfaccia utente moderna (React/Angular)
- Dashboard e reportistica avanzata
