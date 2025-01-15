using HexaCraft;

public class PathGenerationCommand : IBasicCommand
{
    private PathGeneration _receiver;

    public PathGenerationCommand(PathGeneration receiver)
    {
        _receiver = receiver;
    }

    public void Execute()
    {
        _receiver.GeneratePath();
    }
}
