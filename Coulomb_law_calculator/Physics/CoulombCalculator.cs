namespace CoulombPhysics
{
    public static class CoulombCalculator
    {
        public static double CalculateForce(Charge charge1, Charge charge2)
        {
            double k = 8.9875517923e9; 
            double r = GetDistance(charge1.Position, charge2.Position);
            return k * (charge1.Value * charge2.Value) / (r);
        }

        private static double GetDistance(MyVector2 pos1, MyVector2 pos2)
        {
            double dx = pos2.x - pos1.x;
            double dy = pos2.y - pos1.y;
            return (dx * dx + dy * dy);
        }
        public static string GetForceDirection(Charge charge1, Charge charge2)
        {
            if (charge1.Sign == charge2.Sign)
            {
                return "Repulsive";
            }
            else
            {
                return "Attractive";
            }
        }
        public static MyVector2 GetForceVector(Charge charge1, Charge charge2)
        {
            double force = CalculateForce(charge1, charge2);
            string direction = GetForceDirection(charge1, charge2);
            double dx = charge2.Position.x - charge1.Position.x;
            double dy = charge2.Position.y - charge1.Position.y;
            double magnitude = Math.Sqrt(dx * dx + dy * dy);
            if (magnitude == 0) return new MyVector2(0, 0); 
            double unitX = dx / magnitude;
            double unitY = dy / magnitude;
            if (direction == "Repulsive")
            {
                unitX = -unitX;
                unitY = -unitY;
            }
            return new MyVector2((force * unitX), (force * unitY));
        }
    }
}