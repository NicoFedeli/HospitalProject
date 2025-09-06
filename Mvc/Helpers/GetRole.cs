namespace Hospital.Helpers
{
    public static class GetRole
    {
        public static string GetUserTitle(string role, bool isAdmin)
        {
            return (role, isAdmin) switch
            {
                ("Doctor", true) => "Primary Doctor",
                ("Doctor", false) => "Doctor",
                ("Nurse", true) => "Nursing Coordinator",
                ("Nurse", false) => "Nurse",
                ("Patient", false) => "Patient",
                _ => role // fallback
            };
        }
    }
}