# Ultz.Extensions.Commands
An asynchronous platform-independent .NET Standard command framework that can be used with any input source, whether that be Discord messages, IRC, or a terminal. 

Inspired by [Discord.Net.Commands](https://github.com/RogueException/Discord.Net/tree/dev/src/Discord.Net.Commands) and [DSharpPlus.CommandsNext](https://github.com/DSharpPlus/DSharpPlus/tree/master/DSharpPlus.CommandsNext).

Originally forked from [Qmmands](https://github.com/Quahu/Qmmands)

## A Simple Usage Example
**CommandHandler.cs**
```cs
private readonly CommandService _service = new CommandService();

public void Setup()
    => _service.AddModule<CommandModule>();

// Imagine this being a message callback, whether it be from an IRC bot,
// a Discord bot, or any other chat based service.
private async Task MessageReceivedAsync(Message message)
{
    if (!CommandUtilities.HasPrefix(message.Content, '!', out string output))
        return;
        
    IResult result = await _service.ExecuteAsync(output, new CustomCommandContext(message));
    if (result is FailedResult failedResult)
        await message.Channel.SendMessageAsync(failedResult.Reason); 
}
```
**CustomCommandContext.cs**
```cs
public sealed class CustomCommandContext : CommandContext
{
    public Message Message { get; }
    
    public Channel Channel => Message.Channel;
  
    // Pass your service provider to the base command context.
    public CustomCommandContext(Message message, IServiceProvider provider = null) : base(provider)
    {
        Message = message;
    }
}
```
**CommandModule.cs**
```cs
public sealed class CommandModule : ModuleBase<CustomCommandContext>
{
    // Dependency Injection via the constructor or public settable properties.
    // CommandService and IServiceProvider self-inject into modules,
    // properties and other types are requested from the provided IServiceProvider
    public CommandService Service { get; set; }

    // Invoked with:   !help
    // Responds with:  `help` - Lists available commands.
    //                 `sum` - Sums two given numbers.
    //                 `echo` - Echoes given text.
    [Command("help", "commands")]
    [Description("Lists available commands.")]
    public Task HelpAsync()
        => Context.Channel.SendMessageAsync(
            string.Join('\n', Service.GetAllCommands().Select(x => $"`{x.Name}` - {x.Description}")));

    // Invoked with:  !sum 3 5
    // Responds with: 3 + 5 = 8
    [Command("sum")]
    [Description("Sums two given numbers.")]
    public Task SumAsync(int firstNumber, int secondNumber)
      => Context.Channel.SendMessageAsync(
          $"{firstNumber} + {secondNumber} = {firstNumber + secondNumber}");

    // Invoked with:  !echo Hello, world.
    // Responds with: Hello, world.
    [Command("echo")]
    [Description("Echoes given text.")]
    public Task EchoAsync([Remainder] string text)
      => Context.Channel.SendMessageAsync(text);
}
```

## License
```
MIT License

Copyright (c) 2018-2020 Quahu
Copyright (c) 2020 Ultz Limited

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```