using BookApp.DataAccess.Repository.iRepository;
using BookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.DataAccess.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private AppDbContext _db;

        public CoverTypeRepository(AppDbContext db) : base(db)  
        {
            _db = db;

        }

        public void Update(CoverType obj)
        {
            _db.CoverTypes.Update(obj);
        }
    }
}
