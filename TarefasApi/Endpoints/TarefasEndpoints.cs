using Dapper.Contrib.Extensions;
using TarefasApi.Data;
using static TarefasApi.Data.TarefaContext;

namespace TarefasApi.Endpoints;

public static class TarefasEndpoints
{
    public static async void MapTarefasEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => $"Bem vindo a API Tarefas - {DateTime.Now}");

        app.MapGet("/tarefas", async (GetConnection connectionGetter) =>
        {
            using var con = await connectionGetter();
            var tarefas = con.GetAll<Tarefa>().ToList();
            if (tarefas is null) Results.NotFound();
            return Results.Ok(tarefas);
        });

        app.MapGet("/tarefas/{id}", async (GetConnection connectionGetter, int id) =>
        {
            using var con = await connectionGetter();
            var tarefa = con.Get<Tarefa>(id);
            if (tarefa is null) Results.NotFound();
            return Results.Ok(tarefa);
        });

        app.MapPost("/tarefas", async (GetConnection connectionGetter, Tarefa tarefa) =>
        {
            using var con = await connectionGetter();
            var id = con.Insert(tarefa);
            return Results.Created($"/tarefas/{id}", tarefa);
        });

        app.MapPut("/tarefas", async (GetConnection connectionGetter, Tarefa tarefa) =>
        {
            using var con = await connectionGetter();
            var id = con.Update(tarefa);
            return Results.Ok;
        });

        app.MapDelete("/tarefas/{id}", async (GetConnection connectionGetter, int id) =>
        {
            using var con = await connectionGetter();
            var delete = con.Get<Tarefa>(id);
            if (delete is null) Results.NotFound();

            con.Delete(delete);
            return Results.Ok(delete);
        });






    }
}
