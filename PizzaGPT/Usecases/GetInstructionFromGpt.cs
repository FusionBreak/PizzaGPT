﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using MediatR;

namespace PizzaGPT.Usecases
{
    public static class GetInstructionFromGpt
    {
        public record Command(string requestMessage) : IRequest<Result>;
        public record Result(string ReponseMessage, IEnumerable<Gpt.Message> History);

        public class Handler : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var url = "https://api.openai.com/v1/chat/completions";
                var bearerToken = Environment.GetEnvironmentVariable("OpenAI-API-Key");

                var gptRequest = new Gpt.Request()
                {
                    messages = new Gpt.Message[]
                    {
                        new Gpt.Message { content =
                        """
                        You are an AI assistant who must generate instructions for a program from the input of a human user.
                        You must fulfill your goal using only the listed options.
                        You are not allowed to invent new possibilities for instructions.
                        The instructions follow a fixed, unchanging scheme.
                        Write only the instructions, no other text!

                        The scheme:
                        <statement> : <parameter>,<parameter>, ...

                        Examples:

                        AddOrder : "Max Mustermann","Salami"
                        GetOrdersFromCustomer : "Peter Mueller"

                        If you cannot execute something, inform the program as follows.
                        If an error occurs, always use ERROR and NOT the actual instruction.
                        
                        ERROR: <message>
                        
                        Example:
                        
                        Userinput = "I want to order a salami pizza."
                        ERROR: "<CustomersName> is missing."
                        
                        Userinput = "Delete my order."
                        ERROR: "<OrderId> is missing>."
                        """, role = Gpt.Roles.System },
                        new Gpt.Message { content =
                        """
                        Here is a list of possible instructions:

                        AddOrder : <CustomersName>,<PizzaName>
                        DeleteOrder: <Id>
                        GetLastOrders: <Count>
                        GetOrder: <Id>
                        GetOrdersFromCustomer: <CustomersName>
                        """, role = Gpt.Roles.System },
                        new Gpt.Message { content = request.requestMessage, role = Gpt.Roles.User }
                    }
                };

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                var response = await client.PostAsJsonAsync(url, gptRequest);

                var messages = gptRequest.messages.ToList();
                if(response.IsSuccessStatusCode)
                {
                    var gptResponse = await response.Content.ReadFromJsonAsync<Gpt.Response>();
                    var answer = gptResponse.choices[0].message;
                    messages.Add(answer);
                    return new Result(answer.content, messages);
                }
                else
                {
                    return new Result(response.ReasonPhrase, messages);
                }
            }
        }
    }
}
