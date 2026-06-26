using System.Collections.Generic;

namespace CybersecurityChatbotWPF.Services
{
    public class SentimentService
    {
        private List<string> _worriedKeywords = new List<string> { "worried", "scared", "afraid", "concerned", "nervous", "anxious", "unsafe" };
        private List<string> _curiousKeywords = new List<string> { "curious", "interested", "want to learn", "tell me", "explain", "how does", "what is" };
        private List<string> _frustratedKeywords = new List<string> { "frustrated", "confused", "don't understand", "too hard", "complicated", "difficult" };

        public string DetectSentiment(string input)
        {
            string lowerInput = input.ToLower();

            foreach (string keyword in _worriedKeywords)
                if (lowerInput.Contains(keyword)) return "worried";

            foreach (string keyword in _curiousKeywords)
                if (lowerInput.Contains(keyword)) return "curious";

            foreach (string keyword in _frustratedKeywords)
                if (lowerInput.Contains(keyword)) return "frustrated";

            return "neutral";
        }
    }
}