using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.EmployeeAccount;
using MyAlbum.Models;
using MyAlbum.Models.Employee;
using MyAlbum.Shared.Enums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAlbum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeAccountReadService _employeeAccountReadService;
        public EmployeeController(IEmployeeAccountReadService employeeAccountReadService)
        {
            _employeeAccountReadService = employeeAccountReadService;
        }

        public IActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetEmployeeList([FromBody] PageRequestBase<EmployeeListReq> req)
        {
            var resp = await _employeeAccountReadService.GetEmployeeListAsync(req);
            if (resp == null || resp.StatusCode != (long)ReturnCode.Succeeded) return NotFound();
            return Ok(new { items = resp.Data, total = resp.Count });
        }
    }
}

