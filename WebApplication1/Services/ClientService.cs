using System.Data;
using Microsoft.Data.SqlClient;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Services;

public class ClientService : IClientService
{
    private readonly string _connectionString;
    public ClientService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? string.Empty;
    }
    
    public async Task<ClientVisitsInfoDto> GetClientVisitById(int visitId)
    {
        var command =
            @"SELECT v.date,  c.first_name, c.last_name, c.date_of_birth, m.mechanic_id, m.licence_number, s.service_id, s.name, s.base_fee
            FROM Visit v
            JOIN Client c ON c.client_id = v.client_id
            JOIN Mechanic m ON m.mechanic_id = v.mechanic_id
            JOIN Visit_Service vs ON vs.visit_id = v.visit_id
            JOIN Service s ON s.service_id = vs.service_id
            WHERE v.visit_id = @visitId;";

        await using SqlConnection conn = new SqlConnection(_connectionString);
        await using SqlCommand cmd = new SqlCommand(command, conn);
        await conn.OpenAsync();
        cmd.Parameters.AddWithValue("@visitId", visitId);
        ClientVisitsInfoDto? clientVisit  = null;
        var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            if (clientVisit is null)
            {
                clientVisit = new ClientVisitsInfoDto()
                {
                    date = reader.GetDateTime(0),
                    client = new ClientInfoDto()
                    {
                        firstName = reader.GetString(1),
                        lastName = reader.GetString(2), 
                        dateOfBirth = reader.GetDateTime(3),
                    },
                    mechanic = new MechanicInfoDto()
                    {
                        mechanicId = reader.GetInt32(4),
                        licenceNumber = reader.GetString(5),
                    },
                    services = new List<VisitServicesInfoDto>()
                };
            }
            int serviceId = reader.GetInt32(6);
            var service = clientVisit.services.FirstOrDefault(e => e.id.Equals(serviceId));
            if (service is null)
            {
                service = new VisitServicesInfoDto()
                {
                    name = reader.GetString(7),
                    serviceFee = reader.GetInt32(8),
                };
                clientVisit.services.Add(service);
            }
        }
        return clientVisit;
    }
}