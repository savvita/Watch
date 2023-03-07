namespace Watch.Domain.Interfaces
{
    public interface IWatchCounter
    {
        Task<int> Brand(int id);
        Task<int> CaseColor(int id);
        Task<int> CaseMaterial(int id);
        Task<int> CaseShape(int id);
        Task<int> Collection(int id);
        Task<int> Country(int id);
        Task<int> DialColor(int id);
        Task<int> DialType(int id);
        Task<int> IncrustationType(int id);
        Task<int> Function(int id);
        Task<int> Gender(int id);
        Task<int> GlassType(int id);
        Task<int> MovementType(int id);
        Task<int> StrapColor(int id);
        Task<int> StrapType(int id);
        Task<int> Style(int id);
        Task<int> WaterResistance(int id);
    }
}
