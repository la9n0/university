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
            int sum = 0;
            foreach (Student student in Students)
            {
                sum += student.TuitionFees;
            }
            return sum;
        }
    }
    
    public class Student
    {
        public string StudentName;
        public int Age;
        public string Speciality;
        public string DateOfBirth;
        public string Course;
        public string Group;
        public float Gpa;
        public char Gender;
        public Address Address;
        public StudWork StudWork;
        public int TuitionFees;

        public Student(string studentName, int age, string speciality,
            string dateOfBirth, string course, string group, float gpa, char gender,
            string street, string city, int postalCode, int houseNumber, int apartmentNumber, 
            bool studHaveWork, string companyName="", string jobTitle="", string workExperience="")
        {
            this.StudentName = studentName;
            this.Age = age;
            this.Speciality = speciality;
            this.DateOfBirth = dateOfBirth;
            this.Course = course;
            this.Group = group;
            this.Gpa = gpa;
            this.Gender = gender;
            Address = new Address(street, city, postalCode, houseNumber, apartmentNumber);
            PriceOfPayment();
            if (studHaveWork)
            {
                HaveAWork(companyName, jobTitle, workExperience);
            }
        }

        public void HaveAWork(string companyName, string jobTitle, string workExperience)
        {
            StudWork=new StudWork(companyName, jobTitle, workExperience);
        }

        private void PriceOfPayment()
        {
            if (this.Gpa < 6)
                this.TuitionFees += 1500;
            else if (this.Gpa >= 6 && this.Gpa <8.5)
                this.TuitionFees += 1000;
            else if (this.Gpa >= 8.5)
                this.TuitionFees += 750;
        }
    }

    public class StudWork
    {
        public string CompanyName;
        public string JobTitle;
        public string WorkExperience;

        public StudWork(string companyName, string jobTitle, string workExperience)
        {
            this.CompanyName = companyName;
            this.JobTitle = jobTitle;
            this.WorkExperience = workExperience;
        }
    }
    public class Address
    {
        public string Street;
        public string City;
        public int PostalCode;
        public int HouseNumber;
        public int ApartmentNumber;

        public Address(string street, string city, int postalCode, int houseNumber, int apartmentNumber)
        {
            this.Street = street;
            this.City = city;
            this.PostalCode = postalCode;
            this.HouseNumber = houseNumber;
            this.ApartmentNumber = apartmentNumber;
        }
    }
}