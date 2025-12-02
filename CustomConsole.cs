using Colossal.Patches;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Colossal
{
    public class CustomConsole : MonoBehaviour
    {
        public static bool isBeta = false;

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        private IntPtr consoleWindowHandle;
        private StreamWriter logWriter;
        private StreamWriter fileWriter; // StreamWriter to log to a file

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeConsole();

        private void Awake()
        {
            if(isBeta)
            {
                // Other initialization logic...

                AllocConsole();
                consoleWindowHandle = GetConsoleWindow();
                ShowWindow(consoleWindowHandle, SW_SHOW);

                // Set up console to write to both the standard output and the log file
                logWriter = new StreamWriter(System.Console.OpenStandardOutput());
                logWriter.AutoFlush = true;
                System.Console.SetOut(logWriter);

                // Initialize the file writer for logging to file
                string logFilePath = "Colossal\\BetaLog.txt";
                if (!Directory.Exists("Colossal"))
                {
                    Directory.CreateDirectory("Colossal");
                }
                if (File.Exists(logFilePath))
                {
                    File.Delete(logFilePath);
                }

                fileWriter = new StreamWriter(logFilePath, append: true);
                fileWriter.AutoFlush = true;

                System.Console.SetOut(new MultiWriter(logWriter, fileWriter));

                System.Console.WriteLine(
                    @"
                _________  ________  .____    ________    _________ _________   _____  .____     
                \_   ___ \ \_____  \ |    |   \_____  \  /   _____//   _____/  /  _  \ |    |    
                /    \  \/  /   |   \|    |    /   |   \ \_____  \ \_____  \  /  /_\  \|    |    
                \     \____/    |    \    |___/    |    \/        \/        \/    |    \    |___ 
                 \______  /\_______  /_______ \_______  /_______  /_______  /\____|__  /_______ \
                        \/         \/        \/       \/        \/        \/         \/        \/
                                      Trolling lemming since 21/05/07.
                ");
            }
        }

        private void OnApplicationQuit()
        {
            FreeConsole();
            logWriter.Close();
            fileWriter.Close();
        }

        public static void Debug(string message)
        {
            if (isBeta)
            {
                // Set color to blue for debugging messages
                System.Console.ForegroundColor = ConsoleColor.Magenta;
                System.Console.WriteLine($"[COLOSSAL DEBUG] {message}");
                System.Console.ResetColor();
            }
        }

        public static void Error(string message)
        {
            if (isBeta)
            {
                // Set color to red for error messages
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($"[COLOSSAL ERROR] {message}");
                System.Console.ResetColor();
            }
        }

        private class MultiWriter : TextWriter
        {
            private TextWriter _consoleWriter;
            private TextWriter _fileWriter;

            public MultiWriter(TextWriter consoleWriter, TextWriter fileWriter)
            {
                _consoleWriter = consoleWriter;
                _fileWriter = fileWriter;
            }

            public override void Write(char value)
            {
                _consoleWriter.Write(value);
                _fileWriter.Write(value);
            }

            public override void Write(string value)
            {
                _consoleWriter.Write(value);
                _fileWriter.Write(value);
            }

            public override void WriteLine(string value)
            {
                _consoleWriter.WriteLine(value);
                _fileWriter.WriteLine(value);
            }

            public override Encoding Encoding => _consoleWriter.Encoding;
        }
    }
}
