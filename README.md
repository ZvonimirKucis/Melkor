# Melkor

Web aplikacija pomoću koje se može vršiti provjera određenih kodova tj. zadaća. Aplikacija pomoću github link-a skida projekta, kompajlira ga i testira, ako su definirani testovi za taj projekt. 

Svaka razina je zasebno ostvarena:
  
1. Skidanje projekta:

Aplikacija skida projekt sa git-a. U svrhu preuzimanja koristimo klasu WebClient kojoj predajemo url projekta. Projekt se tada preuzima u .zip formatu. Nakon čega ga raspakiravamo u zasebnu datoteku za svakog korisnika(root_datoteka\guid_korisnika).

2. Kompajliranje(build):

Nakon skidanja prolazi se kroz sve datoteke, te se svaki projekt zasebno kompajlira. Kompajliranje se vrši uz pomoć .NET framework-a...

3. Testiranje:

Testiranje je definirano samo za neke projekte. U trenutnoj fazi aplikacije, testiraju se samo .dll datotetke. Kod testiranja prvo se pokreće klasa TestPicker kojom se odabiru definirani testovi za taj projekt. Ta klasa tada instancira nove klase ovisno o zadaći(TesterH2T1, testira zadaću(homework) 2 i zadatak(task) 1). U tim klasama se učitava assembly .dll datoteka iz zadaće, te se nad njima provode testovi. U samoj metodi testa se stvaraju nove instance klasa definiranih u učitanim .dll datotekama, te se pozivaju metode definirane unutar njih. Pozivanjem metoda se također utvrđuje njihov ispravan rad. Prolazak testa se kasnji prikazuje na web stranici(više o tome u nastavku).

**OPIS KORIŠTENJA WEB STRANICE:**

Home(Melkor) stranica:<p></p>
Na početnoj stranici se vide trenutne obavijesti, koje postavlja admin stranice. Obavijseti se uzimaju iz baze, te se najaktualnije prikazuju korisniku.

