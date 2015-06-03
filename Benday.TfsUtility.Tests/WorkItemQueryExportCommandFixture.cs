using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Benday.TfsUtility.Tests
{

    [TestClass]
    public class WorkItemQueryExportCommandFixture : UnitTestBase
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
            Args = null;
        }

        public string[] Args { get; set; }

        private WorkItemQueryExportCommand _SystemUnderTest;
        public WorkItemQueryExportCommand SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest =
                        new WorkItemQueryExportCommand(Args);
                }

                return _SystemUnderTest;
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

            Assert.IsFalse(String.IsNullOrWhiteSpace(wiql), "string was empty");
        }
    }
}
