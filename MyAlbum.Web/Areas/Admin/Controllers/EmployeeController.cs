using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.EmployeeAccount;
using MyAlbum.Models;
using MyAlbum.Models.Employee;
using MyAlbum.Models.ViewModel;
using MyAlbum.Models.ViewModel.Employee;
using MyAlbum.Shared.Enums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAlbum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeAccountReadService _employeeAccountReadService;
        private readonly IAntiforgery _antiforgery;
        public EmployeeController(IEmployeeAccountReadService employeeAccountReadService, IAntiforgery antiforgery)
        {
            _employeeAccountReadService = employeeAccountReadService;
            _antiforgery = antiforgery;
        }

        public IActionResult Index() => View();

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = "perm:Employee.Read")]
        public async Task<IActionResult> IndexPartial([FromBody] PageRequestBase<EmployeeListReq> req)
        {
            var resp = await _employeeAccountReadService.GetEmployeeListAsync(req);
            if (resp == null || resp.StatusCode != (long)ReturnCode.Succeeded) return NotFound();

            var vm = new EmployeeListViewModel
            {
                Items = resp.Data,

                CanRead = User.HasClaim("perm", "Employee.Read"),
                CanUpdate = User.HasClaim("perm", "Employee.Write"),
                CanDelete = User.HasClaim("perm", "Employee.Delete"),

                Pagination = new PaginationViewModel
                {
                    PageIndex = req.PageIndex,
                    PageSize = req.PageSize,
                    Total = resp.Count
                }
            };
            return PartialView("_EmployeeTable", vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = "perm:Employee.Read")]
        public async Task<IActionResult> GetEmployeeList([FromBody] PageRequestBase<EmployeeListReq> req)
        {
            var resp = await _employeeAccountReadService.GetEmployeeListAsync(req);
            if (resp == null || resp.StatusCode != (long)ReturnCode.Succeeded) return NotFound();
            return Ok(new { items = resp.Data, total = resp.Count });
        }

        [HttpGet]
        [Authorize(Policy = "perm:Employee.Read")]
        public async Task<IActionResult> DetailsPartial(int id)
        {
            var req = new GetEmployeeReq { EmployeeId = id };
            var resp = await _employeeAccountReadService.GetEmployeeDataByIdAsync(req);
            if (resp == null || resp.StatusCode != (long)ReturnCode.Succeeded) return NotFound();

            // 直接把 EmployeeDto 丟給 Partial
            return PartialView("_EmployeeDetails", resp.Data);
        }

        [HttpGet]
        [Authorize(Policy = "perm:Employee.Read")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            GetEmployeeReq req = new GetEmployeeReq() { EmployeeId = id };
            var resp = await _employeeAccountReadService.GetEmployeeDataByIdAsync(req);
            if (resp == null || resp.StatusCode != (long)ReturnCode.Succeeded) return NotFound();
            return Ok(resp.Data);
        }

        /*
            [HttpPost, ValidateAntiForgeryToken]
            [Authorize(Policy = "perm:Employee.Write")]
            public async Task<IActionResult> Update([FromBody] EmployeeUpdateReq req) { ... }

            [HttpPost, ValidateAntiForgeryToken]
            [Authorize(Policy = "perm:Employee.Delete")]
            public async Task<IActionResult> Delete([FromBody] IdReq req) { ... }
         */
    }
}

