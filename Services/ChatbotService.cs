using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CybersecurityChatbotWPF.Models;

namespace CybersecurityChatbotWPF.Services
{
    public class ChatbotService
    {
        private Dictionary<string, List<string>> _keywordResponses = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> _randomResponses = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> _nlpPatterns = new Dictionary<string, List<string>>();
        private Random _random = new Random();

        public ChatbotService()
        {
            InitializeKeywordResponses();
            InitializeRandomResponses();
            InitializeNlpPatterns();
        }

        private void InitializeNlpPatterns()
        {
            _nlpPatterns = new Dictionary<string, List<string>>()
            {
                ["add_task"] = new List<string>
                {
                    @"add\s*a?\s*task",
                    @"new\s*task",
                    @"create\s*a?\s*task",
                    @"add\s*a?\s*reminder",
                    @"remind\s*me\s*to",
                    @"i\s*need\s*to",
                    @"should\s*do",
                    @"must\s*do"
                },
                ["view_tasks"] = new List<string>
                {
                    @"show\s*tasks",
                    @"view\s*tasks",
                    @"list\s*tasks",
                    @"what\s*tasks",
                    @"my\s*tasks",
                    @"show\s*my\s*tasks"
                },
                ["complete_task"] = new List<string>
                {
                    @"complete\s*task",
                    @"mark\s*done",
                    @"finish\s*task",
                    @"task\s*complete",
                    @"done\s*with"
                },
                ["delete_task"] = new List<string>
                {
                    @"delete\s*task",
                    @"remove\s*task",
                    @"cancel\s*task",
                    @"clear\s*task"
                },
                ["quiz"] = new List<string>
                {
                    @"start\s*quiz",
                    @"take\s*quiz",
                    @"play\s*quiz",
                    @"quiz\s*me",
                    @"test\s*my\s*knowledge",
                    @"cybersecurity\s*quiz"
                },
                ["view_log"] = new List<string>
                {
                    @"show\s*log",
                    @"view\s*log",
                    @"activity\s*log",
                    @"what\s*have\s*you\s*done",
                    @"what\s*did\s*you\s*do",
                    @"show\s*activity",
                    @"recent\s*actions"
                }
            };
        }

        private void InitializeKeywordResponses()
        {
            _keywordResponses = new Dictionary<string, List<string>>()
            {
                ["password"] = new List<string>
                {
                    "Use strong passwords with at least 12 characters including uppercase, lowercase, numbers, and symbols!",
                    "Never reuse passwords across different accounts - each account needs its own unique password!",
                    "Enable Two-Factor Authentication (2FA) whenever possible for an extra layer of security!"
                },
                ["scam"] = new List<string>
                {
                    "Never click on links in suspicious emails! Always hover over links to see the real URL first.",
                    "Legitimate companies never ask for your password via email.",
                    "Check for spelling errors, urgent language, and suspicious sender addresses!"
                },
                ["privacy"] = new List<string>
                {
                    "Review your privacy settings on social media regularly.",
                    "Be careful what you share online - once posted, it's difficult to remove.",
                    "Use a VPN when using public Wi-Fi to protect your privacy!"
                },
                ["phishing"] = new List<string>
                {
                    "Phishing emails often create a sense of urgency. Take a moment to verify!",
                    "Look for poor grammar, generic greetings, and mismatched email addresses.",
                    "Never enter personal information on a site you reached from an email link."
                },
                ["help"] = new List<string>
                {
                    "I can help you with:\n- Password safety\n- Recognizing scams\n- Privacy protection\n- Safe browsing\n\nType 'help' for this menu.",
                    "Try asking:\n- 'Tell me about password safety'\n- 'How to spot a scam?'\n- 'Privacy tips'"
                },
                ["feeling"] = new List<string>
                {
                    "I'm doing great, thank you for asking! How can I help you today?",
                    "I'm functioning perfectly! What cybersecurity topic interests you?",
                    "I'm excellent! What would you like to know about cybersecurity?"
                },
                ["ask_feeling"] = new List<string>
                {
                    "How are you feeling today? I'm here to listen and help.",
                    "Before we dive in, how are you doing today?",
                    "How's your day going? Staying safe online starts with a mindful mindset!"
                },
                ["empathetic"] = new List<string>
                {
                    "I understand how you feel. Cybersecurity can be overwhelming, but you're taking the right step!",
                    "It's completely normal to feel that way. Many people share your concerns.",
                    "Thank you for sharing that. Remember, you're not alone in this!"
                }
            };
        }

        private void InitializeRandomResponses()
        {
            _randomResponses = new Dictionary<string, List<string>>()
            {
                ["greeting"] = new List<string>
                {
                    "Hi there! How can I help you stay safe online today?",
                    "Hello! Ready to learn about cybersecurity?",
                    "Greetings! I'm your cybersecurity assistant. What would you like to know?"
                },
                ["thanks"] = new List<string>
                {
                    "You're welcome! Stay safe online!",
                    "My pleasure! Cybersecurity is everyone's responsibility.",
                    "Glad I could help! Remember to always think before you click!"
                },
                ["confirmation"] = new List<string>
                {
                    "I've processed your request.",
                    "Action completed successfully.",
                    "Task has been updated."
                }
            };
        }

        private string GetNlpIntent(string input)
        {
            string lowerInput = input.ToLower();

            foreach (var pattern in _nlpPatterns)
            {
                foreach (string regexPattern in pattern.Value)
                {
                    if (Regex.IsMatch(lowerInput, regexPattern, RegexOptions.IgnoreCase))
                    {
                        return pattern.Key;
                    }
                }
            }
            return string.Empty;
        }

        public ChatbotResponse GetResponse(string userInput, string sentiment, string currentTopic)
        {
            string lowerInput = userInput.ToLower();
            string responseText = "";
            string category = "unknown";
            string newTopic = string.Empty;

            string intent = GetNlpIntent(lowerInput);

            if (!string.IsNullOrEmpty(intent))
            {
                switch (intent)
                {
                    case "add_task":
                        category = "task";
                        responseText = "I can help you add a cybersecurity task. Click the 'Tasks' button above to get started.";
                        break;
                    case "view_tasks":
                        category = "task";
                        responseText = "Click the 'Tasks' button above to view and manage all your cybersecurity tasks.";
                        break;
                    case "quiz":
                        category = "quiz";
                        responseText = "Click the 'Quiz' button above to start the cybersecurity knowledge test!";
                        break;
                    case "view_log":
                        category = "log";
                        responseText = "Click the 'Logs' button above to view all recent actions.";
                        break;
                    default:
                        break;
                }
            }

            if (string.IsNullOrEmpty(responseText))
            {
                if (ContainsKeyword(lowerInput, new[] { "how are you", "how's it going", "how do you feel" }))
                {
                    category = "feeling";
                    responseText = GetRandomResponse(_keywordResponses["feeling"]);
                }
                else if (ContainsKeyword(lowerInput, new[] { "i feel", "i'm feeling", "i am feeling" }))
                {
                    category = "empathetic";
                    responseText = GetRandomResponse(_keywordResponses["empathetic"]);
                }
                else if (ContainsKeyword(lowerInput, new[] { "good", "great", "wonderful", "excellent", "happy" }))
                {
                    category = "positive";
                    responseText = "That's wonderful to hear! What cybersecurity topic interests you today?";
                }
                else if (ContainsKeyword(lowerInput, new[] { "bad", "sad", "tired", "stressed", "anxious", "worried" }))
                {
                    category = "negative";
                    responseText = "I'm sorry you're feeling that way. Would you like some easy cybersecurity tips?";
                }
                else if (ContainsKeyword(lowerInput, new[] { "thank", "thanks", "appreciate" }))
                {
                    category = "thanks";
                    responseText = GetRandomResponse(_randomResponses["thanks"]);
                }
                else if (ContainsKeyword(lowerInput, new[] { "password", "passphrase", "login", "credentials" }))
                {
                    category = "password";
                    responseText = GetRandomResponse(_keywordResponses["password"]);
                    newTopic = "password";
                }
                else if (ContainsKeyword(lowerInput, new[] { "scam", "fraud", "fake" }))
                {
                    category = "scam";
                    responseText = GetRandomResponse(_keywordResponses["scam"]);
                    newTopic = "scam";
                }
                else if (ContainsKeyword(lowerInput, new[] { "privacy", "private", "personal information" }))
                {
                    category = "privacy";
                    responseText = GetRandomResponse(_keywordResponses["privacy"]);
                    newTopic = "privacy";
                }
                else if (ContainsKeyword(lowerInput, new[] { "phish", "phishing" }))
                {
                    category = "phishing";
                    responseText = GetRandomResponse(_keywordResponses["phishing"]);
                    newTopic = "phishing";
                }
                else if (ContainsKeyword(lowerInput, new[] { "help", "what can you do", "options" }))
                {
                    category = "help";
                    responseText = GetRandomResponse(_keywordResponses["help"]);
                }
                else if (ContainsKeyword(lowerInput, new[] { "hello", "hi", "hey" }))
                {
                    category = "greeting";
                    responseText = GetRandomResponse(_randomResponses["greeting"]);
                    responseText += " " + GetRandomResponse(_keywordResponses["ask_feeling"]);
                }
                else
                {
                    responseText = GetDefaultResponse();
                }
            }

            responseText = AdjustResponseForSentiment(responseText, sentiment);

            return new ChatbotResponse(userInput, responseText, category) { NewTopic = newTopic };
        }

        private bool ContainsKeyword(string input, string[] keywords)
        {
            foreach (string keyword in keywords)
                if (input.Contains(keyword)) return true;
            return false;
        }

        private string GetRandomResponse(Dictionary<string, List<string>> dict, string key)
        {
            if (dict.ContainsKey(key) && dict[key].Count > 0)
                return dict[key][_random.Next(dict[key].Count)];
            return string.Empty;
        }

        private string GetRandomResponse(List<string> responses)
        {
            return responses[_random.Next(responses.Count)];
        }

        private string AdjustResponseForSentiment(string response, string sentiment)
        {
            if (sentiment == "worried")
                return "I understand your concern. " + response;
            if (sentiment == "frustrated")
                return "I hear you. Let me help simplify this. " + response;
            if (sentiment == "curious")
                return "That's a great question! " + response;
            return response;
        }

        private string GetDefaultResponse()
        {
            string[] defaults = {
                "I'm not sure I understand. Try asking about passwords, scams, privacy, or phishing.",
                "Hmm, I didn't catch that. You can ask me about password safety, spotting scams, or privacy.",
                "Try asking: 'Tell me about password safety', 'How to spot a scam?', or 'Privacy tips'"
            };
            return defaults[_random.Next(defaults.Length)];
        }
    }
}