using System;
using System.IO;
using System.Collections;

string sourceDirectory = @"/home/dunice/tfolder/tfold2";
string requiredFile = "file5.txt";
Searcher.StepByFolders(sourceDirectory, requiredFile);
class Searcher
{
    static public void StepByFolders(string path, string filname)
    {
        string[] hereFiles = Directory.GetFiles(path);
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
        }
        else
        {
            string[] hereFolders = Directory.GetDirectories(path);

            for (int s = 0; s < hereFolders.Length; s++)
            {
                StepByFolders(hereFolders[s], filname);
            }

        }
    }
}
