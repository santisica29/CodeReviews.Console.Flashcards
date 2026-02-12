using Dapper;
using FlashcardsRevisited.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace FlashcardsRevisited.Controllers;
internal class StudySessionController
{
    static string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

    internal int Add(StudySession session)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = @"INSERT INTO StudyArea (Score, DateOfSession, StackId) VALUES (@Score, @DateOfSession, @StackId);";

        return connection.Execute(sql, new
        {
            Score = session.Score,
            DateOfSession = session.DateOfSession,
            StackId = session.Stack.StackId,
        });
    }

    internal List<StudySessionDTO> GetAll()
    {
        using var connection = new SqlConnection(connectionString);

        var sql = @"SELECT 
                        StudyArea.StudySessionId,
                        StudyArea.Score,
                        StudyArea.DateOfSession,
                        st.StackName 
                    FROM StudyArea
                    JOIN Stacks st ON StudyArea.StackId = st.StackId
                    ORDER BY StudyArea.DateOfSession DESC;";

        var list = connection.Query<StudySessionDTO>(sql).ToList();

        return list;
    }

    internal List<StudySessionDTO> GetSessionsFromStack(int stackId)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = @"SELECT 
                        StudyArea.StudySessionId,
                        StudyArea.Score,
                        StudyArea.DateOfSession,
                        st.StackName 
                    FROM StudyArea
                    JOIN Stacks st ON StudyArea.StackId = st.StackId
                    WHERE StudyArea.StackId = @StackId
                    ORDER BY StudyArea.DateOfSession DESC;";

        return connection.Query<StudySessionDTO>(sql, new { StackId = stackId }).ToList();
    }
}
