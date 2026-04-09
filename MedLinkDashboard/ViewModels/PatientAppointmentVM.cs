using DAL.Enums;
using DAL.Models;
using MedLinkDashboard.Models;

namespace MedLinkDashboard.ViewModels
{
    public class PatientAppointmentVM
    {
        public int Id { get; set; }
        public string Patient { get; set; }
        public string Doctor { get; set; }
        public string Clinic { get; set; }
        public DateOnly Day { get; set; }
        public TimeOnly AppointmentStart { get; set; }
        public TimeOnly AppointmentEnd { get; set; }
        public AppointmentStatus Status { get; set; }
        public decimal? Price { get; set; }
    }
}
