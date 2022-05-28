using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Timers;
using pic__simulator__lehmann.Pages;
using RingByteBuffer;

using static pic__simulator__lehmann.Models.Befehlsliste;

namespace pic__simulator__lehmann.Models
{
    public class PIC16
    {
        private readonly ILogger<Einlesen> _logger;
        public Programm _programm;

        private Programmspeicher _programmspeicher;
        private Datenspeicher _datenspeicher;
        
        private int _wregister;
        private bool nopcycle;
        
        private System.Timers.Timer _taktgeber;

        private int _scaler;
        private bool _RA4_timerWertAlt;

        
        private int _programmcounter
        {
            get => _datenspeicher.At(2).Read();
            set => _datenspeicher.At(2).Write(value);
        }
        
        private int _cyclecounter;
        private CircularBuffer<int> _stack;

        //Readonly Properties for UI
        public int W_register => _wregister;
        
        public int Timer0 => _datenspeicher.At(1, true).Read();

 
        public int Scaler => _scaler;
        

        public int GetRAMValueUI(int addr)
        {
            return _datenspeicher.At(addr,true).Read();
        }

        public void SetRAMValueUI(int addr, int value)
        {
            _datenspeicher.At(addr, true).Write(value);
        }


        public int GetPCL()
        {
            return _programmcounter;
        }

        public int GetPCLath()
        {
            return _datenspeicher.At(10,true).Read();
        }

        public int GetStatusRegister()
        {
            return _datenspeicher.At(3, true).Read();
        }
        public int GetOptionRegister()
        {
            return _datenspeicher.At(129, true).Read();
        }
        public int GetIntconRegister()
        {
            return _datenspeicher.At(11, true).Read();
        }

        public int GetFSR()
        {
            return _datenspeicher.At(4, true).Read();
        }

        public int[] GetStack()
        {
            return _stack.ToArray();
        }
        
        
        public bool[] StatusRegister
        {
            get
            {
                return new BitArray(new byte[] {(byte) _datenspeicher.At(3, true).Read()}).Cast<bool>().ToArray();
            }
        }
        public bool[] OptionRegister
        {
            get
            {
                return new BitArray(new byte[] {(byte) _datenspeicher.At(129, true).Read()}).Cast<bool>().ToArray();
            }
        }
        
        public bool[] IntConRegister
        {
            get
            {
                return new BitArray(new byte[] {(byte) _datenspeicher.At(11, true).Read()}).Cast<bool>().ToArray();
            }
        }



        public PIC16(int interval, ILogger<Einlesen> logger, List<String> _programm, Programm programm)
		{
			_logger = logger;
			_logger.LogWarning("Ausgabe Programmspeicher");
            
			_programmspeicher = new Programmspeicher(4096, _programm);
			foreach (var opcode in _programmspeicher._speicher)
			{
				_logger.LogWarning(opcode.ToString());
			}
            _datenspeicher = new Datenspeicher(256,this);
            _stack = new CircularBuffer<int>(7);
            _cyclecounter = 0;
			KonfiguriereTimer(interval);
			this._programm = programm;
            //Defaut Werte für OptionRegister setzen

            _scaler = Convert.ToInt32(Math.Pow(2,_datenspeicher.At(129, true).Read() & 7))*2;
            nopcycle = false;

            //throw new NotImplementedException();
        }

		private void KonfiguriereTimer(int interval)
        {
            _taktgeber = new System.Timers.Timer(interval);
            _taktgeber.Elapsed += OnTakt;
        }

        private void OnTakt(Object source, ElapsedEventArgs e)
        {
            try
            {

                    //TODO checkInterrupt();
                    checkInterrupt();
                    //Steps Timer0 if configured
                    TimerStep();  
                    if (!nopcycle)
                    {
                        //Fetch
                        int befehl = _programmspeicher.Read(_programmcounter);
                        Console.Write(befehl);
                        //Decode
                        var decoded = Decode(befehl);
                        Console.WriteLine("Folgender Befehl wurde erkannt {0}", decoded);
                        //Execute
                        execute(decoded, befehl);
                        _logger.LogWarning("Programmzähler: {0}", _programmcounter.ToString());
                        _programmcounter++;
                    }
                    else
                    {
                        nopcycle = false;
                        execute(Befehlsliste.Befehle.NOP,0);
                    }

                    this._programm.RefreshUI();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                _programmcounter++;
            }
        }

        private void TimerStep()
        {
            //TODO 
            //check T0CS to check Clock Source
            if (!_datenspeicher.At(129,true).ReadBit(5))
            {
                //Check if Prescaler is attached to T0SE ()
                if (!_datenspeicher.At(129,true).ReadBit(3))
                {
                    _scaler--;
                    if (_scaler == 0)
                    {
                        //Reset Scaler
                        ResetScaler();
                        IncreaseTimer();
                    }
                }
                else
                {
                    IncreaseTimer();
                }
            }
            else
            {
                //Check for Edge
                if (_datenspeicher.At(5,true).ReadBit(4) ^ _RA4_timerWertAlt)
                {
                    //check for rising or falling edge
                    if (_RA4_timerWertAlt ==_datenspeicher.At(129,true).ReadBit(4))
                    {
                        IncreaseTimer();
                    }
                }
            }
        }

        public void ResetScaler()
        {
            _scaler = Convert.ToInt32(Math.Pow(2, _datenspeicher.At(129, true).Read() & 7)) * 2;
        }

        private void IncreaseTimer()
        {
                int value = _datenspeicher.At(1, true).Read();
                value++;
                if (value > 255)
                {
                    value &= 255;
                    _datenspeicher.At(0x0b).WriteBit(2, true);
                }

                _datenspeicher.At(1, true).Write(value);
        }


        private Befehlsliste.Befehle Decode(int Befehl)
        {
            int befehlteil1 = (Befehl & (int) Befehlsmaske.MASKE2) / 256;
            int befehlsteil2 = (Befehl & (int) Befehlsmaske.MASKE3) / 16;
            _logger.LogWarning(Befehl.ToString());
            _logger.LogWarning(Convert.ToString(Befehl,2));
            if (Befehl == 0)
            {
                return Befehlsliste.Befehle.NOP;
            }
            if (Befehl is < 4096 and > 0)
            { // 0x00
                if (Befehl == 8)
                {
                    return Befehlsliste.Befehle.RETURN;
                }

                if (Befehl == 9)
                {
                    return Befehlsliste.Befehle.RETFIE;
                }
                switch (befehlteil1)
                {
                    case 0:
                        return Befehlsliste.Befehle.MOVWF;
                    case 1:
                        if (befehlsteil2 > 7)
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
                if (befehlteil1 is < 4 and >= 0) return Befehlsliste.Befehle.BCF;
                if (befehlteil1 is < 8 and > 3) return Befehlsliste.Befehle.BSF;
                if (befehlteil1 is < 12 and > 7) return Befehlsliste.Befehle.BTFSC;
                if (befehlteil1 is < 16 and > 11) return Befehlsliste.Befehle.BTFSS;

                throw new FormatException("Befehl nicht erkannt");

            }
            else if (Befehl is < 12288 and > 8191) //0x10
            {
                if (befehlteil1 is < 16 and > 7)
                {
                    return Befehlsliste.Befehle.GOTO;
                }

                if (befehlteil1 is < 8)
                {
                    return Befehlsliste.Befehle.CALL;
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
                else if (befehlteil1 is < 7 and > 3)
                {
                    return Befehlsliste.Befehle.RETLW;
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
    
        private void execute(Befehlsliste.Befehle decoded, int value)
        {
            switch (decoded)
            {
                    case Befehlsliste.Befehle.ADDWF :
                        addwf(value);
                    break;
                    case Befehlsliste.Befehle.ANDWF :
                        andwf(value);
                        break;
                    case Befehlsliste.Befehle.CLRF  :
                        clrf(value);
                    break;
                    case Befehlsliste.Befehle.CLRW  :
                        clrw();
                    break;
                    case Befehlsliste.Befehle.COMF  :
                        comf(value);
                    break;
                    case Befehlsliste.Befehle.DECF  :
                        decf(value);
                    break;
                    case Befehlsliste.Befehle.DECFSZ:
                        decfsz(value);
                        break;
                    case Befehlsliste.Befehle.INCF  :
                        incf(value);
                    break;
                    case Befehlsliste.Befehle.INCFSZ:
                        incfsz(value);
                    break;
                    case Befehlsliste.Befehle.IORWF :
                        iorwf(value);
                    break;
                    case Befehlsliste.Befehle.MOVF  :
                        movf(value);
                    break;
                    case Befehlsliste.Befehle.MOVWF :
                        movwf(value);
                    break;
                    case Befehlsliste.Befehle.NOP   :
                    break;
                    case Befehlsliste.Befehle.RLF   :
                        rlf(value);
                    break;
                    case Befehlsliste.Befehle.RRF   :
                        rrf(value);
                    break;
                    case Befehlsliste.Befehle.SUBWF :
                        subwf(value);
                    break;
                    case Befehlsliste.Befehle.SWAPF :
                        swapwf(value);
                    break;
                    case Befehlsliste.Befehle.XORWF :
                        xorwf(value);
                    break;
                    case Befehlsliste.Befehle.BCF   :
                        bcf(value);
                    break;
                    case Befehlsliste.Befehle.BSF   :
                        bsf(value);
                    break;
                    case Befehlsliste.Befehle.BTFSC :
                        btfsc(value);
                    break;
                    case Befehlsliste.Befehle.BTFSS :
                        btfss(value);
                    break;
                    case Befehlsliste.Befehle.ADDLW :
                        addlw(value);
                    break;
                    case Befehlsliste.Befehle.ANDLW :
                        andlw(value);
                    break;
                    case Befehlsliste.Befehle.CALL  :
                        call(value);
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
                        retlw(value);
                    break;
                    case Befehlsliste.Befehle.RETURN:
                        _return();
                        break;
                    case Befehlsliste.Befehle.RETFIE:
                        retfie();
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
            _logger.LogWarning("Inhalt W Register: {0}",_wregister);
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
            _taktgeber.Interval = interval;
        }

        public void checkInterrupt()
        {
            //Check if already in Interrupt or Interrupts ar masked if yes cancel interrupt checks
            if (!_datenspeicher.At(11,true).ReadBit(7))
            {
                return;
            }
            //Check wich Interrupts are enabled and Check if Interrupt Flags are raised
                //If Enabled and flag set call Interrupt Service Routine at Adress 4;
                var intcon = new BitArray(new int[] {_datenspeicher.At(11,true).Read()});

                if (intcon[3] && intcon[0])
                {
                    enterInterrupt();
                }
                //INTF Interrupt Bit wird automatisch zurückgesetzt laut Datenblatt
                else if (intcon[4] && intcon[1])
                {
                    _datenspeicher.At(11,true).WriteBit(1,false);
                    enterInterrupt();

                }
                //TImer Interrupt
                else if (intcon[5] && intcon[2])
                {
                    enterInterrupt();
                }
                else
                {
                    return;
                }

        }

        private void enterInterrupt()
        {
            _datenspeicher.At(11,true).WriteBit(7, false);
            call(4);
        }

        public void retfie()
        {
            _datenspeicher.At(11).WriteBit(7, true);
            _return();
        }
        public bool checkZero(int value)
        {
            if (value == 0)
            {
                _datenspeicher.At(3).WriteBit(2, true);
                return true;
            }
            else
            {
                _datenspeicher.At(3).WriteBit(2,false);
                return false;
            }
        }
        
        public void checkZeroNeg(int value)
        {
            if (value == 0)
            {
                _datenspeicher.At(3).WriteBit(2, false);
            }
            else
            {
                _datenspeicher.At(3).WriteBit(2,true);
            }
        }
        private bool checkCarry(int value)
        {
            if (value > 255)
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
            _wregister = (byte) befehl;
        }

        private void andlw(int befehl)
        {
            _wregister = _wregister & (befehl & 255);
            checkZero(_wregister);
        }

        private void andwf(int befehl)
        {
            int addr = befehl & 127;
            int value = _wregister & _datenspeicher.At(addr).Read();
            checkZero(value);
            bool destinationbit = Convert.ToBoolean(value & 128);

            if (destinationbit) _datenspeicher.At(addr).Write(value);
            else _wregister = value;




        }

        private void iorlw(int befehl)
        {
            _wregister = _wregister | (befehl & 255);
            checkZero(_wregister);
        }

        private void sublw(int befehl)
        {
            int payload = (befehl & 255);
            _wregister = ~_wregister;
            _wregister += 1;
            _wregister += payload;
           
            
            //Fehler im PIC, Carry wird falsch gesetzt. Carryflag müsste invertiert werden. Wenn Ergebnis von Subtraktion < 0 dann muss Carry gelöscht werden. Wenn Ergebnis > 0 muss Carry gesetzt werden
            if (checkCarry(_wregister))
            {
                 _datenspeicher.At(3).WriteBit(0,false);
            }
            else
            {
                _datenspeicher.At(3).WriteBit(0,true);
                _logger.LogWarning("Carry Set");
            }
            // Setze übrige Bits aus Integer auf 0;
            _wregister &= 255;

            if (checkDC(_wregister,payload))
            {
                _datenspeicher.At(3).WriteBit(1,false);
            }
            else
            {
                _datenspeicher.At(3).WriteBit(1,true);
                _logger.LogWarning("DigitCarry Set");

            }

        }



        private void xorlw(int befehl)
        {
            int payload = befehl & 255;
            _wregister = payload ^ _wregister;
            checkZero(_wregister);
        }

        private void addlw(int befehl)
        {
            int payload = befehl & 255;
            _wregister += payload;
            if (checkCarry(_wregister))
            {
                _datenspeicher.At(3).WriteBit(0,true);
            }
            else
            {
                _datenspeicher.At(3).WriteBit(0,false);
            }

            if (checkDC(payload, befehl))
            {
                _datenspeicher.At(3).WriteBit(1, true);
            }
            else
            {
                _datenspeicher.At(3).WriteBit(1,false);
            }
            checkZero(_wregister);
            _wregister &= 255;
        }
        
        private void _goto(int Befehl)
        {
            int payload = Befehl & 2047;
            _programmcounter = _datenspeicher.At(10).Read();
            _programmcounter <<= 11;
            _programmcounter += payload;
            //Programmzähler wird wieder um eins erhöht dann steht die richtige Adresse drinnen.
            _logger.LogWarning("Programm Counter {0}", payload);
            nopcycle = true;
        }

        private void call(int Befehl)
        {
            _stack.PushFront(_programmcounter);
            _logger.LogInformation("Stack Inhalt nach Call {0} vorne und {1} hinten", _stack.Front(), _stack.Back());
            _goto(Befehl);
        }
        
        private void _return()
        {
            _programmcounter = _stack.Back();
            _stack.PopBack();
            _logger.LogInformation("Stack Inhalt nach Return {0} vorne und {1} hinten", _stack.Front(), _stack.Back());
            nopcycle = true;
        }

        private void retlw(int Befehl)
        {
            _wregister = Befehl & 255;
            _return();
        }

        private void movwf(int Befehl)
        {
            int addr = Befehl & 127;
            //If Timer set reset prescaler
            if (addr == 1) ResetScaler();
            _datenspeicher.At(addr).Write(_wregister);
        }
        
        private void addwf(int Befehl)
        {
            int addr = Befehl & 127;
            int value = _wregister + _datenspeicher.At(addr).Read();
            
            if (checkCarry(value))
            {
                _datenspeicher.At(3).WriteBit(0,true);
            }
            else
            {
                _datenspeicher.At(3).WriteBit(0,false);
            }

            if (checkDC(value, _wregister))
            {
                _datenspeicher.At(3).WriteBit(1,true);
            }
            else
            {
                _datenspeicher.At(3).WriteBit(1,false);
            }
            
            bool destinationbit = Convert.ToBoolean(Befehl & 128);
            value &= 255;
            if (destinationbit)
            {
               _datenspeicher.At(addr).Write(value);
            }
            else
            { 
                _wregister = value;
            } 
        }

        private void clrf(int Befehl)
        {
            int addr = Befehl & 127;
            _datenspeicher.At(addr).Write(0);
            checkZero(_datenspeicher.At(addr).Read());
        }

        private void comf(int Befehl)
        {
            int addr = Befehl & 127;
            int value = _datenspeicher.At(addr).Read();
            value = ~value;
            value = value & 255;
            bool destinationbit = Convert.ToBoolean(Befehl & 128);

            if (destinationbit)
            {
                _datenspeicher.At(addr).Write(value);
            }
            else _wregister = value;
            checkZero(value);
        }

        private int decf(int Befehl)
        {
            int addr = Befehl & 127;
            int value = _datenspeicher.At(addr).Read() - 1;
            value &= 255;
            checkZero(value);
            bool destinationbit = Convert.ToBoolean(Befehl & 128);
            if (destinationbit)
            {
                _datenspeicher.At(addr).Write(value);
            }
            else
            {
                _wregister = value;
            }

            return value;
        }
        
        public void decfsz(int Befehl)
        {
            int value = decf(Befehl);
            if (value == 0)  
            {
                _programmcounter++;
                nopcycle = true;
            }
        }

        private int incf(int Befehl)
        {
            int addr = Befehl & 127;
            int value = _datenspeicher.At(addr).Read() + 1;
            value &= 255;
            checkZero(value);
            bool destinationbit = Convert.ToBoolean(Befehl & 128);
            if (destinationbit)
            {
                _datenspeicher.At(addr).Write(value);
            }
            else
            {
                _wregister = value;
            }

            return value;
        }

        public void incfsz(int Befehl)
        {
            int value = incf(Befehl);
            if (value == 0)
            {
                _programmcounter++;
                nopcycle = true;
            }
        }

        public void bsf(int Befehl)
        {
            int addr = Befehl & 127;
            int bitnumber = Befehl & 896;
            bitnumber >>= 7;
            Console.WriteLine("Write Bitnumber {0} at addr {1}",bitnumber,addr);
            _datenspeicher.At(addr).WriteBit(bitnumber, true);
        }

        public void bcf(int Befehl)
        {
            int addr = Befehl & 127;
            int bitnumber = Befehl & 896;
            bitnumber >>= 7;
            _datenspeicher.At(addr).WriteBit(bitnumber,false);
        }

        public void btfsc(int Befehl)
        {
            int addr = Befehl & 127;
            int bitnumber = Befehl & 896;
            bitnumber >>= 7;
            if (!Convert.ToBoolean(_datenspeicher.At(addr).ReadBit(bitnumber)))            {
                _programmcounter++;
                nopcycle = true;
            }
        }

        public void btfss(int Befehl)
        {
            int addr = Befehl & 127;
            int bitnumber = Befehl & 896;
            bitnumber >>= 7;
            if (Convert.ToBoolean(_datenspeicher.At(addr).ReadBit(bitnumber)))
            {
                _programmcounter++;
                nopcycle = true;
            }

        }
        
        private void movf(int Befehl)
        {
            int addr = Befehl & 127;
            int value = _datenspeicher.At(addr).Read();
            checkZero(value);
            bool destinationbit = Convert.ToBoolean(Befehl & 128);
            if (destinationbit)
            {
                _datenspeicher.At(addr).Write(value);
            }
            else _wregister = value;
        }

        private void iorwf(int Befehl)
        {
            int addr = Befehl & 127;
            int value = _datenspeicher.At(addr).Read() | _wregister;
            checkZeroNeg(value);
            bool destinationbit = Convert.ToBoolean(Befehl & 128);
            if (destinationbit)
            {
                _datenspeicher.At(addr).Write(value);
            }
            else _wregister = value;
        }

        private void subwf(int Befehl)
        {
            //TODO Implementieren
            int addr = Befehl & 127;
            //Value - W Register
            int value = _datenspeicher.At(addr).Read();

            int subtrahend = ~_wregister;
            subtrahend += 1;
            value += subtrahend;
            
            //Fehler im PIC, Carry wird falsch gesetzt. Carryflag müsste invertiert werden. Wenn Ergebnis von Subtraktion < 0 dann muss Carry gelöscht werden. Wenn Ergebnis > 0 muss Carry gesetzt werden
            if (checkCarry(value))
            {
                _datenspeicher.At(3).WriteBit(0,false);
            }
            else
            {
                _datenspeicher.At(3).WriteBit(0,true);
                _logger.LogWarning("Carry Set");
            }
            // Setze übrige Bits aus Integer auf 0;
            value &= 255;

            if (checkDC(value,_datenspeicher.At(addr).Read()))
            {
                _datenspeicher.At(3).WriteBit(1,false);
            }
            else
            {
                _datenspeicher.At(3).WriteBit(1,true);
                _logger.LogWarning("DigitCarry Set");

            }

            bool destinationbit = Convert.ToBoolean(Befehl & 128);
            if (destinationbit) _datenspeicher.At(addr).Write(value);
            else _wregister = value;

        }

        private void swapwf(int Befehl)
        {
            int addr = Befehl & 127;
            int teil1 = _datenspeicher.At(addr).Read() & 15;
            int teil2 = _datenspeicher.At(addr).Read() & 240;

            teil1 <<= 4;
            teil2 >>= 4;

            int value = teil1 + teil2;

            bool destinationbit = Convert.ToBoolean(Befehl & 128);
            if (destinationbit) _datenspeicher.At(addr).Write(value);
            else _wregister = value;
        }

        private void xorwf(int Befehl)
        {
            int addr = Befehl & 127;
            
            int value = _datenspeicher.At(addr).Read() ^ _wregister;
            checkZero(value);
            bool destinationbit = Convert.ToBoolean(Befehl & 128);
            if (destinationbit) _datenspeicher.At(addr).Write(value);
            else _wregister = value;
        }

        private void clrw()
        {
            _wregister = 0;
            checkZero(_wregister);
        }

        private void rlf(int Befehl)
        {
            int addr = Befehl & 127;
            int value = _datenspeicher.At(addr).Read();

            value <<= 1;
            if (Convert.ToBoolean(_datenspeicher.At(3).Read() & 1)) value++;
            if (checkCarry(value)) _datenspeicher.At(3).WriteBit(0,true);
            else _datenspeicher.At(3).WriteBit(0,false);

            value &= 255;

            bool destinationbit = Convert.ToBoolean(Befehl & 128);
            if (destinationbit) _datenspeicher.At(addr).Write(value);
            else _wregister = value;

        }

        private void rrf(int Befehl)
        {
            int addr = Befehl & 127;
            //Lese aktuellen Wert aus F Register
            int value = _datenspeicher.At(addr).Read();
            //Check if LSB bit is set to actualize Carry if needed
            bool lsb_set = Convert.ToBoolean(value & 1);
            //Shift Value to right
            value >>= 1;
            //If Carry Bit was already set before add +128 to set MSB Bit
            if (Convert.ToBoolean(_datenspeicher.At(3).Read())) value += 128;
            //Set new Carry Bit with info from lsb set
            _datenspeicher.At(3).WriteBit(0,lsb_set);

            value &= 255;

            bool destinationbit = Convert.ToBoolean(Befehl & 128);
            if (destinationbit) _datenspeicher.At(addr).Write(value);
            else _wregister = value;
        }


    }
}
