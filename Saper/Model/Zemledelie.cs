using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Saper.Model
{
    internal class Zemledelie : INotifyPropertyChanged
    {
        public string[,] Pole { get; set; }
        public string[,] Values { get; set; }
        public int s;
        public int x;
        public int y;
        public Zemledelie(int x, int y)
        {
            Pole = new string[x + 2, y + 2];
            Values = new string[x + 2, y + 2];
            s = x * y;
            this.x = x;
            this.y = y;
            Generator();
        }
        public void Generator()
        {
            Full();
            for (int i = 1; i < x + 1; i++)
            {
                for (int j = 1; j < y + 1; j++)
                {
                    Values[i, j] = " ";
                }
            }
            double ss = Convert.ToDouble(s);
            int mines = Convert.ToInt32(ss / 100 * 16);
            Random rnd = new Random();
            while (mines > 0)
            {
                for (int i = 1; i < x + 1; i++)
                {
                    for (int j = 1; j < y + 1; j++)
                    {
                        if (mines > 0)
                        {
                            if (rnd.Next(0, s) <= 9)
                            {
                                Pole[i, j] = "Mine";
                                mines--;
                            }
                        }
                    }
                }
            }
            for (int i = 1; i < x + 1; i++)
            {
                for (int j = 1; j < y + 1; j++)
                {
                    if (Pole[i, j] != "Mine")
                    {
                        int count = 0;
                        //L-T
                        if (Pole[i - 1, j - 1] == "Mine")
                        {
                            count++;
                            Pole[i, j] = count.ToString();
                        }
                        //T
                        if (Pole[i - 1, j] == "Mine")
                        {
                            count++;
                            Pole[i, j] = count.ToString();
                        }
                        //R-T
                        if (Pole[i - 1, j + 1] == "Mine")
                        {
                            count++;
                            Pole[i, j] = count.ToString();
                        }
                        //L
                        if (Pole[i, j - 1] == "Mine")
                        {
                            count++;
                            Pole[i, j] = count.ToString();
                        }
                        //R
                        if (Pole[i, j + 1] == "Mine")
                        {
                            count++;
                            Pole[i, j] = count.ToString();
                        }
                        //L-B
                        if (Pole[i + 1, j - 1] == "Mine")
                        {
                            count++;
                            Pole[i, j] = count.ToString();
                        }
                        //B
                        if (Pole[i + 1, j] == "Mine")
                        {
                            count++;
                            Pole[i, j] = count.ToString();
                        }
                        //R-B
                        if (Pole[i + 1, j + 1] == "Mine")
                        {
                            count++;
                            Pole[i, j] = count.ToString();
                        }
                    }
                }
            }
            ToStr();
        }

        public void Full()
        {
            for (int i = 0; i < x + 2; i++)
            {
                for (int j = 0; j < y + 2; j++)
                {
                    Pole[i, j] = "0";
                }
            }
        }
        public void ToStr()
        {
            for (int i = 1; i < x + 1; i++)
            {
                for (int j = 1; j < y + 1; j++)
                {
                    Console.Write(Pole[i, j] + " ");
                }
                Console.WriteLine();
            }

        }

        public string OpenCell(int row, int column)
        {
            // Проверяем, что переданные координаты в пределах массива
            if (row < 0 || row >= x || column < 0 || column >= y)
            {
                return string.Empty; // Возвращаем пустую строку для некорректных координат
            }

            // Проверяем, открыта ли уже ячейка
            if (Values[row, column] == "открыта")
            {
                return Pole[row, column]; // Если открыта, возвращаем её текущее состояние
            }

            // Открываем ячейку
            Values[row, column] = "открыта";

            // Возвращаем содержимое ячейки
            return Pole[row, column];
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
