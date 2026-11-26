MovieVault Advanced Web API – Assignment 4

Gruppemedlemmer

Gruppe 10.

Christian Broe, 201502223

Mohammad Rajabi, 202106601



Samarbejde:

Vi har i Assignment 4 bygget videre på MovieVault Web API fra Assignment 3.

Vi har ligesom i Assignment 3 arbejdet sammen om projektet og løbende bygget videre på hinandens kode. Vi har brugt GitHub til versionsstyring og arbejdet på forskellige branches.

Arbejdsfordelingen nedenfor beskriver primært, hvad vi hver især har arbejdet mest med. Vi har også løbende hjulpet hinanden med de forskellige dele af opgaven:



Christian Broe, 201502223

Har arbejdet med Background Service, som automatisk simulerer billetsalg.

Har også arbejdet med JWT, så simulatoren kan logge ind og få adgang til de forskellige endpoints.

Har bygget videre på den logging, som vi lavede i Assignment 3 med Serilog og MongoDB.

Har lavet et endpoint, hvor man som Admin kan hente de forskellige logs og filtrere dem.

Har løbende testet simulatoren, logging og de forskellige adgangsrettigheder for at se, om det virker.

Har også arbejdet med README.md og dokumentation af projektet.



Mohammad Rajabi, 202106601

Har bl.a. arbejdet med ASP.NET Core Identity, JWT og de forskellige roller og hvad de hver især har adgang til.

Har arbejdet med at tilføje filmanmeldelser og medier til vores eksisterende projekt.

Har også arbejdet med datamodellerne, opdateret E/R-diagrammet og migrations

Har arbejdet med de forskellige forretningsregler og valideringer fra Assignment 3.

Har også arbejdet med at sætte Docker og Docker Compose op, så vi kan køre hele projektet samlet.



Gennem projektet har begge parter testet systemet i /Scalar og gennem Postman for at checke at det hele virker.



Installation og Opstart af Applikationen:

1) Docker Engine skal køre i baggrunden

2) På projektets root directory (samme sted som .csproj er), kør kommandoen "docker compose up --build"

3) Tilgå http://localhost:5203/Scalar for at tilgå Scalar



Database, Migrations og Seeding:

Vi bruger fortsat SQL Server og Entity Framework Core.

Databasenavn:MovieVaultTestDB 

Vi bygger videre på vores database fra assignment 3 og har udvidet den med bl.a. brugere, roller, filmanmeldelse og medier.

Vi har lavet nye migrationer i forbindelse med de ændringer der foretaget i datamodellerne.

Ved opstart af Web API kører SeedUsers, som bruges til oprette de forskellige brugere og roller.

MongoDB bruges til at gemme vores logs.

API base URL: http://localhost:5203

Scalar UI URL: http://localhost:5203/scalar/v1

OpenAPI JSON URL: http://localhost:5203/openapi/v1.json



Authentication, Authorization og demo-brugere:

Vi bruger JWT til at logge ind og få adgang til de forskellige endpoints i vores Web API.

Vi har oprettet fire forskellige roller:

Admin 

Navn: Admin User

Brugernavn: admin@movievault.com

Actor 

Navn: Leonardo DiCaprio

Brugernavn: lD@movievault.com

Director 

Navn: Christopher Nolan

Brugernavn: CN@movievault.com

Reviewer 

Navn: Alex Morgan

Brugernavn: AM@movievault.com

Navn: Jamie Taylor

Brugernavn: JT@movievault.com

Navn: Sam Rivera

Brugernavn: SR@movievault.com

Adgangskoderne er gemt i env-filen.
Man logger ind gennem følgende endpoints:
POST /api/auth/login 
Når man har logget ind, får man et JWT-token, som kan bruges til at få adgang til de forskellige endpoints. 



Test i Postman: 

For at teste i Postman, udfør følgende steps: 

1) Kør kommandoen "docker compose up --build" (docker skal køre i baggrunden) ved rodmappen i projektet (samme sted som .csproj filen)

2) Når applikationen er startet, og for at tilgå de forskellige endpoints med HTTP request methods, gå til "http://localhost:5203/Scalar". 

3) For endpoints, der kræver authentication, skal brugeren først logge ind gennem en post request til "http://localhost:5203/api/auth/login" med de rigtige credentials (se de forskellige accounts under roller). Efter JWT token er returneret som response, kopieres token og indsættes under fanen "Authorization", hvor typen "Bearer Token" vælges. 

4) Vælg de rigtige HTTP request methods og angiv endpoints efter base urlen "http://localhost:5203/". Ved endpoints, der kræver en request body, gå til Body -> Raw og angiv JSON body. 
5) Klik på send.  


Ticket Sale Simulator:
Vi har lavet en Background Service, som automatisk simulerer billetsalg.

Simulatoren kommunikerer med de forskellige endpoints, gennem Web API. Simulatoren har ikke direkte adgang til databasen.

Vi har oprettet en separat bruger (Admin), som simulatoren bruger når den skal logge ind og få et JWT-token.

Simulatoren vælger en tilfældigt film, forestilling og X antal billetter og forsøger derefter at registrere billetsalget gennem vores Web API.

Simulatoren starter op automatisk, når vi starter projektet i Docker Compose.

Vi har sat simulatoren til at registrere et billetsalg hvert 10. sekund. Dette kan ændres i .env-filen under:

SIMULATOR_INTERVAL_SECONDS=10

Brugernavn og adgangskode til simulatoren konfigureres også i .env-filen:

SIMULATOR_USERNAME

SIMULATOR_PASSWORD

Hvis der sker en midlertidig fejl, fortsætter simulatoren og forsøger igen efter 10. sekunder eller hvad intervallet er sat til.



Forretningsregler og antagelser:

Vi har arbejdet videre på forretningsreglerne fra Assignment 3. 

Antal af købte billetter skal være større end 0. 

Salgsdatoen skal være i fortiden. 

Billetprisen må ikke være negativ.

Antallet af solgte billetter må ikke overstige biografsalens kapacitet.

Simulerede billetsalg og manuelle billetsalg, behandles på samme måde gennem vores Web API. Det er kun brugere Admin, der kan oprette, ændre og slette billetsalg. Ved ændringer eller hvis der slettes et billetsalg, skal antallet af ledige pladser og omsætningen også ændres.



