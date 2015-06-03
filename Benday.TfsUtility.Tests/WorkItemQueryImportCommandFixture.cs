using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Benday.TfsUtility.Tests
{
    [TestClass]
    public class WorkItemQueryImportCommandFixture : UnitTestBase
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
            Args = null;
        }

        public string[] Args { get; set; }

        private WorkItemQueryImportCommand _SystemUnderTest;
        public WorkItemQueryImportCommand SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest =
                        new WorkItemQueryImportCommand(Args);
                }

                return _SystemUnderTest;
            }
        }

        public string QueryName
        {
            get
            {
                return "Uploaded Query";
            }
        }

        public string FolderPath
        {
            get
            {
                return "Shared Queries/Current Sprint";
            }
        }

        public string FilenameForQueryInWiqFormat
        {
            get
            {
                return @"C:\code\Repos\misc-201312\Benday.TfsUtility\SprintBacklog.wiq.xml";
            }
        }

        public string FilenameForQueryInTextFormat
        {
            get
            {
                return @"C:\code\Repos\misc-201312\Benday.TfsUtility\blocked-tasks-wiq.txt";
            }
        }

        [TestMethod]
        public void ValidateArgumentsSucceedsWithValidArguments()
        {
            InitializeArgsForWiqFormattedQueryFile();

            Assert.IsNotNull(SystemUnderTest);
        }

        private void InitializeArgsForWiqFormattedQueryFile()
        {
            Args = CreateArgsArray(
                TfsUtilityConstants.CommandArgumentNameWorkItemQueryExport,
                GetArgEntry(TfsUtilityConstants.ArgumentNameTfsCollection, TfsCollectionUrl),
                GetArgEntry(TfsUtilityConstants.ArgumentNameTeamProject, TeamProjectName),
                GetArgEntry(TfsUtilityConstants.ArgumentNameFolder, FolderPath),
                GetArgEntry(TfsUtilityConstants.ArgumentNameQueryName, QueryName),
                GetArgEntry(TfsUtilityConstants.ArgumentNameFilename, FilenameForQueryInWiqFormat));
        }

        private void InitializeArgsForTextFormattedQueryFile()
        {
            Args = CreateArgsArray(
                TfsUtilityConstants.CommandArgumentNameWorkItemQueryExport,
                GetArgEntry(TfsUtilityConstants.ArgumentNameTfsCollection, TfsCollectionUrl),
                GetArgEntry(TfsUtilityConstants.ArgumentNameTeamProject, TeamProjectName),
                GetArgEntry(TfsUtilityConstants.ArgumentNameFolder, FolderPath),
                GetArgEntry(TfsUtilityConstants.ArgumentNameQueryName, QueryName),
                GetArgEntry(TfsUtilityConstants.ArgumentNameFilename, FilenameForQueryInTextFormat));
        }

        [TestMethod]
        [ExpectedException(typeof(MissingArgumentException))]
        public void ValidateArgumentsFailsWithInvalidArgumentsNoArgs()
        {
            Args = CreateArgsArray();

            Assert.IsNotNull(SystemUnderTest);
        }

        [TestMethod]
        public void RawQueryTextIsPopulated()
        {
            InitializeArgsForTextFormattedQueryFile();

            string wiql = SystemUnderTest.RawQueryText;

            Console.WriteLine(wiql);

            Assert.IsFalse(String.IsNullOrWhiteSpace(wiql), "string was empty");
        }

        [TestMethod]
        public void QueryTextIsPopulatedForTextFormattedWiq()
        {
            InitializeArgsForTextFormattedQueryFile();

            string wiql = SystemUnderTest.QueryText;

            Console.WriteLine(wiql);

            Assert.IsFalse(String.IsNullOrWhiteSpace(wiql), "string was empty");
        }

        [TestMethod]
        public void QueryTextIsPopulatedForWiqFormattedWiq()
        {
            InitializeArgsForWiqFormattedQueryFile();

            string wiql = SystemUnderTest.QueryText;

            Console.WriteLine(wiql);

            Assert.IsFalse(String.IsNullOrWhiteSpace(wiql), "string was empty");
        }

        [TestMethod]
        public void QueryTextIsTheSameAsRawQueryTextForTextFormattedWiq()
        {
            InitializeArgsForTextFormattedQueryFile();

            Assert.AreEqual<string>(SystemUnderTest.RawQueryText, SystemUnderTest.QueryText, "Query text was wrong.");
        }

        [TestMethod]
        public void QueryTextIsNotTheSameAsRawQueryTextForWiqFormattedWiq()
        {
            InitializeArgsForWiqFormattedQueryFile();

            Assert.AreNotEqual<string>(SystemUnderTest.RawQueryText, SystemUnderTest.QueryText, "Query text was wrong.");
        }

        [TestMethod]
        public void ProjectNameVariableIsReplacedInQueryTextForWiqFormattedWiq()
        {
            InitializeArgsForWiqFormattedQueryFile();

            string projectNameVariable = "$$PROJECTNAME$$";
            string originalString = @"$$PROJECTNAME$$\Release 1\Sprint 1";
            string expectedReplacedString = originalString.Replace(projectNameVariable, TeamProjectName);

            Assert.IsTrue(SystemUnderTest.RawQueryText.Contains(projectNameVariable), 
                "The raw query text did not contain the expected variable.  Something is wrong with the setup for this test.");

            Assert.IsTrue(SystemUnderTest.RawQueryText.Contains(originalString),
                "The raw query text did not contain the expected original string.");

            Assert.IsFalse(SystemUnderTest.QueryText.Contains(projectNameVariable),
                "The query text should not contain the project name variable.");

            Assert.IsFalse(SystemUnderTest.QueryText.Contains(originalString),
                "The query text should not contain the original string.");

            Assert.IsTrue(SystemUnderTest.QueryText.Contains(expectedReplacedString),
                "The query text did not contain the expected replaced string.");
        }

        [TestMethod]
        public void QueryTextForWiqFormattedWiqShouldNotBeXml()
        {
            InitializeArgsForWiqFormattedQueryFile();

            Console.WriteLine(SystemUnderTest.QueryText);

            Assert.IsTrue(SystemUnderTest.QueryText.Trim().StartsWith("SELECT [System.Id], [System.Title]"), 
                "Query text should start with a select statement.");
        }

        [TestMethod]
        public void UploadWorkItemQuery()
        {
            InitializeArgsForWiqFormattedQueryFile();

            SystemUnderTest.UploadQuery();
        }

        [TestMethod]
        public void QueryNameIsPopulated()
        {
            InitializeArgsForWiqFormattedQueryFile();

            Assert.AreEqual<string>(QueryName, SystemUnderTest.QueryName, "Query name was wrong.");
        }

        [TestMethod]
        public void FilenameIsPopulated()
        {
            InitializeArgsForWiqFormattedQueryFile();

            Assert.AreEqual<string>(FilenameForQueryInWiqFormat, SystemUnderTest.Filename, "Filename was wrong.");
        }

        [TestMethod]
        public void WorkItemQueryFolderIsPopulated()
        {
            InitializeArgsForWiqFormattedQueryFile();

            Assert.IsNotNull(SystemUnderTest.WorkItemQueryFolder);

            Console.WriteLine(SystemUnderTest.WorkItemQueryFolder.Path);

            Assert.IsTrue(SystemUnderTest.WorkItemQueryFolder.Path.EndsWith(FolderPath), "Work item query folder was wrong.");
        }
    }
}
