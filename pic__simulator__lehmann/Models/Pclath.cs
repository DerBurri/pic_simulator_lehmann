using System.Collections;

namespace pic__simulator__lehmann.Models
{
    public class Pclath : Register
    {
        public Pclath()
        {
               _inhalt = new BitArray(5);
               _inhalt.SetAll(false);
               } 
        }
    
}
