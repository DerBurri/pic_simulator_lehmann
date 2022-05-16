namespace pic__simulator__lehmann.Models
{
    public class Eecon1: Register
    {
        private bool RD { get; set; }
        /* Read Control Bit
         * 0 = Does not initiate an EEPROM read
         * 1 = Initiates an EEPROM read (read takes one cycle. RD is cleared in hardware. The RD bit can only be set (not cleared) in software).
         */
        private bool WR { get; set; }
        /* Write Control Bit
         * 0 = Write cycle to the data EEPROM is complete
         * 1 = initiates a write cycle. (The bit is cleared by hardware once write is complete. The WR bit can only be set (not cleared) in software.
         */
        private bool WREN { get; set; }
        /* EEPROM Write Enable bit
         * 0 = Inhibits write to the data EEPROM
         * 1 = Allows write cycles
         */
        private bool WRERR { get; set; }
        /* EEPROM Error Flag bit
         * 0 = The write operation completed
         * = A write operation is prematurely terminated (any MCLR reset or any WDT reset during normal operation)
         */
        private bool EEIF { get; set; }
        /* EEPROM Write Operation Interrupt Flag bit
         * 0 = The write operation is not complete or has not been started
         * 1 = The write operation completed (must be cleared in software)
         */

        /*
         * Bit 5 - 7 unimplemented : Read as '0'
         */

        public Eecon1()
        {
            RD = false;
            WR = false;
            WREN = false;
            WRERR = false;
            EEIF = false;


        }


        public override int Read()
        {
            int bits = 0;

            //bits = bits << Convert.ToInt32();

            return bits;
        }
    }
}
