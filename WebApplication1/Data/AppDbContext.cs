using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity("WebApplication1.Models.TreeNode", b =>
            {
                b.HasOne("WebApplication1.Models.TreeNode", "ParentNode")
                    .WithMany()
                    .HasForeignKey("ParentNodeId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.Navigation("ParentNode");
            });
        }

        DbSet<TreeNode> TreeNodes { get; set; }
    }
}
