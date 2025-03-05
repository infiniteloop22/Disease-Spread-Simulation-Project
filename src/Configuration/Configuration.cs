using System.Text.Json;
using System.Text.Json.Serialization;

namespace Disease_Spread_Simulation_Project_3
{
    public class Configuration
    {
        public double MeanPopulationSize { get; set; }
        public double StandardDeviationPopulationSize { get; set; }
        public double DiseaseSpreadChance { get; set; }
        public double ChanceOfDeath { get; set; }
        public int DiseaseDurationInHours { get; set; }
        public int QuarantineDurationInHours { get; set; }
        public double MeanQuarantineChance { get; set; }
        public double StandardDeviationQuarantineChance { get; set; }
        public int SimulationDurationInHours { get; set; }
        public double TravelChance { get; set; }

        /// <summary>
        /// Initializes a new instance of the Configuration class with the specified parameters.
        /// This constructor is used to deserialize a configuration from JSON, setting up
        /// various parameters for the disease spread simulation, including population size,
        /// disease spread chance, mortality rate, and simulation duration.
        /// </summary>
        [JsonConstructor]
        public Configuration(double meanPopulationSize, double standardDeviationPopulationSize, double diseaseSpreadChance, double chanceOfDeath, int diseaseDurationInHours, int quarantineDurationInHours, double meanQuarantineChance, double standardDeviationQuarantineChance, int simulationDurationInHours, double travelChance)
        {
            MeanPopulationSize = meanPopulationSize;
            StandardDeviationPopulationSize = standardDeviationPopulationSize;
            DiseaseSpreadChance = diseaseSpreadChance;
            ChanceOfDeath = chanceOfDeath;
            DiseaseDurationInHours = diseaseDurationInHours;
            QuarantineDurationInHours = quarantineDurationInHours;
            MeanQuarantineChance = meanQuarantineChance;
            StandardDeviationQuarantineChance = standardDeviationQuarantineChance;
            SimulationDurationInHours = simulationDurationInHours;
            TravelChance = travelChance;
        }

        /// <summary>
        /// Initializes a new instance of the Configuration class by loading a configuration from the specified file path.
        /// The configuration is loaded from a JSON file and sets up various parameters for the disease spread simulation, including population size,
        /// disease spread chance, mortality rate, and simulation duration.
        /// </summary>
        public Configuration(string filePath)
        {
            LoadConfiguration(filePath);
        }

        /// <summary>
        /// Loads the configuration from the specified file path.
        /// The configuration is read from a JSON file and sets up various parameters for the disease spread simulation, including population size,
        /// disease spread chance, mortality rate, and simulation duration.
        /// If there is an error reading the file, an error message is logged to the console.
        /// </summary>
        private void LoadConfiguration(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                Configuration config = JsonSerializer.Deserialize<Configuration>(json);

                MeanPopulationSize = config.MeanPopulationSize;
                StandardDeviationPopulationSize = config.StandardDeviationPopulationSize;
                DiseaseSpreadChance = config.DiseaseSpreadChance;
                ChanceOfDeath = config.ChanceOfDeath;
                DiseaseDurationInHours = config.DiseaseDurationInHours;
                QuarantineDurationInHours = config.QuarantineDurationInHours;
                MeanQuarantineChance = config.MeanQuarantineChance;
                StandardDeviationQuarantineChance = config.StandardDeviationQuarantineChance;
                SimulationDurationInHours = config.SimulationDurationInHours;
                TravelChance = config.TravelChance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading configuration file: {ex.Message}");
            }
        }
    }
}
