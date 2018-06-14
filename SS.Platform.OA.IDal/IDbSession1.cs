 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Platform.OA.IDal
{
    public partial interface IDbSession
    {
	
		IDal.IActionInfoDal ActionInfoDal { get; }
	
		IDal.IBaseEntityDal BaseEntityDal { get; }
	
		IDal.IControlDal ControlDal { get; }
	
		IDal.IEmployeeDal EmployeeDal { get; }
	
		IDal.IExperienceDal ExperienceDal { get; }
	
		IDal.IFamilyDal FamilyDal { get; }
	
		IDal.IMenuInfoDal MenuInfoDal { get; }
	
		IDal.IModuleDal ModuleDal { get; }
	
		IDal.ITrainDal TrainDal { get; }
	
		IDal.IUserInfoDal UserInfoDal { get; }
	}
}