MovieVault Web API – Assignment 3

Group members and short contribution report 

Gruppe 10.
Christian Broe, 201502223
Mohammad Rajabi, 202106601

Samarbejde:
Vi har i hele forløbet kørt lidt ping-pong frem og tilbage med projektet.
Vi har begge været lidt inde over hinandens kode og bygget videre på hinandens arbejde.
Vi har løbende hjulpet hinanden med de forskellige dele af opgaven.
Arbejdsfordelingen nedenfor beskriver primært, hvad vi hver især har arbejdet mest med. Det betyder dog ikke, at vi ikke også har bidraget til hinandens kode.
Selvom vi har prøvet at dele projektet op i forskellige fokusområder, har vi arbejdet sammen om at få projektet til at fungere.

Christian Broe, 201502223
Har bl.a. arbejdet med de forskellige endpoints i Web API'et.
Har arbejdet med DTO'er og validering.
Har sat Serilog op med MongoDB til logging.
Har testet de forskellige endpoints i Scalar for at se, om de virker.

Mohammad Rajabi, 202106601
Startede projektet op og begyndte med datamodellerne og de forskellige entities.
Har også arbejdet med Entity Framework Core og DbContext.
Har lavet den første grundlæggende opsætning af Web API'et i Program.cs og testet det.
Har arbejdet med seeding af databasen og med at hente data fra den.
Har testet de forskellige endpoints i Scalar for at se, om de virker.


Instructions for running the application 
1. Åbn projektets .sln-fil i Visual Studio 2026.
2. Konfigurer databaseforbindelsen via User Secrets.
3. Opret databasen ved at køre Update-Database i Package Manager Console.
4. Start MongoDB på port 27017, så Serilog kan gemme logs.
5. Start projektet med F5.
6. Åbn http://localhost:5203/scalar/v1 for at teste API'et.

Information about the database setup/migrations 
Vi bruger SQL Server LocalDB og EF Core.

Databasenavn: MovieVaultTestDB
Databaseforbindelsen konfigureres med User Secrets.

1. Højreklik på projektnavnet "assignment_3_movievault" i Solution Explorer, og vælg Manage User Secrets.    
2. Indsæt:
        {
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=MovieVaultTestDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}

3. Kør kommandoen Update-Database i Package Manager Console for at oprette databasen og køre migrationerne.
4. Databasemigrationen indeholder testdata, som indsættes ved oprettelsen.


API base URL:
http://localhost:5203

Scalar UI URL:
http://localhost:5203/scalar/v1

OpenAPI JSON URL:
http://localhost:5203/openapi/v1.json