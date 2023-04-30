using MediatR;
using PizzaGPT.Database;

namespace PizzaGPT.Usecases
{
    public static class GetOrder
    {
        public record Command(int Id) : IRequest<Result>;
        public record Result(string CustomersName, string PizzaName, DateTime OrderDateTime);
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly PizzaContext _pizzaContext;
            public Handler(PizzaContext pizzaContext) => _pizzaContext = pizzaContext;
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var order = await _pizzaContext.Orders.FindAsync(request.Id);
                return new Result(order.CustomersName, order.PizzaName, order.OrderDateTime);
            }
        }
    }
}
