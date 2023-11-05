using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class TaskScheduler<TTask, TPriority>
{
    private readonly SortedDictionary<TPriority, Queue<TTask>> taskQueue = new SortedDictionary<TPriority, Queue<TTask>>();
    private readonly object lockObject = new object();
    private readonly AutoResetEvent taskAddedEvent = new AutoResetEvent(false);

    public delegate void TaskExecution(TTask task);

    public void AddTask(TTask task, TPriority priority)
    {
        lock (lockObject)
        {
            if (!taskQueue.ContainsKey(priority))
            {
                taskQueue[priority] = new Queue<TTask>();
            }
            taskQueue[priority].Enqueue(task);
            taskAddedEvent.Set();
        }
    }

    public bool ExecuteNext(TaskExecution executionDelegate)
    {
        lock (lockObject)
        {
            if (taskQueue.Count > 0)
            {
                var highestPriority = taskQueue.Keys.Max();
                var task = taskQueue[highestPriority].Dequeue();
                if (taskQueue[highestPriority].Count == 0)
                {
                    taskQueue.Remove(highestPriority);
                }

                Task.Factory.StartNew(() => executionDelegate(task));
                return true;
            }
            return false;
        }
    }
}

class Program
{
    static void Main()
    {
        var scheduler = new TaskScheduler<string, int>();

        Task.Factory.StartNew(() =>
        {
            scheduler.AddTask("Task 1", 3);
            Thread.Sleep(1000);
            scheduler.AddTask("Task 2", 1);
            Thread.Sleep(1000);
            scheduler.AddTask("Task 3", 2);
        });

        var executionDelegate = new TaskScheduler<string, int>.TaskExecution(task =>
        {
            Console.WriteLine("Executing task: " + task);
        });

        while (scheduler.ExecuteNext(executionDelegate))
        {
            Thread.Sleep(1000);
        }
    }
}

