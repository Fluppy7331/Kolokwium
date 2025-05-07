using WebApplication1.Models.DTOs;

namespace WebApplication1.Services;

public interface IClientService
{
    Task<ClientVisitsInfoDto> GetClientVisitById(int visitId);
    
}