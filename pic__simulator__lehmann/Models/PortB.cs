namespace pic__simulator__lehmann.Models;

public class PortB : Register
{
    private bool RB0 { get; set; }
    private bool RB1 { get; set; }
    private bool RB2 { get; set; }
    private bool RB3 { get; set; }
    private bool RB4 { get; set; }
    private bool RB5 { get; set; }
    private bool RB6 { get; set; }
    private bool RB7 { get; set; }

    public PortB()
    {
        RB0 = false;
        RB1 = false;
        RB2 = false;
        RB3 = false;
        RB4 = false;
        RB5 = false;
        RB6 = false;
        RB7 = false;

    }


    public override int Read()
    {
        int bits = 0;

        //bits = bits << Convert.ToInt32();

        return bits;
    }
}