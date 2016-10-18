using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsUtility
{
    public class TeamProjectListCommand : TfsCommandBase
    {
        public TeamProjectListCommand(string[] args)
            : base(args)
        {

        }

        protected override List<string> GetRequiredArguments()
        {
            var argumentNames = new List<string>();

            argumentNames.Add(TfsUtilityConstants.ArgumentNameTfsCollection);

            return argumentNames;
        }

        protected override void DisplayUsage(StringBuilder builder)
        {
            base.DisplayUsage(builder);

            string usageString =
                $"{TfsUtilityConstants.ExeName} {CommandArgumentName} /collection:collectionurl";

            builder.AppendLine(usageString);
        }

        protected override string CommandArgumentName
        {
            get { return TfsUtilityConstants.CommandArgumentNameTeamProjectList; }
        }

        public List<string> GetResult()
        {
            Connect();

            var store = Tpc.GetService<WorkItemStore>();

            List<string> returnValues = new List<string>();

            foreach (Project item in store.Projects)
            {
                returnValues.Add(item.Name);
            }

            return returnValues;
        }

        public override void Run()
        {
            Console.WriteLine();
            GetResult().ForEach(x => Console.WriteLine(x));
        }
    }
}
