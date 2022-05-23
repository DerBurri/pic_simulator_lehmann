namespace pic__simulator__lehmann.Models;

public class IND : Register
{
    public Register FSR;
    public Register[] speicher;

    public IND(Register _fsr, Register[] _speicher)
    {
        FSR = _fsr;
        speicher = _speicher;
    }
    public override int Read()
    {
        return speicher[FSR.Read()].Read();
    }
}