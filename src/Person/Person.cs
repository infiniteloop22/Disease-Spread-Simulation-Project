namespace Disease_Spread_Simulation_Project_3;
public class Person
{
    public string Id { get; set; }
    public int TravelStartTime { get; set; }
    public int TravelEndTime { get; set; }
    public bool IsInfected { get; set; }
    public int InfectionCount { get; set; }
    public int InfectionSpreadCount { get; set; }
    public bool IsDead { get; set; }
    public bool IsQuarantined { get; set; }
    public double QuarantineChance { get; set; }
    public int InfectionDuration { get; set; }
    public int QuarantineDuration { get; set; }

    /// <summary>
    /// Constructor for Person class.
    /// Sets default values for all of the person's properties.
    /// </summary>
    public Person(string Id)
    {
        this.Id = Id;
        TravelStartTime = 0;
        TravelEndTime = 24;
        IsInfected = false;
        IsDead = false;
        IsQuarantined = false;
        InfectionDuration = 0;
        InfectionSpreadCount = 0;
        InfectionCount = 0;
    }

    /// <summary>
    /// Generates a random quarantine chance based on the provided configuration.
    /// The result is a double between 0 and 1.
    /// </summary>
    /// <returns>A double between 0 and 1 representing the quarantine chance.</returns>
    public double GenerateQuarantineChance(Configuration config)
    {
        double mean, stdDev, u1, u2, standardNormal, quarantineChance;
        Random random = new Random();

        mean = config.MeanQuarantineChance;
        stdDev = config.StandardDeviationQuarantineChance;

        u1 = 1.0 - random.NextDouble();
        u2 = 1.0 - random.NextDouble();

        // Box-Muller transform to generate a random value based on normal distribution
        standardNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        quarantineChance = mean + stdDev * standardNormal;

        // Make sure value is between 0 and 1
        return Math.Clamp(quarantineChance, 0, 1);
    }

    /// <summary>
    /// Generates a random population size based on the provided mean and standard deviation.
    /// The result is an integer between 0 and the mean + standard deviation.
    /// </summary>
    /// <returns>An integer representing the population size.</returns>
    public static int GeneratePopulation(int mean, int stdDev)
    {
        int populationSize;
        double u1, u2, standardNormal;
        Random random = new Random();
        u1 = 1.0 - random.NextDouble();
        u2 = 1.0 - random.NextDouble();

        // Box-Muller transform to generate a random value based on normal distribution
        standardNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        populationSize = (int)Math.Round(mean + stdDev * standardNormal);

        return Math.Max(0, populationSize);
    }
}