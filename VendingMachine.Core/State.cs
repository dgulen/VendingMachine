namespace VendingMachine.Core
{
    public enum State : int
    {
        UNKNOWN = 0,
        ALLOW_RUNNING,
        ASKING_USER,
        USER_CANCELLED
    }
}
