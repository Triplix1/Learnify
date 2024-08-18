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
    
    public static TransactionScope CreateReadCommittedAsync()
    {
        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout
        };

        return new TransactionScope(TransactionScopeOption.Required, options, TransactionScopeAsyncFlowOption.Enabled);

    }
}