namespace pic__simulator__lehmann.Models
{
    public class OptionReg: Register
    {
        private bool PS0 { get; set; }
        /*
         */

        private bool PS1 { get; set; }
        /*
         */

        private bool PS2 { get; set; }
        /*PS2:PS0: Prescaler Rate Select bits    
         * 
         * Bit Value  |  TMR0 Rate  |  WDT bits
         * ------------------------------------
         *    000     |    1 : 2    |   1 : 1
         *    001     |    1 : 4    |   1 : 2 
         *    010     |    1 : 8    |   1 : 4
         *    011     |    1 : 16   |   1 : 8
         *    100     |    1 : 32   |   1 : 16
         *    101     |    1 : 64   |   1 : 32
         *    110     |    1 : 128  |   1 : 64
         *    111     |    1 : 256  |   1 : 128
        */

        private bool PSA { get; set; }
        /* Prescaler Assignment bit 
         * 0 = Prescaler assigned to TMR0
         * 1 = Prescaler assigned to the WDT 
         */

        private bool T0SE { get; set; }
        /*TMR0 Source Edge Select bit 
         * 0 = Increment on low-to-high transition on RA4/T0CKI pin
         * 1 = Increment on high-to-low transition on RA4/T0CKI pin 
         */

        private bool T0CS { get; set; }
        /* TMR0 Clock Source Select bit 
         * 0 = Internal instruction cycle clock (CLKOUT)
         * 1 = Transition on RA4/T0CKI pin 
         */

        private bool INTEDG { get; set; }
        /*Interrupt Edge Select bit 
         * 0 = Interrupt on falling edge of RB0/INT pin
         * 1 = Interrupt on rising edge of RB0/INT pin 
         */

        private bool RBPU { get; set; }
        /* RBPU: PORTB Pull-up Enable bit 
         * 0 = PORTB pull-ups are enabled (by individual port latch values)
         * 1 = PORTB pull-ups are disabled 
         */



        public OptionReg()
        {
            PS0 = false;
            PS1 = false;
            PS2 = false;
            PSA = false;
            T0SE = false;
            T0CS = false;
            INTEDG = false;
            RBPU = false;


        }


        public override int Read()
        {
            int bits = 0;

            //bits = bits << Convert.ToInt32();

            return bits;
        }
    }
}
