namespace HospitalAPI.Models
{
    public class RecordResponse
    {
        public string Status { get; set; }
        public List<Record> Data { get; set; }
    }
}