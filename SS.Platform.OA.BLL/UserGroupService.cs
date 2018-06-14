using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS.Platform.OA.IBLL;
using SS.Platform.OA.Model;

namespace SS.Platform.OA.BLL
{
    public partial class UserGroupService : BaseService<UserGroup>, IUserGroupService
    {

        private string _Name = DateTime.Now.ToString();

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        #region 批量删除
        /// <summary>
        /// addeb by  laoma
        /// add a method to delete userGroups by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteIds(params int[] ids)
        {
            foreach (var id in ids)
            {
                var userGroup = DbSession.UserGroupDal.LoadEntities(u => u.ID == id).FirstOrDefault();
                userGroup.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return DbSession.SaveChanges();
        }


        #endregion

        #region 搜索查询
        public IQueryable<UserGroup> LoadSearchData(BaseQueryParam param)
        {
            //首先第一步先进行过滤

            short delNoarml = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            //过滤删除的
            var temp = DbSession.UserGroupDal.LoadEntities(u => u.DelFlag == delNoarml);

            //过滤用户组名搜索的
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
