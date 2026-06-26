using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using CybersecurityChatbotWPF.Services;
using CybersecurityChatbotWPF.Views;

namespace CybersecurityChatbotWPF
{
    public partial class MainWindow : Window
    {
        private ChatbotService? _chatbotService;
        private SentimentService? _sentimentService;
        private MemoryService? _memoryService;
        private AudioService? _audioService;
        private string _currentTopic = string.Empty;
        private bool _voiceEnabled = true;
        private string _logFilePath = string.Empty;
        private System.Collections.Generic.List<string> _recentActions = new System.Collections.Generic.List<string>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeServices();
            InitializeLogging();
            _ = DisplayAsciiArt(); // Run rainbow animation
            PlayWelcomeGreeting();
            AddBotMessage("Hello! I'm your Cybersecurity Awareness Assistant.", "#00FFAA");
            AddBotMessage("I can help you with passwords, scams, privacy, and phishing.", "#00FFAA");
            AddBotMessage("Before we start, how are you feeling today?", "#88FF88");
            LogActivity("Chatbot started");
        }

        private void InitializeServices()
        {
            _chatbotService = new ChatbotService();
            _sentimentService = new SentimentService();
            _memoryService = new MemoryService();
            _audioService = new AudioService();
        }

        private void InitializeLogging()
        {
            try
            {
                string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logDirectory))
                    Directory.CreateDirectory(logDirectory);

                _logFilePath = Path.Combine(logDirectory, $"chatbot_log_{DateTime.Now:yyyy-MM-dd}.txt");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Log init error: {ex.Message}");
            }
        }

        private void LogActivity(string activity)
        {
            try
            {
                _recentActions.Insert(0, $"{DateTime.Now:HH:mm:ss} - {activity}");
                if (_recentActions.Count > 50)
                    _recentActions.RemoveAt(_recentActions.Count - 1);

                if (!string.IsNullOrEmpty(_logFilePath))
                {
                    string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {activity}";
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Log error: {ex.Message}");
            }
        }

        private async Task DisplayAsciiArt()
        {
            string ascii = @"
╔══════════════════════════════════════════════════════════════════════════════════════════╗
║                                                                                          ║
║     ██████╗██╗   ██╗██████╗ ███████╗██████╗ ███████╗ ██████╗██╗   ██╗██████╗ ██╗████████╗║
║    ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗██╔════╝██╔════╝██║   ██║██╔══██╗██║╚══██╔══╝║
║    ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝███████╗██║     ██║   ██║██████╔╝██║   ██║   ║
║    ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗╚════██║██║     ██║   ██║██╔══██╗██║   ██║   ║
║    ╚██████╗   ██║   ██████╔╝███████╗██║  ██║███████║╚██████╗████████║██║  ██║██║   ██║   ║
║     ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝╚══════╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═╝   ╚═╝   ║
║                                                                                          ║
║           CYBERSECURITY AWARENESS CHATBOT: PROTECTING SOUTH AFRICAN CITIZENS             ║
║                       Stay Safe Online - Think Before You Click!                         ║
║                                                                                          ║
╚══════════════════════════════════════════════════════════════════════════════════════════╝";

            AsciiArt.Text = ascii;

            // Rainbow color animation
            string[] rainbowColors = {
                "#FF0000", "#FF4400", "#FF8800", "#FFCC00",
                "#44FF00", "#00FF44", "#00FF88", "#00FFCC",
                "#0044FF", "#8800FF", "#CC00FF", "#FF00CC"
            };

            int colorIndex = 0;
            for (int cycle = 0; cycle < 3; cycle++)
            {
                foreach (string hex in rainbowColors)
                {
                    AsciiArt.Foreground = (Brush)new BrushConverter().ConvertFromString(hex);
                    await Task.Delay(100);
                }
            }
            // End on cyan
            AsciiArt.Foreground = (Brush)new BrushConverter().ConvertFromString("#00FFAA");
        }

        private void PlayWelcomeGreeting()
        {
            if (_voiceEnabled && _audioService != null)
            {
                string welcomeMessage = "Hello! Welcome to the Cybersecurity Awareness Bot. I'm here to help you stay safe online.";
                _audioService.SpeakAsync(welcomeMessage);
                LogActivity("Welcome greeting played");
            }
        }

        private void AddBotMessage(string message, string colorHex = "#00FFAA")
        {
            try
            {
                Run run = new Run(message);
                run.Foreground = (Brush)new BrushConverter().ConvertFromString(colorHex);

                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run("Bot: ") { Foreground = Brushes.LightGreen, FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(run);

                rtbChatDisplay.Document.Blocks.Add(paragraph);
                ScrollToBottom();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding bot message: {ex.Message}");
            }
        }

        private void AddUserMessage(string message)
        {
            try
            {
                string userName = _memoryService?.GetUserName() ?? "You";
                string displayName = string.IsNullOrEmpty(userName) ? "You" : userName;

                Run run = new Run(message);
                run.Foreground = Brushes.Cyan;

                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run($"{displayName}: ") { Foreground = Brushes.Yellow, FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(run);

                rtbChatDisplay.Document.Blocks.Add(paragraph);
                ScrollToBottom();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding user message: {ex.Message}");
            }
        }

        private void ScrollToBottom()
        {
            try
            {
                scrollViewer.ScrollToBottom();
            }
            catch (Exception) { }
        }

        private void BtnSaveName_Click(object sender, RoutedEventArgs e)
        {
            string name = txtUserName.Text.Trim();
            if (!string.IsNullOrEmpty(name) && name.Length >= 2)
            {
                _memoryService?.SaveUserName(name);
                AddBotMessage($"Nice to meet you, {name}! I'll remember that. What cybersecurity topic interests you?", "#00FFAA");

                if (_voiceEnabled && _audioService != null)
                {
                    string spokenGreeting = $"Nice to meet you, {name}! I'll remember that. What cybersecurity topic interests you?";
                    _audioService.SpeakAsync(spokenGreeting);
                }

                lblMemory.Text = $"Remembering: {name}";
                txtUserName.Text = "";
                LogActivity($"User saved name: {name}");
            }
            else
            {
                AddBotMessage("Please enter a valid name (at least 2 characters).", "#FF6666");
                if (_voiceEnabled && _audioService != null)
                    _audioService.SpeakAsync("Please enter a valid name with at least 2 characters.");
            }
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        private void TxtUserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUserInput();
            }
        }

        private void BtnVoice_Click(object sender, RoutedEventArgs e)
        {
            if (!_voiceEnabled)
            {
                AddBotMessage("Voice is currently OFF. Turn it ON using the toggle button.", "#FFAA00");
                return;
            }

            if (_audioService == null) return;

            string userName = _memoryService?.GetUserName() ?? string.Empty;
            string welcomeMessage = string.IsNullOrEmpty(userName)
                ? "Hello! Welcome to the Cybersecurity Awareness Bot. I'm here to help you stay safe online."
                : $"Hello {userName}! Welcome back to the Cybersecurity Awareness Bot. I'm here to help you stay safe online.";

            _audioService.SpeakAsync(welcomeMessage);
            AddBotMessage("Playing welcome message...", "#888888");
            LogActivity("Voice greeting played via button");
        }

        private void BtnVoiceOnOff_Checked(object sender, RoutedEventArgs e)
        {
            _voiceEnabled = true;
            btnVoiceOnOff.Content = "ON";
            btnVoiceOnOff.Background = (Brush)new BrushConverter().ConvertFromString("#00FFAA");
            AddBotMessage("Voice responses ENABLED.", "#00FFAA");
            LogActivity("Voice responses enabled");
        }

        private void BtnVoiceOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            _voiceEnabled = false;
            btnVoiceOnOff.Content = "OFF";
            btnVoiceOnOff.Background = (Brush)new BrushConverter().ConvertFromString("#FF4444");
            _audioService?.StopSpeaking();
            AddBotMessage("Voice responses DISABLED.", "#FFAA00");
            LogActivity("Voice responses disabled");
        }

        private void BtnStopVoice_Click(object sender, RoutedEventArgs e)
        {
            _audioService?.StopSpeaking();
            AddBotMessage("Speech stopped.", "#888888");
            LogActivity("Speech stopped by user");
        }

        private void BtnExitChat_Click(object sender, RoutedEventArgs e)
        {
            string userName = _memoryService?.GetUserName() ?? string.Empty;
            string farewellMessage = string.IsNullOrEmpty(userName)
                ? "Goodbye! Stay safe online!"
                : $"Goodbye {userName}! Stay safe online!";

            AddBotMessage(farewellMessage, "#00FFAA");
            LogActivity("Chatbot exited by user");

            if (_voiceEnabled && _audioService != null)
            {
                _audioService.SpeakAsync(farewellMessage);
                Task.Delay(1500).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() => Application.Current.Shutdown());
                });
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void BtnOpenTasks_Click(object sender, RoutedEventArgs e)
        {
            string userName = _memoryService?.GetUserName() ?? string.Empty;
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Please enter your name first before accessing tasks.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                TaskWindow taskWindow = new TaskWindow(userName);
                taskWindow.ShowDialog();
                LogActivity("Opened Task Assistant");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnOpenQuiz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = _memoryService?.GetUserName() ?? string.Empty;
                QuizWindow quizWindow = new QuizWindow(userName);
                quizWindow.ShowDialog();
                LogActivity("Opened Quiz");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening quiz: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnViewLogs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logDirectory))
                {
                    MessageBox.Show("No logs found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                string logFile = Path.Combine(logDirectory, $"chatbot_log_{DateTime.Now:yyyy-MM-dd}.txt");
                if (File.Exists(logFile))
                {
                    string logContent = File.ReadAllText(logFile);
                    if (string.IsNullOrEmpty(logContent))
                    {
                        MessageBox.Show("Log file is empty.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    Window logWindow = new Window
                    {
                        Title = "Activity Log",
                        Width = 800,
                        Height = 500,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A2E")),
                        ResizeMode = ResizeMode.CanResize
                    };

                    TextBox txtLogs = new TextBox
                    {
                        IsReadOnly = true,
                        Text = logContent,
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F3460")),
                        Foreground = Brushes.LightGreen,
                        FontFamily = new FontFamily("Consolas"),
                        FontSize = 12,
                        Margin = new Thickness(10),
                        TextWrapping = TextWrapping.Wrap,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                        HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
                    };

                    logWindow.Content = txtLogs;
                    logWindow.ShowDialog();
                    LogActivity("Viewed activity log");
                }
                else
                {
                    MessageBox.Show("No log file found for today.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error viewing logs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowRecentActions()
        {
            if (_recentActions.Count == 0)
            {
                AddBotMessage("No recent actions recorded yet.", "#FFAA00");
                return;
            }

            string logMessage = "Recent Actions:\n";
            int count = Math.Min(10, _recentActions.Count);
            for (int i = 0; i < count; i++)
            {
                logMessage += $"{i + 1}. {_recentActions[i]}\n";
            }

            if (_recentActions.Count > 10)
                logMessage += $"\n... and {_recentActions.Count - 10} more actions. Click 'Logs' to view all.";

            AddBotMessage(logMessage, "#88FF88");
        }

        private async void ProcessUserInput()
        {
            string userInput = txtUserInput.Text.Trim();

            if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                userInput.Equals("quit", StringComparison.OrdinalIgnoreCase) ||
                userInput.Equals("close", StringComparison.OrdinalIgnoreCase))
            {
                BtnExitChat_Click(null, null);
                return;
            }

            if (userInput.Equals("show activity log", StringComparison.OrdinalIgnoreCase) ||
                userInput.Equals("what have you done for me", StringComparison.OrdinalIgnoreCase) ||
                userInput.Equals("what have you done", StringComparison.OrdinalIgnoreCase) ||
                userInput.Equals("show log", StringComparison.OrdinalIgnoreCase))
            {
                ShowRecentActions();
                LogActivity("User requested activity log");
                txtUserInput.Text = "";
                return;
            }

            if (string.IsNullOrEmpty(userInput))
            {
                AddBotMessage("Please type a message. I'm here to help you stay safe online!", "#FFAA00");
                if (_voiceEnabled && _audioService != null)
                    _audioService.SpeakAsync("Please type a message. I'm here to help you stay safe online.");
                return;
            }

            AddUserMessage(userInput);
            LogActivity($"User: {userInput}");
            txtUserInput.Text = "";

            lblStatus.Text = "Bot is thinking...";
            lblStatus.Foreground = Brushes.Yellow;

            if (_sentimentService == null || _memoryService == null || _chatbotService == null)
            {
                lblStatus.Text = "Error: Services not initialized";
                return;
            }

            string sentiment = _sentimentService.DetectSentiment(userInput);
            UpdateSentimentDisplay(sentiment);

            _memoryService.StoreInformation(userInput, _currentTopic);
            UpdateMemoryDisplay();

            var response = _chatbotService.GetResponse(userInput, sentiment, _currentTopic);

            if (!string.IsNullOrEmpty(response.NewTopic))
            {
                _currentTopic = response.NewTopic;
                UpdateTopicDisplay(_currentTopic);
            }

            await Task.Delay(500);

            string responseColor = response.Category == "unknown" ? "#FFAA00" : "#00FFAA";
            AddBotMessage(response.BotAnswer, responseColor);

            string logResponse = response.BotAnswer.Length > 50 ? response.BotAnswer.Substring(0, 50) + "..." : response.BotAnswer;
            LogActivity($"Bot: {logResponse}");

            if (_voiceEnabled && _audioService != null)
                _audioService.SpeakResponse(response.BotAnswer);

            lblStatus.Text = "Ready";
            lblStatus.Foreground = Brushes.LightGreen;
        }

        private void UpdateSentimentDisplay(string sentiment)
        {
            string sentimentIcon = sentiment switch
            {
                "worried" => "😟",
                "curious" => "🤔",
                "frustrated" => "😤",
                _ => "😊"
            };
            string displaySentiment = char.ToUpper(sentiment[0]) + sentiment.Substring(1);
            lblSentiment.Text = $"{sentimentIcon} Sentiment: {displaySentiment}";
        }

        private void UpdateMemoryDisplay()
        {
            lblMemoryStatus.Text = _memoryService?.GetMemorySummary() ?? "No memory data";
        }

        private void UpdateTopicDisplay(string topic)
        {
            if (!string.IsNullOrEmpty(topic))
            {
                lblTopic.Text = $"Current topic: {topic}";
                lblTopic.Visibility = Visibility.Visible;
            }
            else
            {
                lblTopic.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            LogActivity("Chatbot closed");
            _audioService?.Dispose();
        }
    }
}