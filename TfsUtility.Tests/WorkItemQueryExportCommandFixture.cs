using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TfsUtility.Tests
{

    [TestClass]
    public class WorkItemQueryExportCommandFixture : UnitTestBase
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _systemUnderTest = null;
            Args = null;
        }

        public string[] Args { get; set; }

        private WorkItemQueryExportCommand _systemUnderTest;
        public WorkItemQueryExportCommand SystemUnderTest
        {
            get
            {
                if (_systemUnderTest == null)
                {
                    _systemUnderTest =
                        new WorkItemQueryExportCommand(Args);
                }

                return _systemUnderTest;
            }
        }

        public string QueryName
        {
            get
            {
                return "Shared Queries/Current Sprint/Work in Progress";
            }
        }

        [TestMethod]
        public void ValidateArgumentsSucceedsWithValidArguments()
        {
            Args = CreateArgsArray(
                TfsUtilityConstants.CommandArgumentNameWorkItemQueryExport,
                GetArgEntry(TfsUtilityConstants.ArgumentNameTfsCollection, TfsCollectionUrl),
                GetArgEntry(TfsUtilityConstants.ArgumentNameTeamProject, TeamProjectName),
                GetArgEntry(TfsUtilityConstants.ArgumentNameQuery, QueryName));

            Assert.IsNotNull(SystemUnderTest);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingArgumentException))]
        public void ValidateArgumentsFailsWithInvalidArgumentsNoArgs()
        {
            Args = CreateArgsArray();

            Assert.IsNotNull(SystemUnderTest);
        }

        [TestMethod]
        public void GetResult()
        {
            Args = CreateArgsArray(
                TfsUtilityConstants.CommandArgumentNameWorkItemQueryExport,
                GetArgEntry(TfsUtilityConstants.ArgumentNameTfsCollection, TfsCollectionUrl),
                GetArgEntry(TfsUtilityConstants.ArgumentNameTeamProject, TeamProjectName),
                GetArgEntry(TfsUtilityConstants.ArgumentNameQuery, QueryName));

            string wiql = SystemUnderTest.GetResult();

            Console.WriteLine(wiql);

            Assert.IsFalse(string.IsNullOrWhiteSpace(wiql), "string was empty");
        }
    }
}
