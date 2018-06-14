
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Platform.OA.Model;

namespace SS.Platform.OA.BLL
{
    public partial class RoleService:BaseService<Role>,IBLL.IRoleService
    {
        public RoleService()
        {
            //CurrentDal = DbSession.RoleDal;
        }

        #region 已在T4生成代码文件中包含
        //public override void SetCurrentDal()
        //{
        //    CurrentDal = DbSession.RoleDal;
        //}
        #endregion
    }
}
