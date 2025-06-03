using System;
using System.IO;

public struct Film
{
    public string genere;
    public string titolo;
    public string durata;
}

class Program
{
    static void Main(string[] args)
    {
        Film[] films = new Film[100];
        string percorsoFile = "films.txt";
        int N = CaricaFilms(films, percorsoFile);
        char scelta;

        Console.WriteLine("PROGRAMMA GESTIONE FILM");

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\nScegli un'opzione: ");
            Console.ResetColor();
            Console.WriteLine("1. Aggiungi film");
            Console.WriteLine("2. Elenco film");
            Console.WriteLine("3. Modifica film");
            Console.WriteLine("4. Elimina film");
            Console.WriteLine("5. Salva film");
            Console.WriteLine("6. Scegli un film casuale per genere");
            Console.WriteLine("0. Esci");
            scelta = Console.ReadKey().KeyChar;

            switch (scelta)
            {
                case '1':
                    AggiungiFilms(films, ref N);
                    break;
                case '2':
                    VisualizzaFilms(films, N);
                    break;
                case '3':
                    ModificaFilm(percorsoFile);
                    N = CaricaFilms(films, percorsoFile);
                    break;
                case '4':
                    EliminaFilm(percorsoFile);
                    N = CaricaFilms(films, percorsoFile);
                    break;
                case '5':
                    SalvaFilms(films, N);
                    break;
                case '6':
                    ScegliFilmCasualePerGenere(percorsoFile);
                    break;
                case '0':
                    return;
                default:
                    Console.WriteLine("Scelta non valida.");
                    break;
            }
        }
    }

    // Aggiunta di uno o più film e salvataggio su file
    static void AggiungiFilms(Film[] films, ref int N)
    {
        Console.WriteLine("\nQuanti film vuoi aggiungere?");
        if (!int.TryParse(Console.ReadLine(), out int numFilms) || numFilms <= 0)
        {
            Console.WriteLine("Numero non valido.");
            return;
        }

        using (StreamWriter file = new StreamWriter("films.txt", append: true))
        {
            for (int i = 0; i < numFilms; i++)
            {
                Film nuovoFilm = new Film();

                Console.WriteLine("Genere del film:");
                nuovoFilm.genere = Console.ReadLine();
                Console.WriteLine("Titolo del film:");
                nuovoFilm.titolo = Console.ReadLine();
                Console.WriteLine("Durata (in minuti):");
                nuovoFilm.durata = Console.ReadLine();

                films[N] = nuovoFilm;
                N++;

                string dati = nuovoFilm.genere + ";" + nuovoFilm.titolo + ";" + nuovoFilm.durata;
                file.WriteLine(dati);
            }
        }
        Console.WriteLine("Film aggiunti con successo.");
    }

    // Visualizza i film presenti nell'array in memoria
    static void VisualizzaFilms(Film[] films, int N)
    {
        if (N == 0)
        {
            Console.WriteLine("Nessun film da visualizzare.");
            return;
        }

        for (int i = 0; i < N; i++)
        {
            Console.WriteLine($"\nFilm {i + 1}:");
            Console.WriteLine("Genere: " + films[i].genere);
            Console.WriteLine("Titolo: " + films[i].titolo);
            Console.WriteLine("Durata: " + films[i].durata + " minuti");
        }
    }

    // Carica i film dal file nel vettore in memoria
    static int CaricaFilms(Film[] films, string percorsoFile)
    {
        int conta = 0;
        if (File.Exists(percorsoFile))
        {
            using (StreamReader file = new StreamReader(percorsoFile))
            {
                string riga;
                while ((riga = file.ReadLine()) != null)
                {
                    string[] dati = riga.Split(';');
                    if (dati.Length == 3)
                    {
                        films[conta].genere = dati[0];
                        films[conta].titolo = dati[1];
                        films[conta].durata = dati[2];
                        conta++;
                    }
                }
            }
        }
        return conta;
    }

    // Salva l'array dei film su un file
    static void SalvaFilms(Film[] films, int N)
    {
        string percorsoFile = "films_salvati.txt";
        using (StreamWriter file = new StreamWriter(percorsoFile))
        {
            for (int i = 0; i < N; i++)
            {
                string dati = films[i].genere + ";" + films[i].titolo + ";" + films[i].durata;
                file.WriteLine(dati);
            }
        }
        Console.WriteLine("Film salvati con successo in films_salvati.txt.");
    }

    // Modifica i dati di un film presente nel file
    static void ModificaFilm(string percorsoFile)
    {
        Console.WriteLine("\nInserisci il titolo del film da modificare:");
        string titolo = Console.ReadLine();
        bool filmTrovato = false;

        if (File.Exists(percorsoFile))
        {
            string[] righe = File.ReadAllLines(percorsoFile);
            for (int i = 0; i < righe.Length; i++)
            {
                string[] dati = righe[i].Split(';');
                if (dati.Length == 3 && dati[1] == titolo)
                {
                    filmTrovato = true;
                    Console.WriteLine("Nuovo genere:");
                    dati[0] = Console.ReadLine();
                    Console.WriteLine("Nuovo titolo:");
                    dati[1] = Console.ReadLine();
                    Console.WriteLine("Nuova durata (minuti):");
                    dati[2] = Console.ReadLine();
                    righe[i] = string.Join(";", dati);
                    break;
                }
            }

            if (filmTrovato)
            {
                File.WriteAllLines(percorsoFile, righe);
                Console.WriteLine("Film modificato con successo.");
            }
            else
            {
                Console.WriteLine("Film non trovato.");
            }
        }
    }

    // Elimina un film dal file
    static void EliminaFilm(string percorsoFile)
    {
        Console.WriteLine("\nInserisci il titolo del film da eliminare:");
        string titolo = Console.ReadLine();
        bool filmTrovato = false;

        if (File.Exists(percorsoFile))
        {
            string[] righe = File.ReadAllLines(percorsoFile);
            using (StreamWriter file = new StreamWriter(percorsoFile))
            {
                for (int i = 0; i < righe.Length; i++)
                {
                    string[] dati = righe[i].Split(';');
                    if (dati.Length == 3 && dati[1] != titolo)
                    {
                        file.WriteLine(righe[i]);
                    }
                    else if (dati.Length == 3 && dati[1] == titolo)
                    {
                        filmTrovato = true;
                    }
                }
            }

            if (filmTrovato)
            {
                Console.WriteLine("Film eliminato con successo.");
            }
            else
            {
                Console.WriteLine("Film non trovato.");
            }
        }
    }
    // Scegli un film casuale per genere
    static void ScegliFilmCasualePerGenere(string percorsoFile)
    {
        Console.WriteLine("\nInserisci il genere di film che vuoi vedere:");
        string genereRichiesto = Console.ReadLine();
        string[] righe = File.Exists(percorsoFile) ? File.ReadAllLines(percorsoFile) : null;

        // Conta quanti film corrispondono al genere
        int conta = 0;
        for (int i = 0; righe != null && i < righe.Length; i++)
        {
            string[] dati = righe[i].Split(';');
            if (dati.Length == 3 && dati[0].Equals(genereRichiesto, StringComparison.OrdinalIgnoreCase))
                conta++;
        }

        if (conta == 0)
        {
            Console.WriteLine("Nessun film trovato per il genere selezionato.");
            return;
        }

        // Scegli un indice casuale tra quelli che corrispondono
        Random rnd = new Random();
        int scelta = rnd.Next(conta);
        int trovato = -1;
        for (int i = 0; i < righe.Length; i++)
        {
            string[] dati = righe[i].Split(';');
            if (dati.Length == 3 && dati[0].Equals(genereRichiesto, StringComparison.OrdinalIgnoreCase))
            {
                trovato++;
                if (trovato == scelta)
                {
                    Console.WriteLine($"\nFilm consigliato:");
                    Console.WriteLine("Titolo: " + dati[1]);
                    Console.WriteLine("Genere: " + dati[0]);
                    Console.WriteLine("Durata: " + dati[2] + " minuti");
                    return;
                }
            }
        }
    }
}
