using MediatR;
using Microsoft.EntityFrameworkCore;
using PizzaGPT.Database;

namespace PizzaGPT.Usecases
{
    public static class GetOrdersFromCustomer
    {
        public record Command(string CustomersName) : IRequest<Response>;

        public record Response(List<Order> OrdersFromCustomer);

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly PizzaContext _context;
            public Handler(PizzaContext context) => _context = context;
            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var orders = await _context.Orders.Where(o => o.CustomersName == request.CustomersName).ToListAsync(cancellationToken);
                return new Response(orders);
            }
        }
    }
}
