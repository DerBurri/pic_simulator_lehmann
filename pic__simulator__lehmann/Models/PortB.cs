using System.Collections;

namespace pic__simulator__lehmann.Models;

public class PortB : Register
{
    private Register _trisB;
    public PortB(Register trisB)
    {
        _trisB = trisB;
    }
    public override void Write(int value)
    {
        value = value & 255;
        _inhalt = new BitArray(new int[] {value});
        for (int i = 0; i < 8; i++)
        {
            WriteBit(i,_inhalt[i]);
        }
    
    }

    public override void WriteBit(int bit, bool value)
    {
        if (_trisB.ReadBit(bit))
        {
            base.WriteBit(bit, value);
        }
    }

    
    public override int Read()
    {
        int value = 0;
        for (int i = 7; i > 0; i--)
        {
            value += Convert.ToInt32(ReadBit(i));
            value <<= 1;
        }

        value += Convert.ToInt32(ReadBit(0));
        return value & 255;
    }

    public override bool ReadBit(int bit)
    {
        if (!_trisB.ReadBit(bit))
        {
            return base.ReadBit(bit);
        }
        else
        {
            return false;
        }
    }
    
}