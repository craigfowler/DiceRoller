
using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using CraigFowler.Gaming.Diceroller.DomainObjects;

namespace CraigFowler.Test.Gaming.Diceroller
{
  [TestFixture]
  public class TestRollingDice
  {
    private const int
      NUMBER_OF_TRIES                   = 10000;
    private const string
      FILE_PATH = "/home/craig/projects/diceroller/doc/rollingOutput.csv";

    [Test]
    public void Roll1()
    {
      string roll = "4d6", filePath = FILE_PATH;
      StringBuilder output = new StringBuilder();

      for(int i = 0; i < NUMBER_OF_TRIES; i++)
      {
        output.Append(String.Format("{0}\n",
                                    DiceSpecification.RollOnce(roll)));
      }

      writeResultsToFile(output.ToString(), filePath);
    }

    private void writeResultsToFile(string results, string path)
    {
      if(File.Exists(path))
      {
        File.Delete(path);
      }

      File.WriteAllText(path, results);
    }
  }
}
