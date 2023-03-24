using System.Diagnostics;

namespace LinearMapPort;

internal static class Program
{
    private static void Main(string[] args)
    {
        LinearMap torqueRatioLookUpTable = new LinearMap();
        torqueRatioLookUpTable.Add( 0.0f, 1.323f );
        torqueRatioLookUpTable.Add( 0.1f, 1.291f );
        torqueRatioLookUpTable.Add( 0.2f, 1.259f );
        torqueRatioLookUpTable.Add( 0.3f, 1.226f );
        torqueRatioLookUpTable.Add( 0.4f, 1.194f );
        torqueRatioLookUpTable.Add( 0.5f, 1.162f );
        torqueRatioLookUpTable.Add( 0.6f, 1.130f );
        torqueRatioLookUpTable.Add( 0.7f, 1.100f );
        torqueRatioLookUpTable.Add( 0.8f, 1.065f );
        torqueRatioLookUpTable.Add( 0.9f, 1.032f );
        torqueRatioLookUpTable.Add( 1.0f, 1.000f );
        
        LinearMapAlt torqueRatioLookUpTableAlt = new LinearMapAlt();
        torqueRatioLookUpTableAlt.Add( 0.0f, 1.323f );
        torqueRatioLookUpTableAlt.Add( 0.1f, 1.291f );
        torqueRatioLookUpTableAlt.Add( 0.2f, 1.259f );
        torqueRatioLookUpTableAlt.Add( 0.3f, 1.226f );
        torqueRatioLookUpTableAlt.Add( 0.4f, 1.194f );
        torqueRatioLookUpTableAlt.Add( 0.5f, 1.162f );
        torqueRatioLookUpTableAlt.Add( 0.6f, 1.130f );
        torqueRatioLookUpTableAlt.Add( 0.7f, 1.100f );
        torqueRatioLookUpTableAlt.Add( 0.8f, 1.065f );
        torqueRatioLookUpTableAlt.Add( 0.9f, 1.032f );
        torqueRatioLookUpTableAlt.Add( 1.0f, 1.000f );
        
        FastLinearMapWithMultiplePoints fastTorqueRatioLookUpTable = new FastLinearMapWithMultiplePoints();
        fastTorqueRatioLookUpTable.Add( 0.0f, 1.323f );
        fastTorqueRatioLookUpTable.Add( 0.1f, 1.291f );
        fastTorqueRatioLookUpTable.Add( 0.2f, 1.259f );
        fastTorqueRatioLookUpTable.Add( 0.3f, 1.226f );
        fastTorqueRatioLookUpTable.Add( 0.4f, 1.194f );
        fastTorqueRatioLookUpTable.Add( 0.5f, 1.162f );
        fastTorqueRatioLookUpTable.Add( 0.6f, 1.130f );
        fastTorqueRatioLookUpTable.Add( 0.7f, 1.100f );
        fastTorqueRatioLookUpTable.Add( 0.8f, 1.065f );
        fastTorqueRatioLookUpTable.Add( 0.9f, 1.032f );
        fastTorqueRatioLookUpTable.Add( 1.0f, 1.000f );

        Stopwatch stopwatch = new Stopwatch();
        
        const int numberOfIterations = 1000000;

        // ------------------------
        
        Console.WriteLine("Linear Map ALT");
        
        // ------------------------
        
        stopwatch.Restart();
        for (int i = numberOfIterations-1; i>=0; i--)
        {
            float firstItem = torqueRatioLookUpTableAlt.Get(0.0f);
        }
        stopwatch.Stop();
        Console.WriteLine("torqueRatioLookUpTableAlt.Get(0.0f): " + stopwatch.ElapsedTicks + ", " + fastTorqueRatioLookUpTable.Get(0.0f));
        
        stopwatch.Restart();
        for (int i = numberOfIterations - 1; i >= 0; i--)
        {
            float midValue = torqueRatioLookUpTableAlt.Get(0.55f);
        }
        stopwatch.Stop();
        Console.WriteLine("torqueRatioLookUpTableAlt.Get(0.55f): " + stopwatch.ElapsedTicks + ", " + fastTorqueRatioLookUpTable.Get(0.55f));
        
        stopwatch.Restart();
        for (int i = numberOfIterations - 1; i >= 0; i--)
        {
            float lastValue = torqueRatioLookUpTableAlt.Get(1.0f);
        }
        stopwatch.Stop();
        Console.WriteLine("torqueRatioLookUpTableAlt.Get(1.0f): " + stopwatch.ElapsedTicks + ", " + fastTorqueRatioLookUpTable.Get(1.0f));
        
        // ------------------------
        
        Console.WriteLine("Slow look-up table");
        
        // ------------------------
        
        stopwatch.Restart();
        for (int i = numberOfIterations-1; i>=0; i--)
        {
            float firstItem = torqueRatioLookUpTable.Get(0.0f);
        }
        stopwatch.Stop();
        Console.WriteLine("torqueRatioLookUpTable.Get(0.0f): " + stopwatch.ElapsedTicks + ", " + torqueRatioLookUpTable.Get(0.0f));
        
        stopwatch.Restart();
        for (int i = numberOfIterations - 1; i >= 0; i--)
        {
            float midValue = torqueRatioLookUpTable.Get(0.55f);
        }
        stopwatch.Stop();
        Console.WriteLine("torqueRatioLookUpTable.Get(0.55f): " + stopwatch.ElapsedTicks + ", " + torqueRatioLookUpTable.Get(0.55f));
        
        stopwatch.Restart();
        for (int i = numberOfIterations - 1; i >= 0; i--)
        {
            float lastValue = torqueRatioLookUpTable.Get(1.0f);
        }
        stopwatch.Stop();
        Console.WriteLine("torqueRatioLookUpTable.Get(1.0f): " + stopwatch.ElapsedTicks + ", " + torqueRatioLookUpTable.Get(1.0f));
        
        // ------------------------
        
        Console.WriteLine("Fast look-up table");
        
        // ------------------------
        
        stopwatch.Restart();
        for (int i = numberOfIterations-1; i>=0; i--)
        {
            float firstItem = fastTorqueRatioLookUpTable.Get(0.0f);
        }
        stopwatch.Stop();
        Console.WriteLine("fastTorqueRatioLookUpTable.Get(0.0f): " + stopwatch.ElapsedTicks + ", " + fastTorqueRatioLookUpTable.Get(0.0f));
        
        stopwatch.Restart();
        for (int i = numberOfIterations - 1; i >= 0; i--)
        {
            float midValue = fastTorqueRatioLookUpTable.Get(0.55f);
        }
        stopwatch.Stop();
        Console.WriteLine("fastTorqueRatioLookUpTable.Get(0.55f): " + stopwatch.ElapsedTicks + ", " + fastTorqueRatioLookUpTable.Get(0.55f));
        
        stopwatch.Restart();
        for (int i = numberOfIterations - 1; i >= 0; i--)
        {
            float lastValue = fastTorqueRatioLookUpTable.Get(1.0f);
        }
        stopwatch.Stop();
        Console.WriteLine("fastTorqueRatioLookUpTable.Get(1.0f): " + stopwatch.ElapsedTicks + ", " + fastTorqueRatioLookUpTable.Get(1.0f));
    }
}