using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Statutis.DbRepository.Migrations;
using Statutis.Entity;
using Statutis.Entity.History;
using Statutis.Entity.Service;
using Statutis.Entity.Service.Check;

namespace Statutis.DbRepository;

public class StatutisContext : DbContext
{
	public DbSet<User> User { get; set; } = null!;
	public DbSet<Team> Team { get; set; } = null!;
	public DbSet<Group> Group { get; set; } = null!;
	public DbSet<Service> Service { get; set; } = null!;
	public DbSet<ServiceType> ServiceType { get; set; } = null!;
	public DbSet<HttpService> HttpService { get; set; } = null!;
	public DbSet<DnsService> DnsService { get; set; } = null!;
	public DbSet<PingService> PingService { get; set; } = null!;
	public DbSet<AtlassianStatusPageService> AtlassianStatusPageService { get; set; } = null!;
	public DbSet<HistoryEntry> History { get; set; } = null!;

	public StatutisContext()
	{
	}

	public StatutisContext(DbContextOptions<StatutisContext> options) : base(options)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseNpgsql(@"Host=localhost;Username=admin;Password=password;Database=statutis");
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Service>(m =>
		{
			m.HasKey(x => x.ServiceId);
			m.HasIndex(x => new { x.Name, x.GroupId }).IsUnique();
			m.HasOne(x => x.ServiceType).WithMany().HasForeignKey(x => x.ServiceTypeName);
			m.HasOne(x => x.Group)
				.WithMany(x => x.Services).HasForeignKey(x => x.GroupId);
		});
		modelBuilder.Entity<HttpService>().ToTable(nameof (HttpService));
		modelBuilder.Entity<DnsService>().ToTable(nameof (DnsService));
		modelBuilder.Entity<PingService>().ToTable(nameof (PingService));
		modelBuilder.Entity<AtlassianStatusPageService>().ToTable(nameof (AtlassianStatusPageService));
		
		modelBuilder.Entity<Group>(m =>
		{
			m.HasKey(x => x.GroupId);
			m.HasIndex(x => new { x.Name }).IsUnique();
			m.HasMany(x => x.Teams).WithMany(x => x.Groups);
			
			m.Property(x => x.IsPublic).HasDefaultValue(true);
		});
		modelBuilder.Entity<User>(m => { m.HasKey(x => x.Email); });
		modelBuilder.Entity<Team>(m =>
		{
			m.HasKey(x => new { x.TeamId });
			m.HasIndex(x => new { x.Name }).IsUnique();
		});
		modelBuilder.Entity<ServiceType>(m => { m.HasKey(x => x.Name); });

		modelBuilder.Entity<HistoryEntry>(m =>
		{
			m.HasKey(x => new { x.ServiceId, x.DateTime });
			m.HasOne(x => x.Service).WithMany(x => x.HistoryEntries).HasForeignKey(x => x.ServiceId);
		});

		StatutisInitializer.Initialize(modelBuilder);
	}

}