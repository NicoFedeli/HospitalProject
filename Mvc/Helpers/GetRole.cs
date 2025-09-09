namespace Hospital.Helpers
{
    public static class GetRole
    {
        public static string GetUserTitle(string role)
        {
            return (role) switch
            {
                ("DoctorAdmin") => "Primary Doctor",
                ("NurseAdmin") => "Nursing Coordinator",
                //in teoria non necessari
                //("Doctor") => "Doctor",
                //("Nurse") => "Nurse",
                //("Patient") => "Patient",
                _ => role // fallback
            };
        }
    }
}