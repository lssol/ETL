namespace Amaris.ETL.SQL
{
    public class SQLExtractorOption
    {
        public string ConnectionString { get; set; }
        // The stored procedure must have in this order a batchSize and an offset argument
        public string StoredProcedureName { get; set; }
        public int BatchSize { get; set; } = 100;
        public int QueryPeriod { get; set; } = 100;
        public int InitialOffset { get; set; } = 0;
        public int Limit { get; set; } = -1;
    }
}