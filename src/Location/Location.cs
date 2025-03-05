namespace Disease_Spread_Simulation_Project_3;
public class Location
{
    public string Id { get; set; }
    public ICollection<Person> People { get; set; }
    public ICollection<Location> Neighbors { get; set; }

    /// <summary>
    /// Initializes a new instance of the Location class with the specified identifier.
    /// Sets up collections for people and neighboring locations.
    /// </summary>
    public Location(string Id)
    {
        this.Id = Id;
        People = new List<Person>();
        Neighbors = new List<Location>();
    }

    /// <summary>
    /// Adds the specified person to the location's population.
    /// </summary>
    public void AddPerson(Person person)
    {
        People.Add(person);
    }

    /// <summary>
    /// Simulates the spread of disease within the location's population based on the configuration.
    /// Updates the infection status, duration, quarantine status, and life status of each person.
    /// If a person is infected, it decrements their infection duration and checks for death or quarantine.
    /// Increases the infection count for each newly infected person and logs the appropriate messages.
    /// </summary>
    public void SpreadDisease(Configuration config)
    {
        Random random = new Random();
        foreach (Person person in People)
        {
            if (person.IsInfected && !person.IsDead)
            {
                person.InfectionDuration--;

                // Check if the person dies
                if (random.NextDouble() < config.ChanceOfDeath)
                {
                    person.IsDead = true;
                    Console.WriteLine($"{person.Id} has died.");
                    continue; // Skip further processing for this person
                }

                // Check if the infection duration has ended
                if (person.InfectionDuration <= 0)
                {
                    if (random.NextDouble() < person.GenerateQuarantineChance(config))
                    {
                        person.IsQuarantined = true;
                        person.QuarantineDuration = config.QuarantineDurationInHours;
                        person.TravelStartTime--;
                        Console.WriteLine($"{person.Id} is in quarantine and has stopped traveling.");
                    }
                    else
                    {
                        person.IsInfected = false;
                        person.IsQuarantined = false;
                        person.TravelStartTime++;
                        Console.WriteLine($"{person.Id} has exited quarantine and has started traveling again.");
                    }
                }

                // Checl Quarantine duration
                if (person.IsQuarantined && !person.IsDead)
                {
                    person.QuarantineDuration--;
                    // Exit quarantine after the specified duration
                    if (person.QuarantineDuration <= 0)
                    {
                        person.IsQuarantined = false;
                        Console.WriteLine($"{person.Id} has exited quarantine.");
                    }

                    // Spread the disease to other people
                    foreach (Person otherPerson in People)
                    {
                        if (!otherPerson.IsInfected && !otherPerson.IsQuarantined && !otherPerson.IsDead)
                        {
                            if (random.NextDouble() < config.DiseaseSpreadChance)
                            {
                                otherPerson.IsInfected = true;
                                otherPerson.InfectionDuration = config.DiseaseDurationInHours;
                                otherPerson.InfectionCount++;
                                person.InfectionSpreadCount++;
                                Console.WriteLine($"{otherPerson.Id} has been infected.");
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Moves people from the current location to neighboring locations based on the travel chance.
    /// Updates the travel start time and logs the movement.
    /// </summary>
    public void MovePeople(int CurrentHour, Configuration config)
    {
        var peopleCopy = new List<Person>(People); // Creating a copy of the People collection
        Random random = new Random();
        Location targetLocation;

        foreach (Person person in peopleCopy)
        {
            if (!person.IsQuarantined && !person.IsDead && random.NextDouble() < config.TravelChance)
            {
                // To select a neighboring location
                targetLocation = SelectNeighboringLocation();
                if (targetLocation != null)
                {
                    People.Remove(person);
                    targetLocation.AddPerson(person);
                    person.TravelStartTime++;
                    Console.WriteLine($"{person.Id} has traveled to {targetLocation.Id}.");
                }
            }
        }
    }

    /// <summary>
    /// Selects a neighboring location at random from the list of neighbors.
    /// Returns null if the list of neighbors is empty.
    /// </summary>
    /// <returns>A neighboring location, or null if there are no neighbors.</returns>
    private Location SelectNeighboringLocation()
    {
        int index;
        Random random = new Random();
        // To select a neighboring location randomly
        if (Neighbors.Count > 0)
        {
            index = random.Next(Neighbors.Count);
            return Neighbors.ElementAt(index);
        }
        return null;
    }
}