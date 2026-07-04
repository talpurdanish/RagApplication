namespace RagWebApi.Agent
{
    using System.ComponentModel;
    using Microsoft.SemanticKernel;

    public class EmailPlugin
    {
      

        // Plugin to mock the functionality of email for demo purpose
        private static readonly string emailTemplate = @"To: {0}\n" +
            "From: talpurdanish@gmail.com\n" +
            "Subject: List of all Tasks\n" +
            "{1}";
        

        [KernelFunction, Description("Email Tasks")]
        public static string EmailTasks([Description("Email the task")] string email, TaskItem[] tasks)
        {
            string allTasks = string.Join("\n", tasks.Select(t =>
                $"#{t.Id} [{(t.IsDone ? "x" : " ")}] {t.Description}"));
            string emailtext = string.Format(emailTemplate, email, allTasks);
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine(emailtext);
            Console.ResetColor();
            return emailtext;
        }

    }
}
