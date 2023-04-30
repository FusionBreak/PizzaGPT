namespace PizzaGPT.Gpt
{
    public class Choice
    {
        public string finish_reason { get; set; }
        public int index { get; set; }
        public Message message { get; set; }
    }
}
