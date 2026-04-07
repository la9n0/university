namespace lab01
{

    public partial class InitialForm : Form
    {
        // измерения
        private Dictionary<string, double> lengthToMeter = new Dictionary<string, double>()
        {
            {"Километры", 1000},
            {"Метры", 1},
            {"Сантиметры", 0.01},
            {"Миллиметры", 0.001},
            {"Дюймы", 0.0254},
            {"Футы", 0.3048},
            {"Ярды", 0.9144},
            {"Мили", 1609.344}
        };

        private Dictionary<string, double> weightToKg = new Dictionary<string, double>()
        {
            {"Тонны", 1000},
            {"Килограммы", 1},
            {"Граммы", 0.001},
            {"Фунты", 0.45359237},
            {"Унции", 0.0283495231},
            {"Стоуны", 6.35029318}
        };

        private Dictionary<string, double> volumeToM3 = new Dictionary<string, double>()
        {
            {"м³", 1},
            {"Литры", 0.001},
            {"Галлоны", 0.00378541},
            {"Кварты", 0.000946353},
            {"Пинты", 0.000473176},
            {"Джиллы", 0.000118294},
            {"Жидкие ункции", 0.0000295735}
        };
        public InitialForm()
        {
            InitializeComponent();
            this.Size = new Size(420, 100);
            Button headButton1 = new Button
            {
                Text = "Длина",
                Left = 20,
                Top = 5,
                Width = 80,
                Height = 30
            };
            Button headButton2 = new Button
            {
                Text = "Вес",
                Left = 145,
                Top = 5,
                Width = 80,
                Height = 30
            };
            Button headButton3 = new Button
            {
                Text = "Объём",
                Left = 270,
                Top = 5,
                Width = 80,
                Height = 30
            };
            headButton1.Click += button1_Click;
            headButton2.Click += button2_Click;
            headButton3.Click += button3_Click;

            this.Controls.Add(headButton1);
            this.Controls.Add(headButton2);
            this.Controls.Add(headButton3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Base();
            comboBox1.Items.AddRange(new string[]
            {
                "Километры", "Метры", "Сантиметры", "Миллиметры"
            });
            comboBox2.Items.AddRange(new string[]
            {
                "Дюймы", "Футы", "Ярды", "Мили"
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Base();
            comboBox1.Items.AddRange(new string[]
            {
                "Тонны", "Килограммы", "Граммы"
            });
            comboBox2.Items.AddRange(new string[]
            {
                "Фунты", "Унции", "Стоуны"
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Base();
            comboBox1.Items.AddRange(new string[]
            {
                "м³", "Литры"
            });
            comboBox2.Items.AddRange(new string[]
            {
                "Галлоны", "Кварты", "Пинты", "Джиллы", "Жидкие ункции"
            });
        }
        
        private ComboBox comboBox1 = new ComboBox(); // это в Base использую
        private ComboBox comboBox2 = new ComboBox();
        private TextBox textBox1 = new TextBox();
        private TextBox textBox2 =  new TextBox();
        private Button arrowButton1  = new Button();
        private Button arrowButton2  = new Button();
        private Button reverseButton  = new Button();
        private void Base()
        {
            this.Size = new Size(420, 600);

            // тут будет часть с выбором 
            comboBox1.Left = 50;
            comboBox1.Top = 100;
            comboBox1.Width = 125;
            comboBox1.Items.Clear();

            comboBox2.Left = 245;
            comboBox2.Top = 100;
            comboBox2.Width = 125;
            comboBox2.Items.Clear();

            arrowButton1.Left = 185;
            arrowButton1.Top = 100;
            arrowButton1.Width = 50;
            arrowButton1.Height = 30;
            arrowButton1.Text = "⮕";

            reverseButton.Left = 160;
            reverseButton.Top = 150;
            reverseButton.Width = 100;
            reverseButton.Height = 50;
            reverseButton.Text = "⮂";
            reverseButton.Click += reverseButton_Click;

            if(!this.Controls.Contains(comboBox1)) this.Controls.Add(comboBox1);
            if (!this.Controls.Contains(comboBox2)) this.Controls.Add(comboBox2);
            if (!this.Controls.Contains(arrowButton1)) this.Controls.Add(arrowButton1);
            if (!this.Controls.Contains(reverseButton)) this.Controls.Add(reverseButton);

            // тут часть с измерениями
            textBox1.Left = 50;
            textBox1.Top = 250;
            textBox1.Width = 320;
            textBox1.Height = 100;
            textBox1.Multiline = true;
            textBox1.BorderStyle = BorderStyle.FixedSingle;

            textBox2.Left = 50;
            textBox2.Top = 425;
            textBox2.Width = 320;
            textBox2.Height = 100;
            textBox2.Multiline = true;
            textBox2.BorderStyle = BorderStyle.FixedSingle;

            arrowButton2.Top = 365;
            arrowButton2.Left = 190;
            arrowButton2.Width = 20;
            arrowButton2.Height = 50;
            arrowButton2.Text = "⬇";
            
            // косметическая работа над кнопками со стрелками
            arrowButton2.FlatStyle = FlatStyle.Flat;
            arrowButton2.FlatAppearance.BorderSize = 0;
            arrowButton2.FlatAppearance.MouseDownBackColor = arrowButton2.BackColor;
            arrowButton2.FlatAppearance.MouseOverBackColor = arrowButton2.BackColor;

            arrowButton1.FlatStyle = FlatStyle.Flat;
            arrowButton1.FlatAppearance.BorderSize = 0;
            arrowButton1.FlatAppearance.MouseDownBackColor = arrowButton1.BackColor;
            arrowButton1.FlatAppearance.MouseOverBackColor = arrowButton1.BackColor;

            if(!this.Controls.Contains(textBox1)) this.Controls.Add(textBox1);
            if(!this.Controls.Contains(textBox2)) this.Controls.Add(textBox2);
            if(!this.Controls.Contains(arrowButton2)) this.Controls.Add(arrowButton2);
            
            textBox1.TextChanged -= Convert;
            textBox1.TextChanged += Convert;

            comboBox1.SelectedIndexChanged -= Convert;
            comboBox1.SelectedIndexChanged += Convert;

            comboBox2.SelectedIndexChanged -= Convert;
            comboBox2.SelectedIndexChanged += Convert;
        }
        
        private void reverseButton_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count == 0 || comboBox2.Items.Count == 0)
                return;

            object selected1 = comboBox1.SelectedItem;
            object selected2 = comboBox2.SelectedItem;

            var items1 = comboBox1.Items.Cast<object>().ToList();
            var items2 = comboBox2.Items.Cast<object>().ToList();

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            comboBox1.Items.AddRange(items2.ToArray());
            comboBox2.Items.AddRange(items1.ToArray());
            
            comboBox1.SelectedItem = selected2;
            comboBox2.SelectedItem = selected1;
            
            Convert(null, EventArgs.Empty);
        }
        
        // перевод
        private void Convert(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null)
                return;

            if (!double.TryParse(textBox1.Text.Replace(",", "."),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out double value))
            {
                textBox2.Clear();
                return;
            }

            string from = comboBox1.SelectedItem.ToString();
            string to = comboBox2.SelectedItem.ToString();

            double result = ConvertValue(from, to, value);

            textBox2.Text = result.ToString();
        }
        
        private double ConvertValue(string from, string to, double value)
        {
            // длина
            if (lengthToMeter.ContainsKey(from) && lengthToMeter.ContainsKey(to))
            {
                double meters = value * lengthToMeter[from];
                return meters / lengthToMeter[to];
            }

            // вес
            if (weightToKg.ContainsKey(from) && weightToKg.ContainsKey(to))
            {
                double kg = value * weightToKg[from];
                return kg / weightToKg[to];
            }

            // объем
            if (volumeToM3.ContainsKey(from) && volumeToM3.ContainsKey(to))
            {
                double m3 = value * volumeToM3[from];
                return m3 / volumeToM3[to];
            }

            return 0;
        }
    }
}