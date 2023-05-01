using System.Net.Http.Headers;
using System.Net.Http.Json;
using MediatR;

namespace PizzaGPT.Usecases
{
    public static class GetAnswerForUserFromGpt
    {
        public record Command(string SystemMessage, IEnumerable<Gpt.Message> History) : IRequest<Result>;
        public record Result(string ReponseMessage);

        public class Handler : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var url = "https://api.openai.com/v1/chat/completions";
                var bearerToken = "XXXX";
                var messages = request.History.ToList();
                messages.Add(new Gpt.Message
                {
                    content =
                    """
                    Your role is changing now. You should rewrite the following answer from the system into a message for the user.
                    Never write the json string in the answer but reformulate it so that the user can understand it.
                    Ask afterwards if you are allowed to do anything else. Be polite and nice. Use the user's language for your answer.
                    """,
                    role = Gpt.Roles.System
                });
                messages.Add(new Gpt.Message
                {
                    content =
                    $"""
                    Here is the answer from the system:
                    {request.SystemMessage}
                    """,
                    role = Gpt.Roles.System
                });

                var gptRequest = new Gpt.Request()
                {
                    messages = messages
                };

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                var response = await client.PostAsJsonAsync(url, gptRequest);

                if(response.IsSuccessStatusCode)
                {
                    var gptResponse = await response.Content.ReadFromJsonAsync<Gpt.Response>();
                    var answer = gptResponse.choices[0].message;
                    return new Result(answer.content);
                }
                else
                {
                    return new Result(response.ReasonPhrase);
                }
            }
        }
    }
}
