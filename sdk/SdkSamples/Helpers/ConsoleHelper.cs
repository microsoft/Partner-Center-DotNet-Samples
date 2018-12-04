// -----------------------------------------------------------------------
// <copyright file="ConsoleHelper.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Helpers
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Store.PartnerCenter.Models;

    /// <summary>
    /// Provides useful helpers that aid in writing to the console.
    /// </summary>
    public class ConsoleHelper : IDisposable
    {
        /// <summary>
        /// A lazy reference to the singleton console helper instance.
        /// </summary>
        private static Lazy<ConsoleHelper> instance = new Lazy<ConsoleHelper>(() => new ConsoleHelper());

        /// <summary>
        /// A task that displays progress indicator on the console.
        /// </summary>
        private Task progressBackgroundTask;

        /// <summary>
        /// A token source which controls cancelling the progress indicator.
        /// </summary>
        private CancellationTokenSource progressCancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Prevents a default instance of the <see cref="ConsoleHelper"/> class from being created.
        /// </summary>
        private ConsoleHelper()
        {
        }

        /// <summary>
        /// Gets the single instance of the <see cref="ConsoleHelper"/>.
        /// </summary>
        public static ConsoleHelper Instance
        {
            get
            {
                return ConsoleHelper.instance.Value;
            }
        }

        /// <summary>
        /// Writes a success message to the console.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="newLine">Whether or not to write a new line after the message.</param>
        public void Success(string message, bool newLine = true)
        {
            this.WriteColored(message, ConsoleColor.Green, newLine);
        }

        /// <summary>
        /// Writes a progress message to the console and starts a progress animation.
        /// </summary>
        /// <param name="message">The progress message to write.</param>
        public void StartProgress(string message)
        {
            if (this.progressBackgroundTask == null || this.progressBackgroundTask.Status != TaskStatus.Running)
            {
                this.progressBackgroundTask = new Task(() =>
                {
                    int dotCounter = 0;

                    while (!this.progressCancellationTokenSource.Token.IsCancellationRequested)
                    {
                        var initialCursorPositionX = Console.CursorLeft;
                        var initialCursorPositionY = Console.CursorTop;

                        for (dotCounter = 0; dotCounter < 5; dotCounter++)
                        {
                            this.WriteColored(".", ConsoleColor.DarkCyan, false);
                            Thread.Sleep(200);

                            if (this.progressCancellationTokenSource.Token.IsCancellationRequested)
                            {
                                return;
                            }
                        }
                        
                        // Erase dots.
                        Console.SetCursorPosition(initialCursorPositionX, initialCursorPositionY);
                        for (int i = 0; i < dotCounter; ++i)
                        {
                            Console.Write(" ");
                        }

                        Console.SetCursorPosition(initialCursorPositionX, initialCursorPositionY);
                    }
                });

                Console.WriteLine();
                this.WriteColored(message, ConsoleColor.DarkCyan, false);
                this.progressBackgroundTask.Start();
            }
        }

        /// <summary>
        /// Stops the progress animation.
        /// </summary>
        public void StopProgress()
        {
            if (this.progressBackgroundTask != null && this.progressBackgroundTask.Status == TaskStatus.Running)
            {
                this.progressCancellationTokenSource.Cancel();
                this.progressBackgroundTask.Wait();
                this.progressBackgroundTask.Dispose();
                this.progressBackgroundTask = null;

                this.progressCancellationTokenSource.Dispose();
                this.progressCancellationTokenSource = new CancellationTokenSource();

                Console.WriteLine();
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Writes a warning message to the console.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="newLine">Whether or not to write a new line after the message.</param>
        public void Warning(string message, bool newLine = true)
        {
            this.WriteColored(message, ConsoleColor.Yellow, newLine);
        }

        /// <summary>
        /// Writes an error message to the console.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="newLine">Whether or not to write a new line after the message.</param>
        public void Error(string message, bool newLine = true)
        {
            this.WriteColored(message, ConsoleColor.Red, newLine);
        }

        /// <summary>
        /// Writes a message with the requested color to the console.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="color">The console color to use.</param>
        /// <param name="newLine">Whether or not to write a new line after the message.</param>
        public void WriteColored(string message, ConsoleColor color, bool newLine = true)
        {
            Console.ForegroundColor = color;
            Console.Write(message + (newLine ? "\n" : string.Empty));
            Console.ResetColor();
        }

        /// <summary>
        /// Reads a non empty string from the console.
        /// </summary>
        /// <param name="promptMessage">The prompt message to display.</param>
        /// <param name="validationMessage">The error message to show in case of user input error.</param>
        /// <returns>The string input from the console.</returns>
        public string ReadNonEmptyString(string promptMessage, string validationMessage = default(string))
        {
            string input = string.Empty;
            validationMessage = validationMessage ?? "Enter a non-empty value";

            while (true)
            {
                Console.Write("{0}: ", promptMessage);
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    this.Error(validationMessage);
                }
                else
                {
                    break;
                }
            }

            return input;
        }

        /// <summary>
        /// Reads a string from the console (it can be empty as it is intended to be used with optional values).
        /// </summary>
        /// <param name="promptMessage">The prompt message to display.</param>
        /// <returns>The string input from the console.</returns>
        public string ReadOptionalString(string promptMessage)
        {
            string input = string.Empty;
            Console.Write("{0}: ", promptMessage);
            input = Console.ReadLine();
            return input;
        }

        /// <summary>
        /// Writes an object and its properties recursively to the console. Properties are automatically indented.
        /// </summary>
        /// <param name="object">The object to print to the console.</param>
        /// <param name="title">An optional title to display.</param>
        /// <param name="indent">The starting indentation.</param>
        public void WriteObject(object @object, string title = default(string), int indent = 0)
        {
            if (@object == null)
            {
                return;
            }

            const int TabSize = 4;
            bool isTitlePresent = !string.IsNullOrWhiteSpace(title);
            string indentString = new string(' ', indent * TabSize);
            Type objectType = @object.GetType();
            var collection = @object as ICollection;

            if (objectType.Assembly.FullName == typeof(ResourceBase).Assembly.FullName && objectType.IsClass)
            {
                // this is a partner SDK model class, iterate over it's properties recursively
                if (indent == 0 && !string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine(title);
                    Console.WriteLine(new string('-', 90));
                }
                else if (isTitlePresent)
                {
                    this.WriteColored(string.Format(CultureInfo.InvariantCulture, "{0}{1}: ", indentString, title), ConsoleColor.Yellow);
                }
                else
                {
                    // since the current element does not have a title, we do not want to shift it's children causing a double shift appearance
                    // to the user
                    indent--;
                }

                PropertyInfo[] properties = objectType.GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    this.WriteObject(property.GetValue(@object), property.Name, indent + 1);
                }

                if (indent == 0 && !string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine(new string('-', 90));
                }
            }
            else if (collection != null)
            {
                // this is a collection, loop through its element and print them recursively
                this.WriteColored(string.Format(CultureInfo.InvariantCulture, isTitlePresent ? "{0}{1}: " : string.Empty, indentString, title), ConsoleColor.Yellow);

                foreach (var element in collection)
                {
                    this.WriteObject(element, indent: indent + 1);
                    var elementType = element.GetType();

                    if (indent == 1)
                    {
                        Console.WriteLine(new string('-', 80));
                    }
                }
            }
            else
            {
                // print the object as is
                this.WriteColored(string.Format(CultureInfo.InvariantCulture, isTitlePresent ? "{0}{1}: " : "{0}", indentString, title), ConsoleColor.DarkYellow, false);
                Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}", @object));
            }
        }

        /// <summary>
        /// Disposes of the console helper instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the console helper instance.
        /// </summary>
        /// <param name="shouldDispose">Whether to dispose the object or not.</param>
        private void Dispose(bool shouldDispose)
        {
            if (shouldDispose)
            {
                this.StopProgress();

                if (this.progressBackgroundTask != null)
                {
                    this.progressBackgroundTask.Dispose();
                }

                if (this.progressCancellationTokenSource != null)
                {
                    this.progressCancellationTokenSource.Dispose();
                    this.progressCancellationTokenSource = null;
                }
            }
        }
    }
}
