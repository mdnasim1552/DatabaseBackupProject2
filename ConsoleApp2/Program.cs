class Program
{
    static void Main(string[] args)
    {
        //string directoryPath = @"E:\IQVIA\Nasim"; // Change this to the desired directory
        string directoryPath = args[0];
        GetFolderNames(directoryPath);
        //Console.WriteLine("Press any key to exit...");
        //Console.ReadKey();
    }
    static void GetFolderNames(string directoryPath)
    {

        try
        {
            string[] directories = Directory.GetDirectories(directoryPath);

            foreach (string dir in directories)
            {
                string folderName = Path.GetFileName(dir);
                //folderNames.Add(folderName);
                DeleteOldBackups(directoryPath+"\\"+folderName);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
        
    }

    public static void DeleteOldBackups(string backupFolderPath)
    {
        DirectoryInfo backupDirectory = new DirectoryInfo(backupFolderPath);
        FileInfo[] backupFiles = backupDirectory.GetFiles("*.*");

        var groupedBackups = backupFiles.GroupBy(file => new { file.LastWriteTime.Year, file.LastWriteTime.Month });

        foreach (var group in groupedBackups)
        {
            FileInfo lastBackup = group.OrderByDescending(file => file.LastWriteTime).First();

            foreach (FileInfo backupFile in group)
            {
                if (backupFile != lastBackup)
                {
                    DateTime backupDate = backupFile.LastWriteTime;
                    // Delete backups older than the last backup of the month
                    backupFile.Delete();
                    Console.WriteLine($"Deleted: {backupDirectory}\\{backupFile.Name}");
                }
            }
        }

    }
}