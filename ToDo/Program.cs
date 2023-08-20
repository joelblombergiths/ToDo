using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Data.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ToDoContext>(options =>
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseCamelCaseNamingConvention());

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("todo", (ToDoContext context) => context.ToDoItems.Any() ? Results.Ok(context.ToDoItems.ToList()) : Results.NoContent());

app.MapGet("todo/{Id:int}", (ToDoContext context, int Id) =>
{
    ToDoModel? toDo = context.ToDoItems.FirstOrDefault(t => t.ID == Id);
    return null == toDo ? Results.NotFound() : Results.Ok(toDo);
});

app.MapPost("todo", (ToDoContext context, ToDoModel todo) =>
{
    try
    {
        context.ToDoItems.Add(todo);
        context.SaveChanges();

        return Results.Ok();
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
});

app.MapPatch("todo/{Id:int}/{Done:bool}", (ToDoContext context, int Id, bool Done) =>
{
    ToDoModel? toDo = context.ToDoItems.FirstOrDefault(t => t.ID == Id);
    if (toDo == null) return Results.NotFound();

    toDo.IsDone = Done;
    context.ToDoItems.Update(toDo);
    context.SaveChanges();

    return Results.Ok();
});

app.MapDelete("todo/{Id:int}", (ToDoContext context, int Id) =>
{
    ToDoModel? toDo = context.ToDoItems.FirstOrDefault(t => t.ID == Id);
    if (toDo == null) return Results.NotFound();

    context.ToDoItems.Remove(toDo);
    context.SaveChanges();

    return Results.Ok();
});

app.Run();
