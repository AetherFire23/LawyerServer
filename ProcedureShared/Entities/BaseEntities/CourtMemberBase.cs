using ProcedureMakerServer.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcedureShared.Entities.BaseEntities;


public abstract class CourtMemberBase : PersonBase
{
	// if string.empty, means that the target is not notifiabvle
	public string NotificationEmail { get; set; } = string.Empty;


	[NotMapped]
	public string FullName => $"{FirstName} {LastName}";
	public CourtRoles CourtRole { get; set; } = CourtRoles.Intimated;

	[NotMapped]
	public bool IsNotifiable => NotificationEmail != string.Empty && MustNotify;

	public bool MustNotify { get; set; } = false;

	public string GetGenderedCourtRoleName()
	{
		switch (CourtRole)
		{
			case CourtRoles.Plaintiff:
				{
					return this.Gender == Genders.Male ? "Demandeur" : "Demanderesse";
				}
			case CourtRoles.Defender:
				{
					return this.Gender == Genders.Female ? "Défendeur" : "Défenderesse";
				}
			default: return string.Empty;
		}
	}

	public void CopyFromCourtMember(CourtMemberBase other)
	{
		this.Id = other.Id;
		// Copy properties individually
		this.FirstName = other.FirstName;
		this.LastName = other.LastName;
		this.PostalCode = other.PostalCode;
		this.Country = other.Country;
		this.Email = other.Email;
		this.Address = other.Address;
		this.City = other.City;
		this.MobilePhoneNumber = other.MobilePhoneNumber;
		this.WorkPhoneNumber = other.WorkPhoneNumber;
		this.HomePhoneNumber = other.HomePhoneNumber;
		this.HasJuridicalAid = other.HasJuridicalAid;
		this.PostalCase = other.PostalCase;
		this.Fax = other.Fax;
		this.Gender = other.Gender;
		this.DateOfBirth = other.DateOfBirth;
		this.SocialSecurityNumber = other.SocialSecurityNumber;

		// Additional properties specific to CourtMemberBase
		this.NotificationEmail = other.NotificationEmail;
		this.CourtRole = other.CourtRole;
		this.MustNotify = other.MustNotify;
	}

}
