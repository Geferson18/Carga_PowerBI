using Billycock_MS_Reusable.Models.Billycock;
using Billycock_MS_Reusable.Models.Utils;
using Billycock_MS_Reusable.Models.Utils;
using Microsoft.EntityFrameworkCore;

namespace Billycock_MS_Reusable.Service
{
    public class BillycockServiceContext: DbContext
    {
        public BillycockServiceContext(DbContextOptions<BillycockServiceContext> options)
        : base(options)
        {
        }

        public DbSet<User> USER { get; set; }
        public DbSet<Platform> PLATFORM { get; set; }
        public DbSet<State> STATE { get; set; }
        public DbSet<Account> ACCOUNT { get; set; }
        public DbSet<PlatformAccount> PLATFORMACCOUNT { get; set; }
        public DbSet<UserPlatform> USERPLATFORM { get; set; }
        public DbSet<UserPlatformAccount> USERPLATFORMACCOUNT { get; set; }
        public DbSet<History> HISTORY { get; set; }
        public DbSet<Audit> AUDIT { get; set; }
        public DbSet<TokenUser> TOKENUSER { get; set; }
        public DbSet<Correlative> CORRELATIVE { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlatformAccount>().HasKey(pa => new { pa.idPlatform, pa.idAccount });
            modelBuilder.Entity<UserPlatform>().HasKey(up => new { up.idUser, up.idPlatform });
            modelBuilder.Entity<UserPlatformAccount>().HasKey(upa => new { upa.idUser, upa.idPlatform, upa.idAccount, upa.GuiID });
        }
    }
}
