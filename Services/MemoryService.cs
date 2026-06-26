using System.Collections.Generic;
using CybersecurityChatbotWPF.Models;

namespace CybersecurityChatbotWPF.Services
{
    public class MemoryService
    {
        private UserProfile _userProfile = new UserProfile();

        public void SaveUserName(string name)
        {
            _userProfile.UserName = name;
            _userProfile.ConversationHistory.Add($"User introduced as: {name}");
        }

        public string GetUserName()
        {
            return _userProfile.UserName;
        }

        public void StoreInformation(string userInput, string currentTopic)
        {
            string lowerInput = userInput.ToLower();

            if (lowerInput.Contains("password") && string.IsNullOrEmpty(_userProfile.FavoriteTopic))
            {
                _userProfile.FavoriteTopic = "password";
                _userProfile.Interests.Add("password safety");
            }
            else if ((lowerInput.Contains("scam") || lowerInput.Contains("phish")) && string.IsNullOrEmpty(_userProfile.FavoriteTopic))
            {
                _userProfile.FavoriteTopic = "scam";
                _userProfile.Interests.Add("scam protection");
            }
            else if (lowerInput.Contains("privacy") && string.IsNullOrEmpty(_userProfile.FavoriteTopic))
            {
                _userProfile.FavoriteTopic = "privacy";
                _userProfile.Interests.Add("privacy");
            }

            _userProfile.ConversationCount++;
            _userProfile.ConversationHistory.Add($"Q: {userInput}");
        }

        public string GetMemorySummary()
        {
            if (!string.IsNullOrEmpty(_userProfile.UserName))
            {
                string topicInfo = string.IsNullOrEmpty(_userProfile.FavoriteTopic) ? "" : $" | Interested in: {_userProfile.FavoriteTopic}";
                return $"Remembering: {_userProfile.UserName}{topicInfo} | {_userProfile.ConversationCount} messages";
            }
            return "No saved info yet. Tell me your name!";
        }
    }
}