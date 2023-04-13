using Microsoft.EntityFrameworkCore;
using ZetaTrading.Models;

namespace ZetaTrading.API.Domain.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity("ZetaTrading.Models.TreeNode", b =>
            {
                b.HasOne("ZetaTrading.Models.TreeNode", "ParentNode")
                    .WithMany()
                    .HasForeignKey("ParentNodeId")
                    .OnDelete(DeleteBehavior.Restrict);

                b.Navigation("ParentNode");
            });
        }

        public DbSet<TreeNode> TreeNodes { get; set; }
        public DbSet<JournalRecord> JournalRecords { get; set; }
    }
}
