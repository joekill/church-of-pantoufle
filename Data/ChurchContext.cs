using Microsoft.EntityFrameworkCore;
using ChurchOfPantoufle.Models;

namespace ChurchOfPantoufle.Data;

public class ChurchContext : DbContext
{
    public ChurchContext(DbContextOptions<ChurchContext> options) : base(options) { }

    public DbSet<Testimonial> Testimonials => Set<Testimonial>();
}
