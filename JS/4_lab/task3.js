let storage_of_products=new Map();

function add_product3()
{
    let id=prompt("Введите id товара");
    let obj={
        name:prompt("Введите название товара"),
        quantity:prompt("Введите количество товара"),
        price:prompt("Введите цену товара")
    };
    storage_of_products.set(id,obj)
    return storage_of_products;
}

function del_product_by_id3()
{
    let id=prompt("Введите id товара для удаления из хранилища");
    if(storage_of_products.has(id))
    {
        storage_of_products.delete(id);
        console.log("Товар удалён из хранилища");
    }
    else
    {
        console.log("Товар не найден");
    }
    return storage_of_products;
}

function del_product_by_name3()
{
    let a=true;
    let name=prompt("Введите имя товара для удаления из хранилища");
    for (let [key, value] of storage_of_products.entries()) 
    {
        if(value.name==name)
        {
            storage_of_products.delete(key);
            console.log("Товар удалён");
            a=false;
        }
    }
    if(a)
    {
        console.log("Товар с данным именем не найден");
    }
}

function change_quantity3()
{
    for (let [key, value] of storage_of_products.entries()) 
    {
        value.quantity=prompt("Введите новое количество товара с id: "+ key);
        storage_of_products.set(key,value);
    }
    return storage_of_products;
}

function change_price3()
{
    let id=prompt("Введите id товара, цену которого вы хотите изменить");
    if(!storage_of_products.has(id))
    {
        console.log("Товар с данным id не найден:(");
        return storage_of_products;
    }
    let price=prompt("Введите новую цену для товара с id: "+id);
    let value=storage_of_products.get(id);
    value.price=price;
    storage_of_products.set(id, value);
    return storage_of_products;
}

function storage_info3()
{
    let count = storage_of_products.size;
    let total = 0;

    for (let value of storage_of_products.values())
    {
        total += Number(value.quantity) * Number(value.price);
    }

    console.log("Количество позиций: " + count);
    console.log("Общая сумма: " + total);
    return storage_of_products;
}