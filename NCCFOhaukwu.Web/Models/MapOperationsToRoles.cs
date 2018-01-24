using System.Collections.Generic;
using System.Web.Mvc;
using DAL;

namespace NCCFOhaukwu.Web.Models
{
    public class MapOperationsToRoles
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string RoleName { get; set; }
        public Operations Operations { get; set; }

        public List<SelectListItem> ResoucesList
        {
            get { return ListOfThings.ResourcesList(); }
        }

        public IEnumerable<SelectListItem> OperationList { get; set; }
    }
}