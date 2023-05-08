using LMC.Simulator;

var simulator = new LMCSimulator();
var program = new int[] { 507, 306, 508, 106, 902, 0, 0, 5, 10 };

simulator.LoadProgram(program);

simulator.OutputEvent += Console.WriteLine;

while (!simulator.IsHalted)
    simulator.MakeClockImpulse();