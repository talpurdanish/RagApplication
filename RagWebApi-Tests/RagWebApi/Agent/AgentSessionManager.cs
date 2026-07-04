
using System.Collections.Concurrent;
using Microsoft.Agents.AI;
namespace RagWebApi.Agent
{


    public class AgentSessionManager
    {
        // Caches active sessions in memory using the conversation ID as the key
        private readonly ConcurrentDictionary<string, AgentSession> _sessions = new();
        

        public async Task<AgentSession> GetOrCreateSessionAsync(string conversationId, AIAgent agent)
        {
            if (_sessions.TryGetValue(conversationId, out var existingSession))
            {
                return existingSession;
            }

            // Framework creates a fresh session if none exists
            AgentSession newSession = await agent.CreateSessionAsync();
            _sessions[conversationId] = newSession;

            return newSession;
        }
    }

}
