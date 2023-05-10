# File Search System 💻🔍

This code defines a Searcher class with a static StepByFolders method that searches for a required file in a given directory and its subdirectories. The search is performed recursively for all the subdirectories. If the required file is found, its path is printed to the console and the program exits.


The program starts by defining the source directory and the name of the required file. It then creates a CancellationTokenSource object and obtains a CancellationToken object from it. This token will be used to cancel the search if needed.


The StepByFolders method is called with the source directory, required file, and token as parameters. It uses a semaphore to limit the number of concurrently executing threads to the number of processors available. It then obtains the list of files and subdirectories in the current directory and checks if the required file is among the files. If it is, the file path is printed to the console and the program exits.


If the required file is not found in the current directory, the method creates a list of tasks to search for the file in the subdirectories using the StepByFolders method recursively. It waits for all the tasks to complete using Task.WhenAll and catches the OperationCanceledException if the token was canceled during the search.


Overall, this code provides a useful example of recursive file searching in C# using Task and SemaphoreSlim for concurrency control.

## Installation 📦

1. Clone this repo to your local machine using `git clone`
2. Open the solution in Visual Studio
3. Run the program to start the File Search System

```bash
git clone git@github.com:marfutin/marfutin_2.5-File-Searching-System.git 
dotnet run
```

## This is a training project, just enjoy ☺

If you want me to upgrade the project, please send me here: r.marfutin@sh.dunice.net
