using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Statutis.Entity.Service;

namespace Statutis.DbRepository.Migrations;

public static class StatutisInitializer
{
	public static void Initialize(ModelBuilder builder)
	{
		LoadServiceType(builder.Entity<ServiceType>());
	}


	private static void LoadServiceType(EntityTypeBuilder<ServiceType> serviceTypes)
	{
		ServiceType webType = new ServiceType("Site Web");
		ServiceType smtpType = new ServiceType("SMTP");
		ServiceType dnsType = new ServiceType("DNS");
		ServiceType sshType = new ServiceType("SSH");
		ServiceType serverType = new ServiceType("Serveur");

		serviceTypes.HasData(webType, smtpType, dnsType, sshType, serverType);
	}

}