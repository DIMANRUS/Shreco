namespace Shreco.API.Interfaces;

public interface IHistoryService {
    Task<bool> AddHistory(params History[] histories);
}