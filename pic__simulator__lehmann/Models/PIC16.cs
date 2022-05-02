﻿using System.Timers;

namespace pic__simulator__lehmann.Models
{
    public class PIC16
    {
        private Programmspeicher _programmspeicher;
        private Datenspeicher _datenspeicher;
        private System.Timers.Timer _taktgeber;

        private int _programmcounter;
        

        public PIC16(int interval)
        {
            _programmspeicher = new Programmspeicher(4096);
            _datenspeicher = new Datenspeicher(4096);
            KonfiguriereTimer(interval);
            //throw new NotImplementedException();
        }

        private void KonfiguriereTimer(int interval)
        {
            _taktgeber = new System.Timers.Timer(interval);
            _taktgeber.Elapsed += OnTakt;
            _taktgeber.Start();
        }
        
        private void OnTakt(Object source, System.Timers.ElapsedEventArgs e)
        {
            
        }

        public void Stop()
        {
            _taktgeber.Stop();
        }

        public void Start()
        {
            _taktgeber.Start();
            _taktgeber.AutoReset = true;
        }

        public void Step()
        {
            _taktgeber.AutoReset = false;
            _taktgeber.Start();
        }
        
    }
}
