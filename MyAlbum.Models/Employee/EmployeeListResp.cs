using System;
namespace MyAlbum.Models.Employee
{
	public class EmployeeListResp
	{
        public long Total { get; set; }
        public IEnumerable<EmployeeItem> Items { get; set; } = Enumerable.Empty<EmployeeItem>();
    }
}

