/*
 * FudgeRoller Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using CraigFowler.Diceroller;

namespace CraigFowler.Diceroller.Plugins
{
  public class FudgeRoller : BasicRoller
  {
    public FudgeRoller() : base()
    { }
    
    public new int Roll(string diceSpec)
    {
      throw new NotImplementedException();
    }
    
    public FudgeRating Roll(string diceSpec,
                            FudgeRating baseRating)
    {
      throw new NotImplementedException();
    }
  }
}
