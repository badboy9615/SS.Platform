using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS.Platform.OA.IDal
{
    public partial interface IDbSession
    {
        //IDal.IRoleDal RoleDal { get; }
        //IDal.IUserInfoDal UserInfoDal { get;}
        int SaveChanges();
    }
}
