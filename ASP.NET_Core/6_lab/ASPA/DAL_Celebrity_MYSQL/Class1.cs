using Microsoft.EntityFrameworkCore;

namespace DAL_Celebrity
{
    public interface IRepository : DAL_Celebrity.IRepository<Celebrity, Lifeevent> {}
    public class Repository : IRepository
    {
        Context context;
        private IRepository _repositoryImplementation;

        public Repository()
        {
            this.context = new Context();
        }

        public Repository(string connectionstring)
        {
            this.context = new Context(connectionstring);
        }

        public static IRepository Create()
        {
            return new Repository();
        }

        public static IRepository Create(string connectionstring)
        {
            return new Repository(connectionstring);
        }

        public List<Celebrity> GetAllCelebrities()
        {
            return this.context.Celebrities.ToList<Celebrity>();
        }

        public Celebrity? GetCelebrityById(int Id)
        {
            return this.context.Celebrities.Find(Id);
        }

        public bool AddCelebrity(Celebrity celebrity)
        {
            this.context.Celebrities.Add(celebrity);
            return this.context.SaveChanges() > 0;
        }

        public bool DelCelebrity(int id)
        {
            var c = GetCelebrityById(id);
            if (c != null)
            {
                this.context.Celebrities.Remove(c);
                return this.context.SaveChanges() > 0;
            }

            return false;
        }

        public bool UpdCelebrity(int id, Celebrity celebrity)
        {
            var c = GetCelebrityById(id);
            if (c != null)
            {
                c.Update(celebrity);
                return this.context.SaveChanges() > 0;
            }

            return false;
        }

        public List<Lifeevent> GetAllLifeevents()
        {
            return this.context.Lifeevents.ToList<Lifeevent>();
        }

        public Lifeevent? GetLifeeventById(int Id)
        {
            return this.context.Lifeevents.Find(Id);
        }

        public bool AddLifeevent(Lifeevent lifeevent)
        {
            this.context.Lifeevents.Add(lifeevent);
            return this.context.SaveChanges() > 0;
        }

        public bool DelLifeevent(int id)
        {
            var l = GetLifeeventById(id);
            if (l != null)
            {
                this.context.Lifeevents.Remove(l);
                return this.context.SaveChanges() > 0;
            }

            return false;
        }

        public bool UpdLifeevent(int id, Lifeevent lifeevent)
        {
            var l = GetLifeeventById(id);
            if (l != null)
            {
                l.Update(lifeevent);
                return this.context.SaveChanges() > 0;
            }

            return false;
        }

        public List<Lifeevent> GetLifeeventsByCelebrityId(int celebrityId)
        {
            return this.context.Lifeevents.Where(l => l.CelebrityId == celebrityId).ToList();
        }

        public Celebrity? GetCelebrityByLifeeventId(int lifeeventId)
        {
            var l = GetLifeeventById(lifeeventId);
            if (l != null) return GetCelebrityById(l.CelebrityId);
            return null;
        }

        public int GetCelebrityIdByName(string name)
        {
            var c = this.context.Celebrities.FirstOrDefault(c => c.FullName.Contains(name));
            return c?.Id ?? -1;
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }

    public class Context : DbContext
    {
        public string? ConnectionString { get; private set; } = null;

        public DbSet<Celebrity> Celebrities { get; set; }
        public DbSet<Lifeevent> Lifeevents { get; set; }

        public Context(string constring) : base()
        {
            this.ConnectionString = constring;
        }

        public Context() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (this.ConnectionString is null)
                this.ConnectionString =
                    @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CelebrityDB;Integrated Security=True";
            optionsBuilder.UseSqlServer(this.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Celebrity>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).IsRequired();
                entity.Property(c => c.FullName).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Nationality).IsRequired().HasMaxLength(2);
                entity.Property(c => c.ReqPhotoPath).HasMaxLength(200);
            });
            
            modelBuilder.Entity<Lifeevent>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Id).IsRequired();
                entity.HasOne<Celebrity>().WithMany().HasForeignKey(l => l.CelebrityId);
                entity.Property(l => l.CelebrityId).IsRequired();
                entity.Property(l => l.Date);
                entity.Property(l => l.Description).HasMaxLength(256);
                entity.Property(l => l.ReqPhotoPath).HasMaxLength(256);
            });
        }
    }
}