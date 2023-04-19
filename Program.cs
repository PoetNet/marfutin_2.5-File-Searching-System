using System;
using System.IO;
using System.Threading;
using System.Collections;
using static System.Environment;


string sourceDirectory = @"/home/dunice/tfolder/tfold2";
string requiredFile = "file5.txt";
int processorCount = Environment.ProcessorCount;

for (int i = 1; i <= processorCount; i++)
{
    Searcher searcher = new Searcher(i, sourceDirectory, requiredFile, processorCount);
}

class Searcher
{
    static Semaphore sem = new Semaphore(1, ProcessorCount);
    string RequiredFile {set;get;}
    string Path {set;get;}
    static int ProcessorCount;

    Thread myThread;
    public Searcher (int i, string path, string requiredFile, int processorCount)
    {
        RequiredFile = requiredFile;
        Path = path;
        ProcessorCount = processorCount;

        myThread = new Thread(StepByFolders);
        myThread.Name = $"Searcher №{i}";
        myThread.Start();
    } 

    public void StepByFolders()
    {
        string[] hereFiles = Directory.GetFiles(Path);
        string[] hereFolders = Directory.GetDirectories(Path);

        List<FileInfo> currentFiles = new List<FileInfo>();
        List<string> fileNames = new List<string>();

        for (int s = 0; s < hereFiles.Length; s++)
        {
            currentFiles.Add(new FileInfo(hereFiles[s]));
            fileNames.Add(currentFiles[s].Name);
        }

        if (fileNames.Contains(RequiredFile))
        {
            int foundedIndex = fileNames.IndexOf(RequiredFile);
            Console.WriteLine($"We found it, nice! Your file is here: '{hereFiles[foundedIndex]}'");
            Environment.Exit(0);
        }
        else
        {
            
            for (int s = 0; s < hereFolders.Length; s++)
            {
                sem.WaitOne();
                StepByFolders();
                Thread.Sleep(1000);
                sem.Release();
                Thread.Sleep(1000);
                
                if (s == hereFolders.Length - 1)
                {
                    Console.WriteLine($"Sorry, I can't find your file here");
                    Environment.Exit(0);
                }
            }

        }
    }
}


class Reader
{
    static Semaphore sem = new Semaphore(3, 3);
    Thread myThread;
    int count = 6;

    public Reader(int i)
    {
        myThread = new Thread(Read);
        myThread.Name = $"Читатель {i}";
        myThread.Start();
    }

    public void Read()
    {
        while (count > 0)
        {
            sem.WaitOne();

            Console.WriteLine($"{Thread.CurrentThread.Name} входит в библиотеку");
            Console.WriteLine($"{Thread.CurrentThread.Name} читает");
            Thread.Sleep(1000);

            Console.WriteLine($"{Thread.CurrentThread.Name} покидает библиотеку");
            sem.Release();

            count--;
            Thread.Sleep(1000);
        }

    }
}
