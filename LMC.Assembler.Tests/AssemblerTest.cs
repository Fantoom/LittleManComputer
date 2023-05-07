namespace LMC.Assembler.Tests;

[UseInvariantCulture]
public class AssemblerTest
{
    [Fact]
    public void TestCountDownTimer()
    {
        var code =
@"      INP
loop    OUT
        STA count
        SUB one
        STA count
        BRP loop
        HLT
		
one     DAT 1
count   DAT 0";

        var expected = new int[] {901, 902, 308, 207, 308, 801, 0, 1, 0};
        var actual = LMCAssembler.Assemble(code);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestTriangularNumbers()
    {
        var code =
@"loop  LDA number
ADD counter
OUT
STA number
LDA counter
ADD one
STA counter
LDA ten
SUB counter
BRP loop
HLT

counter DAT 1
number  DAT 0
one     DAT 1
ten     DAT 10";

        var expected = new int[] { 512, 111, 902, 
                                    312, 511, 113, 
                                    311, 514, 211, 
                                    800, 0, 1, 
                                    0, 1, 10 };
        var actual = LMCAssembler.Assemble(code);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestFactorial()
    {
        var code = @"
INP 
          STA final 
          BRZ oneval
          SUB one
          STA iteration
          STA counter
          LDA final
          STA num
mult      LDA iteration
          BRZ end
          SUB one
          BRZ end
          LDA final
          ADD num
          STA final
          LDA counter
          SUB one
          STA counter
          SUB one
          BRZ next
          BRA mult
next      LDA final
          STA num 
          LDA iteration
          SUB one
          STA iteration
          STA counter
          SUB one
          BRZ end
          BRA mult
end       LDA final
          OUT
          HLT
oneval    LDA one
          OUT
          HLT
		  
final     DAT 0
counter   DAT 0
one       DAT 1
iteration DAT 0
num       DAT 0";

        var expected = new int[] { 901, 336, 733, 238,
                                    339, 337, 536, 340,
                                    539, 730, 238, 730,
                                    536, 140, 336, 537,
                                    238, 337, 238, 721,
                                    608, 536, 340, 539,
                                    238, 339, 337, 238,
                                    730, 608, 536, 902,
                                    000, 538, 902, 000,
                                    000, 000, 001, 000,
                                    000 };
        var actual = LMCAssembler.Assemble(code);

        Assert.Equal(expected, actual);
    }
}