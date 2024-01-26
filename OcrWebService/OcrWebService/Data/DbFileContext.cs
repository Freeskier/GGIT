using Microsoft.EntityFrameworkCore;
using OcrWebService.Data.Entity;

namespace OcrWebService.Data;
public class DbFileContext: DbContext
{
    public DbFileContext(DbContextOptions<DbFileContext> options) : base(options)
    {
    }

    public DbSet<OcrResultEntity> OcrResults { get; set; }

}
