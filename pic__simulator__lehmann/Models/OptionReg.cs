using System.Collections;

namespace pic__simulator__lehmann.Models;

public class OptionReg : Register
{
    private readonly PIC16 _controller;

    public OptionReg(PIC16 controller)
    {
        _inhalt = new BitArray(8);
        _inhalt.SetAll(true);
        _controller = controller;
    }

    public override void Write(int value)
    {
        base.Write(value);
        _controller.ResetScaler();
    }
}