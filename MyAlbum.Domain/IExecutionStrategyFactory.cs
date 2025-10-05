using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAlbum.Domain
{
    public interface IExecutionStrategyFactory
    {
        IExecutionStrategy Create(IAlbumDbContext ctx);
    }
}
