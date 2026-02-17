using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace FlashcardsRevisited.Services;
internal class DatabaseManager
{
    internal static string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

    internal static string serverConnection = ConfigurationManager.ConnectionStrings["ServerConnection"].ConnectionString;

    public void Initialize()
    {
        CreateDatabaseIfNotExist();
        CreateTable(connectionString);
    }
    internal void CreateDatabaseIfNotExist()
    {
        using var connection = new SqlConnection(serverConnection);

        var sql = @"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'FlashcardsDB')
            BEGIN
                CREATE DATABASE FlashcardsDB
            END;";

        connection.Execute(sql);

    }
    internal static void CreateTable(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);

        connection.Open();

        var sqlStack = @"IF OBJECT_ID('dbo.Stacks', 'U') IS NULL
            BEGIN
                CREATE TABLE Stacks (
                    StackId INT IDENTITY NOT NULL,
                    StackName VARCHAR(25) NOT NULL,
                    Description VARCHAR(255),
                    CreatedDate DATE,

                    CONSTRAINT PK_Stacks PRIMARY KEY (StackId),
                    CONSTRAINT UQ_StackName UNIQUE (StackName)
                );
            END";

        connection.Execute(sqlStack);

        var sqlFlashcards = @"IF OBJECT_ID('dbo.Flashcards', 'U') IS NULL
            BEGIN
                CREATE TABLE Flashcards (
                    FlashcardId INT IDENTITY NOT NULL,
                    Front VARCHAR(255) NOT NULL,
                    Back VARCHAR(255) NOT NULL,
                    StackId INT NOT NULL,

                    CONSTRAINT PK_Flashcards PRIMARY KEY (FlashcardId),
                    CONSTRAINT FK_Flashcards_StackId FOREIGN KEY (StackId)
                        REFERENCES Stacks(StackId)
                        ON DELETE CASCADE
                    );
            END";

        connection.Execute(sqlFlashcards);

        var sqlStudySession = @"IF OBJECT_ID('dbo.StudyArea', 'U') IS NULL
            BEGIN
                CREATE TABLE StudyArea (
                    StudySessionId INT IDENTITY NOT NULL,
                    Score INT NOT NULL,
                    DateOfSession DATE NOT NULL,
                    StackId INT NOT NULL,

                    CONSTRAINT PK_StudyArea PRIMARY KEY (StudySessionId),
                    CONSTRAINT FK_StudyArea_StackId FOREIGN KEY (StackId)
                        REFERENCES Stacks(StackId)
                        ON DELETE CASCADE,
                    );
            END";

        connection.Execute(sqlStudySession);
    }
}
