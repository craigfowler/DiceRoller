
using System;

namespace CraigFowler.Gaming.Diceroller.DomainObjects
{
  public enum CalculationMethod : int
  {
    Roll,
    Minimum,
    Maximum,
    Mean,
    HighestDiceRolls,
    LowestDiceRolls
  }
}
