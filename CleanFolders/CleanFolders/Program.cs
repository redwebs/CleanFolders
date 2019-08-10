using System;
using System.IO;
using System.Reflection;
using Serilog;


namespace CleanFolders
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("CleanFolders.log")
                .CreateLogger();

            Log.Information("CleanFolders started.");


            ClearFolders();

            Log.Information("CleanFolders ended.");

        }

        private static void ClearFolders()
        {
            try
            {
                var fileInfo = new FileInfo(@"DeletionFolders.txt");

                if (!fileInfo.Exists)
                {
                    Log.Error($"File DeletionFolders.txt not found in {GetExecutingDirectory()}");
                    return;
                }

                var folderPaths = System.IO.File.ReadAllLines(@"DeletionFolders.txt");


                foreach (var folderPath in folderPaths)
                {
                    Log.Information($"Path to delete {folderPath}");

                    try
                    {
                        var filePaths = Directory.GetFiles(folderPath);

                        foreach (var filePath in filePaths)
                        {
                            try
                            {
                                File.Delete(filePath);
                                Log.Information($"Deleted {filePath}");
                            }
                            catch (Exception ex)
                            {
                                Log.Error($"Could not delete {filePath}, {ex.Message}");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Path {folderPath}, errors: {ex.Message}");
                    }

                }

            }
            catch (Exception ex)
            {
                Log.Error($"Exception in ClearFolders: {ex.Message}");
            }
        }

        public static string GetExecutingDirectory()
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null) return "Unknown Assembly";

            var location = new Uri(assembly.GetName().CodeBase);
            var info = new FileInfo(location.AbsolutePath).Directory;
            return info != null ? info.FullName : "Unknown Directory";
        }
    }
}
