using SQLite;

namespace TIMP.Models;

public class Marks
{
    [PrimaryKey, AutoIncrement]
    public int Marks_ID { get; set; }
    public string FIO { get; set; }
    public string? d1 { get; set; }
    public string? d2 { get; set; }
    public string? d3 { get; set; }
    public string? d4 { get; set; }
    public string? d5 { get; set; }
    public string? d6 { get; set; }
    public string? d7 { get; set; }
    public string? d8 { get; set; }
    public string? d9 { get; set; }
    public string? d10 { get; set; }
    public string? d11 { get; set; }
    public string? d12 { get; set; }
    public string? d13 { get; set; }
    public string? d14 { get; set; }
    public string? d15 { get; set; }
    public string? d16 { get; set; }
    [Indexed]
    public int Subject_ID { get; set; }
    [Indexed]
    public int studentgr_ID { get; set; }
}