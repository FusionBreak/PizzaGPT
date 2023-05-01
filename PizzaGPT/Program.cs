using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PizzaGPT.Database;
using PizzaGPT.Usecases;

namespace PizzaGPT
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            var mediator = services.GetRequiredService<IMediator>();

            Console.WriteLine("PizzaGPT\nHow can I help you?");

            while(true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                var userInput = Console.ReadLine();

                var instructionFromGpt = await mediator.Send(new PostGptRequest.Command(userInput));
                var parsedInstruction = InstructionParser.Parse(instructionFromGpt.ReponseMessage);
                var systemResponse = await mediator.Send(new ExecuteInstruction.Command(parsedInstruction));
                var answer = await mediator.Send(new GetAnswerForUserFromGpt.Command(systemResponse.ReponseMessage, instructionFromGpt.History));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(answer.ReponseMessage);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n----------------\n");
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