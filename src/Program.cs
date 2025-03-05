namespace Disease_Spread_Simulation_Project_3;

internal class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// This method asks the user for a file path to a configuration file, 
    /// creates a new Configuration object and a new Simulation object from it, 
    /// and then runs the simulation.
    /// </summary>
    static void Main(string[] args)
    {
        string filePath;
        Console.Write("Enter the file path to the configuration file: ");
        filePath = Console.ReadLine();

        Configuration config = new Configuration(filePath);
        Simulation simulation = new Simulation(config);

        simulation.Run();
    }
}