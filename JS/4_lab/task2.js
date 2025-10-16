let students=new Set();
let groupCache = new WeakMap(); 

function add_student2()
{
    let student={
    num:Number(prompt("Введите номер зачётки студента")),
    group:prompt("Введите группу студента"),
    FIO:prompt("Введите ФИО студента")
    }
    students.add(student);
    console.log("Студент", student.FIO, "добавлен");
    return students;
}

function del_student2()
{
    let a=false;
    let num=prompt("Введите номер зачётки для удаления студента из списка");
    let student={};
    for(student of students)
    {
        if(student.num==num)
        {
            a=true;
            break;
        }
    }
    if(a)
    {
        students.delete(student);
        console.log("Студент", student.FIO, "удалён из списка");
    }
    else
        console.log("Студент не найден");
    return students;
}

function filter_by_group2()
{
    let group=prompt("Введите группу для фильтрации списка студентов");
    console.log("Из списка удалены:");
    let a=false;
    for(let student of students)
    {
        if(student.group!=group)
        {
            a=true
            students.delete(student);
            console.log(student.FIO);
        }
    }
    if(!a)
        console.log("Никто")
    return students;
}

function sort_by_number2()
{
    let sort_stud=new Set();
    let n=0;
    for (let student of students)
    {
        sort_stud.add(student);
        n++;
    }
    students.clear();
    let rec_num=Number.MAX_VALUE, min_stud={};
    for(let i=0; i<n; i++)
    {
        rec_num=Number.MAX_VALUE;
        min_stud={};
        for(let student of sort_stud)
        {
            if(student.num<rec_num)
            {
                rec_num=student.num;
                min_stud=student;
            }
        }
        students.add(min_stud)
        sort_stud.delete(min_stud);
    }
    return students;
}

//task_4

let groupObjects = new Map();

function get_students_by_group()
{
    let groupName = prompt("Введите группу для поиска студентов");

    //для создания/взятия ключа
    let groupObj;
    if (!groupObjects.has(groupName))
    {
        groupObj = { group: groupName };
        groupObjects.set(groupName, groupObj);
    }
    else
    {
        groupObj = groupObjects.get(groupName);
    }

    //другое
    if (groupCache.has(groupObj))
    {
        console.log("Данные взяты из кеша:");
        return groupCache.get(groupObj);
    }
    else
    {
        let result = [];
        for (let student of students)
        {
            if (student.group === groupName)
            {
                result.push(student);
            }
        }
        groupCache.set(groupObj, result);
        console.log("Данные добавлены в кеш:");
        return result;
    }
}