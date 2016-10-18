using System;
using System.Text;

namespace TfsUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                DisplayUsage();
            }
            else
            {
                try
                {
                    string commandName = args[0];

                    if (commandName == TfsUtilityConstants.CommandArgumentNameListWorkItemQueryFolders)
                    {
                        new WorkItemQueryFolderListCommand(args).Run();
                    }
                    else if (commandName == TfsUtilityConstants.CommandArgumentNameListWorkItemQueries)
                    {
                        new WorkItemQueryListCommand(args).Run();
                    }
                    else if (commandName == TfsUtilityConstants.CommandArgumentNameWorkItemQueryExport)
                    {
                        new WorkItemQueryExportCommand(args).Run();
                    }
                    else if (commandName == TfsUtilityConstants.CommandArgumentNameWorkItemQueryImport)
                    {
                        new WorkItemQueryImportCommand(args).Run();
                    }
                    else if (commandName == TfsUtilityConstants.CommandArgumentNameTeamProjectList)
                    {
                        new TeamProjectListCommand(args).Run();
                    }
                    else
                    {
                        DisplayUsage();
                    }
                }
                catch
                {
                    
                }                   
            }           
        }

        private static void DisplayUsage()
        {
            string indent = "\t";

            StringBuilder builder = new StringBuilder();

            builder.AppendLine();
            builder.AppendLine("Team Foundation Server Utility");
            builder.AppendLine();
            builder.AppendLine("Available commands:");
            builder.AppendLine(indent + TfsUtilityConstants.CommandArgumentNameTeamProjectList);
            builder.AppendLine(indent + TfsUtilityConstants.CommandArgumentNameListWorkItemQueryFolders);
            builder.AppendLine(indent + TfsUtilityConstants.CommandArgumentNameListWorkItemQueries);
            builder.AppendLine(indent + TfsUtilityConstants.CommandArgumentNameWorkItemQueryExport);
            builder.AppendLine(indent + TfsUtilityConstants.CommandArgumentNameWorkItemQueryImport);

            Console.WriteLine(builder.ToString());
        }
    }
}
