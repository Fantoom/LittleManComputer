using System.Globalization;

namespace LMC.Assembler;

public static class LMCAssembler
{
    private static Instruction[] ParseInstructions(string assemblyCode)
    {
        Dictionary<string, int> labels = new();
        var splitedCode = assemblyCode.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var unparsedOpcodes = splitedCode.Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)).ToArray();

        var parsedOpcodes = unparsedOpcodes.Select(ParseOpcode).ToArray();

        Instruction[] result = new Instruction[parsedOpcodes.Length];
        labels = parsedOpcodes.Select((x, i) => (opcode: x, i)).Where(x => x.opcode.Label is { }).ToDictionary(k => k.opcode.Label!, e => (int)e.i);
        for (int i = 0; i < parsedOpcodes.Length; i++)
        {
            var opcode = parsedOpcodes[i];
            var value = opcode.Value;
            if (opcode.TargetLabel is { } targetLabel && labels.TryGetValue(targetLabel, out var tryValue))
                value = tryValue;
            result[i] = new Instruction(opcode.OpCode, value);
        }

        return result;
    }
    public static int[] Assemble(string assemblyCode)
    {
        var instructions = ParseInstructions(assemblyCode);
        return instructions.Select(x =>
        {
            if (x.Value is { } value)
            {
                if ((int)x.OpCode > 0)
                    return Concat((int)x.OpCode, value);
                else
                    return value;
            }
            else if ((int)x.OpCode > 0)
                return (int)x.OpCode;
            else
                return 0;
        }).ToArray();
    }

    private static ParsedInstruction ParseOpcode(string[] x)
    {
        ParsedInstruction result;
        if (x.Length >= 3)
        {
            int? value = null;
            string? valueLabel = null;
            if (int.TryParse(x[2], out var tryValue))
                value = tryValue;
            else
                valueLabel = x[2];
            var opcode = Enum.Parse<OpCodes>(x[1]);
            result = new(x[0], opcode, value, valueLabel);
        }
        else if (x.Length == 2)
        {
            if (Enum.TryParse<OpCodes>(x[0], out var opcode))
            {
                int? value = null;
                string? valueLabel = null;
                if (int.TryParse(x[1], out var tryValue))
                    value = tryValue;
                else
                    valueLabel = x[1];
                result = new ParsedInstruction(null, opcode, value, valueLabel);
            }
            else
                result = new ParsedInstruction(x[0], Enum.Parse<OpCodes>(x[1]), null, null);
        }
        else
            result = new ParsedInstruction(null, Enum.Parse<OpCodes>(x[0]), null, null);
        return result;
    }
    private static int Concat(int a, int b)
    {
        FormattableString formattableString = $"{a}{b:00}";
        return Convert.ToInt32(formattableString.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
    }
}
