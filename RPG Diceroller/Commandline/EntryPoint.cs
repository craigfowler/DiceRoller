
using System;
using CraigFowler;
using CraigFowler.Gaming.Diceroller.DomainObjects;
using CraigFowler.Gaming.Diceroller.Plugins;

namespace CraigFowler.Gaming.Diceroller.Commandline
{
  public static class EntryPoint
  {
    #region constants
    
    private const string
      USAGE_STATEMENT = @"
-------------------------------------------------------------------------------
Diceroller.exe                                                            Usage
-------------------------------------------------------------------------------

Diceroller.exe [options] dice-specification

Core options
------------
  -P,--plugin                 Optional, the diceroller plugin to use (for
                              specialist dice rolls).  May be 'sr4', 'dndStats'
                              or 'fudge' or 'enh'.

  -C,--calculation-method     Optional, the alternative calculation method to
                              use for this roll.  May be 'max', 'min', 'mean',
                              'high-dice' (max results on all dice, might not
                              be the same as 'max') or 'low-dice'.

  -R,--roll-again-threshold   Optional, if the results of an individual roll
                              are equal to or higher than this amount then
                              an extra roll will me made, increasing the number
                              of results returned.

  -L,--reroll-if-lower-than   Optional, if an individual roll is lower than
                              this amount then the roll will be discarded and
                              attempted again.

  -H,--reroll-if-higher-than  Optional, if an individual roll is higher than
                              this amount then the roll will be discarded and
                              attempted again.

  -E,--exploding-threshold    Optional, any roll that is higher than or equal
                              to this amount is rolled again, with the new
                              result added on to the previous result, creating
                              a cumulative result whilst successive rolls meet
                              this threshold.

Plugin options
--------------
  --dnd-reroll-less-than      Optional when using the 'dndStats' plugin.
                              Rerolls any individual die that comes up with
                              this result or less.  May be 1-5.  Default is no
                              rerolling.

  --sr4-edge-used             Optional when using the 'sr4' plugin.  Indicates
                              the use of edge when appropriate.  This amount
                              should be equal to the character's edge
                              attribute.  This is added on automatically to the
                              dice pool when making the roll.

  --sr4-gremlins              Optional when using the 'sr4' plugin.  Indicates
                              the gremlins nagative quality where appropriate.
                              May be between 1 and 4, depending how many
                              gremlins the character has.

Dice specifications
-------------------
  When not using any plugin, dice specifications can be quite freeform.  It
  might be useful to enclose the specification in quotes to avoid any
  unexpected results.  Most who are familiar with dice rolling will understand
  the notation:

  2d4+4                       Rolls two 4-sided dice and adds 4 to the final
                              result.

  d%                          Abbreviation for d100.

  (6d6-2d12)/(d8+2)           Parentheses, subtraction, multiplication and
                              division all work too.

  When using the 'sr4' plugin for rolling Shadowrun 4th edition dice, the dice
  specification is just a number, indicating the character's dice pool.  If
  edge is being used then DO NOT add it on here.  It will be added in
  automatically.

  When the 'fudge' plugin is being used, this is number of 'fudge dice' to
  roll, usually 4.

  When the 'dndStats' plugin is being used then the dice specification is
  optional.  If it is present then it must be '3d6', which forces the
  diceroller to use the classic '3d6' method of generating attributes, instead
  of the '4d6, drop lowest' method (which is used by default).
";
    
    #endregion
    
    #region entry point
    
    public static void Main(string[] commandLine)
    {
      CommandlineParameterProcessor parameters = new CommandlineParameterProcessor(ParameterStyle.Unix, commandLine);
      
      addMainParameterDefinitions(parameters);
      addPluginParameterDefinitions(parameters);
      
      try
      {
        Console.WriteLine(rollDice(parameters));
      }
      catch(Exception)
      {
        Console.Error.WriteLine(USAGE_STATEMENT);
      }
    }
    
    #endregion
    
    #region private methods
    
    private static string rollDice(CommandlineParameterProcessor parameters)
    {
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
        case "enh":
          output = rollEnhDice(parameters);
          break;
        default:
          throw new NotSupportedException("Unsupported plugin");
        }
      }
      else
      {
        output = rollNormalDice(parameters);
      }
      
      return output;
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
          throw new InvalidOperationException("Unexpected calculation method");
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
    
    private static string rollEnhDice(CommandlineParameterProcessor parameters)
    {
      Enh roller;
      int result;
      bool fumble, threeSixes;
      
      if(parameters.RemainingText.Length == 1)
      {
        roller = new Enh();
        result = roller.RollEnh(Int32.Parse(parameters.RemainingText[0]), out fumble, out threeSixes);
      }
      else
      {
        throw new ArgumentOutOfRangeException("parameters", "Incorrect number of parameters.");
      }
      
      return String.Format("Total result: {0}\nFumble:       {1}\nThree sixes:  {2}\n", result, fumble, threeSixes);
    }
    
    #endregion
  }
}
