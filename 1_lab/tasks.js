function task_1() 
{
let a=5;
let name="Name"
let i=0;
let double=0.23;
let result=1/0;
let answer=true;
let no=null;

console.log(typeof(a));
console.log(typeof(name));
console.log(typeof(i));
console.log(typeof(double));
console.log(typeof(result));
console.log(typeof(answer));
console.log(typeof(no));
}

function task_2()
{
let B=5;
let dl=45;
let sh=21;

a=Math.floor(dl/B);
let b=Math.floor(sh/B);
return(a*b);
}

function task_3() 
{
let i=2;
let a=++i;
let b=i++;

if (a>b)
    return("a>b")
else if(a<b)
    return("a<b")
else
    return("a=b")
}

function task_4()
{
   console.log("Котик" == "котик" ? "равны" : "не равны");
   console.log("Котик" == "китик" ? "равны" : "не равны");
   console.log("Кот" == "Котик" ? "равны" : "не равны");
   console.log("Привет" == "Пока" ? "равны" : "не равны");
   console.log(73 == "53" ? "равны" : "не равны");
   console.log(false == 0.54 ? "равны" : "не равны");
   console.log(true == 0.54 ? "равны" : "не равны");
   console.log(123 == false ? "равны" : "не равны");
   console.log("3" == true ? "равны" : "не равны");
   console.log(3 == "5мм" ? "равны" : "не равны");
   console.log(8 == "-2" ? "равны" : "не равны");
   console.log(34 == "34" ? "равны" : "не равны");
   console.log(null == undefined ? "равны" : "не равны");
}

function task_5()
{
    let teacherFullName = "якубенко ксения дмитриевна";
    let teacherNameMiddle = "ксения дмитриевна";
    let teacherName = "ксения";

  let input = prompt("Введите имя:");

  input = input.trim().toLowerCase();

  if (input === teacherFullName ||
    input === teacherNameMiddle ||
    input === teacherName) 
    {console.log("Введенные данные верные");} 
  else
    {console.log("Данные не совпадают");} 
}

function task_6()
{
  let russian = true;
  let math = true;
  let english = true;

  if (russian && math && english) 
    {console.log("Студент переведен на следующий курс");}
  else if (!russian && !math && !english) 
    {console.log("Студент отчислен");} 
  else 
    {console.log("Студента ждёт пересдача");}
}

function task_7()
{
    console.log(true+true);
    console.log(0+"5");
    console.log(5+"мм");
    console.log(8/Infinity);
    console.log(9*"\n9");
    console.log(null-1);
    console.log("5"-2);
    console.log("5px"-3);
    console.log(true-3);
    console.log(7||0);
}

function task_8()
{
    for (let i = 1; i <= 10; i++) 
    {
    if (i % 2 === 0) 
        {console.log(i + 2);} 
    else 
        {console.log(i + "мм");}
    }
}

function task_9()
{
    const daysObj = {
    1: "пн",
    2: "вт",
    3: "ср",
    4: "чт",
    5: "пт",
    6: "сб",
    7: "вс"
  };

  const daysArr = ["пн", "вт", "ср", "чт", "пт", "сб", "вс"];

  let dayNumber=prompt("Введите номер дня");

  if (dayNumber >= 1 && dayNumber <= 7) 
    {
    console.log("Через объект:", daysObj[dayNumber]);
    console.log("Через массив:", daysArr[dayNumber - 1]);
    } 
  else 
    {
    console.log("Неверный номер дня!");
    }
}

function task_10(parm1="olegher")
{
    let parm2="43544";
    let parm3=prompt("Введите 3 параметр:");
    let parm=parm1+" "+parm2+" "+parm3;
    return(parm)
}

//task_11

function paramsDec()
{
    let a=prompt("a=")
    let b=prompt("b=")
    if (a === b) 
        {return 4 * a;} 
    else 
        {return a * b;}
}

const paramsExp = function()
{
    let a=prompt("a=")
    let b=prompt("b=")
    if (a === b) 
        {return 4 * a;} 
    else 
        {return a * b;}
}

const paramsArr = (a, b) => a === b ? 4 * a : a * b;