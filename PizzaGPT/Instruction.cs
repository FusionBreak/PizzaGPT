namespace PizzaGPT
{
    public record Instruction(string Name, string[] Paramter);

    public class InstructionParser
    {
        public static Instruction Parse(string instruction)
        {
            try
            {
                var parts = instruction.Split(':');
                var name = parts[0].Trim();
                var parameters = parts[1].Split(',').Select(p => p.Trim()).ToArray();
                return new Instruction(name, parameters);
            }
            catch
            {
                return new Instruction("ERROR", new string[] { instruction });
            }
        }
    }
}
