using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using SQLite;
using TIMP.Models;

namespace TIMP.Views;

public partial class MainWindow : Window
{
    private static string Role;
    private static long ID;
    private static Employee? InfoEmployee;
    private static Student? InfoStudent;
    
    private readonly SQLiteAsyncConnection _databaseData;
    private readonly SQLiteAsyncConnection _databaseStudentData;
    private readonly SQLiteAsyncConnection _databaseProfileInfo;
    private readonly SQLiteAsyncConnection _databaseMarks;
    

    private static List<Faculty> Faculties = new ();
    private static List<Subject> Subjecties = new ();
    private static List<StudGr> Groups = new ();
    private static List<Marks> MarksList = new();
    private static int StudentGroup = 0;

    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(string? role, long id)
    {
        InitializeComponent();
        _databaseProfileInfo = new SQLiteAsyncConnection("data.db");
        _databaseProfileInfo.CreateTableAsync<Employee>().Wait();
        _databaseProfileInfo.CreateTableAsync<Student>().Wait();
        
        _databaseData = new SQLiteAsyncConnection("data.db");
        _databaseData.CreateTableAsync<Faculty>().Wait();
        _databaseData.CreateTableAsync<EmployeeInFaculty>().Wait();
        _databaseData.CreateTableAsync<Subject>().Wait();
        _databaseData.CreateTableAsync<StudGr>().Wait();
        _databaseData.CreateTableAsync<GrInSubject>().Wait();
        
        _databaseStudentData = new SQLiteAsyncConnection("data.db");
        _databaseStudentData.CreateTableAsync<Student>().Wait();
        _databaseStudentData.CreateTableAsync<StudGr>().Wait();
        _databaseStudentData.CreateTableAsync<StudentsInGr>().Wait();
        _databaseStudentData.CreateTableAsync<Subject>().Wait();
        _databaseStudentData.CreateTableAsync<GrInSubject>().Wait();
        
        _databaseMarks = new SQLiteAsyncConnection("marks.db");
        _databaseMarks.CreateTableAsync<Marks>().Wait();
        Role = role;
        ID = id;
        
        LoadProfileInformation();
        DatePicker.SelectedDate = DateTime.Today;   
    }

    private void LoadFacultyInformation()
    {
        if (Role is "Employee")
        {
            var facultiesInEmployee = _databaseData.Table<EmployeeInFaculty>()
                .Where(c => c.Employee_ID == InfoEmployee.Employee_ID)
                .ToListAsync().Result
                .Select(c => c.Faculty_ID);
            
            var faculties = _databaseData.Table<Faculty>()
                .Where(c => facultiesInEmployee.Contains(c.Faculty_ID))
                .ToListAsync().Result;
            Faculties = faculties;
        }
    }
    private void LoadFacultyToComboBox()
    {
        List<string> facultyName = new List<string>();
        foreach (var faculty in Faculties)
        {
            facultyName.Add(faculty.facultyShortName);
        }
        FacultyComboBox.ItemsSource = facultyName;
    }
    
    private void LoadSubjectInformation()
    {
        if (Role is "Employee")
        {
            int FacultyIdentificator = Faculties[FacultyComboBox.SelectedIndex].Faculty_ID;
            var subjectInEmployee = _databaseData.Table<Subject>()
                .Where(c => c.Employee_ID == InfoEmployee.Employee_ID &&
                            c.Faculty_ID == FacultyIdentificator)
                .ToListAsync().Result;
            Subjecties = subjectInEmployee;
        }
        else
        {
            var group = _databaseStudentData.Table<StudentsInGr>()
                .Where(c => c.Student_ID == InfoStudent.Student_ID)
                .FirstOrDefaultAsync().Result.studentgr_ID;
            StudentGroup = group;
            
            var subjectInGriup = _databaseStudentData.Table<GrInSubject>()
                .Where(c => c.studentgr_ID == group)
                .ToListAsync().Result
                .Select(c => c.Subject_ID);
            
            var subjects = _databaseStudentData.Table<Subject>()
                .Where(c => subjectInGriup.Contains(c.Subject_ID))
                .ToListAsync().Result;
            Subjecties = subjects;
        }
    }
    private void LoadSubjectToComboBox()
    {
        List<string> subjectName = new List<string>();
        foreach (var subject in Subjecties)
        {
            subjectName.Add(subject.subjectShortName);
        }
        SubjectComboBox.ItemsSource = subjectName;
    }
    
    private void LoadGroupInformation()
    {
        if (Role is "Employee")
        {
            int SubjectIdentificator = Subjecties[SubjectComboBox.SelectedIndex].Subject_ID;
            var groupsInSubject = _databaseData.Table<GrInSubject>()
                .Where(c => c.Subject_ID == SubjectIdentificator)
                .ToListAsync().Result
                .Select(c => c.studentgr_ID);
            
            var groups = _databaseData.Table<StudGr>()
                .Where(c => groupsInSubject.Contains(c.studentgr_ID))
                .ToListAsync().Result;
            Groups = groups;
        }
    }
    private void LoadGroupToComboBox()
    {
        List<string> groupName = new List<string>();
        foreach (var group in Groups)
        {
            groupName.Add(group.grNumber + "-" + group.grSubnumber);
        }
        GroupComboBox.ItemsSource = groupName;
    }
    



    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if (Role is "Employee")
        {
            if (!(FacultyComboBox.SelectedIndex == -1
                || SubjectComboBox.SelectedIndex == -1
                || GroupComboBox.SelectedIndex == -1))
            {
                WarningTextBlock.IsVisible = false;
                LoadEmployeeData();
            }
            else
            {
                WarningTextBlock.IsVisible = true;
            }
        }
        else if(Role is "Student")
        {
            if (SubjectComboBox.SelectedIndex != -1)
            {
                WarningTextBlock.IsVisible = false;
                LoadStudentData();
            }
            else
            {
                WarningTextBlock.IsVisible = true;
            }
        }
    }

    public void LoadEmployeeData()
    {
        int a = Subjecties[SubjectComboBox.SelectedIndex].Subject_ID;
        int b = Groups[GroupComboBox.SelectedIndex].studentgr_ID;
        var studMarks = _databaseMarks.Table<Marks>()
            .Where(c => c.Subject_ID == a
                        && c.studentgr_ID == b)
            .ToListAsync().Result;

        MarksList = studMarks;
        ViewDataGrid.ItemsSource = MarksList;
    }

    public void LoadStudentData()
    {
        int a = Subjecties[SubjectComboBox.SelectedIndex].Subject_ID;
        var studMarks = _databaseMarks.Table<Marks>()
            .Where(c => c.Subject_ID == a
                        && c.studentgr_ID == StudentGroup)
            .ToListAsync().Result;

        MarksList = studMarks;
        ViewDataGrid.ItemsSource = MarksList;
    }

    private void Button_OnClick2(object? sender, RoutedEventArgs e)
    {
        var selectedItems = ViewDataGrid.SelectedItems.OfType<Faculty>();
        
        foreach (var faculty in selectedItems)
        {
            var result = _databaseData.DeleteAsync(faculty).Result;
        }
            
        ViewDataGrid.ItemsSource = _databaseData.Table<Faculty>().ToListAsync().Result;
    }

    private void ViewDataGrid_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (Role is "Employee")
        {
            if (ViewDataGrid.SelectedIndex > -1)
            {
                if (ViewDataGrid.CurrentColumn.DisplayIndex > 1)
                {
                    var index = ViewDataGrid.SelectedIndex;
                    var columnDisplayIndex = ViewDataGrid.CurrentColumn.DisplayIndex;
                    string? columnHeader = ViewDataGrid.CurrentColumn.Header.ToString();
                    ChangeMarkWindow changeMarkWindow = new ChangeMarkWindow(MarksList[index],columnDisplayIndex,columnHeader, _databaseMarks)
                    {
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };
                    changeMarkWindow.ShowDialog(this);
                    changeMarkWindow.Closed += (_, _) =>
                    {
                        LoadEmployeeData();
                        LoadButton.Focus();
                    };
                }
                ViewDataGrid.SelectedIndex = -1;      
            }
        }
        else
        {
            ViewDataGrid.SelectedIndex = -1;  
        }
    }
    
    
    private void LoadProfileInformation()
    {
        if (Role is "Employee")
        {
            var isRole = _databaseProfileInfo
                .Table<Employee>()
                .Where(c => c.Employee_ID == ID)
                .FirstOrDefaultAsync().Result;
            InfoEmployee = isRole;

            LoadFacultyInformation();
            LoadFacultyToComboBox();
            
        }
        else
        {
            FacultyStackPanel.IsVisible = false;
            GroupStackPanel.IsVisible = false;
            var isRole = _databaseProfileInfo
                .Table<Student>()
                .Where(c => c.Student_ID == ID)
                .FirstOrDefaultAsync().Result;
            InfoStudent = isRole;
            
            LoadSubjectInformation();
            LoadSubjectToComboBox();
        }
    }

    private void ProfileButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ProfileWindow profileWindow = new ProfileWindow(Role, InfoEmployee ,InfoStudent)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        profileWindow.ShowDialog(this);
    }

    private void FacultyComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        GroupComboBox.ItemsSource = new List<string>();
        LoadSubjectInformation();
        LoadSubjectToComboBox();
    }

    private void SubjectComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (Role is "Employee")
        {
            if (SubjectComboBox.SelectedIndex > -1)
            {
                LoadGroupInformation();
                LoadGroupToComboBox();
            }
        }

    }

    private void ViewDataGrid_OnAutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        DataGridBoundColumn TempColumn;
        string BindingName;

        if (e.Column is DataGridBoundColumn)
        {
            TempColumn = e.Column as DataGridBoundColumn;
            BindingName = (TempColumn.Binding as Binding).Path;
            if (BindingName == "d1")
            {
                TempColumn.Header = "01.05.2024";
            }
            else if (BindingName == "d2")
            {
                TempColumn.Header = "02.05.2024";
            }
            else if (BindingName == "d3")
            {
                TempColumn.Header = "03.05.2024";
            }
            else if (BindingName == "d4")
            {
                TempColumn.Header = "04.05.2024";
            }
            else if (BindingName == "d5")
            {
                TempColumn.Header = "05.05.2024";
            }
            else if (BindingName == "d6")
            {
                TempColumn.Header = "06.05.2024";
            }
            else if (BindingName == "d7")
            {
                TempColumn.Header = "07.05.2024";
            }
            else if (BindingName == "d8")
            {
                TempColumn.Header = "08.05.2024";
            }
            else if (BindingName == "d9")
            {
                TempColumn.Header = "09.05.2024";
            }
            else if (BindingName == "d10")
            {
                TempColumn.Header = "10.05.2024";
            }
            else if (BindingName == "d11")
            {
                TempColumn.Header = "11.05.2024";
            }
            else if (BindingName == "d12")
            {
                TempColumn.Header = "12.05.2024";
            }
            else if (BindingName == "d13")
            {
                TempColumn.Header = "13.05.2024";
            }
            else if (BindingName == "d14")
            {
                TempColumn.Header = "14.05.2024";
            }
            else if (BindingName == "d15")
            {
                TempColumn.Header = "15.05.2024";
            }
            else if (BindingName == "d16")
            {
                TempColumn.Header = "16.05.2024";
            }
            else if (BindingName == "Marks_ID")
            {
                TempColumn.Header = "Номер в системе";
            }
            else if (BindingName == "FIO")
            {
                TempColumn.Header = "ФИО";
            }

            if (BindingName == "Subject_ID")
            {
                TempColumn.IsVisible = false;
            }
            else if (BindingName == "studentgr_ID")
            {
                TempColumn.IsVisible = false;
            }
        }
    }

    private void ReportButton_OnClick(object? sender, RoutedEventArgs e)
    {
        //Отчетность
    }
}