namespace pic__simulator__lehmann.Models;

public class StatusRegister : Register
{
    private bool Carry { get; set; }
    /*
     * 0 = no carry-out from the most significant bit of the result occured
     * 1 = a carry-out from the most significant bit of the rsult occured
    */

    private bool DigitCarry { get; set; }
    /*
     * 0 = no carry-out from the 4th lower order bit pf the result
     * 1 = a carry-out from the 4th lower order bit of the result occured
    */

    private bool ZeroBit { get; set; }
    /*
     * 0 = result of arithmetic or logic operation is not zero
     * 1 = result of arithmetic or logic operation is zero
    */
    
    private bool PowerDownBit { get; set; }
    /*
     * 0 = by exection of the SLEEP instruction
     * 1 = after power-up or by the CLRWDT instruction
    */

    private bool TimeOutBit { get; set; }
    /*
     * 0 = A Watchdog Timer time-out occured
     * 1 = after power-up, CLRWDT instruction, or SLEEP instruction
    */

    private bool RegisterBankSelectBit0 { get; set; } //RP0
    private bool RegisterBankSelectBit1 { get; set; } //RP1
    /* 
     * RP1:RP0
     * 00 = Bank 0 (00h - 7Fh)
     * 01 = Bank 1 (80h - FFh)
     * 10 = Bank 2 (100h - 17Fh)
     * 11 = Bank 3 (180h - 1FFh)
     */

    private bool IndirectRegisterBankSelectBit { get; set; }
    /*
     * 0 = Bank 0, 1 (00h - FFh)
     * 1 = Bank 2, 3 (100h - 1FFh)
     */


    public StatusRegister()
    {
        Carry = false;
        DigitCarry = false;
        ZeroBit = false;
        PowerDownBit = false;
        TimeOutBit = false;
        RegisterBankSelectBit0 = false;
        RegisterBankSelectBit1 = false;
        IndirectRegisterBankSelectBit = false;
    }


    public override int Read()
    {
        int bits = 0;

        bits = bits << Convert.ToInt32(Carry);
        bits = bits << Convert.ToInt32(DigitCarry);
        bits = bits << Convert.ToInt32(ZeroBit);
        bits = bits << Convert.ToInt32(PowerDownBit);
        bits = bits << Convert.ToInt32(TimeOutBit);
        bits = bits << Convert.ToInt32(RegisterBankSelectBit0);
        bits = bits << Convert.ToInt32(RegisterBankSelectBit1);
        bits = bits << Convert.ToInt32(IndirectRegisterBankSelectBit);

        return bits;
    }

    public override void WriteBit(int bit, bool value)
    {
     switch (bit)
     {
      case 0: Carry = value;
       break;
      case 1: DigitCarry = value;
       break;
      case 2: ZeroBit = value;
       break;
      case 3: PowerDownBit = value;
       break;
      case 4: TimeOutBit = value;
       break;
      case 5: RegisterBankSelectBit0 = value;
       break;
      case 6: RegisterBankSelectBit1 = value;
       break;
      case 7: IndirectRegisterBankSelectBit = value;
       break;
     }
    }

    public void setCarry()
    {
     Carry = true;
    }

    public void removeCarry()
    {
     Carry = false;
    }
}