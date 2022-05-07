using System.Timers;
using RingByteBuffer;

namespace pic__simulator__lehmann.Models
{
    public class PIC16
    {
        public readonly ILogger<Programm> _logger;
        
        private Programmspeicher _programmspeicher;
        private Datenspeicher _datenspeicher;
        private System.Timers.Timer _taktgeber;
        
        private int _programmcounter;
        private RingBuffer _stack;
        

        public PIC16(int interval, ILogger<Programm> logger, List<String> _programm)
        {
            _logger = logger;
            _logger.LogWarning("Ausgabe Programmspeicher");
            _programmspeicher = new Programmspeicher(4096,_programm);
            foreach (var opcode in _programmspeicher._speicher)
            {
                _logger.LogWarning(opcode.ToString());
            }
            _datenspeicher = new Datenspeicher(4096);
            _stack = new SequentialRingBuffer(7);
            _programmcounter = 0;
            KonfiguriereTimer(interval);
            //throw new NotImplementedException();
        }

        private void KonfiguriereTimer(int interval)
        {
            _taktgeber = new System.Timers.Timer(interval);
            _taktgeber.Elapsed += OnTakt;
        }
        
        private void OnTakt(Object source, System.Timers.ElapsedEventArgs e)
        {
           //TODO checkInterrupt();
           int Befehl = _programmspeicher.Read(_programmcounter);
           _logger.LogInformation(Befehl.ToString());
           if (Befehl == 0)
           {
               _logger.LogInformation("NOP Befehl");
           }
           else if(Enumerable.Range(1,4095).Contains(Befehl)) //MSB Bit 1 und 2 ist 0
           {
               switch (Befehl)
               {
                   case (int) Befehlsliste.Befehle.CLRWDT: //CLRWDT
                       break;
                   case (int) Befehlsliste.Befehle.RETFIE: //RETFIE
                       break;
                   case (int) Befehlsliste.Befehle.RETURN: //RETURN
                       break;
                   case (int) Befehlsliste.Befehle.SLEEP: //SLEEP
                       break;
                   default:

                       break;
               }
           }
           else if (Enumerable.Range(4096, 8191).Contains(Befehl)) // 0x01
           {
               
           }
           else if (Enumerable.Range(8192, 12287).Contains(Befehl))//0x10
           {
               
           }
           else if (Enumerable.Range(12288, 16383).Contains(Befehl))
           {
               _logger.LogInformation("JAAAA");
               if (Befehlsliste.Befehle.MOVLW.Equals(Befehl & (int) Befehlsliste.Befehle.MOVLW))
               {
                   _logger.LogCritical("HEUREKA!! DAS WAR EIN MOVLW DU FIGGER");
               }



           }

           _logger.LogInformation("takt");
           _programmcounter++;
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
            OnTakt(null,null);
        }

        public void Reset()
        {
            _taktgeber.Stop();
            _programmcounter = 0;
        }
        
        public void IntervalChange(int interval)
        {
            _taktgeber.Interval = interval* 1000;
        }
        
    }
}
