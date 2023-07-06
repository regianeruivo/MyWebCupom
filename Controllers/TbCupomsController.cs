using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebCupom.Models;

namespace MyWebCupom.Controllers
{
    public class TbCupomsController : Controller
    {
        private readonly DbTechchallengeContext _context;

        private readonly BlobServiceClient _blobServiceClient;

        private readonly string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["AzureBlobStorage"];

        private readonly string pathImageAzure = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["PathImgAzure"];

        public TbCupomsController(DbTechchallengeContext context)
        {
            _context = context;
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        // GET: TbCupoms
        public async Task<IActionResult> Index()
        {
              return _context.TbCupoms != null ? 
                          View(await _context.TbCupoms.ToListAsync()) :
                          Problem("Entity set 'DbTechchallengeContext.TbCupoms'  is null.");
        }

        // GET: TbCupoms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbCupoms == null)
            {
                return NotFound();
            }

            var tbCupom = await _context.TbCupoms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbCupom == null)
            {
                return NotFound();
            }
            else
            {
                tbCupom.UrlCupom = pathImageAzure + tbCupom.UrlCupom;
            }
            return View(tbCupom);
        }

        // GET: TbCupoms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbCupoms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] TbCupom tbCupom)
        {
            if (ModelState.IsValid)
            {

                var httpRequest = HttpContext.Request;

                if (httpRequest.Form.Files.Count > 0)
                {
                    const int fileIndex = 0;

                    IFormFile postedFile = httpRequest.Form.Files[fileIndex];

                    var containerInstance = _blobServiceClient.GetBlobContainerClient("techticketimage");

                    var fileName = Guid.NewGuid() + postedFile.FileName;

                    var blobInstance = containerInstance.GetBlobClient(fileName);

                    await blobInstance.UploadAsync(postedFile.OpenReadStream());

                    tbCupom.UrlCupom = fileName;                    

                }
                _context.Add(tbCupom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbCupom);
        }

        // GET: TbCupoms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbCupoms == null)
            {
                return NotFound();
            }

            var tbCupom = await _context.TbCupoms.FindAsync(id);            

            if (tbCupom == null)
            {
                return NotFound();
            }            
            return View(tbCupom);
        }

        // POST: TbCupoms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,UrlCupom")] TbCupom tbCupom)
        {
            if (id != tbCupom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {                    
                    _context.Update(tbCupom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbCupomExists(tbCupom.Id))
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
            return View(tbCupom);
        }

        // GET: TbCupoms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbCupoms == null)
            {
                return NotFound();
            }

            var tbCupom = await _context.TbCupoms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbCupom == null)
            {
                return NotFound();
            }

            return View(tbCupom);
        }

        // POST: TbCupoms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbCupoms == null)
            {
                return Problem("Entity set 'DbTechchallengeContext.TbCupoms'  is null.");
            }
            var tbCupom = await _context.TbCupoms.FindAsync(id);
            if (tbCupom != null)
            {
                _context.TbCupoms.Remove(tbCupom);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbCupomExists(int id)
        {
          return (_context.TbCupoms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
