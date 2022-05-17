using BookApp.DataAccess.Repository.iRepository;
using BookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private AppDbContext _db;

        public CompanyRepository(AppDbContext db) : base(db)
        {
            _db = db;

        }

        public void Update(Company obj)
        {
            var objFromDb = _db.Companies.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.StreetAddress = obj.StreetAddress;
                objFromDb.City = obj.City;
                objFromDb.State = obj.State;
                objFromDb.PostalCode = obj.PostalCode;
                objFromDb.PhoneNumber = obj.PhoneNumber;                  
            }
        }
    }
}
