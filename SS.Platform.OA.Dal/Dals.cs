 

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using SS.Platform.OA.IDal; 
using SS.Platform.OA.Model;

namespace SS.Platform.OA.Dal 
{
   
	
	public partial class ActionInfoDal : BaseDal<ActionInfo>, IActionInfoDal 
    {
    }
	
	public partial class BaseEntityDal : BaseDal<BaseEntity>, IBaseEntityDal 
    {
    }
	
	public partial class ControlDal : BaseDal<Control>, IControlDal 
    {
    }
	
	public partial class EmployeeDal : BaseDal<Employee>, IEmployeeDal 
    {
    }
	
	public partial class ExperienceDal : BaseDal<Experience>, IExperienceDal 
    {
    }
	
	public partial class FamilyDal : BaseDal<Family>, IFamilyDal 
    {
    }
	
	public partial class MenuInfoDal : BaseDal<MenuInfo>, IMenuInfoDal 
    {
    }
	
	public partial class ModuleDal : BaseDal<Module>, IModuleDal 
    {
    }
	
	public partial class TrainDal : BaseDal<Train>, ITrainDal 
    {
    }
	
	public partial class UserInfoDal : BaseDal<UserInfo>, IUserInfoDal 
    {
    }
	
}