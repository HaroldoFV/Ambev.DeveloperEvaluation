namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Serviço responsável por gerar números únicos para vendas.
/// </summary>
public interface ISaleNumberGenerator
{
    long GenerateUniqueSaleNumber();
}


// implementacao
// using System.Threading;
//
// namespace Ambev.DeveloperEvaluation.Infrastructure.Services;
//
// /// <summary>
// /// Implementação de exemplo do ISaleNumberGenerator usando um contador em memória.
// /// </summary>
// public class SaleNumberGenerator : ISaleNumberGenerator
// {
//     private static long _currentSaleNumber = 0;
//
//     public long GenerateUniqueSaleNumber()
//     {
//         return Interlocked.Increment(ref _currentSaleNumber);
//     }
// }