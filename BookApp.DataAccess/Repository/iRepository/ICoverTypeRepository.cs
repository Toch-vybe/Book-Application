using BookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.DataAccess.Repository.iRepository
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {
        void Update (CoverType obj);
    }
}
