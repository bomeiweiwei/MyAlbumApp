using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAlbum.Models;
using MyAlbum.Models.Employee;
using MyAlbum.Models.Member;

namespace MyAlbum.Application.MemberAccount
{
    public interface IMemberAccountWriteService
    {
        Task<ResponseBase<int>> CreateMemberWithAccountAsync(CreateMemberReq req, CancellationToken ct = default);
    }
}
