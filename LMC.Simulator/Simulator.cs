using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMC.Simulator;

public class LMCSimulator
{
    public int Input { get; set; }
    public int Output { get; set; }
    public int Accumulator { get; private set; }
    
    public int InstructionPointer { get; private set; }
    public bool OverflowRegister { get; private set; }
    public bool UnderflowRegister { get; private set; }
    public bool IsHalted { get; private set; }

    public bool InputRequired = false;
    public event Action? HaltEvent;
    public event Action<int>? OutputEvent;
    public event Func<int>? InputEvent;
    public IReadOnlyList<int> Memory => memory;

    private int[] memory = new int[100];


    public void LoadProgram(int[] program)
    {
        Array.Copy(program, memory, program.Length);
        InstructionPointer = 0;
        Accumulator = 0;
        OverflowRegister = false;
        UnderflowRegister = false;
        IsHalted = false;
    }

    public void MakeClockImpulse()
    {
        InputRequired = false;
        if (IsHalted)
            return;
        var instruction = Decode(memory[InstructionPointer]);
        ExecuteInstruction(instruction);
    }

    private void ExecuteInstruction(Instruction instruction)
    {
        if (instruction.Value is { } value)
        {
            switch (instruction.OpCode)
            {
                case OpCodes.BRA:
                    InstructionPointer = value;
                    return;
                case OpCodes.BRZ:
                    if(Accumulator == 0 && !OverflowRegister && !UnderflowRegister)
                        InstructionPointer = value;
                    return;
                case OpCodes.BRP:
                    if (Accumulator >= 0 && !UnderflowRegister)
                        InstructionPointer = value;
                    return;
            }

            switch (instruction.OpCode)
            {
                case OpCodes.ADD:
                    Accumulator += memory[value];
                    if(Accumulator > 999)
                    {
                        Accumulator %= 1000;
                        OverflowRegister = true;
                    }
                    break;
                case OpCodes.SUB:
                    Accumulator -= memory[value];
                    if (Accumulator < 0)
                    {
                        Accumulator += 1000;
                        UnderflowRegister = true;
                    }
                    break;
                case OpCodes.STA:
                    memory[value] = Accumulator;
                    break;
                case OpCodes.LDA:
                    Accumulator = memory[value];
                    break;
            }
        }
        else
        {
            switch (instruction.OpCode)
            {
                case OpCodes.INP:
                    InputRequired = true;
                    if (InputEvent is { })
                        Input = InputEvent();
                    Accumulator = Input;
                    break;
                case OpCodes.OUT:
                    Output = Accumulator;
                    if (OutputEvent is { })
                        OutputEvent(Output);
                    break;
                case OpCodes.HLT:
                    IsHalted = true;
                    HaltEvent?.Invoke();
                    break;
            }
        }

        InstructionPointer = Math.Min(InstructionPointer + 1, 100);
    }

    private Instruction Decode(int rawInstruction)
    {
        var length = Math.Floor(Math.Log10(rawInstruction)) + 1;
        var powerOfTen = (int)Math.Pow(10, length - 1);
        return rawInstruction switch
        {
            (int)OpCodes.INP => new Instruction(OpCodes.INP, null),
            (int)OpCodes.OUT => new Instruction(OpCodes.OUT, null),
            (int)OpCodes.HLT => new Instruction(OpCodes.HLT, null),
            _ => new Instruction((OpCodes)(rawInstruction / powerOfTen), rawInstruction % powerOfTen)
        };
    }
}
