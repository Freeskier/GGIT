using Microsoft.EntityFrameworkCore;
using OcrWebService.Data.Entity;

namespace OcrWebService.Data;
public class DbFileContext: DbContext
{
    public DbSet<OcrResultEntity> OcrResults { get; set; }

    public string DbPath { get; }

    public DbFileContext()
    {
        string path = "./db/";
        try
        {
            // Determine whether the directory exists.
            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
            }

            DbPath = Path.Join(path, "database.db");
        }
        catch (Exception e)
        {
            Console.WriteLine("The process failed: {0}", e.ToString());
        }
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

}
