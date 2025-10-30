function task_2()
{
    let user = {name: "yay", age: 20};
    let admin = {a: 5, ...user};    // ... - это spread оператор
    console.log(user);
    console.log(admin);
}