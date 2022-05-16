namespace pic__simulator__lehmann.Models
{
    public class Datenspeicher: Speicher
    {
        // Bank0 = 0 - 127
        // Bank1 = 128 - 255

        private Fsr _fsr;
        private Tmr0 _tmr0;
        private Pcl _pcl;
        private StatusRegister _statusRegister;
        private PortA _portA;
        private PortB _portB;
        private Eedata _eedata;
        private Eeadr _eeadr;
        private Pclath _pclath;
        private Intcon _intcon;

        private OptionReg _optionReg;

        private TrisA _trisA;
        private TrisB _trisB;

        private Eecon1 _eecon1;
        private Eecon2 _eecon2;


        public Datenspeicher(int size) : base(size)
        {
            //Bank 0
            _speicher[0] = _fsr; //indf
            _speicher[1] = _tmr0;
            _speicher[2] = _pcl;
            _speicher[3] = _statusRegister;
            _speicher[4] = _fsr;
            _speicher[5] = _portA;
            _speicher[6] = _portB;
            //_speicher[7] = ;
            _speicher[8] = _eedata;
            _speicher[9] = _eeadr;
            _speicher[10] = _pclath;
            _speicher[11] = _intcon;

            //Bank 1
            _speicher[12] = _fsr;  //indf 
            _speicher[13] = _optionReg;
            _speicher[14] = _pcl;
            _speicher[15] = _statusRegister;
            _speicher[16] = _fsr;
            _speicher[17] = _trisA;
            _speicher[18] = _trisB;
            //_speicher[19] = ;
            _speicher[20] = _eecon1;
            _speicher[21] = _eecon2;
            _speicher[22] = _pclath;
            _speicher[23] = _intcon;
        } 


   

        public void Write(int addr, int value)
        {
            _speicher[addr] = value;
        }
    }
}
