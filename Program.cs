using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
List<Votante> _votantes = new();

app.MapGet("/votantes", () => TypedResults.Ok(_votantes));
app.MapGet("/votantes/{id}", ([FromRoute] int id) => TypedResults.Ok(_votantes.FirstOrDefault(v => v.Id == id)));
app.MapPost("/votantes", ([FromBody] Votante _nuevoVotante) => {
    _nuevoVotante.Id = _votantes.Count + 1;
    _votantes.Add(_nuevoVotante);
    return TypedResults.Ok(_nuevoVotante.Id);
});
app.MapPut("/votantes/{id}", ([FromRoute] int id, [FromBody] Votante _votanteActualizado) =>  {
    foreach (var _votante in _votantes)
    {
        if (_votante.Id == id) 
        {
            _votante.Nombre = _votanteActualizado.Nombre;
            _votante.Apellido = _votanteActualizado.Apellido;
            _votante.DNI = _votanteActualizado.DNI;
            _votante.FechaNacimiento = _votanteActualizado.FechaNacimiento;
        }
    }
    return TypedResults.Ok();
});
app.MapDelete("/votantes/{id}", ([FromRoute] int id) => {
    _votantes = _votantes.Where(c => c.Id != id).ToList();
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