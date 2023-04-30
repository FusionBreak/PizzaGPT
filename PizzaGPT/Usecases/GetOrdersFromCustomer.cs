using MediatR;
using Microsoft.EntityFrameworkCore;
using PizzaGPT.Database;

namespace PizzaGPT.Usecases
{
    public static class GetOrdersFromCustomer
    {
        public record Command(string CustomersName) : IRequest<Response>;

        public class Response
        {
            public List<Order> Orders { get; set; }
        }
        public class Order
        {
            public int Id { get; set; }
            public string CustomersName { get; set; }
            public string PizzaName { get; set; }
            public DateTime OrderDateTime { get; set; }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly PizzaContext _context;
            public Handler(PizzaContext context) => _context = context;
            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var orders = await _context.Orders.Where(o => o.CustomersName == request.CustomersName).ToListAsync(cancellationToken);
                return new Response
                {
                    Orders = orders.Select(o => new Order
                    {
                        Id = o.Id,
                        CustomersName = o.CustomersName,
                        PizzaName = o.PizzaName,
                        OrderDateTime = o.OrderDateTime
                    }).ToList()
                };
            }
        }
    }
}
