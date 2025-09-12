using System;
using System.ComponentModel;

namespace MyAlbum.Shared.Enums
{
	public enum LoginUserType
	{
        /// <summary>
        /// Member
        /// </summary>
        [Description("Member")]
        Member = 0,
        /// <summary>
        /// Employee
        /// </summary>
        [Description("Employee")]
        Employee = 1,
    }
}

