using System;
using System.ComponentModel;

namespace MyAlbum.Shared.Enums
{
	public enum ReturnCode
	{
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Succeeded = 200,
        /// <summary>
        /// 資料不存在
        /// </summary>
        [Description("資料不存在")]
        DataNotFound = 404,
        /// <summary>
        /// 異常錯誤
        /// </summary>
        [Description("異常錯誤")]
        ExceptionError = 500,
        /// <summary>
        /// 邏輯錯誤
        /// </summary>
        [Description("檢查錯誤")]
        BusinessError = 9001,
        /// <summary>
        /// 資料異動錯誤
        /// </summary>
        [Description("資料異動錯誤")]
        DbUpdateError = 9002
    }
}

