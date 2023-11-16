using Saper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Saper.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private string _selectedDifficulty;
        private ObservableCollection<CellViewModel> _cells;
        private string _status;

        private Zemledelie _zemledelie;

        public MainViewModel()
        {
            DifficultyLevels = new ObservableCollection<string> { "Easy", "Medium", "Hard" };
            SelectedDifficulty = DifficultyLevels[0]; // Выберите сложность по умолчанию
            InitializeGame();
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
            // Создаем экземпляр Zemledelie с использованием выбранной сложности
            _zemledelie = new Zemledelie(GetRows(), GetColumns());

            // Инициализируем ячейки в соответствии с Zemledelie
            Cells = new ObservableCollection<CellViewModel>();
            Columns = _zemledelie.y;
            Rows = _zemledelie.x;
            for (int i = 1; i <= _zemledelie.x; i++)
            {
                for (int j = 1; j <= _zemledelie.y; j++)
                {
                    string cellContent = _zemledelie.Pole[i, j]; // Получаем содержимое ячейки из Zemledelie
                    Cells.Add(new CellViewModel { DisplayText = cellContent });
                }
            }

            // Дополнительная логика инициализации статуса и других параметров, если необходимо
            Status = "Game in progress";
        }

        private int GetRows()
        {
            // Вернуть количество строк в соответствии с выбранной сложностью
            // Например, для каждого уровня сложности может быть свое количество строк
            // Замените этот код на свой
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
            // Вернуть количество столбцов в соответствии с выбранной сложностью
            // Замените этот код на свой
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

        private void CellClick(object parameter)
        {
            // Обработка события клика по ячейке
            // В параметре будет передаваться информация о ячейке (например, координаты)

            // Пример:
            // var cellInfo = (CellInfo)parameter;
            // int row = cellInfo.Row;
            // int column = cellInfo.Column;

            // Ваш код обработки клика по ячейке
        }

    }
}
