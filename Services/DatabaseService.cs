namespace IncidentManagementSystem.Services;

public class DatabaseService
{
    public async Task<bool> IsHealthyAsync()
    {
        // Implementación básica para verificar salud de la BD
        await Task.Delay(100); // Simular delay
        return true;
    }
}
