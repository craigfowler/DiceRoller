
using System;
using CraigFowler;
using CraigFowler.Gaming.Diceroller.DomainObjects;
using CraigFowler.Gaming.Diceroller.Plugins;

namespace CraigFowler.Gaming.Diceroller.Commandline
{
  public static class EntryPoint
  {
    public static void Main(string[] commandLine)
    {
      CommandlineParameterProcessor parameters = new CommandlineParameterProcessor(ParameterStyle.Unix, commandLine);
      addMainParameterDefinitions(parameters);
      addPluginParameterDefinitions(parameters);
      string output;
      
      if(parameters.Parameters.ContainsKey("plugin"))
      {
        switch(parameters.Parameters["plugin"])
        {
        case "sr4":
          output = rollShadowrun4Dice(parameters);
          break;
        case "dndStats":
          output = rollDnDStatsDice(parameters);
          break;
        case "fudge":
          output = rollFudgeDice(parameters);
          break;
        default:
          throw new NotSupportedException("Unsupported plugin");
        }
      }
      else
      {
        output = rollNormalDice(parameters);
      }
      
      Console.WriteLine(output);
    }
    
    private static void addMainParameterDefinitions(CommandlineParameterProcessor parameters)
    {
      ParameterDefinition def;
      
      def = new ParameterDefinition("exploding-threshold",
                                    new string[] {"exploding-threshold"},
                                    new string[] {"E"});
      def.Type = ParameterType.ValueRequired;
      parameters.Definitions.Add(def);
      
      def = new ParameterDefinition("reroll-if-higher-than",
                                    new string[] {"reroll-if-higher-than"},
                                    new string[] {"H"});
      def.Type = ParameterType.ValueRequired;
      parameters.Definitions.Add(def);
      
      def = new ParameterDefinition("reroll-if-lower-than",
                                    new string[] {"reroll-if-lower-than"},
                                    new string[] {"L"});
      def.Type = ParameterType.ValueRequired;
      parameters.Definitions.Add(def);
      
      def = new ParameterDefinition("roll-again-threshold",
                                    new string[] {"roll-again-threshold"},
                                    new string[] {"R"});
      def.Type = ParameterType.ValueRequired;
      parameters.Definitions.Add(def);
      
      def = new ParameterDefinition("calculation-method",
                                    new string[] {"calculation-method"},
                                    new string[] {"C"});
      def.Type = ParameterType.ValueRequired;
      parameters.Definitions.Add(def);
      
      def = new ParameterDefinition("plugin",
                                    new string[] {"plugin"},
                                    new string[] {"P"});
      def.Type = ParameterType.ValueRequired;
      parameters.Definitions.Add(def);
    }
    
    private static void addPluginParameterDefinitions(CommandlineParameterProcessor parameters)
    {
      ParameterDefinition def;
      
      def = new ParameterDefinition("dnd-reroll-less-than",
                                    new string[] {"dnd-reroll-less-than"});
      def.Type = ParameterType.ValueRequired;
      parameters.Definitions.Add(def);
      
      def = new ParameterDefinition("sr4-edge-used",
                                    new string[] {"sr4-edge-used"});
      def.Type = ParameterType.ValueRequired;
      parameters.Definitions.Add(def);
      
      def = new ParameterDefinition("sr4-gremlins",
                                    new string[] {"sr4-gremlins"});
      def.Type = ParameterType.ValueRequired;
      parameters.Definitions.Add(def);
    }
    
    private static string rollShadowrun4Dice(CommandlineParameterProcessor parameters)
    {
      Shadowrun4 roller = new Shadowrun4();
      
      if(parameters.Parameters.ContainsKey("sr4-edge-used"))
      {
        roller.EdgeUsed = Int32.Parse(parameters.Parameters["sr4-edge-used"]);
      }
      
      if(parameters.Parameters.ContainsKey("sr4-gremlins"))
      {
        roller.Gremlins = Int32.Parse(parameters.Parameters["sr4-gremlins"]);
      }
      
      if(parameters.RemainingText.Length == 1)
      {
        roller.DicePool = Int32.Parse(parameters.RemainingText[0]);
      }
      else
      {
        throw new ArgumentOutOfRangeException("parameters", "Incorrect number of parameters");
      }
      
      return roller.ToString();
    }
    
    private static string rollDnDStatsDice(CommandlineParameterProcessor parameters)
    {
      DnD3e roller = new DnD3e();
      
      if(parameters.Parameters.ContainsKey("dnd-reroll-less-than"))
      {
        roller.DieRerollThreshold = Int32.Parse(parameters.Parameters["dnd-reroll-less-than"]);
      }
      
      if(parameters.RemainingText.Length == 1 && parameters.RemainingText[0] == "3d6")
      {
        roller.RollingMethod = DnD3eRollingMethod.ThreeD6;
      }
      else
      {
        roller.RollingMethod = DnD3eRollingMethod.FourD6DropLowest;
      }
      
      return roller.ToString();
    }
    
    private static string rollFudgeDice(CommandlineParameterProcessor parameters)
    {
      FudgeDice roller;
      
      if(parameters.RemainingText.Length == 1)
      {
        roller = new FudgeDice(Int32.Parse(parameters.RemainingText[0]));
      }
      else
      {
        throw new ArgumentOutOfRangeException("parameters", "Incorrect number of parameters");
      }
      
      roller.Roll();
      
      return roller.ResultText;
    }
    
    private static string rollNormalDice(CommandlineParameterProcessor parameters)
    {
      CalculationMethod method = CalculationMethod.Roll;
      DiceSpecification roller = new DiceSpecification();
      decimal[] results;
      string output = String.Empty;
      
      if(parameters.Parameters.ContainsKey("exploding-threshold"))
      {
        roller.ExplodingThreshold = Int32.Parse(parameters.Parameters["exploding-threshold"]);
      }
      
      if(parameters.Parameters.ContainsKey("reroll-if-higher-than"))
      {
        roller.RerollIfHigherThan = Int32.Parse(parameters.Parameters["reroll-if-higher-than"]);
      }
      
      if(parameters.Parameters.ContainsKey("reroll-if-lower-than"))
      {
        roller.RerollIfLowerThan = Int32.Parse(parameters.Parameters["reroll-if-lower-than"]);
      }
      
      if(parameters.Parameters.ContainsKey("roll-again-threshold"))
      {
        roller.RollAgainThreshold = Int32.Parse(parameters.Parameters["roll-again-threshold"]);
      }
      
      if(parameters.Parameters.ContainsKey("calculation-method"))
      {
        switch(parameters.Parameters["calculation-method"])
        {
        case "high-dice":
          method = CalculationMethod.HighestDiceRolls;
          break;
        case "low-dice":
          method = CalculationMethod.LowestDiceRolls;
          break;
        case "max":
          method = CalculationMethod.Maximum;
          break;
        case "min":
          method = CalculationMethod.Minimum;
          break;
        case "mean":
          method = CalculationMethod.Mean;
          break;
        default:
          method = CalculationMethod.Roll;
          break;
        }
      }
      
      if(parameters.RemainingText.Length == 1)
      {
        roller.SpecificationString = parameters.RemainingText[0];
      }
      else
      {
        throw new ArgumentOutOfRangeException("parameters", "Incorrect number of parameters");
      }
      
      results = roller.Roll(method);
      if(results.Length > 0)
      {
        output = results[0].ToString();
      }
      
      for(int i = 0; i < results.Length; i++)
      {
        if(i == 0)
        {
          output = results[0].ToString();
        }
        else
        {
          output += String.Format(", {0}", results[i].ToString());
        }
      }
      
      return String.Format("{0} = [{1}]",
                           roller.ToString(),
                           output);
    }
  }
}
