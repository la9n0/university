function task_3()
{
    let X=0;
    let Y=0;
    console.log("Объект находится на координатах X=0; Y=0")
    gen();
    function gen()
    {
        let move = prompt("Введите направление движения(left, right, up, down)")
        if(move=="left")
        {
            X-=10;
        }
        else if(move=="right")
        {
            X+=10;
        }
        else if(move=="up")
        {
            Y+=10;
        }
        else if(move=="down")
        {
            Y-=10;
        }
        else
        {
            console.log("Введены неверные данные");
            return 1;
        }
        console.log("Объект находится на координатах X=", X, "; Y=", Y, ".");
        return gen();
    }
}