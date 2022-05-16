namespace pic__simulator__lehmann.Models
{
    public class Intcon : Register
    {
        private bool RBIF { get; set; }
        /* RB Port Change Interrupt Flag bit 
         * 1 = When at least one of the RB7:RB4 pins changed state (must be cleared in software) 
         * 0 = None of the RB7:RB4 pins have changed state
         */

        private bool INTF { get; set; }
        /*
         * RB0/INT Interrupt Flag bit 
         * 0 = The RB0/INT interrupt did not occur
         * 1 = The RB0/INT interrupt occurred 
         */

        private bool T0IF { get; set; }
        /* TMR0 overflow interrupt flag bit 
         * 0 = TMR0 did not overflow
         * 1 = TMR0 has overflowed (must be cleared in software)
         */

        private bool RBIE { get; set; }
        /* RB Port Change Interrupt Enable bit 
         * 0 = Disables the RB port change interrupt
         * 1 = Enables the RB port change interrupt
         */

        private bool INTE { get; set; }
        /* RB0/INT Interrupt Enable bit 
         * 0 = Disables the RB0/INT interrupt
         * 1 = Enables the RB0/INT interrupt
         */

        private bool T0IE { get; set; }
        /* TMR0 Overflow Interrupt Enable bit 
         * 0 = Disables the TMR0 interrupt
         * 1 = Enables the TMR0 interrupt
         */

        private bool EEIE { get; set; }
        /* EE Write Complete Interrupt Enable bit 
         * 0 = Disables the EE write complete interrupt
         * 1 = Enables the EE write complete interrupt
         */

        private bool GIE { get; set; }
        /* Global Interrupt Enable bit 
         * 0 = Disables all interrupts
         * 1 = Enables all un-masked interrupts 
         */



    }
}
