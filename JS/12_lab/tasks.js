class Sudoku
{
    constructor()   //1 массив - строка; 2 - столбец
    {
        this.field = Array.from({ length: 9 }, () =>
        Array.from({ length: 9 }, () => 0)
        );
    }
    checkColumn()
    {
        for(let i=0;i<9;i++)
        {
            for(let j=0;j<9;j++)
            {
                for(let k=0;k<9;k++)
                {
                    if(this.field[j][i]==this.field[k][i] && j!=k && this.field[j][i]!=0)
                        console.log("Ошибка в столбце ", i+1);
                }
                
            }
        }
    }
    checkLine()
    {
        for(let i=0;i<9;i++)
        {
            for(let j=0;j<9;j++)
            {
                for(let k=0;k<9;k++)
                {
                    if(this.field[i][j]==this.field[i][k] && j!=k && this.field[i][j]!=0)
                        console.log("Ошибка в строке ", i+1);
                }
                
            }
        }
    }
    checkSquare()
    {
        for(let i=0; i<9; i+=3)
        {
            for(let j=0; j<9; j+=3) //проходка по квадратам
            {
                for(let k=i;k<i+3;k++)
                {
                    for(let n=j;n<j+3;n++)//проходка
                    {
                        for(let q=i;q<i+3;q++)
                        {
                            for(let w=j;w<j+3;w++)  //x2
                            {
                                if(this.field[k][n]==this.field[q][w] && this.field[k][n]!=0 && !(k==q && n==w))
                                    console.log("Ошибка в квадрате: ", (i+3)/3, " сверху и ",(j+3)/3, "слева")
                            }
                        }
                    }
                }
            }
        }
    }

    reset()
    {
        this.field = Array.from({ length: 9 }, () =>
        Array.from({ length: 9 }, () => 0)
        );
    }
    errorsChecking()
    {
        this.checkColumn();
        this.checkLine();
        this.checkSquare();
        
    }

   isSafe(row, col, num)
    {
        for (let c = 0; c < 9; c++) 
        {
            if (this.field[row][c] === num) return false;
        }

        for (let r = 0; r < 9; r++) 
        {
            if (this.field[r][col] === num) return false;
        }

        const startRow = Math.floor(row / 3) * 3;
        const startCol = Math.floor(col / 3) * 3;

        for (let r = startRow; r < startRow + 3; r++) 
        {
            for (let c = startCol; c < startCol + 3; c++) 
            {
                if (this.field[r][c] === num) return false;
            }
        }
        return true;
    }

    fillingField()
    {
        for (let row = 0; row < 9; row++) 
        {
            for (let col = 0; col < 9; col++) 
            {
                if (this.field[row][col] === 0) 
                {
                    const nums = [1,2,3,4,5,6,7,8,9]
                        .sort(() => Math.random() - 0.5);
                    for (const num of nums) {
                        if (this.isSafe(row, col, num)) 
                        {
                            this.field[row][col] = num;
                            if (this.fillingField()) 
                            {
                                return true;
                            }
                            this.field[row][col] = 0;
                        }
                    }
                    return false;
                }
            }
        }
        return true;
    }
}

let a = new Sudoku;