using System;

public class Program
{
    public static void Main()
    {
        try
        {
            var robotProcess = new RobotProcess("input.txt");
            foreach (var message in robotProcess.ProcessMartianRobots())
            {
                Console.WriteLine(message);
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"The program has failed with the error message: '{ex.Message}'");
        }
    }
}