using System.Linq;
using DAL;
using MySql.BLL.Service;

namespace NCCFOhaukwu.Web.Models
{
    public class ResourceService
    {
        private static readonly IService Service;

        static ResourceService()
        {
            Service = new Service();
        }

        public static bool Authorize(int resourceId, Operations operations, string[] roles)
        {
            return (from role in roles
                let items = Service.OperationToRoles.Get().ToList()
                select
                    items.Count(
                        o => o.RoleName == role && o.ResourceId == resourceId && (o.Operations & operations) != 0)).Any(
                            count => count > 0);
        }

        public void MapOperationToRole(int resourceId, string resourceName, Operations operations, string role)
        {
            var o2R = new OperationToRoles
            {
                Operations = operations,
                ResourceId = resourceId,
                ResourceName = resourceName,
                RoleName = role
            };

            Service.OperationToRoles.Add(o2R);
        }

        public static int ResourceId(string resourceName)
        {
            var resourceId = 0;
            switch (resourceName)
            {
                case "Album":
                    resourceId = Resources.Album;
                    break;
                case "AlbumPhoto":
                    resourceId = Resources.AlbumPhoto;
                    break;
                case "Batch":
                    resourceId = Resources.Batch;
                    break;
                case "Roles":
                    resourceId = Resources.Roles;
                    break;
                case "Users":
                    resourceId = Resources.Users;
                    break;
                case "News":
                    resourceId = Resources.News;
                    break;
                case "Manage":
                    resourceId = Resources.Manage;
                    break;
                case "ResourceOperation":
                    resourceId = Resources.ResourceOperation;
                    break;
                case "CorpMember":
                    resourceId = Resources.CorpMember;
                    break;
                case "Portfolio":
                    resourceId = Resources.Portfolio;
                    break;
                case "ServiceYear":
                    resourceId = Resources.ServiceYear;
                    break;
                case "Event":
                    resourceId = Resources.Event;
                    break;
                case "ProjectPicture":
                    resourceId = Resources.ProjectPicture;
                    break;
                case "Project":
                    resourceId = Resources.Project;
                    break;
                case "Give":
                    resourceId = Resources.Give;
                    break;
                case "Carousel":
                    resourceId = Resources.Carousel;
                    break;
                case "WhoWeAre":
                    resourceId = Resources.WhoWeAre;
                    break;
                case "NewHere":
                    resourceId = Resources.NewHere;
                    break;
                case "Sermon":
                    resourceId = Resources.Sermon;
                    break;
                case "Article":
                    resourceId = Resources.Article;
                    break;
            }
            return resourceId;
        }
    }
}