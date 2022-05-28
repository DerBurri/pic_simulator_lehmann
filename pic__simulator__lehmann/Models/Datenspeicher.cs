using System.Data;

namespace pic__simulator__lehmann.Models
{
    public class Datenspeicher
    {
        // Bank0 = 0 - 127
        // Bank1 = 128 - 255
        public readonly Register[] _speicher;
        private readonly int _size;
        private PIC16 _controller;
        


        public Datenspeicher(int size, PIC16 controller)
        {
            _size = size;
            _speicher = new Register[_size];
            for (int i = 0; i < _speicher.Length; i++)
            {
                _speicher[i] = new Register();
            }

            _controller = controller;
            //Bank 0
            //TODO Spiegelung und gute 
            //indf
            
            _speicher[1] = new Tmr0(); ;
            _speicher[2] = new Pcl(); ;
            _speicher[3] = new StatusRegister();
            _speicher[4] = new Fsr();
            
            //Notwendig da IND auf andere Register für die indirekte Adressierung zugreift.
            _speicher[0] = new IND(_speicher[4], this);
            _speicher[5] = new PortA();
            _speicher[6] = new PortB();
            //_speicher[7] = ;
            _speicher[8] = new Eedata();
            _speicher[9] = new Eeadr();
            _speicher[10] = new Pclath();
            _speicher[11] = new Intcon();
            
            

            //Bank 1
            _speicher[128] = _speicher[4];//indf 
            _speicher[129] = new OptionReg(_controller);
            _speicher[130] = _speicher[2];
            _speicher[131] = _speicher[3];
            _speicher[132] = _speicher[4];
            _speicher[133] = new TrisA();
            _speicher[134] = new TrisB();
            //_speicher[19] = ;
            _speicher[136] = new Eecon1();
            _speicher[137] = new Eecon2();
            _speicher[138] = _speicher[10];
            _speicher[139] = _speicher[11];
        } 

        public Register At(int index,bool ignoreBankValue = false)
        {
            if (index > _size)
            {
                throw new OverflowException("Datenspeicher Ende erreicht");
            }
            
            if (_speicher[3].ReadBit(5) && !(ignoreBankValue))
            {
                return _speicher[index + 128];
            }
            else
            {
                return _speicher[index];
            }
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
