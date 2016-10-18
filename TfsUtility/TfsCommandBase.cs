using System;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsUtility
{
    public abstract class TfsCommandBase : CommandBase
    {
        public TfsCommandBase(string[] args)
            : base(args)
        {
        }

        private TfsTeamProjectCollection m_Tpc;
        protected TfsTeamProjectCollection Tpc
        {
            get
            {
                if (m_Tpc == null)
                {
                    throw new InvalidOperationException(
                        "Team project collection has not been initialized.  Call Connect(string).");
                }

                return m_Tpc;
            }
        }

        protected void Connect()
        {
            Connect(Arguments["collection"]);
        }

        protected void Connect(string tfsUrl)
        {
            if (string.IsNullOrEmpty(tfsUrl))
                throw new ArgumentException("tfsUrl is null or empty.", "tfsUrl");
            
            Uri uri = new Uri(tfsUrl);

            m_Tpc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(uri);
            
            Tpc.Authenticate();
        }

        protected Project FindProjectByName(string projectName)
        {
            var store = Tpc.GetService<WorkItemStore>();

            Project returnValue = null;

            foreach (Project item in store.Projects)
            {
                if (string.Equals(item.Name, projectName, StringComparison.CurrentCultureIgnoreCase))
                {
                    returnValue = item;
                    break;
                }
            }

            return returnValue;
        }
    }
}
