using System.Text.Json;
using System.ComponentModel.DataAnnotations;


namespace lab02
{
    public class University
    {
        public List<Student> Students = new List<Student>();

        public void AddStudent(Student student)
        {
            Students.Add(student);
        }

        public int UniversityBudget()
        {
            return Students.Sum(s => s.TuitionFees);
        }

        public void SaveJSON()
        {
            string path = "D:/university/4_sem/oop/3_lab/university.json";
            string json = JsonSerializer.Serialize(Students);
            File.WriteAllText(path, json);
        }

        public void LoadJSON()
        {
            string path = "D:/university/4_sem/oop/3_lab/university.json";

            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);

            var restored = JsonSerializer.Deserialize<List<Student>>(json);

            if (restored != null)
                Students = restored;
        }
    }


    public class Student
    {
        public string StudentName { get; set; }
        public string Group { get; set; }

        public int Age { get; set; }
        public string Speciality { get; set; }
        public string DateOfBirth { get; set; }
        public string Course { get; set; }
        public float Gpa { get; set; }
        public char Gender { get; set; }
        public Address Address { get; set; }
        public StudWork StudWork { get; set; }
        public int TuitionFees { get; set; }

        public void PriceOfPayment()
        {
            if (this.Gpa < 6)
                this.TuitionFees += 1500;
            else if (this.Gpa < 8.5)
                this.TuitionFees += 1000;
            else
                this.TuitionFees += 750;
        }
    }

    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }

        public int PostalCode { get; set; }
        public int HouseNumber { get; set; }
        public int ApartmentNumber { get; set; }
    }

    public class StudWork
    {
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string WorkExperience { get; set; }
    }

    public class SearchInput
    {
        public string Type { get; set; } // "ФИО" или "Группа"

        [Required(ErrorMessage = "Поле поиска не может быть пустым")]
        [SearchValueValidation]
        public string Value { get; set; }
    }
    
    public class SearchValueValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance as SearchInput;

            string input = value.ToString();
            
            if (instance.Type == "ФИО")
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^[A-Za-zА-Яа-я\s]+$"))
                    return new ValidationResult("ФИО должно содержать только буквы");
            }
            
            if (instance.Type == "Группа")
            {
                if (!int.TryParse(input, out int group) || group < 1 || group > 10)
                    return new ValidationResult("Группа должна быть числом от 1 до 10");
            }
            return ValidationResult.Success;
        }
    }
    
}