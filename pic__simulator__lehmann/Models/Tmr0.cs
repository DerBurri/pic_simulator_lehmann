namespace pic__simulator__lehmann.Models
{
    public class Tmr0: Register
    {
        private bool tmr0_0 { get; set; }
        private bool tmr0_1 { get; set; }
        private bool tmr0_2 { get; set; }
        private bool tmr0_3 { get; set; }
        private bool tmr0_4 { get; set; }
        private bool tmr0_5 { get; set; }
        private bool tmr0_6 { get; set; }
        private bool tmr0_7 { get; set; }


        public Tmr0()
        {
            tmr0_0 = false;
            tmr0_1 = false;
            tmr0_2 = false;
            tmr0_3 = false;
            tmr0_4 = false;
            tmr0_5 = false;
            tmr0_6 = false;
            tmr0_7 = false;


        }


        public override int Read()
        {
            int bits = 0;

            //bits = bits << Convert.ToInt32();

            return bits;
        }
    }
}
