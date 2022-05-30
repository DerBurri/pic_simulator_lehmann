using System.Collections;

namespace pic__simulator__lehmann.Models
{
    public class Pcl : Register
    {
        private Register _pclath;
        public Pcl(Register pclath)
        {
            _pclath = pclath;
            _inhalt = new BitArray(11);
            _inhalt.SetAll(false);
        }

        public override void Write(int value)
        {
            //TODO PCLATH richtig berechnen
            int pcl = value & 2047;
            _inhalt = new BitArray(new int[] {pcl});
            int pclath = value & 14336;
            pclath >>= 11;
            _pclath.Write(pclath);
        }
    }
}
