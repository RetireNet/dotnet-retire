namespace RetireNet.Packages.Tool.Services
{
    public interface IExitCodeHandler
    {
        void HandleExitCode(int exitCode, bool exitImmediately = false);
    }
}
