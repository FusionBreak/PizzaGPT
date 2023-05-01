using System.Text.Json;
using MediatR;

namespace PizzaGPT.Usecases
{
    public static class ExecuteInstruction
    {
        public record Command(Instruction Instruction) : IRequest<Result>;
        public record Result(string ReponseMessage);
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IMediator _mediator;

            public Handler(IMediator mediator) => _mediator = mediator;

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                object response = request.Instruction.Name switch
                {
                    "AddOrder" => await _mediator.Send(new AddOrder.Command(request.Instruction.Paramter[0], request.Instruction.Paramter[1], DateTime.Now)),
                    "DeleteOrder" => await _mediator.Send(new DeleteOrder.Command(int.Parse(request.Instruction.Paramter[0]))),
                    "GetLastOrders" => await _mediator.Send(new GetLastOrders.Command(int.Parse(request.Instruction.Paramter[0]))),
                    "GetOrder" => await _mediator.Send(new GetOrder.Command(int.Parse(request.Instruction.Paramter[0]))),
                    "GetOrdersFromCustomer" => await _mediator.Send(new GetOrdersFromCustomer.Command(request.Instruction.Paramter[0])),
                    _ => new Result($"Instruction could not be executed:\n{request.Instruction.Paramter[0]}")
                };

                return new Result(JsonSerializer.Serialize(response));
            }
        }
    }
}
