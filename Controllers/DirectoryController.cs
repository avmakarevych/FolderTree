using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FolderTree.Data;
using FolderTree.Models;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FolderTree.Controllers
{
    public class DirectoryController : Controller
    {
        private readonly DirectoryHierarchyContext _context;

        public DirectoryController(DirectoryHierarchyContext context)
        {
            _context = context;
        }
        
        // GET: Directory
        public async Task<IActionResult> Index()
        {
            var allDirectories = await _context.DirectoryNodes.ToListAsync();
            var rootDirectories = allDirectories.Where(d => d.ParentDirectoryId == null).ToList();

    
            foreach (var directory in allDirectories)
            {
                directory.ChildrenDirectories = allDirectories.Where(d => d.ParentDirectoryId == directory.Id).ToList();
            }

            return View(new DirectoryViewModel { RootDirectories = rootDirectories });
        }


        // GET: Directory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var directoryNode = await _context.DirectoryNodes
                .Include(d => d.ParentDirectory)
                .Include(d => d.ChildrenDirectories)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (directoryNode == null)
            {
                return NotFound();
            }

            return View(directoryNode);
        }


        // GET: Directory/Create
        public IActionResult Create()
        {
            ViewData["ParentDirectoryId"] = new SelectList(_context.DirectoryNodes, "Id", "Id");
            return View();
        }

        // POST: Directory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ParentDirectoryId")] DirectoryNode directoryNode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(directoryNode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentDirectoryId"] = new SelectList(_context.DirectoryNodes, "Id", "Id", directoryNode.ParentDirectoryId);
            return View(directoryNode);
        }

        // GET: Directory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DirectoryNodes == null)
            {
                return NotFound();
            }

            var directoryNode = await _context.DirectoryNodes.FindAsync(id);
            if (directoryNode == null)
            {
                return NotFound();
            }
            ViewData["ParentDirectoryId"] = new SelectList(_context.DirectoryNodes, "Id", "Id", directoryNode.ParentDirectoryId);
            return View(directoryNode);
        }

        // POST: Directory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ParentDirectoryId")] DirectoryNode directoryNode)
        {
            if (id != directoryNode.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(directoryNode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectoryNodeExists(directoryNode.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentDirectoryId"] = new SelectList(_context.DirectoryNodes, "Id", "Id", directoryNode.ParentDirectoryId);
            return View(directoryNode);
        }

        // GET: Directory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DirectoryNodes == null)
            {
                return NotFound();
            }

            var directoryNode = await _context.DirectoryNodes
                .Include(d => d.ParentDirectory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (directoryNode == null)
            {
                return NotFound();
            }

            return View(directoryNode);
        }

        // POST: Directory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DirectoryNodes == null)
            {
                return Problem("Entity set 'DirectoryHierarchyContext.DirectoryNodes'  is null.");
            }
            var directoryNode = await _context.DirectoryNodes.FindAsync(id);
            if (directoryNode != null)
            {
                _context.DirectoryNodes.Remove(directoryNode);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        // POST: Directory/DeleteAll
        [HttpPost]
        public async Task<IActionResult> DeleteAll()
        {
            _context.RemoveRange(_context.DirectoryNodes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DirectoryNodeExists(int id)
        {
          return (_context.DirectoryNodes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile jsonFile, string directoryPath)
        {
            if (jsonFile != null)
            {
                using var reader = new StreamReader(jsonFile.OpenReadStream());
                string jsonContent = await reader.ReadToEndAsync();

                List<DirectoryNode> directoriesFromJson;
                try
                {
                    directoriesFromJson = JsonConvert.DeserializeObject<List<DirectoryNode>>(jsonContent);
                }
                catch (JsonReaderException ex)
                {
                    ModelState.AddModelError("jsonFile", "Invalid JSON format. Please check the structure of your JSON file.");
                    return View();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("jsonFile", $"An error occurred while processing the file: {ex.Message}");
                    return View();
                }

                if (directoriesFromJson != null)
                    foreach (var dir in directoriesFromJson)
                    {
                        if (!DirectoryNodeExists(dir.Id))
                        {
                            _context.DirectoryNodes.Add(dir);
                        }
                    }

                await _context.SaveChangesAsync();
            }
            else if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                if (Directory.Exists(directoryPath))
                {
                    var importedDirectories = ImportDirectoriesFromPath(directoryPath);

                    foreach (var dir in importedDirectories)
                    {
                        if (!DirectoryNodeExists(dir.Id))
                        {
                            _context.DirectoryNodes.Add(dir);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("directoryPath", "The directory path does not exist or is not accessible.");
                    return View();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private List<DirectoryNode> ImportDirectoriesFromPath(string path, int? parentId = null)
        {
            var result = new List<DirectoryNode>();
            var directories = Directory.GetDirectories(path);

            foreach (var directory in directories)
            {
                var directoryName = Path.GetFileName(directory);
                var directoryNode = new DirectoryNode { Name = directoryName, ParentDirectoryId = parentId };

                _context.DirectoryNodes.Add(directoryNode);
                _context.SaveChanges();

                result.Add(directoryNode);
                result.AddRange(ImportDirectoriesFromPath(directory, directoryNode.Id));
            }

            return result;
        }
        
        public async Task<IActionResult> Export()
        {
            var allDirectories = await _context.DirectoryNodes.ToListAsync();
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            };
            var jsonString = JsonSerializer.Serialize(allDirectories, options);

            var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            return new FileStreamResult(fileStream, "application/octet-stream")
            {
                FileDownloadName = "DirectoryStructure_Export_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json"
            };
        }
    }
}
