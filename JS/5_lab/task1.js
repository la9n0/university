//Вариант 1: выведет 1, 2, 3, 1

/*function makeCounter()
{
    let currentCount = 1;

    return function()
    {
        return currentCount++;
    };
}

let counter = makeCounter();

alert( counter());
alert( counter());
alert( counter());

let counter2 = makeCounter();
alert(counter2());*/

//Вариант 2: выведет 1, 2, 3, 4

/*let currentCount = 1;

function makeCounter()
{
    return function()
    {
        return currentCount++;
    };
}

let counter = makeCounter();
let counter2 = makeCounter();

alert( counter());
alert( counter());

alert( counter2());
alert(counter2());*/

//Так происходит по той причине, что в 1 случае переменная задана
//локально, а во втором - глобально
