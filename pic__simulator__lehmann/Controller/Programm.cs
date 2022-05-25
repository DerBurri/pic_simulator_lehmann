using pic__simulator__lehmann.Pages;
using System.Text;

namespace pic__simulator__lehmann.Models;

public class Programm
{
    private PIC16 _controller;
    public  List<String> _programm;
    public List<String> _programmzeilen;
    public readonly ILogger<Einlesen> _logger;

    public List<int> _SelectedBreakpoints { get; set; }  
    
    public Programm(int interval, ILogger<Einlesen> logger)
    {
        _logger = logger;
        _SelectedBreakpoints = new List<int>();
        Einlesen();
        _controller = new PIC16(interval, logger,_programm);
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
                _logger.LogCritical("Voller Teil");
                _programmzeilen.Add(line2.Substring(0, 4));
            }
            else
            {
                _logger.LogCritical("Leerer Teil");
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

    public void Stop()
    {
        _controller.Stop();
    }
    
    public void Step()
    {
        _controller.Step();
    }

    public void IntervalChange(int interval)
    {
        _controller.IntervalChange(interval);
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

    public int GetWRegister()
    {
        return _controller.W_register;
    }

    public bool[] getStatusRegister()
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