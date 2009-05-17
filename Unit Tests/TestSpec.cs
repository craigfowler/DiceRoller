/*
 * TestResult Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;

namespace CraigFowler.Test.Diceroller
{
  public class TestSpec
  {
    public string DiceSpecification, StringResult;
    public decimal NumericResult;
    public bool ExpectedException;
    
    public TestSpec()
    {
      this.DiceSpecification = String.Empty;
      this.NumericResult = 0m;
      this.ExpectedException = false;
      this.StringResult = String.Empty;
    }
    
    public TestSpec(string spec, decimal result)
    {
      this.DiceSpecification = spec;
      this.NumericResult = result;
      this.ExpectedException = false;
      this.StringResult = String.Empty;
    }
    
    public TestSpec(string spec)
    {
      this.DiceSpecification = spec;
      this.NumericResult = 0m;
      this.ExpectedException = true;
      this.StringResult = String.Empty;
    }
    
    public TestSpec(string spec, string result)
    {
      this.DiceSpecification = spec;
      this.NumericResult = 0m;
      this.ExpectedException = true;
      this.StringResult = result;
    }
  }
}
