using MediatR;
using PizzaGPT.Database;

namespace PizzaGPT.Usecases
{
    public static class DeleteOrder
    {
        public record Command(int Id) : IRequest<Result>;
        public record Result(Order DeletedOrder);
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly PizzaContext _pizzaContext;
            public Handler(PizzaContext pizzaContext) => _pizzaContext = pizzaContext;
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var order = await _pizzaContext.Orders.FindAsync(request.Id);
                _pizzaContext.Orders.Remove(order);
                await _pizzaContext.SaveChangesAsync(cancellationToken);
                return new Result(order);
            }
        }
    }
}
