using System.Transactions;

namespace Learnify.Core.Transactions;

public static class TransactionScopeBuilder
{
    public static TransactionScope CreateReadCommitted()
    {
        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout
        };

        return new TransactionScope(TransactionScopeOption.Required, options);
    }
    
    public static TransactionScope CreateRepeatableRead()
    {
        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.RepeatableRead,
            Timeout = TransactionManager.DefaultTimeout
        };

        return new TransactionScope(TransactionScopeOption.Required, options);
    }
    
    public static TransactionScope CreateReadCommittedAsync()
    {
        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout
        };

        return new TransactionScope(TransactionScopeOption.Required, options, TransactionScopeAsyncFlowOption.Enabled);
    }
    
    public static TransactionScope CreateRepeatableReadAsync()
    {
        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.RepeatableRead,
            Timeout = TransactionManager.DefaultTimeout
        };

        return new TransactionScope(TransactionScopeOption.Required, options, TransactionScopeAsyncFlowOption.Enabled);
    }
}