namespace pic__simulator__lehmann.Models
{
    public class Datenspeicher: Speicher
    {

        private StatusRegister _StatusRegister;
        private PortA _portA;
        private PortB _portB;
        
        public Datenspeicher(int size) : base(size)
        {
            
        }
        
        public void Write(int addr, int value)
        {
            _speicher[addr] = value;
        }
    }
}
