using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Ambev.DeveloperEvaluation.Messages;

public abstract class Command : Message, IRequest<bool>
{
    public DateTime Timestamp { get; private set; }
    public ValidationResult ValidationResult { get; set; }

    protected Command()
    {
        Timestamp = DateTime.Now;
    }
}