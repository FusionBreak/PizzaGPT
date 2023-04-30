namespace PizzaGPT.Gpt
{
    internal class Request
    {
        public string model { get; set; } = "gpt-3.5-turbo";
        public double temperature { get; set; } = 0.7;
        public int max_tokens { get; set; } = 2048;
        public int top_p { get; set; } = 1;
        public double frequency_penalty { get; set; } = 0.5;
        public double presence_penalty { get; set; } = 0.0;
        public IEnumerable<Message> messages { get; set; } = Array.Empty<Message>();
    }
}
