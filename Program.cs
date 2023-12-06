using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connection = new SqliteConnection("DataSource=votos.db");
connection.Open();
builder.Services.AddDbContext<VotoDb>(opt => opt.UseSqlite(connection));


var app = builder.Build();

app.MapGet("/votantes", (VotoDb _db) => TypedResults.Ok(_db.Votantes.ToList()));
app.MapGet("/votantes/{id}", (VotoDb _db, [FromRoute] int id) => TypedResults.Ok(_db.Votantes.Find(id)));
app.MapPost("/votantes", (VotoDb _db, [FromBody] Votante _nuevoVotante) => {
    _db.Votantes.Add(_nuevoVotante);
    _db.SaveChanges();
    return TypedResults.Ok(_nuevoVotante.Id);
});
app.MapPut("/votantes/{id}", (VotoDb _db, [FromRoute] int id, [FromBody] Votante _votanteActualizado) =>  {
    var _votante = _db.Votantes.Find(id);
    _votante.Nombre = _votanteActualizado.Nombre;
    _votante.Apellido = _votanteActualizado.Apellido;
    _votante.DNI = _votanteActualizado.DNI;
    _votante.FechaNacimiento = _votanteActualizado.FechaNacimiento;
    _db.SaveChanges();
    return TypedResults.Ok();
});
app.MapDelete("/votantes/{id}", (VotoDb _db, [FromRoute] int id) => {
    var _votante = _db.Votantes.Find(id);
    _db.Votantes.Remove(_votante);
    _db.SaveChanges();
    return TypedResults.Ok();
});

app.Run();

public class Votante
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
    public required string DNI {get; set;}
    public required string FechaNacimiento { get; set; }
}

public class VotoDb : DbContext
{
    public VotoDb(DbContextOptions<VotoDb> options) : base(options) { 
        Database.EnsureCreated();
    }

    public DbSet<Votante> Votantes => Set<Votante>();
}