using MediatR;
using Microsoft.EntityFrameworkCore;
using PizzaGPT.Database;

namespace PizzaGPT.Usecases
{
    public static class GetLastOrders
    {
        public record Command(int Count) : IRequest<Result>;
        public record Result(IEnumerable<Order> LastOrders);
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly PizzaContext _pizzaContext;
            public Handler(PizzaContext pizzaContext) => _pizzaContext = pizzaContext;
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var orders = await _pizzaContext.Orders
                    .OrderByDescending(o => o.OrderDateTime)
                    .Take(request.Count)
                    .ToListAsync(cancellationToken);
                return new Result(orders);
            }
        }
    }
}
