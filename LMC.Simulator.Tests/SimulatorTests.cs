namespace LMC.Simulator.Tests;

public class SimulatorTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(10, 5)]
    [InlineData(999, 1)]
    public void AddTwoNumbers(int a, int b)
    {
        var simulator = new LMCSimulator();
        var program = new int[] { 507, 306, 508, 106, 902, 0, 0, a, b };

        simulator.LoadProgram(program);

        while (!simulator.IsHalted)
            simulator.MakeClockImpulse();

        Assert.Equal((a + b) % 1000, simulator.Output);
    }
}