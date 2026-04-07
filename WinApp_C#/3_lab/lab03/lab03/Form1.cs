using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab02
{
    public partial class Form1 : Form
    {
        public University university = new University();
        private Button newUser = new Button()
        {
            Text = "Добавить нового студента",
            Left = 45,
            Top = 30,
            Height = 30,
            Width = 200
        };
        
        private Label titleLabel = new Label();
        private TextBox fioTextBox = new TextBox();
        private NumericUpDown ageNumeric = new NumericUpDown();
        private ComboBox specialtyCombo = new ComboBox();
        private DateTimePicker birthDatePicker = new DateTimePicker();
        private ComboBox courseCombo = new ComboBox();
        private ComboBox groupCombo = new ComboBox();
        private NumericUpDown avgGrade = new NumericUpDown();
        private GroupBox genderGroup = new GroupBox();
        private RadioButton maleRadio = new RadioButton();
        private RadioButton femaleRadio = new RadioButton();

        private GroupBox addressGroup = new GroupBox();
        private TextBox cityTextBox = new TextBox();
        private MaskedTextBox indexMasked = new MaskedTextBox("000000");
        private TextBox streetTextBox = new TextBox();
        private TextBox houseTextBox = new TextBox();
        private NumericUpDown apartmentNumeric = new NumericUpDown();

        private Label workLabel = new Label();
        private CheckBox workCheckBox = new CheckBox();
        private GroupBox workGroupBox = new GroupBox();
        private TextBox companyTextBox = new TextBox();
        private TextBox positionTextBox = new TextBox();
        private NumericUpDown experienceNumeric = new NumericUpDown();

        private Button addButton = new Button()
        {
            Text="Добавить данные о студентах",
            Top=815,
            Left=330,
            Height = 30,
            Width = 230
        };

        private Button Budget = new Button()
        {
            Text = "Подсчитать бюджет университета",
            Top = 815,
            Left = 50,
            Height = 30,
            Width = 230
        };

        private Label budgetLabel = new Label();
        private Button toForm2;

        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(300, 150);
            newUser.Click += newUser_Click;
            Controls.Add(newUser);
            this.Text = "Добавление студентов";
        }

        private void newUser_Click(object sender, EventArgs e)
        {
            BuildForm();
        }

        public void BuildForm()
        {
            newUser.Hide();
            this.Size = new Size(600, 950);
            int top = 20;

            // Заголовок
            titleLabel.Text = "Введите данные о студенте:";
            titleLabel.Font = new Font("Arial", 12, FontStyle.Bold);
            titleLabel.Location = new Point(20, top);
            titleLabel.AutoSize = true;
            Controls.Add(titleLabel);
            top += 40;

            // ФИО
            Controls.Add(new Label() { Text = "ФИО:", Location = new Point(20, top), AutoSize = true });
            fioTextBox.SetBounds(200, top, 300, 25);
            Controls.Add(fioTextBox);
            top += 35;

            // Возраст
            Controls.Add(new Label() { Text = "Возраст:", Location = new Point(20, top), AutoSize = true });
            ageNumeric.SetBounds(200, top, 100, 25);
            ageNumeric.Minimum = 16;
            ageNumeric.Maximum = 100;
            Controls.Add(ageNumeric);
            top += 35;

            // Специальность
            Controls.Add(new Label() { Text = "Специальность:", Location = new Point(20, top), AutoSize = true });
            specialtyCombo.SetBounds(200, top, 200, 25);
            specialtyCombo.Items.AddRange(new string[] { "ПИ", "Дизайн", "ИСИС" });
            Controls.Add(specialtyCombo);
            top += 35;

            // Дата рождения
            Controls.Add(new Label() { Text = "Дата рождения:", Location = new Point(20, top), AutoSize = true });
            birthDatePicker.SetBounds(200, top, 200, 25);
            birthDatePicker.Format = DateTimePickerFormat.Short;
            Controls.Add(birthDatePicker);
            top += 35;

            // Курс
            Controls.Add(new Label() { Text = "Курс:", Location = new Point(20, top), AutoSize = true });
            courseCombo.SetBounds(200, top, 100, 25);
            courseCombo.Items.AddRange(new string[] { "1", "2", "3", "4" });
            Controls.Add(courseCombo);
            top += 35;

            // Группа
            Controls.Add(new Label() { Text = "Группа:", Location = new Point(20, top), AutoSize = true });
            groupCombo.SetBounds(200, top, 100, 25);
            for (int i = 1; i <= 10; i++)
                groupCombo.Items.Add(i.ToString());
            Controls.Add(groupCombo);
            top += 35;

            // GPA
            Controls.Add(new Label() { Text = "Средний балл:", Location = new Point(20, top), AutoSize = true });
            avgGrade.SetBounds(200, top, 100, 25);
            avgGrade.DecimalPlaces = 2;
            avgGrade.Minimum = 0;
            avgGrade.Maximum = 10;
            avgGrade.Increment = 0.1M;
            Controls.Add(avgGrade);
            top += 45;

            // Пол
            Controls.Add(new Label() { Text = "Пол:", Location = new Point(20, top), AutoSize = true });

            genderGroup.SetBounds(200, top - 5, 250, 50);

            maleRadio.Text = "М";
            maleRadio.Location = new Point(40, 20);

            femaleRadio.Text = "Ж";
            femaleRadio.Location = new Point(170, 20);

            genderGroup.Controls.Add(maleRadio);
            genderGroup.Controls.Add(femaleRadio);
            Controls.Add(genderGroup);
            top += 70;

            // Адрес
            addressGroup.Text = "Адрес";
            addressGroup.SetBounds(20, top, 520, 200);

            int addrTop = 25;

            addressGroup.Controls.Add(new Label()
                { Text = "Город:", Location = new Point(10, addrTop), AutoSize = true });
            cityTextBox.SetBounds(150, addrTop, 200, 25);
            addressGroup.Controls.Add(cityTextBox);

            addrTop += 30;
            addressGroup.Controls.Add(new Label()
                { Text = "Индекс:", Location = new Point(10, addrTop), AutoSize = true });
            indexMasked.SetBounds(150, addrTop, 100, 25);
            addressGroup.Controls.Add(indexMasked);

            addrTop += 30;
            addressGroup.Controls.Add(new Label()
                { Text = "Улица:", Location = new Point(10, addrTop), AutoSize = true });
            streetTextBox.SetBounds(150, addrTop, 200, 25);
            addressGroup.Controls.Add(streetTextBox);

            addrTop += 30;
            addressGroup.Controls.Add(new Label()
                { Text = "Дом:", Location = new Point(10, addrTop), AutoSize = true });
            houseTextBox.SetBounds(150, addrTop, 100, 25);
            addressGroup.Controls.Add(houseTextBox);

            addrTop += 30;
            addressGroup.Controls.Add(new Label()
                { Text = "Квартира:", Location = new Point(10, addrTop), AutoSize = true });
            apartmentNumeric.SetBounds(150, addrTop, 100, 25);
            apartmentNumeric.Minimum = 1;
            apartmentNumeric.Maximum = 10000;
            addressGroup.Controls.Add(apartmentNumeric);

            Controls.Add(addressGroup);
            top += 220;

            // Работа
            workLabel.Text = "Имеется ли работа:";
            workLabel.Location = new Point(20, top);
            workLabel.AutoSize = true;
            Controls.Add(workLabel);

            workCheckBox.Location = new Point(200, top);
            Controls.Add(workCheckBox);
            top += 40;

            workGroupBox.Text = "Информация о работе";
            workGroupBox.SetBounds(20, top, 520, 130);
            workGroupBox.Visible = false;

            workGroupBox.Controls.Add(new Label()
                { Text = "Компания:", Location = new Point(10, 25), AutoSize = true });
            companyTextBox.SetBounds(150, 22, 300, 25);
            workGroupBox.Controls.Add(companyTextBox);

            workGroupBox.Controls.Add(
                new Label() { Text = "Должность:", Location = new Point(10, 55), AutoSize = true });
            positionTextBox.SetBounds(150, 52, 300, 25);
            workGroupBox.Controls.Add(positionTextBox);

            workGroupBox.Controls.Add(new Label()
                { Text = "Стаж (лет):", Location = new Point(10, 85), AutoSize = true });
            experienceNumeric.SetBounds(150, 82, 100, 25);
            experienceNumeric.Minimum = 0;
            experienceNumeric.Maximum = 50;
            workGroupBox.Controls.Add(experienceNumeric);

            Controls.Add(workGroupBox);

            workCheckBox.CheckedChanged += (s, ev) => { workGroupBox.Visible = workCheckBox.Checked; };
            Controls.Add(addButton);
            addButton.Click += AddButton_click;

            Controls.Add(Budget);
            Budget.Click += Budget_click;

            budgetLabel.SetBounds(50, 780, 300, 30);
            Controls.Add(budgetLabel);

            toForm2 = new Button()
            {
                Text = "Информация о студентах",
                Top = 860,
                Left = 200,
                Height = 30,
                Width = 200
            };
            toForm2.Click += toForm2_click;
            Controls.Add(toForm2);
        }

        private void AddButton_click(object sender, EventArgs e)
        {
            bool valid = true;
            
            void ClearErrors(Control parent)
            {
                for (int i = parent.Controls.Count - 1; i >= 0; i--)
                {
                    if (parent.Controls[i] is Label lbl && lbl.ForeColor == Color.Red)
                        parent.Controls.RemoveAt(i);
                }

                foreach (Control c in parent.Controls)
                    ClearErrors(c);
            }

            ClearErrors(this);

            void ShowError(Control c)
            {
                Control parent = c.Parent;

                Label err = new Label();
                err.Text = "Заполните поле";
                err.ForeColor = Color.Red;
                err.AutoSize = true;

                int rightSpace = parent.Width - c.Right;

                if (rightSpace > 120)
                {
                    err.Left = c.Right + 10;
                    err.Top = c.Top + 3;
                }
                else
                {
                    err.Left = c.Left + 5;
                    err.Top = c.Bottom - 23;
                }

                parent.Controls.Add(err);
                err.BringToFront();

                valid = false;
            }

            if (string.IsNullOrWhiteSpace(fioTextBox.Text))
                ShowError(fioTextBox);

            if (specialtyCombo.SelectedItem == null)
                ShowError(specialtyCombo);

            if (courseCombo.SelectedItem == null)
                ShowError(courseCombo);

            if (groupCombo.SelectedItem == null)
                ShowError(groupCombo);

            if (!maleRadio.Checked && !femaleRadio.Checked)
                ShowError(genderGroup);

            if (string.IsNullOrWhiteSpace(cityTextBox.Text))
                ShowError(cityTextBox);

            if (string.IsNullOrWhiteSpace(indexMasked.Text.Trim()))
                ShowError(indexMasked);

            if (string.IsNullOrWhiteSpace(streetTextBox.Text))
                ShowError(streetTextBox);

            if (string.IsNullOrWhiteSpace(houseTextBox.Text))
                ShowError(houseTextBox);

            if (workCheckBox.Checked)
            {
                if (string.IsNullOrWhiteSpace(companyTextBox.Text))
                    ShowError(companyTextBox);

                if (string.IsNullOrWhiteSpace(positionTextBox.Text))
                    ShowError(positionTextBox);
            }

            if (!valid)
                return;
            
            Student newStudent = new Student
            {
                StudentName = fioTextBox.Text,
                Age = (int)ageNumeric.Value,
                Speciality = specialtyCombo.SelectedItem.ToString(),
                DateOfBirth = birthDatePicker.Value.ToShortDateString(),
                Course = courseCombo.SelectedItem.ToString(),
                Group = groupCombo.SelectedItem.ToString(),
                Gpa = (float)avgGrade.Value,
                Gender = maleRadio.Checked ? 'М' : 'Ж',
                Address = new Address
                {
                    Street = streetTextBox.Text,
                    City = cityTextBox.Text,
                    PostalCode = int.Parse(indexMasked.Text),
                    HouseNumber = int.Parse(houseTextBox.Text),
                    ApartmentNumber = (int)apartmentNumeric.Value
                },
                TuitionFees = 0
            };
            newStudent.PriceOfPayment();
            
            if (workCheckBox.Checked)
            {
                newStudent.StudWork = new StudWork
                {
                    CompanyName = companyTextBox.Text,
                    JobTitle = positionTextBox.Text,
                    WorkExperience = experienceNumeric.Value.ToString()
                };
            }
            university.AddStudent(newStudent);

            university.SaveJSON();
            MessageBox.Show("Студент добавлен");
        }

        private void Budget_click(object sender, EventArgs e)
        {
            budgetLabel.Text = $"Бюджет университета = {university.UniversityBudget()}";
        }

        private void toForm2_click(object sender, EventArgs e)
        {
            Form form2 = new Form2();
            form2.Show();
            this.Hide();
        }
    }
}