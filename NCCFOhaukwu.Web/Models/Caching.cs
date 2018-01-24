using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Web.Caching;
using DAL;
using MySql.BLL;

namespace NCCFOhaukwu.Web.Models
{
    public class Caching
    {
        private static EntityConnectionStringBuilder entityConnectionString = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public static IEnumerable<CorpMember> GetCategoryData()
        {
            IEnumerable<CorpMember> categoryData = HttpContext.Current.Cache.Get("CorpMember") as IEnumerable<CorpMember>;
            if (categoryData == null)
            {
                using (var context = new ApplicationDbContext())
                {
                    IQueryable<CorpMember> categoryDataCache = context.CorpMembers;
                    var query = (DbQuery<CorpMember>) categoryDataCache;
                    using (SqlConnection connection = new SqlConnection(entityConnectionString.ProviderConnectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query.ToString(), connection);
                        SqlCacheDependency dependency = new SqlCacheDependency(command);
                        categoryData = categoryDataCache.ToList();
                        HttpContext.Current.Cache.Insert("CorpMember", categoryData, dependency);
                        command.ExecuteNonQuery();
                    }
                }
            }
            return categoryData;
        }
    }
}