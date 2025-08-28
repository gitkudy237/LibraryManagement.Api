﻿namespace LibraryManagement.Core.Models;

public class Borrower
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public ICollection<Borrowing> Borrowings { get; set; } = [];
}
