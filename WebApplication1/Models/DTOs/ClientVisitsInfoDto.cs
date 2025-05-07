namespace WebApplication1.Models.DTOs;

public class ClientVisitsInfoDto
{
    public DateTime date { get; set; }
    public ClientInfoDto client { get; set; }
    public MechanicInfoDto mechanic { get; set; }
    public List<VisitServicesInfoDto> services { get; set; }
    
}
public class ClientInfoDto
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public DateTime dateOfBirth { get; set; }
       
}
public class MechanicInfoDto
{
    public int mechanicId { get; set; }
    public string licenceNumber { get; set; }
    
}

public class VisitServicesInfoDto
{
    public int id { get; set; }
    public string name { get; set; }
    public decimal serviceFee { get; set; }
}