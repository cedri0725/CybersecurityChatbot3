using System;

namespace CybersecurityChatbotWPF.Models
{
    public class ChatbotResponse
    {
        public string UserQuestion { get; set; } = string.Empty;
        public string BotAnswer { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Sentiment { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string NewTopic { get; set; } = string.Empty;

        public ChatbotResponse()
        {
            Timestamp = DateTime.Now;
        }

        public ChatbotResponse(string userQuestion, string botAnswer, string category)
        {
            UserQuestion = userQuestion ?? string.Empty;
            BotAnswer = botAnswer ?? string.Empty;
            Category = category ?? string.Empty;
            Timestamp = DateTime.Now;
            NewTopic = string.Empty;
        }
    }
}