using DAL;
using MySql.BLL.Repository;

namespace MySql.BLL.Service
{
    public interface IService
    {
        Repository<ServiceYear> Year { get; }
        Repository<Batch> Batch { get; }
        Repository<CorpMember> CorpMember { get; }
        Repository<Portfolio> Portfolio { get; }
        Repository<Album> Album { get; }
        Repository<AlbumPhoto> AlbumPhoto { get; }
        Repository<Event> Event { get; }
        Repository<News> News { get; }
        Repository<Project> Project { get; }
        Repository<ProjectPicture> ProjectPicture { get; }
        Repository<OperationToRoles> OperationToRoles { get; }
        Repository<Give> Give { get; }
        Repository<Carousel> Carousel { get; }
        Repository<WhoWeAre> WhoWeAre { get; }
        Repository<NewHere> NewHere { get; }
        Repository<Sermon> Sermon { get; }
        Repository<Articles> Articles { get; }
    }
}