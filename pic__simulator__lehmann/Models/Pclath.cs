namespace pic__simulator__lehmann.Models
{
    public class Pclath : Register
    {
        private bool pclath_0 { get; set; }
        private bool pclath_1 { get; set; }
        private bool pclath_2 { get; set; }
        private bool pclath_3 { get; set; }
        private bool pclath_4 { get; set; }

        //5-7 nicht implentiert

        public Pclath()
        {
            pclath_0 = false;
            pclath_1 = false;
            pclath_2 = false;
            pclath_3 = false;
            pclath_4 = false;

        }


        public override int Read()
        {
            int bits = 0;
            bits += Convert.ToInt32(pclath_4);
            bits <<= 1;
            bits += Convert.ToInt32(pclath_3);
            bits <<= 1;
            return bits;
        }
    }
}
