using Core.ORM.Dapper;

namespace News.DAL
{
    /// <summary>
    /// 数据访问层基类
    /// </summary>
    public class RepositoryBase
    {
        protected DapperExtension _dal = new DapperExtension();
    }
}
