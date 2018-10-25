namespace Amaris.ETL.Test.Models
{
    public class Dog
    {
        public string Name { get; set; }
        public string Race { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Race}";
        }
    }
}