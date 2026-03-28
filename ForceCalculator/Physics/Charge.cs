public class Charge
{
    public double Value { get; set; }
    public char Sign { get; set; }
    public char Name { get; set; }
    public Vector2 Position { get; set; }

    public Charge(double value, Vector2 position, char name, char sign)
    {
        Value = value;
        Position = position;
        Name = name;
        Sign = sign;
    }
  
}