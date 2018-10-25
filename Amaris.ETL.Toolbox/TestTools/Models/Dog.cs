namespace Amaris.ETL.Toolbox.TestTools.Models
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