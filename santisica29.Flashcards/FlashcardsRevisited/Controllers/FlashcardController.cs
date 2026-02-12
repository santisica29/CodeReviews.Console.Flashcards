using Dapper;
using FlashcardsRevisited.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace FlashcardsRevisited.Controllers;

internal class FlashcardController
{
    static string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    internal List<FlashcardDTO>? GetAll()
    {
        using var connection = new SqlConnection(connectionString);

        var sql = "SELECTION * FROM Flashcards";

        var list = connection.Query<Flashcard>(sql).ToList();

        var dtoList = new List<FlashcardDTO>();

        foreach (var item in list)
        {
            dtoList.Add(
                new FlashcardDTO
                {
                    FlashcardId = item.FlashcardId,
                    Front = item.Front,
                    Back = item.Back,
                });
        }

        return dtoList;
    }

    internal List<FlashcardDTO> GetFlashcardsFromStack(int stackId)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = @"SELECT 
                        f.FlashcardId,
                        f.Front,
                        f.Back
                        FROM Flashcards f 
                    WHERE f.StackId = @StackId";

        var list = connection.Query<FlashcardDTO>(sql, new {StackId = stackId}).ToList();

        int id = 1;

        foreach (var item in list)
        {
            item.DTOId = id++;
        }

        return list;
    }

    internal Flashcard? GetById(int id)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = "SELECT * FROM Flashcards WHERE FlashcardId = @FlashcardId";

        return connection.QuerySingleOrDefault<Flashcard>(sql, new { FlashcardId = id });
    }

    internal int Add(Flashcard flashcard)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = "INSERT INTO Flashcards (Front, Back, StackId) VALUES (@Front, @Back, @StackId)";

        return connection.Execute(sql, new
        {
            Front = flashcard.Front,
            Back = flashcard.Back,
            StackId = flashcard.Stack.StackId,
        });
    }

    internal int Delete(int id)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = "DELETE FROM Flashcards WHERE FlashcardId = @DeleteId";

        return connection.Execute(sql, new
        {
            DeleteId = id
        });
    }

    internal int Update(Flashcard newFlashcard)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = @"UPDATE Flashcards 
                    SET Front = @Front, Back = @Back
                    WHERE FlashcardId = @FlashcardId;";

        return connection.Execute(sql, new
        {
            Front = newFlashcard.Front,
            Back = newFlashcard.Back,
            FlashcardId = newFlashcard.FlashcardId,
        });
    }

    internal int GetTotalNumberOfFlashcardsInStack(int stackId)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = "SELECT COUNT(*) FROM Flashcards WHERE StackId = @StackId;";

        var count = connection.ExecuteScalar(sql, new {StackId = stackId});

        return Convert.ToInt32(count);
    }
}
