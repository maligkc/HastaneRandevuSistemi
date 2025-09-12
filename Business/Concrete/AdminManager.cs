using Business.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;

namespace Business.Concrete
{
    public class AdminManager : IAdminService
    {
        private readonly IAdminDal _adminDal;

        public AdminManager(IAdminDal adminDal)
        {
            _adminDal = adminDal;
        }

        public Admin GetByEmailAndPassword(string email, string password)
        {
            return _adminDal.Get(p=> p.AdminEmail == email && p.AdminPassword == password);
        }
    }
}
