using System.ComponentModel.DataAnnotations;

namespace FolderTree.Models;

public class DirectoryNode
{
    public int Id { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
    public string Name { get; set; }
    public int? ParentDirectoryId { get; set; }
    public virtual DirectoryNode ParentDirectory { get; set; }
    public virtual List<DirectoryNode> ChildrenDirectories { get; set; }

}