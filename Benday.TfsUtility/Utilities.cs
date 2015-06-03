using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.TfsUtility
{
    public static class Utilities
    {

        public static bool PathMatchesFilter(bool hasFolderFilter, string folderFilter, QueryFolder currentFolder)
        {
            return PathMatchesFilter(hasFolderFilter, folderFilter, currentFolder.Path);
        }


        public static bool PathMatchesFilter(bool hasFolderFilter, string folderFilter, string path)
        {
            if (hasFolderFilter == false)
            {
                return true;
            }
            else
            {
                if (path != null &&
                    path.ToLower().StartsWith(folderFilter) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static string GetQueryFilter(string queryName, string teamProjectName)
        {
            return GetFolderFilter(queryName, teamProjectName);
        }

        public static string GetFolderFilter(string folderFilter, string teamProjectName)
        {
            string template;

            if (folderFilter.StartsWith("/") == true)
            {
                template = "{0}{1}";
            }
            else
            {
                template = "{0}/{1}";
            }

            return String.Format(template, teamProjectName, folderFilter).ToLower();
        }
    }
}
