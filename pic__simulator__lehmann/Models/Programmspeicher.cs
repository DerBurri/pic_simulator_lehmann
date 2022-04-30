using System.Linq.Expressions;
using System.Text;

namespace pic__simulator__lehmann.Models
{
    public class Programmspeicher : Speicher
    {
        public Programmspeicher(int size) : base(size)
        {

            if (!File.Exists("geladenesProgramm"))
            {
                throw new FileNotFoundException();
            }
            else
            {
                FileStream fs = new FileStream("geladenesProgramm", FileMode.Open);
                using (StreamReader sr = new StreamReader(fs,Encoding.UTF8))
                {
                    String line;
                    int i = 0;
                    while((line = sr.ReadLine()) != null)
                    {
                        if (!line.StartsWith(" "))
                        {
                            String[] tokens = line.Split(" ");
                            _speicher[i] = Int32.Parse(tokens[1],System.Globalization.NumberStyles.HexNumber);
                        }
                    }
                }
                fs.Close();
            }
        }
    }
}
