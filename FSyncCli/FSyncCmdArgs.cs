namespace FSyncCli
{
    public class FSyncCmdArgs : IFSyncCmdArgs
    {
        public string[] Args { get; }

        private FSyncCmdArgs(string[] args)
        {
            Args = args;
        }

        public static FSyncCmdArgs CreateFSyncCmdArgs(string[] args)
        {
            return new FSyncCmdArgs(args);
        }

    }

    public interface IFSyncCmdArgs
    {
        string[] Args { get;}
    }
}