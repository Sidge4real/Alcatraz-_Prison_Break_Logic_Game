using Alcatraz__Prison_Break_Logic_Game;
using System;
using System.Data.Common;
using System.Numerics;

public class Game
{
    private const int Rows = 10;
    private const int Columns = 10;

    private ConsoleColor[,] tiles;
    private bool[,] walls;
    private int escapeRow;
    private int escapeColumn;
    private Player player;
    private int round;
    public int steps;
    public ConsoleColor[] roundColors = { ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Green };
    public int roundColorIndex;

    public Game()
    {
        tiles = new ConsoleColor[Rows, Columns];
        /*
          {
            {a,b,c},
            {a,b,c}
          }
        */
        walls = new bool[Rows, Columns];
        steps = 3; // initiele start
        round= 1; //first rounds
        roundColorIndex = 0;
        GenerateTiles();
        GenerateWalls();
        PlacePlayer();
    }

    private void GenerateTiles()
    {
        Random random = new Random();
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                ConsoleColor[] colors = { ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Green };
                tiles[i, j] = colors[random.Next(colors.Length)];
            }
        }
    }

    private void GenerateWalls()
    {
        for (int i = 0; i < Rows; i++)
        {
            walls[i, 0] = true; 
            walls[i, Columns - 1] = true; 
        }
        for (int j = 0; j < Columns; j++)
        {
            walls[0, j] = true;
            walls[Rows - 1, j] = true;
        }

        Random random = new Random();
        int side = random.Next(4); 
        switch (side)
        {
            case 0: // Bovenrand
                escapeRow = 0;
                escapeColumn = random.Next(1, Columns - 1);
                break;
            case 1: // Rechterrand
                escapeRow = random.Next(1, Rows - 1);
                escapeColumn = Columns - 1;
                break;
            case 2: // Onderste rand
                escapeRow = Rows - 1;
                escapeColumn = random.Next(1, Columns - 1);
                break;
            case 3: // Linkerrand
                escapeRow = random.Next(1, Rows - 1);
                escapeColumn = 0;
                break;
        }

        walls[escapeRow, escapeColumn] = false; 
        tiles[escapeRow, escapeColumn] = ConsoleColor.Black; 

        for (int i = 1; i < Rows - 1; i++)
        {
            for (int j = 1; j < Columns - 1; j++)
            {
                walls[i, j] = random.Next(4) == 0; 
            }
        }

        for (int i = 1; i < Rows - 1; i += 2)
        {
            for (int j = 1; j < Columns - 1; j += 2)
            {
                walls[i, j] = false; 
            }
        }
    }

    public void DisplayBoard()
    {
        Console.Clear();
        Console.WriteLine("=== Game Board ===");
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (i == player.Row && j == player.Column)
                {
                    Console.BackgroundColor = tiles[i, j];
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("x ");
                }
                else if (i == escapeRow && j == escapeColumn)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("  ");
                }
                else if (walls[i, j])
                {
                    Console.BackgroundColor = ConsoleColor.Gray; // Muur
                    Console.Write("  ");
                }
                else
                {
                    Console.BackgroundColor = tiles[i, j]; // Vloer
                    Console.Write("  ");
                }
                Console.ResetColor(); // Reset de achtergrondkleur
            }
            Console.WriteLine();
        }
    }

    private void PlacePlayer()
    {
        Random random = new Random();
        int row;
        int column;

        do
        {
            row = random.Next(1, Rows - 1);
            column = random.Next(1, Columns - 1);
        } while (walls[row, column]); // Zorg ervoor dat de speler niet op een muur staat

        player = new Player(row, column);
    }

    public void Move(string direction)
    {
        Console.WriteLine(direction);
        int newRow = player.Row;
        int newColumn = player.Column;

        switch (direction)
        {
            case "up":
                newRow--;
                break;
            case "down":
                newRow++;
                break;
            case "left":
                newColumn--;
                break;
            case "right":
                newColumn++;
                break;
            default:
                Console.WriteLine("Ongeldige richting.");
                return; // Stop de methode als de richting ongeldig is
        }

        if (newRow >= 0 && newRow < Rows && newColumn >= 0 && newColumn < Columns)
        {
            // Controleer of er een muur staat op de nieuwe positie
            if (!walls[newRow, newColumn])
            {
                // Controleer of de nieuwe tegel grijs is
                if (tiles[newRow, newColumn] != ConsoleColor.Gray)
                {
                    // Controleer of de nieuwe tegel overeenkomt met de kleur van de huidige ronde
                    if (tiles[newRow, newColumn] != roundColors[roundColorIndex])
                    {
                        // Verplaats de speler naar de nieuwe positie
                        player.Row = newRow;
                        player.Column = newColumn;
                        steps--;
                        if (steps == 0)
                        {
                            Round(true);
                            steps = 3;
                        }

                        // Controleer of de speler op de ontsnappingsplaats staat
                        if (newRow == escapeRow && newColumn == escapeColumn)
                        {
                            Console.WriteLine("Je hebt de ontsnappingsplaats bereikt! Het spel is voorbij.");
                            return; // Stop het spel
                        }
                    }
                    else
                    {
                        Console.WriteLine("Je kunt die kant niet op bewegen vanwege de kleur van deze ronde.");
                    }
                }
                else
                {
                    Console.WriteLine("Je kunt die kant niet op bewegen vanwege een muur.");
                }
            }
            else
            {
                Console.WriteLine("Je kunt die kant niet op bewegen vanwege een muur.");
            }
        }
        else
        {
            Console.WriteLine("Je kunt die kant niet op bewegen omdat je het bord verlaat.");
        }
    }

    public void Round(bool? add)
    {
        if (add == true) 
        { 
            round++; 
            if(roundColorIndex == 2)
            {
                roundColorIndex = 0;
            }
            else
            {
                roundColorIndex++;
            }
        };
        Console.Write(round);
    }
}
