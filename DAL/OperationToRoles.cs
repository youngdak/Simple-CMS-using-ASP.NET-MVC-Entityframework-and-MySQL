namespace DAL
{
    public class OperationToRoles : EntityBase
    {
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string RoleName { get; set; }
        public Operations Operations { get; set; }
        public string SearchTag { get; set; }
    }
}