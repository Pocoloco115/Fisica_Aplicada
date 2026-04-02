namespace CoulombPhysics
{
    public class Charge
    {
        public double Value { get; set; }
        public char Sign { get; set; }
        public char Name { get; set; }
        public MyVector2 Position { get; set; }

        public Charge(double value, MyVector2 position, char name, char sign)
        {
            Value = value;
            Position = position;
            Name = name;
            Sign = sign;
        }
    
    }
}
