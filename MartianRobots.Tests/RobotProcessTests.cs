using System;
using System.IO;
using Xunit;
using System.Linq;
using FluentAssertions;
using System.Collections.Generic;

namespace MartianRobots.Tests
{
    public class RobotProcessTests
    {
        const string INPUT_PATH = "testInput.txt";

        [Theory]
        [InlineData("5 3-1 1 E-RFRFRFRF-3 2 N-FRRFLLFFRRFLL-0 3 W-LLFFFLFLFL", "1 1 E-3 3 N LOST-2 3 S")]
        public void Given_correct_inputs_should_generate_corresponding_output(string input, string expectedOutput)
        {
            // Arrange
            var sut = new RobotProcess(INPUT_PATH);
            using (StreamWriter writer = System.IO.File.AppendText(INPUT_PATH))
            {
                var lines = input.Split("-");
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }

            // Act
            var result = sut.ProcessMartianRobots().ToList();

            // Assert
            var expectedLines = expectedOutput.Split("-");
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Should().Be(expectedLines[i]);
            }

            System.IO.File.WriteAllText(INPUT_PATH, string.Empty);
        }

        [Theory]
        [InlineData("51 3-1 1 E-RFRFRFRF", "The maximum value for any coordinate is 50.")]
        [InlineData("5 3-1 100 E-RFRFRFRF", "The maximum value for any coordinate is 50.")]
        [InlineData("5 3-1 100 E-A", "Instruction A not found")]
        public void Given_invalid_inputs_should_throw_corresponding_exception(string input, string expectedOutput)
        {
            // Arrange
            var sut = new RobotProcess(INPUT_PATH);
            using (StreamWriter writer = System.IO.File.AppendText(INPUT_PATH))
            {
                var lines = input.Split("-");
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }

            // Act
            Func<List<string>> act = () => sut.ProcessMartianRobots().ToList();

            // Assert
            act.Should()
                .Throw<Exception>()
                .WithMessage(expectedOutput);

            System.IO.File.WriteAllText(INPUT_PATH, string.Empty);
        }
    }
}
