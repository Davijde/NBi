#region Using directives
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NUnit.Framework;
using System.Linq;

#endregion

namespace NBi.Testing.Integration.Core.Analysis
{
    [TestFixture]
    [Category ("Olap")]
    public class MembersAdomdEngineTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [OneTimeSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [OneTimeTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        [Test]
        [Category("Olap")]
        public void GetMembers_ByLevel_ReturnListMembersWithCorrectCaptions()
        {
            //Buiding object used during test
            var mae = new MembersAdomdEngine();
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                string.Empty,
                "Adventure Works",
                "Geography",
                "Geography",
                "Country"
                );

            //Call the method to test
            var actual = mae.GetMembers(disco);

            ////Assertion
            Assert.That(actual.Select(x => x.Caption), Has.None.EqualTo("All"));
            Assert.That(actual.Select(x => x.Caption), Has.Member("Canada"));
            Assert.That(actual.Select(x => x.Caption), Has.Member("France"));
        }

        [Test]
        public void GetMembers_ByLevel_ReturnListMembersWithCorrectUniqueName()
        {
            //Buiding object used during test
            var mae = new MembersAdomdEngine();
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                string.Empty,
                "Adventure Works",
                "Geography",
                "Geography",
                "Country"
                );

            //Call the method to test
            var actual = mae.GetMembers(disco);

            //Assertion
            Assert.That(actual.Select(x => x.UniqueName), Has.Member("[Geography].[Geography].[Country].&[Canada]"));
            Assert.That(actual.Select(x => x.UniqueName), Has.Member("[Geography].[Geography].[Country].&[France]"));
        }

        [Test]
        public void GetMembers_ByLevel_ReturnListMembersWithCorrectOrdinal()
        {
            var mae = new MembersAdomdEngine();
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                string.Empty,
                "Adventure Works",
                "Geography",
                "Geography",
                "Country"
                );

            //Call the method to test
            var actual = mae.GetMembers(disco);

            //Assertion
            Assert.That(actual.Select(x => x.Ordinal), Is.Unique);
            Assert.That(actual.Select(x => x.Ordinal), Has.All.GreaterThan(0));
        }

        [Test]
        public void GetMembers_ByLevel_ReturnListMembersWithCorrectLevelNumber()
        {
            var mae = new MembersAdomdEngine();
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                string.Empty,
                "Adventure Works",
                "Geography",
                "Geography",
                "Country"
                );

            //Call the method to test
            var actual = mae.GetMembers(disco);

            //Assertion
            Assert.That(actual.Select(x => x.LevelNumber), Has.All.EqualTo(1));
        }

        [Test]
        public void GetMembers_ByHierarchy_ReturnListMembersWithCorrectCaptions()
        {
            //Buiding object used during test
            var mae = new MembersAdomdEngine();
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                string.Empty,
                "Adventure Works",
                "Geography",
                "Geography",
                null
                );

            //Call the method to test
            var actual = mae.GetMembers(disco);

            //Assertion
            Assert.That(actual.Select(x => x.Caption), Has.Member("All Geographies"));
            Assert.That(actual.Select(x => x.Caption), Has.Member("Canada"));
            Assert.That(actual.Select(x => x.Caption), Has.Member("France"));
        }

        [Test]
        public void GetMembers_ByHierarchy_ReturnListMembersWithCorrectUniqueName()
        {
            //Buiding object used during test
            var mae = new MembersAdomdEngine();
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                string.Empty,
                "Adventure Works",
                "Geography",
                "Geography",
                null
                );

            //Call the method to test
            var actual = mae.GetMembers(disco);

            ////Assertion
            Assert.That(actual.Select(x => x.UniqueName), Has.Member("[Geography].[Geography].[Country].&[Canada]"));
            Assert.That(actual.Select(x => x.UniqueName), Has.Member("[Geography].[Geography].[City].&[Toronto]&[ON]"));
        }

        [Test]
        public void GetMembers_ByHierarchy_ReturnListMembersWithCorrectOrdinal()
        {
            //Buiding object used during test
            var mae = new MembersAdomdEngine();
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                string.Empty,
                "Adventure Works",
                "Geography",
                "Geography",
                null
                );

            //Call the method to test
            var actual = mae.GetMembers(disco);

            ////Assertion
            Assert.That(actual, Has.Count.GreaterThan(0));
            Assert.That(actual.Select(x => x.Ordinal), Is.Unique);
            Assert.That(actual.Select(x => x.Ordinal), Has.All.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void GetMembers_ByHierarchy_ReturnListMembersWithCorrectLevelNumber()
        {
            //Buiding object used during test
            var mae = new MembersAdomdEngine();
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomd(),
                string.Empty,
                "Adventure Works",
                "Geography",
                "Geography",
                null
                );

            //Call the method to test
            var actual = mae.GetMembers(disco);

            ////Assertion
            Assert.That(actual, Has.Count.GreaterThan(0));
            //0 = All
            Assert.That(actual.Select(x => x.LevelNumber), Has.Some.EqualTo(0));
            //1 = Country
            Assert.That(actual.Select(x => x.LevelNumber), Has.Some.EqualTo(1));
            //2 = State/Province
            Assert.That(actual.Select(x => x.LevelNumber), Has.Some.EqualTo(2));
            //3 = Town
            Assert.That(actual.Select(x => x.LevelNumber), Has.Some.EqualTo(3));
            //4 = Zip code
            Assert.That(actual.Select(x => x.LevelNumber), Has.Some.EqualTo(4));
            //Nothing else 
            Assert.That(actual.Select(x => x.LevelNumber), Has.All.LessThanOrEqualTo(4));

        }

        [Test]
        public void GetMembersDax_ByLevel_ReturnListMembersWithCorrectCaptions()
        {
            //Buiding object used during test
            var mae = new MembersAdomdEngine();
            var disco = new DiscoveryRequestFactory().Build(
                ConnectionStringReader.GetAdomdTabular(),
                string.Empty,
                "Internet Operation",
                "Geography",
                "Geography",
                "Country Region"
                );

            //Call the method to test
            var actual = mae.GetMembers(disco);

            ////Assertion
            Assert.That(actual.Select(x => x.Caption), Has.None.EqualTo("All"));
            Assert.That(actual.Select(x => x.Caption), Has.Member("Canada"));
            Assert.That(actual.Select(x => x.Caption), Has.Member("France"));
        }
    }
}
