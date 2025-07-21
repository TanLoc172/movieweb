using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data; // Thay thế bằng namespace của DbContext của bạn
using MovieWebsite.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// Giả sử bạn có namespace này cho ViewModel, nếu khác hãy thay đổi

[Route("reels")] // Sử dụng route attribute để URL đẹp hơn (ví dụ: /reels/play/5)
public class ReelsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly UserManager<AppUser> _userManager;

    public ReelsController(AppDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _userManager = userManager;
    }

    // --- CHỨC NĂNG CHO ADMIN ---

    // GET: /reels/manage (Danh sách quản lý cho Admin)
//    [Authorize(Roles = "Admin")]
    [HttpGet("manage")]
    public async Task<IActionResult> Index()
    {
        var reels = await _context.Reels.Include(r => r.Uploader).OrderByDescending(r => r.CreatedAt).ToListAsync();
        return View(reels);
    }

    // GET: /reels/create (Hiển thị form tạo mới)
//    [Authorize(Roles = "Admin")]
    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new ReelViewModel());
    }

    // POST: /reels/create (Xử lý tạo mới Reel)
    [HttpPost("create")]
//    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReelViewModel viewModel)
    {
        if (viewModel.VideoFile == null || viewModel.VideoFile.Length == 0)
        {
            ModelState.AddModelError("VideoFile", "Vui lòng chọn một file video.");
        }

        if (ModelState.IsValid)
        {
            string videoPath = await SaveVideoFile(viewModel.VideoFile);

            var reel = new Reel
            {
                Title = viewModel.Title,
                VideoPath = videoPath, // Lưu đường dẫn tương đối
                IsPublished = viewModel.IsPublished,
                UploaderId = _userManager.GetUserId(User), // Lấy ID của Admin đang đăng nhập
                CreatedAt = DateTime.UtcNow
            };

            _context.Add(reel);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Tạo Reel thành công!";
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }

    // GET: /reels/edit/5 (Hiển thị form chỉnh sửa)
//    [Authorize(Roles = "Admin")]
    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var reel = await _context.Reels.FindAsync(id);
        if (reel == null)
        {
            return NotFound();
        }

        var viewModel = new ReelViewModel
        {
            Id = reel.Id,
            Title = reel.Title,
            IsPublished = reel.IsPublished,
            ExistingVideoPath = reel.VideoPath
        };

        return View(viewModel);
    }

    // POST: /reels/edit/5 (Xử lý chỉnh sửa Reel)
    [HttpPost("edit/{id}")]
//    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ReelViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var reelToUpdate = await _context.Reels.FindAsync(id);
            if (reelToUpdate == null)
            {
                return NotFound();
            }

            // Nếu có file video mới được tải lên
            if (viewModel.VideoFile != null && viewModel.VideoFile.Length > 0)
            {
                // 1. Xóa file video cũ
                DeleteVideoFile(reelToUpdate.VideoPath);

                // 2. Lưu file video mới và cập nhật đường dẫn
                reelToUpdate.VideoPath = await SaveVideoFile(viewModel.VideoFile);
            }
            
            reelToUpdate.Title = viewModel.Title;
            reelToUpdate.IsPublished = viewModel.IsPublished;

            try
            {
                _context.Update(reelToUpdate);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật Reel thành công!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReelExists(reelToUpdate.Id))
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
        // Nếu model không hợp lệ, cần gán lại ExistingVideoPath để view hiển thị đúng
        var reel = await _context.Reels.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        if (reel != null)
        {
            viewModel.ExistingVideoPath = reel.VideoPath;
        }
        return View(viewModel);
    }

    // GET: /reels/delete/5 (Hiển thị trang xác nhận xóa)
//    [Authorize(Roles = "Admin")]
    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var reel = await _context.Reels
            .Include(r => r.Uploader)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (reel == null)
        {
            return NotFound();
        }

        return View(reel);
    }

    // POST: /reels/delete/5 (Xác nhận và thực hiện xóa)
    [HttpPost("delete/{id}"), ActionName("Delete")]
//    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var reel = await _context.Reels.FindAsync(id);
        if (reel != null)
        {
            // Xóa file vật lý trước khi xóa record trong DB
            DeleteVideoFile(reel.VideoPath);

            _context.Reels.Remove(reel);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa Reel thành công!";
        }
        
        return RedirectToAction(nameof(Index));
    }


    // --- CHỨC NĂNG CHO NGƯỜI DÙNG ---

    // GET: /reels/play/5 (Trang xem video cho người dùng)
    [AllowAnonymous]
    [HttpGet("ReelsDetails/{id}")]
    public async Task<IActionResult> ReelsDetails(int id)
    {
        var reel = await _context.Reels.FirstOrDefaultAsync(r => r.Id == id && r.IsPublished);

        if (reel == null)
        {
            // Có thể trả về trang lỗi 404 hoặc trang "Reel không tồn tại"
            return NotFound();
        }

        // Tăng lượt xem
        reel.Views++;
        _context.Update(reel);
        await _context.SaveChangesAsync();

        return View("ReelsDetails", reel);
    }



    [HttpGet("watch")]
    [AllowAnonymous]
    public async Task<IActionResult> Watch()
    {
        var allPublishedReels = await _context.Reels
            .Where(r => r.IsPublished)
            .OrderByDescending(r => r.CreatedAt) // Hiển thị video mới nhất trước
            .ToListAsync();

        if (allPublishedReels == null || !allPublishedReels.Any())
        {
            // Có thể trả về một view thông báo "Chưa có video nào"
            return View("NoReelsAvailable"); 
        }

        return View(allPublishedReels);
    }


    // --- HÀM HỖ TRỢ ---

    private bool ReelExists(int id)
    {
        return _context.Reels.Any(e => e.Id == id);
    }

    private async Task<string> SaveVideoFile(IFormFile videoFile)
    {
        // Tạo tên file duy nhất để tránh trùng lặp
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(videoFile.FileName);
        
        // Xác định đường dẫn thư mục lưu trữ
        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "reels");
        
        // Tạo thư mục nếu chưa tồn tại
        Directory.CreateDirectory(uploadsFolder);
        
        // Đường dẫn đầy đủ đến file
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        // Lưu file
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await videoFile.CopyToAsync(fileStream);
        }

        // Trả về đường dẫn tương đối để lưu vào DB
        return "/uploads/reels/" + uniqueFileName;
    }

    private void DeleteVideoFile(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath)) return;

        // Chuyển đổi đường dẫn tương đối thành đường dẫn vật lý
        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath.TrimStart('/'));

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }
}