using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace RagWebApi.Agent.WorkerAgents

{


    public class TaskAgent(IChatClient client)
    {

        public AIAgent Create() => client.AsAIAgent(

                    name: "TaskBot",
                    instructions: """
                        You are a helpful task management assistant.
                        Use the Tasks plugin to add, list, complete, or delete tasks.
                        Always confirm what you did in plain language, not just raw tool output.
                        For add command 
                        - if only name is given, generate the description then add task with provided name and generated description
                        - if name and description is given, add the task with provided name and description
                        For list command
                        -   return all tasks as data array
                        Return Format for all commands should contains
                            1. success : True or false
                            2. message : string message about the task
                            3. data: for list return Array of TaskItems{id, name, description, isDone} othervise empty
                            4. completed: true task command completed successfully, else false
                        If a request is ambiguous (e.g. which task to complete), ask which task ID.
                        """,
                    tools: [
                        AIFunctionFactory.Create(TaskPlugin.AddTask),
                        AIFunctionFactory.Create(TaskPlugin.ListTasks),
                        AIFunctionFactory.Create(TaskPlugin.DeleteTask),
                        AIFunctionFactory.Create(TaskPlugin.CompleteTask)

                        ]

                );

                
    }
}
