 

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using SS.Platform.OA.Dal;
using SS.Platform.OA.IDal;

namespace SS.Platform.OA.DalFactory
{
    /// <summary>
    /// DbSsesion本质就是一个简单工厂，但从抽象意义来说，
    /// 它就是整个数据访问层的统一入口，
    /// 拿到了Dbssion就能拿到整个数据访问层所有的DAL
    /// 
    /// </summary>
    public partial class DbSession : IDbSession
    {
	
		public IActionInfoDal ActionInfoDal
        {
            get { return new ActionInfoDal(); }
        }
	
		public IBaseEntityDal BaseEntityDal
        {
            get { return new BaseEntityDal(); }
        }
	
		public IControlDal ControlDal
        {
            get { return new ControlDal(); }
        }
	
		public IEmployeeDal EmployeeDal
        {
            get { return new EmployeeDal(); }
        }
	
		public IExperienceDal ExperienceDal
        {
            get { return new ExperienceDal(); }
        }
	
		public IFamilyDal FamilyDal
        {
            get { return new FamilyDal(); }
        }
	
		public IMenuInfoDal MenuInfoDal
        {
            get { return new MenuInfoDal(); }
        }
	
		public IModuleDal ModuleDal
        {
            get { return new ModuleDal(); }
        }
	
		public ITrainDal TrainDal
        {
            get { return new TrainDal(); }
        }
	
		public IUserInfoDal UserInfoDal
        {
            get { return new UserInfoDal(); }
        }
	}
}