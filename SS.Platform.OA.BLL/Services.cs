 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Platform.OA.Model;

namespace SS.Platform.OA.BLL
{
   
	
	public partial class ActionInfoService : BaseService<ActionInfo>, IBLL.IActionInfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = DbSession.ActionInfoDal;
        }

        #region 批量删除
        /// <summary>
        /// add a method to delete by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteIds(params int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = DbSession.ActionInfoDal.LoadEntities(u => u.ID == id).FirstOrDefault();
                entity.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return DbSession.SaveChanges();
        }


        #endregion

        #region 搜索查询
        public IQueryable<ActionInfo> LoadSearchData(BaseQueryParam param)
        {
            short delNoarml = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            //过滤删除的
            var temp = DbSession.ActionInfoDal.LoadEntities(u => u.DelFlag == delNoarml);			
			//根据编码查询
            if (!string.IsNullOrEmpty(param.SCode))
            {
                temp = temp.Where(u => u.Code.Contains(param.SCode));
            }
            
			//根据名称查询
            if (!string.IsNullOrEmpty(param.SName))
            {
                temp = temp.Where(u => u.Name.Contains(param.SName));
            }

            //设置总条数
            param.Total = temp.Count();

            //分页的处理
            return temp.OrderBy(u => u.ID).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
        }
        #endregion
    }

	
	public partial class BaseEntityService : BaseService<BaseEntity>, IBLL.IBaseEntityService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = DbSession.BaseEntityDal;
        }

        #region 批量删除
        /// <summary>
        /// add a method to delete by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteIds(params int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = DbSession.BaseEntityDal.LoadEntities(u => u.ID == id).FirstOrDefault();
                entity.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return DbSession.SaveChanges();
        }


        #endregion

        #region 搜索查询
        public IQueryable<BaseEntity> LoadSearchData(BaseQueryParam param)
        {
            short delNoarml = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            //过滤删除的
            var temp = DbSession.BaseEntityDal.LoadEntities(u => u.DelFlag == delNoarml);			
			//根据编码查询
            if (!string.IsNullOrEmpty(param.SCode))
            {
                temp = temp.Where(u => u.Code.Contains(param.SCode));
            }
            
			//根据名称查询
            if (!string.IsNullOrEmpty(param.SName))
            {
                temp = temp.Where(u => u.Name.Contains(param.SName));
            }

            //设置总条数
            param.Total = temp.Count();

            //分页的处理
            return temp.OrderBy(u => u.ID).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
        }
        #endregion
    }

	
	public partial class ControlService : BaseService<Control>, IBLL.IControlService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = DbSession.ControlDal;
        }

        #region 批量删除
        /// <summary>
        /// add a method to delete by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteIds(params int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = DbSession.ControlDal.LoadEntities(u => u.ID == id).FirstOrDefault();
                entity.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return DbSession.SaveChanges();
        }


        #endregion

        #region 搜索查询
        public IQueryable<Control> LoadSearchData(BaseQueryParam param)
        {
            short delNoarml = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            //过滤删除的
            var temp = DbSession.ControlDal.LoadEntities(u => u.DelFlag == delNoarml);			
			//根据编码查询
            if (!string.IsNullOrEmpty(param.SCode))
            {
                temp = temp.Where(u => u.Code.Contains(param.SCode));
            }
            
			//根据名称查询
            if (!string.IsNullOrEmpty(param.SName))
            {
                temp = temp.Where(u => u.Name.Contains(param.SName));
            }

            //设置总条数
            param.Total = temp.Count();

            //分页的处理
            return temp.OrderBy(u => u.ID).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
        }
        #endregion
    }

	
	public partial class EmployeeService : BaseService<Employee>, IBLL.IEmployeeService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = DbSession.EmployeeDal;
        }

        #region 批量删除
        /// <summary>
        /// add a method to delete by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteIds(params int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = DbSession.EmployeeDal.LoadEntities(u => u.ID == id).FirstOrDefault();
                entity.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return DbSession.SaveChanges();
        }


        #endregion

        #region 搜索查询
        public IQueryable<Employee> LoadSearchData(BaseQueryParam param)
        {
            short delNoarml = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            //过滤删除的
            var temp = DbSession.EmployeeDal.LoadEntities(u => u.DelFlag == delNoarml);			
			//根据编码查询
            if (!string.IsNullOrEmpty(param.SCode))
            {
                temp = temp.Where(u => u.Code.Contains(param.SCode));
            }
            
			//根据名称查询
            if (!string.IsNullOrEmpty(param.SName))
            {
                temp = temp.Where(u => u.Name.Contains(param.SName));
            }

            //设置总条数
            param.Total = temp.Count();

            //分页的处理
            return temp.OrderBy(u => u.ID).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
        }
        #endregion
    }

	
	public partial class ExperienceService : BaseService<Experience>, IBLL.IExperienceService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = DbSession.ExperienceDal;
        }

        #region 批量删除
        /// <summary>
        /// add a method to delete by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteIds(params int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = DbSession.ExperienceDal.LoadEntities(u => u.ID == id).FirstOrDefault();
                entity.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return DbSession.SaveChanges();
        }


        #endregion

        #region 搜索查询
        public IQueryable<Experience> LoadSearchData(BaseQueryParam param)
        {
            short delNoarml = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            //过滤删除的
            var temp = DbSession.ExperienceDal.LoadEntities(u => u.DelFlag == delNoarml);			
			//根据编码查询
            if (!string.IsNullOrEmpty(param.SCode))
            {
                temp = temp.Where(u => u.Code.Contains(param.SCode));
            }
            
			//根据名称查询
            if (!string.IsNullOrEmpty(param.SName))
            {
                temp = temp.Where(u => u.Name.Contains(param.SName));
            }

            //设置总条数
            param.Total = temp.Count();

            //分页的处理
            return temp.OrderBy(u => u.ID).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
        }
        #endregion
    }

	
	public partial class FamilyService : BaseService<Family>, IBLL.IFamilyService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = DbSession.FamilyDal;
        }

        #region 批量删除
        /// <summary>
        /// add a method to delete by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteIds(params int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = DbSession.FamilyDal.LoadEntities(u => u.ID == id).FirstOrDefault();
                entity.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return DbSession.SaveChanges();
        }


        #endregion

        #region 搜索查询
        public IQueryable<Family> LoadSearchData(BaseQueryParam param)
        {
            short delNoarml = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            //过滤删除的
            var temp = DbSession.FamilyDal.LoadEntities(u => u.DelFlag == delNoarml);			
			//根据编码查询
            if (!string.IsNullOrEmpty(param.SCode))
            {
                temp = temp.Where(u => u.Code.Contains(param.SCode));
            }
            
			//根据名称查询
            if (!string.IsNullOrEmpty(param.SName))
            {
                temp = temp.Where(u => u.Name.Contains(param.SName));
            }

            //设置总条数
            param.Total = temp.Count();

            //分页的处理
            return temp.OrderBy(u => u.ID).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
        }
        #endregion
    }

	
	public partial class MenuInfoService : BaseService<MenuInfo>, IBLL.IMenuInfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = DbSession.MenuInfoDal;
        }

        #region 批量删除
        /// <summary>
        /// add a method to delete by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteIds(params int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = DbSession.MenuInfoDal.LoadEntities(u => u.ID == id).FirstOrDefault();
                entity.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return DbSession.SaveChanges();
        }


        #endregion

        #region 搜索查询
        public IQueryable<MenuInfo> LoadSearchData(BaseQueryParam param)
        {
            short delNoarml = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            //过滤删除的
            var temp = DbSession.MenuInfoDal.LoadEntities(u => u.DelFlag == delNoarml);			
			//根据编码查询
            if (!string.IsNullOrEmpty(param.SCode))
            {
                temp = temp.Where(u => u.Code.Contains(param.SCode));
            }
            
			//根据名称查询
            if (!string.IsNullOrEmpty(param.SName))
            {
                temp = temp.Where(u => u.Name.Contains(param.SName));
            }

            //设置总条数
            param.Total = temp.Count();

            //分页的处理
            return temp.OrderBy(u => u.ID).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
        }
        #endregion
    }

	
	public partial class ModuleService : BaseService<Module>, IBLL.IModuleService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = DbSession.ModuleDal;
        }

        #region 批量删除
        /// <summary>
        /// add a method to delete by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteIds(params int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = DbSession.ModuleDal.LoadEntities(u => u.ID == id).FirstOrDefault();
                entity.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return DbSession.SaveChanges();
        }


        #endregion

        #region 搜索查询
        public IQueryable<Module> LoadSearchData(BaseQueryParam param)
        {
            short delNoarml = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            //过滤删除的
            var temp = DbSession.ModuleDal.LoadEntities(u => u.DelFlag == delNoarml);			
			//根据编码查询
            if (!string.IsNullOrEmpty(param.SCode))
            {
                temp = temp.Where(u => u.Code.Contains(param.SCode));
            }
            
			//根据名称查询
            if (!string.IsNullOrEmpty(param.SName))
            {
                temp = temp.Where(u => u.Name.Contains(param.SName));
            }

            //设置总条数
            param.Total = temp.Count();

            //分页的处理
            return temp.OrderBy(u => u.ID).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
        }
        #endregion
    }

	
	public partial class TrainService : BaseService<Train>, IBLL.ITrainService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = DbSession.TrainDal;
        }

        #region 批量删除
        /// <summary>
        /// add a method to delete by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteIds(params int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = DbSession.TrainDal.LoadEntities(u => u.ID == id).FirstOrDefault();
                entity.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return DbSession.SaveChanges();
        }


        #endregion

        #region 搜索查询
        public IQueryable<Train> LoadSearchData(BaseQueryParam param)
        {
            short delNoarml = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            //过滤删除的
            var temp = DbSession.TrainDal.LoadEntities(u => u.DelFlag == delNoarml);			
			//根据编码查询
            if (!string.IsNullOrEmpty(param.SCode))
            {
                temp = temp.Where(u => u.Code.Contains(param.SCode));
            }
            
			//根据名称查询
            if (!string.IsNullOrEmpty(param.SName))
            {
                temp = temp.Where(u => u.Name.Contains(param.SName));
            }

            //设置总条数
            param.Total = temp.Count();

            //分页的处理
            return temp.OrderBy(u => u.ID).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
        }
        #endregion
    }

	
	public partial class UserInfoService : BaseService<UserInfo>, IBLL.IUserInfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = DbSession.UserInfoDal;
        }

        #region 批量删除
        /// <summary>
        /// add a method to delete by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteIds(params int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = DbSession.UserInfoDal.LoadEntities(u => u.ID == id).FirstOrDefault();
                entity.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return DbSession.SaveChanges();
        }


        #endregion

        #region 搜索查询
        public IQueryable<UserInfo> LoadSearchData(BaseQueryParam param)
        {
            short delNoarml = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            //过滤删除的
            var temp = DbSession.UserInfoDal.LoadEntities(u => u.DelFlag == delNoarml);			
			//根据编码查询
            if (!string.IsNullOrEmpty(param.SCode))
            {
                temp = temp.Where(u => u.Code.Contains(param.SCode));
            }
            
			//根据名称查询
            if (!string.IsNullOrEmpty(param.SName))
            {
                temp = temp.Where(u => u.Name.Contains(param.SName));
            }

            //设置总条数
            param.Total = temp.Count();

            //分页的处理
            return temp.OrderBy(u => u.ID).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
        }
        #endregion
    }

	
}