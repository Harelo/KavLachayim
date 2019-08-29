using System.Threading.Tasks;

namespace KavLachayim.DependencyInterfaces
{
    /// <summary>
    /// An interface to be used with DependencyService to get the local path of a file on each platform
    /// </summary>
    public interface IDatabaseDS
    {
        string GetDatabasePath(string DBName);
    }
}
