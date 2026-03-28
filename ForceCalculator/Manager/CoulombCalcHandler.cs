public class CoulombCalcHandler
{
    public int NumberOfCharges { get; set; }
    public List<Charge> Charges { get; set; }
    public Charge MainCharge { get; set; }
    public CoulombCalcHandler(int numberOfCharges, Charge mainCharge)
    {
        NumberOfCharges = numberOfCharges;
        Charges = new List<Charge>();
        MainCharge = mainCharge;
    }
    public void AddCharge(Charge charge)
    {
        if (Charges.Count < NumberOfCharges)
        {
            Charges.Add(charge);
        }
        else
        {
            Console.WriteLine("Maximum number of charges reached.");
        }
    }
    public List<List<string>> CalculateForces()
    {
        List<List<string>> results = new List<List<string>>();
        foreach (var charge in Charges)
        {
            double force = CoulombCalculator.CalculateForce(MainCharge, charge);
            string direction = CoulombCalculator.GetForceDirection(MainCharge, charge);
            Vector2 forceVector = CoulombCalculator.GetForceVector(MainCharge, charge);
            results.Add(new List<string> { $"Results between {MainCharge.Name} and {charge.Name}", 
            $"Force: {force} N", $"Direction: {direction}", $"Force Vector: {forceVector}" });
        }
        return results;
    }
}