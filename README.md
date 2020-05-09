# BoxBot
#### BotBox is a wrapper that lets you create and add you commands to a discord bot. BotBox uses Discord.Net

This guide assumes that you already know how to [create and use your commands and services](https://discord.foxbot.me/stable/guides/commands/intro.html) 
and how to use delegates.


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
says whether to add the default implementations for IConfiguration and IDiscordLogger.


### 2. Configuring the bot
#### Bot configuration
You can change the configuration that the bot will have using box.Config
```csharp
box.Config.ClientType = ClientType.Socket;
box.Config.DiscordToken = GetToken();
```

#### Client configuration
You can also pass the client configuration you want to have 
```csharp
box.SetDiscordSocketConfig(new DiscordSocketConfig()
{
  WebSocketProvider = Discord.Net.Providers.WS4Net.WS4NetProvider.Instance,
  LogLevel = Discord.LogSeverity.Info
});
```

#### Adding commands
To add commands use AddCommandModulesAsync()
```csharp
Assembly commands; // Assembly that contains the commands
await box.AddCommandModulesAsync(commands);
```

### 3. Starting the bot
The bot has simple managemnt functions
```csharp
box.StartAsync();
box.Stop();
box.Restart();
```


## Custom handling
### Custom command handling
To have control on your commands you can write your own handling functions and pass them like this
```csharp
box.SetOnMessageRecieved(new OnMessageRecieved(YourFunction));
```
[Here](https://discord.foxbot.me/stable/guides/commands/post-execution.html) is a detailed guide on how to do this


box.SetOnMessageRecieved() is for the HandleCommandAsync function

box.SetOnCommandExecuted() is for [CommandExecuted Event](https://discord.foxbot.me/stable/guides/commands/post-execution.html#commandexecuted-event)

box.SetOnExceptionCatched() is for [CommandService.Log Event](https://discord.foxbot.me/stable/guides/commands/post-execution.html#commandservicelog-event)

By default, the prefix is the @mention and if an error is encountered it tries to send the error reason to the channel

### Adding TypeReaders
To add TypeReaders you need to use AddTypeReaders delegate and SetTypeReaders() function
```csharp
void AddCustomTypeReaders(CommandService service)
{
  // Add your TypeReaders
  service.AddTypeReader(...);
  ...
}

// Provide a function that adds you TypeReaders
box.SetTypeReaders(new AddTypeReaders(AddCustomTypeReaders));
```
