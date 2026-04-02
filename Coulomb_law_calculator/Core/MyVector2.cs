namespace CoulombPhysics
{
    public struct MyVector2
    {
        public double x;
        public double y;

        public MyVector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
        public MyVector2 GetPosition()
        {
            return this;
        }
    }
}
