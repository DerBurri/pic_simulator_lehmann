using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace pic__simulator__lehmann.Models
{
    public class Programmspeicher : Speicher
    {
        public Programmspeicher(int size, List<String> programm) : base(size)
        {
            int b = 0;
            foreach (String line in programm)
            {
                //TODO Regex ändern und only First Match einfügen
                Match match = Regex.Match(line,@" [0-9a-fA-F]{4} ");
                if (match.Success)
                {
                    match.Value.Trim();
                    _speicher[b] = Int32.Parse(match.Value,NumberStyles.HexNumber);
                    b++;
                }

                


            }
        }
    }
}
