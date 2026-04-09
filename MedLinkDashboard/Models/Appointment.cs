using DAL.Enums;
using DAL.Models;

namespace MedLinkDashboard.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; }

        public DateOnly Day { get; set; }
        public TimeOnly AppointmentStart { get; set; }
        public TimeOnly AppointmentEnd { get; set; }
        public AppointmentStatus Status { get; set; }
        public decimal? Price { get; set; }

    }
}
