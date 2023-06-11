using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Context
{
    public interface IDatabase
    {
        Task Setup();
    }
}
