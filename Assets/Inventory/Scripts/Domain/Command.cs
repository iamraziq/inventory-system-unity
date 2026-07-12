using System.Collections.Generic;

public interface ICommand{ void Execute(); void Undo(); }

public class MoveCommand : ICommand
{
  private readonly Inventory inv;
  private readonly int from, to;
  public MoveCommand(Inventory inv,int from, int to)
    {
        this.inv = inv;
        this.from = from;
        this.to = to;
    }

    public void Execute() => inv.MoveStack(from, to);
    public void Undo() => inv.MoveStack(to, from); // the command remembers how to reverse itself
}

public class CommandProcessor
{
    private readonly Stack<ICommand> history = new(); // LIFO = undo order
    public void Do(ICommand cmd)
    {
        cmd.Execute();
        history.Push(cmd);
    }

    public void UndoLast()
    {
        if(history.Count > 0)
            history.Pop().Undo();
    }
}
