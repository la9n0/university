//task_1
function basicOperation(operation, value1, value2)
{
    let result=0;
    if (operation==="+")
        result=value1+value2;
    else if (operation==="-")
        result=value1-value2;
    else if (operation==="*")
        result=value1*value2;
    else if(operation==="/")
        result=value1/value2;
    return result;
}

let result = basicOperation("-", 5, 10);
console.log("task_1: ", result);

//task_2

function task_2(n)
{
    let result=0;
    for(let i=1; i<=n;i++)
    {
        result+=i**3;
    }
    return result;
}

console.log("task_2: ", task_2(4));

//task_3

function task_3(arr)
{
    let result=0;
    for(let i=0; i<arr.length; i++)
    {
        result+=arr[i];
    }
    result/=arr.length;

    return result;
}

let arr=[5,4,6,2,8];
console.log("task_3: ", task_3(arr));

//task_4

function task_4(str)
{
    let result = "";
    for(let i=str.length-1; i>=0;i--)
    {
        if((str.charCodeAt(i)>64 && str.charCodeAt(i)<91) || (str.charCodeAt(i)>96 && str.charCodeAt(i)<123))
            result+=str[i];
    }
    return result;
}

console.log("task_4: ", task_4("Help pls екснмгишзнесмгпшзнае"));

//task_5

function task_5(n, s)
{
    return s.repeat(n);
}

console.log("task_5: ", task_5(4, "Да"));

//task_6

function task_6(arr1, arr2)
{
    let arr3=[];
    for(let i=0; i<arr1.length;i++)
    {
        for(let j=0; j<arr2.length;j++)
        {
            if(arr1[i]==arr2[j])
                break;
            if(j==arr2.length-1)
                arr3.push(arr1[i]);
        }
    }
    return arr3;
}

let arr1=["34", "rt", "op", "65"];
let arr2=["34", "65", "765435g4"];
console.log("task_6: ", task_6(arr1, arr2));