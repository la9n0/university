function task_3() {
    let X = 0;
    let Y = 0;
    console.log("Объект находится на координатах X=0; Y=0");

    function* move() {
        while (true) {
            const move = prompt("Введите направление движения (left, right, up, down):");

            if (move === "left") {
                X -= 10;
            } 
            else if (move === "right") {
                X += 10;
            } 
            else if (move === "up") {
                Y += 10;
            } 
            else if (move === "down") {
                Y -= 10;
            } 
            else {
                console.log("Введены неверные данные. Завершение работы.");
                return 1;
            }

            yield { X, Y }; //return но для генератора
            console.log(`Объект находится на координатах X=${X}; Y=${Y}.`);
        }
    }

    const gen = move();

    while (true) {
        const res = gen.next();
        if (res.done) break;
    }
}
