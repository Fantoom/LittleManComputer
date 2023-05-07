namespace LMC.Assembler;

internal record struct ParsedInstruction(string? Label, OpCodes OpCode, int? Value, string? TargetLabel);
