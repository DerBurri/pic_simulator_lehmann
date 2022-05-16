namespace pic__simulator__lehmann.Models;

public class TrisA : Register
{
    private bool RA0 { get; set; }
    private bool RA1 { get; set; }
    private bool RA2 { get; set; }
    private bool RA3 { get; set; }
    private bool RA4 { get; set; }


    public TrisA()
    {
        RA0 = false;
        RA1 = false;
        RA2 = false;
        RA3 = false;
        RA4 = false;

    }


    public override int Read()
    {
        int bits = 0;

        //bits = bits << Convert.ToInt32();

        return bits;
    }
}