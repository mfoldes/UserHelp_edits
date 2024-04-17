﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Appy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static StringBuilder sortOutput = null;
        private static int numOutputLines = 0;
        private StringBuilder ConsoleOutputLines;
        List<Classes.Category> Categories = new List<Classes.Category>();

        public MainWindow()
        {

            InitializeComponent();

            ConsoleOutputLines = new StringBuilder();
            string userHelpFilePath = Environment.GetEnvironmentVariable("ProgramData") + "\\UserHelp\\UserHelp.json";

            //This variable isn't needed. File copy functionality has been removed.
            //string WorkingDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); 

            Console.WriteLine(userHelpFilePath);

            if (!System.IO.File.Exists(userHelpFilePath))
            {
                Console.WriteLine("UserHelp.json not found. A Xaml Window notating that will go here.");
            }
            else
            {
                Categories = Appy.Data.ParseCategoriesJSON.GetData(userHelpFilePath);
                string title = this.Title;
                EventLog.WriteEntry("Application", $"{title}:\nOpened {userHelpFilePath}", EventLogEntryType.Information, 1000);
                Appy.Views.CategoriesPage categoryPage = new Views.CategoriesPage(ref Categories, ref NavigationFrame, ref ConsoleOutputLines);
                NavigationFrame.Navigate(categoryPage);
            }
        }

        private void PowerShellOutputHandler(object sendingProcess,
            DataReceivedEventArgs outLine)
        {
            // Collect the sort command output.
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                numOutputLines++;

                // Add the text to the collected output.
                sortOutput.Append(Environment.NewLine +
                    $"[{numOutputLines}] - {outLine.Data}");
#if DEBUG
                Console.WriteLine(Environment.NewLine +
                    $"[{numOutputLines}] - {outLine.Data}");
#endif

            }
        }
    }
}
