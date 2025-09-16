using System;
using MyAlbum.Models.Employee;

namespace MyAlbum.Models.ViewModel.Employee
{
	public class EmployeeListViewModel
	{
        public IEnumerable<EmployeeItem> Items { get; set; } = Enumerable.Empty<EmployeeItem>();
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long Total { get; set; }
        public string? FullName { get; set; }

        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }
}

