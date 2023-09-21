namespace FolderTree.Models;

public class DirectoryNode
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentDirectoryId { get; set; }
    public DirectoryNode ParentDirectory { get; set; }
    public List<DirectoryNode> ChildrenDirectories { get; set; }
}