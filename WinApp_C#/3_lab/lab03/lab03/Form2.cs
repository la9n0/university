using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace lab02
{
    public partial class Form2 : Form
    {
        private Label searchLabel;
        private ComboBox searchCombo;
        private TextBox searchText;
        private Label resultLabel;

        private Label sortLabel;
        private ComboBox sortCombo;
        private ComboBox orderCombo;

        private Button saveButton;
        private List<Student> studentsToSave = new List<Student>();
        private bool hasError = false;

        private Button newUsers;
        private Button aboutProgram;
        private University university = new University();
        
        private ToolStrip toolBar;
        private Button toggleToolBarButton;
        private bool isToolBarVisible = true;
        private readonly int toolBarHeight = 40; 
        private ToolStripButton Clear;
        private ToolStripButton Del;
        private ToolStripButton Back;
        private ToolStripButton Forward;
        
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private ToolStripStatusLabel timeLabel;
        private System.Windows.Forms.Timer timer;

        public Form2()
        {
            InitializeComponent();
            this.Size = new Size(500, 500);
            this.Text = "Сортировка студентов";
            university.LoadJSON();
            BuildForm();
        }

        private void BuildForm()
        {
            // Поиск
            searchLabel = new Label();
            searchLabel.Text = "Поиск по:";
            searchLabel.Location = new Point(15, 30);
            searchLabel.AutoSize = true;

            searchCombo = new ComboBox();
            searchCombo.Location = new Point(95, 25);
            searchCombo.Size = new Size(150, 30);
            searchCombo.Items.AddRange(new string[]
            {
                "ФИО",
                "Группе",
            });

            searchText = new TextBox();
            searchText.Location = new Point(265, 25);
            searchText.Size = new Size(200, 30);

            resultLabel = new Label()
            {
                BorderStyle = BorderStyle.FixedSingle,

                Location = new Point(100, 65),
                Size = new Size(300, 80),

            };

            this.Controls.Add(searchLabel);
            this.Controls.Add(searchCombo);
            this.Controls.Add(searchText);
            this.Controls.Add(resultLabel);
            searchCombo.SelectedIndexChanged -= FoundStudent;
            searchCombo.SelectedIndexChanged += FoundStudent;
            searchText.TextChanged -= FoundStudent;
            searchText.TextChanged += FoundStudent;

            // Сортировка
            sortLabel = new Label();
            sortLabel.Text = "Сортировка по:";
            sortLabel.Location = new Point(15, 150);
            sortLabel.AutoSize = true;

            sortCombo = new ComboBox();
            sortCombo.Location = new Point(135, 155);
            sortCombo.Size = new Size(150, 30);
            sortCombo.Items.AddRange(new string[]
            {
                "Возрасту",
                "Баллам",
                "Курсу",
                "Группе"
            });

            orderCombo = new ComboBox();
            orderCombo.Location = new Point(305, 155);
            orderCombo.Size = new Size(160, 30);
            orderCombo.Items.AddRange(new string[]
            {
                "По возрастанию",
                "По убыванию"
            });

            this.Controls.Add(sortLabel);
            this.Controls.Add(sortCombo);
            this.Controls.Add(orderCombo);
            sortCombo.SelectedIndexChanged -= FoundStudent;
            sortCombo.SelectedIndexChanged += FoundStudent;
            orderCombo.SelectedIndexChanged -= FoundStudent;
            orderCombo.SelectedIndexChanged += FoundStudent;

            // Сохранение
            saveButton = new Button();
            saveButton.Text = "Сохранить";
            saveButton.Size = new Size(170, 40);
            saveButton.Location = new Point(165, 200);

            this.Controls.Add(saveButton);
            saveButton.Click -= saveButton_Click;
            saveButton.Click += saveButton_Click;

            // Нижняя часть
            newUsers = new Button()
            {
                Text = "Добавить данные о студентах",
                Top = 265,
                Left = 240,
                Height = 30,
                Width = 230
            };
            newUsers.Click += newUser_click;
            Controls.Add(newUsers);

            aboutProgram = new Button()
            {
                Text = "О программе",
                Top = 265,
                Left = 20,
                Height = 30,
                Width = 200
            };
            aboutProgram.Click += aboutProgram_click;
            Controls.Add(aboutProgram);
            
            CreateToolBar();
            
            CreateStatusBar();
            StartTimer();
        }
        
        private void CreateToolBar()
        {
            toolBar = new ToolStrip();
            toolBar.Location = new Point(0, 0);
            toolBar.Size = new Size(this.Width - 40, toolBarHeight);
            Clear = new ToolStripButton("Очистить");
            Del = new ToolStripButton("Удалить");
            Back = new ToolStripButton("Назад");
            Forward = new ToolStripButton("Вперед");
            
            toolBar.Items.Add(Clear);
            toolBar.Items.Add(Del);
            toolBar.Items.Add(new ToolStripSeparator());
            toolBar.Items.Add(Back);
            toolBar.Items.Add(Forward);
            Clear.Click -= Clear_click;
            Clear.Click += Clear_click;
            Del.Click -= Del_click;
            Del.Click += Del_click;
            Back.Click -= Back_click;
            Back.Click += Back_click;
            Forward.Click -= Forward_click;
            Forward.Click += Forward_click;

            this.Controls.Add(toolBar);
            
            toggleToolBarButton = new Button();

            int size = 25;
            toggleToolBarButton.Size = new Size(size, size);
            toggleToolBarButton.Location = new Point(this.Width - size - 20, 0);
            toggleToolBarButton.BringToFront();
            toggleToolBarButton.Click += ToggleToolBar;
            this.Controls.Add(toggleToolBarButton);
            this.Controls.Add(toggleToolBarButton);
            toggleToolBarButton.BringToFront();
            toggleToolBarButton.Parent = this;

            MoveControls(toolBarHeight);
        }

        private void Clear_click(object sender, EventArgs e)
        {
            orderCombo.SelectedItem = null;
            sortCombo.SelectedItem = null;
            searchCombo.SelectedItem = null;
            searchText.Text = null;
        }
        private void Del_click(object sender, EventArgs e)
        {
            if(studentsToSave.Count>0)
                studentsToSave.RemoveAt(studentsToSave.Count-1);
            resultLabel.Text = null;
            foreach (Student student in studentsToSave)
            {
                resultLabel.Text += $"ФИО: {student.StudentName}, Группа: {student.Group};\n";
            }
        }

        private void Back_click(object sender, EventArgs e)
        {
            string path = "D:/university/4_sem/oop/3_lab/StructuredStudents.json";

            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);

            var restored = JsonSerializer.Deserialize<List<Student>>(json);

            if (restored != null)
                studentsToSave = restored;
            resultLabel.Text = null;
            foreach (Student student in studentsToSave)
            {
                resultLabel.Text += $"ФИО: {student.StudentName}, Группа: {student.Group};\n";
            }
        }

        private void Forward_click(object sender, EventArgs e)
        {
            FoundStudent(sender, e);
        }
        
        private void MoveControls(int offset)
        {
            foreach (Control control in this.Controls)
            {
                if (control == toolBar || control == toggleToolBarButton)
                    continue;

                control.Top += offset;
            }
        }
        
        private void ToggleToolBar(object sender, EventArgs e)
        {
            if (isToolBarVisible)
            {
                toolBar.Visible = false;
                MoveControls(-toolBarHeight);
                toggleToolBarButton.Text = "+";
                isToolBarVisible = false;
            }
            else
            {
                toolBar.Visible = true;
                MoveControls(toolBarHeight);
                toggleToolBarButton.Text = "-";
                isToolBarVisible = true;
            }
        }
        
        private void CreateStatusBar()
        {
            statusStrip = new StatusStrip();

            statusLabel = new ToolStripStatusLabel();
            timeLabel = new ToolStripStatusLabel();

            statusStrip.Items.Add(statusLabel);
            statusStrip.Items.Add(new ToolStripStatusLabel() { Spring = true });
            statusStrip.Items.Add(timeLabel);

            this.Controls.Add(statusStrip);

            UpdateStatus("Приложение запущено");
        }
        
        private void UpdateStatus(string action)
        {
            statusLabel.Text = $"Студентов: {university.Students.Count} | {action}";
            timeLabel.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }
        
        private void StartTimer()
        {
            this.timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += (s, e) =>
            {
                timeLabel.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            };
            timer.Start();
        }

        private void newUser_click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            form1.BuildForm();
            this.Hide();
        }

        private void aboutProgram_click(object sender, EventArgs e)
        {
            MessageBox.Show("Version 2.1\nArodz Vladislav Andreevich", "Information");
        }

        private void FoundStudent(object sender, EventArgs e)
        {
            resultLabel.Text = "";
            
            if (!ValidateInput(out string error))
            {
                resultLabel.Text = error;
                return;
            }

            List<Student> students = new List<Student>();
            string text = searchText.Text;

            if (searchCombo.SelectedIndex == 0)
            {
                foreach (Student student in university.Students)
                {
                    if (!string.IsNullOrEmpty(student.StudentName) &&
                        student.StudentName.Contains(text, StringComparison.OrdinalIgnoreCase))
                    {
                        students.Add(student);
                    }
                }
            }
            else if (searchCombo.SelectedIndex == 1)
            {
                foreach (Student student in university.Students)
                {
                    if (student.Group == text)
                    {
                        students.Add(student);
                    }
                }
            }
            else
            {
                students = new List<Student>(university.Students);
            }

            if (students.Count == 0)
            {
                resultLabel.Text = "Студенты не найдены";
                return;
            }

            if (sortCombo.SelectedItem != null && orderCombo.SelectedItem != null)
            {
                SortStudents(students);
            }

            foreach (Student student in students)
            {
                resultLabel.Text += $"ФИО: {student.StudentName}, Группа: {student.Group};\n";
            }
            studentsToSave=students;
            UpdateStatus("Поиск/сортировка выполнена");
        }

        private void SortStudents(List<Student> students)
        {
            switch (sortCombo.SelectedIndex)
            {
                case 0:
                    for (int i = 0; i < students.Count - 1; i++)
                    {
                        for (int j = 0; j < students.Count - i - 1; j++)
                        {
                            if (students[j].Age > students[j + 1].Age)
                            {
                                (students[j], students[j + 1]) = (students[j + 1], students[j]);
                            }
                        }
                    }
                    break;

                case 1:
                    for (int i = 0; i < students.Count - 1; i++)
                    {
                        for (int j = 0; j < students.Count - i - 1; j++)
                        {
                            if (students[j].Gpa > students[j + 1].Gpa)
                            {
                                (students[j], students[j + 1]) = (students[j + 1], students[j]);
                            }
                        }
                    }
                    break;

                case 2:
                    for (int i = 0; i < students.Count - 1; i++)
                    {
                        for (int j = 0; j < students.Count - i - 1; j++)
                        {
                            if (int.Parse(students[j].Course) > int.Parse(students[j + 1].Course))
                            {
                                (students[j], students[j + 1]) = (students[j + 1], students[j]);
                            }
                        }
                    }
                    break;

                case 3:
                    for (int i = 0; i < students.Count - 1; i++)
                    {
                        for (int j = 0; j < students.Count - i - 1; j++)
                        {
                            if (int.Parse(students[j].Group) > int.Parse(students[j + 1].Group))
                            {
                                (students[j], students[j + 1]) = (students[j + 1], students[j]);
                            }
                        }
                    }
                    break;
            }

            if (orderCombo.SelectedIndex == 1)
            {
                students.Reverse();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (hasError)
            {
                MessageBox.Show("Нельзя сохранить данные, есть ошибка ввода", "Ошибка");
                return;
            }

            string path = "D:/university/4_sem/oop/3_lab/StructuredStudents.json";
            string json = JsonSerializer.Serialize(studentsToSave);
            File.WriteAllText(path, json);
            
            UpdateStatus("Данные сохранены");
        }
        private bool ValidateInput(out string error)
        {
            var input = new SearchInput()
            {
                Type = searchCombo.SelectedItem?.ToString(),
                Value = searchText.Text
            };

            var context = new ValidationContext(input);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(input, context, results, true);

            if (!isValid)
            {
                error = results.First().ErrorMessage;
                hasError = true;
                return false;
            }

            error = null;
            hasError = false;
            return true;
        }
    }
}