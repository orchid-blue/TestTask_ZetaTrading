using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ZetaTrading.Models
{
    public class TreeNode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? TreeName { get; set; }

        [AllowNull]
        public TreeNode? ParentNode { get; set;}
    }
}
