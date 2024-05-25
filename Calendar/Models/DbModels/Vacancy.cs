namespace Calendar.Models.DbModels
{
    public class Vacancy
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public string Comment { get; set; }
        public string VacancyCategoryId { get; set; }
        public VacancyCategory? VacancyCategory { get; set; }
        public bool Approved { get; set; }
        public DateTime DateApproved { get; set; }
        public string UserApprovedId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        //VacancyStatus
    }
}
