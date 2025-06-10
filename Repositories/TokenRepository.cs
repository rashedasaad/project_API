namespace project_API.Repositories;

public class TokenRepository 
{
    private readonly appdbcontext _db;

    public TokenRepository(appdbcontext db)
    {
        _db = db;
    }
    
    public async Task Add(Token token)
    {
        await _db.Tokens.AddAsync(token);
    }
    
    public async Task<Token?> Get(string token)
    {
        return await _db.Tokens.FirstOrDefaultAsync(x => x.Value == token);
    }
    
    public Task Delete(Token token)
    {
        _db.Tokens.Remove(token);
         
        return Task.CompletedTask;
    }
    
    
    
    
}