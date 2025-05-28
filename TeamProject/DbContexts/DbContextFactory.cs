using Microsoft.EntityFrameworkCore;

namespace TeamProject.DbContexts;

public class DbContextFactory
{
    private readonly string _connectionString;

    public DbContextFactory(string connectionString)
    {
        _connectionString=connectionString;
        
        
    }

    public BmpEditorDbContext CreateContext()
    {
        DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;
        return new BmpEditorDbContext(options);
    }
    
    
}