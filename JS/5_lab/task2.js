function task_2()
{
    let k = prompt("Введите, будет ли 1 из рёбер повторяться(0 - нет, 1 - да)?")
    if(k==1)
    {
        let a = prompt("Введите длину этого ребра");
        return aa();
        function aa()
        {
            let b = prompt("Введите длину 2 ребра");
            let c = prompt("Введите длину 3 ребра");
            console.log("Объём параллепипеда =", a*b*c);
            return aa();
        }
    }
    else if(k==0)
    {
        return bb();
        function bb()
        {
            let a = prompt("Введите длину 1 ребра");
            let b = prompt("Введите длину 2 ребра");
            let c = prompt("Введите длину 3 ребра");
            console.log("Объём параллепипеда =", a*b*c);
            return a*b*c;
        }
        
    }
    else
    {
        console.log("Введены неверные данные");
    }
}