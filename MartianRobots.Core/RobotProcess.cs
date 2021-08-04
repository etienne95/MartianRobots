using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class RobotProcess
{
    private readonly string _inputPath;

    IDictionary<string, int> orientationToAngle = new Dictionary<string, int>
    {
        { "N", 90 }, { "S", 270 }, { "E", 0 }, { "W", 180 }
    };
    IDictionary<int, string> angleToOrientation = new Dictionary<int, string>
    {
        {  90, "N" }, { 270, "S" }, {  0, "E" }, {  180, "W" }
    };
    IDictionary<int, int> cos = new Dictionary<int, int>
    {
         { 0, 1 }, { 90, 0 }, { 180, -1 }, { 270, 0 }
    };
    IDictionary<int, int> sin = new Dictionary<int, int>
    {
         { 0, 0 }, { 90, 1 }, { 180, 0 }, { 270, -1 }
    };

    public RobotProcess(string inputPath)
    {
        _inputPath = inputPath;
    }

    public IEnumerable<string> ProcessMartianRobots()
    {
        FileStream fileStream = new FileStream(_inputPath, FileMode.Open);
        using StreamReader reader = new StreamReader(fileStream);
        string line;
        int lineNumber = 1;
        int[] upperRightCoordinates = default;
        var scents = new List<(int x, int y)>();

        while ((line = reader.ReadLine()) != null)
        {
            if (lineNumber == 1)
            {
                upperRightCoordinates = line.Split(" ").Select(int.Parse).ToArray();
                ValidateCoordinates(upperRightCoordinates);
            }
            else
            {
                bool lost = false;
                var positionParts = line.Split(" ");
                var (x, y, angle) = (int.Parse(positionParts[0]), int.Parse(positionParts[1]), orientationToAngle[positionParts[2]]);
                var instructions = reader.ReadLine();
                if (instructions == null)
                {
                    throw new Exception("Empty instructions are not valid");
                }

                foreach (var instruction in instructions)
                {
                    if (lost) break;
                    int anguleResult;
                    switch (instruction)
                    {
                        case 'R':
                            anguleResult = angle - 90;
                            angle = anguleResult >= 0 ? GetReducedAngle(anguleResult) : (360 + anguleResult);
                            break;
                        case 'L':
                            anguleResult = angle + 90;
                            angle = GetReducedAngle(anguleResult);
                            break;
                        case 'F':
                            var tentativeX = x + cos[angle];
                            var tentativeY = y + sin[angle];

                            ValidateCoordinates(tentativeX, tentativeY);

                            if (tentativeX > upperRightCoordinates[0] || tentativeY > upperRightCoordinates[1])
                            {
                                if (!scents.Contains((x, y)))
                                {
                                    scents.Add((x, y));
                                    lost = true;
                                    break;
                                }
                            }
                            else
                            {
                                x = tentativeX;
                                y = tentativeY;
                            }
                            break;
                        default:
                            throw new Exception($"Instruction {instruction} not found");
                    }
                }
                var lostText = lost ? " LOST" : "";
                yield return $"{x} {y} {angleToOrientation[angle]}{lostText}";
            }
            lineNumber++;
        }
    }

    int GetReducedAngle(int angle) => angle == 360 ? 0 : angle;
    void ValidateCoordinates(params int[] coordinates)
    {
        if (coordinates.Any(c => c > 50))
        {
            throw new Exception("The maximum value for any coordinate is 50.");
        }
    }
}