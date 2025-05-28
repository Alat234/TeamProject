using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TeamProject.DbContexts;

public class BmpEditorDbDesignTimeContextFactory: IDesignTimeDbContextFactory<BmpEditorDbContext> 
{
    public BmpEditorDbContext CreateDbContext(string[] args)
    {
        
        var optionsBuilder = new DbContextOptionsBuilder<BmpEditorDbContext>();
        optionsBuilder.UseSqlite("Data Source=appdb.db");
        return new BmpEditorDbContext(optionsBuilder.Options);
    }
}
