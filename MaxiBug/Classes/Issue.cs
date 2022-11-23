﻿// Copyright(c) João Martiniano. All rights reserved.
// Licensed under the MIT license.

using System;
using System.ComponentModel;

namespace MaxiBug
{
    /// <summary>
    ///  Status of an issue.
    /// </summary>
    public enum IssueStatus
    {
        None = 0,
        Unconfirmed,
        Confirmed,
        [DescriptionAttribute("In progress")]
        InProgress,
        Resolved,
        Closed
    };

    /// <summary>
    /// Priority of an issue.
    /// </summary>
    public enum IssuePriority { None = 0, Low, Normal, High, Urgent, Immediate };

    /// <summary>
    /// Fields used on the user interface (in a DataGridView and on a form) to represent an issue.
    /// </summary>
    public enum IssueFieldsUI
    {
        ID = 0,
        Priority,
        Status,
        Version,
        [DescriptionAttribute("Target version")]
        TargetVersion,
        Summary,
        Description,
        [DescriptionAttribute("Date created")]
        DateCreated,
        [DescriptionAttribute("Date modified")]
        DateModified
    };

    [Serializable]
    public class Issue
    {
        /// <summary>
        /// Gets the ID of this issue.
        /// </summary>
        public int ID { get; set; } = 0;

        /// <summary>
        /// Gets or sets the priority of this issue.
        /// </summary>
        public IssuePriority Priority { get; set; } = IssuePriority.None;

        /// <summary>
        /// Gets or sets the status of this issue.
        /// </summary>
        public IssueStatus Status { get; set; } = IssueStatus.None;

        /// <summary>
        /// Gets or sets the version of the software on which this issue occurs.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the future version on which this issue will be resolved.
        /// </summary>
        public string TargetVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the summary of this issue.
        /// </summary>
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of this issue.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Git history of this issue.
        /// </summary>
        public string GitHistory { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date/time this issue was created.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the date/time this issue was modified.
        /// </summary>
        public DateTime DateModified { get; set; }

        /// <summary>
        /// Gets or sets the created by name.
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the modified by name.
        /// </summary>
        public string ModifiedBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional image file name.
        /// </summary>
        public string ImageFilename { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional image id.
        /// </summary>
        public int ImageId { get; set; }

        /// <summary>
        /// Creates a new issue.
        /// </summary>
        public Issue()
        {
            ;
        }

        /// <summary>
        /// Creates a new issue.
        /// </summary>
        /// <param name="id">The ID of this issue.</param>
        public Issue(int id)
        {
            ID = id;
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        /// <summary>
        /// Creates a new issue.
        /// </summary>
        public Issue(IssueStatus status, IssuePriority priority, string summary, string description, string version, string targetVersion, string githistory = "")
        {
            Status = status;
            Priority = priority;
            Summary = summary;
            Description = description;
            GitHistory = githistory;
            Version = version;
            TargetVersion = targetVersion;
            this.ImageFilename = string.Empty;
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        /// <summary>
        /// Create a clone of an instance of the Issue class.
        /// </summary>
        /// <param name="clonedInstance">The instance of the Issue class that will get the cloned instance's data.</param>
        public void Clone(ref Issue clonedInstance)
        {
            clonedInstance.Status = this.Status;
            clonedInstance.Priority = this.Priority;
            clonedInstance.Summary = this.Summary;
            clonedInstance.Description = this.Description;
            clonedInstance.GitHistory = this.GitHistory;
            clonedInstance.Version = this.Version;
            clonedInstance.TargetVersion = this.TargetVersion;
            clonedInstance.DateCreated = clonedInstance.DateModified = DateTime.Now;
            clonedInstance.CreatedBy = this.CreatedBy;
            clonedInstance.ModifiedBy = this.ModifiedBy;
            clonedInstance.ImageFilename = this.ImageFilename;
            clonedInstance.ImageId = this.ImageId;
        }
    }
}
