using System;
using System.IO;
using System.Collections;

string sourceDirectory = "/home/dunice/tfolder/tfold2";
string requiredFile = "file5.txt";

CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
CancellationToken token = cancelTokenSource.Token;

await Task.Run(() => Searcher.StepByFolders(sourceDirectory, requiredFile, token));
class Searcher
{
    static SemaphoreSlim semaphore = new SemaphoreSlim(Environment.ProcessorCount);

    static public async Task StepByFolders(string path, string filname, CancellationToken token)
    {
        semaphore.Wait();

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
            List<Task> tasks = new List<Task>();

            foreach (string folder in hereFolders)
            {
                Task task = Task.Run(() => Searcher.StepByFolders(folder, filname, token));
                tasks.Add(task);
            }
            try
            {
                await Task.WhenAll(tasks);
            }
            catch (OperationCanceledException){}
        }
        semaphore.Release();
    }
}
