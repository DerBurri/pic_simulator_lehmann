using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pic__simulator__lehmann.Class
{
    public class PIC16
    {
        private Programmspeicher _programmspeicher;
        private Datenspeicher _datenspeicher;
        public PIC16()
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, string> Befehlsliste = new Dictionary<string, string>()
        {
            //BYTE-ORIENTED FILE REGISTER OPERATIONS
            {"ADDWF", "00 0111 dfff ffff"},
            {"ANDWF", "00 0101 dfff ffff"},
            {"CLRF", "00 0001 lffff ffff "},
            {"CLRW", "00 0001 oxxx xxxx"},
            {"COMF", "00 1001 dfff ffff"},
            {"DECF", "00 0011 dfff ffff"},
            {"DECFSZ", "00 1011 dfff ffff"},
            {"INCF", "00 1010 dfff ffff"},
            {"INCFSZ", "00 1111 dfff ffff"},
            {"IORWF", "00 0100 dfff ffff"},
            {"MOVF", "00 1000 dfff ffff"},
            {"MOVWF", "00 000 lfff ffff"},
            {"NOP", "00 0000 0xx0 0000"},
            {"RLF", "00 1101 dfff ffff"},
            {"RRF", "00 1100 dfff ffff"},
            {"SUBWF", "00 0010 dfff ffff"},
            {"SWAPF", "00 1110 dfff ffff"},
            {"XORWF", "00 0110 dfff ffff"},

            //BIT-ORIENTED FILE REGISTER OPERATIONS
            {"BCF", "01 00bb bfff ffff"},
            {"BSF", ""},
            {"BTFSC", ""},
            {"BTFSS", ""},



            {"", ""},
            {"", ""},
            {"", ""},
            {"", ""},
            {"", ""},


        };

        private void ProgrammEinlesen(string pfad)
        {
            Parser.Parser ProgrammParser = new Parser.Parser(pfad);
        }
    }
}
