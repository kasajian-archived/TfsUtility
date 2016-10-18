using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsUtility
{
    public class WorkItemQueryListCommand : TfsCommandBase
    {
        public WorkItemQueryListCommand(string[] args)
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
                $"{TfsUtilityConstants.ExeName} {CommandArgumentName} /collection:collectionurl /project:projectname [/filter:folderpath]";

            builder.AppendLine(usageString);
        }

        protected override string CommandArgumentName
        {
            get { return TfsUtilityConstants.CommandArgumentNameListWorkItemQueries; }
        }


        public List<string> GetResult()
        {
            List<string> paths = new List<string>();

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

            if (hasFolderFilter)
            {
                folderFilter = Utilities.GetFolderFilter(Arguments[TfsUtilityConstants.ArgumentNameFolderFilter], project.Name);
            }

            QueryHierarchy projectQueryHierarchy = project.QueryHierarchy;

            ProcessQueryHierarchy(projectQueryHierarchy, paths, project, hasFolderFilter, folderFilter);

            return paths;
        }

        private void ProcessQueryHierarchy(QueryHierarchy hierarchy, List<string> paths, Project project, bool hasFolderFilter, string folderFilter)
        {
            foreach (QueryItem item in project.QueryHierarchy)
            {
                ProcessQueryHierarchy(paths, hasFolderFilter, folderFilter, item);
            }
        }

        private void ProcessQueryHierarchy(List<string> paths, bool hasFolderFilter, string folderFilter, QueryItem item)
        {
            if (item is QueryFolder)
            {
                ProcessQueryHierarchy(hasFolderFilter, folderFilter, item as QueryFolder, paths);
            }
            else
            {
                if (Utilities.PathMatchesFilter(hasFolderFilter, folderFilter, item.Path))
                {
                    paths.Add(item.Path);
                }
            }
        }

        private void ProcessQueryHierarchy(bool hasFolderFilter, string folderFilter, QueryFolder folder, List<string> paths)
        {
            foreach (var item in folder)
            {
                ProcessQueryHierarchy(paths, hasFolderFilter, folderFilter, item);
            }
        }        

        public override void Run()
        {
            Console.WriteLine();
            GetResult().ForEach(x => Console.WriteLine(x));
        }
    }
}