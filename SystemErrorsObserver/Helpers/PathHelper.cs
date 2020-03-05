using System.IO;
using System.Reflection;

namespace SystemErrorsObserver.Helpers
{
    public static class PathHelper
    {
        public static string AssemblyDirectory
        {
            get
            {
                return GetRuntimePath();
            }
        }

        private static string _runtimePath = string.Empty;
        private static readonly object _runtimePathlock = new object();
        public static string GetRuntimePath()
        {
            lock (_runtimePathlock)
            {
                if (!string.IsNullOrEmpty(_runtimePath))
                    return _runtimePath;

                string assemblyPath = Assembly.GetExecutingAssembly().Location;
                _runtimePath = Path.GetDirectoryName(assemblyPath);
                return _runtimePath;
            }
        }
    }
}
