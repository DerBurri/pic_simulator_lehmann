using System.Collections;

namespace pic__simulator__lehmann.Models;

public class Register
{
    protected BitArray _inhalt;

    public Register()
    {
        _inhalt = new BitArray(8);
        _inhalt.SetAll(false);
    }
    public virtual int Read()
    {
        int value = 0;
        for (int i = 0; i < _inhalt.Count; i++)
        {
            if (_inhalt[i])
                value += Convert.ToInt16(Math.Pow(2, i));
        }
        return value;
    }

    public virtual bool ReadBit(int bitnumber)
    {
        return _inhalt[bitnumber];
    }

    public virtual void Write(int value)
    {
        value = value & 255;
        _inhalt = new BitArray(new int[] {value});
    }

    public virtual void WriteBit(int bit, bool value)
    {
        _inhalt.Set(bit, value);
    }


}