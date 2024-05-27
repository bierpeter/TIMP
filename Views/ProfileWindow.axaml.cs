using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TIMP.Models;

namespace TIMP.Views;

public partial class ProfileWindow : Window
{
    private static string _role;
    private static Employee? _infoEmployee;
    private static Student? _infoStudent;
    
    public ProfileWindow()
    {
        InitializeComponent();
    }
    public ProfileWindow(string role, Employee? infoEmployee, Student? infoStudent)
    {
        InitializeComponent();
        _role = role;
        _infoEmployee = infoEmployee;
        _infoStudent = infoStudent;
        DataReplace();
    }

    private void DataReplace()
    {
        if (_role is "Student")
        {
            LastNameLabel.Text = _infoStudent?.lastName;
            FirstNameLabel.Text = _infoStudent?.firstName;
            FatherNameLabel.Text = _infoStudent?.fatherName;
            PositionLabel.Text = "Студент";
            MailLabel.Text = _infoStudent?.mail;
            NumberLabel.Text = _infoStudent?.phoneNumber;
            DobLabel.Text = _infoStudent?.DOB;
        }
        else
        {
            LastNameLabel.Text = _infoEmployee?.lastName;
            FirstNameLabel.Text = _infoEmployee?.firstName;
            FatherNameLabel.Text = _infoEmployee?.fatherName;
            PositionLabel.Text = _infoEmployee?.position;
            MailLabel.Text = _infoEmployee?.mail;
            NumberLabel.Text = _infoEmployee?.phoneNumber;
            DobLabel.Text = _infoEmployee?.DOB;
        }
    }
}