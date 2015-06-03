using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.TfsUtility
{
    public class WorkItemQueryFolderListCommand : TfsCommandBase
    {
        public WorkItemQueryFolderListCommand(string[] args)
            : base(args)
        {

        }

        protected override List<string> GetRequiredArguments()
        {
            var argumentNames = new List<string>();

            argumentNames.Add(TfsUtilityConstants.ArgumentNameTfsCollection);
            argumentNames.Add(TfsUtilityConstants.ArgumentNameTeamProject);

            return argumentNames;
        }

        protected override void DisplayUsage(StringBuilder builder)
        {
            base.DisplayUsage(builder);

            string usageString =
                String.Format("{0} {1} /collection:collectionurl /project:projectname [/filter:folderpath]",
                TfsUtilityConstants.ExeName,
                CommandArgumentName);

            builder.AppendLine(usageString);
        }

        protected override string CommandArgumentName
        {
            get { return TfsUtilityConstants.CommandArgumentNameListWorkItemQueryFolders; }
        }

        public List<string> GetResult()
        {
            List<string> folderPaths = new List<string>();

            Connect();

            var project = FindProjectByName(
                Arguments[TfsUtilityConstants.ArgumentNameTeamProject]);

            if (project == null)
            {
                RaiseExceptionAndDisplayError(true, "TFS project name does not exist.");
            }

            QueryFolder currentFolder;

            bool hasFolderFilter = Arguments.ContainsKey(TfsUtilityConstants.ArgumentNameFolderFilter);

            string folderFilter = null;

            if (hasFolderFilter == true)
            {
                folderFilter = Utilities.GetFolderFilter(Arguments[TfsUtilityConstants.ArgumentNameFolderFilter], project.Name);
            }

            foreach (var item in project.QueryHierarchy)
            {
                currentFolder = item as QueryFolder;

                if (currentFolder != null)
                {
                    if (Utilities.PathMatchesFilter(hasFolderFilter, folderFilter, currentFolder) == true)
                    {
                        folderPaths.Add(currentFolder.Path);
                    }

                    AddSubFolders(hasFolderFilter, folderFilter, currentFolder, folderPaths);
                }
            }

            return folderPaths;
        }

        private void AddSubFolders(bool hasFolderFilter, string folderFilter, QueryFolder parentFolder, List<string> folderPaths)
        {
            QueryFolder currentFolder;

            foreach (var item in parentFolder)
            {
                currentFolder = item as QueryFolder;

                if (currentFolder != null)
                {
                    if (Utilities.PathMatchesFilter(hasFolderFilter, folderFilter, currentFolder) == true)
                    {
                        folderPaths.Add(currentFolder.Path);
                    }

                    AddSubFolders(hasFolderFilter, folderFilter, currentFolder, folderPaths);
                }
            }
        }

        public override void Run()
        {
            Console.WriteLine();
            GetResult().ForEach(x => Console.WriteLine(x));
        }


        
    }
}