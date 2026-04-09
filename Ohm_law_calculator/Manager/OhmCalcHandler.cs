class OhmCalcHandler
{
    public List<double> Resistances { get; private set; }
    public OhmCalcHandler()
    {
        Resistances = new List<double>();
    }
    public void AddResistance(double resistance)
    {
        Resistances.Add(resistance);
    }
    public double CalculateTotalResistance(string circuitType)
    {
        if (circuitType == "Series")
        {
            return SeriesCircuitCalculator.CalculateTotalResistance(Resistances.ToArray());
        }
        else
        {
            return ParallelCircuitCalculator.CalculateTotalResistance(Resistances.ToArray());
        }
    }
}