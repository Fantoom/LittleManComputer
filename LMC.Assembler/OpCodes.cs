namespace LMC.Assembler;

public enum OpCodes 
{
    ADD = 1, 
    SUB = 2,
    STA = 3,
    LDA = 5,
    BRA = 6,
    BRZ = 7,
    BRP = 8,
    INP = 901,
    OUT = 902,
    HLT = 000,
    DAT = -1
}
