using System.Collections.Generic;

namespace CybersecurityChatbotWPF.Models
{
    public class UserProfile
    {
        public string UserName { get; set; } = string.Empty;
        public string FavoriteTopic { get; set; } = string.Empty;
        public List<string> Interests { get; set; } = new List<string>();
        public int ConversationCount { get; set; }
        public List<string> ConversationHistory { get; set; } = new List<string>();

        public UserProfile()
        {
            ConversationCount = 0;
        }
    }
}