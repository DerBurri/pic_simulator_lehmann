namespace pic__simulator__lehmann.Models
{
    public class Pcl:Register
    {
        private bool pcl_0 { get; set; }
        private bool pcl_1 { get; set; }
        private bool pcl_2{ get; set; }
        private bool pcl_3 { get; set; }
        private bool pcl_4 { get; set; }
        private bool pcl_5 { get; set; }
        private bool pcl_6 { get; set; }
        private bool pcl_7 { get; set; }



        public Pcl()
        {
            pcl_0 = false;
            pcl_1 = false;
            pcl_2 = false;
            pcl_3 = false;
            pcl_4 = false;

        }


        public override int Read()
        {
            int bits = 0;

            //bits = bits << Convert.ToInt32();

            return bits;
        }

    }
}
