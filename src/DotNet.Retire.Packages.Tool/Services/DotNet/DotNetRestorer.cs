namespace DotNet.Retire.Packages.Tool.Services.DotNet
{
    public class DotNetRestorer
    {
        private readonly DotNetRunner _dotNetRunner;
        private readonly IFileService _fileSystem;

        public DotNetRestorer(DotNetRunner dotNetRunner, IFileService fileSystem)
        {
            _dotNetRunner = dotNetRunner;
            _fileSystem = fileSystem;
        }

        public RunStatus Restore()
        {
            return _dotNetRunner.Run(_fileSystem.GetCurrentDirectory(), new[] {"restore"});
        }
    }
}
