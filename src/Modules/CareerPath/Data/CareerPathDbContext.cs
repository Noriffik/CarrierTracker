using Microsoft.EntityFrameworkCore;

namespace CareerTracker.CareerPath.Data;

public class CareerPathDbContext : DbContext
{
    public CareerPathDbContext(DbContextOptions<CareerPathDbContext> options) : base(options) { }
}
