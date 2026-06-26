using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CybersecurityChatbotWPF.Models;
using CybersecurityChatbotWPF.Services;

namespace CybersecurityChatbotWPF.Views
{
    public partial class TaskWindow : Window
    {
        private TaskService _taskService = new TaskService();
        private string _currentUser = string.Empty;
        private List<TaskModel> _currentTasks = new List<TaskModel>();
        private string _logFilePath = string.Empty;

        public TaskWindow(string userName)
        {
            InitializeComponent();
            _currentUser = userName ?? "Anonymous";
            LoadTasks();
            LogActivity("Task window opened");
        }

        private void LoadTasks()
        {
            try
            {
                _currentTasks = _taskService.GetAllTasks(_currentUser);
                lstTasks.ItemsSource = _currentTasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterTasks(string filter)
        {
            try
            {
                List<TaskModel> filteredTasks = new List<TaskModel>();

                switch (filter)
                {
                    case "pending":
                        filteredTasks = _currentTasks.FindAll(t => !t.IsCompleted);
                        break;
                    case "completed":
                        filteredTasks = _currentTasks.FindAll(t => t.IsCompleted);
                        break;
                    default:
                        filteredTasks = _currentTasks;
                        break;
                }

                lstTasks.ItemsSource = filteredTasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogActivity(string activity)
        {
            try
            {
                string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logDirectory))
                    Directory.CreateDirectory(logDirectory);

                _logFilePath = Path.Combine(logDirectory, $"chatbot_log_{DateTime.Now:yyyy-MM-dd}.txt");
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {activity}";
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Log error: {ex.Message}");
            }
        }

        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            AddTaskDialog();
        }

        private void AddTaskDialog()
        {
            try
            {
                Window dialog = new Window
                {
                    Title = "Add New Cybersecurity Task",
                    Width = 500,
                    Height = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A2E")),
                    ResizeMode = ResizeMode.NoResize
                };

                StackPanel panel = new StackPanel { Margin = new Thickness(20) };

                // Title input
                panel.Children.Add(new TextBlock { Text = "Task Title:", Foreground = Brushes.White, FontSize = 14, Margin = new Thickness(0, 0, 0, 5) });
                TextBox txtTitle = new TextBox { Height = 30, Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F3460")), Foreground = Brushes.White, BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFAA")), BorderThickness = new Thickness(1) };
                panel.Children.Add(txtTitle);

                // Description input
                panel.Children.Add(new TextBlock { Text = "Description:", Foreground = Brushes.White, FontSize = 14, Margin = new Thickness(0, 10, 0, 5) });
                TextBox txtDescription = new TextBox { Height = 60, TextWrapping = TextWrapping.Wrap, Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F3460")), Foreground = Brushes.White, BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFAA")), BorderThickness = new Thickness(1) };
                panel.Children.Add(txtDescription);

                // Reminder checkbox
                CheckBox chkReminder = new CheckBox { Content = "Set Reminder", Foreground = Brushes.White, FontSize = 14, Margin = new Thickness(0, 10, 0, 5) };
                panel.Children.Add(chkReminder);

                // Date picker
                DatePicker dpReminder = new DatePicker { Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F3460")), Foreground = Brushes.White, BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFAA")), BorderThickness = new Thickness(1), Margin = new Thickness(0, 0, 0, 10) };
                dpReminder.SelectedDate = DateTime.Now.AddDays(7);
                panel.Children.Add(dpReminder);

                // Buttons
                StackPanel buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0, 10, 0, 0) };

                Button btnSave = new Button { Content = "Save Task", Width = 100, Height = 35, Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFAA")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A2E")), FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 10, 0) };
                btnSave.Click += (s, args) =>
                {
                    if (string.IsNullOrWhiteSpace(txtTitle.Text))
                    {
                        MessageBox.Show("Please enter a task title.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    DateTime? reminderDate = chkReminder.IsChecked == true ? dpReminder.SelectedDate : null;
                    bool success = _taskService.AddTask(txtTitle.Text, txtDescription.Text, reminderDate, _currentUser);

                    if (success)
                    {
                        LogActivity($"Task added: {txtTitle.Text}");
                        MessageBox.Show("Task added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        dialog.Close();
                        LoadTasks();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add task. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                };
                buttonPanel.Children.Add(btnSave);

                Button btnCancel = new Button { Content = "Cancel", Width = 100, Height = 35, Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4444")), Foreground = Brushes.White, FontWeight = FontWeights.Bold };
                btnCancel.Click += (s, args) => dialog.Close();
                buttonPanel.Children.Add(btnCancel);

                panel.Children.Add(buttonPanel);
                dialog.Content = panel;
                dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating task dialog: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCompleteTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TaskModel selectedTask = lstTasks.SelectedItem as TaskModel;
                if (selectedTask == null)
                {
                    MessageBox.Show("Please select a task to complete.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (selectedTask.IsCompleted)
                {
                    MessageBox.Show("This task is already completed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBoxResult result = MessageBox.Show($"Mark '{selectedTask.Title}' as completed?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    bool success = _taskService.MarkTaskAsCompleted(selectedTask.Id);
                    if (success)
                    {
                        LogActivity($"Task completed: {selectedTask.Title}");
                        MessageBox.Show("Task marked as completed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadTasks();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update task.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error completing task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TaskModel selectedTask = lstTasks.SelectedItem as TaskModel;
                if (selectedTask == null)
                {
                    MessageBox.Show("Please select a task to delete.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBoxResult result = MessageBox.Show($"Delete '{selectedTask.Title}'?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    bool success = _taskService.DeleteTask(selectedTask.Id);
                    if (success)
                    {
                        LogActivity($"Task deleted: {selectedTask.Title}");
                        MessageBox.Show("Task deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadTasks();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete task.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadTasks();
            LogActivity("Tasks refreshed");
        }

        private void BtnAllTasks_Click(object sender, RoutedEventArgs e)
        {
            FilterTasks("all");
            SetActiveFilterButton(btnAllTasks);
        }

        private void BtnPendingTasks_Click(object sender, RoutedEventArgs e)
        {
            FilterTasks("pending");
            SetActiveFilterButton(btnPendingTasks);
        }

        private void BtnCompletedTasks_Click(object sender, RoutedEventArgs e)
        {
            FilterTasks("completed");
            SetActiveFilterButton(btnCompletedTasks);
        }

        private void SetActiveFilterButton(Button activeButton)
        {
            Button[] filterButtons = { btnAllTasks, btnPendingTasks, btnCompletedTasks };
            foreach (Button btn in filterButtons)
            {
                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F3460"));
                btn.Foreground = Brushes.White;
            }

            if (activeButton != null)
            {
                activeButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFAA"));
                activeButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A2E"));
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            LogActivity("Task window closed");
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _taskService?.StopReminderChecker();
        }
    }
}