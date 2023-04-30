namespace PizzaGPT.Gpt
{
    public class Response
    {
        public List<Choice> choices { get; set; }
        public int created { get; set; }
        public string id { get; set; }
        public string model { get; set; }
    }
}
