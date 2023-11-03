using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

class Program
{
    static void Main(string[] args)
    {
      
        Console.WriteLine("Read 3 files");
        CountSpaceInThreeFiles();
        Console.WriteLine(new string('=',80));
        Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine);

        Console.WriteLine("ReadFolder");
        CountSpacesInFolder("FILES");
        Console.ReadKey();
    }

    private static void CountSpaceInThreeFiles() 
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        string[] fileNames = { "file01.txt", "file02.txt", "file03.txt" };
        Task<int>[] tasks = new Task<int>[fileNames.Length];

        for (int i = 0; i < fileNames.Length; i++)
        {
            string fileName = fileNames[i];
            tasks[i] = Task.Run(() => CountSpacesInFile(fileName));
        }

        Task.WaitAll(tasks);

        for (int i = 0; i < tasks.Length; i++)
        {
            Console.WriteLine("File {0}: {1} spaces", fileNames[i], tasks[i].Result);
        }

        stopwatch.Stop();

        Console.WriteLine("Execution time: {0}", stopwatch.Elapsed);
    }

    private static void CountSpacesInFolder(string folderPath)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        string[] fileNames = Directory
                .EnumerateFiles(folderPath, "*", SearchOption.AllDirectories)
                .Select(Path.GetFileName).ToArray();

        Task<int>[] tasks = new Task<int>[fileNames.Length];

        for (int i = 0; i < fileNames.Length; i++)
        {
            string fileName = fileNames[i];
            tasks[i] = Task.Run(() => CountSpacesInFile(fileName, folderPath));
        }

        Task.WaitAll(tasks);

        for (int i = 0; i < tasks.Length; i++)
        {
            Console.WriteLine("File {0}: {1} spaces", Path.GetFileName(fileNames[i]), tasks[i].Result);
        }
        stopwatch.Stop();

        Console.WriteLine("Execution time: {0}", stopwatch.Elapsed);
    }

    private static int CountSpacesInFile(string fileName , string folder = "FILES")
    {
        int count = 0;
        using (StreamReader reader = new StreamReader(folder + "\\" + fileName))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                foreach (char c in line)
                {
                    if (c == ' ')
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }
}