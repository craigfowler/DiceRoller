
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
      FILE_PATH = "/home/craig/projects/DiceRoller/doc/rollingOutput.csv";

    [Test]
    public void Roll6D4()
    {
      string roll = "6d4", filePath = FILE_PATH;
      DiceSpecification spec = new DiceSpecification(roll);
      StringBuilder output = new StringBuilder();
      
      for(int i = 0; i < NUMBER_OF_TRIES; i++)
      {
        output.Append(String.Format("{0}\n", spec.RollOnce()));
      }

      writeResultsToFile(output.ToString(), filePath);
    }
    
    [Test]
    public void RollComplexGroup()
    {
      string roll = "d%+3-(2*1d4/(1d6))";
      DiceSpecification spec = new DiceSpecification(roll);
      
      Console.WriteLine("Input dice spec: '{0}'",
                        roll);
      Console.WriteLine("Parsed dice spec: '{0}'",
                        spec.ToString());
      Console.WriteLine("Min: {0}, Max: {1}, Mean (average): {2}",
                        spec.RollOnce(CalculationMethod.Minimum),
                        spec.RollOnce(CalculationMethod.Maximum),
                        spec.RollOnce(CalculationMethod.Mean));
      Console.WriteLine("Sample roll: {0}", spec.RollOnce());
    }
    
    [Test]
    public void RollWithRollAgain()
    {
      DiceSpecification spec = new DiceSpecification("9#d6");
      spec.RollAgainThreshold = 6;
      StringBuilder output = new StringBuilder();
      decimal[] results;
      
      for(int i = 0; i < NUMBER_OF_TRIES; i++)
      {
        results = spec.Roll();
        
        output.Append(String.Format("{0} results", results.Length));
        foreach(decimal result in results)
        {
          output.Append(String.Format(", {0}", result));
        }
        output.Append("\n");
      }
      
      writeResultsToFile(output.ToString(), FILE_PATH);
    }
    
    [Test]
    public void RollWithDiscardLowest()
    {
      string roll = "4d6";
      string strResult = String.Empty;
      DiceSpecification spec = new DiceSpecification(roll);
      
      spec.GetDice().DiscardLowest = 1;
      
      for(int i = 0; i < NUMBER_OF_TRIES; i++)
      {
        strResult += String.Format("{0}\n", spec.RollOnce().ToString());
      }
      
      writeResultsToFile(strResult, FILE_PATH);
    }
    
    [Test]
    public void RollWithMinMaxResults()
    {
      string roll = "2d10";
      string strResult = String.Empty;
      DiceSpecification spec = new DiceSpecification(roll);
      decimal result;
      
      spec.GetDice().RerollLowerThan = 2;
      spec.GetDice().RerollHigherThan = 8;
      
      for(int i = 0; i < NUMBER_OF_TRIES; i++)
      {
        result = spec.RollOnce();
        Assert.GreaterOrEqual(result, 4, "Minimum possible result");
        Assert.LessOrEqual(result, 16, "Maximum possible result");
        strResult += String.Format("{0}\n", result);
      }
      
      writeResultsToFile(strResult, FILE_PATH);
      
    }

    private void writeResultsToFile(string results, string path)
    {
      if(File.Exists(path))
      {
        File.Delete(path);
      }

      File.WriteAllText(path, results);
    }
  
    [Test]
    public void TestDiablo()
    {
      string roll = "6#3d4+6-1d6";
      DiceSpecification spec = new DiceSpecification(roll);
      foreach(decimal result in spec.Roll())
      {
        Console.WriteLine("Result: {0}", result);
//        Console.WriteLine("Average: {0}", spec.RollOnce(CalculationMethod.Mean));
      }
    }
  }
}
