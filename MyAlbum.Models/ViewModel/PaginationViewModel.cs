using System;
namespace MyAlbum.Models.ViewModel
{
	public class PaginationViewModel
	{
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long Total { get; set; }
    }
}

