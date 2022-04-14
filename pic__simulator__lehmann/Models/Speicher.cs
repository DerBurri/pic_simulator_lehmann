namespace pic__simulator__lehmann.Models
{
    public abstract class Speicher
    {
        
        protected int[] _speicher;
        protected int _size;

        public Speicher(int size)
        {
            _speicher = new int[size];
            _speicher.Initialize();
            _size = size;
        }
        
        public void Read(int index){}

        public virtual void Write(int addr, int value)
        {
        }
    }
}
