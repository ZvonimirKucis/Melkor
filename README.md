# Melkor

Web aplikacija pomoću koje se može vršiti provjera određenih kodova tj. zadaća. Aplikacija pomoću github link-a skida projekta, kompajlira ga i testira, ako su definirani testovi za taj projekt. 

Svaka razina je zasebno ostvarena:
  
1. Skidanje projekta:
Aplikacija skida projekt sa git-a. U svrhu preuzimanja koristimo klasu WebClient kojoj predajemo url projekta. Projekt se tada preuzima u .zip formatu. Nakon čega ga raspakiravamo u zasebnu datoteku za svakog korisnika(root_datoteka\guid_korisnika).

2. Kompajliranje(build):
