namespace pic__simulator__lehmann.Models;

public class PortA : Register
{
    private bool RA0 { get; set; }  /* Input/Output */
    private bool RA1 { get; set; }  /* Input/Output */
    private bool RA2 { get; set; }  /* Input/Output */
    private bool RA3 { get; set; }  /* Input/Output */
    private bool RA4 { get; set; }  
    /* RA4 / T0CKI
     * Input/Output or external input for TMR0
     * Output is open drain type
     */

}