using System.Text;

namespace Alcatraz__Prison_Break_Logic_Game;
class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        Player player = new Player();

        // Oneindige spelloop
        while (true)
        {
            game.DisplayBoard();

            // Vraag om invoer
            Console.WriteLine("Geef een richting (up/down/left/right) of typ 'exit' om te stoppen:");
            Console.Write("round ");
            game.Round(false);
            Console.Write(", step " + game.steps + ", " + "forbidden color: " + game.roundColors[game.roundColorIndex]);
            Console.WriteLine();
            string input = Console.ReadLine();

            // Controleer op stoppen
            if (input.ToLower() == "exit")
                break;

            game.Move(input);
        }
    }
}
