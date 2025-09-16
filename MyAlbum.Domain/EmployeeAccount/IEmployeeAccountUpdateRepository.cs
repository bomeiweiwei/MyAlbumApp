using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAlbum.Models;
using MyAlbum.Models.Employee;

namespace MyAlbum.Domain.EmployeeAccount
{
    public interface IEmployeeAccountUpdateRepository
    {
        Task<ResponseBase<bool>> UpdateEmployeeAsync(UpdateEmployeeReq req, CancellationToken ct = default);
    }
}
