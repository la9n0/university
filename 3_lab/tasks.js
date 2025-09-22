//task_1

function task_1(arr1, arr2) 
{
    return [...arr1, ...arr2].reduce((acc, item) => {
        if (Array.isArray(item)) 
            return [...acc, ...task_1(item, [])];
        else 
            return [...acc, item];
    }, []);
}

let arr1=[1,[1,2,[3,4]]]
let arr2=[2,4];
console.log("task_1: ", task_1(arr1, arr2))

//task_2

function task_2(arr) 
{
    return arr.reduce((sum, item) => {
        return sum + (Array.isArray(item) ? task_2(item) : item);
    }, 0);
}

let arr = [5, 3, 7, [45, 23, [422], 34], 6];
console.log("task_2: ", task_2(arr));

//task_3

function task_3(arr)
{
    a={};
    for(let i=0; i<arr.length; i++)
    {
        if (arr[i].age>17)
        {
            if (!a[arr[i].group_number]) 
                a[arr[i].group_number] = [];
            a[arr[i].group_number].push(arr[i].name);
        }
    }
    return a;
}

arr=[{name:"John", age: 20, group_number: 7}, 
    {name:"Jonathan", age: 19, group_number: 5},
    {name:"Jessica", age: 17, group_number: 10},
    {name:"Genadii", age: 21, group_number: 7}];
console.log("task_3: ", task_3(arr));

//task_4

function task_4()
{
    str="ABC"
    let total1="", total2="";    
    for(let i=0;i<str.length;i++)
    {
        total1+=str.charCodeAt(i);
    }
    for(let i=0;i<total1.length;i++)
    {
        if (total1[i]==="7")
            total2+=1;
        else
            total2+=total1[i];
    }
    return Number(total1)-Number(total2)
}

console.log("task_4: ", task_4());

//task_5

function task_5(obj1, obj2, obj3)
{
    
    let obj = Object.assign({},obj1, obj2, obj3)
    return obj;
}

obj1={name:"John"};
obj2={age: 19};
obj3={group_number: 10};
console.log("task_5: ", task_5(obj1, obj2, obj3));

//task_6    #башне немного плохо

function task_6(n)
{
    let space=" ", star="*";
    let tower="\n";
    for(let i=1;i<=n;i++)
    {
        tower+=space.repeat(Math.trunc((n-i)/2))+star.repeat(i)+space.repeat(Math.trunc((n-i)/2))+"\n";
    }
    return tower;
}

console.log("task_6: ", task_6(5));