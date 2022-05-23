using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace pic__simulator__lehmann.Models
{
    public class Programmspeicher
    {
        public readonly int[] _speicher;
        private int _size;
        public Programmspeicher(int size, List<String> programm)
        {
            _size = size;
            _speicher = new int[_size];
            for (int i = 0; i < _size; i++)
            {
                _speicher[i] = 0;
            }
            
            int b = 0;
            foreach (String line in programm)
            {
                if (Char.IsDigit(line[0]))
                {
                    
                    String  command = line.Substring(5, 4);
                _speicher[b] = Int32.Parse(command, NumberStyles.HexNumber);
                b++;
                }
            }
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
