using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using System.Web.Configuration;
using System.Collections.Generic;

namespace MySql.BLL {
    public class ApplicationUser : IdentityUser {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    //[DbConfigurationType(typeof(MySqlConfiguration))]
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext()
            : base("DefaultConnection") {
            Database.SetInitializer(new ApplicationDbInitializer());
        }

        public DbSet<ServiceYear> ServiceYears { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<CorpMember> CorpMembers { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<AlbumPhoto> AlbumPhotos { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectPicture> ProjectPictures { get; set; }
        public DbSet<OperationToRoles> OperationToRoles { get; set; }
        public DbSet<Give> Gives { get; set; }
        public DbSet<Carousel> Carousels { get; set; }
        public DbSet<WhoWeAre> WhoWeAre { get; set; }
        public DbSet<NewHere> NewHere { get; set; }
        public DbSet<Sermon> Sermon { get; set; }
        public DbSet<Articles> Articles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<IdentityRole>()
                .Property(c => c.Name).HasMaxLength(128).IsRequired();

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("AspNetUsers") //I have to declare the table name, otherwise IdentityUser will be created
                .Property(c => c.UserName).HasMaxLength(128).IsRequired();
        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }
    }



    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext> {
        protected override void Seed(ApplicationDbContext context) {
            InitializeIdentityForEf(context);
            base.Seed(context);
        }

        private void InitializeIdentityForEf(ApplicationDbContext context) {
            var service = new MySql.BLL.Service.Service();
            service.OperationToRoles.Add(new OperationToRoles {
                Operations = Operations.All,
                ResourceId = Resources.ResourceOperation,
                RoleName = "Administrator",
                ResourceName = "ResourceOperation"
            });
            
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var email = WebConfigurationManager.AppSettings["email"];
            var username = WebConfigurationManager.AppSettings["email"];
            var password = WebConfigurationManager.AppSettings["access"];
            var roleName = WebConfigurationManager.AppSettings["role"];

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null) {
                role = new IdentityRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(username);
            if (user == null) {
                user = new ApplicationUser { UserName = username, Email = email };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name)) {
                var result = userManager.AddToRole(user.Id, role.Name);
            }

            //var role = Task.Run(() => context.Roles.FirstOrDefaultAsync(x => x.Name == roleName)).Result;
            //if (role == null) {
            //    role = new IdentityRole(roleName);
            //    context.Roles.Add(role);
            //    context.SaveChanges();
            //}

            //var user = Task.Run(() => context.Users.FirstOrDefaultAsync(x => x.UserName == username)).Result;
            //if (user == null) {
            //    user = new ApplicationUser { UserName = username, Email = email, PasswordHash =  };
            //    var result = context.Users.Add(user);
            //    context.SaveChanges();
            //}

            //// Add user admin to Role Admin if not already added
            //user.Roles.Add(new IdentityUserRole {
            //    RoleId = role.Id,
            //    UserId = user.Id
            //});
            //context.SaveChanges();
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role
        //public static void InitializeIdentityForEf(ApplicationDbContext db)
        //{
        //    var service = new Service();
        //    service.OperationToRoles.Add(new OperationToRoles
        //    {
        //        Operations = Operations.All,
        //        ResourceId = Resources.ResourceOperation,
        //        RoleName = "Administrator",
        //        ResourceName = "ResourceOperation"
        //    });

        //    var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
        //    var email = WebConfigurationManager.AppSettings["email"];
        //    var username = WebConfigurationManager.AppSettings["username"];
        //    var password = WebConfigurationManager.AppSettings["access"];
        //    var roleName = WebConfigurationManager.AppSettings["role"];

        //    //Create Role Admin if it does not exist
        //    var role = roleManager.FindByName(roleName);
        //    if (role == null)
        //    {
        //        role = new IdentityRole(roleName);
        //        var roleresult = roleManager.Create(role);
        //    }

        //    var user = userManager.FindByName(username);
        //    if (user == null)
        //    {
        //        user = new ApplicationUser {UserName = username, Email = email};
        //        var result = userManager.Create(user, password);
        //        result = userManager.SetLockoutEnabled(user.Id, false);
        //    }

        //    // Add user admin to Role Admin if not already added
        //    var rolesForUser = userManager.GetRoles(user.Id);
        //    if (!rolesForUser.Contains(role.Name))
        //    {
        //        var result = userManager.AddToRole(user.Id, role.Name);
        //    }
        //}
    }
}