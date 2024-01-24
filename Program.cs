using MessagePack.AspNetCoreMvcFormatter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddSignalR().AddMessagePackProtocol();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.UseWebSockets();
app.Use(async(context, next)=>{
    if(context.Request.Path == "/ws"){
        if(context.WebSockets.IsWebSocketRequest){
            var webSocketHandler = new ChatController();
            await webSocketHandler.HandleWebSocket(context);
        }
        else{
            context.Response.StatusCode = 400;
        }
    }
    else{
        await next();
    }
});


app.Run();
