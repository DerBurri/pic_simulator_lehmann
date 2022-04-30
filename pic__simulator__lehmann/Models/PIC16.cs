namespace pic__simulator__lehmann.Models
{
    public class PIC16
    {
        private Programmspeicher _programmspeicher;
        private Datenspeicher _datenspeicher;
        
        
        
        public PIC16()
        {
            _programmspeicher = new Programmspeicher(4096);
            _datenspeicher = new Datenspeicher(4096);
            //throw new NotImplementedException();
        }
        
    }
}
