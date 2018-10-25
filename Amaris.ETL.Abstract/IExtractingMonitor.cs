namespace Amaris.ETL.Abstract
{
    public interface IExtractingMonitor
    {
        int Limit { get; set; }
        int Offset { get; set; }
    }
}