using System;
using MyAlbum.Models.Employee;

namespace MyAlbum.Models.ViewModel.Employee
{
	public class EmployeeListViewModel
	{
        public IEnumerable<EmployeeItem> Items { get; set; } = Enumerable.Empty<EmployeeItem>();

        // 權限
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }

        // 分頁只留這一份
        public PaginationViewModel Pagination { get; set; } = new();
    }
}

