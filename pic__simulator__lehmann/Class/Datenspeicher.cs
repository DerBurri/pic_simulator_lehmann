using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pic__simulator__lehmann.Class
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
