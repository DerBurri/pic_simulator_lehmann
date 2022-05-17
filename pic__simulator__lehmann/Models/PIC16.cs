﻿using System.Collections;
using System.Timers;
using RingByteBuffer;

using static pic__simulator__lehmann.Models.Befehlsliste;

namespace pic__simulator__lehmann.Models
{
    public class PIC16
    {
        private readonly ILogger<Programm> _logger;

        private Programmspeicher _programmspeicher;
        private Datenspeicher _datenspeicher;
        private int _w_register;
        private System.Timers.Timer _taktgeber;

        private int _programmcounter;
        private RingBuffer _stack;
        
        public int W_register
        {
            get { return _w_register; }
        }

        public bool[] StatusRegister
        {
            get
            {
                _logger.LogCritical("Status Flags {0}",_datenspeicher.Read(3).Read().ToString());
                return new BitArray(new byte[] {(byte) _datenspeicher.Read(3).Read()}).Cast<bool>().ToArray();
            }
        }


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

        private void OnTakt(Object source, ElapsedEventArgs e)
        {
            try
            {
                //TODO checkInterrupt();
                //Fetch
                int befehl = _programmspeicher.Read(_programmcounter);
                Console.Write(befehl);
                //Decode
                var decoded = Decode(befehl);
                Console.WriteLine("Folgender Befehl wurde erkannt {0}", decoded);
                //Execute
                execute(decoded, befehl);
                _logger.LogCritical("Programmzähler: {0}",_programmcounter.ToString());
                _programmcounter++;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                _programmcounter++;
            }
        }

        private void execute(Befehlsliste.Befehle decoded, int value)
        {
            switch (decoded)
            {
                    case Befehlsliste.Befehle.ADDWF :
                    break;
                    case Befehlsliste.Befehle.ANDWF :
                    break;
                    case Befehlsliste.Befehle.CLRF  :
                    break;
                    case Befehlsliste.Befehle.CLRW  :
                    break;
                    case Befehlsliste.Befehle.COMF  :
                    break;
                    case Befehlsliste.Befehle.DECF  :
                    break;
                    case Befehlsliste.Befehle.DECFSZ:
                    break;
                    case Befehlsliste.Befehle.INCF  :
                    break;
                    case Befehlsliste.Befehle.INCFSZ:
                    break;
                    case Befehlsliste.Befehle.IORWF :
                    break;
                    case Befehlsliste.Befehle.MOVF  :
                    break;
                    case Befehlsliste.Befehle.MOVWF :
                    break;
                    case Befehlsliste.Befehle.NOP   :
                    break;
                    case Befehlsliste.Befehle.RLF   :
                    break;
                    case Befehlsliste.Befehle.RRF   :
                    break;
                    case Befehlsliste.Befehle.SUBWF :
                    break;
                    case Befehlsliste.Befehle.SWAPF :
                    break;
                    case Befehlsliste.Befehle.XORWF :
                    break;
                    case Befehlsliste.Befehle.BCF   :
                    break;
                    case Befehlsliste.Befehle.BSF   :
                    break;
                    case Befehlsliste.Befehle.BTFSC :
                    break;
                    case Befehlsliste.Befehle.BTFSS :
                    break;
                    case Befehlsliste.Befehle.ADDLW :
                        addlw(value);
                    break;
                    case Befehlsliste.Befehle.ANDLW :
                        andlw(value);
                    break;
                    case Befehlsliste.Befehle.CALL  :
                    break;
                    case Befehlsliste.Befehle.GOTO  :
                        _goto(value);
                    break;
                    case Befehlsliste.Befehle.IORLW :
                        iorlw(value);
                    break;
                    case Befehlsliste.Befehle.MOVLW :
                        movlw(value);
                    break;
                    case Befehlsliste.Befehle.RETLW :
                    break;
                    case Befehlsliste.Befehle.SUBLW :
                        sublw(value);
                    break;
                    case Befehlsliste.Befehle.XORLW :
                        xorlw(value);
                    break;
                    case Befehlsliste.Befehle.ERROR :
                    break;
            }
            _logger.LogCritical("Inhalt W Register: {0}",_w_register);
        }

        private Befehlsliste.Befehle Decode(int Befehl)
        {
            int befehlteil1 = (Befehl & (int) Befehlsmaske.MASKE2) / 256;
            int befehlsteil2 = (Befehl & (int) Befehlsmaske.MASKE3) / 16;
            _logger.LogCritical(Befehl.ToString());
            _logger.LogCritical(Convert.ToString(Befehl,2));
            /*switch (Befehl)
            {
                case (int) Befehlsliste.Befehle.CLRWDT: //CLRWDT
                {
                    
                    break;
                }
                case (int) Befehlsliste.Befehle.RETFIE: //RETFIE
                    break;
                case (int) Befehlsliste.Befehle.RETURN: //RETURN
                    break;
                case (int) Befehlsliste.Befehle.SLEEP: //SLEEP
                    break;
            }*/
            if (Befehl == 0)
            {
                return Befehlsliste.Befehle.NOP;
            }
            if (Befehl is < 4096 and > 0)
            { // 0x00
                switch (befehlteil1)
                {
                    case 0:
                        return Befehlsliste.Befehle.MOVWF;
                    case 1:
                        if (befehlsteil2 > 128)
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
                if (befehlteil1 is < 16 and > 7)
                {
                    return Befehlsliste.Befehle.GOTO;
                }
                else
                {
                    throw new NotImplementedException("Befehle noch nicht implementiert");
                }
            }
            else if (Befehl is < 16384 and > 12287) //0x11
            {
                if (befehlteil1 is < 3 and >= 0)
                {
                    return Befehlsliste.Befehle.MOVLW;
                }
                else if (befehlteil1 is 0b1001)
                {
                    return Befehlsliste.Befehle.ANDLW;
                }
                else if (befehlteil1 is 0b1010)
                {
                    return Befehlsliste.Befehle.XORLW;
                }
                else if (befehlteil1 is 0b1110 or 0b1111)
                {
                    return Befehlsliste.Befehle.ADDLW;
                }
                else if (befehlteil1 is 0b1000)
                {
                    return Befehlsliste.Befehle.IORLW;
                }
                else if (befehlteil1 is 0b1100 or 0b1101)
                {
                    return Befehlsliste.Befehle.SUBLW;
                }
                else
                {
                    throw new NotImplementedException("Befehe noch nicht vorhadnden");
                }
            }

            return Befehlsliste.Befehle.ERROR;
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

        
        public void IntervalChange(int interval)
        {
            _taktgeber.Interval = interval* 1000;
        }

        public void checkZero()
        {
            if (_w_register == 0)
            {
                _datenspeicher._speicher[3].WriteBit(2, true);
            }
            else
            {
                _datenspeicher._speicher[3].WriteBit(2,false);
            }
        }
        
        private bool checkCarry()
        {
            if (_w_register > 255)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkDC(int value1, int value2)
        {
            value1 = value1 & 15;
            value2 = value2 & 15;
            int ergebnis = value2 + value1;
            
            if (ergebnis > 15)
            {
                return true;
            }
            return false;
        }

        private void movlw(int befehl)
        {
            _w_register = (byte) befehl;
        }

        private void andlw(int befehl)
        {
            _w_register = _w_register & (befehl & 255);
            checkZero();
        }

        private void iorlw(int befehl)
        {
            _w_register = _w_register | (befehl & 255);
            checkZero();
        }

        private void sublw(int befehl)
        {
            int payload = (befehl & 255);
            _w_register = ~_w_register;
            _w_register += 1;
            _w_register += payload;
           
            
            //Fehler im PIC, Carry wird falsch gesetzt. Carryflag müsste invertiert werden. Wenn Ergebnis von Subtraktion < 0 dann muss Carry gelöscht werden. Wenn Ergebnis > 0 muss Carry gesetzt werden
            if (checkCarry())
            {
                 _datenspeicher._speicher[3].WriteBit(0,false);
            }
            else
            {
                _datenspeicher._speicher[3].WriteBit(0,true);
                _logger.LogCritical("Carry Set");
            }
            // Setze übrige Bits aus Integer auf 0;
            _w_register &= 255;

            if (checkDC(_w_register,payload))
            {
                _datenspeicher._speicher[3].WriteBit(1,false);
            }
            else
            {
                _datenspeicher._speicher[3].WriteBit(1,true);
                _logger.LogCritical("DigitCarry Set");

            }

        }



        private void xorlw(int befehl)
        {
            int payload = befehl & 255;
            _w_register = payload ^ _w_register;
            checkZero();
        }

        private void addlw(int befehl)
        {
            int payload = befehl & 255;
            _w_register += payload;
            if (checkCarry())
            {
                _datenspeicher._speicher[3].WriteBit(0,true);
            }
            else
            {
                _datenspeicher._speicher[3].WriteBit(0,false);
            }

            if (checkDC(payload, befehl))
            {
                _datenspeicher._speicher[3].WriteBit(1, true);
            }
            else
            {
                _datenspeicher._speicher[3].WriteBit(1,false);
            }
            checkZero();
            _w_register &= 255;
        }
        
        private void _goto(int Befehl)
        {
            int payload = Befehl & 1023;
            _programmcounter++;
            _programmcounter = _datenspeicher.Read(4).Read();
            _programmcounter <<= 11;
            //Programmzähler wird wieder um eins erhöht dann steht die richtige Adresse drinnen.
            _programmcounter += payload - 1;
            _logger.LogCritical("Programm Counter {0}", payload);
        }
        
    }
}
