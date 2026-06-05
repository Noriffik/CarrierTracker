using CareerTracker.Kernel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTracker.Identity.Features.RequestPasswordReset;

public record RequestPasswordResetCommand(string Email) : IRequest<Result>;
