using System;

namespace TfsUtility.Tests
{
    public class UnitTestBase
    {

        public UnitTestBase()
        {

        }
        public string TfsCollectionUrl 
        {
            get
            {
                return "http://demotfs2013:8080/tfs/DefaultCollection";                    
            }
        }

        public string TeamProjectName
        {
            get
            {
                return "scrum-test-20131023";
            }
        }
        protected string GetArgEntry(string argumentName, string value)
        {
            return $"/{argumentName}:{value}";
        }
        protected string[] CreateArgsArray(params string[] args)
        {
            return args;
        }
    }
}
