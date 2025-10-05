﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAlbum.Domain
{
    public interface IExecutionStrategy
    {
        Task ExecuteAsync(Func<Task> operation, CancellationToken ct = default);
    }
}
