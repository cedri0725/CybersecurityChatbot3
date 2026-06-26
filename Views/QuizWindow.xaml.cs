using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CybersecurityChatbotWPF.Views
{
    public partial class QuizWindow : Window
    {
        private List<QuizQuestion> _questions = new List<QuizQuestion>();
        private int _currentQuestionIndex = 0;
        private int _score = 0;
        private bool _answered = false;
        private string _userName = string.Empty;

        public QuizWindow(string userName = "")
        {
            InitializeComponent();
            _userName = userName ?? string.Empty;
            InitializeQuestions();
            UpdateScoreDisplay();
            lblFeedback.Text = "Ready to test your cybersecurity knowledge!";
        }

        private void InitializeQuestions()
        {
            _questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What is a strong password?",
                    Options = new List<string> { "Your birthdate", "'password123'", "A mix of uppercase, lowercase, numbers, and symbols", "Your pet's name" },
                    CorrectAnswer = 2,
                    Explanation = "Strong passwords use a combination of uppercase, lowercase, numbers, and special characters."
                },
                new QuizQuestion
                {
                    Question = "What should you do if you receive a suspicious email asking for personal information?",
                    Options = new List<string> { "Reply with the requested information", "Click the link in the email", "Delete the email and report it as phishing", "Forward it to your friends" },
                    CorrectAnswer = 2,
                    Explanation = "Never respond to suspicious emails. Delete them and report them as phishing."
                },
                new QuizQuestion
                {
                    Question = "What is Two-Factor Authentication (2FA)?",
                    Options = new List<string> { "Using two different passwords", "An extra security layer requiring two verification methods", "Having two email accounts", "Using two different browsers" },
                    CorrectAnswer = 1,
                    Explanation = "2FA adds an extra layer of security by requiring a second verification method."
                },
                new QuizQuestion
                {
                    Question = "What is phishing?",
                    Options = new List<string> { "A type of computer virus", "A fishing technique", "A scam attempt where attackers pose as trusted entities", "A password manager" },
                    CorrectAnswer = 2,
                    Explanation = "Phishing is a scam where attackers pretend to be trusted organizations."
                },
                new QuizQuestion
                {
                    Question = "Why is it important to update your software regularly?",
                    Options = new List<string> { "To get new features", "To fix security vulnerabilities", "To improve performance", "All of the above" },
                    CorrectAnswer = 3,
                    Explanation = "Software updates fix security vulnerabilities that attackers could exploit."
                },
                new QuizQuestion
                {
                    Question = "What should you do with public Wi-Fi networks?",
                    Options = new List<string> { "Use them without any protection", "Avoid entering sensitive information", "Use a VPN for security", "Both B and C" },
                    CorrectAnswer = 3,
                    Explanation = "Public Wi-Fi is insecure. Use a VPN and avoid sensitive information."
                },
                new QuizQuestion
                {
                    Question = "What is a password manager?",
                    Options = new List<string> { "A tool that stores all your passwords securely", "A password guessing tool", "A type of malware", "A physical device" },
                    CorrectAnswer = 0,
                    Explanation = "Password managers securely store and manage all your passwords."
                },
                new QuizQuestion
                {
                    Question = "How often should you update your passwords?",
                    Options = new List<string> { "Never", "Every month", "Every 3-6 months or when a breach occurs", "Every year" },
                    CorrectAnswer = 2,
                    Explanation = "Update passwords every 3-6 months or immediately after a breach."
                },
                new QuizQuestion
                {
                    Question = "True or False: Using the same password for multiple accounts is safe.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = 1,
                    Explanation = "False! Using the same password across multiple accounts is dangerous."
                },
                new QuizQuestion
                {
                    Question = "What is social engineering?",
                    Options = new List<string> { "Building social networks online", "A programming technique", "Psychological manipulation to trick people into revealing information", "A type of computer hardware" },
                    CorrectAnswer = 2,
                    Explanation = "Social engineering exploits human psychology to gain unauthorized access."
                }
            };
        }

        private void UpdateScoreDisplay()
        {
            if (_questions.Count > 0)
                lblScore.Text = $"Score: {_score} | Questions: {_currentQuestionIndex + 1}/{_questions.Count}";
            else
                lblScore.Text = "Score: 0 | Questions: 0";
        }

        private void DisplayQuestion()
        {
            if (_currentQuestionIndex >= _questions.Count)
            {
                ShowQuizComplete();
                return;
            }

            var question = _questions[_currentQuestionIndex];
            lblQuestion.Text = question.Question;
            lblQuestionNumber.Text = $"Question {_currentQuestionIndex + 1} of {_questions.Count}";
            lblFeedback.Text = "Select an answer below:";
            lblFeedback.Foreground = Brushes.LightGray;
            _answered = false;
            btnNext.IsEnabled = false;

            spAnswers.Children.Clear();

            for (int i = 0; i < question.Options.Count; i++)
            {
                int optionIndex = i;
                Button btnOption = new Button
                {
                    Content = question.Options[i],
                    Tag = i,
                    Width = 500,
                    Height = 40,
                    Margin = new Thickness(0, 5, 0, 5),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F3460")),
                    Foreground = Brushes.White,
                    FontSize = 14,
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFAA")),
                    BorderThickness = new Thickness(1),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                btnOption.Click += (s, e) => AnswerQuestion(optionIndex);
                spAnswers.Children.Add(btnOption);
            }

            UpdateScoreDisplay();
        }

        private void AnswerQuestion(int selectedIndex)
        {
            if (_answered) return;

            var question = _questions[_currentQuestionIndex];
            _answered = true;

            foreach (Button btn in spAnswers.Children)
            {
                btn.IsEnabled = false;
            }

            Button selectedBtn = spAnswers.Children[selectedIndex] as Button;

            if (selectedIndex == question.CorrectAnswer)
            {
                _score++;
                lblFeedback.Text = $"Correct! {question.Explanation}";
                lblFeedback.Foreground = Brushes.LightGreen;
                selectedBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFAA"));
                selectedBtn.Foreground = Brushes.Black;
            }
            else
            {
                lblFeedback.Text = $"Incorrect. {question.Explanation}";
                lblFeedback.Foreground = Brushes.OrangeRed;
                selectedBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4444"));

                Button correctBtn = spAnswers.Children[question.CorrectAnswer] as Button;
                correctBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFAA"));
                correctBtn.Foreground = Brushes.Black;
            }

            UpdateScoreDisplay();

            if (_currentQuestionIndex < _questions.Count - 1)
            {
                btnNext.IsEnabled = true;
                btnNext.Content = "Next Question";
            }
            else
            {
                btnNext.IsEnabled = true;
                btnNext.Content = "View Results";
            }
        }

        private void ShowQuizComplete()
        {
            lblQuestion.Text = "Quiz Complete!";
            lblQuestionNumber.Text = "";
            spAnswers.Children.Clear();
            btnNext.IsEnabled = false;
            btnStartQuiz.IsEnabled = true;

            double percentage = _questions.Count > 0 ? (_score / (double)_questions.Count) * 100 : 0;
            string feedback = $"You scored {_score} out of {_questions.Count} ({percentage:F0}%)!";

            if (percentage >= 80)
                feedback += "\nExcellent! You're a cybersecurity expert!";
            else if (percentage >= 60)
                feedback += "\nGood job! You have a solid understanding.";
            else if (percentage >= 40)
                feedback += "\nNot bad! Review the topics to improve your score.";
            else
                feedback += "\nKeep learning! Cybersecurity is important for everyone.";

            lblFeedback.Text = feedback;
            lblFeedback.Foreground = Brushes.Cyan;
        }

        private void BtnStartQuiz_Click(object sender, RoutedEventArgs e)
        {
            _currentQuestionIndex = 0;
            _score = 0;
            _answered = false;
            btnStartQuiz.IsEnabled = false;
            DisplayQuestion();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            _currentQuestionIndex++;
            if (_currentQuestionIndex < _questions.Count)
            {
                DisplayQuestion();
            }
            else
            {
                ShowQuizComplete();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class QuizQuestion
    {
        public string Question { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
        public int CorrectAnswer { get; set; }
        public string Explanation { get; set; } = string.Empty;
    }
}