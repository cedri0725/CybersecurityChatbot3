using System;
using System.Collections.Generic;
using System.Windows;
using CybersecurityChatbotWPF.Models;

namespace CybersecurityChatbotWPF.Services
{
    public class TaskService
    {
        private List<TaskModel> _tasks = new List<TaskModel>();
        private int _nextId = 1;

        public TaskService()
        {
            // Initialize with sample tasks
            _tasks.Add(new TaskModel("Enable Two-Factor Authentication", "Set up 2FA on your email and social media accounts", DateTime.Now.AddDays(3)) { Id = _nextId++, UserName = "Anonymous" });
            _tasks.Add(new TaskModel("Review Privacy Settings", "Check and update privacy settings on all social media platforms", DateTime.Now.AddDays(5)) { Id = _nextId++, UserName = "Anonymous" });
            _tasks.Add(new TaskModel("Update Passwords", "Change passwords for important accounts to strong, unique passwords", DateTime.Now.AddDays(7)) { Id = _nextId++, UserName = "Anonymous" });
        }

        public bool AddTask(string title, string description, DateTime? reminderDate, string userName)
        {
            try
            {
                var task = new TaskModel(title, description, reminderDate)
                {
                    Id = _nextId++,
                    UserName = userName ?? "Anonymous"
                };
                _tasks.Add(task);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AddTask error: {ex.Message}");
                return false;
            }
        }

        public List<TaskModel> GetAllTasks(string userName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                    return _tasks;
                return _tasks.FindAll(t => t.UserName == userName || t.UserName == "Anonymous");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAllTasks error: {ex.Message}");
                return new List<TaskModel>();
            }
        }

        public bool MarkTaskAsCompleted(int taskId)
        {
            try
            {
                var task = _tasks.Find(t => t.Id == taskId);
                if (task != null)
                {
                    task.IsCompleted = true;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MarkTaskAsCompleted error: {ex.Message}");
                return false;
            }
        }

        public bool DeleteTask(int taskId)
        {
            try
            {
                var task = _tasks.Find(t => t.Id == taskId);
                if (task != null)
                {
                    _tasks.Remove(task);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DeleteTask error: {ex.Message}");
                return false;
            }
        }

        public void StopReminderChecker()
        {
            // No-op for in-memory version
        }
    }
}