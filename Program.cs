using System; // importerer system-navnet, som giver adgang til konsol-funktioner.

namespace Rpssl; // angiver programmets namespace.

internal class Program // definerer klassen Program, som indeholder logikken for spillet.
{
    private const int WinningScore = 3; // angiver hvor mange point der skal til for at vinde spillet.

    private static void Main(string[] args) // hovedmetoden hvor programmet starter sin eksekvering.
    {
        Console.WriteLine("RPSSL — Rock, Paper, Scissors, Spock, Lizard"); //skriver spillets titel i terminalen.
        Console.WriteLine($"Først til {WinningScore} vinder. Vælg R/P/S/Sp/L eller Q for at stoppe.\n"); //informerer spilleren om regler og kommandoer.

        int userScore = 0, agentScore = 0; // initialiserer to variabler til at holde styr på point for bruger og agent.

        while (userScore < WinningScore && agentScore < WinningScore) // løkke, der kører indtil enten brugeren eller agenten har nået vinderscoren.
        {
            Console.Write("Dit valg (R/P/S/Sp/L eller Q): "); // Skriver en prompt for at få spillerens input.
            var raw = Console.ReadLine(); // Læser input fra brugeren som en streng.

            if (string.Equals(raw, "q", StringComparison.OrdinalIgnoreCase)) // Tjekker om spilleren har skrevet q eller Q for at afslutte spillet.
            {
                Console.WriteLine("Afslutter spillet. Tak for kampen!"); // skriver en afslutningsbesked.
                return; // stopper programmet og vender tilbage fra Main-metoden.
            }

            if (!TryParseShape(raw, out var user)) // kalder TryParseShape for at oversætte tekstinput til en shape. Retunerer false, hvis input er ugyldigt.
            {
                Console.WriteLine("Ugyldigt input. Brug R/P/S/Sp/L (eller Q)."); // Informerer brugeren om ugyldigt input.
                continue; // Hopper tilbage til starten af while-løkken for et nyt forsøg.
            }

            var agent = PickAgent(); // kalder PickAgent(), som vælger en tilfældig form til agenten.
            var result = ResolveRound(user, agent); // Kalder ResolveRound() for at finde ud af, hvem der vandt runden.

            if (result == Result.Win) userScore++; // Hvis spilleren vandt runden, øges brugerens score med 1.
            else if (result == Result.Lose) agentScore++; // Hvis spilleren tabte, øges agentens score med 1.

            PrintRound(user, agent, result, userScore, agentScore); // Kalder PrintRound() for at vise rundens detaljer og den aktuelle score. 
        }

        Console.WriteLine(userScore > agentScore ? "\n Du vandt spillet!" : "\n Agenten vandt spillet!"); // Når løkken stopper, skrives vinderen af spillet.
    }

    // Hjælpefunktioner

    private static bool TryParseShape(string? input, out Shape shape) // Funktion der prøver at konvertere tekstinput til et Shape-objekt.
    {
        switch ((input ?? string.Empty).Trim().ToLower()) // Gør input småt, fjerner mellemrum og håndterer null.
        {
            case "r": // hvis brugeren skrev r...
            case "rock":     shape = Shape.Rock;     return true; // eller rock, sættes shape til rock og der returneres true.
            case "p":
            case "paper":    shape = Shape.Paper;    return true; // paper vælges.
            case "s":
            case "scissors": shape = Shape.Scissors; return true; // scissors vælges.
            case "sp":
            case "spock":    shape = Shape.Spock;    return true; // Spock vælges.
            case "l":
            case "lizard":   shape = Shape.Lizard;   return true; // Lizard vælges
            default:
                shape = default; // hvis input ikke matcher nogen af ovenstående, sættes shape til standard (0).
                return false; // Retunerer false, så programmet ved, at inputtet er ugyldigt.
        }
    }

    private static Shape PickAgent() // funktion der vælger en tilfældig form for agenten.
        => (Shape)Random.Shared.Next(0, 5); // Returnerer et tilfældigt tal mellem 0 og 4, som konverteres til et Shape-enum.
    
    private static Result ResolveRound(Shape p1, Shape p2) // Funktion der bestemmer udfaldet af en runde.
    {
        if (p1 == p2) return Result.Tie; // Hvis begge vælger samme form, bliver resultatet uafgjort.

        // Beregner forskellen mellem spillernes valg som heltal.
        int diff = (int)p2 - (int)p1;

        // Tjekker differencen for at finde vinder.
        switch (diff)
        {
            case -4: // kombinationer hvor spilleren vinder.
            case -2:
            case 1:
            case 3:
                return Result.Win; // Returnerer Win, hvis spilleren har vundet.

            // Kombinationer hvor spilleren taber.
            case -3:
            case -1:
            case 2:
            case 4:
                return Result.Lose; // Returnerer Lose, hvis spilleren har tabt.

            default:
                // Bør ikke ske, men fallback: sikrer at der altid returneres et resultat
                return Result.Tie;
        }
    }

    private static void PrintRound(Shape user, Shape agent, Result result, int userScore, int agentScore) // Funktion der viser rundens resultat.
    {
        Console.WriteLine($"\nDu: {user}  vs  Agent: {agent}"); // Viser hvad brugeren og agenten valgte.
        Console.WriteLine(result switch // brug af switch expression til at vælge besked ud fra resultatet.
        {
            Result.Win  => "→ Du vandt runden!", // Hvis spilleren vandt
            Result.Lose => "→ Agenten vandt runden!", // Hvis agenten vandt.
            _           => "→ Uafgjort!", // Ellers er det uafgjort.
        });
        Console.WriteLine($"Stilling: Du {userScore} : {agentScore} Agent\n"); //Viser den opdaterede stilling efter runden.
    }

    // Typer

    private enum Shape // Enum der definerer de fem mulige valg i spillet.
    {
        Rock = 0, // sten
        Paper = 1, // papir
        Scissors = 2, // saks
        Spock = 3, // Spock
        Lizard = 4 // Firben
    }

    private enum Result { Win, Lose, Tie } // Enum der bruges til at angive udfaldet.
}
