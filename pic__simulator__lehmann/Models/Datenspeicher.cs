namespace pic__simulator__lehmann.Models
{
    public class Datenspeicher: Speicher
    {
        // Bank0 = 0 - 127
        // Bank1 = 128 - 255

        public Datenspeicher(int size) : base(size)
        {
            //Bank 0
            _speicher[0] = new Fsr(); //indf
            _speicher[1] = new Tmr0(); ;
            _speicher[2] = new Pcl(); ;
            _speicher[3] = new StatusRegister();
            _speicher[4] = new Fsr();
            _speicher[5] = new PortA();
            _speicher[6] = new PortB();
            //_speicher[7] = ;
            _speicher[8] = new Eedata();
            _speicher[9] = new Eeadr();
            _speicher[10] = new Pclath();
            _speicher[11] = new Intcon();

            //Bank 1
            _speicher[12] = new Fsr();  //indf 
            _speicher[13] = new OptionReg();
            _speicher[14] = new Pcl();
            _speicher[15] = new StatusRegister();
            _speicher[16] = new Fsr();
            _speicher[17] = new TrisA();
            _speicher[18] = new TrisB();
            //_speicher[19] = ;
            _speicher[20] = new Eecon1();
            _speicher[21] = new Eecon2();
            _speicher[22] = new Pclath();
            _speicher[23] = new Intcon();
        } 


   

        public void Write(int addr, int value)
        {
            _speicher[addr] = value;
        }
    }
}
