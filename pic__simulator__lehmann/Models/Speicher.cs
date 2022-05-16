namespace pic__simulator__lehmann.Models
{
    public abstract class Speicher
    {
        
        public object[] _speicher;
        protected int _size;

        public Speicher(int size)
        {
            _speicher = new object[size];
            for (int i = 0; i < size; i++)
            {
                _speicher[i] = 0;
            }
            _size = size;
        }

        public int Read(int index)
        {
            if (index > _size)
            {
                throw new OverflowException("Programmspeicher Ende erreicht");
            }

            int inhalt = Convert.ToInt32(_speicher[index]);

            return inhalt;
        }
        
    }
}
