namespace RagWebApi.Agent
{
    using System.ComponentModel;
    using Microsoft.SemanticKernel;

    public class TaskPlugin
    {
        // Shared in-memory store (swap for EF Core/SQL Server later — your usual stack)
        private static readonly List<TaskItem> _tasks = [];
        private static int _nextId = 1;

        [KernelFunction, Description("Adds a new task to the to-do list.")]
        public static string AddTask([Description("What the task is")] string name, string description)
        {
            var task = new TaskItem { Id = _nextId++, Name = name, Description = description };
            _tasks.Add(task);
            return $"Added task #{task.Id}: {name}";
        }



        [KernelFunction, Description("Lists all current tasks, including completion status.")]
        public static List<TaskItem> ListTasks()
        {
            //if (_tasks.Count == 0) return [];
            //return string.Join("\n", _tasks.Select(t =>
            //    $"#{t.Id} [{(t.IsDone ? "x" : " ")}] {t.Description}"));
            return _tasks;
        }

        [KernelFunction, Description("Marks a task as completed by its ID.")]
        public static string CompleteTask([Description("The task ID")] int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task is null) return $"No task with ID {id}.";
            task.IsDone = true;
            return $"Marked #{id} as done.";
        }

        [KernelFunction, Description("Deletes a task by its ID.")]
        public static string DeleteTask([Description("The task ID")] int id)
        {
            var removed = _tasks.RemoveAll(t => t.Id == id);
            return removed > 0 ? $"Deleted #{id}." : $"No task with ID {id}.";
        }
    }
}
