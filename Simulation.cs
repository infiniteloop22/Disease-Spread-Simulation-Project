namespace Disease_Spread_Simulation_Project_3;

using System;
using System.IO;

public class Simulation
{
    public ICollection<Location> Locations { get; set; }
    public int TotalHours { get; set; }
    public int CurrentHour { get; set; }
    public Configuration Config { get; set; }

    /// <summary>
    /// Initializes a new instance of the Simulation class with the specified configuration.
    /// Sets up the locations collection and initializes the total and current hours for the simulation.
    /// </summary>
    public Simulation(Configuration Config)
    {
        this.Config = Config;
        Locations = new List<Location>();
        TotalHours = Config.SimulationDurationInHours;
        CurrentHour = 0;
    }

    /// <summary>
    /// Initializes the simulation by creating 5 locations and adding a population to each.
    /// The population size is randomly generated based on the mean and standard deviation in the configuration.
    /// Then, the neighbors of each location are randomly connected, and one person is randomly infected at each location.
    /// </summary>
    public void Initialize()
    {
        int populationSize;
        Location location;
        Person person;

        // Initialize locations and people based on the configuration
        for (int i = 1; i <= 5; i++)
        {
            location = new Location($"Location {i}");
            Locations.Add(location);

            // Add people to the location
            populationSize = Person.GeneratePopulation((int)Config.MeanPopulationSize, (int)Config.StandardDeviationPopulationSize);
            for (int j = 1; j <= populationSize; j++)
            {
                person = new Person($"Person {j} at Location {i}");
                location.AddPerson(person);
            }
        }

        // Set up neighbors (graph)
        ConnectNeighbors();
        RandomInfect();
    }

    /// <summary>
    /// Connects the neighbors of each location. The number of neighbors for each location is randomly generated,
    /// and the neighbors are randomly selected from the list of locations. This function ensures that the connections
    /// between locations are bidirectional.
    /// </summary>
    private void ConnectNeighbors()
    {
        int numberOfNeighbors, neighborIndex;
        var locationList = new List<Location>(Locations);
        Random random = new Random();

        // Iterate over each location in the location list
        for (int i = 0; i < locationList.Count; i++)
        {
            numberOfNeighbors = random.Next(1, locationList.Count / 2); // generate a random number of neighbors

            // Iterate over the number of neighbors for the current location
            for (int j = 0; j < numberOfNeighbors; j++)
            {
                // Generate a random index for a potential neighbor
                neighborIndex = random.Next(locationList.Count);

                // Check if the potential neighbor is not the same location and is not already a neighbor
                if (neighborIndex != i && !locationList[i].Neighbors.Contains(locationList[neighborIndex]))
                {
                    // Add the neighbor to the current location's list of neighbors
                    locationList[i].Neighbors.Add(locationList[neighborIndex]);

                    // Add the current location to the neighbor's list of neighbors (to make the connection bidirectional)
                    locationList[neighborIndex].Neighbors.Add(locationList[i]);
                }
            }
        }
    }

    /// <summary>
    /// Randomly infects a person at a randomly selected location.
    /// Sets the infection duration to the disease duration.
    /// Logs a message indicating which person was infected.
    /// </summary>
    private void RandomInfect()
    {
        int randomLocationIndex, randomPersonIndex;
        Random random = new Random();

        // Generate a random index for the location
        randomLocationIndex = random.Next(Locations.Count);

        // Get the location at the random index
        var randomLocation = Locations.ElementAt(randomLocationIndex);

        // Generate a random index for the person in the location
        randomPersonIndex = random.Next(randomLocation.People.Count);

        // Get the person at the random index
        var randomPerson = randomLocation.People.ElementAt(randomPersonIndex);
        randomPerson.IsInfected = true;
        randomPerson.InfectionDuration = Config.DiseaseDurationInHours;

        Console.WriteLine($"Randomly infected {randomPerson.Id}");
    }

    /// <summary>
    /// Executes the simulation by initializing locations and populations, 
    /// and iteratively simulating the spread of disease and movement of people until 
    /// the simulation duration is reached or completion criteria are met.
    /// Logs the progress at each hour and generates a report at the end.
    /// </summary>
    public void Run()
    {
        Initialize(); // Ensure that locations and people are initialized

        Console.WriteLine($"Total Locations: {Locations.Count}");
        foreach (var location in Locations)
        {
            Console.WriteLine($"Location {location.Id} has {location.People.Count} people.");
        }

        while (CurrentHour <= TotalHours)
        {
            Console.WriteLine($"Current hour: {CurrentHour}");
            SpreadDisease();
            MovePeople(Config);

            // Check if everyone is dead or if there are no infected people left
            if (IsSimulationComplete())
            {
                Console.WriteLine("Simulation ended early.");
                break;
            }

            CurrentHour++;
        }
        GenerateReport();
    }

    /// <summary>
    /// Simulates the spread of disease across all locations in the simulation.
    /// Each location will have its SpreadDisease method called with the current
    /// configuration.
    /// </summary>
    private void SpreadDisease()
    {
        foreach (Location location in Locations)
        {
            location.SpreadDisease(Config);
        }
    }

    /// <summary>
    /// Simulates the movement of people across all locations in the simulation.
    /// Each location will have its MovePeople method called with the current hour and configuration.
    /// </summary>
    private void MovePeople(Configuration config)
    {
        foreach (Location location in Locations)
        {
            location.MovePeople(CurrentHour, config);
        }
    }

    /// <summary>
    /// Checks if the simulation should be ended early.
    /// The simulation should be ended if everyone is dead or if there are no infected people left.
    /// </summary>
    /// <returns>True if the simulation should be ended, false otherwise.</returns>
    private bool IsSimulationComplete()
    {
        int totalPeople = 0;
        int totalInfected = 0;
        int totalDead = 0;
        foreach (Location location in Locations)
        {
            foreach (Person person in location.People)
            {
                totalPeople++;
                if (person.IsInfected) totalInfected++;
                if (person.IsDead) totalDead++;
            }
        }

        // End the simulation if everyone is dead or if there are no infected people left
        if (totalDead == totalPeople || totalInfected == 0) { return true; }
        return false;
    }

    /// <summary>
    /// Generates a report of the simulation's current state and writes it to a csv file.
    /// The report includes total and current simulation hours, and the count of infected, dead,
    /// and quarantined individuals for each location in the simulation.
    /// </summary>
    private void GenerateReport()
    {
        int infectedCount = 0;
        int deadCount = 0;
        int quarantinedCount = 0;
        string filePath = "simulation_report.csv";
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Simulation Report");
            writer.WriteLine($"Total Hours: {TotalHours}");
            writer.WriteLine($"Current Hour: {CurrentHour}");
            writer.WriteLine("Location,Infected,Dead,Quarantined");

            foreach (Location location in Locations)
            {

                foreach (Person person in location.People)
                {
                    if (person.IsInfected) infectedCount++;
                    if (person.IsDead) deadCount++;
                    if (person.IsQuarantined) quarantinedCount++;
                }
                writer.WriteLine($"{location.Id},{infectedCount},{deadCount},{quarantinedCount}");
            }
        }
    }
}