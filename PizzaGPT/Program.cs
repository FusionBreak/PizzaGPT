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
                var userInput = Console.ReadLine();

                var result = await mediator.Send(new PostGptRequest.Command(userInput));
                var instruction = InstructionParser.Parse(result.reponseMessage);
                await ExecuteInstruction(mediator, instruction);
                Console.WriteLine("\n----------------\n");
            }
        }

        private static async Task ExecuteInstruction(IMediator mediator, Instruction instruction)
        {
            switch(instruction.Name)
            {
                case "AddOrder":
                    var newOrder = await mediator.Send(new AddOrder.Command(instruction.Paramter[0], instruction.Paramter[1], DateTime.Now));
                    Console.WriteLine($"Order {newOrder.Id} created!");
                    break;
                case "DeleteOrder":
                    await mediator.Send(new DeleteOrder.Command(int.Parse(instruction.Paramter[0])));
                    Console.WriteLine("Order deleted!");
                    break;
                case "GetLastOrders":
                    var lastOrders = await mediator.Send(new GetLastOrders.Command(int.Parse(instruction.Paramter[0])));
                    Console.WriteLine($"The last orders:");
                    foreach(var o in lastOrders.Orders)
                        Console.WriteLine($"{o.Id} : {o.CustomersName}, {o.PizzaName}, {o.OrderDateTime}");
                    break;
                case "GetOrder":
                    var order = await mediator.Send(new GetOrder.Command(int.Parse(instruction.Paramter[0])));
                    Console.WriteLine($"{order.CustomersName}, {order.PizzaName}, {order.OrderDateTime}");
                    break;
                case "GetOrdersFromCustomer":
                    var orders = await mediator.Send(new GetOrdersFromCustomer.Command(instruction.Paramter[0]));
                    Console.WriteLine($"All orders of the customer:");
                    foreach(var o in orders.Orders)
                        Console.WriteLine($"{o.Id} : {o.PizzaName}, {o.OrderDateTime}");
                    break;
                case "ERROR":
                    Console.WriteLine($"{instruction.Paramter[0]}");
                    break;
                default:
                    break;
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