using PayslipManagement.Common.Models;
using System.ComponentModel.DataAnnotations;
using PaySlipManagement.UI.Utilities;
using PaySlipManagement.Common.Models;

namespace PaySlipManagement.UI.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Employee Code")]
        public string Emp_Code { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Full Name")]
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        [Display(Name = "Designation")]
        public string Designation { get; set; }
        public List<string>? Role { get; set; }  // To store the employee roles

        public String Division { get; set; }
        [Required]
        [EmailAddress]
        public String Email { get; set; }
        [Required(ErrorMessage = "PAN Number is required.")]
        [Display(Name = "PAN Number")]
        public string PAN_Number { get; set; }
        public string MaskedPanNumber => StringHelpers.MaskPanNumber(PAN_Number);
        [Display(Name = "Joining Date")]
        public DateTime? JoiningDate { get; set; }
        [Display(Name = "Department Name")]
        public string? DepartmentName { get; set; }
        // New property to track if the employee is active
        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true; // Default to true (active)
        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^\+?\d{10}$", ErrorMessage = "Enter a valid phone number.")]

        [Display(Name = "Phone Number")]
        public long PhoneNumber { get; set; }
        public string MaskedPhoneNumber => StringHelpers.MaskPhoneNumber(PhoneNumber);
        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^\+?\d{10}$", ErrorMessage = "Enter a valid phone number.")]
        [Display(Name = "Emergency Contact")]
        public long EmergencyContact { get; set; }
        [Display(Name = "Emergency Relation")]
        public string EmergencyRelation { get; set; }
        [Required(ErrorMessage = "Current Address is required.")]
        [Display(Name = "Current Address")]
        public string CurrentAddress { get; set; }
        [Required(ErrorMessage = "Permanennt Address is required.")]
        [Display(Name = "Permanent Address")]
        public string PermanentAddress { get; set; }
        [Required(ErrorMessage = "Mail is required.")]
        [Display(Name = "Personal Email")]
        public string PersonalEmail { get; set; }
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }
        [Required(ErrorMessage = "Aadhar Number is required.")]
        [RegularExpression(@"^\+?\d{12}$", ErrorMessage = "Enter a valid Aadhar number.")]
        [Display(Name = "Aadhar Number")]
        public long AadharNumber { get; set; }
        [Required(ErrorMessage = "Date of Birth is required.")]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }
        [Display(Name = "Date of Celebration")]
        public DateTime? DateOfCelebration { get; set; }
        [Display(Name = "Mother's Name")]
        public string MotherName { get; set; }
        [Display(Name = "Father's Name")]
        public string FatherName { get; set; }
        [Display(Name = "Spouse Name")]
        public string SpouseName { get; set; }
        [Display(Name = "Children's Name")]
        public string ChildrenNames { get; set; }
        [Display(Name = "LinkedIn Profile URL")]
        public string LinkedInProfile { get; set; }
        [Required(ErrorMessage = "Account Holder Name is required.")]
        [Display(Name = "Account Holder Name")]
        public string AccountHolderName { get; set; }
        [Required(ErrorMessage = "Bank Name is required.")]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Required(ErrorMessage = "Bank Account Number is required.")]
        [Display(Name = "Bank Account Number")]
        public long BankAccountNumber { get; set; }
        [Required(ErrorMessage = "IFSC Code is required.")]
        [Display(Name = "IFSC Code")]
        public string IFSCCode { get; set; }
        [Display(Name = "Previous Company Name")]
        public string PreviousCompanyName { get; set; }
        public string Tenure { get; set; }
        [Display(Name = "Reporting Manager")]
        public string ReportingManager { get; set; }
        [Display(Name = "Latest Company Reference")]
        public string LatestCompanyReference { get; set; }

    }
    public class EmployeePayPeriodsViewModel
    {
        public EmployeeDetails Employee { get; set; }
        public List<string> PayPeriods { get; set; }
        public HolidayImagePDFViewModel Holiday {  get; set; }
        public LeavesViewModel Leaves { get; set; } = new LeavesViewModel();
        public IEnumerable<LeaveRequestsViewModel>? LeaveRequests { get; set; }
        //public IEnumerable<EmployeeTasksViewModel>? EmployeeTasks { get; set; }
        

    }
}
