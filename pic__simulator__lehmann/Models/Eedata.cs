namespace pic__simulator__lehmann.Models
{
    public class Eedata: Register
    {
        private bool eedata_0 { get; set; }
        private bool eedata_1 { get; set; }
        private bool eedata_2 { get; set; }
        private bool eedata_3 { get; set; }
        private bool eedata_4 { get; set; }
        private bool eedata_5 { get; set; }
        private bool eedata_6 { get; set; }
        private bool eedata_7 { get; set; }


        public Eedata()
        {
            eedata_0 = false;
            eedata_1 = false;
            eedata_2 = false;
            eedata_3 = false;
            eedata_4 = false;
            eedata_5 = false;
            eedata_6 = false;
            eedata_7 = false;
            


        }


        public override int Read()
        {
            int bits = 0;

            //bits = bits << Convert.ToInt32();

            return bits;
        }

    }
}
