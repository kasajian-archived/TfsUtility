using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsUtility
{
    public class WorkItemQueryImportCommand : TfsCommandBase
    {

        public WorkItemQueryImportCommand(string[] args)
            : base(args)
        {
            InitializeQueryText();

            QueryName = Arguments[TfsUtilityConstants.ArgumentNameQueryName];            
        }

        protected override List<string> GetRequiredArguments()
        {
            var argumentNames = new List<string>
            {
                TfsUtilityConstants.ArgumentNameTfsCollection,
                TfsUtilityConstants.ArgumentNameTeamProject,
                TfsUtilityConstants.ArgumentNameQueryName,
                TfsUtilityConstants.ArgumentNameFolder
            };

            return argumentNames;
        }

        protected override void DisplayUsage(StringBuilder builder)
        {
            base.DisplayUsage(builder);

            string usageString =
                $"{TfsUtilityConstants.ExeName} {CommandArgumentName} /collection:collectionurl /project:projectname /folder:folderpath /name:queryname /filename:filepath";

            builder.AppendLine(usageString);
        }

        protected override string CommandArgumentName
        {
            get { return TfsUtilityConstants.CommandArgumentNameWorkItemQueryImport; }
        }

        private void InitializeQueryText()
        {
            ReadQueryFromFile();

            XDocument doc = null;            

            try
            {
                doc = XDocument.Parse(RawQueryText);
            }
            catch
            {
                // not xml
            }

            if (doc != null)
            {
                PopulateQueryTextFromXml(doc);
            }
            else
            {
                QueryText = RawQueryText;                
            }
        }

        private void PopulateQueryTextFromXml(XDocument doc)
        {
            if (doc.Root.Name != "WorkItemQuery")
            {
                throw new InvalidOperationException("Work item query text is XML but the root node is not WorkItemQuery.");
            }
            var wiql = doc.Root.ElementValue("Wiql");

            if (string.IsNullOrWhiteSpace(wiql))
            {
                throw new InvalidOperationException("Work item query text is XML but the wiql node does not exist or is empty.");
            }

            string teamProjectNameArgValue = Arguments[TfsUtilityConstants.ArgumentNameTeamProject];

            QueryText = wiql.Replace("$$PROJECTNAME$$", teamProjectNameArgValue);
        }

        private void ReadQueryFromFile()
        {
            string filenameArgValue = Arguments[TfsUtilityConstants.ArgumentNameFilename];

            var filename = Path.Combine(Environment.CurrentDirectory, filenameArgValue);

            FileInfo info = new FileInfo(filename);

            Filename = info.FullName;

            RawQueryText = File.ReadAllText(Filename);
        }

        public void IfExistsDeleteQuery()
        {
            foreach (var item in WorkItemQueryFolder)
            {
                if (item is QueryFolder)
                {
                    continue;
                }
                if (item.Name != QueryName)
                {
                    continue;
                }
                item.Delete();
                WorkItemQueryFolder.Project.QueryHierarchy.Save();
                break;
            }
        }

        public void UploadQuery()
        {
            IfExistsDeleteQuery();

            var queryDef = new QueryDefinition(QueryName, QueryText);

            WorkItemQueryFolder.Add(queryDef);
            WorkItemQueryFolder.Project.QueryHierarchy.Save();
        }

        private QueryFolder _workItemQueryFolder;
        public QueryFolder WorkItemQueryFolder
        {
            get
            {
                if (_workItemQueryFolder == null)
                {
                    Connect();

                    _workItemQueryFolder = FindFolder();
                }

                return _workItemQueryFolder;
            }
        }

        private QueryFolder FindFolder()
        {
            var project = FindProjectByName(
                Arguments[TfsUtilityConstants.ArgumentNameTeamProject]);

            if (project == null)
            {
                RaiseExceptionAndDisplayError(true, "TFS project name does not exist.");
            }

            string folderPath = Utilities.GetQueryFilter(Arguments[TfsUtilityConstants.ArgumentNameFolder], project.Name);

            QueryHierarchy projectQueryHierarchy = project.QueryHierarchy;

            var folder = FindFolder(projectQueryHierarchy, folderPath) as QueryFolder;

            return folder;
        }

        private QueryFolder FindFolder(QueryHierarchy projectQueryHierarchy, string searchQueryPath)
        {
            QueryFolder result = null;

            foreach (QueryItem item in projectQueryHierarchy)
            {
                result = FindFolder(searchQueryPath, item);

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        private QueryFolder FindFolder(string searchQueryPath, QueryItem item)
        {
            if (item is QueryFolder)
            {
                if (Utilities.PathMatchesFilter(true, searchQueryPath, item.Path))
                {
                    return item as QueryFolder;
                }
                return FindFolder(item as QueryFolder, searchQueryPath);
            }
            return null;
        }

        private QueryFolder FindFolder(QueryFolder folder, string searchQueryPath)
        {
            QueryFolder result = null;

            foreach (QueryFolder item in folder)
            {
                result = FindFolder(searchQueryPath, item);

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public override void Run()
        {
            Console.WriteLine();

            UploadQuery();

            Console.WriteLine("Query uploaded.");
        }

        public string RawQueryText { get; private set; }
        public string QueryText { get; private set; }
        public string QueryName { get; private set; }
        public string Filename { get; private set; }
    }
}
