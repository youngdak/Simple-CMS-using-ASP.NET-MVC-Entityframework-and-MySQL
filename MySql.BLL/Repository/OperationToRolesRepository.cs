using DAL;

namespace MySql.BLL.Repository
{
    internal class OperationToRolesRepository : Repository<OperationToRoles>
    {
        public OperationToRolesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}