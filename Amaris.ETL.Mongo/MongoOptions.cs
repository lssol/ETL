namespace Amaris.ETL.Mongo
{
    public class MongoOptions
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
        public int Period { get; set; }
        public int BatchSize { get; set; }
    }
}