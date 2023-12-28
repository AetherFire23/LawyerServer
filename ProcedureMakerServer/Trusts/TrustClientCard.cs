using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Trusts;

public class TrustClientCard : EntityBase
{
	public Guid ClientId { get; set; }
	public Client Client { get; set; }

	public List<TrustPayment> Payments { get; set; } = new();
}

public class TrustConfig : IEntityTypeConfiguration<TrustClientCard>
{
	public void Configure(EntityTypeBuilder<TrustClientCard> builder)
	{
		//builder.Dele
		//  builder.Dd
	}
}