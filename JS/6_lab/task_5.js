function task_5()
{
    function sumValues(x, y, z)
    {
        return x+y+z;
    }

    console.log("Результат: ", sumValues(...[1,2,3]));
}