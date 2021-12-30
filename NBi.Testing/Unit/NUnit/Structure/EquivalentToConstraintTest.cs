using System.Collections.Generic;
using Moq;
using NBi.Core.Structure;
using NBi.NUnit.Structure;
using NUnit.Framework;
using NBi.Core.Structure.Olap;

namespace NBi.Testing.Unit.NUnit.Structure
{
    [TestFixture]
    public class EquivalentToConstraintTest
    {
        [Test]
        public void WriteTo_FailingAssertionForListOfLevels_TextContainsFewKeyInfo()
        {
            var exp = new string[] { "Expected level 1", "Expected level 2" };
            
            var description = new CommandDescription(
                        Target.Levels,
                        new CaptionFilter[]
                            {
                                new CaptionFilter(Target.Perspectives, "perspective-name")
                                , new CaptionFilter(Target.Dimensions, "dimension-caption")
                                , new CaptionFilter(Target.Hierarchies, "hierarchy-caption" )
                        });


            var actuals = new string[] { "Actual level 1", "Actual level 2", "Actual level 3" };

            var commandStub = new Mock<IStructureDiscoveryCommand>();
            commandStub.Setup(cmd => cmd.Execute()).Returns(actuals);
            commandStub.Setup(cmd => cmd.Description).Returns(description);

            var containsConstraint = new EquivalentToConstraint(exp);

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(commandStub.Object, containsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion            
            Assert.That(assertionText, Does.Contain("exact").And
                                            .Contain("perspective-name").And
                                            .Contain("dimension-caption").And
                                            .Contain("hierarchy-caption").And
                                            .Contain("levels").And
                                            .Contain("Expected level 1").And
                                            .Contain("Expected level 2"));
        }


        

       
    }
}
