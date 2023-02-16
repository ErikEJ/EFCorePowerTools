using System;
using System.Collections.Generic;

namespace Ef7Playground.Models;

public partial class Person
{
    public int Id { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }
}
