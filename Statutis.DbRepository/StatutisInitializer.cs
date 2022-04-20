using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Statutis.Entity;
using Statutis.Entity.Service;
using Statutis.Entity.Service.Check;

namespace Statutis.DbRepository.Migrations;

public static class StatutisInitializer
{
	public static void Initialize(ModelBuilder builder)
	{

		#region Type de services

		ServiceType webType = new ServiceType("Site Web");
		ServiceType smtpType = new ServiceType("SMTP");
		ServiceType dnsType = new ServiceType("DNS");
		ServiceType sshType = new ServiceType("SSH");
		ServiceType serverType = new ServiceType("Serveur");

		builder.Entity<ServiceType>().HasData(webType, smtpType, dnsType, sshType, serverType);

		#endregion

		#region Team

		Team adminTeam = new Team("Administrateur", "#e74c3c") { TeamId = Guid.Parse("582745B3-1C8C-4C89-A772-19F6D9102F42") };
		Team defaultTeam = new Team("Default", "#34495e") { TeamId = Guid.Parse("3074C258-5EB5-4598-AAFD-26AB51E2FCFA") };
		Team subDefaultTeam = new Team("Sub Team", "#95a5a6") { TeamId = Guid.Parse("9DE07AEC-CE5D-43BC-A909-C648A3B4956A"), MainTeamId = defaultTeam.TeamId };

		builder.Entity<Team>().HasData(adminTeam, defaultTeam, subDefaultTeam);

		#endregion


		#region Group

		Group defaultGroup = new Group("Défaut", "Groupe par défaut") { GroupId = Guid.Parse("2395B8A3-1ABB-4E2D-AF1E-B3B830DA10F9") };
		Group statutisGroup = new Group("Statutis", "Groupe par défaut") { GroupId = Guid.Parse("CAD77A46-5C06-4741-B3CE-76D520D5B4AE") };

		builder.Entity<Group>().HasData(defaultGroup, statutisGroup);
		builder.Entity<Group>()
			.HasMany(x => x.Teams)
			.WithMany(x => x.Groups).UsingEntity(j => j.HasData(
				new { TeamsTeamId = adminTeam.TeamId, GroupsGroupId = defaultGroup.GroupId },
				new { TeamsTeamId = defaultTeam.TeamId, GroupsGroupId = defaultGroup.GroupId },
				new { TeamsTeamId = adminTeam.TeamId, GroupsGroupId = statutisGroup.GroupId },
				new { TeamsTeamId = defaultTeam.TeamId, GroupsGroupId = statutisGroup.GroupId }
			));

		#endregion

		#region Service

		HttpService statutisService = new HttpService()
		{
			ServiceId = Guid.Parse("CBB3D7EB-0C88-46EB-9370-112E90271659"), ServiceTypeName = webType.Name,
			Code = 200, Description = "Serveur frontend de statutis", Host = "https://statutis.silvain.eu", Name = "Frontend", GroupId = statutisGroup.GroupId,
			Port = 443
		};
		HttpService statutisApiService = new HttpService()
		{
			ServiceId = Guid.Parse("0CAC5B5A-FC9F-4894-849B-2B2B97538C2E"), ServiceTypeName = webType.Name,
			Code = 404, Description = "Serveur backend de statutis", Host = "https://api.statutis.silvain.eu", Name = "API", GroupId = statutisGroup.GroupId,
			Port = 443
		};
		PingService serverGoogle = new PingService()
		{
			ServiceId = Guid.Parse("3B0374EC-9A34-4E87-ADE1-7FD3CC4E04F5"), ServiceTypeName = serverType.Name,
			Description = "Serveur de google", Host = "8.8.8.8", Name = "Serveur de Google", GroupId = defaultGroup.GroupId,
		};
		DnsService dnsSilvain = new DnsService()
		{
			ServiceId = Guid.Parse("E847AF71-CEC3-4C0E-9DC1-E67AC54A0CB4"), ServiceTypeName = dnsType.Name,
			Description = "DNS A pour silvain.eu", Host = "silvain.eu", Name = "DNS Silvain.eu", GroupId = defaultGroup.GroupId,
			Type = "A", Result = "89.234.182.183"
		};


		builder.Entity<HttpService>().HasData(statutisService, statutisApiService);
		builder.Entity<DnsService>().HasData(dnsSilvain);
		builder.Entity<PingService>().HasData(serverGoogle);

		#endregion

		#region User

		User admin = new User("contact@silvain.eu", "admin", @"$argon2id$v=19$m=1024,t=1,p=1$c29tZXNhbHQ$Wg1s/1X6O1f1ZQCQtAvkGaCMwteH+F2h6p6AGDM8om4") { Roles = "ROLE_ADMIN"};

		builder.Entity<User>().HasData(admin);
		
		builder.Entity<User>()
			.HasMany(x => x.Teams)
			.WithMany(x => x.Users).UsingEntity(j => j.HasData(
				new { TeamsTeamId = adminTeam.TeamId, UsersEmail = admin.Email }
			));

		#endregion


	}

}