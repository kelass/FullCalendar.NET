namespace Calendar.Models.DbModels
{
    public class Employee
    {
        public string Id { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfEmployment { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
        public CalendarUser? User { get; set; }
        public string DepartmentId { get; set; }
        public string BossId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ImageModelId { get; set; }
        //IsConsultant
        //EmploymentStatus

    }
}
