namespace pic__simulator__lehmann.Models;

public abstract class Register
{
    public virtual int Read() { return 0; }

    public virtual void Write(int value) { }


}