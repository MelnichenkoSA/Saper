using Saper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Windows.Shapes;
using System.Text.Json;
using Saper.View;

namespace Saper.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private string _selectedDifficulty;
        private ObservableCollection<CellViewModel> _cells;
        private string _status;
        private int _highscore;
        private Zemledelie _zemledelie;

        public MainViewModel()
        {
            DifficultyLevels = new ObservableCollection<string> { "Easy", "Medium", "Hard" };
            SelectedDifficulty = DifficultyLevels[0];
            InitializeGame();
            Scorenik();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void Scorenik()
        {
                using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
                {
                    Highscore = JsonSerializer.Deserialize<int>(fs);
                }
        }
        public ObservableCollection<string> DifficultyLevels { get; }

        public string SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                if (_selectedDifficulty != value)
                {
                    _selectedDifficulty = value;
                    OnPropertyChanged();
                    InitializeGame();
                }
            }
        }

        public ObservableCollection<CellViewModel> Cells
        {
            get => _cells;
            set
            {
                if (_cells != value)
                {
                    _cells = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Highscore
        {
            get => _highscore;
            set
            {
                if (_highscore != value)
                {
                    _highscore = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _columns;

        public int Columns
        {
            get => _columns;
            set
            {
                if (_columns != value)
                {
                    _columns = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _rows;

        public int Rows
        {
            get => _rows;
            set
            {
                if (_rows != value)
                {
                    _rows = value;
                    OnPropertyChanged();
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InitializeGame()
        {
            _zemledelie = new Zemledelie(GetRows(), GetColumns());

            Cells = new ObservableCollection<CellViewModel>();
            Columns = _zemledelie.y;
            Rows = _zemledelie.x;
            for (int i = 1; i <= _zemledelie.x; i++)
            {
                for (int j = 1; j <= _zemledelie.y; j++)
                {
                    string cellContent = _zemledelie.Values[i, j]; 
                    Cells.Add(new CellViewModel { DisplayText = cellContent });
                }
            }

        }

        private int GetRows()
        {
            switch (SelectedDifficulty)
            {
                case "Easy":
                    return 8;
                case "Medium":
                    return 10;
                case "Hard":
                    return 12;
                default:
                    return 8;
            }
        }

        private int GetColumns()
        { 
            switch (SelectedDifficulty)
            {
                case "Easy":
                    return 8;
                case "Medium":
                    return 10;
                case "Hard":
                    return 12;
                default:
                    return 8;
            }
        }

        private RelayCommand _cellClickCommand;

        public ICommand CellClickCommand
        {
            get
            {
                if (_cellClickCommand == null)
                {
                    _cellClickCommand = new RelayCommand(param => CellClick(param));
                }
                return _cellClickCommand;
            }
        }

        private RelayCommand _optionClickCommand;

        public RelayCommand OptionClickCommand
        {
            get
            {
                return _optionClickCommand ??
                  (_optionClickCommand = new RelayCommand(obj =>
                  {
                      Window1 passwordWindow = new Window1();

                      if (passwordWindow.ShowDialog() == true)
                      {
                          SelectedDifficulty = passwordWindow.str;
                          MessageBox.Show("Сложность установлена");
                      }
                      else
                      {
                          MessageBox.Show("Сложность не изменена");
                      }

                  }));
            }
        }
        private RelayCommand _deleteClickCommand;

        public RelayCommand DeleteClickCommand
        {
            get
            {
                return _deleteClickCommand ??
                  (_deleteClickCommand = new RelayCommand(obj =>
                  {
                      Highscore = 0;
                      using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
                      {
                          JsonSerializer.Serialize<int>(fs, Highscore);
                      }
                  }));
            }
        }

        private void CellClick(object parameter)
        {
            var cell = parameter as CellViewModel;

            if (cell != null && cell.DisplayText != "открыта") 
            {
                int row = Cells.IndexOf(cell) / Columns;
                int column = Cells.IndexOf(cell) % Columns;

                string cellContent = _zemledelie.OpenCell(row, column);
                cell.DisplayText = cellContent;


                if(_zemledelie.Count == _zemledelie.s - _zemledelie.Mines) 
                {
                    Status = "Game won!";
                }
                if (cellContent == "Mine")
                {
                    Status = "Game over!";
                }
                else if (cellContent == "0")
                {
                    OpenAdjacentCells(row, column);
                }

                CheckGameCompletion();
            }
        }

        private void OpenAdjacentCells(int row, int column)
        {
            OpenCellIfValid(row - 1, column - 1);
            OpenCellIfValid(row - 1, column);
            OpenCellIfValid(row - 1, column + 1);
            OpenCellIfValid(row, column - 1);
            OpenCellIfValid(row, column + 1);
            OpenCellIfValid(row + 1, column - 1);
            OpenCellIfValid(row + 1, column);
            OpenCellIfValid(row + 1, column + 1);
        }

        private void OpenCellIfValid(int row, int column)
        {

            if (((row < _zemledelie.x)&&(row >= 0)) && ((column < _zemledelie.y)&&(column >= 0)))
            {

                string cellContent = _zemledelie.OpenCell(row, column);
                Cells[row * Columns + column].DisplayText = cellContent;

                if (cellContent == "0" && _zemledelie.Values[row, column] == " ")
                {
                    OpenAdjacentCells(row, column);
                }
            }
        }

        private void CheckGameCompletion()
        {

            if(Status == "Game over!")
            {
                if (Highscore < _zemledelie.Score) 
                {
                    Highscore = _zemledelie.Score;
                    using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
                    {
                        JsonSerializer.Serialize<int>(fs, Highscore);
                    }
                }
                MessageBox.Show("ВЗРЫВ" + $"\nВаш счёт: {_zemledelie.Score}" + $"\nЛучший счёт: {Highscore}");
                Application.Current.Shutdown();
            }
            if(Status == "Game won!")
            {
                if (Highscore < _zemledelie.Score)
                {
                    Highscore = _zemledelie.Score;
                    using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
                    {
                        JsonSerializer.Serialize<int>(fs, Highscore);
                    }
                }
                MessageBox.Show("Победа" + $"\nВаш счёт: {_zemledelie.Score}" + $"\nЛучший счёт: {Highscore}");
                Application.Current.Shutdown();
            }
        }

    }
}
