using System.ComponentModel;

namespace pic__simulator__lehmann.Models;

public class Programm
{
    private readonly string _name;
    private readonly PIC16 _controller;
    public readonly SortedDictionary<int, String> _programm;
    
    public Programm(int interval)
    {
        _controller = new PIC16(interval);
        int i = 0;
        _programm = new SortedDictionary<int, string>();
        FileStream fs = new FileStream("geladenesProgramm", FileMode.Open);
        using (StreamReader sr = new StreamReader(fs))
        {
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                _programm[i] = line;
            }
            fs.Close();
        }

    }
}