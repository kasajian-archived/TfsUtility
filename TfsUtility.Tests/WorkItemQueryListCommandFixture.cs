using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TfsUtility.Tests
{

    [TestClass]
    public class WorkItemQueryListCommandFixture : UnitTestBase
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _systemUnderTest = null;
            Args = null;
        }
        public string[] Args { get; set; }

        private WorkItemQueryListCommand _systemUnderTest;
        public WorkItemQueryListCommand SystemUnderTest
        {
            get
            {
                if (_systemUnderTest == null)
                {
                    _systemUnderTest =
                        new WorkItemQueryListCommand(Args);
                }

                return _systemUnderTest;
            }
        }
        [TestMethod]
        public void ValidateArgumentsSucceedsWithValidArguments()
        {
            Args = CreateArgsArray(
                TfsUtilityConstants.CommandArgumentNameListWorkItemQueryFolders,
                GetArgEntry(TfsUtilityConstants.ArgumentNameTfsCollection, TfsCollectionUrl),
                GetArgEntry(TfsUtilityConstants.ArgumentNameTeamProject, TeamProjectName));

            Assert.IsNotNull(SystemUnderTest);
        }

        [TestMethod]
        public void ValidateArgumentsSucceedsWithValidArgumentsPlusFilter()
        {
            Args = CreateArgsArray(
                TfsUtilityConstants.CommandArgumentNameListWorkItemQueryFolders,
                GetArgEntry(TfsUtilityConstants.ArgumentNameTfsCollection, TfsCollectionUrl),
                GetArgEntry(TfsUtilityConstants.ArgumentNameTeamProject, TeamProjectName));

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
        public void GetResultForNoFilter()
        {
            Args = CreateArgsArray(
                TfsUtilityConstants.CommandArgumentNameListWorkItemQueryFolders,
                GetArgEntry(TfsUtilityConstants.ArgumentNameTfsCollection, TfsCollectionUrl),
                GetArgEntry(TfsUtilityConstants.ArgumentNameTeamProject, TeamProjectName));

            List<string> folders = SystemUnderTest.GetResult();

            AssertDoesNotContainFolders(folders);

            AssertContainsQuery(folders, "scrum-test-20131023/Shared Queries/Current Sprint/Work in Progress");
            AssertContainsQuery(folders, "scrum-test-20131023/Shared Queries/Current Sprint/Unfinished Work");
            AssertContainsQuery(folders, "scrum-test-20131023/Shared Queries/Feedback");
            AssertContainsQuery(folders, "scrum-test-20131023/Shared Queries/Product Backlog");
        }

        [TestMethod]
        public void GetResultForFilterOnSingleSubirectory()
        {
            Args = CreateArgsArray(
                TfsUtilityConstants.CommandArgumentNameListWorkItemQueryFolders,
                GetArgEntry(TfsUtilityConstants.ArgumentNameTfsCollection, TfsCollectionUrl),
                GetArgEntry(TfsUtilityConstants.ArgumentNameTeamProject, TeamProjectName),
                GetArgEntry(TfsUtilityConstants.ArgumentNameFolderFilter, "Shared Queries/Current Sprint"));

            List<string> folders = SystemUnderTest.GetResult();

            if (folders != null) folders.ForEach(x => Console.WriteLine(x));

            Assert.AreNotEqual<int>(0, folders.Count, "Folder count is wrong.");

            AssertDoesNotContainFolders(folders);
            AssertDoesNotContainQuery(folders, "scrum-test-20131023/Shared Queries/Product Backlog");
            AssertContainsQuery(folders, "scrum-test-20131023/Shared Queries/Current Sprint/Work in Progress");
            AssertContainsQuery(folders, "scrum-test-20131023/Shared Queries/Current Sprint/Unfinished Work");
        }

        private void AssertDoesNotContainFolders(List<string> folders)
        {
            AssertDoesNotContainQuery(folders, "scrum-test-20131023/Shared Queries");
            AssertDoesNotContainQuery(folders, "scrum-test-20131023/Shared Queries/Current Sprint");            
        }
                
        [TestMethod]
        public void GetResultForFilterOnFolderThatDoesntExistReturnsZeroResults()
        {
            Args = CreateArgsArray(
                TfsUtilityConstants.CommandArgumentNameListWorkItemQueryFolders,
                GetArgEntry(TfsUtilityConstants.ArgumentNameTfsCollection, TfsCollectionUrl),
                GetArgEntry(TfsUtilityConstants.ArgumentNameTeamProject, TeamProjectName),
                GetArgEntry(TfsUtilityConstants.ArgumentNameFolderFilter, "bogus folder name"));

            List<string> folders = SystemUnderTest.GetResult();

            Assert.IsNotNull(folders, "Collection should not be null.");

            Assert.AreEqual<int>(0, folders.Count, "Count is wrong.");
        }

        private void AssertContainsQuery(List<string> actualQueryPaths, string expectedQueryPath)
        {
            Assert.IsTrue(actualQueryPaths.Contains(expectedQueryPath),
                $"'{expectedQueryPath}' was not in the result.");
        }

        private void AssertDoesNotContainQuery(List<string> actualQueryPaths, string expectedQueryPath)
        {
            Assert.IsFalse(actualQueryPaths.Contains(expectedQueryPath),
                $"'{expectedQueryPath}' was in the result.");
        }
    }
}
