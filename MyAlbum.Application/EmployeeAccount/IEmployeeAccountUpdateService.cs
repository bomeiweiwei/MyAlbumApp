using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAlbum.Models;
using MyAlbum.Models.Employee;

namespace MyAlbum.Application.EmployeeAccount
{
    public interface IEmployeeAccountUpdateService
    {
        Task<ResponseBase<bool>> UpdateEmployee(UpdateEmployeeReq req, CancellationToken ct = default);

        Task<ResponseBase<bool>> UpdateEmployeeActive(UpdateEmployeeActiveReq req, CancellationToken ct = default);
    }
}
