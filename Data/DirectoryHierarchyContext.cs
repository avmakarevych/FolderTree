using FolderTree.Models;
using Microsoft.EntityFrameworkCore;

namespace FolderTree.Data;

public class DirectoryHierarchyContext : DbContext
{
    public DbSet<DirectoryNode> DirectoryNodes { get; set; }
    public DirectoryHierarchyContext(DbContextOptions<DirectoryHierarchyContext> options)
        : base(options)
    { }
}