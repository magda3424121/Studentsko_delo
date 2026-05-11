# Studentsko delo

Spletna aplikacija za iskanje, objavljanje in upravljanje studentskih del. Studentom omogoca pregled oglasov, prijavo na delo in spremljanje statusa prijav, podjetjem pa objavo del ter sprejem ali zavrnitev kandidatov. Administrator ima loceno prijavo in pregled glavnih entitet sistema.

## Struktura projekta

```text
Studentsko_delo-main/
├── Backend/        .NET Core Web API, modeli, kontrolerji, baza in wwwroot
├── Frontend/       React + TypeScript + Vite izvorna koda
├── Docs/           projektna dokumentacija, ER-model in slike
├── README.md       predstavitev projekta in navodila
└── Studentsko_delo-main.sln
```

## Tehnoloski sklad

- Backend: .NET Core Web API, C#
- Frontend: React, TypeScript, Vite
- Baza podatkov: PostgreSQL na zunanjem gostitelju
- ORM: Entity Framework Core
- PostgreSQL gonilnik: Npgsql.EntityFrameworkCore.PostgreSQL
- Verzijsko vodenje: Git in GitHub
- Projektno vodenje: Trello

## Funkcionalnosti

- Registracija in prijava studentov
- Registracija in prijava podjetij
- Locena prijava administratorja iz baze
- Pregled in filtriranje oglasov za delo
- Prikaz podrobnosti oglasa v pojavnem oknu
- Prijava dijaka/studenta na oglas
- Pregled statusa lastnih prijav
- Nadzorna plosca za podjetja
- Pregled prosenj kandidatov ter sprejem ali zavrnitev prijave
- CRUD za oglase, uporabnike, podjetja, kraje, vrste uporabnikov in prijave
- Povezava na zunanjo PostgreSQL bazo

## Navodila za zagon

1. Namesti .NET SDK, Node.js in npm.
2. Preveri povezavo do zunanje PostgreSQL baze v `Backend/appsettings.json`.
3. Zgradi frontend:

```bash
cd Frontend
npm install
npm run build
cd ..
```

Ukaz `npm run build` pripravi React aplikacijo v `Backend/wwwroot`, zato jo potem servira .NET backend.

4. Zazeni backend:

```bash
dotnet restore Backend/Studentski_servis.csproj
dotnet build Backend/Studentski_servis.csproj
dotnet run --project Backend/Studentski_servis.csproj
```

5. Odpri aplikacijo:

```text
http://localhost:5052
```

Admin prijava:

```text
http://localhost:5052/admin-prijava.html
```

## REST API koncne tocke

| Entiteta | Metoda | Pot | Opis |
| --- | --- | --- | --- |
| Oglasi | GET | `/api/JobOffer` | Seznam oglasov |
| Oglasi | POST | `/api/JobOffer` | Dodajanje oglasa |
| Oglasi | PUT | `/api/JobOffer/{id}` | Urejanje oglasa |
| Oglasi | DELETE | `/api/JobOffer/{id}` | Brisanje oglasa |
| Uporabniki | GET/POST/PUT/DELETE | `/api/Uporabnik` | CRUD za uporabnike |
| Podjetja | GET/POST/PUT/DELETE | `/api/Podjetje` | CRUD za podjetja |
| Kraji | GET/POST/PUT/DELETE | `/api/Kraj` | CRUD za kraje |
| Prijave | GET/POST/PUT/DELETE | `/api/Prijava` | CRUD za prijave |
| Prijave | GET | `/api/Prijava/podjetje/{podjetjeId}` | Prijave na oglase podjetja |
| Prijave | GET | `/api/Prijava/uporabnik/{uporabnikId}` | Prijave izbranega studenta |
| Prijave | PUT | `/api/Prijava/{id}/status` | Sprejem ali zavrnitev prijave |
| Vrste uporabnikov | GET/POST/PUT/DELETE | `/api/VrstaUporabnika` | CRUD za vrste uporabnikov |
| Avtentikacija | POST | `/api/Auth/register` | Registracija studenta |
| Avtentikacija | POST | `/api/Auth/login` | Prijava studenta |
| Podjetja | POST | `/api/Podjetje/register` | Registracija podjetja |
| Podjetja | POST | `/api/Podjetje/login` | Prijava podjetja |
| Admin | POST | `/api/Admin/login` | Prijava administratorja |

## Zaslonske slike

![Glavna stran](Docs/screenshots/glavna.png)

![Prijava](Docs/screenshots/prijava.png)

![Admin prijava](Docs/screenshots/admin.png)

## Projektno vodenje

Trello tabla: https://trello.com/invite/b/699418d140e7886cb9aa54ee/ATTIa93712659a93b3a8367a324baa070af4C7DD6197/my-trello-board

Tabla mora vsebovati stolpce `To-Do`, `Doing` in `Done`. Vsaka kartica naj bo zapisana kot uporabniska zgodba in dodeljena odgovorni osebi.

## GitHub

Povezava do GitHub repozitorija: https://github.com/magda3424121/Studentsko_delo
