using FolderTree.Models;
using Newtonsoft.Json;

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

            try
            {
                var directoryNodesData = File.ReadAllText("seedData.json");
                var directoryNodes = JsonConvert.DeserializeObject<List<DirectoryNode>>(directoryNodesData);

                foreach (DirectoryNode node in directoryNodes)
                {
                    context.DirectoryNodes.Add(node);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
            }
        }
    }
}
