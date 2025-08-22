namespace Hospital.Models
{
    public class ApiResponse<T>
    {
        public string Status { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
/*
 * Esempio Login o errore: nessun dato passato
 var response = new ApiResponse<List<DoctorViewModel>>
{
    Status = "OK",
    Data = doctorsList
};

* Esempio Dottori: lista di dottori passata
var response = new ApiResponse<List<DoctorViewModel>>
{
    Status = "OK",
    Data = doctorsList
};


* Esempio Infermiere: lista di infermieri passata
var response = new ApiResponse<List<NurseViewModel>>
{
    Status = "OK",
    Data = nursesList
};

* Esempio Pazienti: lista di pazienti passata
var response = new ApiResponse<List<PatientViewModel>>
{
    Status = "OK",
    Data = patientsList
};
 
 */