namespace Hospital.Models
{
    public class ApiResponse<T>
    {
        public string Status { get; set; }      // "OK" o "KO"
        public string? Message { get; set; }    // opzionale, present in caso di errore
        public T? Data { get; set; }            // opzionale, presente in caso di successo. Può essere qualsiasi cosa (lista, oggetto, ecc.)
    }
}
