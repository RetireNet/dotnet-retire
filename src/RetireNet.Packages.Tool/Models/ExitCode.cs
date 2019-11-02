namespace RetireNet.Packages.Tool.Models
{
    public static class ExitCode
    {
        public const int OK = 0x0;
        public const int UNEXPECTED_TERMINATION = 0x1;
        public const int FILE_NOT_FOUND = 0x2;
        public const int FOUND_VULNERABILITIES = 0x3;
        public const int DIRECTORY_NOT_FOUND = 0x4;
        public const int ASSETS_NOT_FOUND = 0x5;
    }
}
