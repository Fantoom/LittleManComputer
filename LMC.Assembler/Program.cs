using LMC.Assembler;

var code = @"        
        INP
loop    OUT
        STA count
        SUB one
        STA count
        BRP loop
        HLT
		
one     DAT 1
count   DAT 0		
";

var assemblyCode = LMCAssembler.Assemble(code);
Console.WriteLine(string.Join(" ",  assemblyCode));