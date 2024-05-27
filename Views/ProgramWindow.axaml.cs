using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Security.Cryptography;
using System.Text;
using SQLite;
using TIMP.Models;

namespace TIMP.Views;

public partial class ProgramWindow : Window
{

    private readonly SQLiteAsyncConnection _databaseUsers;

    public ProgramWindow()
    {
        InitializeComponent();
        _databaseUsers = new SQLiteAsyncConnection("users.db");
        _databaseUsers.CreateTableAsync<User>().Wait();
    }


    private void AutorizationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        
            var login = LoginTextBox.Text;
            var password = GetHash(PasswordTextBox.Text);
            
            var isLogined = _databaseUsers
                .Table<User>()
                .Where(c => c.Login == login && c.Password == password)
                .FirstOrDefaultAsync().Result;
            
            if (isLogined != null)
            {
                MainWindow mainWindow = new MainWindow(isLogined.Role, isLogined.UserID)
                {
                    WindowState = WindowState.Maximized
                };
                
                mainWindow.Show();
                Close();
            }
            else
            {
                WarningAutorizationTextBlock.IsVisible = true;
            }
    }

    private string GetHash(string line)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(line);
        SHA256Managed hashstring = new SHA256Managed();
        byte[] hash = hashstring.ComputeHash(bytes);
        string hashString = string.Empty;
        foreach (byte x in hash)
        {
            hashString += String.Format("{0:x2}", x);
        }
        return hashString;
    }
}