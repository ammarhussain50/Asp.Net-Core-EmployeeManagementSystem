using EMS_Backend.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS_Backend.Entity
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string JobTitle { get; set; }
        public int Gender { get; set; }
        [ForeignKey(nameof(Department))]
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public DateOnly JoiningDate { get; set; }
        public DateOnly LastWorkingDate { get; set; }
        public DateOnly DateOfBirth { get; set; }

        // foreign key
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }

 }

