﻿// Copyright(c) João Martiniano. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using CsvHelper;

namespace MaxiBug
{
    /// <summary>
    /// Stores the issues and tasks of a software project in memory.
    /// </summary>
    [Serializable]
    public class Project
    {
        /// <summary>
        /// Gets the file format version of the project file.
        /// </summary>
        [JsonProperty]
        public string Version { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the current value of issue ID counter: the next issue created will have this value. This property is incremented automatically.
        /// </summary>
        [JsonProperty]
        public int IssueIdCounter { get; set; } = 0;

        /// <summary>
        /// Gets the current value of task ID counter: the next task created will have this value. This property is incremented automatically.
        /// </summary>
        [JsonProperty]
        public int TaskIdCounter { get; private set; } = 0;

        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database name in lower case.
        /// </summary>
        public string DbName { get; set; } = string.Empty;

        /// <summary>
        /// Deprecated: Gets or sets the name of the project file.
        /// </summary>
        [JsonIgnore]
        public string Filename { get; set; } = string.Empty;

        /// <summary>
        /// Deprecated: Gets or sets the location of the project file.
        /// </summary>
        [JsonIgnore]
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the project issues.
        /// </summary>
        public Dictionary<int, Issue> Issues { get; set; } = new Dictionary<int, Issue>();

        /// <summary>
        /// Gets or sets the project tasks.
        /// </summary>
        public Dictionary<int, Task> Tasks { get; set; } = new Dictionary<int, Task>();

        /// <summary>
        /// Gets or sets the user names.
        /// </summary>
        public List<string> Users { get; set; } = new List<string>();

        /// <summary>
        /// Information about the result of the last import operation that was executed.
        /// </summary>
        public ImportExportResult ImportResult { get; set; } = null;

        /// <summary>
        /// Information about the result of the last export operation that was executed.
        /// </summary>
        public ImportExportResult ExportResult { get; set; } = null;

        /// <summary>
        /// Creates a new project.
        /// </summary>
        public Project()
        {
            Version = ApplicationSettings.ProjectFileFormatVersion;
            IssueIdCounter = 1;
            TaskIdCounter = 1;
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="name">The project name.</param>
        public Project(string name)
        {
            Version = ApplicationSettings.ProjectFileFormatVersion;
            Name = name;
            DbName = name.ToLower();
            IssueIdCounter = 1;
            TaskIdCounter = 1;
        }

        /// <summary>
        /// Add a new issue and save in database.
        /// </summary>
        /// <param name="newIssue">An instance of the Issue class to add</param>
        /// <returns>The id of the added issue</returns>
        public int AddIssue(Issue newIssue)
        {
            IssueIdCounter = Database.SaveIssue(newIssue);
            newIssue.ID = IssueIdCounter;
            Issues.Add(IssueIdCounter, newIssue);
            return newIssue.ID;
        }

        /// <summary>
        /// Add a new task and save in database.
        /// </summary>
        /// <param name="newTask">An instance of the Task class to add.</param>
        /// <returns>The id of the added task.</returns>
        public int AddTask(Task newTask)
        {
            TaskIdCounter = Database.SaveTask(newTask);
            newTask.ID = TaskIdCounter;
            Tasks.Add(TaskIdCounter, newTask);
            return newTask.ID;
        }

        /// <summary>
        /// Export the issues and tasks of a project to a file.
        /// </summary>
        public void Export(string fileNameIssues, string fileNameTasks)
        {
            ExportResult = new ImportExportResult(ImportExportOperation.Export);

            // Export the issues
            if ((Issues != null) && (Issues.Count > 0))
            {
                ExportResult.Issues.Result = ExportIssues(fileNameIssues);
                ExportResult.Issues.FileName = System.IO.Path.GetFileName(fileNameIssues);
                ExportResult.Issues.Path = System.IO.Path.GetDirectoryName(fileNameIssues);
            }

            // Export the tasks
            if ((Tasks != null) && (Tasks.Count > 0))
            {
                ExportResult.Tasks.Result = ExportTasks(fileNameTasks);
                ExportResult.Tasks.FileName = System.IO.Path.GetFileName(fileNameTasks);
                ExportResult.Tasks.Path = System.IO.Path.GetDirectoryName(fileNameTasks);
            }
        }

        /// <summary>
        /// Export the project issues in CSV format.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Status</returns>
        private FileSystemOperationStatus ExportIssues(string fileName)
        {
            try
            {
                using (var writer = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteRecords(Issues.Values);
                    }
                }
            }
            catch (System.IO.DirectoryNotFoundException) // The directory does not exist
            {
                return FileSystemOperationStatus.ExportToCsvErrorDirectoryNotFound;
            }
            catch (System.IO.PathTooLongException) // The path is too long
            {
                return FileSystemOperationStatus.ExportToCsvErrorPathTooLong;
            }
            catch (CsvHelperException) // Error reported by CsvHelper
            {
                return FileSystemOperationStatus.ExportToCsvErrorExporterComponent;
            }
            catch // General input/output error
            {
                return FileSystemOperationStatus.ExportToCsvIOError;
            }

            return FileSystemOperationStatus.ExportOK;
        }

        /// <summary>
        /// Export the project tasks in CSV format.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="softwareProject"></param>
        /// <returns>Status</returns>
        private FileSystemOperationStatus ExportTasks(string fileName)
        {
            try
            {
                using (var writer = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteRecords(Tasks.Values);
                    }
                }
            }
            catch (System.IO.DirectoryNotFoundException) // The directory does not exist
            {
                return FileSystemOperationStatus.ExportToCsvErrorDirectoryNotFound;
            }
            catch (System.IO.PathTooLongException) // The path is too long
            {
                return FileSystemOperationStatus.ExportToCsvErrorPathTooLong;
            }
            catch (CsvHelperException) // Error reported by CsvHelper
            {
                return FileSystemOperationStatus.ExportToCsvErrorExporterComponent;
            }
            catch // General input/output error
            {
                return FileSystemOperationStatus.ExportToCsvIOError;
            }

            return FileSystemOperationStatus.ExportOK;
        }
    }
}
