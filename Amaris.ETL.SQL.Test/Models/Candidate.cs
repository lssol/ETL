namespace Amaris.ETL.SQL.Test.Models
{
    public class CandidateInput
    {
        public int CandidateId { get; set; }
        public int AdminFileId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
    }
    public class CandidateOutput
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return $"{Firstname} {Lastname} - {Email}";
        }
    }
}