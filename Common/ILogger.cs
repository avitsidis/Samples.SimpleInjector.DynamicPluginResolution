namespace Common
{
    public interface ILogger
    {
        void Debug(string s);
        void Info(string s);
        void Error(string s);
        void Warning(string s);
    }
}
