namespace pic__simulator__lehmann.Class.Parser;

public class Parser
{

    private string _programmpfad;

    public Parser(string programmpfad)
    {
        _programmpfad = programmpfad;
    }

    public Programmspeicher OpenProgramm()
    {
        Programmspeicher newProgramm = new Programmspeicher(65536);
        foreach (string line in System.IO.File.ReadLines(_programmpfad))
        {
            System.Console.WriteLine(line);
            //TODO Tokenizing der Befehle implementieren.
            line.Split(" ");
            
        }

        return newProgramm;
    }
    
    
}