using System.Collections;
using pic__simulator__lehmann.Pages;
using System.Text;

namespace pic__simulator__lehmann.Models;

public class Programm
{
    private PIC16 _controller;
    public  List<String> _programm;
    public List<String> _programmzeilen;
    public readonly ILogger<Einlesen> _logger;
    public Component _ui;

    public List<int> _SelectedBreakpoints;
    
    public Programm(double interval, ILogger<Einlesen> logger, Component ui)
	{
		_logger = logger;
		_SelectedBreakpoints = new List<int>();
		Einlesen();
		_controller = new PIC16(interval, logger, _programm, this);
		_ui = ui;
	}

	private void Einlesen()
    {
        _programm = new List<String>();
        _programmzeilen = new List<String>();
        
        FileStream fs = new FileStream("geladenesProgramm", FileMode.Open);
        StreamReader sr = new StreamReader(fs, Encoding.Latin1);
        String line;
        while ((line = sr.ReadLine()) != null)
        {
            _logger.LogInformation(line);
            _programm.Add(line);
        }

        foreach (String line2 in _programm)
        {
            if (Char.IsDigit(line2[0]))
            {
                _programmzeilen.Add(line2.Substring(0, 4));
            }
            else
            {
                _programmzeilen.Add(" ");
            }
        }
        fs.Close();

        Console.WriteLine("Programmzeilen");

        foreach (var zeile in _programmzeilen)
        {
            Console.WriteLine(zeile);
        }
    }

    public void Start()
    {
        _controller.Start();
    }

    public List<int> GetSelectedBreakpoints()
    {
        return _SelectedBreakpoints;
    }
    public void Stop()
    {
        _controller.Stop();
    }
    
    public void Step()
    {
        _controller.Step();
    }

    public void RefreshUI()
	{
        _ui.RefreshUI();
	}

    public void FrequencyChange(double interval)
    {
        _controller.frequency = interval;
    }

    public int GetCycleCounter()
    {
        return _controller.GetCycleCounter();
    }
    public int GetLst()
    {
        throw new NotImplementedException();
    }

    public int GetGPR()
    {
        throw new NotImplementedException();
    }

    public int GetFSR()
    {
        return _controller.GetFSR();
    }
    
    public int GetPCLath()
    {
        return _controller.GetPCLath();
    }

    public int GetStatus()
    {
        return _controller.GetStatusRegister();
    }

    public bool[] GetOptionRegisterFlags()
    {
        return _controller.OptionRegister;
    }

    public bool[] GetIntconRegisterFlags()
    {
        return _controller.IntConRegister;
    }

    public static BitArray GetConfigRegister()
    {
        return PIC16.configBits;
    }

    public double GetFrequency()
    {
        return _controller.frequency;
    }
    public int GetOptionRegister()
    {
        return _controller.GetOptionRegister();
    }

    public int GetIntconRegister()
    {
        return _controller.GetIntconRegister();
    }
    public int GetWRegister()
    {
        return _controller.W_register;
    }

    public int GetTimer0()
    {
        return _controller.Timer0;
    }

    public int GetPrescaler()
    {
        return _controller.Scaler;
    }

    public bool[] GetStatusRegister()
    {
        return _controller.StatusRegister;
    }

    public int GetPCIntern()
    {
        throw new NotImplementedException();
    }
    
    public int[] GetStack()
    {
        return _controller.GetStack();
    }

    public int GetRAMValueUI(int addr)
    {
        return _controller.GetRAMValueUI(addr);
    }

    public void SetRAMValueUI(int addr, int value)
    {
        _controller.SetRAMValueUI(addr, value);
    }

    public int GetPCL()
    {
        return _controller.GetPCL();
    }
}