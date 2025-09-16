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
        ExceptionError = 500
    }
}

