
using System.Text;

namespace pic__simulator__lehmann.Models;

public class Programm
{
    private PIC16 _controller;
    public  List<String> _programm;
    public readonly ILogger<Programm> _logger;

    public List<int> _SelectedBreakpoints { get; set; }  
    
    public Programm(int interval, ILogger<Programm> logger)
    {
        _logger = logger;
        _SelectedBreakpoints = new List<int>();
        Einlesen();
        _controller = new PIC16(interval, logger,_programm);
    }

    private void Einlesen()
    {
        _programm = new List<String>();
        FileStream fs = new FileStream("geladenesProgramm", FileMode.Open);
        StreamReader sr = new StreamReader(fs, Encoding.Latin1);
        String line;
        while ((line = sr.ReadLine()) != null)
        {
            _logger.LogInformation(line);
            _programm.Add(line);
        }
        fs.Close();
    }

    public void Start()
    {
        _controller.Start();
    }

    public void Stop()
    {
        _controller.Stop();
    }

    public void Reset()
    {
        _controller.Reset();
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

    public int GetSFR()
    {
        throw new NotImplementedException();
    }
}