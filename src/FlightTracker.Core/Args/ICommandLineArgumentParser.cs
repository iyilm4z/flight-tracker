namespace FlightTracker.Args
{
    public interface ICommandLineArgumentParser
    {
        CommandLineArgs Parse(string[] args);
    }
}