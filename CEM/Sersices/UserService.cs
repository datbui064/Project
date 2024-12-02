using CEM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class UserService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public UserService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<NguoiDung?> LoginAsync(string username, string password)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<QlbContext>();

            var user = await context.NguoiDungs
                .FirstOrDefaultAsync(u => u.TenDangNhap == username);

            if (user != null && user.MatKhau == password)
            {
                return user;
            }

            return null;
        }
    }

    public async Task<bool> RegisterAsync(NguoiDung newUser)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<QlbContext>();

            // Kiểm tra xem tên đăng nhập đã tồn tại chưa
            var existingUser = await context.NguoiDungs
                .AnyAsync(u => u.TenDangNhap == newUser.TenDangNhap);

            if (existingUser)
            {
                return false; // Tên đăng nhập đã tồn tại
            }

            // Thêm người dùng mới vào CSDL
            context.NguoiDungs.Add(newUser);
            await context.SaveChangesAsync();
            return true; // Đăng ký thành công
        }
    }
}
