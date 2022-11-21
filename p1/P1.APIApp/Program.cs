using Microsoft.AspNetCore.Mvc;
using P1.Data;
using P1.Logic;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionvalue = File.ReadAllText(@"/Users/pisit/Desktop/SchoolFolder/WorkRevature/ConnectionStrings/P1ConnectionString.txt");
builder.Services.AddTransient<SqlRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//!This will only create the API in where we call to our functions to perform the various tasks. The best way to think about this is to no longer utilize any function within our IO class. Anything that uses it should not be implemented with it. The function that we call doesn't know what the inputs are until it gets a request from the client.

//https:locahost:7284/getuser
app.MapGet("/getuser", (SqlRepository repo) =>
repo.GetAllUsers()
);

app.MapPost("/getvalid", (User currentuser, SqlRepository repo) =>
{
    return repo.isCredentialValid(currentuser);
});

app.MapPost("/getlogin", (string username, SqlRepository repo) =>
{
    return repo.GetCurrentUser(username);
});

app.MapGet("/emailexist", (string email, SqlRepository repo) =>
    repo.doesEmailExist(email));

app.MapPost("/getregister", (User newuser, SqlRepository repo) =>
{
    repo.AddNewUser(newuser);
    newuser = repo.GetCurrentUser(newuser.Email);
    return newuser;
});

app.MapGet("/getemails", (SqlRepository repo) =>
    repo.getAllEmail()
);

app.MapPost("/ticket", (Ticket newticket, SqlRepository repo) =>
{
    newticket = repo.AddNewTicket(newticket);
    return Results.Created($"/ticket/{newticket.TicketId}", newticket);
});

//!Gets all user tickets
app.MapPost("/alltickets/{employeeid}", (int employeeid, User currentuser, SqlRepository repo) =>
{
    var listoftickets = repo.getUserTickets(currentuser);
    employeeid = currentuser.EmployeeId;
    return listoftickets;
});
//!Gets all pending tickets
app.MapGet("/allpendingtickets", (SqlRepository repo) =>
repo.getPendingTickets());



app.MapPost("/pendingtickets/{employeeid}", (int employeeid, User currentuser, SqlRepository repo) =>
{
    var pendingtickets = repo.PendingUserTickets(currentuser);
    employeeid = currentuser.EmployeeId;
    return pendingtickets;
});

app.MapPost("/approvedtickets/{employeeid}", (int employeeid, User currentuser, SqlRepository repo) =>
{
    var approvedtickets = repo.ApprovedUserTickets(currentuser);
    employeeid = currentuser.EmployeeId;
    return approvedtickets;
});

app.MapPost("/deniedtickets/{employeeid}", (int employeeid, User currentuser, SqlRepository repo) =>
{
    var deniedtickets = repo.DeniedUserTickets(currentuser);
    employeeid = currentuser.EmployeeId;
    return deniedtickets;
});

app.MapPut("/updateticket", (Ticket updateticket, SqlRepository repo) =>
{
    repo.UpdateTicket(updateticket);
    return Results.NoContent();
});

app.Run();