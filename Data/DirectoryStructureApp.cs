using FolderTree.Models;

namespace FolderTree.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DirectoryHierarchyContext context)
        {
            context.Database.EnsureCreated();

            if (context.DirectoryNodes.Any())
            {
                return;
            }

            var directoryNodes = new DirectoryNode[]
            {
                new DirectoryNode { Id = 1, Name = "Creating Digital Images", ParentDirectoryId = null },
                new DirectoryNode { Id = 2, Name = "Resources", ParentDirectoryId = 1 },
                new DirectoryNode { Id = 3, Name = "Evidence", ParentDirectoryId = 1 },
                new DirectoryNode { Id = 4, Name = "Graphic Products", ParentDirectoryId = 1 },
                new DirectoryNode { Id = 5, Name = "Primary Sources", ParentDirectoryId = 2 },
                new DirectoryNode { Id = 6, Name = "Secondary Sources", ParentDirectoryId = 2 },
                new DirectoryNode { Id = 7, Name = "Process", ParentDirectoryId = 4 },
                new DirectoryNode { Id = 8, Name = "Final Product", ParentDirectoryId = 4 }
            };
            
            foreach (DirectoryNode node in directoryNodes)
            {
                context.DirectoryNodes.Add(node);
            }
            context.SaveChanges();
        }
    }
}
