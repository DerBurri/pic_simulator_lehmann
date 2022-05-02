
using Microsoft.AspNetCore.Components;

namespace pic__simulator__lehmann.Models;
public class Programm
{
    private readonly string _name;
    private readonly PIC16 _controller;
    public readonly List<String> _programm;

    public readonly ILogger<Programm> _logger;
    
    public Programm(int interval, ILogger<Programm> logger)
    {
        _logger = logger;
        _controller = new PIC16(interval);
        int i = 0;
        _programm = new List<String>();
        FileStream fs = new FileStream("geladenesProgramm", FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        String line;
        while ((line = sr.ReadLine()) != null)
        {
           logger.LogInformation(line);
            _programm.Add(line);
        }
        fs.Close();
    }
}