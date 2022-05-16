namespace pic__simulator__lehmann.Models
{
    public class Eecon2 : Register
    {
        private bool eecon2_0 { get; set; }
        private bool eecon2_1 { get; set; }
        private bool eecon2_2 { get; set; }
        private bool eecon2_3 { get; set; }
        private bool eecon2_4 { get; set; }
        private bool eecon2_5 { get; set; }
        private bool eecon2_6 { get; set; }
        private bool eecon2_7 { get; set; }


        public Eecon2()
        {
            eecon2_0 = false;
            eecon2_1 = false;
            eecon2_2 = false;
            eecon2_3 = false;
            eecon2_4 = false;
            eecon2_5 = false;
            eecon2_6 = false;
            eecon2_7 = false;

        }


        public override int Read()
        {
            int bits = 0;

            //bits = bits << Convert.ToInt32();

            return bits;
        }
    }
}
