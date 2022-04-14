namespace pic__simulator__lehmann.Models
{
    public class Datenspeicher: Speicher
    {
        public Datenspeicher(int size) : base(size)
        {

        }

        public override void Write(int addr, int value)
        {
            _speicher[addr] = value;
        }
    }
}
