using DAL;
using MySql.BLL.Repository;

namespace MySql.BLL.Service
{
    public class Service : IService
    {
        private Repository<Album> _album;
        private Repository<AlbumPhoto> _albumPhoto;
        private Repository<Batch> _batch;
        private Repository<Carousel> _carousel;
        private Repository<CorpMember> _corpMember;
        private Repository<Event> _event;
        private Repository<Give> _give;
        private Repository<NewHere> _newHere;
        private Repository<News> _news;
        private Repository<OperationToRoles> _operationToRoles;
        private Repository<Portfolio> _portfolio;
        private Repository<Project> _project;
        private Repository<ProjectPicture> _projectPicture;
        private Repository<WhoWeAre> _whoWeAre;
        private Repository<ServiceYear> _year;
        private Repository<Sermon> _sermon;
        private Repository<Articles> _articles;

        public Repository<ServiceYear> Year
        {
            get { return _year ?? new ServiceYearRepository(new ApplicationDbContext()); }
        }

        public Repository<Batch> Batch
        {
            get { return _batch ?? new BatchRepository(new ApplicationDbContext()); }
        }

        public Repository<CorpMember> CorpMember
        {
            get { return _corpMember ?? new CorpMembersRepository(new ApplicationDbContext()); }
        }

        public Repository<Portfolio> Portfolio
        {
            get { return _portfolio ?? new PortfolioRepository(new ApplicationDbContext()); }
        }

        public Repository<Album> Album
        {
            get { return _album ?? new AlbumRepository(new ApplicationDbContext()); }
        }

        public Repository<AlbumPhoto> AlbumPhoto
        {
            get { return _albumPhoto ?? new AlbumPhotoRepository(new ApplicationDbContext()); }
        }

        public Repository<Event> Event
        {
            get { return _event ?? new EventRepository(new ApplicationDbContext()); }
        }

        public Repository<News> News
        {
            get { return _news ?? new NewsRepository(new ApplicationDbContext()); }
        }

        public Repository<Project> Project
        {
            get { return _project ?? new ProjectRepository(new ApplicationDbContext()); }
        }

        public Repository<ProjectPicture> ProjectPicture
        {
            get { return _projectPicture ?? new ProjectPictureRepository(new ApplicationDbContext()); }
        }

        public Repository<OperationToRoles> OperationToRoles
        {
            get { return _operationToRoles ?? new OperationToRolesRepository(new ApplicationDbContext()); }
        }

        public Repository<Give> Give
        {
            get { return _give ?? new GiveRepository(new ApplicationDbContext()); }
        }

        public Repository<Carousel> Carousel
        {
            get { return _carousel ?? new CarouselRepository(new ApplicationDbContext()); }
        }

        public Repository<WhoWeAre> WhoWeAre
        {
            get { return _whoWeAre ?? new WhoWeAreRepository(new ApplicationDbContext()); }
        }

        public Repository<NewHere> NewHere
        {
            get { return _newHere ?? new NewHereRepository(new ApplicationDbContext()); }
        }

        public Repository<Sermon> Sermon
        {
            get { return _sermon ?? new SermonRepository(new ApplicationDbContext()); }
        }

        public Repository<Articles> Articles
        {
            get { return _articles ?? new ArticleRepository(new ApplicationDbContext()); }
        }
    }
}