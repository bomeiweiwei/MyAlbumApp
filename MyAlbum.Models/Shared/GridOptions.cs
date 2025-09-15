using System;
namespace MyAlbum.Models.Shared
{
    public record GridColumn(
        string Field,          // 對應 JSON 欄位（如 employeeId, fullName）
        string Title,          // 表頭顯示文字
        string? Width = null,  // css width，可空
        string? Format = null  // 內建: "date" | "bool"；或自訂 formatter 名稱
    );

    public class GridOptions
    {
        public string ListUrl { get; set; } = "";              // 你的 GetEmployeeList endpoint
        public string TableId { get; set; } = "gridTable";     // 表格 tbody 容器 id
        public string PagerId { get; set; } = "gridPager";     // 分頁容器 id
        public string TotalId { get; set; } = "gridTotal";     // 總筆數顯示容器 id
        public string? SearchFormId { get; set; } = null;      // 綁定的查詢表單 id（可 null）
        public int PageSize { get; set; } = 10;
        public int[] PageSizes { get; set; } = new[] { 10, 20, 50 };
        public List<GridColumn> Columns { get; set; } = new();
    }
}

