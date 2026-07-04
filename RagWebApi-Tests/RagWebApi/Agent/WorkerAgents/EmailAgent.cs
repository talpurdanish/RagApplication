
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace RagWebApi.Agent.WorkerAgents

{

    public class EmailAgent(IChatClient client)
    {

        public AIAgent Create() => client.AsAIAgent(

                    name: "EmailBot",
                    instructions: """
                        You are a helpful email management assistant.
                        Use the Email plugin to send all tasks to an email if list not empty
                        Return Format for all commands should contains
                            1. success : True or false
                            2. message : string message about the task
                        Return Format for all commands should contains
                            1. success : True or false
                            2. message : string message about the email
                            4. Complete: true email sent, else false
                        Always confirm what you did in plain language, not just raw tool output.
                        """,
                    tools: [
                        AIFunctionFactory.Create(EmailPlugin.EmailTasks),
                        ]
                );
    }
}
