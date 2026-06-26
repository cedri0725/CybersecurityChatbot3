using System;
using System.Speech.Synthesis;

namespace CybersecurityChatbotWPF.Services
{
    public class AudioService
    {
        private SpeechSynthesizer? _synthesizer;

        public AudioService()
        {
            try
            {
                _synthesizer = new SpeechSynthesizer();
                _synthesizer.SetOutputToDefaultAudioDevice();
                _synthesizer.Rate = 0;
                _synthesizer.Volume = 100;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Speech error: {ex.Message}");
            }
        }

        public void Speak(string text)
        {
            try
            {
                if (_synthesizer != null)
                {
                    _synthesizer.Speak(text);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Speak error: {ex.Message}");
            }
        }

        public void SpeakAsync(string text)
        {
            try
            {
                if (_synthesizer != null)
                {
                    _synthesizer.SpeakAsync(text);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SpeakAsync error: {ex.Message}");
            }
        }

        public void SpeakResponse(string response)
        {
            try
            {
                if (_synthesizer != null)
                {
                    string cleanResponse = System.Text.RegularExpressions.Regex.Replace(response, @"[^\u0000-\u007F]+", "");
                    _synthesizer.SpeakAsync(cleanResponse);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SpeakResponse error: {ex.Message}");
            }
        }

        public void StopSpeaking()
        {
            try
            {
                if (_synthesizer != null)
                {
                    _synthesizer.SpeakAsyncCancelAll();
                }
            }
            catch (Exception) { }
        }

        public void Dispose()
        {
            try
            {
                if (_synthesizer != null)
                {
                    _synthesizer.Dispose();
                }
            }
            catch (Exception) { }
        }
    }
}