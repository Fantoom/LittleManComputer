using LMC.Assembler;

var code = @"        
      LDA a
      STA num1      
      LDA b     
      ADD num1
      OUT
      HLT
	  
num1  DAT
a DAT 5
b DAT 10	
";

var assemblyCode = LMCAssembler.Assemble(code);
Console.WriteLine(string.Join(", ",  assemblyCode));