using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAlbum.Domain;
using MyAlbum.Domain.EmpRole;
using MyAlbum.Domain.RolePerm;
using MyAlbum.Models;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;

namespace MyAlbum.Application.RolePerm.implement
{
    public class EmpRolePermissionService: BaseService, IEmpRolePermissionService
    {
        private readonly IEmployeeRoleReadRepository _employeeRoleReadRepository;
        private readonly IRolePermissionReadRepository _rolePermissionReadRepository;
        public EmpRolePermissionService(IAlbumDbContextFactory factory, IEmployeeRoleReadRepository employeeRoleReadRepository, IRolePermissionReadRepository rolePermissionReadRepository) : base(factory)
        {
            _employeeRoleReadRepository = employeeRoleReadRepository;
            _rolePermissionReadRepository = rolePermissionReadRepository;
        }
        /// <summary>
        /// 取得員工角色權限代碼
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ResponseBase<List<string>>> GetEmpRolePermissionCodesAsync(int empId, CancellationToken ct = default)
        {
            var result = new ResponseBase<List<string>>()
            {
                Data = new List<string>()
            };

            var empRoleResp = await _employeeRoleReadRepository.GetEmployeeRolesAsync(empId, ct);
            if (empRoleResp == null || empRoleResp.StatusCode != (long)ReturnCode.Succeeded)
            {
                result.StatusCode = (long)ReturnCode.DataNotFound;
                result.Message = ReturnCode.DataNotFound.GetDescription();
                return result;
            }
            var roleIds = empRoleResp.Data;
            if (roleIds.Count > 0)
            {
                var rolePermResp = await _rolePermissionReadRepository.GetRolePermissionCodesAsync(roleIds, ct);
                if (rolePermResp == null || rolePermResp.StatusCode != (long)ReturnCode.Succeeded)
                {
                    result.StatusCode = (long)ReturnCode.DataNotFound;
                    result.Message = ReturnCode.DataNotFound.GetDescription();
                    return result;
                }
                result.Data = rolePermResp.Data.Distinct().ToList();
            }

            return result;
        }
    }
}
