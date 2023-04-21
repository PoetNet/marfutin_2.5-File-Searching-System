using System;
using System.IO;
using System.Collections;

string sourceDirectory = @"/home/dunice/tfolder/tfold2";
string requiredFile = "file5.txt";

Task t1 = Task.Run(() => Searcher.StepByFolders(sourceDirectory, requiredFile));
//Searcher.StepByFolders(sourceDirectory, requiredFile);
//t1.Wait();
CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
CancellationToken token = cancelTokenSource.Token;
await t1.WaitAsync(token);
class Searcher
{
    static SemaphoreSlim semaphore = new SemaphoreSlim(Environment.ProcessorCount);

    static public async void StepByFolders(string path, string filname)
    {
        semaphore.Wait();

        Console.WriteLine($"Current thread's id: {Thread.CurrentThread.ManagedThreadId}");

        string[] hereFiles = Directory.GetFiles(path);
        string[] hereFolders = Directory.GetDirectories(path);

        List<FileInfo> currentFiles = new List<FileInfo>();
        List<string> fileNames = new List<string>();

        for (int s = 0; s < hereFiles.Length; s++)
        {
            currentFiles.Add(new FileInfo(hereFiles[s]));
            fileNames.Add(currentFiles[s].Name);
        }

        if (fileNames.Contains(filname))
        {
            int foundedIndex = fileNames.IndexOf(filname);
            Console.WriteLine($"We found it, nice! Your file is here: '{hereFiles[foundedIndex]}'");
            Environment.Exit(0);
        }
        else
        {
            // Parallel.ForEach(hereFolders, folder =>
            // {
            //     StepByFolders(folder, filname);
            // });
            // Console.WriteLine($"Sorry, I can't find your file here");
            // Environment.Exit(0);

            for (int s = 0; s < hereFolders.Length; s++)
            {
                Task newTask = Task.Run(() => StepByFolders(hereFolders[s], filname));

                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken token = cancelTokenSource.Token;

                await newTask.WaitAsync(token);
                //StepByFolders(hereFolders[s], filname);

                if (s == hereFolders.Length - 1)
                {
                    Console.WriteLine($"Sorry, I can't find your file here");
                    Environment.Exit(0);
                }
            }
        }
        semaphore.Release();
    }
}