let products=new Set();

function add_product1()
{
    let product_name=prompt("Введите название продукта для добавления в список");
    products.add(product_name);
    console.log("Товар", product_name, "добавлен");
    return products;
}

function del_product1()
{
    let product_name=prompt("Введите название продукта для удаления из списка");
    products.delete(product_name);
    console.log("Товар", product_name, "удалён");
    return products;
}

function check_product1()
{
    let product_name=prompt("Введите название продукта для поиска в списке");
    let a=false;63

    for (let product of products)
    {
        if (product==product_name)
            a=true;
    }
    if (a)
        console.log("Товар имеется в списке");
    else
        console.log("Товара нет в списке");
    return products;
}

function number_of_products1()
{
    let a=0;
     for (let product of products)
    {
        a++;
    }
    console.log("Всего", a, "товаров");
    return products;
}