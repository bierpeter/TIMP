using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SQLite;
using TIMP.Models;

namespace TIMP.Views;

public partial class ChangeMarkWindow : Window
{
    private Marks _marks;
    private string _columnHeader;
    private int _columnIndex;
    private List<string> listOfMarks = new() {"Н","2","3","4","5"};
    private readonly SQLiteAsyncConnection _databaseMarks;
    
    public ChangeMarkWindow()
    {
        InitializeComponent();
        
    }
    
    public ChangeMarkWindow(Marks mark, int columnIndex, string? columnHeader, SQLiteAsyncConnection databaseMarks)
    {
        InitializeComponent();
        _marks = mark;
        _columnHeader = columnHeader;
        _columnIndex = columnIndex;
        LoadInformation();
        _databaseMarks = databaseMarks;
    }

    public void LoadInformation()
    {
        FioTextBlock.Text = _marks.FIO;
        DateTextBlock.Text = _columnHeader;
        
        ComboBox.ItemsSource = listOfMarks;
        var res = _columnIndex switch
        {
            2 => _marks.d1,
            3 => _marks.d2,
            4 => _marks.d3,
            5 => _marks.d4,
            6 => _marks.d5,
            7 => _marks.d6,
            8 => _marks.d7,
            9 => _marks.d8,
            10 => _marks.d9,
            11 => _marks.d10,
            12 => _marks.d11,
            13 => _marks.d12,
            14 => _marks.d13,
            15 => _marks.d14,
            16 => _marks.d15,
            17 => _marks.d16,
            _ => ""
        };
        ComboBox.SelectedIndex = res switch
        {
            "" => -1,
            "Н" => 0,
            "2" => 1,
            "3" => 2,
            "4" => 3,
            "5" => 4,
            _ => ComboBox.SelectedIndex
        };
        
    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (ComboBox.SelectedIndex != -1)
        {
            var res = ComboBox.SelectedItem?.ToString();
            if (_columnIndex == 2)
            {
                _marks.d1 = res;
            }
            else if (_columnIndex == 3)
            {
                _marks.d2 = res;
            }
            else if (_columnIndex == 4)
            {
                _marks.d3 = res;
            }
            else if (_columnIndex == 5)
            {
                _marks.d4 = res;
            }
            else if (_columnIndex == 6)
            {
                _marks.d5 = res;
            }
            else if (_columnIndex == 7)
            {
                _marks.d6 = res;
            }
            else if (_columnIndex == 8)
            {
                _marks.d7 = res;
            }
            else if (_columnIndex == 9)
            {
                _marks.d8 = res;
            }
            else if (_columnIndex == 10)
            {
                _marks.d9 = res;
            }
            else if (_columnIndex == 11)
            {
                _marks.d10 = res;
            }
            else if (_columnIndex == 12)
            {
                _marks.d11 = res;
            }
            else if (_columnIndex == 13)
            {
                _marks.d12 = res;
            }
            else if (_columnIndex == 14)
            {
                _marks.d13 = res;
            }
            else if (_columnIndex == 15)
            {
                _marks.d14 = res;
            }
            else if (_columnIndex == 16)
            {
                _marks.d15 = res;
            }
            else if (_columnIndex == 17)
            {
                _marks.d16 = res;
            }
            var result1 = _databaseMarks.InsertOrReplaceAsync(_marks).Result;
            Close();
        }
    }
}