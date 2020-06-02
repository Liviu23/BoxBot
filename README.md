# BoxBot
#### BotBox is a wrapper that lets you create and add you commands to a discord bot using Discord.Net and DI

This guide assumes that you already know how to [create and use your commands](https://discord.foxbot.me/stable/guides/commands/intro.html).


## Available on nuget.org
https://www.nuget.org/packages/BoxBot/


## How to use BoxBot
### 1. We need a box
```csharp
var services = Box.GetEssentials(true);
// Add your own services here 
...
var box = new Box(services.BuildServiceProvider());
```
#### What is GetEssentials?
GetEssensials returns the services that the bot needs to be able to run. The function accepts a true or false argument that 
says whether to add the default implementations for IConfiguration, IDiscordLogger and ICommandHandler.



### 2. Configuring the bot
#### Bot configuration
You can change the configuration that the bot will have using box.Config or bot.Configuration
```csharp
private static void ConfigureBot(Bot bot)
{
  bot.Configuration.ClientType = ClientType.Socket;
  bot.Configuration.DiscordToken = File.ReadAllText("token.txt");
  bot.DiscordSocketConfig = new DiscordSocketConfig()
  {
    // I use windows server 2008 so I need WS4Net
    WebSocketProvider = Discord.Net.Providers.WS4Net.WS4NetProvider.Instance,
    LogLevel = Discord.LogSeverity.Verbose
  };
}
```


### 3. Managing the bot
The bot has simple managemnt functions
```csharp
box.StartAsync();
box.Stop();
box.Restart();
```



## Custom commands, handlers and services
### Creating a custom command handler
```csharp
class CommandHandler : ICommandHandler
{
  // You need a CommandService for your commands
  private CommandService commandService;
  // clientManager will contain your client
  private IClientManager clientManager;
  // This one contains you services
  private IServiceProvider services;

  public CommandHandler(IClientManager clientManager, IServiceProvider services)
  {
    this.clientManager = clientManager;
    this.services = services;
    commandService = new CommandService();
  }

  public async Task Initialize()
  {
    // Set handlers
    var client = (DiscordSocketClient)clientManager.Client;
    client.MessageReceived += HandleCommandAsync;
    commandService.CommandExecuted += OnCommandExecuted;
    commandService.Log += LogAsync;
    
    // Add your commands
    await commandService.AddModulesAsync(...,services);
  }
  
  private async Task HandleCommandAsync(SocketMessage s)
  {
    // Check for commands and run them if needed
    ...
  }
}
```
[Here](https://discord.foxbot.me/stable/guides/commands/post-execution.html) is a detailed guide on how to create your custom 
handlers

[Here](https://github.com/Liviu23/BoxBot/blob/master/ConsoleUI/CommandHandler.cs) is my implementation of ICommandHandler



### Using your custom handler and services
This is a bit harder but it's still simple
```csharp
IServiceCollection services = SetServices();
Box box = new Box(services.BuildServiceProvider());


private static IServiceCollection SetServices()
  // This will let us set our own implementations
  => Box.GetEssentials(false)
  // These can be the same (If you are to lazy to change them)
  .AddSingleton<IConfiguration, BotConfiguration>()
  .AddSingleton<IDiscordLogger, ConsoleLogger>()
  // However, this is your custom handler. 
  .AddSingleton<ICommandHandler, CommandHandler>()
  // Add other services
  .AddCustomServices();
```

By default, the prefix is the @mention and if an error is encountered it tries to send the error reason to the channel
