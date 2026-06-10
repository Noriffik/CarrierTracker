using CareerTracker.Kernel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTracker.Infrastructure.Auth;

public sealed class EmailSender : IEmailSender
{
    public Task SendPasswordResetEmailAsync(string email, string resetToken)
    {
        throw new NotImplementedException();
    }
}
