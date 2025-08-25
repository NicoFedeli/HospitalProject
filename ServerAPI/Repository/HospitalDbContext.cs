using HospitalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Repository
{
    public class HospitalDbContext : DbContext
    {
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<Nurse> nurses { get; set; }
        public DbSet<Patient> patients { get; set; }
        public DbSet<Bill> bills { get; set; }
        public DbSet<Appointment> appointments { get; set; }
        public DbSet<Record> records { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var connectionString = "Server=localhost;Database=Hospital;User Id=nico;Password=Admin123;TrustServerCertificate=True;"; // NICO
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=Hospital;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;"; // BERTA


            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}

//dotnet ef dbcontext scaffold "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Hospital" Microsoft.EntityFrameworkCore.SqlServer -o Models