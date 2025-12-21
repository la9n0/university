class Task
{
    id=0;
    name="";
    state=false;
    
    constructor(id, name)
    {
        this.id=id;
        this.name=name;
    }

    changeName(name)
    {
        this.name=name;
    }
    changeState(s)
    {
        this.state=s;
    }
}

class Todolist
{
    id=0;
    name="";
    tasks=[];
    changeName(name)
    {
        this.name=name;
    }
    addTask()
    {
        let a = new Task();
        a.id=prompt("Введите id задачи");
        a.name=prompt("Введите имя задачи");
        this.tasks.push(a);
    }
    filter()
    {
        let a=[];
        for(let i=0; i<this.tasks.length;i++)
        {
            a.push(this.tasks[i]);
        }
        this.tasks.length=0;
        for(let i=0;i<a.length;i++)
        {
            if(a[i].state==true)
            {
                this.tasks.push(a[i]);
            }
        }
        for(let i=0;i<a.length;i++)
        {
            if(a[i].state==false)
            {
                this.tasks.push(a[i]);
            }
        }
    }
}

let dud = new Todolist();
dud.name="spisok1";
dud.id=239234;

let sus = new Todolist();
sus.name="spis2";
sus.id=7565434;