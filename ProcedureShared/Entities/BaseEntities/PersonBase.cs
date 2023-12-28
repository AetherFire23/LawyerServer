using EFCoreBase.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcedureShared.Entities.BaseEntities;


public abstract class PersonBase : EntityBase
{
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string PostalCode { get; set; } = string.Empty;
	public string Country { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string MobilePhoneNumber { get; set; } = string.Empty;
	public string WorkPhoneNumber { get; set; } = string.Empty;
	public string HomePhoneNumber { get; set; } = string.Empty;
	public bool HasJuridicalAid { get; set; } = false;
	public string PostalCase { get; set; } = string.Empty;
	public string Fax { get; set; } = string.Empty;
	public Genders Gender { get; set; } = Genders.Male;






	[NotMapped]
	public string LowerCaseFormattedFullName => $"{this.FirstName} {this.LastName}";
	[NotMapped]
	public string UppercaseFormattedFullName => ($"{this.FirstName} {this.LastName}").ToUpper();


	public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
	public string SocialSecurityNumber { get; set; } = string.Empty;

	[NotMapped]
	public string GenderedName => Gender is Genders.Male ? "Monsieur" : "Madame";
}



public enum Genders
{
	Male,
	Female,
}