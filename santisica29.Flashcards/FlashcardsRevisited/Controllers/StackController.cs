using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Configuration;
using FlashcardsRevisited.Models;

namespace FlashcardsRevisited.Controllers;
internal class StackController
{
    static string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    internal int Add(StackDeck stack)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = @"INSERT INTO Stacks (StackName, Description, CreatedDate) VALUES (@StackName, @Description, @CreatedDate)";

        return connection.Execute(sql, new
        {
            stack.StackName,
            stack.Description,
            stack.CreatedDate,
        });
    }

    internal int Update(StackDeck newStackDeck)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = @"UPDATE Stacks 
                    SET StackName = @NewStackName, Description = @NewDescription
                    WHERE StackId = @StackId";

        return connection.Execute(sql, new
        {
            NewStackName = newStackDeck.StackName,
            NewDescription = newStackDeck.Description,
            StackId = newStackDeck.StackId,
        });
    }

    internal int Delete(int id)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = @"DELETE FROM Stacks WHERE StackId = @StackId";

        return connection.Execute(sql, new {StackId = id});
    }

    internal StackDeck? GetById(int id)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = @"SELECT * FROM Stacks
                    WHERE StackId = @StackId";

        return connection.QuerySingleOrDefault<StackDeck>(sql, new
        {
            StackId = id,
        });
    }

    internal StackDeck? GetByName(string name)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = @"SELECT * FROM Stacks
                    WHERE StackName = @Name";

        return connection.QuerySingleOrDefault<StackDeck>(sql, new {Name = name});
    }

    internal List<StackDeck> GetAll()
    {
        using var connection = new SqlConnection(connectionString);

        var sql = "SELECT * FROM Stacks";

        return connection.Query<StackDeck>(sql).ToList();
    }
}
