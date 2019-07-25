namespace LibMediator.Command
{
    public class CommandResult
    {
        public bool Succeed => true;

        public static CommandResult Ok = new CommandResult();
    }
}
