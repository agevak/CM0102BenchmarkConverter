using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CM0102
{
    public class CM0102BenchmarkConverter
    {
        private const string DEFAULT_CLUB_NAME = "PAS Giannina";

        public static void Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("CM0102BenchmarkConverter.exe <test_name> [club_name]");
                Console.WriteLine();
                Console.WriteLine("test_name - name of the test (tactics).");
                Console.WriteLine($"club_name - name of the club to look for in the benchmark results. Default is '{DEFAULT_CLUB_NAME}'.");
                Console.WriteLine();
                Console.WriteLine("Parses benchmark results from 'benchresult.csv' (or 'benchresult.txt' if first one is absent) and appends them in format of CMTacTool to 'tactool.txt' and in a format of CMTactics to 'repository.csv'.");
                return;
            }
            new CM0102BenchmarkConverter().Start(args[0], args.Length > 1 ? args[1] : DEFAULT_CLUB_NAME);
        }

        private void Start(string testName, string clubName)
        {
            string inputFilename = "benchresult.csv";
            if (!File.Exists(inputFilename)) inputFilename = "benchresult.txt";
            if (!File.Exists(inputFilename))
            {
                Console.WriteLine("Input file 'benchresult.csv' / 'benchresult.txt' is missing.");
                return;
            }
            ConvertBenchmarkResultToCMTacToolFormat(inputFilename, "repository.csv", "tactool.txt", testName, clubName);
        }

        private void ConvertBenchmarkResultToCMTacToolFormat(string inputFilename, string outputFilename, string cmTactToolFilename, string testName, string clubName)
        {
            // Parse input file.
            int playedSum = 0, pointsSum = 0, winHomeSum = 0, winAwaySum = 0, drawHomeSum = 0, drawAwaySum = 0,
                lossHomeSum = 0, lossAwaySum = 0, goalForHomeSum = 0, goalForAwaySum = 0, goalAgHomeSum = 0, goalAgAwaySum = 0, seasonCount = 0;
            string line;
            using (StreamReader reader = new StreamReader(inputFilename))
            {
                StreamWriter cmTackToolWriter = null;
                if (cmTactToolFilename != null) cmTackToolWriter = new StreamWriter(cmTactToolFilename);
                try
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!line.ToLower().Contains(clubName.ToLower())) continue;

                        // Parse single season result.
                        string[] tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        int i = tokens.Length - 1;
                        int points = int.Parse(tokens[i--]);
                        int goalAgAway = int.Parse(tokens[i--]);
                        int goalForAway = int.Parse(tokens[i--]);
                        int lossAway = int.Parse(tokens[i--]);
                        int drawAway = int.Parse(tokens[i--]);
                        int winAway = int.Parse(tokens[i--]);
                        int goalAgHome = int.Parse(tokens[i--]);
                        int goalForHome = int.Parse(tokens[i--]);
                        int lossHome = int.Parse(tokens[i--]);
                        int drawHome = int.Parse(tokens[i--]);
                        int winHome = int.Parse(tokens[i--]);
                        int played = int.Parse(tokens[i--]);

                        // Add to sums.
                        pointsSum += points;
                        goalAgHomeSum += goalAgHome;
                        goalAgAwaySum += goalAgAway;
                        goalForHomeSum += goalForHome;
                        goalForAwaySum += goalForAway;
                        lossHomeSum += lossHome;
                        lossAwaySum += lossAway;
                        drawHomeSum += drawHome;
                        drawAwaySum += drawAway;
                        winHomeSum += winHome;
                        winAwaySum += winAway;
                        playedSum += played;
                        seasonCount++;

                        // Write single season result to CMTactTool file.
                        if (cmTackToolWriter != null) cmTackToolWriter.WriteLine($"{testName} {goalForHome + goalForAway}-{goalAgHome + goalAgAway} {points} {played} {winHome + winAway}-{drawHome + drawAway}-{lossHome + lossAway}");
                    }
                }
                finally
                {
                    try { cmTackToolWriter.Close(); } catch { }
                }
            }

            if (outputFilename != null && seasonCount > 0)
            {
                // Calculate output.
                double goalForAvg = (goalForHomeSum + goalForAwaySum) / (double)seasonCount;
                double goalAgAvg = (goalAgHomeSum + goalAgAwaySum) / (double)seasonCount;
                double pointsAvg = pointsSum / (double)seasonCount;

                // Write output.
                IList<string> lines = new string[] {
                    "Name;Creator;Sub-formation;Sweeper;Defender;Defensive Midfielder;Midfielder;Attacking Midfielder;Forward;;WW / NWW"
                    + ";Games Played;Games Won;Games Drawn;Games Lost;For;Against;Games Won;Games Drawn;Games Lost;For;Against;Points;Goal Difference;Seasons;Avg Goals;Avg Conceded;Avg GD;Avg Points" }.ToList();
                if (File.Exists(outputFilename)) lines = File.ReadAllLines(outputFilename).ToList();
                int i;
                for (i = 0; i < lines.Count; i++) if (lines[i].ToLower().StartsWith(clubName.ToLower())) break;
                if (i == lines.Count) lines.Add("");
                lines[i] = $"{testName};;;;;;;;;;;{playedSum};{winHomeSum};{drawHomeSum};{lossHomeSum};{goalForHomeSum};{goalAgHomeSum}"
                    + $";{winAwaySum};{drawAwaySum};{lossAwaySum};{goalForAwaySum};{goalAgAwaySum}"
                    + $";{pointsSum};{goalForHomeSum + goalForAwaySum - goalAgHomeSum - goalAgAwaySum};{seasonCount};{goalForAvg:0.00};{goalAgAvg:0.00};{goalForAvg - goalAgAvg:0.00};{pointsAvg:0.00}";
                File.WriteAllLines(outputFilename, lines);
            }
        }
    }
}