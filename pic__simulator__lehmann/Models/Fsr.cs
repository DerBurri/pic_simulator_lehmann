namespace pic__simulator__lehmann.Models
{
    public class Fsr : Register
    {
        private bool fsr_0 { get; set; }
        private bool fsr_1 { get; set; }
        private bool fsr_2 { get; set; }
        private bool fsr_3 { get; set; }
        private bool fsr_4 { get; set; }
        private bool fsr_5 { get; set; }
        private bool fsr_6 { get; set; }
        private bool fsr_7 { get; set; }


        public Fsr()
        {
            fsr_0 = false;
            fsr_1 = false;
            fsr_2 = false;
            fsr_3 = false;
            fsr_4 = false;
            fsr_5 = false;
            fsr_6 = false;
            fsr_7 = false;

        }

        public override void Write(int value)
        {
            fsr_0 = true;
        }

        public override int Read()
        {
            int bits = 0;

            //bits = bits << Convert.ToInt32();

            return bits;
        }

    }
}
