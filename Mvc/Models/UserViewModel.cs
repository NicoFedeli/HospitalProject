namespace Hospital.Models
{
    // ViewModel per l'utente, usato in login e signup
    public class UserViewModel
    {
        public int Id { get; set; }                // Id utente (futuro: db identity)
        public string Username { get; set; }       // Nome utente
        public string Email { get; set; }          // Email (opzionale, utile più avanti)
        public string Role { get; set; }           // Ruolo (es: "doctor", "patient", "admin")
        public string Token { get; set; }          // JWT o altro token di sessione
        public DateTime? TokenExpiration { get; set; } // Scadenza del token, se usi JWT

    }
}
