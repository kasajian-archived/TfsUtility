using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Benday.TfsUtility.Tests
{
    [TestClass]
    public class TeamProjectListCommandFixture : UnitTestBase
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
            Args = null;
        }
        public string[] Args { get; set; }

        private TeamProjectListCommand _SystemUnderTest;
        public TeamProjectListCommand SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest =
                        new TeamProjectListCommand(Args);
                }

                return _SystemUnderTest;
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
                String.Format("'{0}' was not in the result.", expectedProjectName));
        }
    }
}
