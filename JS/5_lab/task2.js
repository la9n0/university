function task_2() {
    function volume(a) {
        return function(b) {
            return function(c) {
                return a * b * c;
            };
        };
    }

    let k = prompt("Введите, будет ли 1 из рёбер повторяться (0 - нет, 1 - да)?");

    if (k == 1) {
        let a = prompt("Введите длину этого ребра:");

        function fix() {
            let b = prompt("Введите длину 2-го ребра:");
            let c = prompt("Введите длину 3-го ребра:");
            let result = volume(a)(b)(c);

            console.log("Объём параллелепипеда =", result);
            return fix();
        }

        return fix();
    } 
    else if (k == 0) {
        let a = prompt("Введите длину 1-го ребра:");
        let b = prompt("Введите длину 2-го ребра:");
        let c = prompt("Введите длину 3-го ребра:");
        return volume(a)(b)(c);
    } 
    else {
        return "Введены неверные данные.";
    }
}