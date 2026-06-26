using System;

namespace CybersecurityChatbotWPF.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public string UserName { get; set; } = string.Empty;

        public TaskModel()
        {
            CreatedDate = DateTime.Now;
            IsCompleted = false;
        }

        public TaskModel(string title, string description, DateTime? reminderDate = null)
        {
            Title = title ?? string.Empty;
            Description = description ?? string.Empty;
            ReminderDate = reminderDate;
            CreatedDate = DateTime.Now;
            IsCompleted = false;
        }

        public string GetStatusDisplay()
        {
            return IsCompleted ? "Completed" : "Pending";
        }

        public string GetReminderDisplay()
        {
            if (ReminderDate.HasValue)
                return ReminderDate.Value.ToString("yyyy-MM-dd HH:mm");
            return "No reminder set";
        }
    }
}