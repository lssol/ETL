namespace Amaris.ETL.Mongo
{
    public class MongoLoaderOptions
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
        public int Period { get; set; } = 0;
        public int BatchSize { get; set; } = 1;
    }
}