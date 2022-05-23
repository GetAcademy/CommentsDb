using System.Data.SqlClient;
using Comments.Model;
using Dapper;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
const string connStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Comments;Integrated Security=True";

// Create
app.MapPost("/comments", async (Comment comment) =>
{
    var conn = new SqlConnection(connStr);
    const string sql = @"
        INSERT INTO Comment
        VALUES (@Id, @Text, @Author, @Created)
    ";
    var rowsAffected = await conn.ExecuteAsync(sql, comment);
    return rowsAffected;
});
// Read
app.MapGet("/comments", async () =>
{
    var conn = new SqlConnection(connStr);
    const string sql = "SELECT * FROM Comment";
    var comments = await conn.QueryAsync<Comment>(sql);
    return comments;
});
// Update
app.MapPut("/comments", async (Comment comment) =>
{
    var conn = new SqlConnection(connStr);
    const string sql = @"
        UPDATE Comment
        SET 
            Text=@Text,
            Author=@Author,
            Created=@Created
        WHERE Id=@Id
    ";
    var rowsAffected = await conn.ExecuteAsync(sql, comment);
    return rowsAffected;
});
// Delete
app.MapDelete("/comments/{id}", async (Guid id) =>
{
    var conn = new SqlConnection(connStr);
    const string sql = @"
        DELETE FROM Comment
        WHERE Id=@Id
    ";
    var rowsAffected = await conn.ExecuteAsync(sql, new { Id = id });
    return rowsAffected;
});
app.UseStaticFiles();
app.Run();
