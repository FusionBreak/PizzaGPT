namespace PizzaGPT.Database
{
    public class Order
    {
        public int Id { get; set; }
        public required string CustomersName { get; init; } = string.Empty;
        public required string PizzaName { get; init; } = string.Empty;
        public required DateTime OrderDateTime { get; init; }
    }
}
