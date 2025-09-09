namespace HospitalAPI.Models
{
    public class BillResponse
    {
        public string Status { get; set; }
        public List<Bill> Data { get; set; }
    }
}
