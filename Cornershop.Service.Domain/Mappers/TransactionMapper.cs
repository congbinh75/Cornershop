using Cornershop.Service.Infrastructure.Entities;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Mappers;

public static class TransactionMapper
{
    public static TransactionDTO Map(this Transaction transaction)
    {
        return new TransactionDTO
        {
            Id = transaction.Id,
            Order = transaction.Order.Map(),
            Amount = transaction.Amount,
            CreatedOn = transaction.CreatedOn,
            CreatedBy = transaction.CreatedBy?.Map(),
            UpdatedOn = transaction.UpdatedOn,
            UpdatedBy = transaction.UpdatedBy?.Map()
        };
    }

    public static Transaction Map(this TransactionDTO transactionDTO)
    {
        return new Transaction
        {
            Id = transactionDTO.Id,
            Order = transactionDTO.Order.Map(),
            Amount = transactionDTO.Amount,
            CreatedBy = transactionDTO.CreatedBy.Map(),
            CreatedOn = transactionDTO.CreatedOn,
            UpdatedBy = transactionDTO.UpdatedBy.Map(),
            UpdatedOn = transactionDTO.UpdatedOn,
        };
    }
}