namespace Shreco.API.Services;

public class HistoryService : IHistoryService {
    #region Private fields
    private readonly AppContext _appContext;
    #endregion

    public HistoryService(AppContext appContext) {
        _appContext = appContext;
    }
    public async Task<bool> AddHistory(params History[] histories) {
        try {
            await _appContext.Histories.AddRangeAsync(histories);
            await _appContext.SaveChangesAsync();
            return true;
        } catch {
            return false;
        }
    }
}