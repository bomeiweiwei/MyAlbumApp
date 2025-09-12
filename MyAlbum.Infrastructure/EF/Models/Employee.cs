using System;
using System.Collections.Generic;

namespace MyAlbum.Infrastructure.EF.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public Guid AccountId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Title { get; set; }

    public DateOnly? HireDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<EmployeeRole> EmployeeRoles { get; set; } = new List<EmployeeRole>();
}
