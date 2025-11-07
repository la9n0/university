// 1_task
let person=
{
    name:"Vvviv",
    age: 28,
    greet()
    {
        console.log("Hi", this.name)
    },
    ageAfterYears(years)
    {
        return this.age + years
    }
}

// 2_task
let car2 =
{
    model:"Honda",
    year:2020,
    getInfo()
    {
        console.log("Model:", this.model,"\nYear:", this.year)
    }
}

// 3_task
function Book(author, title)
{
    this.author=author;
    this.title=title;
    this.getTitle=function()
    {
        console.log("Название: ", title);
    }
    this.getAuthor=function()
    {
        console.log("Автор: ", author);
    }
}
const book = new Book("Смелов", "Бессонница")

// 4_task
let team =
{
    arr: ["Пупкин", "Лупкин", "Запупкин"],

    getInfo()
    {
        console.log("Игроки: ");
        for(let a of this.arr)
        console.log(a);
    }
}

// 5_task
const counter = (function()
{
    let count=0;
    return {
        increment()
        {
            count++
            return count;
        },
        decrement()
        {
            count--;
            return count;
        },
        getCount()
        {
            return count;
        }
    };
}) ();

// 6_task
let item =
{
    price:1000,
}
Object.defineProperty(item, "price", //переопределение дескрипторов
{
    writable: false,    //изменение запрещено
    configurable: false //удаление запрещено
});

// 7_task
let circle =
{
    radius: 1,
    get square()
    {
        return (this.radius**2)*Math.PI;
    }
}

// 8_task
let car8 =
{
    make: "Japan",
    model:"Honda",
    year:2020
}
Object.defineProperties(car8, {
make: {
    writable: false,
    configurable: false
},
model: {
    writable: false,
    configurable: false
},
year: {
    writable: false,
    configurable: false
}
});

// 9_task
function task9()
{
    let arr = [1,2,3,4,5,6,7,8,9];
    Object.defineProperty(arr, "sum",
        {
            get() 
            {
                return arr.reduce((acc, val) =>
                {
                    return acc+=val;
                }, 0);
            }
        });
    return arr.sum
}
rectangle.square=4554
// 10_task
let rectangle =
{
    height: 10,
    width: 20,

    get square()
    {
        return this.width*this.height;       
    },
        
    get Width() {return this.width;},
    set Width(value) {},

    get Height() {return this.height;},
    set Height(value) {},
}

// 11_task
let user =
{
    firstName: "vl",
    lastName: "ar",

    get getName()
    {
        return this.firstName+" "+this.lastName;
    },
    set setName(value)
    {
        let arr = value.split(" ");
        this.firstName=arr[0];
        this.lastName=arr[1];
    }
}