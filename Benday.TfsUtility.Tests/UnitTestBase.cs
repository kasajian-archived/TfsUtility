using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benday.TfsUtility.Tests
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
            return String.Format("/{0}:{1}", argumentName, value);
        }
        protected string[] CreateArgsArray(params string[] args)
        {
            return args;
        }
    }
}
