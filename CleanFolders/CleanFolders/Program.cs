using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void ClearFolders()
        {
            try
            {
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
                                Log.Information($"Could not delete {filePath}, {ex.Message}");
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

    }
}
