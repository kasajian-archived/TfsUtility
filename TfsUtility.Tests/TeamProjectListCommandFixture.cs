using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TfsUtility.Tests
{
    [TestClass]
    public class TeamProjectListCommandFixture : UnitTestBase
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _systemUnderTest = null;
            Args = null;
        }
        public string[] Args { get; set; }

        private TeamProjectListCommand _systemUnderTest;
        public TeamProjectListCommand SystemUnderTest
        {
            get
            {
                if (_systemUnderTest == null)
                {
                    _systemUnderTest =
                        new TeamProjectListCommand(Args);
                }

                return _systemUnderTest;
            }
        }
        [TestMethod]
        public void ValidateArgumentsSucceedsWithValidArguments()
        {
            Args = CreateArgsArray(
                TfsUtilityConstants.CommandArgumentNameListWorkItemQueryFolders,
                GetArgEntry(TfsUtilityConstants.ArgumentNameTfsCollection, TfsCollectionUrl));

            Assert.IsNotNull(SystemUnderTest);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingArgumentException))]
        public void ValidateArgumentsFailsWithInvalidArguments()
        {
            Args = CreateArgsArray();

            Assert.IsNotNull(SystemUnderTest);
        }

        [TestMethod]
        public void GetResult()
        {
            Args = CreateArgsArray(
                TfsUtilityConstants.CommandArgumentNameListWorkItemQueryFolders,
                GetArgEntry(TfsUtilityConstants.ArgumentNameTfsCollection, TfsCollectionUrl));

            List<string> projects = SystemUnderTest.GetResult();

            AssertContainsProject(projects, "scrum-test-20131023");
        }
        
        private void AssertContainsProject(List<string> actualProjects, string expectedProjectName)
        {
            Assert.IsTrue(actualProjects.Contains(expectedProjectName),
                $"'{expectedProjectName}' was not in the result.");
        }
    }
}
