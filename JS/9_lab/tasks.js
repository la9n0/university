function task_1()
{
    // квадрат
    {
        let square =
        {
            type: "square",
            color:"yellow"
        }

        let little_square=Object.create(square);
        little_square.size="little";
        console.log("Маленький квадрат: ",little_square);

        let big_square=Object.create(square);
        big_square.size="big";
        console.log("Большой квадрат: ", big_square);
    }

    //круг
    {
        let circle =
        {
            type:"circle"
        }

        let white_circle=Object.create(circle);
        white_circle.color="white";
        console.log("Белый круг: ", white_circle);

        let green_circle=Object.create(circle);
        green_circle.color="green";
        console.log("Зелёный круг: ", green_circle);
    }

    //треугольник
    {
        let triangle =
        {
            type:"triangle",
            color:"white"
        }

        let line_triangle=Object.create(triangle);
        line_triangle.vertical_lines=1;
        console.log("Треугольник с 1 вертикальной линией: ", line_triangle);

        let lines_triangle=Object.create(triangle);
        lines_triangle.vertical_lines=3;
        console.log("Треугольник с 3 вертикальными линиями: ", lines_triangle);
    }
}

function task_2()
{
    class Human
    {
        name="vl";
        surname="ar";
        age=20;
        address="st. Pushkina";

        changeAge(age)
        {
            this.age=age;
        }
        changeName(address)
        {
            this.address=address;
        }

        year_of_birth=2005;
        get ageGet()
        {
            return 2025-this.year_of_birth;
        }
        set ageSet(val)
        {
            this.age=2025-this.year_of_birth;
        }
    }

    class Student extends Human
    {
        constructor(name, surname, group, recordNumber)
        {
        faculty="ФИТ";
        course=2;
        group=9;
        recordNumber=71242123;
        Faculty.students.push(this);
        }

        changeCourseAndGroup()
        {
            let course=prompt("Введите новый курс");
            let group=prompt("Введите новую группу");
            this.course=course;
            this.group=group;
        }

        getFullName()
        {
            console.log("Имя:", this.name, "\nФамилия:", this.surname);
        }
    }

    class Faculty
    {
        static students=[];
        fucName="ФИТ";
        numberOfGroup=10;
        numberOfStudents=290;


        changeNumberOfGroup(value)
        {
            this.numberOfGroup=value;
        }
        changeNumberOfStudents(value)
        {
            this.numberOfStudents=value;
        }
        getDev(a)
        {
            let quen=0;
            for(let i=0; i<students.length(); i++)
            {
                let str=string(students[i].recordNumber);
                if (str[1]=='3')
                    quen++;
            }
            return quen;
        }
        getGroup(a)
        {
            let quen=0;
            for(let i=0; i<students.length(); i++)
            {
                if(a==students[i].group)
                {
                    console.log(students[i]);
                }
            }
            return quen;
        }
    }
}