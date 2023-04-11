using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class TreeNode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public TreeNode ParentNode { get; set;}
    }
}
