using Labs.api;
using Labs.lab1;
using Labs.lab2;
using Labs.lab3;
using System;

namespace Labs
{
    static class Program
    {
        static void Main()
        {
            runConsoleProgram();
        }

        private delegate bool ValueChecker<T>(T value);

        private static void runConsoleProgram()
        {
            byte variantsCount = 3;

            byte userChoise = 255;

            ValueChecker<byte> isUserChoiseInRange = (value) => value != 0 && value <= variantsCount;

            showVariantsTable();

            while (!isUserChoiseInRange(userChoise))
            {
                Log.console("Choose value in range from 1 to {0} : ", variantsCount);
                string input = Console.ReadLine();
                try
                {
                    userChoise = Convert.ToByte(input);
                    if (!isUserChoiseInRange(userChoise))
                    {
                        Log.console("You chose wrong value. Try again.");
                    }
                    else break;
                }
                catch (Exception e)
                {
                    Log.console("Something went wrong : {0}", e.Message);
                }
            }

            switch (userChoise)
            {
                case 1:
                    runLab1();
                    break;
                case 2:
                    runLab2();
                    break;
                case 3:
                    runLab3();
                    break;
                default:
                    Log.console("Unsuported variant");
                    break;
            }

            Console.ReadLine();
        }

        private static void showVariantsTable()
        {
            Log.console(
                "Welcome\n\n" +
                "(1) Lab1\n" +
                "(2) Lab2\n" +
                "(3) Lab3\n");
        }

        private static void runLab1()
        {
            var calc = new IntegersCalculation(
                mutationChance: 0.1,
                algoStepsCount: 60,
                populationSize: 20,
                minValue: -10_000,
                maxValue: 10_000,
                function: x => -(x * x)
                );

            var result = calc.runAlgorithm();

            Log.console("Best x: {0}", result.Value);
        }

        private static void runLab2()
        {
            var calc = new FloatCalculation(
                mutationChance: 0.1,
                algoStepsCount: 10_000,
                populationSize: 30,
                minValue: -100,
                maxValue: 100,
                epsilon: 0.001,
                function: x => -(x * x)
                );

            var result = calc.runAlgorithm();

            Log.console("Best x: {0}", result.Value);
        }

        private static void runLab3()
        {
            var entities = new Entity[]{
                    new Entity(5, 5),
                    new Entity(4, 4),
                    new Entity(3, 3),
                    new Entity(2, 2),
                    new Entity(1, 1),
                };

            var calc = new BackpackProblemCalculations(
                mutationChance: 0.1,
                algoStepsCount: 10_000,
                populationSize: 10,
                maxVolume: 13,
                entities: entities
                );

            var result = calc.runAlgorithm();

            Log.console(result.Value.getFormatedString(entities));
        }
    }
}
