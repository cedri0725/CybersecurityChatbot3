Cybersecurity Awareness Chatbot

Project Overview

This is a Windows Presentation Foundation (WPF) desktop application developed for PROG6221 - Programming 2B Part 3. The application is a cybersecurity awareness chatbot that educates South African citizens about online safety threats including password security, phishing scams, privacy protection, and safe browsing habits.

The chatbot features a graphical user interface with text-to-speech voice capabilities, sentiment detection, memory recall, task management, a cybersecurity quiz, and activity logging.

Features

Core Features

- Graphical User Interface built with WPF
- Text-to-speech voice responses using System.Speech
- Voice toggle to enable or disable speech output
- Stop speaking button to interrupt current speech
- Keyboard support (press Enter to send messages)
- Animated rainbow ASCII art banner
- User name memory and personalized greetings
- Sentiment detection (worried, curious, frustrated, neutral)
- Keyword recognition for cybersecurity topics
- Random responses using arrays and lists
- Follow-up question handling
- Exit commands (type 'exit', 'quit', or 'close')
- Exit button to close the application gracefully

Part 3 Features

- Task management system with reminders
- MySQL database integration for task storage
- Add, view, complete, and delete tasks
- Cybersecurity quiz with 10+ questions
- Multiple choice and true/false question types
- Immediate feedback with explanations
- Score tracking and final results
- Natural Language Processing simulation with regex patterns
- Activity logging with file-based storage
- View activity log from the GUI
- Show recent actions command

Technology Stack

- C# .NET 8.0
- Windows Presentation Foundation (WPF)
- System.Speech (text-to-speech)
- MySQL (optional - file-based storage also available)
- XAML for user interface design

Project Structure

```
CybersecurityChatbotWPF/
├── Models/
│   ├── ChatbotResponse.cs
│   ├── TaskModel.cs
│   └── UserProfile.cs
├── Services/
│   ├── AudioService.cs
│   ├── ChatbotService.cs
│   ├── MemoryService.cs
│   ├── SentimentService.cs
│   └── TaskService.cs
├── Views/
│   ├── QuizWindow.xaml
│   ├── QuizWindow.xaml.cs
│   ├── TaskWindow.xaml
│   └── TaskWindow.xaml.cs
├── App.xaml
├── App.xaml.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
└── README.md
```

Class Descriptions

| File | Purpose |
|------|---------|
| MainWindow.xaml | GUI layout with chat display, input box, buttons, and status panels |
| MainWindow.xaml.cs | Main application logic, event handlers, and UI updates |
| ChatbotService.cs | Keyword detection, response selection, and follow-up handling |
| SentimentService.cs | Detects user mood from input text |
| MemoryService.cs | Stores user name and conversation history |
| AudioService.cs | Handles text-to-speech functionality |
| TaskService.cs | Manages cybersecurity tasks and reminders |
| DatabaseHelper.cs | Handles MySQL database operations (or file-based storage) |
| ChatbotResponse.cs | Data model for storing conversation responses |
| UserProfile.cs | Data model for storing user information |
| TaskModel.cs | Data model for storing task information |

Installation Instructions

Prerequisites

- Visual Studio 2022 or later
- .NET 8.0 SDK
- Windows operating system (required for text-to-speech)
- MySQL (optional - for database storage)

Setup Steps

1. Clone the repository
```
git clone https://github.com/your-username/CybersecurityChatbotWPF.git
```

2. Open the solution file in Visual Studio
```
Double-click CybersecurityChatbotWPF.sln
```

3. Restore NuGet packages
- Right-click on the solution
- Select "Restore NuGet Packages"

4. Build the solution
- Press Ctrl + Shift + B

5. Run the application
- Press F5

How to Use

Starting the Application

1. Launch the application from Visual Studio
2. The rainbow animated ASCII art banner will display
3. A voice greeting will play automatically
4. The chatbot will introduce itself

Saving Your Name

1. Type your name in the text box labeled "Your Name"
2. Click the "Save" button
3. The chatbot will remember your name and use it in future responses

Chatting with the Bot

Type your questions in the input box at the bottom and press Enter or click the Send button.

Example Questions

| Topic | Example Question |
|-------|------------------|
| Password safety | "Tell me about passwords" |
| Phishing | "What is phishing?" |
| Scams | "How to spot a scam?" |
| Privacy | "Privacy tips" |
| Help | "Help" or "What can you ask?" |
| Greeting | "Hello" or "How are you?" |
| Feelings | "I feel worried about online security" |

Voice Controls

| Control | Function |
|---------|----------|
| Voice Toggle (ON/OFF) | Enable or disable voice responses |
| Stop Speaking | Immediately stop any ongoing speech |
| Voice Button | Replay the welcome greeting |

Task Management

| Button | Function |
|--------|----------|
| Tasks | Open the task management window |
| Add Task | Add a new cybersecurity task with reminder |
| Complete | Mark selected task as completed |
| Delete | Delete selected task |
| Refresh | Refresh the task list |

Quiz

| Button | Function |
|--------|----------|
| Quiz | Open the cybersecurity quiz |
| Start Quiz | Begin the quiz |
| Next Question | Go to the next question |
| Close | Close the quiz window |

Activity Log

| Button | Function |
|--------|----------|
| Logs | View the activity log |
| Show activity log | Type this command to see recent actions |

Exiting the Application

You can exit in three ways:
- Click the "Exit" button
- Type "exit" in the chat
- Type "quit" in the chat
- Type "close" in the chat

Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Enter | Send message |
| F5 | Run the application |
| Ctrl + Shift + B | Build the solution |

Response Categories

The chatbot recognizes the following keywords and provides appropriate responses:

| Category | Keywords |
|----------|----------|
| Password | password, passphrase, login, credentials |
| Scam | scam, fraud, fake |
| Privacy | privacy, private, personal information |
| Phishing | phish, phishing |
| Help | help, options, what can you do |
| Feeling | how are you, i feel, i'm feeling |

Database Setup (Optional)

Using MySQL

1. Install MySQL (XAMPP or MySQL Workbench)
2. Create a database named `cybersecurity_chatbot`
3. Run the SQL script to create the tasks table
4. Update the connection string in DatabaseHelper.cs

Using File-Based Storage (Default)

The application uses file-based storage by default. Tasks are saved in:
```
C:\Users\YourName\AppData\Local\CybersecurityChatbot\tasks.json
```

No additional setup is required for file-based storage.

Error Handling

- Empty messages prompt the user to type a question
- Unrecognized keywords return a helpful default response
- Voice system errors are caught and logged without crashing
- Invalid name entries (less than 2 characters) are rejected
- Database connection errors are handled gracefully

GitHub Requirements

- Minimum of six commits with meaningful messages
- Two releases with tags

\\Sample Conversation\\

```
User: Hello
Bot: Hi there! How can I help you stay safe online today? How are you feeling today?

User: I'm worried about online scams
Bot: I understand your concern. Never click on links in suspicious emails. Always hover over links to see the real URL first. Would you like to learn about a specific cybersecurity topic to feel more confident?

User: Tell me about passwords
Bot: Use strong passwords with at least 12 characters including uppercase, lowercase, numbers, and symbols!

User: exit
Bot: Goodbye! Stay safe online!
```

Troubleshooting

| Issue | Solution |
|-------|----------|
| No voice output | Check that speakers are working and System.Speech NuGet package is installed |
| Voice not working on some systems | Text-to-speech requires Windows operating system |
| Build errors | Clean the solution and rebuild (Build > Clean Solution, then Build > Rebuild Solution) |
| Namespace errors | Ensure all files use the same namespace: CybersecurityChatbotWPF |
| Database connection errors | Check MySQL is running and connection string is correct |

Future Enhancements

- Save conversation history to a file
- Add more cybersecurity topics
- Export chat logs
- Add dark/light theme toggle
- Add more quiz questions

Author

Name: Your Name
Student Number: Your Student Number
Course: PROG6221 - Programming 2B
Institution: Your Institution
Year: 2026

Acknowledgments

- Department of Cybersecurity for the campaign initiative
- Lecturers and tutors for guidance

License

This project is for educational purposes as part of PROG6221 coursework.

Reference

Pieterse, H. 2021. The Cyber Threat Landscape in South Africa: A 10-Year Review. The African Journal of Information and Communication, 28(28).

Stay Safe Online

Think before you click. Use strong passwords. Enable two-factor authentication. Always verify the sender of unexpected emails.

**End of README**