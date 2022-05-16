using System.Timers;
using RingByteBuffer;

namespace pic__simulator__lehmann.Models
{
    public class PIC16
    {
        private readonly ILogger<Programm> _logger;

        private Programmspeicher _programmspeicher;
        private Datenspeicher _datenspeicher;
        private int _w_registar;
        private System.Timers.Timer _taktgeber;

        private int _programmcounter;
        private RingBuffer _stack;


        public PIC16(int interval, ILogger<Programm> logger, List<String> _programm)
        {
            _logger = logger;
            _logger.LogWarning("Ausgabe Programmspeicher");
            _programmspeicher = new Programmspeicher(4096, _programm);
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
            _taktgeber = new System.Timers.Timer(interval*1000);
            _taktgeber.Elapsed += OnTakt;
        }

        private void OnTakt(Object source, System.Timers.ElapsedEventArgs e)
        {
            //TODO checkInterrupt();
            //Fetch
            int befehl = _programmspeicher.Read(_programmcounter);
            Console.Write(befehl);
            //Decode
            var decoded = Decode(befehl);
            Console.WriteLine("Folgender Befehl wurde erkannt {0}", decoded);
            //Execute
            execute(decoded);
            //_logger.LogInformation("takt");
            _programmcounter++;
        }

        private void execute(Befehlsliste.Befehle decoded)
        {
            throw new NotImplementedException();
        }

        private Befehlsliste.Befehle Decode(int Befehl)
        {
            int Befehlteil1 = (Befehl & (int) Befehlsliste.Befehlsmaske.MASKE2) / 256;
            int Befehlsteil2 = (Befehl & (int) Befehlsliste.Befehlsmaske.MASKE3) / 16;
            
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
            }
            if (Befehl == 0)
            {
                return Befehlsliste.Befehle.NOP;
            }
            if (Befehl is < 4096 and > 0)
            { // 0x00
                switch (Befehlteil1)
                {
                    case 0:
                        return Befehlsliste.Befehle.MOVWF;
                    case 1:
                        if (Befehlsteil2 > 128)
                        {
                            return Befehlsliste.Befehle.CLRF;
                        }
                        return Befehlsliste.Befehle.CLRW;
                    case 2:
                        return Befehlsliste.Befehle.SUBWF;
                    case 3:
                        return Befehlsliste.Befehle.DECF;
                    case 4:
                        return Befehlsliste.Befehle.IORWF;
                    case 5:
                        return Befehlsliste.Befehle.ANDWF;
                    case 6:
                        return Befehlsliste.Befehle.XORWF;
                    case 7:
                        return Befehlsliste.Befehle.ADDWF;
                    case 8:
                        return Befehlsliste.Befehle.MOVF;
                    case 9:
                        return Befehlsliste.Befehle.COMF;
                    case 0xA:
                        return Befehlsliste.Befehle.INCF;
                    case 0xB:
                        return Befehlsliste.Befehle.DECFSZ;
                    case 0xC:
                        return Befehlsliste.Befehle.RRF;
                    case 0xD:
                        return Befehlsliste.Befehle.RLF;
                    case 0xE:
                        return Befehlsliste.Befehle.SWAPF;
                    case 0xF:
                        return Befehlsliste.Befehle.INCFSZ;
                }
            }

            else if (Befehl is < 8192 and > 4095) // 0x01
            {
               if (false)
               {}
               else
               {
                   throw new NotImplementedException("Befehle noch nicht implementiert");
               }
            }
            else if (Befehl is < 12288 and > 8191) //0x10
            {
                if (false)
                {}
                else
                {
                    throw new NotImplementedException("Befehle noch nicht implementiert");
                }
            }
            else if (Befehl is < 16384 and > 12287) //0x11
            {
                if (Befehlteil1 is < 3 and >= 0)
                {
                    _logger.LogCritical("HEUREKA!! DAS WAR EIN MOVLW DU FIGGER");
                    return Befehlsliste.Befehle.MOVLW;
                }
                else
                {
                    throw new NotImplementedException("Befehe noch nicht vorhadnden");
                }
            }
            throw new Exception("Kein gültiger Befehl");
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

        private void movlw()
        {
            
        }
        
        private void andlw(){}
        
        private void iorlw(){}
        
        private void sublw(){}
        
        private void xorlw(){}
        
        private void addlw(){}
        
        private void _goto()
        {
            
        }
        
    }
}
