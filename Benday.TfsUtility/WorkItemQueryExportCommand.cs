using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Benday.TfsUtility
{
    public class WorkItemQueryExportCommand : TfsCommandBase
    {

        public WorkItemQueryExportCommand(string[] args)
            : base(args)
        {

        }

        protected override List<string> GetRequiredArguments()
        {
            var argumentNames = new List<string>();

            argumentNames.Add(TfsUtilityConstants.ArgumentNameTfsCollection);
            argumentNames.Add(TfsUtilityConstants.ArgumentNameTeamProject);
            argumentNames.Add(TfsUtilityConstants.ArgumentNameQuery);

            return argumentNames;
        }

        protected override void DisplayUsage(StringBuilder builder)
        {
            base.DisplayUsage(builder);

            string usageString =
                String.Format("{0} {1} /collection:collectionurl /project:projectname /query:querypath [/filename:filepath]",
                TfsUtilityConstants.ExeName,
                CommandArgumentName);

            builder.AppendLine(usageString);
        }

        protected override string CommandArgumentName
        {
            get { return TfsUtilityConstants.CommandArgumentNameWorkItemQueryExport; }
        }

        public string GetResult()
        {
            Connect();

            var project = FindProjectByName(
                Arguments[TfsUtilityConstants.ArgumentNameTeamProject]);

            if (project == null)
            {
                RaiseExceptionAndDisplayError(true, "TFS project name does not exist.");
            }

            string searchQueryPath = Utilities.GetQueryFilter(Arguments[TfsUtilityConstants.ArgumentNameQuery], project.Name);

            QueryHierarchy projectQueryHierarchy = project.QueryHierarchy;

            var match = FindQuery(projectQueryHierarchy, searchQueryPath) as QueryDefinition;

            if (match == null)
            {
                return null;
            }
            else
            {
                return match.QueryText;
            }
        }

        private QueryItem FindQuery(QueryHierarchy projectQueryHierarchy, string searchQueryPath)
        {
            QueryItem result = null;

            foreach (QueryItem item in projectQueryHierarchy)
            {
                result = FindQuery(searchQueryPath, item);

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        private QueryItem FindQuery(string searchQueryPath, QueryItem item)
        {
            if (item is QueryFolder)
            {
                return FindQuery(item as QueryFolder, searchQueryPath);
            }
            else
            {
                if (Utilities.PathMatchesFilter(true, searchQueryPath, item.Path) == true)
                {
                    return item;
                }
                else
                {
                    return null;
                }
            }
        }

        private QueryItem FindQuery(QueryFolder folder, string searchQueryPath)
        {
            QueryItem result = null;

            foreach (QueryItem item in folder)
            {
                result = FindQuery(searchQueryPath, item);

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public override void Run()
        {
            if (ArgNameExists(TfsUtilityConstants.ArgumentNameFilename) == false)
            {
                Console.WriteLine();
                Console.WriteLine(GetResult());
            }
            else
            {
                Console.WriteLine();

                string filenameArgValue = Arguments[TfsUtilityConstants.ArgumentNameFilename];

                var filename = Path.Combine(Environment.CurrentDirectory, filenameArgValue);

                FileInfo info = new FileInfo(filename);

                var result = GetResult();
                
                if (String.IsNullOrWhiteSpace(result) == true)
                {
                    Console.WriteLine("Could not locate the query.");
                }
                else
                {
                    File.WriteAllText(filename, result);

                    Console.WriteLine(String.Format("Query written to '{0}'.", info.FullName));
                }
            }
        }
    }
}