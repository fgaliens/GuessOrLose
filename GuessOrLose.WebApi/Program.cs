using GuessOrLose;
using GuessOrLose.Controllers;
using GuessOrLose.WebApi;
using GuessOrLose.WebApi.Authentication;
using GuessOrLose.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(cfg =>
{
    cfg.AddScheme<PlayerAuthenticationHandler>(AuthenticationDefaults.TokenBasedScheme, "Token based authentication");
});

builder.Services.AddAuthorization(auth => 
{
    auth.AddPolicy(AuthorizationDefaults.PlayerPolicy, policy =>
    {
        policy
            .AddAuthenticationSchemes(AuthenticationDefaults.TokenBasedScheme)
            .RequireClaim(ClaimTypes.Id)
            .RequireClaim(ClaimTypes.Name);
    });

    auth.DefaultPolicy = auth.GetPolicy(AuthorizationDefaults.PlayerPolicy)!;
});

builder.Services.AddGuessOrLoseGame(cfg =>
{
    cfg.PlayerServices.SetPlayerProvider<PlayerProvider>();
});
builder.Services.AddWebApiServices();

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ClientsHub>("/api/game/ws");

app.Run();
