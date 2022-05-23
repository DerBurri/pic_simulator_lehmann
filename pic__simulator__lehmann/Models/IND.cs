namespace pic__simulator__lehmann.Models;

public class IND : Register
{
    private Register _FSR;
    private Datenspeicher _Speicher;

    public IND(Register _fsr, Datenspeicher _speicher)
    {
        _FSR = _fsr;
        _Speicher = _speicher;
    }
    public override int Read()
    {
        //Nötig um Zirkelauslesen zu vermeiden
        if (_FSR.Read() == 0) return 0;
        Console.WriteLine("indirect Read at {0}", _FSR.Read());
        return _Speicher.At(_FSR.Read()).Read();
    }

    public override void Write(int value)
    {
        Console.WriteLine("indirect Write at {0}", _FSR.Read());
        _Speicher.At(_FSR.Read()).Write(value);
    }
}