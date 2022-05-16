namespace pic__simulator__lehmann.Models
{
    public class Eeadr: Register
    {
        private bool eeadr_0 { get; set; }
        private bool eeadr_1 { get; set; }
        private bool eeadr_2 { get; set; }
        private bool eeadr_3 { get; set; }
        private bool eeadr_4 { get; set; }
        private bool eeadr_5 { get; set; }
        private bool eeadr_6 { get; set; }
        private bool eeadr_7 { get; set; }

        public Eeadr()
        {
            eeadr_0 = false;
            eeadr_1 = false;
            eeadr_2 = false;
            eeadr_3 = false;
            eeadr_4 = false;
            eeadr_5 = false;
            eeadr_6 = false;
            eeadr_7 = false;


        }


        public override int Read()
        {
            int bits = 0;

            //bits = bits << Convert.ToInt32();

            return bits;
        }
    }
}
