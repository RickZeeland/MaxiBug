using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;

namespace MaxiBug
{
    /// <summary>
    /// Postgres database routines.
    /// Note that the users table is not used for authentication purposes, 
    /// this can be done using Postgres and the Settings for PostgresUser and PostgresPassword.
    /// </summary>
    public static class Database
    {
        /// <summary>
        /// Database connection string.
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// Get Npgsql connection string.
        /// Keepalive (in seconds) determines when notifications are processed if there is no other activity.
        /// </summary>
        /// <param name="dbName">Default is 'postgres'</param>
        /// <param name="keepalive">Default is 0</param>
        /// <returns>The connection string, or a string beginning with "Error:"</returns>
        public static string GetConnectionString(string dbName = "postgres", int keepalive = 0)
        {
            try
            {
                var csb = new NpgsqlConnectionStringBuilder
                {
                    Host = Program.postgresIpaddress,
                    Database = dbName,
                    Username = Program.postgresUser,
                    Password = Program.postgresPassword,
                    Port = Program.postgresPort,
                };

                if (keepalive > 0)
                {
                    csb.KeepAlive = keepalive;
                }

                //Debug.Print(csb.ConnectionString);
                return csb.ConnectionString;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// Create database with tables: project, issues, tasks and users.
        /// Note that the users table is not used for authentication purposes, 
        /// this can be done using Postgres and the Settings for PostgresUser and PostgresPassword.
        /// </summary>
        /// <param name="dbName">The postgresql database name</param>
        public static void CreateProject(string dbName)
        {
            bool result;
            string sql;
            string uri = Program.postgresIpaddress.ToLower();

            if (uri.Contains("amazonaws"))
            {
                // Heroku empty db must be already created
                sql = "set transaction read write; ";
                sql += $@"INSERT INTO project (datecreated, name, version) VALUES (CURRENT_TIMESTAMP, '{Program.SoftwareProject.Name}', '{Program.SoftwareProject.Version}');";
                ExecuteNonQuery(ConnectionString, sql);

                sql = "set transaction read write; ";
                sql += $@"INSERT INTO users (datecreated, name, description) VALUES (CURRENT_TIMESTAMP, '{Program.postgresUser}', '{Environment.UserName} - {Environment.MachineName}');";
                ExecuteNonQuery(ConnectionString, sql);

                Debug.Print($"Updated database {dbName}");
                return;
            }
            else
            {
                // Normal Posgres server
                result = CreateDatabase(dbName);
            }

            ConnectionString = GetConnectionString(dbName);

            if (!result)
            {
                // Already exists
                return;
            }

            // project table
            sql = $@"CREATE TABLE project
            (
              id serial PRIMARY KEY,
              datecreated timestamp with time zone,
              name VARCHAR(50),
              version VARCHAR(20)
            );";

            CreateTable(ConnectionString, "project", sql);

            sql = $@"INSERT INTO project (datecreated, name, version) VALUES (CURRENT_TIMESTAMP, '{Program.SoftwareProject.Name}', '{Program.SoftwareProject.Version}');";
            ExecuteNonQuery(ConnectionString, sql);

            // issues table
            sql = $@"CREATE TABLE issues
            (
              id serial PRIMARY KEY,
              datecreated timestamp with time zone,
              datemodified timestamp with time zone,
              createdby VARCHAR(50),
              modifiedby VARCHAR(50),
              version VARCHAR(20),
              targetversion VARCHAR(20),
              priority smallint,
              status smallint,
              summary VARCHAR(200),
              description text,
              imagefilename VARCHAR(300),
              image_id integer
            );";

            CreateTable(ConnectionString, "issues", sql);

            // tasks table
            sql = $@"CREATE TABLE tasks
            (
              id serial PRIMARY KEY,
              datecreated timestamp with time zone,
              datemodified timestamp with time zone,
              createdby VARCHAR(50),
              modifiedby VARCHAR(50),
              targetversion VARCHAR(20),
              priority smallint,
              status smallint,
              summary VARCHAR(50),
              description text
            );";

            CreateTable(ConnectionString, "tasks", sql);

            // users table
            sql = $@"CREATE TABLE users
            (
              id serial PRIMARY KEY,
              datecreated timestamp with time zone,
              name VARCHAR(50),
              description text,
              issuelock integer,
              tasklock integer
            );";

            CreateTable(ConnectionString, "users", sql);

            sql = $@"INSERT INTO users (datecreated, name, description) VALUES (CURRENT_TIMESTAMP, '{Program.postgresUser}', '{Environment.UserName} - {Environment.MachineName}');";
            ExecuteNonQuery(ConnectionString, sql);

            // images table
            sql = $@"CREATE TABLE images
            (
              id serial PRIMARY KEY,
              datecreated timestamp with time zone,
              name VARCHAR(256),
              data bytea
            );";

            CreateTable(ConnectionString, "images", sql);

            Debug.Print($"Created database {dbName}");
        }

        /// <summary>
        /// Create an empty database if it does not exist.
        /// </summary>
        /// <param name="dbName">The database name</param>
        /// <returns>True on success</returns>
        public static bool CreateDatabase(string dbName)
        {
            bool databaseExist;
            string connectionString = GetConnectionString("postgres");     // Connect to main db first

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var command = new NpgsqlCommand(string.Format("SELECT 1 FROM pg_database WHERE datname='{0}';", dbName), conn))
                {
                    databaseExist = command.ExecuteScalar() != null;
                }

                if (!databaseExist)
                {
                    using (var command = new NpgsqlCommand(string.Format("CREATE DATABASE \"{0}\";", dbName), conn))
                    {
                        command.ExecuteNonQuery();
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Create table if it does not exist.
        /// </summary>
        /// <param name="tablename">The tablename in lower case.</param>
        /// <param name="sql">The create table sql command</param>
        /// <returns>False if no connection possible</returns>
        public static bool CreateTable(string connectionString, string tablename, string sql)
        {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            if (conn.State != ConnectionState.Open)
            {
                // No connection.
                return false;
            }

            if (ExecuteScalar(connectionString, $@"SELECT COUNT(*) FROM information_schema.tables WHERE table_name = '{tablename}'"))
            {
                // Table already exists.
                return true;
            }

            using (var command = new NpgsqlCommand(sql, conn))
            {
                command.ExecuteNonQuery();
            }

            conn.Close();
            return true;
        }

        /// <summary>
        /// Execute sql command.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <param name="query">The query</param>
        /// <returns>Number of affected rows</returns>
        public static int ExecuteNonQuery(string connectionString, string query)
        {
            int rows = 0;

            using (var conn = new NpgsqlConnection(connectionString))
            {
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    conn.Open();
                    rows = cmd.ExecuteNonQuery();
                }
            }

            return rows;
        }

        /// <summary>
        /// Execute scalar command that can return a value.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <param name="query">The query</param>
        /// <returns>True on success</returns>
        public static bool ExecuteScalar(string connectionString, string query)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = query;
                var result = command.ExecuteScalar();

                if (result != null && result.ToString() != "0")
                {
                    return true;
                }
            }

            return false;
        }

        public static string LoadProject(string connectionString, ref Project softwareProject)
        {
            string result = string.Empty;

            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    string query = "SELECT name FROM project;";         // Get the full project name
                    conn.Open();

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            {
                                string projectName = reader.GetString(reader.GetOrdinal("name"));
                                softwareProject.Name = projectName;
                            }
                        }
                    }
                }

                // Load Issues
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    softwareProject.IssueIdCounter = 0;
                    string query = "SELECT * FROM issues;";
                    conn.Open();

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var issue = new Issue();
                            issue.ID = reader.GetInt32(reader.GetOrdinal("id"));
                            issue.DateCreated = reader.GetDateTime(reader.GetOrdinal("datecreated"));
                            issue.DateModified = reader.GetDateTime(reader.GetOrdinal("datemodified"));
                            issue.Description = reader.GetString(reader.GetOrdinal("description"));
                            issue.ImageFilename = reader.GetString(reader.GetOrdinal("imagefilename"));
                            issue.Priority = (IssuePriority)reader.GetInt16(reader.GetOrdinal("priority"));
                            issue.Status = (IssueStatus)reader.GetInt16(reader.GetOrdinal("status"));
                            issue.Summary = reader.GetString(reader.GetOrdinal("summary"));
                            issue.TargetVersion = reader.GetString(reader.GetOrdinal("targetversion"));
                            issue.Version = reader.GetString(reader.GetOrdinal("version"));

                            if (!reader.IsDBNull(reader.GetOrdinal("createdby")))
                            {
                                issue.CreatedBy = reader.GetString(reader.GetOrdinal("createdby"));
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("modifiedby")))
                            {
                                issue.ModifiedBy = reader.GetString(reader.GetOrdinal("modifiedby"));
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("image_id")))
                            {
                                issue.ImageId = reader.GetInt32(reader.GetOrdinal("image_id"));
                            }

                            softwareProject.IssueIdCounter = Math.Max(issue.ID, softwareProject.IssueIdCounter);
                            softwareProject.Issues.Add(issue.ID, issue);
                        }
                    }
                }

                softwareProject.IssueIdCounter++;
                Debug.Print($"softwareProject.IssueIdCounter = {softwareProject.IssueIdCounter}");

                // Load tasks
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    softwareProject.TaskIdCounter = 0;
                    string query = "SELECT * FROM tasks;";
                    conn.Open();

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var task = new Task();
                            task.ID = reader.GetInt32(reader.GetOrdinal("id"));
                            task.DateCreated = reader.GetDateTime(reader.GetOrdinal("datecreated"));
                            task.DateModified = reader.GetDateTime(reader.GetOrdinal("datemodified"));
                            task.Description = reader.GetString(reader.GetOrdinal("description"));
                            task.Priority = (TaskPriority)reader.GetInt16(reader.GetOrdinal("priority"));
                            task.Status = (TaskStatus)reader.GetInt16(reader.GetOrdinal("status"));
                            task.Summary = reader.GetString(reader.GetOrdinal("summary"));
                            task.TargetVersion = reader.GetString(reader.GetOrdinal("targetversion"));

                            if (!reader.IsDBNull(reader.GetOrdinal("createdby")))
                            {
                                task.CreatedBy = reader.GetString(reader.GetOrdinal("createdby"));
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("modifiedby")))
                            {
                                task.ModifiedBy = reader.GetString(reader.GetOrdinal("modifiedby"));
                            }

                            softwareProject.TaskIdCounter = Math.Max(task.ID, softwareProject.TaskIdCounter);
                            softwareProject.Tasks.Add(task.ID, task);
                        }
                    }
                }

                softwareProject.TaskIdCounter++;
                Debug.Print($"softwareProject.TaskIdCounter = {softwareProject.TaskIdCounter}");

                // Load users
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    string query = "SELECT name FROM users ORDER BY name;";
                    conn.Open();

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("name")))
                            {
                                string user = reader.GetString(reader.GetOrdinal("name"));
                                softwareProject.Users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                result = ex.Message;
                softwareProject = null;
            }

            return result;
        }

        /// <summary>
        /// Save a user in the database.
        /// </summary>
        /// <param name="username">The user name</param>
        public static void SaveUser(string username, string description = "")
        {
            string sql = $@"INSERT INTO users (datecreated,name,description) VALUES (CURRENT_TIMESTAMP,@name,@description);";

            string connString = GetConnectionString(Program.databaseName);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("name", username ?? string.Empty);
                    cmd.Parameters.AddWithValue("description", description);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Check if an issue is locked (in use) by a user.
        /// </summary>
        /// <param name="issueId">The issue id</param>
        /// <returns>The user that locked the issue</returns>
        public static string IssueLockedBy(int issueId)
        {
            string sql = $@"SELECT name FROM users WHERE issuelock=@id;";
            string connString = GetConnectionString(Program.databaseName);
            string username = string.Empty;

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("id", issueId);
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        username = result.ToString();
                    }
                }
            }

            return username;
        }

        /// <summary>
        /// Save an issue in the database.
        /// </summary>
        /// <param name="newIssue">The issue</param>
        /// <returns>Returns the new serial id</returns>
        public static int SaveIssue(Issue newIssue)
        {
            string sql = $@"INSERT INTO issues (datecreated, datemodified, createdby, modifiedby, version, targetversion, priority, status, summary, description, imagefilename, image_id) " +
                        "VALUES (@datecreated,@datemodified,@createdby,@modifiedby,@version,@targetversion,@priority,@status,@summary,@description,@imagefilename,@image_id) " +
                        "RETURNING id;";

            string connString = GetConnectionString(Program.databaseName);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("datecreated", NpgsqlTypes.NpgsqlDbType.TimestampTz, newIssue.DateCreated.ToUniversalTime());
                    cmd.Parameters.AddWithValue("datemodified", NpgsqlTypes.NpgsqlDbType.TimestampTz, newIssue.DateModified.ToUniversalTime());
                    cmd.Parameters.AddWithValue("createdby", newIssue.CreatedBy ?? string.Empty);
                    cmd.Parameters.AddWithValue("modifiedby", newIssue.ModifiedBy ?? string.Empty);
                    cmd.Parameters.AddWithValue("version", newIssue.Version ?? string.Empty);
                    cmd.Parameters.AddWithValue("targetversion", newIssue.TargetVersion ?? string.Empty);
                    cmd.Parameters.AddWithValue("priority", (short)newIssue.Priority);
                    cmd.Parameters.AddWithValue("status", (short)newIssue.Status);
                    cmd.Parameters.AddWithValue("summary", newIssue.Summary ?? string.Empty);
                    cmd.Parameters.AddWithValue("description", newIssue.Description ?? string.Empty);
                    cmd.Parameters.AddWithValue("imagefilename", newIssue.ImageFilename ?? string.Empty);
                    cmd.Parameters.AddWithValue("image_id", newIssue.ImageId);
                    var result = cmd.ExecuteScalar();
                    return int.Parse(result.ToString());
                }
            }
        }

        /// <summary>
        /// Save a task in the database.
        /// </summary>
        /// <param name="newTask">The task</param>
        /// <returns>Returns the new serial id</returns>
        public static int SaveTask(Task newTask)
        {
            string sql = $@"INSERT INTO tasks (datecreated, datemodified, createdby, modifiedby, targetversion, priority, status, summary, description) " +
                        "VALUES (@datecreated,@datemodified,@createdby,@modifiedby,@targetversion,@priority,@status,@summary,@description) " +
                        "RETURNING id;";

            string connString = GetConnectionString(Program.databaseName);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("datecreated", NpgsqlTypes.NpgsqlDbType.TimestampTz, newTask.DateCreated.ToUniversalTime());
                    cmd.Parameters.AddWithValue("datemodified", NpgsqlTypes.NpgsqlDbType.TimestampTz, newTask.DateModified.ToUniversalTime());
                    cmd.Parameters.AddWithValue("createdby", newTask.CreatedBy ?? string.Empty);
                    cmd.Parameters.AddWithValue("modifiedby", newTask.ModifiedBy ?? string.Empty);
                    cmd.Parameters.AddWithValue("targetversion", newTask.TargetVersion ?? string.Empty);
                    cmd.Parameters.AddWithValue("priority", (short)newTask.Priority);
                    cmd.Parameters.AddWithValue("status", (short)newTask.Status);
                    cmd.Parameters.AddWithValue("summary", newTask.Summary ?? string.Empty);
                    cmd.Parameters.AddWithValue("description", newTask.Description ?? string.Empty);
                    var result = cmd.ExecuteScalar();
                    return int.Parse(result.ToString());
                }
            }
        }

        /// <summary>
        /// Save an image in the database.
        /// </summary>
        /// <param name="bytes">Byte arrray</param>
        /// <returns>Returns the generated serial id</returns>
        public static int SaveImage(Image img, string filename = "")
        {
            string sql = "INSERT INTO images (datecreated,name,data) VALUES(CURRENT_TIMESTAMP,@name,@image) RETURNING id;";
            string connString = GetConnectionString(Program.databaseName);
            byte[] bytes = imgToByteArray(img);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("name", filename ?? string.Empty);
                    cmd.Parameters.AddWithValue("image", NpgsqlTypes.NpgsqlDbType.Bytea, bytes);
                    var result = cmd.ExecuteScalar();
                    return int.Parse(result.ToString());
                }
            }
        }

        /// <summary>
        /// Load an image from the database.
        /// </summary>
        /// <param name="id">The image id</param>
        /// <returns>The Image or null when not found</returns>
        public static Image LoadImage(int id)
        {
            string sql = $"SELECT data from images WHERE id={id}";
            string connString = GetConnectionString(Program.databaseName);
            Image imageTemp = null;

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var command = new NpgsqlCommand(sql, conn))
                {
                    byte[] bytes = null;
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        bytes = (byte[])reader[0];
                    }

                    reader.Close();

                    if (bytes != null)
                    {
                        using (MemoryStream memStream = new MemoryStream(bytes))
                        {
                            ImageConverter imageConverter = new ImageConverter();
                            imageTemp = imageConverter.ConvertFrom(bytes) as Image;
                        }
                    }
                }
            }

            return imageTemp;
        }

        /// <summary>
        /// Convert an Image to a byte array.
        /// </summary>
        /// <param name="img">The image</param>
        /// <returns>A byte array</returns>
        public static byte[] imgToByteArray(Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }

        /// <summary>
        /// Update project name in the database.
        /// </summary>
        /// <param name="name">Project name</param>
        public static void UpdateProject(string name)
        {
            string sql = $@"UPDATE project SET name=@name;";
            string connString = GetConnectionString(Program.databaseName);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("name", name ?? string.Empty);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Update user locks by name in the database.
        /// </summary>
        /// <param name="username">The user name</param>
        /// <param name="issueId">The issue id or 0</param>
        /// <param name="taskId">The task id or 0</param>
        public static void UpdateUserLocks(string username, int issueId, int taskId)
        {
            string sql = $@"UPDATE users SET issuelock=@issuelock, tasklock=@tasklock WHERE name=@name;";
            string connString = GetConnectionString(Program.databaseName);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("issuelock", issueId);
                    cmd.Parameters.AddWithValue("tasklock", taskId);
                    cmd.Parameters.AddWithValue("name", username ?? string.Empty);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Update an issue in the database by its id.
        /// </summary>
        /// <param name="issue">The issue</param>
        public static void UpdateIssue(Issue issue)
        {
            string sql = $@"UPDATE issues SET datecreated=@datecreated, datemodified=@datemodified, createdby=@createdby, modifiedby=@modifiedby, version=@version, " +
                        "targetversion=@targetversion, priority=@priority, status=@status, summary=@summary, description=@description, imagefilename=@imagefilename, image_id=@image_id " +
                        "WHERE id=@id;";

            string connString = GetConnectionString(Program.databaseName);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("datecreated", NpgsqlTypes.NpgsqlDbType.TimestampTz, issue.DateCreated.ToUniversalTime());
                    cmd.Parameters.AddWithValue("datemodified", NpgsqlTypes.NpgsqlDbType.TimestampTz, issue.DateModified.ToUniversalTime());
                    cmd.Parameters.AddWithValue("createdby", issue.CreatedBy ?? string.Empty);
                    cmd.Parameters.AddWithValue("modifiedby", issue.ModifiedBy ?? string.Empty);
                    cmd.Parameters.AddWithValue("version", issue.Version ?? string.Empty);
                    cmd.Parameters.AddWithValue("targetversion", issue.TargetVersion ?? string.Empty);
                    cmd.Parameters.AddWithValue("priority", (short)issue.Priority);
                    cmd.Parameters.AddWithValue("status", (short)issue.Status);
                    cmd.Parameters.AddWithValue("summary", issue.Summary ?? string.Empty);
                    cmd.Parameters.AddWithValue("description", issue.Description ?? string.Empty);
                    cmd.Parameters.AddWithValue("imagefilename", issue.ImageFilename ?? string.Empty);
                    cmd.Parameters.AddWithValue("image_id", issue.ImageId);
                    cmd.Parameters.AddWithValue("id", issue.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Update a task in the database.
        /// </summary>
        /// <param name="task">The task</param>
        public static void UpdateTask(Task task)
        {
            string sql = $@"UPDATE tasks SET datecreated=@datecreated, datemodified=@datemodified, createdby=@createdby, modifiedby=@modifiedby, " +
                        "targetversion=@targetversion, priority=@priority, status=@status, summary=@summary, description=@description " +
                        "WHERE id=@id;";

            string connString = GetConnectionString(Program.databaseName);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("datecreated", NpgsqlTypes.NpgsqlDbType.TimestampTz, task.DateCreated.ToUniversalTime());
                    cmd.Parameters.AddWithValue("datemodified", NpgsqlTypes.NpgsqlDbType.TimestampTz, task.DateModified.ToUniversalTime());
                    cmd.Parameters.AddWithValue("createdby", task.CreatedBy ?? string.Empty);
                    cmd.Parameters.AddWithValue("modifiedby", task.ModifiedBy ?? string.Empty);
                    cmd.Parameters.AddWithValue("targetversion", task.TargetVersion ?? string.Empty);
                    cmd.Parameters.AddWithValue("priority", (short)task.Priority);
                    cmd.Parameters.AddWithValue("status", (short)task.Status);
                    cmd.Parameters.AddWithValue("summary", task.Summary ?? string.Empty);
                    cmd.Parameters.AddWithValue("description", task.Description ?? string.Empty);
                    cmd.Parameters.AddWithValue("id", task.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Delete an issue from the database.
        /// </summary>
        /// <param name="id">The issue id</param>
        public static void DeleteIssue(int id)
        {
            string sql = $@"DELETE FROM issues WHERE id=@id;";

            string connString = GetConnectionString(Program.databaseName);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Delete a task from the database.
        /// </summary>
        /// <param name="id">The task id</param>
        public static void DeleteTask(int id)
        {
            string sql = $@"DELETE FROM tasks WHERE id=@id;";

            string connString = GetConnectionString(Program.databaseName);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Get local PostgreSQL servers.
        /// </summary>
        /// <returns>A string array</returns>
        public static string[] GetLocalServers()
        {
            var results = new List<string>();
            bool postgresFound = false;
            var scs = ServiceController.GetServices();

            foreach (var sc in scs)
            {
                if (sc.DisplayName.ToLower().StartsWith("postgresql"))
                {
                    postgresFound = true;
                }
            }

            var prc = Process.GetProcessesByName("PostgreSQLPortable");

            if (prc.Length > 0)
            {
                postgresFound = true;
            }

            if (postgresFound)
            {
                results.Add("127.0.0.1");
            }

            results.Sort();

            return results.ToArray();
        }

        /// <summary>
        /// Gets the database names from the PostgreSQL server.
        /// </summary>
        /// <returns>Database names (sorted)</returns>
        public static string[] GetDbNames()
        {
            var databaseNames = new List<string>();

            try
            {
                string connectionString = GetConnectionString("postgres");     // Connect to main db
                //NpgsqlConnection.ClearAllPools();

                for (var tries = 0; tries < 3; tries++)
                {
                    using (var conn = new NpgsqlConnection(connectionString))
                    {
                        conn.Open();

                        using (var command = new NpgsqlCommand("SELECT datname FROM pg_database WHERE NOT datistemplate;", conn))
                        {
                            using (var dr = command.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    var databaseName = dr.GetValue(0).ToString();

                                    if (!databaseName.Equals("postgres"))           // Skip the default database
                                    {
                                        string conn2 = GetConnectionString(databaseName);
                                        var result = ExecuteScalar(conn2, "SELECT * FROM pg_tables WHERE schemaname = 'public' AND tablename = 'issues';");

                                        if (result)
                                        {
                                            databaseNames.Add(databaseName);        // It is a MaxiBug database
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (databaseNames.Count > 0)
                    {
                        databaseNames.Sort();
                        //NpgsqlConnection.ClearAllPools();
                        return databaseNames.ToArray();
                    }

                    Debug.Print("GetDbNames() retry " + tries);
                }
            }
            catch (Exception ex)
            {
                databaseNames.Clear();
                MessageBox.Show(ex.Message, Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Empty array.
            return databaseNames.ToArray();
        }
    }
}
