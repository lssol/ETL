using System;

namespace Amaris.ETL.DocBinariesToRnD.Models
{
    public class DocBinary
    {
        public int DocumentId { get; set; }
        public int CandidateId { get; set; }
        public string Title { get; set; }
        public byte[] Document { get; set; }
        public string Description { get; set; }
        public string LanguageId { get; set; }
        public string Extension { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}