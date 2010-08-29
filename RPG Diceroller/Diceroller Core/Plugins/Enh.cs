
using System;
using CraigFowler.Gaming.Diceroller.DomainObjects;
using System.Collections.Generic;

namespace CraigFowler.Gaming.Diceroller.Plugins
{
  public class Enh
  {
    public int RollEnh(int dice, out bool fumble, out bool threeSixes)
    {
      int remainingDice = dice - 1, chaosResult, output = 0, sixes = 0;
      decimal[] remainingRolls;
      List<decimal> results = new List<decimal>();
      bool firstRoll = true;
      
      if(dice < 1)
      {
        throw new ArgumentOutOfRangeException("dice", "Cannot roll zero dice");
      }
      
      fumble = false;
      remainingRolls = DiceSpecification.Roll(String.Format("{0}#d6", remainingDice));
      results.AddRange(remainingRolls);
      
      do
      {
        chaosResult = (int) DiceSpecification.RollOnce("1d6");
        
        if(firstRoll)
        {
          fumble = (chaosResult == 1);
          firstRoll = false;
        }
        
        results.Add(chaosResult);
      }
      while(chaosResult == 6);
      
      foreach(decimal result in results)
      {
        output += (int) result;
        
        if(result == 6)
        {
          sixes ++;
        }
      }
      
      threeSixes = (sixes >= 3);
      
      return output;
    }
  }
}
