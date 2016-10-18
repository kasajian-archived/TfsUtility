using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsUtility
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
            if (path != null &&
                path.ToLower().StartsWith(folderFilter))
            {
                return true;
            }
            return false;
        }

        public static string GetQueryFilter(string queryName, string teamProjectName)
        {
            return GetFolderFilter(queryName, teamProjectName);
        }

        public static string GetFolderFilter(string folderFilter, string teamProjectName)
        {
            string template;

            if (folderFilter.StartsWith("/"))
            {
                template = "{0}{1}";
            }
            else
            {
                template = "{0}/{1}";
            }

            return string.Format(template, teamProjectName, folderFilter).ToLower();
        }
    }
}
