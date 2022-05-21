using System.Data;

namespace pic__simulator__lehmann.Models
{
    public class Datenspeicher
    {
        // Bank0 = 0 - 127
        // Bank1 = 128 - 255
        public readonly Register[] _speicher;
        private readonly int _size;
        
        public Datenspeicher(int size)
        {
            _size = size;
            _speicher = new Register[_size];
            for (int i = 0; i < _speicher.Length; i++)
            {
                _speicher[i] = new Register();
            }
            bool bank;
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
            _speicher[128] = new Fsr();  //indf 
            _speicher[129] = new OptionReg();
            _speicher[130] = new Pcl();
            _speicher[131] = new StatusRegister();
            _speicher[132] = new Fsr();
            _speicher[133] = new TrisA();
            _speicher[134] = new TrisB();
            //_speicher[19] = ;
            _speicher[136] = new Eecon1();
            _speicher[137] = new Eecon2();
            _speicher[138] = new Pclath();
            _speicher[139] = new Intcon();
        } 

        public Register At(int index)
        {
            if (index > _size)
            {
                throw new OverflowException("Programmspeicher Ende erreicht");
            }
            
            return _speicher[index];
        }
        public void Write(int addr, int value)
        {
            _speicher[addr].Write(value);
        }

        public int[,] GetRamArray()
        {
            int[,] ram = new int[_size, 8];
            for (int i = 0; i < _speicher.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ram[i,j] = _speicher[i+j].Read();
                }
            }

            return ram;
        }
    }
}
