var a=6;
var b="huh";

function task_4()
{
    console.log(window.a);
    console.log(window.b);

    //переопределение:
    window.a=1;
    window.b="123";


    console.log(window.a);
    console.log(window.b);
}