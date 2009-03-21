/*
 * SpecificationParsing Created on 21/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using NUnit.Framework;

namespace Test.CraigFowler.Diceroller
{
  [Ignore("This test isn't ready yet")]
  [TestFixture]
  public class SpecificationParsing
  {
    /* Things I need to test:
     * Several invalid dice specifications.  Can I think of any that would go
     * wrong and would totally prevent the diceroller from being able to do
     * its job?
     * 
     * Actual cases to test (and how the test should behave)
     * * Too many opening brackets
     * > Any open brackets are implicitly closed at the end of the string
     * 
     * * Too many closing brackets
     * > Any extra ones are ignored/discarded
     * 
     * * Adjacent operation symbols
     * > Only the first operation encountered is used, the rest are discarded
     * 
     * * Multiplication/Division at the start of the string
     * > Throw an exception with an error message
     * 
     * * Number followed immediately by an open-bracket (without an operation)
     * > Throw an exception with an error message
     * 
     * * Invalid characters in the specification
     * > Discard them
     */
    [Test]
    public void TestCase()
    {
    }
  }
}
