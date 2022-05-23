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
                String[] tokens = line.Split(';');

                var commandparts = tokens[0].Trim().Split(' ',StringSplitOptions.RemoveEmptyEntries);     

                if (commandparts.Length > 4)
                {
                    Console.WriteLine("Befehl erkannt, baue Programmspeicher Eintrag");

                    //Console.WriteLine(commandparts[1]);
                    _speicher[b] = Int32.Parse(commandparts[1], NumberStyles.HexNumber);
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
