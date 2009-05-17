/*
 * Shadowrun4Roller Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using CraigFowler.Diceroller;

namespace CraigFowler.Diceroller.Plugins
{
  public class Shadowrun4Roller : BasicRoller
  {
    public Shadowrun4Roller() : base()
    { }
    
    public new int Roll(string diceSpec)
    {
      throw new NotImplementedException();
    }
    
    public int Roll(string diceSpec,
                    out Shadowrun4Glitch glitch)
    {
      throw new NotImplementedException();
    }
  }
}
