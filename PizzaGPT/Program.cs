using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PizzaGPT.Database;
using PizzaGPT.Usecases;
using Spectre.Console;

namespace PizzaGPT
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            var mediator = services.GetRequiredService<IMediator>();

            AnsiConsole.Markup("[underline aquamarine3]PizzaGPT[/]\n");

            while(true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                var userInput = AnsiConsole.Ask<string>("[gray]How can I help you?:[/]");

                await AnsiConsole.Status()
                .Spinner(Spinner.Known.Aesthetic)
                .StartAsync("Search instructions...", async ctx =>
                {
                    var instructionFromGpt = await mediator.Send(new GetInstructionFromGpt.Command(userInput));
                    var parsedInstruction = InstructionParser.Parse(instructionFromGpt.ReponseMessage);
                    ctx.Status("Execute instructions...");
                    ctx.Spinner(Spinner.Known.Pipe);
                    var systemResponse = await mediator.Send(new ExecuteInstruction.Command(parsedInstruction));
                    ctx.Status("Generate answer for the user...");
                    var answer = await mediator.Send(new GetAnswerForUserFromGpt.Command(systemResponse.ReponseMessage, instructionFromGpt.History));

                    AnsiConsole.MarkupLine($"\n[orange4_1]{answer.ReponseMessage}[/]");
                    AnsiConsole.Write(new Rule());
                });
            }
        }

        public static IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
            services.AddTransient<PizzaContext>();

            return services.BuildServiceProvider();
        }
    }
}