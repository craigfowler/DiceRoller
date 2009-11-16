
using System;
using CraigFowler;

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
      // TODO: Write method for rolling SR4 dice
      throw new NotImplementedException();
    }
    
    private static string rollDnDStatsDice(CommandlineParameterProcessor parameters)
    {
      // TODO: Write method for rolling SR4 dice
      throw new NotImplementedException();
    }
    
    private static string rollFudgeDice(CommandlineParameterProcessor parameters)
    {
      // TODO: Write method for rolling SR4 dice
      throw new NotImplementedException();
    }
    
    private static string rollNormalDice(CommandlineParameterProcessor parameters)
    {
      // TODO: Write method for rolling SR4 dice
      throw new NotImplementedException();
    }
  }
}
