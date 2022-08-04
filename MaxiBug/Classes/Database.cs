using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;

namespace MaxiBug
{
    /// <summary>
    /// Postgres database routines.
    /// </summary>
    public static class Database
    {
        //private const string Server = "127.0.0.1";
        //private const string User = "postgres";
        //private const string Password = "postgres";
        //private static string database = "minibug";
        //private static string tableName = "project";
        //private NpgsqlConnection notificationConnection;

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
        /// <returns>The connection string</returns>
        public static string GetConnectionString(string dbName = "postgres", int keepalive = 0)
        {
            var csb = new NpgsqlConnectionStringBuilder
            {
                Host = Properties.Settings.Default.PostgresIpaddress,
                Database = dbName,
                Username = Properties.Settings.Default.PostgresUser,
                Password = Properties.Settings.Default.PostgresPassword,
                Port = Properties.Settings.Default.PostgresPort
            };

            if (keepalive > 0)
            {
                csb.KeepAlive = keepalive;
            }

            Debug.Print(csb.ConnectionString);
            return csb.ConnectionString;
        }

        /// <summary>
        /// Create database with tables: project, issues, tasks and users.
        /// Note that the users table is not used for authentication purposes, 
        /// this can be done using Postgres and the Settings PostgresUser and PostgresPassword.
        /// </summary>
        /// <param name="dbName">The postgresql database name</param>
        public static void CreateProject(string dbName)
        {
            bool result = CreateDatabase(dbName);
            ConnectionString = GetConnectionString(dbName);

            if (!result)
            {
                // Already exists
                return;
            }

            // project table
            string sql = $@"CREATE TABLE project
            (
              id serial,
              datecreated timestamp with time zone,
              name character varying(50),
              version character varying(20),
              CONSTRAINT pk_project PRIMARY KEY (id)
            );";

            CreateTable(ConnectionString, "project", sql);

            sql = $@"INSERT INTO project (datecreated, name, version) VALUES (CURRENT_TIMESTAMP, '{Program.SoftwareProject.Name}', '{Program.SoftwareProject.Version}');";
            ExecuteNonQuery(ConnectionString, sql);

            // issues table
            sql = $@"CREATE TABLE issues
            (
              id serial,
              datecreated timestamp with time zone,
              datemodified timestamp with time zone,
              createdby character varying(50),
              modifiedby character varying(50),
              version character varying(20),
              targetversion character varying(20),
              priority smallint,
              status smallint,
              summary character varying(200),
              description text,
              imagefilename character varying(300),
              CONSTRAINT pk_issues PRIMARY KEY (id)
            );";

            CreateTable(ConnectionString, "issues", sql);

            // tasks table
            sql = $@"CREATE TABLE tasks
            (
              id serial,
              datecreated timestamp with time zone,
              datemodified timestamp with time zone,
              createdby character varying(50),
              modifiedby character varying(50),
              targetversion character varying(20),
              priority smallint,
              status smallint,
              summary character varying(50),
              description text,
              CONSTRAINT pk_tasks PRIMARY KEY (id)
            );";

            CreateTable(ConnectionString, "tasks", sql);

            // users table
            sql = $@"CREATE TABLE users
            (
              id serial,
              datecreated timestamp with time zone,
              name character varying(50),
              description text,
              CONSTRAINT pk_users PRIMARY KEY (id)
            );";

            CreateTable(ConnectionString, "users", sql);

            sql = $@"INSERT INTO users (datecreated, name, description) VALUES (CURRENT_TIMESTAMP, '{Environment.UserName}', '{Environment.MachineName}');";
            ExecuteNonQuery(ConnectionString, sql);

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

                            softwareProject.IssueIdCounter = Math.Max(issue.ID, softwareProject.IssueIdCounter);
                            softwareProject.Issues.Add(issue.ID, issue);
                        }
                    }
                }

                softwareProject.IssueIdCounter++;
                Debug.Print($"softwareProject.IssueIdCounter = {softwareProject.IssueIdCounter}");

                // TODO: load tasks

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
        /// Save an issue in the database.
        /// </summary>
        /// <param name="newIssue">The issue</param>
        /// <returns>Returns the new serial id</returns>
        public static int SaveIssue(Issue newIssue)
        {
            string sql = $@"INSERT INTO issues (datecreated, datemodified, createdby, modifiedby, version, targetversion, priority, status, summary, description, imagefilename) " +
                        "VALUES (@datecreated,@datemodified,@createdby,@modifiedby,@version,@targetversion,@priority,@status,@summary,@description,@imagefilename) " +
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
        /// Update an issue in the database by its id.
        /// </summary>
        /// <param name="issue">The issue</param>
        public static void UpdateIssue(Issue issue)
        {
            string sql = $@"UPDATE issues SET datecreated=@datecreated, datemodified=@datemodified, createdby=@createdby, modifiedby=@modifiedby, version=@version, " +
                        "targetversion=@targetversion, priority=@priority, status=@status, summary=@summary, description=@description, imagefilename=@imagefilename " +
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
                                    databaseNames.Add(databaseName);
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

            // Empty array.
            return databaseNames.ToArray();
        }

    }
}
