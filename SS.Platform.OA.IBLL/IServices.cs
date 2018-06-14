 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Platform.OA.Model;

namespace SS.Platform.OA.IBLL
{
   
	
	public partial interface IActionInfoService : IBaseService<ActionInfo>
    {
		int DeleteIds(params int[] ids);

        IQueryable<ActionInfo> LoadSearchData(Model.BaseQueryParam param);
    }
	
	public partial interface IBaseEntityService : IBaseService<BaseEntity>
    {
		int DeleteIds(params int[] ids);

        IQueryable<BaseEntity> LoadSearchData(Model.BaseQueryParam param);
    }
	
	public partial interface IControlService : IBaseService<Control>
    {
		int DeleteIds(params int[] ids);

        IQueryable<Control> LoadSearchData(Model.BaseQueryParam param);
    }
	
	public partial interface IEmployeeService : IBaseService<Employee>
    {
		int DeleteIds(params int[] ids);

        IQueryable<Employee> LoadSearchData(Model.BaseQueryParam param);
    }
	
	public partial interface IExperienceService : IBaseService<Experience>
    {
		int DeleteIds(params int[] ids);

        IQueryable<Experience> LoadSearchData(Model.BaseQueryParam param);
    }
	
	public partial interface IFamilyService : IBaseService<Family>
    {
		int DeleteIds(params int[] ids);

        IQueryable<Family> LoadSearchData(Model.BaseQueryParam param);
    }
	
	public partial interface IMenuInfoService : IBaseService<MenuInfo>
    {
		int DeleteIds(params int[] ids);

        IQueryable<MenuInfo> LoadSearchData(Model.BaseQueryParam param);
    }
	
	public partial interface IModuleService : IBaseService<Module>
    {
		int DeleteIds(params int[] ids);

        IQueryable<Module> LoadSearchData(Model.BaseQueryParam param);
    }
	
	public partial interface ITrainService : IBaseService<Train>
    {
		int DeleteIds(params int[] ids);

        IQueryable<Train> LoadSearchData(Model.BaseQueryParam param);
    }
	
	public partial interface IUserInfoService : IBaseService<UserInfo>
    {
		int DeleteIds(params int[] ids);

        IQueryable<UserInfo> LoadSearchData(Model.BaseQueryParam param);
    }
	
}