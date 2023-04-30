using MediatR;
using PizzaGPT.Database;

namespace PizzaGPT.Usecases
{
    public static class AddOrder
    {
        public record Command(string CustomersName, string PizzaName, DateTime OrderDateTime) : IRequest<Result>;
        public record Result(int Id);

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly PizzaContext _pizzaContext;
            public Handler(PizzaContext pizzaContext) => _pizzaContext = pizzaContext;

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var order = new Order
                {
                    CustomersName = request.CustomersName,
                    PizzaName = request.PizzaName,
                    OrderDateTime = request.OrderDateTime
                };
                _pizzaContext.Orders.Add(order);
                await _pizzaContext.SaveChangesAsync(cancellationToken);
                return new Result(order.Id);
            }
        }
    }
}
