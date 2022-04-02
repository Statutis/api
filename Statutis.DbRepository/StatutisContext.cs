using Microsoft.EntityFrameworkCore;
using Statutis.Entity;
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
	public DbSet<SshService> SshService { get; set; } = null!;

	public StatutisContext()
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseNpgsql(@"Host=localhost;Username=admin;Password=password;Database=statutis");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Service>(m =>
		{
			m.HasKey(x => x.ServiceId);
			m.HasIndex(x => new { x.Name, x.GroupId }).IsUnique();
			m.HasOne(x => x.Group)
				.WithMany(x => x.Services).HasForeignKey(x => x.GroupId);
		});
		modelBuilder.Entity<HttpService>().ToTable(nameof (HttpService));
		modelBuilder.Entity<DnsService>().ToTable(nameof (DnsService));
		modelBuilder.Entity<PingService>().ToTable(nameof (PingService));
		modelBuilder.Entity<SshService>().ToTable(nameof (SshService));

		modelBuilder.Entity<Group>(m =>
		{
			m.HasKey(x => x.GroupId);
			m.HasIndex(x => new { x.Name, x.MainGroupId }).IsUnique();
			m.HasOne(x => x.MainGroup).WithMany(x => x.Children).HasForeignKey(x => x.MainGroupId);
		});
		modelBuilder.Entity<User>(m => { m.HasKey(x => x.Email); });
		modelBuilder.Entity<Team>(m =>
		{
			m.HasKey(x => new { x.TeamId });
			m.HasIndex(x => new { x.Name, x.MainTeamId }).IsUnique();
			m.HasOne(x => x.MainTeam).WithMany(x => x.Children).HasForeignKey(x => x.MainTeamId);
		});
		modelBuilder.Entity<ServiceType>(m => { m.HasKey(x => x.Name); });
	}



}